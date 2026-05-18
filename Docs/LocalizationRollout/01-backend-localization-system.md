# Backend Localization System

## Overview

The backend now supports request-time localization for English (`en`) and Arabic (`ar`) without changing the database language, enum names, or internal business rules.

The localization flow is:

1. Resolve request culture from `Accept-Language`.
2. Fall back to the authenticated user's saved language when the header is absent.
3. Fall back to English.
4. Localize:
   - API messages
   - validation messages
   - enum display names
   - notifications
   - email templates
   - analytics report text
   - contract document text

## Core Components

### Request culture

- `Program.cs`
- `Localization/LocalizationConstants.cs`
- `Services/Implementations/UserCultureService.cs`

`Accept-Language` is the highest-priority source. If it is missing, the app resolves the user's saved language and applies it to the request.

### Shared resource store

- `Resources/SharedResource.resx`
- `Resources/SharedResource.ar.resx`

These files hold:

- shared response messages
- validation strings
- enum display names
- notification templates
- analytics/report text
- role display names

### Text localizer

- `Services/Interfaces/IAppTextLocalizer.cs`
- `Services/Implementations/AppTextLocalizer.cs`
- `Localization/LocalizationKeyBuilder.cs`

`AppTextLocalizer` supports:

- direct resource lookup by key
- fallback lookup by literal text (`TEXT_*`)
- enum display name localization (`ENUM_*`)
- bidi-safe formatting for mixed Arabic and English text

### Payload localizer

- `Services/Interfaces/IResponsePayloadLocalizer.cs`
- `Services/Implementations/ResponsePayloadLocalizer.cs`

This localizes DTO fields that intentionally expose both stable values and localized display names.

## Error and Validation Localization

### Shared error contract

- `Controllers/BaseController.cs`
- `Models/ErrorResponse.cs`
- `Models/ServiceResult.cs`
- `Middleware/GlobalExceptionHandlingMiddleware.cs`

Behavior:

- specific service messages are no longer flattened into generic `BAD_REQUEST` or `UNAUTHORIZED`
- when a service does not provide an explicit error code, the backend derives one from the scenario message
- validation responses use the shared `ErrorResponse` shape instead of default ASP.NET `ProblemDetails`

### Validation localization

- `Program.cs`
- `Localization/ValidationMessageLocalizer.cs`

Validation errors are localized in two ways:

1. direct translation for known literal messages
2. pattern-based localization for framework-generated messages such as:
   - required fields
   - invalid email
   - invalid phone
   - string length limits
   - numeric/date ranges

## Authentication and Framework Responses

`Program.cs` now customizes JWT challenge/forbidden responses and rate-limit rejections so they return the same app-level error shape:

- `AUTHENTICATION_REQUIRED`
- `ACCESS_TOKEN_INVALID`
- `ACCESS_TOKEN_EXPIRED`
- `ACCESS_FORBIDDEN`
- `RATE_LIMIT_EXCEEDED`

The banned-account filter also now returns a localized structured error:

- `ACCOUNT_BANNED_ACCESS_DENIED`

## Notifications and Templates

### Notifications

- `Services/Implementations/NotificationService.cs`
- `Services/Implementations/NotificationContentLocalizer.cs`

Notifications can store localization keys plus arguments, then render in the request/user language when read. This is what enables old English fallback rows and new multilingual notification rows to coexist.

### Emails

- `Services/Implementations/EmailService.cs`

Email subjects and bodies now use localized templates and support Arabic rendering.

## Reports and Documents

### Admin analytics reports

- `Services/Implementations/AdminAnalyticsReportService.cs`

CSV/PDF report text is localized. The PDF report now also adapts alignment and table direction for Arabic.

### Contract PDF

- `Services/Implementations/ContractPdfGenerator.cs`

The contract document uses localized text, bidi-safe rendering, and the current dual-copy layout (English copy followed by Arabic copy).

## Adding New Translations

When adding a new user-facing message:

1. keep the internal logic and DB values unchanged
2. return a stable code when needed
3. add `TEXT_*` or keyed resource entries to both `.resx` files
4. for enums, add `ENUM_<EnumType>_<Value>`
5. for roles, add `ROLE_<RoleName>`

## Recommended Rule

Do not let frontend logic depend on the human-readable `message`. Use stable `code` values for behavior and use localized display/message fields for UI.
