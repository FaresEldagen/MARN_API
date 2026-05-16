# Stage 6 - Analytics Export APIs

## Stage purpose

This stage generates downloadable admin analytics reports and exposes report history.

Supported outputs:
- PDF via QuestPDF
- CSV via CsvHelper

## Common auth

All endpoints in this stage:
- require authentication
- require role: `Admin`

## Enums

### `AdminAnalyticsReportScope`

- `Overview = 1`
- `Users = 2`
- `Properties = 3`
- `Contracts = 4`
- `Revenue = 5`
- `Full = 6`

### `AdminAnalyticsReportFormat`

- `Pdf = 1`
- `Csv = 2`

### `AdminAnalyticsReportPeriod`

- `AllTime = 1`
- `ThisMonth = 2`
- `ThisYear = 3`
- `Custom = 4`

## 1. Generate analytics report

```http
POST /api/admin/analytics-reports/generate
Content-Type: application/json
```

Body:

```json
{
  "scope": "Revenue",
  "format": "Csv",
  "period": "ThisYear",
  "fromUtc": null,
  "toUtc": null
}
```

### Request fields

| Field | Type | Required | Default | Notes |
|---|---|---:|---:|---|
| `scope` | `AdminAnalyticsReportScope` | no | `Full` | determines exported module |
| `format` | `AdminAnalyticsReportFormat` | no | `Pdf` | `Full + Csv` is invalid |
| `period` | `AdminAnalyticsReportPeriod` | no | `ThisMonth` | enum, not free string |
| `fromUtc` | `datetime?` | no | `null` | only used for `Custom` |
| `toUtc` | `datetime?` | no | `null` | only used for `Custom` |

### Null behavior

#### `scope`

If omitted:
- defaults to `Full`

#### `format`

If omitted:
- defaults to `Pdf`

#### `period`

If omitted:
- defaults to `ThisMonth`

#### `fromUtc` / `toUtc`

- ignored unless `period = Custom`
- required when `period = Custom`

### Validation rules

#### `Full + Csv`

Rejected with `400 Bad Request`.

Reason:
- CSV is only supported for:
  - `Overview`
  - `Users`
  - `Properties`
  - `Contracts`
  - `Revenue`

#### `Custom` period

Requires:
- `fromUtc`
- `toUtc`

And:
- `fromUtc < toUtc`

### What the service builds

Depending on scope, the service pulls:
- overview data
- detailed users data
- detailed properties data
- detailed contracts data
- detailed revenue data

### Output file storage

Files are written under:

```text
wwwroot/reports/admin-analytics
```

### Success response

Returns `AdminAnalyticsReportDetailsDto`

Fields:

| Field | Type |
|---|---|
| `reportId` | `long` |
| `scope` | `AdminAnalyticsReportScope` |
| `format` | `AdminAnalyticsReportFormat` |
| `requestedPeriod` | `AdminAnalyticsReportPeriod` |
| `fromUtc` | `datetime?` |
| `toUtc` | `datetime?` |
| `grouping` | `string` |
| `fileName` | `string` |
| `fileSizeBytes` | `long` |
| `generatedAt` | `datetime` |
| `generatedByAdminId` | `guid` |
| `generatedByAdminName` | `string` |
| `contentType` | `string` |
| `downloadUrl` | `string` |

### Grouping meaning

- `day`
- `month`

Rule:
- custom ranges `<= 31` days use `day`
- otherwise use `month`
- `ThisMonth` uses `day`
- `ThisYear` uses `month`

## 2. Get report history

```http
GET /api/admin/analytics-reports?scope=Revenue&format=Pdf&year=2026&month=5&pageNumber=1&pageSize=20
```

### Query fields

| Field | Type | Required | Default |
|---|---|---:|---:|
| `scope` | `AdminAnalyticsReportScope?` | no | `null` |
| `format` | `AdminAnalyticsReportFormat?` | no | `null` |
| `year` | `int?` | no | `null` |
| `month` | `int?` | no | `null` |
| `pageNumber` | `int` | no | `1` |
| `pageSize` | `int` | no | `20` |

### Validation

- `year` range: `1..9999`
- `month` range: `1..12`
- page normalization:
  - `< 1` -> `1`
  - `pageSize < 1` -> `20`
  - `pageSize > 100` -> `100`

### Filter behavior

- `scope` -> exact enum match
- `format` -> exact enum match
- `year` -> compares against `(FromUtc ?? GeneratedAt).Year`
- `month` -> compares against `(FromUtc ?? GeneratedAt).Month`

### Response

Paged `AdminAnalyticsReportListItemDto` rows.

### 3. Get one report metadata row

```http
GET /api/admin/analytics-reports/{reportId}
```

### Response

One `AdminAnalyticsReportDetailsDto`

### Errors

- `404 Not Found`
  - report row does not exist

## 4. Download one analytics report file

```http
GET /api/admin/analytics-reports/{reportId}/download
```

### Response

- raw file response
- content type is:
  - `application/pdf`
  - or `text/csv`

### Errors

- `404`
  - report row not found
  - physical file missing from disk

## Export content notes

### PDF

PDF exports include:
- title
- generated period
- generated-by admin name
- executive snapshot
- section tables depending on scope

### CSV

CSV row shapes differ by scope:

#### `Overview`

Rows are metric/value pairs.

#### `Users`

Columns include:
- user ID
- full name
- email
- account status
- deleted flag
- created at
- roles
- owned properties count
- active properties count
- renter contracts count
- owner contracts count
- active contracts count
- cancelled contracts count
- payments made count
- payments received count
- total paid amount
- total received amount
- reports submitted count
- reports against user count

#### `Properties`

Columns include:
- property ID
- title
- owner info
- status
- type
- city / state
- price
- active flag
- deleted flag

#### `Contracts`

Columns include:
- contract ID
- status
- created at
- lease dates
- total amount
- payment frequency
- property info
- owner info
- renter info

#### `Revenue`

Columns include:
- payment ID
- contract ID
- schedule ID
- status
- sales
- revenue
- owner payout
- paid at
- available at
- currency
- property info
- owner info
- renter info
