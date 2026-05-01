# Property Feedback Refactor

## Overview

This project originally used a single legacy `Review` model that mixed rating and comment behavior in one table.

The feedback system has now been refactored into two separate models:

- `PropertyRating`
- `PropertyComment`

The property module has also been rewired so property-facing data reads from these new tables instead of the old `Reviews` table.

## Why the Refactor Happened

The old `Review` design combined two different concepts:

- a numeric rating
- a written comment

That made the feature less flexible. The new design allows:

- rating without commenting
- commenting without rating
- one rating per user per property
- multiple comments per user per property

This matches the intended business rules more cleanly.

## New Data Model

### `PropertyRating`

Stores a user's rating for a property.

Main fields:

- `Id`
- `PropertyId`
- `UserId`
- `Rating`
- `CreatedAt`
- `UpdatedAt`

Rules:

- rating range is `1..5`
- one rating per user per property

### `PropertyComment`

Stores a user's written comment for a property.

Main fields:

- `Id`
- `PropertyId`
- `UserId`
- `Content`
- `CreatedAt`
- `UpdatedAt`

Rules:

- comments are flat, not threaded
- users can create more than one comment on the same property
- whitespace-only comments are rejected

## Business Rules

To create, update, or delete a rating/comment:

- the property must exist
- the user must be authenticated
- the user must have a contract for that property
- only `Active` or `Expired` contracts qualify

Ownership rules:

- only the owner of a rating can update or delete that rating
- only the owner of a comment can update or delete that comment

## Runtime Wiring

## Property-facing logic now uses the new models

The property module no longer depends on the legacy `Review` entity at runtime.

### Ratings now come from `PropertyRatings`

Used in:

- property cards
- saved properties
- owner dashboard/property profile stats
- search filtering by minimum rating
- search sorting by rating
- property details average/count/current-user rating

### Comments now come from `PropertyComments`

Used in:

- property comments endpoint
- property details comments list
- property details comments count

### Property details response

The old review-style fields were removed from the active contract and replaced with split feedback fields.

Current property details feedback fields include:

- `AverageRating`
- `RatingsCount`
- `CommentsCount`
- `CurrentUserRating`
- `Comments`

Each property detail comment item can also include the commenter's rating for that property when one exists.

## API Surface

### Ratings

- `GET /api/properties/{propertyId}/ratings/summary`
- `POST /api/properties/{propertyId}/ratings`
- `PUT /api/properties/{propertyId}/ratings/me`
- `DELETE /api/properties/{propertyId}/ratings/me`

### Comments

- `GET /api/properties/{propertyId}/comments`
- `POST /api/properties/{propertyId}/comments`
- `PUT /api/properties/{propertyId}/comments/{commentId}`
- `DELETE /api/properties/{propertyId}/comments/{commentId}`

All feedback endpoints currently require authentication.

## DTO Contract Cleanup

The feedback mutation endpoints no longer return EF entities directly.

They now return dedicated DTOs:

- `PropertyRatingDto`
- `PropertyCommentMutationDto`

This keeps the public API contract separate from persistence models.

## Configurations and Seeds

The new feedback system is fully wired into EF Core:

- `PropertyRatingConfiguration`
- `PropertyCommentConfiguration`
- `PropertyRatingSeed`
- `PropertyCommentSeed`

The old `Review` configuration and seed were removed from runtime wiring.

## Migrations

### Legacy review removal

`RemoveLegacyReviews` drops the old `Reviews` table.

### New feedback seeds

`SeedPropertyRatingsAndComments` inserts demo seed data for:

- `PropertyRatings`
- `PropertyComments`

The seed IDs were moved to reserved values (`900001+`) to avoid clashing with earlier inserted rows.

## Deletion Behavior

Soft-deleting a property now also removes its:

- booking requests
- property comments
- property ratings
- property media

This prevents feedback rows from being left behind when a property is deleted.

## Removed Legacy Runtime Pieces

The following legacy review pieces were removed from active runtime usage:

- `Review` entity
- `Review` navigation collections on `Property` and `ApplicationUser`
- review repository interface/implementation
- review config/seed registration
- review DI registration

## Intentional Leftovers

Some remaining uses of the word `Review` are not part of the old feedback model and are intentional:

- notification text such as `Property Submitted for Review`
- `ReportableTypeEnum.Review`
- historical migration files and designer snapshots

These do not mean the runtime still uses the old `Review` entity.

## Final Audit Result

The active runtime flow is consistent with the split feedback design:

- property logic reads ratings from `PropertyRatings`
- property logic reads comments from `PropertyComments`
- legacy `Review` runtime dependencies were removed
- EF configuration and seeding were migrated to the new models
- property deletion cleans up feedback rows
- feedback endpoints use DTOs instead of EF entities

## Files Most Relevant to the Refactor

- `Data/AppDbContext.cs`
- `Models/Property.cs`
- `Models/ApplicationUser.cs`
- `Data/Configurations/PropertyRatingConfiguration.cs`
- `Data/Configurations/PropertyCommentConfiguration.cs`
- `Data/Seed/PropertyRatingSeed.cs`
- `Data/Seed/PropertyCommentSeed.cs`
- `Controllers/PropertyFeedbackController.cs`
- `Services/Implementations/PropertyRatingService.cs`
- `Services/Implementations/PropertyCommentService.cs`
- `Services/Implementations/PropertyService.cs`
- `Repositories/Implementations/PropertyRepo.cs`
- `Repositories/Implementations/SavedPropertyRepo.cs`
- `DTOs/Property/PropertyDetailsDto.cs`
- `Migrations/20260429194054_RemoveLegacyReviews.cs`
- `Migrations/20260429210051_SeedPropertyRatingsAndComments.cs`
