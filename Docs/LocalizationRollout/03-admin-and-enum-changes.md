# Admin and Enum Changes

## Admin Property Stats

Endpoint:

- `GET /api/Admin/stats/properties`

Each property row now includes:

- `averageRating`
- `commentsCount`

These are returned alongside the existing owner/status/location/price fields.

## Admin Property Details Endpoint

New endpoint:

- `GET /api/Admin/stats/properties/{propertyId}`

This returns a deeper admin-facing property payload including:

- base property details
- owner info
- media
- amenities
- rules
- comments
- ratings
- contracts
- booking requests
- saved count
- comments count
- ratings count

Localized display fields are included for:

- property status
- property type
- rental unit
- city
- governorate
- contract status
- contract anchoring status
- payment frequency
- amenity types

## Enum Controller Coverage

`EnumController` now exposes the missing project enums, including:

- `payment-statuses`
- `payment-schedule-statuses`
- `report-moderation-action-types`
- `roommate-search-statuses`
- `admin-analytics-report-formats`
- `admin-analytics-report-scopes`
- `admin-analytics-report-periods`

`GET /api/Enum/all` also now includes these groups.

## Admin Analytics Report RTL Fix

`AdminAnalyticsReportService` now adapts PDF layout for Arabic by:

- right-aligning text blocks
- reversing header/cell order where needed
- keeping section titles and table content visually correct for RTL reading

CSV exports remain language-localized but do not use visual RTL layout rules because CSV is plain data output.
