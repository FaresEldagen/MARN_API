# Frontend Integration Guide

## What Changed

The API is now localization-aware and returns localized human-readable text based on `Accept-Language`.

Supported headers:

- `Accept-Language: en`
- `Accept-Language: ar`

## What Frontend Should Use

### Use `code` for logic

Do this:
بص مش شرط تعمل تشيك للكود نفسه انا ببقا مجهزلك الرسالة كدا كدا اعرف بس ان
status code
دا بتاع ايرور واعمل زي حاجة تظهر رسالة الايرور ايا كانت اية
لكن الكلام اللي تحت دا مجرد اقتراح من شات جي بي تي لكن هيتعبكم لو قعدتوا تتشكوا كل
code
لو حابب تشوف اكتر فايلات الترجمة العربي والانجليزي هتلاقيها في الباك في فولدر ريسورسز

```ts
if (response.code === "INVALID_EMAIL_OR_PASSWORD") {
  showLoginError(response.message);
}
```

Do not do this:

```ts
if (response.message === "Invalid email or password") {
  // fragile
}
```

`message` is now localized and can be Arabic or English.

## Success Responses

Success responses still use:

- `code`
- `message`
- `data`

Some success codes remain generic (`SUCCESS`, `CREATED`, `REQUIRES_TWO_FACTOR`) unless the backend explicitly defines a more specific code.

## Error Responses

All major framework and service errors now use the shared error contract:

- auth challenge
- forbidden access
- banned-account access
- validation failures
- rate limiting
- service-layer failures
- exception middleware responses

## Validation Errors

Validation failures now come back as:

```json
{
  "code": "VALIDATION_FAILED",
  "message": "...",
  "errors": {
    "FieldName": ["..."]
  }
}
```

Frontend should:

1. read `code`
2. show `message` as the page/form summary
3. bind `errors[field]` to field-level validation UI

## Enum Responses

Enum endpoints return:

- `id`
- `name` -> stable internal value
- `displayName` -> localized UI label

Example:

```json
{
  "id": 1,
  "name": "Verified",
  "displayName": "موثق"
}
```

Use:

- `name` for internal app logic when needed
- `displayName` for what the user sees

## Admin Property APIs

### Property list

`GET /api/Admin/stats/properties`

New row fields:

- `averageRating`
- `commentsCount`

### Property details

`GET /api/Admin/stats/properties/{propertyId}`

Use this for the admin property details page instead of stitching together several endpoints.

## Reports and Documents

### Analytics reports

Arabic PDF analytics reports now use RTL alignment. No frontend change is required beyond passing `Accept-Language: ar`.

### Contract PDFs

Contract downloads remain binary file responses. The backend handles bilingual generation internally.

## Recommended Frontend Migration Checklist

1. Audit any logic using `message` and switch it to `code`.
2. Pass `Accept-Language` on every API request.
3. Prefer `displayName` over raw enum `name` in UI.
4. Update form validation handling to read the shared `errors` dictionary.
5. Use the new admin property details endpoint for the property review page.
