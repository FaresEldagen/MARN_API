# Stage 4 - Detailed Stats and Admin Actions APIs

## Stage purpose

This stage powers the drilldown pages behind the main dashboard cards:
- detailed users stats
- detailed properties stats
- detailed contracts stats
- detailed revenue stats
- admin property activate/deactivate actions
- admin contract cancel action
- admin contract PDF download

## Common auth

All endpoints in this stage:
- require authentication
- require role: `Admin`

## Common period query rules

These endpoints use the same base period DTO:
- `GET /api/admin/stats/users`
- `GET /api/admin/stats/properties`
- `GET /api/admin/stats/contracts`
- `GET /api/admin/stats/revenue`

### Period fields

| Field | Type | Required | Default |
|---|---|---:|---:|
| `period` | `string` | no | `"allTime"` |
| `fromUtc` | `datetime?` | no | `null` |
| `toUtc` | `datetime?` | no | `null` |
| `pageNumber` | `int` | no | `1` |
| `pageSize` | `int` | no | `20` |

### Supported `period` values

- `allTime`
- `thisMonth`
- `thisYear`
- `custom`

### Period behavior

#### `allTime`

- `fromUtc` ignored
- `toUtc` ignored

#### `thisMonth`

- backend computes the current UTC month start
- `fromUtc` and `toUtc` from the request are ignored
- user and revenue charts use day grouping

#### `thisYear`

- backend computes January 1st of the current UTC year
- `fromUtc` and `toUtc` from the request are ignored
- grouping is month

#### `custom`

Required:
- `fromUtc`
- `toUtc`

Validation:
- both must be present
- `fromUtc < toUtc`

Grouping:
- if duration is `<= 31` days, grouping becomes `day`
- otherwise grouping becomes `month`

### Paging behavior

- `pageNumber < 1` becomes `1`
- `pageSize < 1` becomes `20`
- `pageSize > 100` becomes `100`

## 1. Detailed users stats

```http
GET /api/admin/stats/users?period=thisMonth&search=ali&accountStatus=Verified&role=Renter&includeDeleted=true
```

### Extra query fields

| Field | Type | Notes |
|---|---|---|
| `search` | `string?` | full name + email |
| `accountStatus` | `AccountStatus?` | exact match |
| `role` | `string?` | normalized role name filter |
| `includeDeleted` | `bool` | default `true` |

### Search behavior

Looks in:
- full name
- email

### Response sections

- `appliedPeriod`
- `totalUsers`
- `deletedUsers`
- `statusBreakdown`
- `roleBreakdown`
- `newUsersOverTime`
- `users` (`PagedResult<AdminDetailedUserListItemDto>`)

### `AdminDetailedUserListItemDto` notable fields

Each user row now includes:
- identity fields:
  - `userId`
  - `fullName`
  - `email`
  - `profileImage`
  - `accountStatus`
  - `isDeleted`
  - `createdAt`
  - `roles`
- activity / finance fields:
  - `ownedPropertiesCount`
  - `activePropertiesCount`
  - `renterContractsCount`
  - `ownerContractsCount`
  - `activeContractsCount`
  - `cancelledContractsCount`
  - `paymentsMadeCount`
  - `paymentsReceivedCount`
  - `totalPaidAmount`
  - `totalReceivedAmount`
  - `reportsSubmittedCount`
  - `reportsAgainstUserCount`

## 2. Detailed properties stats

```http
GET /api/admin/stats/properties?period=allTime&status=Pending&type=Apartment&governorate=Cairo%20Governorate&isActive=true&includeDeleted=true
```

### Extra query fields

| Field | Type | Notes |
|---|---|---|
| `search` | `string?` | title + address + owner full name |
| `status` | `PropertyStatus?` | exact match |
| `type` | `PropertyType?` | exact match |
| `governorate` | `string?` | compares to `State`, case-insensitive |
| `isActive` | `bool?` | exact active filter |
| `includeDeleted` | `bool` | default `true` |

### Search behavior

Looks in:
- property title
- property address
- owner full name

### Response sections

