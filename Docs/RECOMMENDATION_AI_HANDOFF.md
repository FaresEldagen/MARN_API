# Recommendation AI Handoff

This document describes:

- the `UserActivity` model stored by MARN API
- the allowed `UserActivityType` values
- which MARN API endpoints create or remove activity records
- the endpoints/contracts used between MARN API and the recommendation AI service

## 1. UserActivity Model

Stored table/entity: `UserActivity`

```json
{
  "id": 0,
  "userId": "00000000-0000-0000-0000-000000000000",
  "propertyId": 1001,
  "userActivityType": "view",
  "metadata": null,
  "createdAt": "2026-05-26T10:30:00Z"
}
```

### Fields

| Field              | Type       | Required | Notes                                                       |
| ------------------ | ---------- | -------: | ----------------------------------------------------------- |
| `Id`               | `long`     |      Yes | Primary key                                                 |
| `UserId`           | `Guid`     |      Yes | The user who performed the activity                         |
| `PropertyId`       | `long?`    |       No | `null` for `search`, filled for property-related activities |
| `UserActivityType` | `string`   |      Yes | One of the 5 allowed values below                           |
| `Metadata`         | `string?`  |       No | JSON string, used only for `search`                         |
| `CreatedAt`        | `DateTime` |      Yes | Stored in UTC                                               |

## 2. Allowed UserActivityType Values

Only these 5 values are used:

| Value     | Meaning                                    |
| --------- | ------------------------------------------ |
| `view`    | User opened a property details page        |
| `save`    | User saved a property                      |
| `search`  | User searched for properties               |
| `booking` | User created a booking request             |
| `rent`    | User signed a contract and became a renter |

## 3. Activity Rules

### `view`

- Recorded when an authenticated user opens property details
- Recorded for all authenticated users except admins
- Recorded even if the viewer is the property owner
- Owner views create activity records, but the property's public `Views` counter is still not incremented for the owner
- `PropertyId` is filled
- `Metadata` is `null`

Example:

```json
{
  "userId": "6f4c1d11-3a3d-4ec8-a0e8-6f9e1b204001",
  "propertyId": 1001,
  "userActivityType": "view",
  "metadata": null
}
```

### `save`

- Recorded when a user saves a property
- `PropertyId` is filled
- `Metadata` is `null`
- If the user later unsaves the property, MARN deletes the matching `save` activity record(s) for that `userId + propertyId`

Example:

```json
{
  "userId": "6f4c1d11-3a3d-4ec8-a0e8-6f9e1b204001",
  "propertyId": 1001,
  "userActivityType": "save",
  "metadata": null
}
```

### `search`

- Recorded when an authenticated user calls property search
- `PropertyId` is `null`
- `Metadata` contains the search filter object serialized as JSON

Example:

```json
{
  "userId": "6f4c1d11-3a3d-4ec8-a0e8-6f9e1b204001",
  "propertyId": null,
  "userActivityType": "search",
  "metadata": "{\"keyword\":\"maadi\",\"city\":\"Cairo\",\"minPrice\":3000,\"maxPrice\":8000,\"isShared\":true,\"page\":1,\"pageSize\":20}"
}
```

### `booking`

- Recorded when a user successfully creates a booking request
- `PropertyId` is filled
- `Metadata` is `null`

### `rent`

- Recorded when a user successfully signs a contract
- `PropertyId` is filled
- `Metadata` is `null`

## 4. Search Metadata Shape

For `userActivityType = "search"`, `metadata` is the JSON serialization of `PropertySearchFilterDto`.

Possible fields:

```json
{
  "keyword": "maadi",
  "latitude": 30.0,
  "longitude": 31.2,
  "radiusKm": 5,
  "city": "Cairo",
  "governorate": "CairoGovernorate",
  "type": "Apartment",
  "rentalUnit": "Monthly",
  "isShared": true,
  "minPrice": 3000,
  "maxPrice": 8000,
  "minBedrooms": 2,
  "minBeds": 2,
  "minBathrooms": 1,
  "minMaxOccupants": 2,
  "minSquareMeters": 80,
  "maxSquareMeters": 150,
  "minRating": 4,
  "amenities": ["Wifi", "AirConditioning"],
  "sortBy": "Newest",
  "sortAscending": false,
  "page": 1,
  "pageSize": 20
}
```

