using System.Globalization;
using MARN_API.DTOs.Contracts;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MARN_API.Services.Implementations
{
    public class ContractPdfGenerator
    {
        public GeneratedContractPdfResult Generate(ContractPdfRequest request)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var normalized = Normalize(request);
            var pdfBytes = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(32);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(style => style.FontSize(11).FontColor(Colors.Grey.Darken4));

                    page.Header().Element(container => ComposeHeader(container, normalized));
                    page.Content().PaddingVertical(18).Element(container => ComposeContent(container, normalized));
                    page.Footer().Element(ComposeFooter);
                });
            }).GeneratePdf();

            return new GeneratedContractPdfResult
            {
                FileName = $"rental-contract-{SanitizeFilePart(normalized.ContractNumber!)}.pdf",
                Content = pdfBytes,
                ContractNumber = normalized.ContractNumber!,
                GeneratedAtUtc = normalized.IssuedAtUtc!.Value
            };
        }

        private static ContractPdfRequest Normalize(ContractPdfRequest request)
        {
            var issuedAtUtc = request.IssuedAtUtc?.ToUniversalTime() ?? DateTime.UtcNow;
            var contractNumber = string.IsNullOrWhiteSpace(request.ContractNumber)
                ? $"MARN-{issuedAtUtc:yyyyMMdd-HHmmss}"
                : request.ContractNumber.Trim();

            var landlord = request.Landlord ?? new PartyInfo();
            var tenant = request.Tenant ?? new PartyInfo();
            var property = request.Property ?? new PropertyInfo();
            var rentalTerms = request.RentalTerms ?? new RentalTermsInfo();
            var signature = request.ElectronicSignature ?? new ElectronicSignatureInfo();

            landlord.FullName ??= "Mahmoud Hassan";
            landlord.NationalId ??= "28401011234567";
            landlord.Email ??= "mahmoud.owner@example.com";
            landlord.PhoneNumber ??= "+20 100 123 4567";
            landlord.Address ??= "12 Nile Corniche, Maadi, Cairo, Egypt";

            tenant.FullName ??= "Ahmed Mohamed";
            tenant.NationalId ??= "29901011234567";
            tenant.Email ??= "ahmed.tenant@example.com";
            tenant.PhoneNumber ??= "+20 101 765 4321";
            tenant.Address ??= "45 Abbas El Akkad St, Nasr City, Cairo, Egypt";

            property.ListingTitle ??= "Furnished One-Bedroom Apartment";
            property.AddressLine ??= "Building 18, Street 206";
            property.UnitNumber ??= "Unit 7B";
            property.City ??= "Cairo";
            property.Country ??= "Egypt";
            property.Description ??= "A fully furnished residential unit offered for private residential use only.";

            rentalTerms.Currency ??= "EGP";
            rentalTerms.MonthlyRentAmount = rentalTerms.MonthlyRentAmount <= 0 ? 18500m : rentalTerms.MonthlyRentAmount;
            rentalTerms.PlatformFeeAmount ??= 0m;
            rentalTerms.LeaseStartDate ??= DateOnly.FromDateTime(issuedAtUtc.Date);
            rentalTerms.LeaseEndDate ??= rentalTerms.LeaseStartDate.Value.AddMonths(12);
            rentalTerms.PaymentDueDay ??= 1;
            rentalTerms.PaymentMethod ??= "Stripe card payment";
            rentalTerms.CheckInWindow ??= "From 2:00 PM";
            rentalTerms.CheckOutWindow ??= "By 12:00 PM";

            signature.SignerName ??= tenant.FullName;
            signature.SignerNationalId ??= tenant.NationalId;
            signature.SignedAtUtc = signature.SignedAtUtc?.ToUniversalTime() ?? issuedAtUtc;
            signature.PaymentIntentId ??= "pi_3TN_dummy123456789";
            signature.ReceiptUrl ??= $"https://pay.stripe.com/receipts/{contractNumber}";
            signature.ConsentStatement ??= "I have read and agree to the terms of this Rental Agreement and I consent to sign electronically.";

            return new ContractPdfRequest
            {
                ContractNumber = contractNumber,
                IssuedAtUtc = issuedAtUtc,
                Landlord = landlord,
                Tenant = tenant,
                Property = property,
                RentalTerms = rentalTerms,
                ElectronicSignature = signature,
                AdditionalTerms = request.AdditionalTerms,
                GoverningLawNote = string.IsNullOrWhiteSpace(request.GoverningLawNote)
                    ? "This document is electronically signed and intended to be legally binding under Egypt Law No. 15 of 2004."
                    : request.GoverningLawNote.Trim()
            };
        }

        private static void ComposeHeader(IContainer container, ContractPdfRequest request)
        {
            container.Column(column =>
            {
                column.Item().Background("#12343B").Padding(20).Column(inner =>
                {
                    inner.Item().Text("Residential Rental Agreement")
                        .FontSize(24)
                        .SemiBold()
                        .FontColor(Colors.White);

                    inner.Item().PaddingTop(6).Text(text =>
                    {
                        text.Span("Prepared for digital acceptance and payment confirmation").FontColor("#D7E6E8");
                    });
                });

                column.Item().Background("#F2F7F7").Padding(14).Row(row =>
                {
                    row.RelativeItem().Column(inner =>
                    {
                        inner.Item().Text("Contract Number").Bold().FontColor("#12343B");
                        inner.Item().Text(request.ContractNumber!).FontColor("#35555D");
                    });

                    row.RelativeItem().Column(inner =>
                    {
                        inner.Item().Text("Issued (UTC)").Bold().FontColor("#12343B");
                        inner.Item().Text($"{request.IssuedAtUtc:yyyy-MM-dd HH:mm:ss}").FontColor("#35555D");
                    });

                    row.RelativeItem().Column(inner =>
                    {
                        inner.Item().Text("Property").Bold().FontColor("#12343B");
                        inner.Item().Text(request.Property!.ListingTitle!).FontColor("#35555D");
                    });
                });
            });
        }

        private static void ComposeContent(IContainer container, ContractPdfRequest request)
        {
            var rentalTerms = request.RentalTerms!;
            var signature = request.ElectronicSignature!;

            container.Column(column =>
            {
                column.Spacing(18);

                column.Item().Element(section =>
                    ComposeSection(section, "Agreement Overview", body =>
                    {
                        body.Item().Text($"This Residential Rental Agreement is made between {request.Landlord!.FullName} (the \"Landlord\") and {request.Tenant!.FullName} (the \"Tenant\").");
                        body.Item().PaddingTop(8).Text($"The Landlord agrees to rent to the Tenant the property identified as {request.Property!.ListingTitle}, located at {request.Property.AddressLine}, {request.Property.UnitNumber}, {request.Property.City}, {request.Property.Country}.");
                        body.Item().PaddingTop(8).Text(request.Property.Description!);
                    }));

                column.Item().Row(row =>
                {
                    row.RelativeItem().Element(card => ComposePartyCard(card, "Landlord", request.Landlord!));
                    row.ConstantItem(12);
                    row.RelativeItem().Element(card => ComposePartyCard(card, "Tenant", request.Tenant!));
                });

                column.Item().Row(row =>
                {
                    row.RelativeItem().Element(card =>
                        ComposeInfoCard(card, "Financial Terms", new[]
                        {
                            ("Monthly Rent", FormatMoney(rentalTerms.MonthlyRentAmount, rentalTerms.Currency!)),
                            ("Platform Fee", FormatMoney(rentalTerms.PlatformFeeAmount ?? 0m, rentalTerms.Currency!)),
                            ("Payment Due Day", $"Day {rentalTerms.PaymentDueDay}"),
                            ("Payment Method", rentalTerms.PaymentMethod!)
                        }));

                    row.ConstantItem(12);

                    row.RelativeItem().Element(card =>
                        ComposeInfoCard(card, "Term and Occupancy", new[]
                        {
                            ("Lease Start", $"{rentalTerms.LeaseStartDate:yyyy-MM-dd}"),
                            ("Lease End", $"{rentalTerms.LeaseEndDate:yyyy-MM-dd}"),
                            ("Check-In", rentalTerms.CheckInWindow!),
                            ("Check-Out", rentalTerms.CheckOutWindow!),
                            ("Usage", "Residential use only")
                        }));
                });

                column.Item().Element(section =>
                    ComposeSection(section, "Core Obligations", body =>
                    {
                        ComposeBullet(body, "The Tenant shall pay all amounts due on time and maintain the property in good condition, ordinary wear and tear excepted.");
                        ComposeBullet(body, "The Landlord shall provide possession of the property on the lease start date in a condition reasonably fit for residential occupancy.");
                        ComposeBullet(body, "Any material breach not cured after notice may result in termination of this agreement in accordance with applicable law.");
                        ComposeBullet(body, "Refund requests and damage claims shall be documented through the platform workflow used by the parties.");
                    }));

                column.Item().Element(section =>
                    ComposeSection(section, "Electronic Signature and Consent", body =>
                    {
                        body.Item().Text(signature.ConsentStatement!);
                        body.Item().PaddingTop(8).Text("The parties acknowledge that pressing the platform acceptance button, completing identity verification, and authorizing payment together form the electronic act of signature for this agreement.");
                    }));

                column.Item().Border(1).BorderColor("#D5E3E6").Background("#F6FAFA").Padding(16).Column(block =>
                {
                    block.Item().Text("Digital Verification Block").FontSize(15).SemiBold().FontColor("#12343B");
                    block.Item().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Cell().PaddingRight(8).PaddingBottom(10).Element(cell => ComposeVerificationCell(cell, "Digitally Signed By", signature.SignerName!));
                        table.Cell().PaddingLeft(8).PaddingBottom(10).Element(cell => ComposeVerificationCell(cell, "National ID", signature.SignerNationalId!));
                        table.Cell().PaddingRight(8).PaddingBottom(10).Element(cell => ComposeVerificationCell(cell, "Timestamp", $"{signature.SignedAtUtc:yyyy-MM-dd HH:mm:ss} UTC"));
                        table.Cell().PaddingLeft(8).PaddingBottom(10).Element(cell => ComposeVerificationCell(cell, "Payment Intent ID", signature.PaymentIntentId!));
                        table.Cell().PaddingRight(8).Element(cell => ComposeVerificationLinkCell(cell, "Receipt", "View receipt", signature.ReceiptUrl!));
                    });

                    block.Item().PaddingTop(12).Text(request.GoverningLawNote!).Italic().FontColor("#35555D");
                });

                if (request.AdditionalTerms is { Count: > 0 })
                {
                    column.Item().Element(section =>
                        ComposeSection(section, "Additional Terms", body =>
                        {
                            foreach (var term in request.AdditionalTerms.Where(term => !string.IsNullOrWhiteSpace(term)))
                            {
                                ComposeBullet(body, term.Trim());
                            }
                        }));
                }

                column.Item().Element(section =>
                    ComposeSection(section, "Acknowledgement", body =>
                    {
                        body.Item().Text("By accepting electronically, the Tenant confirms that the contract was reviewed in full, that the provided identity details are accurate, and that the digital record may be relied upon as evidence of assent.");
                    }));
            });
        }

        private static void ComposeFooter(IContainer container)
        {
            container.PaddingTop(8).BorderTop(1).BorderColor("#D5E3E6").Row(row =>
            {
                row.RelativeItem().Text("Generated by the rental contract API").FontSize(9).FontColor(Colors.Grey.Darken1);
                row.ConstantItem(60).AlignRight().Text(text =>
                {
                    text.Span("Page ").FontSize(9).FontColor(Colors.Grey.Darken1);
                    text.CurrentPageNumber().FontSize(9).SemiBold();
                });
            });
        }

        private static void ComposeSection(IContainer container, string title, Action<ColumnDescriptor> content)
        {
            container.Column(column =>
            {
                column.Item().Text(title).FontSize(16).SemiBold().FontColor("#12343B");
                column.Item().PaddingTop(8).BorderTop(2).BorderColor("#D5E3E6");
                column.Item().PaddingTop(10).Column(content);
            });
        }

        private static void ComposePartyCard(IContainer container, string title, PartyInfo party)
        {
            container.Border(1).BorderColor("#D5E3E6").Padding(16).Column(column =>
            {
                column.Spacing(5);
                column.Item().Text(title).FontSize(15).SemiBold().FontColor("#12343B");
                column.Item().Text(party.FullName!).Bold();
                column.Item().Text($"National ID: {party.NationalId}");
                column.Item().Text($"Email: {party.Email}");
                column.Item().Text($"Phone: {party.PhoneNumber}");
                column.Item().Text($"Address: {party.Address}").FontColor("#4B6268");
            });
        }

        private static void ComposeInfoCard(IContainer container, string title, IEnumerable<(string Label, string Value)> items)
        {
            container.Border(1).BorderColor("#D5E3E6").Padding(16).Column(column =>
            {
                column.Spacing(6);
                column.Item().Text(title).FontSize(15).SemiBold().FontColor("#12343B");

                foreach (var item in items)
                {
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Text(item.Label).FontColor("#4B6268");
                        row.RelativeItem().AlignRight().Text(item.Value).SemiBold();
                    });
                }
            });
        }

        private static void ComposeBullet(ColumnDescriptor column, string text)
        {
            column.Item().Row(row =>
            {
                row.ConstantItem(14).Text("•").FontColor("#12343B");
                row.RelativeItem().Text(text);
            });
        }

        private static void ComposeVerificationCell(IContainer container, string label, string value)
        {
            container.Column(column =>
            {
                column.Item().Text(label).FontSize(9).SemiBold().FontColor("#5E777D");
                column.Item().PaddingTop(2).Text(value).SemiBold().FontColor("#12343B");
            });
        }

        private static void ComposeVerificationLinkCell(IContainer container, string label, string linkText, string url)
        {
            container.Column(column =>
            {
                column.Item().Text(label).FontSize(9).SemiBold().FontColor("#5E777D");
                column.Item().PaddingTop(2).Text(text =>
                {
                    text.Hyperlink(linkText, url)
                        .SemiBold()
                        .Underline()
                        .FontColor(Colors.Blue.Darken2);
                });
            });
        }

        private static string SanitizeFilePart(string value)
        {
            var invalidChars = Path.GetInvalidFileNameChars();
            var sanitized = new char[value.Length];

            for (var index = 0; index < value.Length; index++)
            {
                sanitized[index] = invalidChars.Contains(value[index]) ? '-' : value[index];
            }

            return new string(sanitized);
        }

        private static string FormatMoney(decimal amount, string currency)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:N2} {1}", amount, currency);
        }
    }
}
