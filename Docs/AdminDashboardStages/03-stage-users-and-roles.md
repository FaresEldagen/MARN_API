# Stage 3 - User Management and Role Management APIs

## Stage purpose

This stage covers:
- user list and user details
- ban / unban / restore soft delete / soft delete
- role list
- role-management user list
- role updates, including `Admin` and future roles like `Moderator`

## Common auth

All endpoints in this stage:
- require authentication
- require role: `Admin`

## Common paging behavior

For list endpoints:
- `pageNumber < 1` becomes `1`
- `pageSize < 1` becomes `20`
- `pageSize > 100` becomes `100`

## Part A - User Management

### 1. Get users

```http
GET /api/admin/users?pageNumber=1&pageSize=20&search=ali&accountStatus=Verified&role=Renter&includeDeleted=true
```

### Query fields

| Field | Type | Required | Default | Notes |
|---|---|---:|---:|---|
| `pageNumber` | `int` | no | `1` | normalized |
| `pageSize` | `int` | no | `20` | clamped to `100` |
| `search` | `string?` | no | `null` | trims whitespace |
| `accountStatus` | `AccountStatus?` | no | `null` | exact enum filter |
| `role` | `string?` | no | `null` | compared to normalized identity role name |
| `includeDeleted` | `bool` | no | `true` | if `false`, soft-deleted users are filtered out |

### Search behavior

Search looks in:
- full name
- email
- phone number

### Important behavior

- admin users are excluded from this stage completely
- use role-management stage for admin accounts

### Response item

`AdminUserListItemDto`

| Field | Type |
|---|---|
| `userId` | `guid` |
| `fullName` | `string` |
| `email` | `string?` |
| `phoneNumber` | `string?` |
| `profileImage` | `string?` |
| `accountStatus` | `AccountStatus` |
| `isDeleted` | `bool` |
| `createdAt` | `datetime` |
| `roles` | `string[]` |
| `ownedPropertiesCount` | `int` |
| `activeContractsCount` | `int` |

### 2. Get one user

```http
GET /api/admin/users/{userId}
```

### Response

`AdminUserDetailsDto`

Main top-level fields:
- identity and profile fields
- KYC fields
- `roles`
- `summary`
- `ownedProperties`
- `renterContracts`
- `ownerContracts`
- `paidPayments`
- `receivedPayments`

### Detail notes

- `ownedProperties` includes soft-deleted properties
- `renterContracts` and `ownerContracts` are history lists, not only active items
- `paidPayments` are payments made as renter
- `receivedPayments` are payments received as owner

### 3. Ban user

```http
PATCH /api/admin/users/{userId}/ban
```

### What happens

- only non-admin users are allowed
- deleted users cannot be banned
- already-banned users return conflict
- `StatusBeforeBan` stores the prior status
- `AccountStatus` becomes `Banned`

### Errors

- `403`
  - target user is an admin user
- `404`
  - user not found
- `409`
  - user already banned
  - user already deleted

### 4. Unban user

```http
PATCH /api/admin/users/{userId}/unban
```

### What happens

- only banned non-admin users are allowed
- user is restored to `StatusBeforeBan`
- if `StatusBeforeBan` is missing, fallback is `Unverified`

### Important note

This does **not** force users back to `Verified`.

### Errors

- `403`
  - target user is an admin user
- `404`
  - user not found
- `409`
  - user is not banned
  - user is deleted

### 5. Restore deleted user

```http
PATCH /api/admin/users/{userId}/restore
```

### What happens

- only deleted non-admin users are allowed
- `DeletedAt` is cleared
- if the delayed Hangfire image deletion job is still pending, it is cancelled
- if more than 7 days passed and verification images were already removed, the user is restored as `Unverified`
- if the deleted user is still banned and `StatusBeforeBan` was `Verified`, `StatusBeforeBan` is downgraded to `Unverified`

### Errors

- `403`
  - target user is an admin user
- `404`
  - user not found
- `409`
  - user is not deleted

### 6. Soft delete user

```http
DELETE /api/admin/users/{userId}
```

### What happens

- only non-admin users are allowed
- uses the existing profile deletion workflow
- this is a soft delete, not a hard delete

### Errors

- `403`
  - target user is an admin user
- `404`
  - user not found
- `409`
  - user already deleted

## Part B - Role Management

### 1. Get roles

```http
GET /api/admin/roles
```

### Response item

`AdminRoleDefinitionDto`

| Field | Type | Meaning |
|---|---|---|
| `roleName` | `string` | display/original role name |
| `normalizedName` | `string` | uppercase identity role name |
| `usersCount` | `int` | users currently assigned |
| `isProtected` | `bool` | true for base roles like `Owner` and `Renter` |
| `isAssignable` | `bool` | false for protected base roles |

### Important behavior

- roles are read dynamically from identity
- future roles like `Moderator` appear automatically
- `Owner` and `Renter` are protected
- `Admin` is assignable in this stage

### 2. Get role-management users

```http
GET /api/admin/roles/users?pageNumber=1&pageSize=20&search=admin&role=Admin&accountStatus=Verified&includeDeleted=true
```

### Query fields

Same structure as user management, but:
- search looks in:
  - full name
  - email
  - user name
- admin users are included here

### Response item

`AdminRoleUserListItemDto`

| Field | Type |
|---|---|
| `userId` | `guid` |
| `fullName` | `string` |
| `email` | `string?` |
| `profileImage` | `string?` |
| `accountStatus` | `AccountStatus` |
| `isDeleted` | `bool` |
| `createdAt` | `datetime` |
| `roles` | `string[]` |

### 3. Get one role-management user

```http
GET /api/admin/roles/users/{userId}
```

### Response

`AdminRoleUserDetailsDto`

Includes:
- current user info
- current assigned roles
- `availableRoles`

### 4. Update user roles

```http
PATCH /api/admin/roles/users/{userId}
Content-Type: application/json
```

Body:

```json
{
  "roles": ["Admin", "Moderator"]
}
```

### Request fields

| Field | Type | Required | Null behavior |
|---|---|---:|---|
| `roles` | `string[]` | no | `null` is treated like empty list |

### Role-update behavior

- whitespace-only values are ignored
- duplicate values are ignored
- `Owner` and `Renter` sent in the body are not treated as assignable managed roles
- current base roles (`Owner`, `Renter`) are preserved automatically
- managed roles are effectively replaced by the new requested set

### Protected behavior

- `Owner` / `Renter` are not removed by this endpoint
- removing the last remaining `Admin` is blocked
- deleted users cannot be edited

### Errors

- `404`
  - user not found
- `409`
  - deleted user
  - attempt to remove the last remaining admin
- `400`
  - unknown role
  - protected role passed as if it were assignable
  - resulting role set would be invalid

