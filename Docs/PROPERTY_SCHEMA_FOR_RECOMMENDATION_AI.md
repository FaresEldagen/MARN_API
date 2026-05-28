# Property Schema For Recommendation AI

This file describes the **actual current code** in MARN API for the property entity/table.

It answers:

- exact table name
- exact property columns in code
- whether latitude/longitude exist
- whether `area` is `SquareMeters`
- whether a furnished column exists
- which fields define whether a property is public/usable

## 1. Exact Table Name

The EF Core table name is:

```text
Properties
```

This is confirmed in the generated EF model snapshot:

- `b.ToTable("Properties");`

## 2. Actual Property Entity

The C# entity is `Models/Property.cs`.

Current fields:

| Column / Property | C# Type | Notes |
|---|---|---|
| `Id` | `long` | Primary key |
| `Title` | `string` | Required |
| `Description` | `string` | Required |
| `Type` | `PropertyType` | Enum |
| `ProofOfOwnership` | `string` | Required in model |
| `MaxOccupants` | `int` | Required |
| `IsShared` | `bool` | Required |
| `Bedrooms` | `int` | Present |
| `Beds` | `int` | Present |
| `Bathrooms` | `int` | Present |
| `SquareMeters` | `double` | This is the area field |
| `Views` | `int` | View counter |
| `Price` | `decimal` | Stored as `decimal(18,2)` |
| `RentalUnit` | `RentalUnit` | Enum |
| `Address` | `string` | Required |
| `City` | `string` | Present |
| `State` | `string` | This is effectively the governorate / region field |
| `ZipCode` | `string` | Present |
| `Latitude` | `double` | Present |
| `Longitude` | `double` | Present |
| `IsActive` | `bool` | Present |
| `Status` | `PropertyStatus` | Enum |
| `CreatedAt` | `DateTime` | UTC default |
| `DeletedAt` | `DateTime?` | Soft delete marker |
| `ImagesDeletionJob` | `string?` | Hangfire cleanup job id |
| `OwnerId` | `Guid` | FK to owner/user |

## 3. Direct Answers To Your Friend's Questions

### What is the exact table name?

```text
Properties
```

### Does it have lat/lng?

Yes.

Actual column/property names are:

- `Latitude`
- `Longitude`

Types:

- `Latitude: double`
- `Longitude: double`

### Is `area` called `SquareMeters`?

Yes.

The actual property name is:

- `SquareMeters`

Type:

- `double`

### Is there a furnished column?

No.

There is **no** current dedicated property column like:

- `Furnished`
- `IsFurnished`
- `FurnishingStatus`

So if your friend's current CSV has a `furnished` feature, that field does **not** have a direct matching column in the current MARN `Properties` table.

### Is there a column to filter only public/active properties?

Yes, and the code uses **three conditions together** for public recommendation/search use:

1. `IsActive == true`
2. `Status == Verified`
3. `DeletedAt == null`

In practice:

- `IsActive` controls active/inactive listing visibility
- `Status` controls verification state
- `DeletedAt` is the soft-delete flag

## 4. Enum Details

### `Type` (`PropertyType`)

Current enum values:

| Name | Stored enum value |
|---|---:|
| `Apartment` | 0 |
| `House` | 1 |
| `Room` | 2 |
| `Villa` | 3 |
| `Studio` | 4 |
| `SharedRoom` | 5 |

In EF configuration, `Type` is explicitly stored as an integer in the database.

### `Status` (`PropertyStatus`)

Current enum values:

| Name | Stored enum value |
|---|---:|
| `Pending` | 0 |
| `Verified` | 1 |
| `Declined` | 2 |

In EF configuration, `Status` is explicitly stored as an integer in the database.

### `RentalUnit` (`RentalUnit`)

Current enum values:

| Name | Stored enum value |
|---|---:|
| `Daily` | 0 |
| `Monthly` | 1 |
| `Yearly` | 2 |

## 5. Important EF / Database Notes

From `Data/Configurations/PropertyConfiguration.cs`:

- table has a global query filter:
  - `DeletedAt == null`
- `Price` is stored as:
  - `decimal(18,2)`
- required fields enforced in configuration:
  - `Title`
  - `Description`
  - `Address`
  - `OwnerId`
  - `Type`
  - `Price`
  - `MaxOccupants`
  - `Latitude`
  - `Longitude`
  - `IsShared`
- indexes exist on:
  - `OwnerId`
  - `Status`

## 6. Public / Recommendation-Safe Property Filter

For recommendation and search logic, the code currently treats a property as usable/public only if:

```text
IsActive == true
AND Status == Verified
AND DeletedAt == null
```

That is the same rule used in the property search/recommendation repository code.

## 7. Mapping From Friend CSV Columns To Current MARN Columns

Your friend's current CSV columns:

```text
price, area, bedrooms, bathrooms, lat, lng, city, region, type, furnished, title
```

Current mapping:

| Friend CSV Column | Current MARN Property Column | Status |
|---|---|---|
| `price` | `Price` | Direct match |
| `area` | `SquareMeters` | Direct match |
| `bedrooms` | `Bedrooms` | Direct match |
| `bathrooms` | `Bathrooms` | Direct match |
| `lat` | `Latitude` | Direct match |
| `lng` | `Longitude` | Direct match |
| `city` | `City` | Direct match |
| `region` | `State` | Closest/current match |
| `type` | `Type` | Direct match as enum |
| `furnished` | No current column | Not available in current schema |
| `title` | `Title` | Direct match |

## 8. Recommended Minimal Property Export Shape

If your friend wants the closest equivalent to his current CSV, the current MARN property data shape would be:

```json
{
  "id": 1001,
  "title": "Modern Apartment in Maadi",
  "price": 6500.00,
  "squareMeters": 120.0,
  "bedrooms": 3,
  "bathrooms": 2,
  "latitude": 30.0444,
  "longitude": 31.2357,
  "city": "Cairo",
  "state": "Cairo",
  "type": "Apartment",
  "isShared": false,
  "maxOccupants": 4,
  "rentalUnit": "Monthly",
  "isActive": true,
  "status": "Verified",
  "deletedAt": null
}
```

## 9. Bottom Line

Exact answers:

- Table name: `Properties`
- Area column: `SquareMeters`
- Latitude/longitude columns: `Latitude`, `Longitude`
- Region-like column: `State`
- Furnished column: **does not exist**
- Public property filtering fields: `IsActive`, `Status`, `DeletedAt`
- Recommendation-safe/public property rule:
  - `IsActive = true`
  - `Status = Verified`
  - `DeletedAt = null`
