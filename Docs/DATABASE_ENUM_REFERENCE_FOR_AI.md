# Database Enum Reference for AI

This document helps services that read the database directly decode integer enum columns.

Most C# enums are stored in SQL Server as `int`. Some fields are enum-like strings instead; those are listed near the end.

## Quick Table/Column Index

| Table | Column | Enum |
| --- | --- | --- |
| `AspNetUsers` | `Language` | `Language` |
| `AspNetUsers` | `Gender` | `Gender` |
| `AspNetUsers` | `Country` | `Country` |
| `AspNetUsers` | `AccountStatus` | `AccountStatus` |
| `AspNetUsers` | `StatusBeforeBan` | `AccountStatus`, nullable |
| `AdminAnalyticsReports` | `Scope` | `AdminAnalyticsReportScope` |
| `AdminAnalyticsReports` | `Format` | `AdminAnalyticsReportFormat` |
| `AdminAnalyticsReports` | `RequestedPeriod` | `AdminAnalyticsReportPeriod` |
| `BookingRequests` | `PaymentFrequency` | `PaymentFrequency` |
| `Contracts` | `Status` | `ContractStatus` |
| `Contracts` | `PaymentFrequency` | `PaymentFrequency` |
| `Contracts` | `AnchoringStatus` | `ContractAnchoringStatus` |
| `Notifications` | `UserType` | `NotificationUserType` |
| `Notifications` | `Type` | `NotificationType` |
| `Notifications` | `ActionType` | `NotificationActionType`, nullable |
| `Payments` | `Status` | `PaymentStatus` |
| `PaymentSchedules` | `Status` | `PaymentScheduleStatus` |
| `Properties` | `Type` | `PropertyType` |
| `Properties` | `RentalUnit` | `RentalUnit` |
| `Properties` | `Status` | `PropertyStatus` |
| `PropertyAmenities` | `Amenity` | `AmenityType` |
| `Reports` | `ReportableType` | `ReportableType` |
| `Reports` | `Status` | `ReportStatus` |
| `Reports` | `ActionTaken` | `ReportModerationActionType`, nullable |
| `RoommatePreferences` | `Governorate` | `Governorate` |
| `RoommatePreferences` | `SearchStatus` | `RoommateSearchStatus` |
| `RoommatePreferences` | `SleepSchedule` | `SleepSchedule` |
| `RoommatePreferences` | `EducationLevel` | `EducationLevel` |
| `RoommatePreferences` | `FieldOfStudy` | `FieldOfStudy` |
| `RoommatePreferences` | `GuestsFrequency` | `GuestsFrequency` |
| `RoommatePreferences` | `WorkSchedule` | `WorkSchedule` |
| `RoommatePreferences` | `SharingLevel` | `SharingLevel` |

## Account/User Enums

### `Language`

Appears in `AspNetUsers.Language`.

| Value | Meaning |
| --- | --- |
| `0` | `English` |
| `1` | `Arabic` |

### `Gender`

Appears in `AspNetUsers.Gender`.

| Value | Meaning |
| --- | --- |
| `0` | `Unknown` |
| `1` | `Male` |
| `2` | `Female` |

### `Country`

Appears in `AspNetUsers.Country`.

| Value | Meaning |
| --- | --- |
| `0` | `Unknown` |
| `1` | `Egypt` |
| `2` | `SaudiArabia` |
| `3` | `UnitedArabEmirates` |
| `4` | `Qatar` |
| `5` | `Kuwait` |
| `6` | `Bahrain` |
| `7` | `Oman` |
| `8` | `Jordan` |
| `9` | `Lebanon` |

### `AccountStatus`

Appears in `AspNetUsers.AccountStatus` and nullable `AspNetUsers.StatusBeforeBan`.

| Value | Meaning |
| --- | --- |
| `0` | `Unverified` |
| `1` | `Pending` |
| `2` | `Verified` |
| `3` | `Declined` |
| `4` | `Banned` |

