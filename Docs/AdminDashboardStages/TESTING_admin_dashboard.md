# Admin Dashboard Testing Plan

This guide is for the admin-dashboard seed pack added in `AddAdminDashboardScenarioSeeds`.

It is meant to give you a reliable test surface for:
- dashboard overview cards and graph
- verification queues
- user management
- role management
- moderation reports
- detailed stats pages
- analytics export flow

## 1. Apply the seed migration

Run:

```powershell
dotnet ef database update
```

Recommended: use a fresh or reset dev database if you want the numbers below to match exactly. If your local database already contains extra real data, the scenarios will still exist, but totals may be higher.

## 2. Shared seeded password

All seeded accounts in this project use:

```text
Password123!
```

## 3. Seeded accounts

### Admins

| Purpose | Email |
|---|---|
| Primary admin | `admin@marn.com` |
| Second admin | `assistant.admin@marn.com` |

### Users for admin testing

| Scenario | Email | Expected state |
|---|---|---|
| Pending verification user | `pending.renter@example.com` | `Pending` |
| Banned user | `banned.renter@example.com` | `Banned`, `StatusBeforeBan = Pending` |
| Soft-deleted user | `deleted.renter@example.com` | soft deleted |
| Recent verified user | `recent.renter@example.com` | `Verified`, created this month |
| Moderator candidate | `moderator.user@example.com` | `Verified`, roles = `Renter` + `Moderator` |

### Existing owners reused by the scenario

| Purpose | Email |
|---|---|
| Owner of new dashboard properties | `owner.y@example.com` |
| Existing owner used in moderated message target | `owner.x@example.com` |

## 4. Overview expectations

On a fresh seeded database, the new scenario should make these dashboard areas easy to validate:

- `Total Users`: should include the new pending, banned, recent, and moderator users
- `Total Properties`: should include recent, pending, declined, and moderated-inactive properties
- `Pending Verifications`: should clearly be non-zero because of:
  - `pending.renter@example.com`
  - property `1201` (`Pending Downtown Apartment`)
- `Contracts`: should now include at least one clear `Pending` contract
- `New Users This Month`: should be non-zero because of:
  - `pending.renter@example.com`
  - `recent.renter@example.com`
- `Monthly Revenue Graph`: should show non-zero points across the last 6 months from the seeded revenue contract

### Fresh-db snapshot

If the database contains only migration-based seed data, a good rough expectation is:

| Metric | Expected value |
|---|---|
| Total users | `12` |
| Total properties | `9` |
| Pending verifications | `2` |
| Total contracts | `12` |
| Active contracts | `9` |
| Pending contracts | `1` |
| Cancelled contracts | `1` |
| Expired contracts | `1` |
| New users this month | `2` |

If your numbers are higher, that usually means your dev DB already had additional rows before this migration.

## 5. Verification queue checks

### User verification

Use the admin pending-user-verification screen and confirm this account appears:

- `pending.renter@example.com`

What to check:
- Arabic full name exists
- Arabic address exists
- national ID exists
- front/back ID photos exist
- account status is `Pending`

### Property verification

Use the admin pending-property-verification screen and confirm this property appears:

- Property `1201` - `Pending Downtown Apartment`

What to check:
- owner is `owner.y@example.com`
- proof-of-ownership path is present
- status is `Pending`

## 6. User management checks

### Ban / restore behavior

Open the user-management list and find:

- `banned.renter@example.com`

Validate:
- user is shown as banned
- restore should not incorrectly make them `Verified`
- because `StatusBeforeBan = Pending`, restoring should return them to `Pending`

### Soft delete visibility

Test include-deleted behavior with:

- `deleted.renter@example.com`

Validate:
- this user is hidden unless deleted users are included
- details still work in admin flows that intentionally include deleted records

### Recent user details

Open:

- `recent.renter@example.com`

Validate:
- created-this-month signal is reflected in stats
- user details page works with a normal verified account

## 7. Role management checks

Open the role-management views and verify:

- role `Moderator` exists
- `moderator.user@example.com` has:
  - `Renter`
  - `Moderator`
- there are two admin users:
  - `admin@marn.com`
  - `assistant.admin@marn.com`

Recommended role-management tests:

