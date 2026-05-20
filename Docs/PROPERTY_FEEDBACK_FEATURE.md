# Property Ratings and Comments Feature

## Overview

This feature adds standalone property feedback support through two separate concepts:

- `PropertyRating`
- `PropertyComment`

They are intentionally independent:

- a user can rate without commenting
- a user can comment without rating
- a user can do both

The feature was implemented without depending on future property module logic beyond validating that a property exists and that the user has an eligible contract for that property.

## Business Rules

### Ratings

- Each user can have only one rating per property.
- Rating value must be between `1` and `5`.
- A user can create, update, and delete only their own rating.
- A user can rate a property only if they have a contract for that property with status:
  - `Active`
  - `Expired`

The following statuses do not qualify:

- `Pending`
- `Cancelled`

### Comments

- Comments are flat only in v1.
- Replies are not supported.
- A user can create multiple comments on the same property.
- A user can update and delete only their own comments.
- A user can comment on a property only if they have a contract for that property with status:
  - `Active`
  - `Expired`

## API Endpoints

Base route:

`/api/properties/{propertyId}`

### Ratings

#### `GET /ratings/summary`

Returns:

- `averageRating`
- `ratingsCount`
- `currentUserRating` if the caller is authenticated and already rated the property

Success response shape:

```json
{
  "message": null,
  "data": {
    "averageRating": 4.5,
    "ratingsCount": 2,
    "currentUserRating": 5
  }
}
```

#### `POST /ratings`

Creates the current user's rating.

Request body:

```json
{
  "rating": 5
}
```

#### `PUT /ratings/me`

Updates the current user's rating.

Request body:

```json
{
  "rating": 4
}
```

#### `DELETE /ratings/me`

Deletes the current user's rating.

### Comments

#### `GET /comments?pageNumber=1&pageSize=20`

Returns paged comments for the property, ordered by newest first.

Success response shape:

```json
{
  "message": null,
  "data": {
    "items": [
      {
        "commentId": 1,
        "userId": "11111111-1111-1111-1111-111111111111",
        "userDisplayName": "John Doe",
        "userProfileImage": "/images/profiles/user1.png",
        "content": "Great place.",
        "createdAt": "2026-04-27T18:00:00Z",
        "updatedAt": null
      }
    ],
    "pageNumber": 1,
    "pageSize": 20,
    "totalCount": 1,
    "totalPages": 1
  }
}
```

#### `POST /comments`

Creates a new comment by the current user.

Request body:

```json
{
  "content": "Very clean and quiet property."
}
```

#### `PUT /comments/{commentId}`

Updates a comment owned by the current user.

Request body:

```json
{
  "content": "Updated comment text."
}
```

#### `DELETE /comments/{commentId}`

Deletes a comment owned by the current user.

## Response Format

### Success responses

Service-based success responses use:

- `DTOs/Common/ApiResponseDto.cs`

Shape:

```json
{
  "message": "Optional success message",
  "data": {}
}
```

### Error responses

Feedback endpoints now use:

- `Models/ErrorResponse.cs`

Shape:

```json
{
  "message": "Error message",
  "action": null,
  "details": null,
  "statusCode": 403,
  "path": "/api/properties/1001/ratings",
  "traceId": "0H...",
  "timestamp": "2026-04-27T18:00:00Z",
  "errors": null
}
```

## Database Design

### `PropertyRatings`

Fields:

- `Id`
- `PropertyId`
- `UserId`
- `Rating`
- `CreatedAt`
- `UpdatedAt`

Rules:

- unique index on `(PropertyId, UserId)`
- check constraint enforcing rating range `1..5`

### `PropertyComments`

Fields:

- `Id`
- `PropertyId`
- `UserId`
- `Content`
- `CreatedAt`
- `UpdatedAt`

Rules:

- comment content is required
- max length is `1000`

## Main Files Added or Updated

### Controllers

- `Controllers/PropertyFeedbackController.cs`
- `Controllers/BaseController.cs`

### DTOs

- `DTOs/Common/ApiResponseDto.cs`
- `DTOs/PropertyFeedback/CreatePropertyRatingDto.cs`
- `DTOs/PropertyFeedback/UpdatePropertyRatingDto.cs`
- `DTOs/PropertyFeedback/PropertyRatingSummaryDto.cs`
- `DTOs/PropertyFeedback/CreatePropertyCommentDto.cs`
- `DTOs/PropertyFeedback/UpdatePropertyCommentDto.cs`
- `DTOs/PropertyFeedback/PropertyCommentDto.cs`

### Models

- `Models/PropertyRating.cs`
- `Models/PropertyComment.cs`
- `Models/ErrorResponse.cs`
- `Models/Property.cs`
- `Models/ApplicationUser.cs`

### Configurations

- `Data/Configurations/PropertyRatingConfiguration.cs`
- `Data/Configurations/PropertyCommentConfiguration.cs`

### Repositories

- `Repositories/Interfaces/IPropertyRatingRepo.cs`
- `Repositories/Interfaces/IPropertyCommentRepo.cs`
- `Repositories/Implementations/PropertyRatingRepo.cs`
- `Repositories/Implementations/PropertyCommentRepo.cs`
- `Repositories/Interfaces/IContractRepo.cs`
- `Repositories/Implementations/ContractRepo.cs`
- `Repositories/Interfaces/IPropertyRepo.cs`
- `Repositories/Implementations/PropertyRepo.cs`

### Services

- `Services/Interfaces/IPropertyRatingService.cs`
- `Services/Interfaces/IPropertyCommentService.cs`
- `Services/Implementations/PropertyRatingService.cs`
- `Services/Implementations/PropertyCommentService.cs`
- `Services/Implementations/ProfileService.cs`

### Database

- `Data/AppDbContext.cs`
- `Migrations/20260427154641_InitialSchemaWithPropertyRatingsAndComments.cs`

## Dependency Injection

Registered in `Program.cs`:

- `IPropertyRatingRepo -> PropertyRatingRepo`
- `IPropertyCommentRepo -> PropertyCommentRepo`
- `IPropertyRatingService -> PropertyRatingService`
- `IPropertyCommentService -> PropertyCommentService`

## User Deletion Behavior

When a user is deleted through profile deletion flow, their new feedback data is also deleted:

- property ratings by that user
- property comments by that user

This cleanup is handled in:

- `Services/Implementations/ProfileService.cs`

## Swagger and XML Documentation

The feature endpoints include:

- XML summary documentation
- documented response codes
- typed Swagger response models for success and error cases

This makes the endpoints show more accurately in Swagger UI.

## Suggested Manual Test Cases

### Ratings

1. Create a rating with an eligible renter account.
2. Try creating a second rating for the same property and same user.
3. Update the rating.
4. Delete the rating.
5. Try rating a property without an eligible contract.

### Comments

1. Create a comment with an eligible renter account.
2. List comments for the property.
3. Update the comment as its owner.
4. Try updating the comment as another user.
5. Delete the comment.
6. Try commenting on a property without an eligible contract.