## Property Enums

### `PropertyType`

Appears in `Properties.Type`.

| Value | Meaning |
| --- | --- |
| `0` | `Apartment` |
| `1` | `House` |
| `2` | `Room` |
| `3` | `Villa` |
| `4` | `Studio` |
| `5` | `SharedRoom` |

### `RentalUnit`

Appears in `Properties.RentalUnit`.

| Value | Meaning |
| --- | --- |
| `0` | `Daily` |
| `1` | `Monthly` |
| `2` | `Yearly` |

### `PropertyStatus`

Appears in `Properties.Status`.

| Value | Meaning |
| --- | --- |
| `0` | `Pending` |
| `1` | `Verified` |
| `2` | `Declined` |

### `AmenityType`

Appears in `PropertyAmenities.Amenity`.

| Value | Meaning |
| --- | --- |
| `0` | `Wifi` |
| `1` | `Parking` |
| `2` | `AirConditioning` |
| `3` | `Heating` |
| `4` | `Tv` |
| `5` | `Elevator` |
| `6` | `Pool` |
| `7` | `Gym` |
| `8` | `Kitchen` |
| `9` | `Dishwasher` |
| `10` | `Microwave` |
| `11` | `Refrigerator` |
| `12` | `Washer` |
| `13` | `Dryer` |
| `14` | `Balcony` |
| `15` | `HardwoodFloors` |
| `16` | `StorageSpace` |
| `17` | `SecuritySystem` |

### `Governorate`

Appears in `RoommatePreferences.Governorate`.

Note: `Properties.State` is a string column, not an integer enum column. The backend stores the `Governorate` enum name there using `Governorate.ToString()`, so values look like `CairoGovernorate`, `GizaGovernorate`, etc.

| Value | Meaning |
| --- | --- |
| `0` | `CairoGovernorate` |
| `1` | `GizaGovernorate` |
| `2` | `AlexandriaGovernorate` |
| `3` | `QalyubiaGovernorate` |
| `4` | `PortSaidGovernorate` |
| `5` | `SuezGovernorate` |
| `6` | `DakhaliaGovernorate` |
| `7` | `SharkiaGovernorate` |
| `8` | `GharbiaGovernorate` |
| `9` | `MonufiaGovernorate` |
| `10` | `BehiraGovernorate` |
| `11` | `KafrElSheikhGovernorate` |
| `12` | `DamiettaGovernorate` |
| `13` | `IsmailiaGovernorate` |
| `14` | `FaiyumGovernorate` |
| `15` | `BeniSuefGovernorate` |
| `16` | `MiniaGovernorate` |
| `17` | `AsyutGovernorate` |
| `18` | `SohagGovernorate` |
| `19` | `QenaGovernorate` |
| `20` | `LuxorGovernorate` |
| `21` | `AswanGovernorate` |
| `22` | `RedSeaGovernorate` |
| `23` | `NewValleyGovernorate` |
| `24` | `MarsaMatruhGovernorate` |
| `25` | `NorthSinaiGovernorate` |
| `26` | `SouthSinaiGovernorate` |

## Roommate Preference Enums

### `RoommateSearchStatus`

Appears in `RoommatePreferences.SearchStatus`.

| Value | Meaning |
| --- | --- |
| `0` | `Searching` |
| `1` | `Offering` |
| `2` | `Found` |

### `SleepSchedule`

Appears in `RoommatePreferences.SleepSchedule`.

| Value | Meaning |
| --- | --- |
| `0` | `Unknown` |
| `1` | `EarlyBird` |
| `2` | `NightOwl` |
| `3` | `Flexible` |

### `EducationLevel`

Appears in `RoommatePreferences.EducationLevel`.

| Value | Meaning |
| --- | --- |
| `0` | `Unknown` |
| `1` | `HighSchool` |
| `2` | `Bachelor` |
| `3` | `Master` |
| `4` | `Doctorate` |

