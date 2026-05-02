using System.Globalization;
using MARN_API.DTOs.Contracts;
using MARN_API.Enums;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MARN_API.Services.Implementations
{
    public class ContractPdfGenerator
    {
        public GeneratedContractPdfResult Generate(ContractPdfRequest request)
        {
            ArgumentNullException.ThrowIfNull(request);
            ArgumentNullException.ThrowIfNull(request.Landlord);
            ArgumentNullException.ThrowIfNull(request.Tenant);
            ArgumentNullException.ThrowIfNull(request.Property);
            ArgumentNullException.ThrowIfNull(request.RentalTerms);
            ArgumentNullException.ThrowIfNull(request.ElectronicSignature);

            request.IssuedAtUtc ??= DateTime.UtcNow;
            request.ContractNumber ??= $"MARN-{request.IssuedAtUtc:yyyyMMdd-HHmmss}";
            request.GoverningLawNote ??= "This document is electronically signed and intended to be legally binding under Egypt Law No. 15 of 2004.";

            QuestPDF.Settings.License = LicenseType.Community;

            var pdfBytes = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(32);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(style => style.FontSize(11).FontColor(Colors.Grey.Darken4));

                    page.Header().Element(container => ComposeHeader(container, request));
                    page.Content().PaddingVertical(18).Element(container => ComposeContent(container, request));
                    page.Footer().Element(ComposeFooter);
                });
            }).GeneratePdf();

            return new GeneratedContractPdfResult
            {
                FileName = $"rental-contract-{SanitizeFilePart(request.ContractNumber)}.pdf",
                Content = pdfBytes,
                ContractNumber = request.ContractNumber,
                GeneratedAtUtc = request.IssuedAtUtc.Value
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
                        text.Span("Prepared for digital acceptance and contract verification").FontColor("#D7E6E8");
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
                            ("Rent Amount", FormatMoney(rentalTerms.RentAmount, rentalTerms.Currency!)),
                            ("Total Contract Amount", FormatMoney(rentalTerms.TotalContractAmount, rentalTerms.Currency!)),
                            ("Payment Frequency", FormatPaymentFrequency(rentalTerms.PaymentFrequency))                        }));

                    row.ConstantItem(12);

                    row.RelativeItem().Element(card =>
                        ComposeInfoCard(card, "Term and Occupancy", new[]
                        {
                            ("Lease Start", $"{rentalTerms.LeaseStartDate:yyyy-MM-dd}"),
                            ("Lease End", $"{rentalTerms.LeaseEndDate:yyyy-MM-dd}"),
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
                        body.Item().PaddingTop(8).Text("The parties acknowledge that pressing the platform acceptance button and completing identity verification together form the electronic act of signature for this agreement.");
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
                        table.Cell().PaddingLeft(8).PaddingBottom(10).Element(cell => ComposeVerificationCell(cell, "Total Amount", FormatMoney(rentalTerms.TotalContractAmount, rentalTerms.Currency!)));
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

        private static string FormatPaymentFrequency(PaymentFrequency frequency)
        {
            return frequency switch
            {
                PaymentFrequency.OneTime => "One-Time",
                PaymentFrequency.Monthly => "Monthly",
                PaymentFrequency.Quarterly => "Quarterly",
                PaymentFrequency.Yearly => "Yearly",
                _ => frequency.ToString()
            };
        }
    }
}
