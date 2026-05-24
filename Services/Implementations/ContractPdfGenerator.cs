using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using MARN_API.DTOs.Contracts;
using MARN_API.Enums.Payment;
using MARN_API.Localization;
using MARN_API.Services.Interfaces;
using QuestPDF.Drawing;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace MARN_API.Services.Implementations
{
    public class ContractPdfGenerator : IContractPdfGenerator
    {
        private const string LocalizedSeparator = " / ";
        private const string EnglishGoverningLawNote = "This document is electronically signed and intended to be legally binding under Egypt Law No. 15 of 2004.";
        private const string ArabicGoverningLawNote = "هذا المستند موقع إلكترونيًا ويقصد به أن يكون ملزمًا قانونًا وفقًا لقانون التوقيع الإلكتروني المصري رقم 15 لسنة 2004.";

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
            request.GoverningLawNote ??= EnglishGoverningLawNote;

            EnsureFontsRegistered();
            QuestPDF.Settings.License = LicenseType.Community;
            QuestPDF.Settings.EnableDebugging = true;

            var pdfBytes = Document.Create(document =>
            {
                ComposeContractPage(document, request, isArabic: false);
                ComposeContractPage(document, request, isArabic: true);
            }).GeneratePdf();

            return new GeneratedContractPdfResult
            {
                FileName = $"rental-contract-{SanitizeFilePart(request.ContractNumber)}.pdf",
                Content = pdfBytes,
                ContractNumber = request.ContractNumber,
                GeneratedAtUtc = request.IssuedAtUtc.Value
            };
        }

        private void ComposeContractPage(IDocumentContainer document, ContractPdfRequest request, bool isArabic)
        {
            document.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(32);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(style => style.FontFamily("Tahoma").FontSize(11).FontColor(Colors.Grey.Darken4));

                page.Header().Element(container => ComposeHeader(container, request, isArabic));
                page.Content().PaddingVertical(18).Element(container => ComposeContent(container, request, _env.WebRootPath ?? string.Empty, isArabic));
                page.Footer().Element(container => ComposeFooter(container, isArabic));
            });
        }

        private void ComposeHeader(IContainer container, ContractPdfRequest request, bool isArabic)
        {
            container.Column(column =>
            {
                column.Item().Background("#12343B").Padding(20).Column(inner =>
                {
                    inner.Item().AlignLeftOrRight(isArabic).Text(T(
                            "Residential Rental Agreement",
                            "عقد إيجار سكني",
                            isArabic))
                        .FontSize(24)
                        .SemiBold()
                        .FontColor(Colors.White);

                    inner.Item().PaddingTop(6).AlignLeftOrRight(isArabic).Text(T(
                            "Prepared for digital acceptance and contract verification",
                            "أعد هذا العقد للقبول الرقمي والتحقق من صحة العقد",
                            isArabic))
                        .FontColor("#D7E6E8");
                });

                column.Item().Background("#F2F7F7").Padding(14).Row(row =>
                {
                    row.RelativeItem().Element(cell => ComposeMetaCell(cell, T("Contract Number", "رقم العقد", isArabic), TextValue(request.ContractNumber, isArabic), isArabic));
                    row.RelativeItem().Element(cell => ComposeMetaCell(cell, T("Issued (UTC)", "تاريخ الإصدار (UTC)", isArabic), TextValue(FormatDateTime(request.IssuedAtUtc), isArabic), isArabic));
                    row.RelativeItem().Element(cell => ComposeMetaCell(cell, T("Property ID", "معرف العقار", isArabic), ValueText(request.Property!.UnitNumber, isArabic), isArabic));
                });
            });
        }

        private void ComposeContent(IContainer container, ContractPdfRequest request, string webRootPath, bool isArabic)
        {
            var property = request.Property!;
            var rentalTerms = request.RentalTerms!;
            var signature = request.ElectronicSignature!;

            container.Column(column =>
            {
                column.Spacing(18);

                column.Item().Element(section =>
                    ComposeSection(section, T("Agreement Overview", "نظرة عامة على الاتفاق", isArabic), isArabic, body =>
                    {
                        body.Item().AlignLeftOrRight(isArabic).Text(TextValue(
                            isArabic
                                ? $"يبرم عقد الإيجار السكني هذا بين {request.Landlord!.FullName} بصفته المؤجر و{request.Tenant!.FullName} بصفته المستأجر."
                                : $"This Residential Rental Agreement is made between {request.Landlord!.FullName} (the \"Landlord\") and {request.Tenant!.FullName} (the \"Tenant\").",
                            isArabic));

                        body.Item().PaddingTop(8).AlignLeftOrRight(isArabic).Text(TextValue(
                            isArabic
                                ? $"يوافق المؤجر على تأجير العقار المسمى {ResolveLocalizedValue(property.ListingTitle, isArabic)} والكائن في {BuildPropertyLocation(property, isArabic)}."
                                : $"The Landlord agrees to rent to the Tenant the property identified as {ResolveLocalizedValue(property.ListingTitle, isArabic)}, located at {BuildPropertyLocation(property, isArabic)}.",
                            isArabic));
                    }));

                column.Item().Row(row =>
                {
                    row.RelativeItem().Element(card => ComposePartyCard(card, T("Landlord", "المؤجر", isArabic), request.Landlord!, isArabic));
                    row.ConstantItem(12);
                    row.RelativeItem().Element(card => ComposePartyCard(card, T("Tenant", "المستأجر", isArabic), request.Tenant!, isArabic));
                });

                column.Item().Row(row =>
                {
                    row.RelativeItem().Element(card =>
                        ComposeInfoCard(card, T("Financial Terms", "الشروط المالية", isArabic), new[]
                        {
                            (T("Rent Amount", "قيمة الإيجار", isArabic), FormatMoney(rentalTerms.RentAmount, rentalTerms.Currency, isArabic)),
                            (T("Total Contract Amount", "إجمالي قيمة العقد", isArabic), FormatMoney(rentalTerms.TotalContractAmount, rentalTerms.Currency, isArabic)),
                            (T("Payment Frequency", "تكرار الدفع", isArabic), FormatPaymentFrequency(rentalTerms.PaymentFrequency, isArabic))
                        }, isArabic));

                    row.ConstantItem(12);

                    row.RelativeItem().Element(card =>
                        ComposeInfoCard(card, T("Term and Occupancy", "المدة والإشغال", isArabic), new[]
                        {
                            (T("Lease Start", "بداية العقد", isArabic), TextValue(FormatDate(rentalTerms.LeaseStartDate), isArabic)),
                            (T("Lease End", "نهاية العقد", isArabic), TextValue(FormatDate(rentalTerms.LeaseEndDate), isArabic)),
                            (T("Usage", "الاستخدام", isArabic), T("Residential use only", "للاستخدام السكني فقط", isArabic))
                        }, isArabic));
                });

                column.Item().Row(row =>
                {
                    row.RelativeItem().Element(card =>
                        ComposeInfoCard(card, T("Property Details", "تفاصيل العقار", isArabic), new[]
                        {
                            (T("Title", "العنوان", isArabic), ValueText(property.ListingTitle, isArabic)),
                            (T("Type", "النوع", isArabic), ValueText(property.Type, isArabic)),
                            (T("Address", "العنوان", isArabic), ValueText(property.AddressLine, isArabic)),
                            (T("City", "المدينة", isArabic), ValueText(property.City, isArabic)),
                            (T("Governorate", "المحافظة", isArabic), ValueText(property.Governorate, isArabic)),
                            (T("Zip Code", "الرمز البريدي", isArabic), ValueText(property.ZipCode, isArabic)),
                            (T("Coordinates", "الإحداثيات", isArabic), TextValue($"{property.Latitude:F4}, {property.Longitude:F4}", isArabic))
                        }, isArabic));

                    row.ConstantItem(12);

                    row.RelativeItem().Element(card =>
                        ComposeInfoCard(card, T("Property Specifications", "مواصفات العقار", isArabic), new[]
                        {
                            (T("Bedrooms", "غرف النوم", isArabic), TextValue(property.Bedrooms.ToString(CultureInfo.InvariantCulture), isArabic)),
                            (T("Beds", "الأسرة", isArabic), TextValue(property.Beds.ToString(CultureInfo.InvariantCulture), isArabic)),
                            (T("Bathrooms", "الحمامات", isArabic), TextValue(property.Bathrooms.ToString(CultureInfo.InvariantCulture), isArabic)),
                            (T("Area", "المساحة", isArabic), TextValue(isArabic ? $"{property.SquareMeters} متر مربع" : $"{property.SquareMeters} sqm", isArabic)),
                            (T("Max Occupants", "الحد الأقصى للشاغلين", isArabic), TextValue(property.MaxOccupants.ToString(CultureInfo.InvariantCulture), isArabic)),
                            (T("Shared Space", "سكن مشترك", isArabic), T(property.IsShared ? "Yes" : "No", property.IsShared ? "نعم" : "لا", isArabic))
                        }, isArabic));
                });

                column.Item().Element(section =>
                    ComposeSection(section, T("Property Description & Amenities", "وصف العقار والمرافق", isArabic), isArabic, body =>
                    {
                        if (!string.IsNullOrWhiteSpace(property.Description))
                        {
                            body.Item().PaddingBottom(4).AlignLeftOrRight(isArabic).Text(T("Description:", "الوصف:", isArabic)).SemiBold().FontColor("#12343B");
                            body.Item().PaddingBottom(12).AlignLeftOrRight(isArabic).Text(ValueText(property.Description, isArabic));
                        }

                        var amenities = SplitLocalizedList(property.Amenities, isArabic).ToList();
                        if (amenities.Count > 0)
                        {
                            body.Item().PaddingBottom(4).AlignLeftOrRight(isArabic).Text(T("Amenities:", "المرافق:", isArabic)).SemiBold().FontColor("#12343B");
                            foreach (var amenity in amenities)
                            {
                                ComposeBullet(body, amenity, isArabic);
                            }
                        }
                    }));

                var mediaEntries = (property.MediaPaths ?? [])
                    .Where(path => !string.IsNullOrWhiteSpace(path))
                    .Select(relativePath =>
                    {
                        var cleanPath = relativePath.Trim();
                        var absolutePath = Path.Combine(webRootPath, cleanPath.TrimStart('/', '\\'));
                        return new
                        {
                            RelativePath = cleanPath,
                            AbsolutePath = absolutePath,
                            Exists = File.Exists(absolutePath)
                        };
                    })
                    .ToList();

                if (mediaEntries.Count > 0)
                {
                    column.Item().Element(section =>
                        ComposeSection(section, T("Property Images", "صور العقار", isArabic), isArabic, body =>
                        {
                            body.Item().Grid(grid =>
                            {
                                grid.Spacing(10);
                                grid.Columns(3);

                                foreach (var mediaEntry in mediaEntries)
                                {
                                    if (mediaEntry.Exists)
                                    {
                                        grid.Item().Height(120).Image(mediaEntry.AbsolutePath).FitArea();
                                        continue;
                                    }

                                    grid.Item().Height(120).Border(1).BorderColor("#D5E3E6").Background("#F6FAFA").Padding(10).Column(card =>
                                    {
                                        card.Item().AlignCenter().Text(T("Image unavailable", "الصورة غير متاحة", isArabic))
                                            .FontColor("#5E777D")
                                            .FontSize(9)
                                            .SemiBold();

                                        card.Item().PaddingTop(8).AlignCenter().Text(TextValue(Path.GetFileName(mediaEntry.RelativePath), isArabic))
                                            .FontColor("#35555D")
                                            .FontSize(8.5f);
                                    });
                                }
                            });
                        }));
                }

                column.Item().Element(section =>
                    ComposeSection(section, T("Core Obligations", "الالتزامات الأساسية", isArabic), isArabic, body =>
                    {
                        foreach (var obligation in GetCoreObligations(isArabic))
                        {
                            ComposeBullet(body, obligation, isArabic);
                        }
                    }));

                var rulesAndTerms = SplitLocalizedList(property.Rules, isArabic)
                    .Concat((request.AdditionalTerms ?? []).Select(term => ResolveLocalizedValue(term, isArabic)).Where(term => !string.IsNullOrWhiteSpace(term)))
                    .ToList();

                if (rulesAndTerms.Count > 0)
                {
                    column.Item().Element(section =>
                        ComposeSection(section, T("Property Rules and Additional Terms", "قواعد العقار والشروط الإضافية", isArabic), isArabic, body =>
                        {
                            foreach (var entry in rulesAndTerms)
                            {
                                ComposeBullet(body, entry, isArabic);
                            }
                        }));
                }

                column.Item().Element(section =>
                    ComposeSection(section, T("Electronic Signature and Consent", "التوقيع الإلكتروني والموافقة", isArabic), isArabic, body =>
                    {
                        body.Item().PaddingTop(8).AlignLeftOrRight(isArabic).Text(TextValue(
                            isArabic
                                ? "يقر الطرفان بأن الضغط على زر قبول العقد داخل المنصة واستكمال التحقق من الهوية يشكلان الفعل الإلكتروني المعتبر توقيعًا لهذا الاتفاق."
                                : "The parties acknowledge that pressing the platform acceptance button and completing identity verification together form the electronic act of signature for this agreement.",
                            isArabic));
                    }));

                column.Item().Border(1).BorderColor("#D5E3E6").Background("#F6FAFA").Padding(16).Column(block =>
                {
                    block.Item().AlignLeftOrRight(isArabic).Text(T("Digital Verification Block", "قسم التحقق الرقمي", isArabic)).FontSize(15).SemiBold().FontColor("#12343B");
                    block.Item().PaddingTop(10).Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });

                        table.Cell().PaddingRight(8).PaddingBottom(10).Element(cell => ComposeVerificationCell(cell, T("Digitally Signed By", "التوقيع الرقمي باسم", isArabic), ValueText(signature.SignerName, isArabic), isArabic));
                        table.Cell().PaddingLeft(8).PaddingBottom(10).Element(cell => ComposeVerificationCell(cell, T("National ID", "الرقم القومي", isArabic), ValueText(signature.SignerNationalId, isArabic), isArabic));
                        table.Cell().PaddingRight(8).PaddingBottom(10).Element(cell => ComposeVerificationCell(cell, T("Timestamp", "الختم الزمني", isArabic), TextValue($"{signature.SignedAtUtc:yyyy-MM-dd HH:mm:ss} UTC", isArabic), isArabic));
                        table.Cell().PaddingLeft(8).PaddingBottom(10).Element(cell => ComposeVerificationCell(cell, T("Total Amount", "إجمالي المبلغ", isArabic), FormatMoney(rentalTerms.TotalContractAmount, rentalTerms.Currency, isArabic), isArabic));
                    });

                    block.Item().PaddingTop(12).AlignLeftOrRight(isArabic).Text(TextValue(ResolveGoverningLawNote(request.GoverningLawNote, isArabic), isArabic)).Italic().FontColor("#35555D");
                });

                column.Item().Element(section =>
                    ComposeSection(section, T("Acknowledgement", "الإقرار", isArabic), isArabic, body =>
                    {
                        foreach (var acknowledgement in GetAcknowledgements(isArabic))
                        {
                            ComposeBullet(body, acknowledgement, isArabic);
                        }
                    }));
            });
        }

        private void ComposeFooter(IContainer container, bool isArabic)
        {
            container.PaddingTop(8).BorderTop(1).BorderColor("#D5E3E6").Row(row =>
            {
                if (isArabic)
                {
                    row.ConstantItem(90).AlignRightOrLeft(isArabic).Text(text =>
                    {
                        text.Span(T("Page ", "الصفحة ", isArabic)).FontSize(9).FontColor(Colors.Grey.Darken1);
                        text.CurrentPageNumber().FontSize(9).SemiBold();
                    });

                    row.RelativeItem()
                        .AlignLeftOrRight(isArabic)
                        .Text("تم إنشاء هذا المستند والتحقق منه إلكترونيًا بواسطة منصة مارن.")
                        .FontSize(9)
                        .FontColor(Colors.Grey.Darken1);
                }
                else
                {
                    row.RelativeItem()
                        .AlignLeftOrRight(isArabic)
                        .Text("Electronically generated and verified by MARN.")
                        .FontSize(9)
                        .FontColor(Colors.Grey.Darken1);

                    row.ConstantItem(90).AlignRightOrLeft(isArabic).Text(text =>
                    {
                        text.Span(T("Page ", "الصفحة ", isArabic)).FontSize(9).FontColor(Colors.Grey.Darken1);
                        text.CurrentPageNumber().FontSize(9).SemiBold();
                    });
                }
            });
        }

        private void ComposeSection(IContainer container, string title, bool isArabic, Action<ColumnDescriptor> content)
        {
            container.Column(column =>
            {
                column.Item().AlignLeftOrRight(isArabic).Text(title).FontSize(16).SemiBold().FontColor("#12343B");
                column.Item().PaddingTop(8).BorderTop(2).BorderColor("#D5E3E6");
                column.Item().PaddingTop(10).Column(content);
            });
        }

        private void ComposePartyCard(IContainer container, string title, PartyInfo party, bool isArabic)
        {
            ComposeInfoCard(container, title, new[]
            {
                (T("Name", "الاسم", isArabic), ValueText(party.FullName, isArabic)),
                (T("National ID", "الرقم القومي", isArabic), ValueText(party.NationalId, isArabic)),
                (T("Email", "البريد الإلكتروني", isArabic), ValueText(party.Email, isArabic)),
                (T("Phone", "الهاتف", isArabic), ValueText(party.PhoneNumber, isArabic)),
                (T("Address", "العنوان", isArabic), ValueText(party.Address, isArabic))
            }, isArabic);
        }

        private void ComposeInfoCard(IContainer container, string title, IEnumerable<(string Label, string Value)> items, bool isArabic)
        {
            container.Border(1).BorderColor("#D5E3E6").Padding(16).Column(column =>
            {
                column.Spacing(6);
                column.Item().AlignLeftOrRight(isArabic).Text(title).FontSize(15).SemiBold().FontColor("#12343B");

                foreach (var item in items)
                {
                    column.Item().Row(row =>
                    {
                        if (isArabic)
                        {
                            row.RelativeItem().ContentFromRightToLeft().AlignRight().Text(item.Value).SemiBold();
                            row.RelativeItem().ContentFromRightToLeft().AlignRight().Text(item.Label).FontColor("#4B6268");
                        }
                        else
                        {
                            row.RelativeItem().Text(item.Label).FontColor("#4B6268");
                            row.RelativeItem().AlignRight().Text(item.Value).SemiBold();
                        }
                    });
                }
            });
        }

        private void ComposeMetaCell(IContainer container, string label, string value, bool isArabic)
        {
            container.Column(column =>
            {
                column.Item().AlignLeftOrRight(isArabic).Text(label).Bold().FontColor("#12343B");
                column.Item().AlignLeftOrRight(isArabic).Text(value).FontColor("#35555D");
            });
        }

        private void ComposeBullet(ColumnDescriptor column, string text, bool isArabic)
        {
            column.Item().Row(row =>
            {
                if (isArabic)
                {
                    row.RelativeItem().ContentFromRightToLeft().AlignRight().Text(text);
                    row.ConstantItem(14).ContentFromRightToLeft().AlignRight().Text("•").FontColor("#12343B");
                }
                else
                {
                    row.ConstantItem(14).Text("•").FontColor("#12343B");
                    row.RelativeItem().Text(text);
                }
            });
        }

        private void ComposeVerificationCell(IContainer container, string label, string value, bool isArabic)
        {
            container.Column(column =>
            {
                column.Item().AlignLeftOrRight(isArabic).Text(label).FontSize(9).SemiBold().FontColor("#5E777D");
                column.Item().PaddingTop(2).AlignLeftOrRight(isArabic).Text(value).SemiBold().FontColor("#12343B");
            });
        }

        private string BuildPropertyLocation(PropertyInfo property, bool isArabic)
        {
            var segments = new[]
            {
                ResolveLocalizedValue(property.AddressLine, isArabic),
                ResolveLocalizedValue(property.UnitNumber, isArabic),
                ResolveLocalizedValue(property.City, isArabic),
                ResolveLocalizedValue(property.Country, isArabic)
            }
            .Where(segment => !string.IsNullOrWhiteSpace(segment))
            .Select(segment => TextValue(segment, isArabic))
            .ToList();

            return string.Join(isArabic ? "، " : ", ", segments);
        }

        private IEnumerable<string> GetCoreObligations(bool isArabic)
        {
            var obligations = isArabic
                ? new[]
                {
                    "يلتزم المستأجر بسداد جميع المبالغ المستحقة في مواعيدها والمحافظة على العقار بحالة جيدة، مع استثناء الاستهلاك المعتاد.",
                    "يلتزم المؤجر بتسليم العقار في تاريخ بدء العقد بحالة مناسبة للسكن.",
                    "إذا تأخر المستأجر عن سداد الإيجار لأكثر من خمسة عشر (15) يومًا بعد تاريخ الاستحقاق وبعد توجيه إشعار رسمي عبر المنصة أو قنوات الاتصال المعتمدة، جاز للمؤجر اتخاذ الإجراءات القانونية المناسبة.",
                    "يشمل مبلغ الإيجار تكاليف الخدمات الأساسية المتعلقة بالعقار، مثل المياه والكهرباء ورسوم الصيانة الدورية وأي خدمات إضافية يحددها المؤجر صراحة ما لم ينص على خلاف ذلك.",
                    "يتحمل المستأجر تكاليف الإصلاحات التشغيلية البسيطة الناتجة عن الاستخدام المعتاد، بينما يتحمل المؤجر الإصلاحات الجوهرية والهيكلية اللازمة للحفاظ على العقار صالحًا للسكن.",
                    "تقدم طلبات الاسترداد أو المطالبات بالتعويض أو إنهاء العقد المبكر عبر مسار العمل المعتمد داخل المنصة بين الطرفين.",
                    "إذا رغب أي من الطرفين في إنهاء هذا العقد قبل تاريخ انتهائه، يقدم الطلب أولًا عبر المنصة لمحاولة التسوية الودية. وعند تعذر التسوية، تختص محاكم القاهرة الابتدائية بنظر النزاع.",
                    "في حالة وجود أي اختلاف في تفسير أو ترجمة بنود العقد بين النسخة العربية والنسخة الإنجليزية، تكون النسخة العربية هي المرجع المعتمد."
                }
                : new[]
                {
                    "The Tenant shall pay all amounts due on time and maintain the property in good condition, ordinary wear and tear excepted.",
                    "The Landlord shall provide possession of the property on the lease start date in a condition reasonably fit for residential occupancy.",
                    "If the Tenant fails to pay rent for more than fifteen (15) days after the due date and following an official notice through the platform or approved communication channels, the Landlord may initiate appropriate legal action.",
                    "The agreed rental amount includes the costs of basic property-related services including water, electricity, routine maintenance fees, and any additional services specified by the Landlord unless otherwise stated.",
                    "The Tenant shall bear the costs of minor operational repairs resulting from normal use, while the Landlord shall bear major and structural repairs necessary to maintain the property in a habitable condition.",
                    "Refund requests, damage claims, and requests related to early contract termination shall be submitted and documented through the platform workflow used by the parties.",
                    "In the event that either party wishes to terminate this agreement before its expiration date, a request shall first be submitted through the platform for review and amicable resolution attempts between the parties. If no resolution is reached, disputes shall fall under the jurisdiction of Cairo Primary Courts.",
                    "In the event of any discrepancy, conflict, or inconsistency in the interpretation or translation of the terms of this Agreement between the Arabic and English versions, the Arabic version shall prevail and be considered the governing version."
                };

            return obligations.Select(item => TextValue(item, isArabic));
        }

        private IEnumerable<string> GetAcknowledgements(bool isArabic)
        {
            var entries = isArabic
                ? new[]
                {
                    "بقبوله الإلكتروني، يقر المستأجر بأنه راجع العقد كاملًا وأن بيانات الهوية المقدمة صحيحة ويمكن الاعتماد على السجل الرقمي بوصفه دليلًا على القبول.",
                    "جرى توثيق قبول الطرفين إلكترونيًا عبر المنصة وربطه ببيانات الهوية وسجلات التحقق الرقمي."
                }
                : new[]
                {
                    "By accepting electronically, the Tenant confirms that the contract was reviewed in full, that the provided identity details are accurate, and that the digital record may be relied upon as evidence of assent.",
                    "Acceptance by both parties has been electronically documented through the platform and linked to identity information and digital verification records."
                };

            return entries.Select(item => TextValue(item, isArabic));
        }

        private string ResolveGoverningLawNote(string? rawValue, bool isArabic)
        {
            return ResolveLocalizedValue(
                rawValue,
                isArabic,
                englishFallback: EnglishGoverningLawNote,
                arabicFallback: ArabicGoverningLawNote);
        }

        private string FormatPaymentFrequency(PaymentFrequency frequency, bool isArabic)
        {
            return _localizer.GetEnumDisplayName(frequency, isArabic ? ArabicCulture : EnglishCulture);
        }

        private string FormatMoney(decimal amount, string? currency, bool isArabic)
        {
            var resolvedCurrency = string.IsNullOrWhiteSpace(currency) ? "EGP" : currency.Trim();
            var formattedAmount = amount.ToString("N2", CultureInfo.InvariantCulture);
            var text = isArabic
                ? $"{resolvedCurrency} {formattedAmount}"
                : $"{formattedAmount} {resolvedCurrency}";

            return TextValue(text, isArabic);
        }

        private static string FormatDate(DateOnly? value)
        {
            return value?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? "N/A";
        }

        private static string FormatDateTime(DateTime? value)
        {
            return value?.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture) ?? "N/A";
        }

        private string ValueText(string? rawValue, bool isArabic)
        {
            return TextValue(ResolveLocalizedValue(rawValue, isArabic), isArabic);
        }

        private string TextValue(string? rawValue, bool isArabic)
        {
            return BidiText.Format(rawValue, isArabic ? ArabicCulture : EnglishCulture);
        }

        private string ResolveLocalizedValue(string? rawValue, bool isArabic, string? englishFallback = null, string? arabicFallback = null)
        {
            if (string.IsNullOrWhiteSpace(rawValue))
            {
                return isArabic
                    ? arabicFallback ?? "غير متاح"
                    : englishFallback ?? "N/A";
            }

            var trimmedValue = rawValue.Trim();
            var localizedParts = trimmedValue.Split(LocalizedSeparator, 2, StringSplitOptions.None);
            if (localizedParts.Length == 2)
            {
                return (isArabic ? localizedParts[1] : localizedParts[0]).Trim();
            }

            if (isArabic && !string.IsNullOrWhiteSpace(englishFallback) && string.Equals(trimmedValue, englishFallback, StringComparison.Ordinal))
            {
                return arabicFallback ?? trimmedValue;
            }

            if (!isArabic && !string.IsNullOrWhiteSpace(arabicFallback) && string.Equals(trimmedValue, arabicFallback, StringComparison.Ordinal))
            {
                return englishFallback ?? trimmedValue;
            }

            return trimmedValue;
        }

        private IEnumerable<string> SplitLocalizedList(string? rawValue, bool isArabic)
        {
            if (string.IsNullOrWhiteSpace(rawValue))
            {
                return [];
            }

            return rawValue
                .Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(item => ResolveLocalizedValue(item, isArabic))
                .Where(item => !string.IsNullOrWhiteSpace(item))
                .Select(item => TextValue(item, isArabic));
        }

        private string T(string english, string arabic, bool isArabic)
        {
            return TextValue(isArabic ? arabic : english, isArabic);
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

        private static void EnsureFontsRegistered()
        {
            if (Interlocked.Exchange(ref _fontsRegistered, 1) == 1)
            {
                return;
            }

            var fontsDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
            var fontFiles = new[]
            {
                "tahoma.ttf",
                "tahomabd.ttf",
                "arial.ttf",
                "arialbd.ttf",
                "ariali.ttf",
                "arialbi.ttf"
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

    internal static class ContractPdfContainerExtensions
    {
        public static IContainer AlignLeftOrRight(this IContainer container, bool isArabic)
        {
            return isArabic ? container.ContentFromRightToLeft().AlignRight() : container.AlignLeft();
        }

        public static IContainer AlignRightOrLeft(this IContainer container, bool isArabic)
        {
            return isArabic ? container.ContentFromRightToLeft().AlignLeft() : container.AlignRight();
        }
    }
}
