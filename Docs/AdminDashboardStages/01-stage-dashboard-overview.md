# Stage 1 - Dashboard Overview API

## Stage purpose

This stage powers the main admin dashboard landing page:
- the top cards
- revenue and sales summary
- monthly revenue graph
- contract status summary

## Base route

```http
GET /api/admin/dashboard/overview
```

## Auth

- requires authentication
- requires role: `Admin`

## Request

This endpoint has:
- no request body
- no query parameters
- no route parameters

## Success response

HTTP status:

```http
200 OK
```

Response wrapper:

```json
{
  "message": null,
  "data": {
    "totalUsers": {
      "value": 12,
      "trendPercentage": 100.0
    },
    "totalProperties": {
      "value": 9,
      "trendPercentage": 100.0
    },
    "pendingVerifications": {
      "value": 2,
      "trendPercentage": 100.0
    },
    "totalContracts": {
      "value": 12,
      "trendPercentage": 100.0
    },
    "revenueSummary": {
      "totalRevenue": 19650.0,
      "totalSales": 196500.0,
      "newUsersThisMonth": 2,
      "activeContracts": 9,
      "revenueTrendPercentage": 0.0
    },
    "contractSummary": {
      "all": 12,
      "active": 9,
      "pending": 1,
      "expired": 1,
      "cancelled": 1
    },
    "monthlyRevenue": [
      {
        "year": 2025,
        "month": 12,
        "label": "Dec",
        "revenue": 600.0,
        "sales": 6000.0
      }
    ]
  }
}
```

## Response fields

### `totalUsers`

- `value: long`
- `trendPercentage: decimal?`

Meaning:
- counts non-admin users only
- excludes soft-deleted users because the overview query uses the normal user query filter
- trend compares the current month window vs the equivalent previous month window

### `totalProperties`

- `value: long`
- `trendPercentage: decimal?`

Meaning:
- counts properties using the normal property query filter
- soft-deleted properties are excluded

### `pendingVerifications`

- `value: long`
- `trendPercentage: decimal?`

Meaning:
- combines:
  - users with account status `Pending`
  - properties with property status `Pending`

### `totalContracts`

- `value: long`
- `trendPercentage: decimal?`

Meaning:
- counts all contracts regardless of status

### `revenueSummary`

- `totalRevenue: decimal`
  - sum of `Payment.PlatformFee`
- `totalSales: decimal`
  - sum of `Payment.AmountTotal`
- `newUsersThisMonth: long`
  - non-admin users created in the current month
- `activeContracts: long`
  - contracts with status `Active`
- `revenueTrendPercentage: decimal?`
  - compares current month revenue vs previous month comparison window

### `contractSummary`

- `all: long`
- `active: long`
- `pending: long`
- `expired: long`
- `cancelled: long`

Contract status enum values used in this project:
- `Pending = 0`
- `Active = 1`
- `Cancelled = 2`
- `Expired = 3`

### `monthlyRevenue`

Array of:
- `year: int`
- `month: int`
- `label: string`
- `revenue: decimal`
- `sales: decimal`

Behavior:
- always returns the last 6 months
- missing months are filled with zero values
- graph values come from `Payment.PaidAt`

## Error responses

### `401 Unauthorized`

If:
- the request has no valid token
- the token has no valid user ID claim

Shape:

```json
{
  "message": "User ID not found in token",
  "statusCode": 401,
  "path": "/api/admin/dashboard/overview",
  "traceId": "...",
  "timestamp": "2026-05-16T08:00:00Z"
}
```

### `403 Forbidden`

If:
- the authenticated user is not in the `Admin` role

## Frontend notes

- this endpoint is meant for the dashboard landing page only
- it does not return drilldown tables
- card clicks should navigate to later-stage endpoints, not retry this endpoint with filters