1. remove `Admin` from one admin user and confirm it is allowed while another admin still exists
2. try to remove `Admin` from the last remaining admin and confirm it is blocked
3. assign and remove `Moderator` from a non-admin user
4. confirm `Owner` and `Renter` are preserved as base roles

## 8. Property stats and admin property actions

Use the detailed properties stats page and confirm these records exist:

| Property ID | Scenario |
|---|---|
| `1201` | pending verification |
| `1202` | declined verification |
| `1203` | soft deleted |
| `1204` | active recent verified property |
| `1205` | inactive verified property, already moderated |

Recommended checks:

1. restore property `1205`
2. deactivate property `1204`
3. include deleted properties and verify `1203` appears
4. filter by `Pending` and verify `1201`
5. filter by `Declined` and verify `1202`

## 9. Contract stats and admin contract actions

Use the detailed contracts stats page and verify:

| Contract ID | Scenario |
|---|---|
| `1000101` | pending contract |
| `1000102` | active contract feeding revenue graph |

Recommended checks:

1. search contract `1000101`
2. confirm it shows `Pending`
3. cancel contract `1000101`
4. verify it becomes `Cancelled`
5. download a contract PDF directly from the admin endpoint:

```http
GET /api/admin/stats/contracts/{contractId}/download
```

Note: `1000101` is a good safe admin-cancel test because it does not depend on live Stripe payment intents.

## 10. Revenue stats checks

The revenue graph/data seed comes from contract `1000102`.

Seeded paid months:

- December 2025
- January 2026
- February 2026
- March 2026
- April 2026
- May 2026

Each seeded payment is:

- sales: `6000`
- platform fee: `600`
- owner payout: `5400`

Recommended checks:

1. open dashboard overview and confirm the monthly graph is populated
2. open revenue detailed stats
3. filter `ThisYear`
4. verify revenue-over-time includes Jan-May 2026
5. search by contract `1000102`

## 11. Moderation reports checks

### Seeded reports

| Report ID | Type | Status | Purpose |
|---|---|---|---|
| `1` | Property | `InReview` | existing placeholder report |
| `9101` | User | `InReview` | active review queue example |
| `9102` | Property | `Resolved` | deactivated property example |
| `9103` | Message | `Resolved` | multi-action example: hide message + ban user |
| `9104` | PropertyComment | `Resolved` | multi-action example: hide comment + ban user |
| `9105` | User | `Rejected` | rejected review example |

### Moderated targets

| Target | Expected result |
|---|---|
| Message `00000000-0000-0000-0000-000000000101` | hidden |
| Comment `900101` | hidden |
| Property `1205` | inactive |
| User `banned.renter@example.com` | banned |

Recommended checks:

1. open report `9103`
2. confirm `ActionsTaken` contains:
   - `HideMessage`
   - `BanUser`
3. open report `9104`
4. confirm `ActionsTaken` contains:
   - `HidePropertyComment`
   - `BanUser`
5. open report `9102`
6. confirm target property is already inactive
7. open report `9105`
8. confirm status is `Rejected` and no moderation action was applied

## 12. Analytics export checks

No export files are pre-seeded on disk. That is intentional.

Use the analytics-reports endpoints to generate real files after applying the migration.

Recommended flow:

1. generate an `Overview` PDF
2. generate a `Revenue` CSV
3. open reports history
4. verify both records appear
5. download both files

This is better than seeding fake export rows because the download endpoint depends on real stored files.

## 13. Suggested smoke-test order

If you want the fastest full admin pass, do it in this order:

1. dashboard overview
2. pending user verification
3. pending property verification
4. user-management list
5. role-management list
6. property detailed stats
7. contract detailed stats
8. revenue detailed stats
9. moderation reports queue
10. analytics export generation

## 14. Files added for this scenario

Main seed files:

- `Data/Seed/AdminDashboardScenarioIds.cs`
- `Data/Seed/AdminDashboardScenarioIdentitySeed.cs`
- `Data/Seed/AdminDashboardScenarioPropertySeed.cs`
- `Data/Seed/AdminDashboardScenarioContractSeed.cs`
- `Data/Seed/AdminDashboardScenarioModerationSeed.cs`

Migration:

- `Migrations/20260516073752_AddAdminDashboardScenarioSeeds.cs`
