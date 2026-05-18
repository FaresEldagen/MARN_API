# Error Codes and Response Contract

## Response Shapes

### Success

```json
{
  "code": "SUCCESS",
  "message": "Success.",
  "data": {}
}
```

### Error

```json
{
  "code": "INVALID_EMAIL_OR_PASSWORD",
  "message": "Invalid email or password",
  "action": null,
  "details": null,
  "statusCode": 401,
  "path": "/api/Account/login",
  "traceId": "...",
  "timestamp": "...",
  "errors": null
}
```

### Validation error

```json
{
  "code": "VALIDATION_FAILED",
  "message": "The request payload is invalid.",
  "statusCode": 400,
  "path": "/api/Support/contact-us",
  "traceId": "...",
  "timestamp": "...",
  "errors": {
    "PhoneNumber": [
      "Phone number is not valid."
    ]
  }
}
```

## How Error Codes Are Produced

### Explicit codes

Some errors use dedicated fixed codes, for example:

- `AUTHENTICATION_REQUIRED`
- `ACCESS_TOKEN_INVALID`
- `ACCESS_TOKEN_EXPIRED`
- `ACCESS_FORBIDDEN`
- `ACCOUNT_BANNED_ACCESS_DENIED`
- `VALIDATION_FAILED`
- `RATE_LIMIT_EXCEEDED`

### Derived scenario codes

Most service-layer errors now become stable codes automatically from the scenario message. Example:

- `Invalid email or password` -> `INVALID_EMAIL_OR_PASSWORD`
- `Current password is incorrect` -> `CURRENT_PASSWORD_IS_INCORRECT`
- `Property not found.` -> `PROPERTY_NOT_FOUND`

This keeps the API informative without forcing every service method to manually define codes line by line.

## Controller/Service Contract Rule

### For service authors

Use `ServiceResult<T>.Fail(...)` with:

- a specific scenario message
- an appropriate `ServiceResultType`
- optional explicit `code` only when a fixed public code is required

### For controller authors

Use `HandleServiceResult(...)` or the `BaseController` localized helpers so the shared error contract stays consistent.

## When To Add an Explicit Code

Prefer explicit codes when:

- the same message could appear in multiple flows but must be tracked separately
- the frontend needs to branch on a public documented value
- the code should stay fixed even if the English wording changes

## Validation Strategy

Validation now follows three layers:

1. explicit DTO messages when the rule is custom or business-specific
2. resource-backed literal translations for existing messages
3. pattern-based localization for framework-generated messages

## Resource Naming

- direct code entries: `AUTHENTICATION_REQUIRED`
- literal fallback entries: `TEXT_INVALID_EMAIL_OR_PASSWORD`
- enum display entries: `ENUM_PropertyStatus_Verified`
- role labels: `ROLE_Admin`

## Important Compatibility Note

Frontend logic must use `code` for behavior. `message` is localized and may change by language.