- `appliedPeriod`
- `totalProperties`
- `deletedProperties`
- `activeProperties`
- `inactiveProperties`
- `statusBreakdown`
- `typeBreakdown`
- `governorateBreakdown`
- `properties` (`PagedResult<AdminDetailedPropertyListItemDto>`)

### Property action fields in each row

| Field | Meaning |
|---|---|
| `canDeactivate` | true only when property is active and not deleted |
| `canRestore` | true only when property is inactive and not deleted |
| `isDeleted` | whether `DeletedAt` exists |

## 3. Deactivate property

```http
PATCH /api/admin/stats/properties/{propertyId}/deactivate
```

### Request body

None.

### What happens

- `IsActive` becomes `false`
- verification `Status` is unchanged
- owner receives a notification

### Errors

- `404`
  - property not found
- `409`
  - property already inactive
  - property is soft deleted

## 4. Restore property

```http
PATCH /api/admin/stats/properties/{propertyId}/restore
```

### What happens

- `IsActive` becomes `true`
- verification `Status` is unchanged
- owner receives a notification

### Errors

- `404`
- `409`
  - property already active
  - property is soft deleted

## 5. Detailed contracts stats

```http
GET /api/admin/stats/contracts?period=allTime&search=1000101&status=Pending
```

### Extra query fields

| Field | Type | Notes |
|---|---|---|
| `search` | `string?` | contract ID + property title + owner full name + renter full name |
| `status` | `ContractStatus?` | exact match |

### ContractStatus enum values

- `Pending = 0`
- `Active = 1`
- `Cancelled = 2`
- `Expired = 3`

### Search behavior

Looks in:
- contract ID
- property title
- owner full name
- renter full name

### Response sections

- `appliedPeriod`
- `totalContracts`
- `totalContractValue`
- `statusBreakdown`
- `contracts` (`PagedResult<AdminDetailedContractListItemDto>`)

### Important row fields

| Field | Meaning |
|---|---|
| `canCancel` | true only for `Pending` or `Active` contracts |
| `paymentFrequency` | returned as string, not enum number |

## 6. Download contract PDF

```http
GET /api/admin/stats/contracts/{contractId}/download
```

### Response

- raw file response
- content type: `application/pdf`

### Errors

- `404`
  - contract not found
  - contract exists but no stored PDF bytes exist

## 7. Cancel contract

```http
PATCH /api/admin/stats/contracts/{contractId}/cancel
```

### Request body

None.

### What happens

- allowed only for `Pending` or `Active` contracts
- status becomes `Cancelled`
- unpaid schedules become `Cancelled`
- unpaid schedule `PaymentIntentId` values are cleared
- renter and owner receive notifications

### Stripe behavior

Before cancel completes:
- the service attempts to cancel issued unpaid Stripe payment intents

If a live intent:
- already succeeded
- or Stripe refuses cancellation

then contract cancellation is blocked with `409 Conflict`

## 8. Detailed revenue stats

```http
GET /api/admin/stats/revenue?period=thisYear&search=1000102&status=Available
```

### Extra query fields

| Field | Type | Notes |
|---|---|---|
| `search` | `string?` | payment ID + contract ID + property title + owner full name + renter full name |
| `status` | `PaymentStatus?` | exact match |

### PaymentStatus enum values

- `OnHold = 0`
- `Available = 1`
- `Withdrawn = 2`

### Search behavior

Looks in:
- payment ID
- contract ID
- property title
- owner full name
- renter full name

### Response sections

- `appliedPeriod`
- `totalPayments`
- `totalSales`
- `totalRevenue`
- `totalOwnerPayouts`
- `statusBreakdown`
- `revenueOverTime`
- `payments` (`PagedResult<AdminDetailedPaymentListItemDto>`)

## Common error scenarios for the stats endpoints

### `400 Bad Request`

Returned when:
- `period` is invalid
- `period=custom` but `fromUtc` or `toUtc` is missing
- `fromUtc >= toUtc`

### `401 Unauthorized`

Returned when:
- request has no valid token

### `403 Forbidden`

Returned when:
- authenticated user is not an admin