### `FieldOfStudy`

Appears in `RoommatePreferences.FieldOfStudy`.

| Value | Meaning |
| --- | --- |
| `0` | `Unknown` |
| `1` | `Engineering` |
| `2` | `Medicine` |
| `3` | `Business` |
| `4` | `ComputerScience` |
| `5` | `Arts` |
| `6` | `Law` |
| `7` | `Physics` |
| `8` | `Psychology` |
| `9` | `Pharmacy` |
| `99` | `Other` |

### `GuestsFrequency`

Appears in `RoommatePreferences.GuestsFrequency`.

| Value | Meaning |
| --- | --- |
| `0` | `Unknown` |
| `1` | `Never` |
| `2` | `Rarely` |
| `3` | `Sometimes` |
| `4` | `Often` |

### `WorkSchedule`

Appears in `RoommatePreferences.WorkSchedule`.

| Value | Meaning |
| --- | --- |
| `0` | `Unknown` |
| `1` | `NotWorking` |
| `2` | `DayShift` |
| `3` | `NightShift` |
| `4` | `Mixed` |
| `5` | `Remote` |

### `SharingLevel`

Appears in `RoommatePreferences.SharingLevel`.

| Value | Meaning |
| --- | --- |
| `0` | `Unknown` |
| `1` | `Low` |
| `2` | `Medium` |
| `3` | `High` |

## Payment and Contract Enums

### `PaymentFrequency`

Appears in `BookingRequests.PaymentFrequency` and `Contracts.PaymentFrequency`.

| Value | Meaning |
| --- | --- |
| `0` | `OneTime` |
| `1` | `Monthly` |
| `2` | `Quarterly` |
| `3` | `Yearly` |

### `ContractStatus`

Appears in `Contracts.Status`.

| Value | Meaning |
| --- | --- |
| `0` | `Pending` |
| `1` | `Active` |
| `2` | `Cancelled` |
| `3` | `Expired` |

### `ContractAnchoringStatus`

Appears in `Contracts.AnchoringStatus`.

| Value | Meaning |
| --- | --- |
| `0` | `Pending` |
| `1` | `Anchored` |
| `2` | `Failed` |

### `PaymentStatus`

Appears in `Payments.Status`.

| Value | Meaning |
| --- | --- |
| `0` | `OnHold` |
| `1` | `Available` |
| `2` | `Withdrawn` |

### `PaymentScheduleStatus`

Appears in `PaymentSchedules.Status`.

| Value | Meaning |
| --- | --- |
| `0` | `NotAvailableYet` |
| `1` | `Available` |
| `2` | `DueToday` |
| `3` | `PaidEarly` |
| `4` | `PaidOnTime` |
| `5` | `PaidLate` |
| `6` | `Overdue` |
| `7` | `Cancelled` |

## Notification Enums

### `NotificationUserType`

Appears in `Notifications.UserType`.

| Value | Meaning |
| --- | --- |
| `0` | `General` |
| `1` | `Renter` |
| `2` | `Owner` |

### `NotificationType`

Appears in `Notifications.Type`.

| Value | Meaning |
| --- | --- |
| `0` | `General` |
| `1` | `NewMessage` |
| `2` | `NewBookingRequest` |
| `3` | `BookingRequestCanceled` |
| `4` | `BookingRequestRejected` |
| `5` | `NewReview` |
| `6` | `ContractStarted` |
| `7` | `ContractCanceled` |
| `8` | `ContractSigned` |
| `9` | `ContractCompleted` |
| `10` | `UpcomingPayment` |
| `11` | `PaymentArrived` |
| `12` | `DelayedPayment` |
| `13` | `PaymentSuccessful` |
| `14` | `PaymentFailed` |
| `15` | `PaymentReceived` |
| `16` | `AvailableForWithdrawal` |
| `17` | `ConnectAccountSuccess` |
| `18` | `ConnectAccountFailed` |
| `19` | `WithdrawSuccess` |
| `20` | `WithdrawFailed` |
| `21` | `PropertyAdded` |
| `22` | `PropertyEdited` |
| `23` | `PropertyDeleted` |