this is the possible governorate values

```json
{
  "code": "SUCCESS",
  "message": "Success.",
  "data": [
    {
      "id": 0,
      "name": "CairoGovernorate",
      "displayName": "Cairo"
    },
    {
      "id": 1,
      "name": "GizaGovernorate",
      "displayName": "Giza"
    },
    {
      "id": 2,
      "name": "AlexandriaGovernorate",
      "displayName": "Alexandria"
    },
    {
      "id": 3,
      "name": "QalyubiaGovernorate",
      "displayName": "Qalyubia"
    },
    {
      "id": 4,
      "name": "PortSaidGovernorate",
      "displayName": "Port Said"
    },
    {
      "id": 5,
      "name": "SuezGovernorate",
      "displayName": "Suez"
    },
    {
      "id": 6,
      "name": "DakhaliaGovernorate",
      "displayName": "Dakahlia"
    },
    {
      "id": 7,
      "name": "SharkiaGovernorate",
      "displayName": "Sharqia"
    },
    {
      "id": 8,
      "name": "GharbiaGovernorate",
      "displayName": "Gharbia"
    },
    {
      "id": 9,
      "name": "MonufiaGovernorate",
      "displayName": "Monufia"
    },
    {
      "id": 10,
      "name": "BehiraGovernorate",
      "displayName": "Beheira"
    },
    {
      "id": 11,
      "name": "KafrElSheikhGovernorate",
      "displayName": "Kafr El Sheikh"
    },
    {
      "id": 12,
      "name": "DamiettaGovernorate",
      "displayName": "Damietta"
    },
    {
      "id": 13,
      "name": "IsmailiaGovernorate",
      "displayName": "Ismailia"
    },
    {
      "id": 14,
      "name": "FaiyumGovernorate",
      "displayName": "Faiyum"
    },
    {
      "id": 15,
      "name": "BeniSuefGovernorate",
      "displayName": "Beni Suef"
    },
    {
      "id": 16,
      "name": "MiniaGovernorate",
      "displayName": "Minya"
    },
    {
      "id": 17,
      "name": "AsyutGovernorate",
      "displayName": "Asyut"
    },
    {
      "id": 18,
      "name": "SohagGovernorate",
      "displayName": "Sohag"
    },
    {
      "id": 19,
      "name": "QenaGovernorate",
      "displayName": "Qena"
    },
    {
      "id": 20,
      "name": "LuxorGovernorate",
      "displayName": "Luxor"
    },
    {
      "id": 21,
      "name": "AswanGovernorate",
      "displayName": "Aswan"
    },
    {
      "id": 22,
      "name": "RedSeaGovernorate",
      "displayName": "Red Sea"
    },
    {
      "id": 23,
      "name": "NewValleyGovernorate",
      "displayName": "New Valley"
    },
    {
      "id": 24,
      "name": "MarsaMatruhGovernorate",
      "displayName": "Marsa Matruh"
    },
    {
      "id": 25,
      "name": "NorthSinaiGovernorate",
      "displayName": "North Sinai"
    },
    {
      "id": 26,
      "name": "SouthSinaiGovernorate",
      "displayName": "South Sinai"
    }
  ]
}
```

All fields are optional except paging defaults:

- `page` defaults to `1`
- `pageSize` defaults to `20`

## 5. MARN API Endpoints That Produce User Activity

### Property view

- Method: `GET`
- Route: `api/Property/{propertyId}`
- Activity written: `view`
- Notes:
  - anonymous users do not create activity
  - admins do not create activity
  - owners do create `view` activity if they open their own property

### Property search

