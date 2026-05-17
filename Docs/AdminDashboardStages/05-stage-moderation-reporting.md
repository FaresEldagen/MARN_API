# Stage 5 - Moderation Reporting APIs

## Stage purpose

This stage covers two sides:

1. user-facing report submission
2. admin moderation queue, details, and review actions

## Shared enums

### `ReportableType`

- `User = 0`
- `Property = 1`
- `Message = 2`
- `PropertyComment = 3`

### `ReportStatus`

- `InReview = 0`
- `Resolved = 1`
- `Rejected = 2`

### `ReportModerationActionType`

- `BanUser = 1`
- `DeactivateProperty = 2`
- `HideMessage = 3`
- `HidePropertyComment = 4`

## Part A - User report submission

### Route

```http
POST /api/reports
```

### Auth

- requires authentication
- any authenticated user can submit

### Request body

```json
{
  "reportableType": "Message",
  "reportableTargetId": "00000000-0000-0000-0000-000000000101",
  "reason": "abusive message in chat"
}
```

### Request fields

| Field | Type | Required | Notes |
|---|---|---:|---|
| `reportableType` | `ReportableType enum` | yes | decides how `reportableTargetId` is parsed |
| `reportableTargetId` | `string` | yes | GUID for `User`/`Message`, positive number for `Property`/`PropertyComment` |
| `reason` | `string` | yes | min length `5`, max length `2000` |

### Null / invalid behavior

#### `reason`

- `null` -> `400`
- empty / whitespace -> `400`
- length `< 5` -> `400`

#### `reportableTargetId`

- `null` -> `400`
- empty / whitespace -> `400`
- invalid GUID for `User` or `Message` -> `400`
- invalid number for `Property` or `PropertyComment` -> `400`
- numeric value must be positive for numeric target types

### Validation rules by target type

#### `User`

- target ID must be GUID
- user must exist
- user cannot report their own account

#### `Property`

- target ID must be positive number
- property must exist

#### `Message`

- target ID must be GUID
- message must exist
- reporter must be sender or receiver of that message

#### `PropertyComment`

- target ID must be positive number
- comment must exist

### Success response

HTTP:

```http
201 Created
```

Body:

```json
{
  "message": "Report submitted successfully.",
  "data": {
    "reportId": 9101,
    "status": "InReview",
    "createdAt": "2026-05-11T09:30:00Z"
  }
}
```

## Part B - Admin moderation

All endpoints below:
- require authentication
- require role: `Admin`

### 1. Get moderation queue

```http
GET /api/admin/reports?status=InReview&reportableType=Message&search=owner&pageNumber=1&pageSize=20
```

### Query fields

| Field | Type | Required | Default |
|---|---|---:|---:|
| `status` | `ReportStatus?` | no | `null` |
| `reportableType` | `ReportableType?` | no | `null` |
| `search` | `string?` | no | `null` |
| `pageNumber` | `int` | no | `1` |
| `pageSize` | `int` | no | `20` |

### Search behavior

Search looks in:
- report reason
- reporter full name
- reporter email
- reviewer full name
- reviewer email
- numeric target IDs
- GUID target IDs
- target-specific content:
  - reported user full name / email
  - property title / address
  - message participants
  - property-comment content / property title

### Response

`AdminModerationQueueDto`

Contains:
- `reports` -> paged list
- `statusBreakdown`
- `typeBreakdown`

### 2. Get one moderation report

```http
GET /api/admin/reports/{reportId}
```

### Response

`AdminModerationReportDetailsDto`

Main fields:
- report metadata
- `actionTaken`
- `actionsTaken`
- reporter / reviewer info
- `reportableTargetId`
- `target`

### `target` object

`AdminModerationTargetDetailsDto`

Possible useful fields by target type:
- `userId`
- `propertyId`
- `messageId`
- `propertyCommentId`
- `title`
- `subtitle`
- `preview`
- `isHidden`
- `isDeletedOrInactive`
- `exists`

## 3. Review a report

```http
PATCH /api/admin/reports/{reportId}/review
Content-Type: application/json
```

Body:

```json
{
  "status": "Resolved",
  "note": "hide the message and ban the sender",
  "actionTypes": ["HideMessage", "BanUser"]
}
```

### Request fields

| Field | Type | Required | Null behavior |
|---|---|---:|---|
| `status` | `ReportStatus` | yes | must not be `InReview` |
| `note` | `string?` | no | allowed, max length `2000` |
| `actionTypes` | `ReportModerationActionType[]?` | no | `null` means no actions |

### Review rules

- `status = InReview` -> `400`
- `status = Rejected` and actions present -> `400`
- only reports currently in `InReview` can be reviewed

### Multiple actions

Allowed:
- one action
- multiple actions

Duplicates:
- duplicate action values are removed internally

### Compatible actions by target type

#### `User`

Allowed:
- `BanUser`

#### `Property`

Allowed:
- `DeactivateProperty`
- `BanUser` (bans the owner)

#### `Message`

Allowed:
- `HideMessage`
- `BanUser` (bans the sender)

#### `PropertyComment`

Allowed:
- `HidePropertyComment`
- `BanUser` (bans the commenter)

### What actions do

#### `BanUser`

- target user account status becomes `Banned`
- previous status is stored in `StatusBeforeBan` if needed

#### `DeactivateProperty`

- target property `IsActive` becomes `false`

#### `HideMessage`

- message gets moderation flags:
  - `IsHiddenByModeration = true`
  - `HiddenAt`
  - `HiddenByAdminId`
  - `HiddenReason`

#### `HidePropertyComment`

- comment gets moderation flags:
  - `IsHiddenByModeration = true`
  - `HiddenAt`
  - `HiddenByAdminId`
  - `HiddenReason`

### Additional persistence

For applied actions:
- admin action logs are written into `AdminActionLogs`
- `actionTaken` stores the first action for backward compatibility
- `actionsTaken` is the real list to use on the frontend

### Errors

- `404`
  - report not found
- `409`
  - report already reviewed
  - target no longer exists
  - target is no longer in a valid mutable state
- `400`
  - incompatible action for target type
  - rejected report with actions
  - invalid review status

