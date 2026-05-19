using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using MARN_API.DTOs.Contracts;
using MARN_API.Enums.Payment;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using static System.Collections.Specialized.BitVector32;

using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class ContractPdfGenerator : IContractPdfGenerator
    {
        private readonly IWebHostEnvironment _env;

        public ContractPdfGenerator(IWebHostEnvironment env)
        {
            _env = env;
        }
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
                    page.Content().PaddingVertical(18).Element(container => ComposeContent(container, request, _env.WebRootPath));
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
                        inner.Item().Text("Property ID").Bold().FontColor("#12343B");
                        inner.Item().Text(request.Property!.UnitNumber!).FontColor("#35555D");
                    });
                });
            });
        }

        private static void ComposeContent(IContainer container, ContractPdfRequest request, string webRootPath)
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
                            ("Payment Frequency", FormatPaymentFrequency(rentalTerms.PaymentFrequency))                        
                        }));

                    row.ConstantItem(12);

                    row.RelativeItem().Element(card =>
                        ComposeInfoCard(card, "Term and Occupancy", new[]
                        {
                            ("Lease Start", $"{rentalTerms.LeaseStartDate:yyyy-MM-dd}"),
                            ("Lease End", $"{rentalTerms.LeaseEndDate:yyyy-MM-dd}"),
                            ("Usage", "Residential use only")
                        }));
                });

                column.Item().Row(row =>
                {
                    row.RelativeItem().Element(card =>
                        ComposeInfoCard(card, "Property Details", new[]
                        {
                            ("Title", request.Property!.ListingTitle ?? "N/A"),
                            ("Type", request.Property.Type ?? "N/A"),
                            ("Address", request.Property.AddressLine ?? "N/A"),
                            ("City", request.Property.City ?? "N/A"),
                            ("State", request.Property.State ?? "N/A"),
                            ("Zip Code", request.Property.ZipCode ?? "N/A"),
                            ("Coordinates", $"{request.Property.Latitude:F4}, {request.Property.Longitude:F4}")
                        }));

                    row.ConstantItem(12);

                    row.RelativeItem().Element(card =>
                        ComposeInfoCard(card, "Property Specifications", new[]
                        {
                            ("Bedrooms", request.Property.Bedrooms.ToString()),
                            ("Beds", request.Property.Beds.ToString()),
                            ("Bathrooms", request.Property.Bathrooms.ToString()),
                            ("Area", $"{request.Property.SquareMeters} sqm"),
                            ("Max Occupants", request.Property.MaxOccupants.ToString()),
                            ("Shared Space", request.Property.IsShared ? "Yes" : "No")
                        }));
                });

                column.Item().Element(section =>
                    ComposeSection(section, "Property Description & Amenities", body =>
                    {
                        if (!string.IsNullOrWhiteSpace(request.Property.Description))
                        {
                            body.Item().PaddingBottom(4).Text("Description:").SemiBold().FontColor("#12343B");
                            body.Item().PaddingBottom(12).Text(request.Property.Description);
                        }

                        if (!string.IsNullOrWhiteSpace(request.Property.Amenities))
                        {
                            body.Item().PaddingBottom(4).Text("Amenities:").SemiBold().FontColor("#12343B");
                            var amenities = request.Property.Amenities.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (var amenity in amenities)
                            {
                                ComposeBullet(body, amenity.Trim());
                            }
                        }
                    }));

                if (request.Property.MediaPaths != null && request.Property.MediaPaths.Any())
                {
                    column.Item().Element(section =>
                        ComposeSection(section, "Property Images", body =>
                        {
                            body.Item().Grid(grid =>
                            {
                                grid.Spacing(10);
                                grid.Columns(3); // 3 images per row

                                foreach (var relativePath in request.Property.MediaPaths)
                                {
                                    var cleanPath = relativePath.TrimStart('/', '\\');
                                    var absolutePath = Path.Combine(webRootPath, cleanPath);

                                    if (File.Exists(absolutePath))
                                    {
                                        grid.Item().Height(120).Image(absolutePath);
                                    }
                                }
                            });
                        }));
                }

                column.Item().Element(section =>
                    ComposeSection(section, "Core Obligations", body =>
                    {
                        ComposeBullet(body, "The Tenant shall pay all amounts due on time and maintain the property in good condition, ordinary wear and tear excepted.");
                        ComposeBullet(body, "The Landlord shall provide possession of the property on the lease start date in a condition reasonably fit for residential occupancy.");
                        ComposeBullet(body, "If the Tenant fails to pay rent for more than fifteen (15) days after the due date and following an official notice through the platform or approved communication channels, the Landlord may initiate appropriate legal action.");
                        ComposeBullet(body, "The agreed rental amount includes the costs of basic property-related services including water, electricity, routine maintenance fees, and any additional services specified by the Landlord unless otherwise stated.");
                        ComposeBullet(body, "The Tenant shall bear the costs of minor operational repairs resulting from normal use, while the Landlord shall bear major and structural repairs necessary to maintain the property in a habitable condition.");
                        ComposeBullet(body, "Refund requests, damage claims, and requests related to early contract termination shall be submitted and documented through the platform workflow used by the parties.");
                        ComposeBullet(body, "In the event that either party wishes to terminate this agreement before its expiration date, a request shall first be submitted through the platform for review and amicable resolution attempts between the parties. If no resolution is reached, disputes shall fall under the jurisdiction of Cairo Primary Courts.");
                    }));

                if (!string.IsNullOrWhiteSpace(request.Property.Rules))
                {
                    column.Item().Element(section =>
                        ComposeSection(section, "Property Rules and Additional Terms", body =>
                            {
                                var rules = request.Property.Rules.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var rule in rules)
                                {
                                    ComposeBullet(body, rule.Trim());
                                }
                                body.Item().PaddingBottom(8);
                            }));
                }

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

                column.Item().Element(section =>
                    ComposeSection(section, "Acknowledgement", body =>
                    {
                        ComposeBullet(body, "By accepting electronically, the Tenant confirms that the contract was reviewed in full, that the provided identity details are accurate, and that the digital record may be relied upon as evidence of assent.");
                        ComposeBullet(body, "Acceptance by both parties has been electronically documented through the platform and linked to identity information and digital verification records.");
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
            ComposeInfoCard(container, title, new[]
            {
                ("Name", party.FullName ?? "N/A"),
                ("National ID", party.NationalId ?? "N/A"),
                ("Email", party.Email ?? "N/A"),
                ("Phone", party.PhoneNumber ?? "N/A"),
                ("Address", party.Address ?? "N/A")
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