- Method: `GET`
- Route: `api/Property/search`
- Activity written: `search`
- Notes:
  - only authenticated users create activity
  - search filters are stored in `metadata`

### Save property

- Method: `POST`
- Route: `api/Property/save/{propertyId}`
- Behavior:
  - if property becomes saved -> create `save` activity
  - if property becomes unsaved -> remove matching `save` activity

### Create booking request

- Method: `POST`
- Route: `api/BookingRequest/add`
- Activity written: `booking`

Request body shape:

```json
{
  "propertyId": 1001,
  "startDate": "2026-06-01T00:00:00",
  "endDate": "2026-07-01T00:00:00",
  "paymentFrequency": "Monthly"
}
```

### Sign contract

- Method: `POST`
- Route: `api/contracts/{contractId}/sign`
- Activity written: `rent`

## 6. Recommendation Endpoint in MARN API

This is the endpoint exposed by MARN API to the frontend/mobile app:

- Method: `GET`
- Route: `api/Homepage/recommendations`

### Behavior

- If the caller is authenticated:
  - MARN API calls the recommendation AI service
  - receives a list of recommended property ids
  - filters out invalid properties
  - fills missing slots using top-viewed properties until the result count reaches 8

- If the caller is anonymous:
  - MARN API does **not** call the recommendation AI service
  - returns the top-viewed public properties directly

### Returned properties must be public/usable

Any property id returned by the AI service is accepted only if the property is:

- active
- verified
- not soft-deleted

If returned ids are invalid or missing, MARN fills the gaps with high-view properties.

## 7. Contract Expected From Recommendation AI Service

This is the endpoint that MARN API calls on your friend's recommendation service.

### Recommendation request

- Method: `POST`
- URL: configured in appsettings as `ExternalPropertyAi:RecommendationUrl`

Request body:

```json
{
  "userId": "6f4c1d11-3a3d-4ec8-a0e8-6f9e1b204001"
}
```

### Recommendation response

Response body must be a plain JSON array of property ids:

```json
[1001, 1044, 1090, 1203]
```

### Important notes

- Response is expected to be `List<long>`
- Duplicates are allowed but MARN API removes duplicates while keeping the original order
- Example:
  - AI returns: `[1001, 1005, 1001, 1010]`
  - MARN keeps: `[1001, 1005, 1010]`
- If response is empty, invalid, or request fails, MARN falls back entirely to top-viewed properties
- If response contains fewer than 8 valid public properties, MARN fills the remaining slots from top-viewed properties

## 8. Property Sync Endpoints Expected From Recommendation AI Service

MARN API also notifies the AI service when properties change.

All 3 use the same request body shape:

```json
{
  "propertyId": 1001
}
```

### Property added

- Method: `POST`
- URL: configured in appsettings as `ExternalPropertyAi:PropertyAddedUrl`
- Triggered when:
  - owner adds a new property through `POST api/Property/add`
  - admin restores a deleted property through `PATCH api/Admin/stats/properties/{propertyId}/restore-deleted`

### Property updated

- Method: `POST`
- URL: configured in appsettings as `ExternalPropertyAi:PropertyUpdatedUrl`
- Triggered when:
  - owner edits a property through `PUT api/Property/edit/{propertyId}`

### Property deleted

- Method: `POST`
- URL: configured in appsettings as `ExternalPropertyAi:PropertyDeletedUrl`
- Triggered when:
  - owner deletes a property through `DELETE api/Property/delete/{propertyId}`
  - admin deletes a property through `DELETE api/Admin/stats/properties/{propertyId}`

## 9. Summary

Your friend mainly needs to know:

- `UserActivity` has 5 possible values only: `view`, `save`, `search`, `booking`, `rent`
- `search` is the only activity that uses `metadata`
- recommendation AI receives only `{ "userId": "..." }`
- recommendation AI must return a plain JSON array of property ids
- MARN API will validate returned ids and fill missing/invalid ones using top-viewed public properties
- MARN API also sends property add/update/delete notifications using `{ "propertyId": 123 }`
