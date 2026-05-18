using System.Globalization;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using MARN_API.DTOs.Contracts;
using MARN_API.Enums.Payment;
using MARN_API.Services.Interfaces;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MARN_API.Services.Implementations
{
    public class ContractPdfGenerator
    {
        private static readonly CultureInfo EnglishCulture = CultureInfo.GetCultureInfo("en");
        private static readonly CultureInfo ArabicCulture = CultureInfo.GetCultureInfo("ar");
        private static int _fontsRegistered;

        private readonly IWebHostEnvironment _env;
        private readonly IAppTextLocalizer _localizer;

        public ContractPdfGenerator(IWebHostEnvironment env, IAppTextLocalizer localizer)
        {
            _env = env;
            _localizer = localizer;
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

            EnsureFontsRegistered();
            QuestPDF.Settings.License = LicenseType.Community;

            var pdfBytes = Document.Create(document =>
            {
                document.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(28);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(style => style.FontFamily("Arial").FontSize(10.5f).FontColor(Colors.Grey.Darken4));

                    page.Header().Element(container => ComposeHeader(container, request));
                    page.Content().PaddingVertical(16).Element(container => ComposeContent(container, request, _env.WebRootPath));
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

        private void ComposeHeader(IContainer container, ContractPdfRequest request)
        {
            container.Column(column =>
            {
                column.Item().Background("#12343B").Padding(18).Column(inner =>
                {
                    inner.Item().Text(Bilingual("Residential Rental Agreement", "عقد إيجار سكني"))
                        .FontSize(20)
                        .SemiBold()
                        .FontColor(Colors.White);

                    inner.Item().PaddingTop(4).Text(Bilingual(
                            "Prepared for digital acceptance and contract verification",
                            "أُعد هذا العقد للقبول الرقمي والتحقق من صحة العقد"))
                        .FontColor("#D7E6E8")
                        .FontSize(10);
                });

                column.Item().Background("#F2F7F7").Padding(12).Row(row =>
                {
                    row.RelativeItem().Element(cell => ComposeMetaCell(cell, "Contract Number", "رقم العقد", request.ContractNumber!));
                    row.RelativeItem().Element(cell => ComposeMetaCell(cell, "Issued (UTC)", "تاريخ الإصدار", $"{request.IssuedAtUtc:yyyy-MM-dd HH:mm:ss}"));
                    row.RelativeItem().Element(cell => ComposeMetaCell(cell, "Property ID", "معرف العقار", request.Property!.UnitNumber ?? NotAvailable()));
                });
            });
        }

        private void ComposeContent(IContainer container, ContractPdfRequest request, string webRootPath)
        {
            var rentalTerms = request.RentalTerms!;
            var signature = request.ElectronicSignature!;

            container.Column(column =>
            {
                column.Spacing(16);

                column.Item().Element(section =>
                    ComposeSection(section, "Agreement Overview", "نظرة عامة على الاتفاق", body =>
                    {
                        ComposeParagraph(body,
                            $"This Residential Rental Agreement is made between {request.Landlord!.FullName} (the \"Landlord\") and {request.Tenant!.FullName} (the \"Tenant\").",
                            $"يُبرم عقد الإيجار السكني هذا بين {request.Landlord!.FullName} بصفته المؤجر و{request.Tenant!.FullName} بصفته المستأجر.");

                        ComposeParagraph(body,
                            $"The Landlord agrees to rent to the Tenant the property identified as {request.Property!.ListingTitle}, located at {request.Property.AddressLine}, {request.Property.UnitNumber}, {request.Property.City}, {request.Property.Country}.",
                            $"يوافق المؤجر على تأجير العقار المسمى {request.Property!.ListingTitle} والكائن في {request.Property.AddressLine}، {request.Property.UnitNumber}، {request.Property.City}، {request.Property.Country} إلى المستأجر.");

                        if (!string.IsNullOrWhiteSpace(request.Property.Description))
                        {
                            body.Item().PaddingTop(4).Text(Bilingual("Description", "الوصف")).SemiBold().FontColor("#12343B");
                            body.Item().Text(request.Property.Description);
                        }
                    }));

                column.Item().Row(row =>
                {
                    row.RelativeItem().Element(card => ComposePartyCard(card, "Landlord", "المؤجر", request.Landlord!));
                    row.ConstantItem(10);
                    row.RelativeItem().Element(card => ComposePartyCard(card, "Tenant", "المستأجر", request.Tenant!));
                });

                column.Item().Row(row =>
                {
                    row.RelativeItem().Element(card =>
                        ComposeInfoCard(card, "Financial Terms", "الشروط المالية", new[]
                        {
                            ("Rent Amount", "قيمة الإيجار", FormatMoney(rentalTerms.RentAmount, rentalTerms.Currency!)),
                            ("Total Contract Amount", "إجمالي قيمة العقد", FormatMoney(rentalTerms.TotalContractAmount, rentalTerms.Currency!)),
                            ("Payment Frequency", "تكرار الدفع", FormatPaymentFrequency(rentalTerms.PaymentFrequency))
                        }));

                    row.ConstantItem(10);

                    row.RelativeItem().Element(card =>
                        ComposeInfoCard(card, "Term and Occupancy", "المدة والإشغال", new[]
                        {
                            ("Lease Start", "بداية العقد", $"{rentalTerms.LeaseStartDate:yyyy-MM-dd}"),
                            ("Lease End", "نهاية العقد", $"{rentalTerms.LeaseEndDate:yyyy-MM-dd}"),
                            ("Usage", "الاستخدام", Bilingual("Residential use only", "للاستخدام السكني فقط"))
                        }));
                });

                column.Item().Row(row =>
                {
                    row.RelativeItem().Element(card =>
                        ComposeInfoCard(card, "Property Details", "تفاصيل العقار", new[]
                        {
                            ("Title", "العنوان", request.Property!.ListingTitle ?? NotAvailable()),
                            ("Type", "النوع", request.Property.Type ?? NotAvailable()),
                            ("Address", "العنوان التفصيلي", request.Property.AddressLine ?? NotAvailable()),
                            ("City", "المدينة", request.Property.City ?? NotAvailable()),
                            ("State", "المحافظة", request.Property.State ?? NotAvailable()),
                            ("Zip Code", "الرمز البريدي", request.Property.ZipCode ?? NotAvailable()),
                            ("Coordinates", "الإحداثيات", $"{request.Property.Latitude:F4}, {request.Property.Longitude:F4}")
                        }));

                    row.ConstantItem(10);

                    row.RelativeItem().Element(card =>
                        ComposeInfoCard(card, "Property Specifications", "مواصفات العقار", new[]
                        {
                            ("Bedrooms", "غرف النوم", request.Property.Bedrooms.ToString(CultureInfo.InvariantCulture)),
                            ("Beds", "الأسرة", request.Property.Beds.ToString(CultureInfo.InvariantCulture)),
                            ("Bathrooms", "الحمامات", request.Property.Bathrooms.ToString(CultureInfo.InvariantCulture)),
                            ("Area", "المساحة", $"{request.Property.SquareMeters} sqm / متر مربع"),
                            ("Max Occupants", "الحد الأقصى للشاغلين", request.Property.MaxOccupants.ToString(CultureInfo.InvariantCulture)),
                            ("Shared Space", "سكن مشترك", request.Property.IsShared ? Bilingual("Yes", "نعم") : Bilingual("No", "لا"))
                        }));
                });

                column.Item().Element(section =>
                    ComposeSection(section, "Property Description and Amenities", "وصف العقار والمرافق", body =>
                    {
                        if (!string.IsNullOrWhiteSpace(request.Property.Description))
                        {
                            body.Item().Text(Bilingual("Description", "الوصف")).SemiBold().FontColor("#12343B");
                            body.Item().PaddingBottom(8).Text(request.Property.Description);
                        }

                        if (!string.IsNullOrWhiteSpace(request.Property.Amenities))
                        {
                            body.Item().Text(Bilingual("Amenities", "المرافق")).SemiBold().FontColor("#12343B");
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
                        ComposeSection(section, "Property Images", "صور العقار", body =>
                        {
                            body.Item().Grid(grid =>
                            {
                                grid.Spacing(8);
                                grid.Columns(3);

                                foreach (var relativePath in request.Property.MediaPaths)
                                {
                                    var cleanPath = relativePath.TrimStart('/', '\\');
                                    var absolutePath = Path.Combine(webRootPath, cleanPath);

                                    if (File.Exists(absolutePath))
                                    {
                                        grid.Item().Height(110).Image(absolutePath);
                                    }
                                }
                            });
                        }));
                }

                column.Item().Element(section =>
                    ComposeSection(section, "Core Obligations", "الالتزامات الأساسية", body =>
                    {
                        ComposeBullet(body, Bilingual(
                            "The Tenant shall pay all amounts due on time and maintain the property in good condition, ordinary wear and tear excepted.",
                            "يلتزم المستأجر بسداد جميع المبالغ المستحقة في مواعيدها والمحافظة على العقار بحالة جيدة مع استثناء الاستهلاك المعتاد."));
                        ComposeBullet(body, Bilingual(
                            "The Landlord shall provide possession of the property on the lease start date in a condition reasonably fit for residential occupancy.",
                            "يلتزم المؤجر بتسليم العقار في تاريخ بدء العقد بحالة مناسبة بصورة معقولة للسكن."));
                        ComposeBullet(body, Bilingual(
                            "The agreed rental amount includes the basic services and charges expressly specified by the Landlord unless otherwise stated.",
                            "يشمل مبلغ الإيجار المتفق عليه الخدمات الأساسية والرسوم التي يحددها المؤجر صراحة ما لم يُنص على خلاف ذلك."));
                        ComposeBullet(body, Bilingual(
                            "Requests related to refunds, damages, or early termination must be submitted through the platform workflow used by both parties.",
                            "يجب تقديم الطلبات المتعلقة برد المبالغ أو الأضرار أو إنهاء العقد مبكرًا من خلال إجراءات المنصة المعتمدة بين الطرفين."));
                        ComposeBullet(body, Bilingual(
                            "If no amicable resolution is reached, disputes shall fall under the jurisdiction of Cairo Primary Courts.",
                            "وفي حال عدم التوصل إلى تسوية ودية، تخضع النزاعات لاختصاص محاكم القاهرة الابتدائية."));
                    }));

                if (!string.IsNullOrWhiteSpace(request.Property.Rules) || (request.AdditionalTerms?.Any() ?? false))
                {
                    column.Item().Element(section =>
                        ComposeSection(section, "Property Rules and Additional Terms", "قواعد العقار والشروط الإضافية", body =>
                        {
                            if (!string.IsNullOrWhiteSpace(request.Property.Rules))
                            {
                                var rules = request.Property.Rules.Split(new[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (var rule in rules)
                                {
                                    ComposeBullet(body, rule.Trim());
                                }
                            }

                            if (request.AdditionalTerms != null)
                            {
                                foreach (var term in request.AdditionalTerms.Where(term => !string.IsNullOrWhiteSpace(term)))
                                {
                                    ComposeBullet(body, term.Trim());
                                }
                            }
                        }));
                }

                column.Item().Element(section =>
                    ComposeSection(section, "Electronic Signature and Consent", "التوقيع الإلكتروني والموافقة", body =>
                    {
                        var consentStatement = signature.ConsentStatement ??
                            "I acknowledge that this electronic signature is legally binding.";

                        body.Item().Text(consentStatement);
                        ComposeParagraph(body,
                            "The parties acknowledge that pressing the platform acceptance button and completing identity verification together form the electronic act of signature for this agreement.",
                            "يقر الطرفان بأن الضغط على زر قبول العقد داخل المنصة مع إتمام التحقق من الهوية يشكلان معًا التوقيع الإلكتروني لهذا العقد.");
                    }));

                column.Item().Border(1).BorderColor("#D5E3E6").Background("#F6FAFA").Padding(16).Column(block =>
                {
                    block.Spacing(8);
                    block.Item().Text(Bilingual("Digital Verification Block", "كتلة التحقق الرقمي")).FontSize(14).SemiBold().FontColor("#12343B");

                    block.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Cell().PaddingRight(6).PaddingBottom(8).Element(cell =>
                            ComposeVerificationCell(cell, "Digitally Signed By", "الموقع رقميًا", signature.SignerName ?? NotAvailable()));
                        table.Cell().PaddingLeft(6).PaddingBottom(8).Element(cell =>
                            ComposeVerificationCell(cell, "National ID", "الرقم القومي", signature.SignerNationalId ?? NotAvailable()));
                        table.Cell().PaddingRight(6).PaddingBottom(8).Element(cell =>
                            ComposeVerificationCell(cell, "Timestamp", "الختم الزمني", $"{signature.SignedAtUtc:yyyy-MM-dd HH:mm:ss} UTC"));
                        table.Cell().PaddingLeft(6).PaddingBottom(8).Element(cell =>
                            ComposeVerificationCell(cell, "Total Amount", "إجمالي المبلغ", FormatMoney(rentalTerms.TotalContractAmount, rentalTerms.Currency!)));
                    });

                    block.Item().PaddingTop(6).Text(Bilingual(
                            request.GoverningLawNote!,
                            "تم توقيع هذا المستند إلكترونيًا ويُقصد به أن يكون ملزمًا قانونًا بموجب القانون المصري رقم 15 لسنة 2004."))
                        .Italic()
                        .FontColor("#35555D");
                });

                column.Item().Element(section =>
                    ComposeSection(section, "Acknowledgement", "الإقرار", body =>
                    {
                        ComposeBullet(body, Bilingual(
                            "By accepting electronically, the Tenant confirms that the contract was reviewed in full and that the provided identity details are accurate.",
                            "بموجب القبول الإلكتروني، يؤكد المستأجر أنه راجع العقد بالكامل وأن بيانات الهوية المقدمة صحيحة."));
                        ComposeBullet(body, Bilingual(
                            "Acceptance by both parties has been electronically documented through the platform and linked to identity information and verification records.",
                            "تم توثيق قبول الطرفين إلكترونيًا عبر المنصة وربطه ببيانات الهوية وسجلات التحقق."));
                    }));
            });
        }

        private static void ComposeFooter(IContainer container)
        {
            container.PaddingTop(8).BorderTop(1).BorderColor("#D5E3E6").Row(row =>
            {
                row.RelativeItem().Text(Bilingual("Generated by the rental contract API", "تم إنشاء هذا العقد بواسطة واجهة عقود الإيجار"))
                    .FontSize(8.5f)
                    .FontColor(Colors.Grey.Darken1);

                row.ConstantItem(70).AlignRight().Text(text =>
                {
                    text.Span(Bilingual("Page", "صفحة") + " ").FontSize(8.5f).FontColor(Colors.Grey.Darken1);
                    text.CurrentPageNumber().FontSize(8.5f).SemiBold();
                });
            });
        }

        private void ComposeSection(IContainer container, string englishTitle, string arabicTitle, Action<ColumnDescriptor> content)
        {
            container.Column(column =>
            {
                column.Item().Text(Bilingual(englishTitle, arabicTitle)).FontSize(14).SemiBold().FontColor("#12343B");
                column.Item().PaddingTop(6).BorderTop(2).BorderColor("#D5E3E6");
                column.Item().PaddingTop(8).Column(content);
            });
        }

        private void ComposePartyCard(IContainer container, string englishTitle, string arabicTitle, PartyInfo party)
        {
            ComposeInfoCard(container, englishTitle, arabicTitle, new[]
            {
                ("Name", "الاسم", party.FullName ?? NotAvailable()),
                ("National ID", "الرقم القومي", party.NationalId ?? NotAvailable()),
                ("Email", "البريد الإلكتروني", party.Email ?? NotAvailable()),
                ("Phone", "الهاتف", party.PhoneNumber ?? NotAvailable()),
                ("Address", "العنوان", party.Address ?? NotAvailable())
            });
        }

        private void ComposeInfoCard(IContainer container, string englishTitle, string arabicTitle, IEnumerable<(string EnLabel, string ArLabel, string Value)> items)
        {
            container.Border(1).BorderColor("#D5E3E6").Padding(14).Column(column =>
            {
                column.Spacing(5);
                column.Item().Text(Bilingual(englishTitle, arabicTitle)).FontSize(13).SemiBold().FontColor("#12343B");

                foreach (var item in items)
                {
                    column.Item().Row(row =>
                    {
                        row.RelativeItem().Text(Bilingual(item.EnLabel, item.ArLabel)).FontColor("#4B6268").FontSize(9.5f);
                        row.RelativeItem().AlignRight().Text(item.Value).SemiBold().FontSize(9.5f);
                    });
                }
            });
        }

        private static void ComposeParagraph(ColumnDescriptor column, string english, string arabic)
        {
            column.Item().Text(Bilingual(english, arabic));
        }

        private static void ComposeBullet(ColumnDescriptor column, string text)
        {
            column.Item().Row(row =>
            {
                row.ConstantItem(14).Text("•").FontColor("#12343B");
                row.RelativeItem().Text(text);
            });
        }

        private static void ComposeMetaCell(IContainer container, string englishLabel, string arabicLabel, string value)
        {
            container.Column(column =>
            {
                column.Item().Text(Bilingual(englishLabel, arabicLabel)).Bold().FontColor("#12343B").FontSize(9.5f);
                column.Item().Text(value).FontColor("#35555D").FontSize(9.5f);
            });
        }

        private static void ComposeVerificationCell(IContainer container, string englishLabel, string arabicLabel, string value)
        {
            container.Column(column =>
            {
                column.Item().Text(Bilingual(englishLabel, arabicLabel)).FontSize(8.5f).SemiBold().FontColor("#5E777D");
                column.Item().PaddingTop(2).Text(value).SemiBold().FontColor("#12343B").FontSize(9.5f);
            });
        }

        private string FormatPaymentFrequency(PaymentFrequency frequency)
        {
            var english = _localizer.GetEnumDisplayName(frequency, EnglishCulture);
            var arabic = _localizer.GetEnumDisplayName(frequency, ArabicCulture);
            return Bilingual(english, arabic);
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

        private static string Bilingual(string english, string arabic)
            => $"{english} / {arabic}";

        private static string NotAvailable()
            => Bilingual("N/A", "غير متاح");

        private static void EnsureFontsRegistered()
        {
            if (Interlocked.Exchange(ref _fontsRegistered, 1) == 1)
            {
                return;
            }

            var fontsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
            var fontFiles = new[]
            {
                "arial.ttf",
                "arialbd.ttf",
                "ariali.ttf",
                "arialbi.ttf",
                "tahoma.ttf",
                "tahomabd.ttf"
            };

            foreach (var fileName in fontFiles)
            {
                var path = Path.Combine(fontsDirectory, fileName);
                if (!File.Exists(path))
                {
                    continue;
                }

                using var stream = File.OpenRead(path);
                FontManager.RegisterFont(stream);
            }
        }
    }
}
