# Stage 2 - Verification Review APIs

## Stage purpose

This stage handles:
- pending user legal-document review
- pending property ownership-document review
- approve / decline actions

## Common auth

All endpoints in this stage:
- require authentication
- require role: `Admin`

## Common response wrappers

Success:

```json
{
  "message": "optional success message",
  "data": {}
}
```

Error:

```json
{
  "message": "human readable message",
  "statusCode": 400,
  "path": "/api/admin/...",
  "traceId": "...",
  "timestamp": "2026-05-16T08:00:00Z",
  "errors": {
    "general": [
      "optional validation details"
    ]
  }
}
```

## Shared query DTO

Used by:
- `GET /api/admin/verifications/users/pending`
- `GET /api/admin/verifications/properties/pending`

Query fields:

| Field | Type | Required | Default | Behavior |
|---|---|---:|---:|---|
| `pageNumber` | `int` | no | `1` | values `< 1` become `1` |
| `pageSize` | `int` | no | `20` | values `< 1` become `20`, values `> 100` become `100` |

## 1. Get pending user verifications

```http
GET /api/admin/verifications/users/pending?pageNumber=1&pageSize=20
```

### What it returns

Paged list of `AdminUserVerificationDto`.

Item fields:

| Field | Type |
|---|---|
| `userId` | `guid` |
| `fullName` | `string` |
| `email` | `string?` |
| `phoneNumber` | `string?` |
| `profileImage` | `string?` |
| `createdAt` | `datetime` |
| `accountStatus` | `AccountStatus enum` |
| `frontIdPhoto` | `string?` |
| `backIdPhoto` | `string?` |
| `arabicFullName` | `string?` |
| `arabicAddress` | `string?` |
| `nationalIDNumber` | `string?` |

### AccountStatus enum values

- `Unverified = 0`
- `Pending = 1`
- `Verified = 2`
- `Declined = 3`
- `Banned = 4`

### Behavior

- only users currently in `Pending` status are returned
- soft-deleted users are not expected in this queue

## 2. Get one user verification

```http
GET /api/admin/verifications/users/{userId}
```

### Success

Returns one `AdminUserVerificationDto`.

### Errors

- `404 Not Found`
  - if the user is not found
  - if the user is not considered a valid verification request target

## 3. Approve user verification

```http
PATCH /api/admin/verifications/users/{userId}/approve
```

### Request body

None.

### Success

```json
{
  "message": "User verification approved.",
  "data": true
}
```

### What happens

- user `AccountStatus` changes from `Pending` to `Verified`

### Errors

- `404`
  - verification target not found
- `409`
  - if current status is not `Pending`

## 4. Decline user verification

```http
PATCH /api/admin/verifications/users/{userId}/decline
Content-Type: application/json
```

Body:

```json
{
  "reason": "document image is unreadable"
}
```

### Request fields

| Field | Type | Required | Null allowed | Notes |
|---|---|---:|---:|---|
| `reason` | `string` | no | yes | max length `1000` |

### Null / empty behavior

- if body is missing, the service creates an empty DTO internally
- if `reason` is `null`, the request is still accepted
- reason is currently only logged; it is not persisted in a dedicated audit table

### What happens

- user `AccountStatus` changes from `Pending` to `Declined`

### Errors

- `404`
  - verification target not found
- `409`
  - if current status is not `Pending`

## 5. Get pending property verifications

```http
GET /api/admin/verifications/properties/pending?pageNumber=1&pageSize=20
```

### What it returns

Paged list of `AdminPropertyVerificationDto`.

Item fields:

| Field | Type |
|---|---|
| `propertyId` | `long` |
| `title` | `string` |
| `description` | `string` |
| `type` | `PropertyType enum` |
| `status` | `PropertyStatus enum` |
| `isActive` | `bool` |
| `createdAt` | `datetime` |
| `ownerId` | `guid` |
| `ownerFullName` | `string` |
| `ownerEmail` | `string?` |
| `ownerAccountStatus` | `AccountStatus enum` |
| `proofOfOwnership` | `string` |
| `primaryImage` | `string?` |
| `price` | `decimal` |
| `rentalUnit` | `RentalUnit enum` |
| `address` | `string` |
| `city` | `string` |
| `state` | `string` |
| `zipCode` | `string` |
| `latitude` | `double` |
| `longitude` | `double` |

### PropertyType enum values

- `Apartment = 0`
- `House = 1`
- `Room = 2`
- `Villa = 3`
- `Studio = 4`
- `SharedRoom = 5`

### PropertyStatus enum values

- `Pending = 0`
- `Verified = 1`
- `Declined = 2`

### RentalUnit enum values

- `Daily = 0`
- `Monthly = 1`
- `Yearly = 2`

## 6. Get one property verification

```http
GET /api/admin/verifications/properties/{propertyId}
```

### Success

Returns one `AdminPropertyVerificationDto`.

### Errors

- `404 Not Found`

## 7. Approve property verification

```http
PATCH /api/admin/verifications/properties/{propertyId}/approve
```

### Success

```json
{
  "message": "Property verification approved.",
  "data": true
}
```

### What happens

- property `Status` changes from `Pending` to `Verified`

### Errors

- `404`
- `409`
  - if current property status is not `Pending`

## 8. Decline property verification

```http
PATCH /api/admin/verifications/properties/{propertyId}/decline
Content-Type: application/json
```

Body:

```json
{
  "reason": "ownership proof does not match the listing"
}
```

### Request behavior

Same DTO rules as user decline:
- body may be omitted
- `reason` may be `null`
- `reason` max length is `1000`
- reason is only logged in the current implementation

### What happens

- property `Status` changes from `Pending` to `Declined`

### Errors

- `404`
- `409`
  - if current property status is not `Pending`