### `NotificationActionType`

Appears in nullable `Notifications.ActionType`.

| Value | Meaning |
| --- | --- |
| `1` | `Property` |
| `2` | `ChatUser` |
| `3` | `EditProfile` |
| `4` | `RenterDashboard` |
| `5` | `OwnerDashboard` |
| `6` | `Contract` |
| `7` | `Payment` |

## Report/Admin Enums

### `ReportableType`

Appears in `Reports.ReportableType`.

| Value | Meaning |
| --- | --- |
| `0` | `User` |
| `1` | `Property` |
| `2` | `Message` |
| `3` | `PropertyComment` |

### `ReportStatus`

Appears in `Reports.Status`.

| Value | Meaning |
| --- | --- |
| `0` | `InReview` |
| `1` | `Resolved` |
| `2` | `Rejected` |

### `ReportModerationActionType`

Appears in nullable `Reports.ActionTaken`.

| Value | Meaning |
| --- | --- |
| `1` | `BanUser` |
| `2` | `DeactivateProperty` |
| `3` | `HideMessage` |
| `4` | `HidePropertyComment` |

### `AdminAnalyticsReportScope`

Appears in `AdminAnalyticsReports.Scope`.

| Value | Meaning |
| --- | --- |
| `1` | `Overview` |
| `2` | `Users` |
| `3` | `Properties` |
| `4` | `Contracts` |
| `5` | `Revenue` |
| `6` | `Full` |

### `AdminAnalyticsReportFormat`

Appears in `AdminAnalyticsReports.Format`.

| Value | Meaning |
| --- | --- |
| `1` | `Pdf` |
| `2` | `Csv` |

### `AdminAnalyticsReportPeriod`

Appears in `AdminAnalyticsReports.RequestedPeriod`.

| Value | Meaning |
| --- | --- |
| `1` | `AllTime` |
| `2` | `ThisMonth` |
| `3` | `ThisYear` |
| `4` | `Custom` |

## String-Coded Fields

These are not integer enums, but they are still useful for the AI service to understand.

### Assistant message roles

Appears in `assistantMessages.Role`.

| Value | Meaning |
| --- | --- |
| `user` | Message sent by the user. |
| `assistant` | Final assistant response visible to the user. |
| `tool` | Tool/internal message, usually hidden from the frontend. |

Use `assistantMessages.ToolOnly = 1` for tool/internal records that should not appear in normal chat history.

### User activity types

Appears in `UserActivities.UserActivityType`.

| Value | Meaning |
| --- | --- |
| `view` | User viewed a property. |
| `save` | User saved a property. |
| `search` | User searched/filter-browsed properties. |
| `booking` | User made a booking request. |
| `rent` | User rented/signed a contract. |

### Admin action log strings

Appears in `AdminActionLogs.ActionType` and `AdminActionLogs.TargetType`.

`ActionType` is stored as a string name from `ReportModerationActionType`:

| Value |
| --- |
| `BanUser` |
| `DeactivateProperty` |
| `HideMessage` |
| `HidePropertyComment` |

`TargetType` is stored as a string name from `ReportableType`:

| Value |
| --- |
| `User` |
| `Property` |
| `Message` |
| `PropertyComment` |

## Important Non-Integer Notes

- `Properties.City` is a string column, not the `City` enum. It stores the submitted city text.
- `Properties.State` is a string column. It stores the `Governorate` enum name as text, for example `CairoGovernorate`, not the integer value `0`.
- `assistantMessages.Role` is a string with a database check constraint, not an integer.
- `ServiceResultType`, `PropertySortBy`, and `PropertyAvailability` are API/backend concepts and are not currently stored as integer columns in the main database tables.
