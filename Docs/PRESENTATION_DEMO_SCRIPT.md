# MARN API — Presentation Demo Script

> **Project:** MARN — A property rental management platform (API)  
> **Demo Password for all seeded accounts:** `Password123!`  
> **Admin Email:** `admin@marn.com`

---
## Overview

This script walks through 6 acts designed to showcase **every major feature** of the MARN system in a logical, story-driven order. The flow simulates a real user journey — from registration to renting, property ownership, and admin management.

---
## Act 1 — Authentication & Account Security (Mixed)

> **Goal:** Show registration, email confirmation, login, Google OAuth, password reset, and 2FA.

### Via Web
| #   | Step                                                                                           | Account / Details                                                                |
| --- | ---------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------- |
| 1   | **Register a new account** with email via **Temp Mail**                                                  | POST `api/Account/register` — show the validation (password rules, email format) |
| 2   | **Show the confirmation email** received (email includes a front-end link with userId & token)           | Demonstrate the email template                                                   |
| 3   | **Try to log in before confirming email** — show the error response                                      | POST `api/Account/login` — should fail                                           |
| 4   | **Confirm the email** using the link/token                                                               | GET `api/Account/confirm-email?userId=...&token=...`                             |
| 5   | **Log in successfully** with the confirmed account                                                       | POST `api/Account/login` — returns JWT token                                     |
| 6   | Show the translation                                                                                     |                                                                                  |
| 7   | **Log out**                                                                                              |                                                                                  |
| 8   | **Try login with Google provider** (show the OAuth flow)                                                 | POST `api/Account/external/google` — first-time creates account automatically    |
| 9   | **Log out**                                                                                              |                                                                                  |
| 10  | **Log in** with the new account using a **wrong password**                                               |                                                                                  |
| 11  | **Request a password reset**                                                                             | POST `api/Account/forgot-password` — email with reset link is sent               |
| 12  | **Reset the password** using the token                                                                   | PUT `api/Account/reset-password`                                                 |

### Via Mobile
| #   | Step                                                   | Account / Details                                            |
| --- | ------------------------------------------------------ | ------------------------------------------------------------ |
| 13  | **Log in with the new password**                                                                         | Verify the reset worked                                      |
| 14  | Show the translation                                                                                     |                                                              |
| 15  | **Enable Two-Factor Authentication (2FA)**                                                               | PUT `api/Profile/toggle-2fa`                                 |
| 16  | **Log out and log back in** — show the 2FA code prompt                                                   | Login returns 202, then verify with `api/Account/verify-2fa` |
| 17  | **Disable 2FA** for easier demo flow                                                                     | PUT `api/Profile/toggle-2fa` again                           |

---
## Act 2 — Renter Journey (Unverified → Verified) (via web)

> **Goal:** Show profile management, KYC verification, property browsing, search & filter, saving, booking, roommate matching, AI chatbot, and real-time chat.

| #   | Step                                                                                                     | Account / Details                                                                |
| --- | -------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------- |
| 18  | **Log in as the newly created user**                                                                     | Account is currently **Unverified**                                              |
| 19  | **Browse the Homepage recommendations** (personalized for authenticated users, popular for anonymous)    | GET `api/Homepage/recommendations`                                               |
| 20  | **View the Renter Dashboard** — show it's empty for a new user                                           | GET `api/Profile/renter-dashboard`                                               |
| 21  | **View personal profile**                                                                                | GET `api/Profile/profile` — show basic info, account status = Unverified         |
| 22  | **Edit basic profile data** — upload a profile image, add bio, phone, DOB, gender                        | PUT `api/Profile/edit-profile-basic`                                             |
| 23  | **Submit legal/KYC verification data** — upload front & back ID photos, Arabic name, national ID         | PUT `api/Profile/edit-profile-legal` — account status changes to **Pending**     |
| 24  | **Change password**                                                                                      | PUT `api/Profile/change-password`                                                |
| 25  | **Set up Roommate Preferences** (smoking, pets, sleep schedule, budget, etc.)                            | PUT `api/Profile/edit-profile-roommate-preferences`                              |
| 26  | **Get Roommate Matches** — view compatibility-ranked profiles                                            | GET `api/Roommate/matches`                                                       |
|     |                                                                                                          |                                                                                  |
| 27  | **Search properties** with filters (e.g., City: Cairo, Type: Apartment, MinPrice: 5000, MaxPrice: 10000) | GET `api/Property/search?city=Cairo&type=Apartment&minPrice=5000&maxPrice=10000` |
| 28  | **Search and view** (`Shared Seed House` [ID: 1100]) which is any **sheared property with active contracts** |                                                                                  |
| 29  | View matching percents with active renters to this property                                              |                                                                                  |
| 30  | **Try to add a booking request** before verification                                                     | POST `api/BookingRequest/add` — should fail (account not verified)               |
| 31  | **Save the property** `Shared Seed House` [ID: 1100])  (toggle bookmark)                                 | POST `api/Property/save/2001`                                                    |
| 32  | **View property's owner public profile**                                                                 | GET `api/Profile/profile/{ownerXId}` — shows owner's properties and rating       |
| 33  | **Try to add a property** before verification                                                            |                                                                                  |
|     |                                                                                                          |                                                                                  |
| 34  | **Use the AI Assistant Chatbot** — ask about property recommendations                                    | POST `api/Assistant/messages` — show AI response                                 |
| 35  | **View AI Assistant session list**                                                                       | GET `api/Assistant/sessions`                                                     |
| 36  | **Contact Support** — send a support message                                                             | POST `api/Support/contact-us`                                                    |
| 37  | **Log out**                                                                                              |                                                                                  |

---
## Act 3 — Admin Verification & Moderation (via web)

> **Goal:** Show the admin dashboard, user/property verification, user management (ban/unban/delete/restore), role management, moderation reports, and analytics.

| #    | Step                                                                                                       | Account / Details                                                                   |
| ---- | ---------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------- |
| 38  | **Log in as Admin** (`admin@marn.com` / `Password123!`)                                                  |                                                                                     |
| 39  | **View the Admin Dashboard Overview** — total users, properties, revenue, pending verifications, chart   | GET `api/Admin/dashboard/overview`                                                  |
|      |                                                                                                            |                                                                                     |
| 40  | **View the pending user verifications queue**                                                            | GET `api/Admin/verifications/users/pending`                                         |
| 41  | **Open the verification details** for the new user — show the uploaded ID photos, Arabic name, national ID | GET `api/Admin/verifications/users/{userId}`                                        |
| 42  | **Approve the user's verification** for the new user                                                     | PATCH `api/Admin/verifications/users/{userId}/approve`                              |
| 43  | **Decline (Pending Renter) user**                                                                        |                                                                                     |
| Oral | Explain that user Account Status and what to get each of them and what is its limitations                  |                                                                                     |
|      |                                                                                                            |                                                                                     |
| 44  | **View the pending property verifications queue** — show properties with Pending status                  | GET `api/Admin/verifications/properties/pending`                                    |
| 45  | **View property verification details** for a pending property (`Nasr City Shared Loft` [ID: 2004])       | GET `api/Admin/verifications/properties/2004`                                       |
| 46  | **Approve the property verification**                                                                    | PATCH `api/Admin/verifications/properties/2004/approve`                             |
| 47  | **Decline another pending property** with a reason (`Miami Beach Studio` [ID: 2009])                     | PATCH `api/Admin/verifications/properties/2009/decline` with reason                 |
|      |                                                                                                            |                                                                                     |
| 48  | **View Detailed Property Statistics** — breakdown by type, governorate, status                           | GET `api/Admin/stats/properties`                                                    |
| 49  | **View a deep admin property view** (`Nasr City Shared Loft` [ID: 2004])                                 | GET `api/Admin/stats/properties/2004` — show owner info, media, contracts, bookings |
| 50  | **Soft-delete a property** from admin (`Nasr City Shared Loft` [ID: 2004])                               | DELETE `api/Admin/stats/properties/2004`                                            |
| 51  | **Restore the deleted property**                                                                         | PATCH `api/Admin/stats/properties/2004/restore-deleted`                             |
|      |                                                                                                            |                                                                                     |
| 52  | **View User Management table** — search, filter by status/role                                           | GET `api/Admin/users`                                                               |
| 53  | **View user details** for a specific user (e.g., "Banned Renter")                                        | GET `api/Admin/users/{bannedRenterId}`                                              |
| 54  | **Assign the Admin role** to a new user                                                                  | PATCH `api/Admin/roles/users/{userId}`                                              |
| 55  | **Ban a user** (Banned Renter)                                                                           | PATCH `api/Admin/users/{renterEId}/ban`                                             |
| 56  | **Log out**                                                                                              |                                                                                     |
|      |                                                                                                            |                                                                                     |
| 57  | **Log in as Banned Renter** (`banned.renter@example.com` / `Password123!`)                               |                                                                                     |
| 58  | **View the Renter Dashboard**                                                                            |                                                                                     |
| Oral | Explain what he can do and what he can not                                                                 |                                                                                     |
| 59  | **Try to add a booking request** to a saved property                                                     | POST `api/BookingRequest/add` — should fail (account not verified)                  |
| 60  | **Try to pay the next payment then cancel**                                                              |                                                                                     |
| 61  | **Log out**                                                                                              |                                                                                     |
|      |                                                                                                            |                                                                                     |
| 62  | **Log in as Admin** (`admin@marn.com` / `Password123!`)                                                  |                                                                                     |
| 63  | **Unban the banned user**                                                                                | PATCH `api/Admin/users/{bannedRenterId}/unban`                                      |
| 64  | **Soft-delete a user** (`Renter A`) - Failed due to his Active Contracts                                 | DELETE `api/Admin/users/{userId}`                                                   |
| 65  | **Soft-delete a user** (`Pending Renter`)<br>(success) - Don't have Active Contracts                     | DELETE `api/Admin/users/{userId}`                                                   |
| 66  | **Try to log in with the deleted account** — show the error                                              |                                                                                     |
| Oral | Explain Soft Delete Procedure and account restoration                                                      |                                                                                     |
| 67  | **Restore a deleted user**                                                                               | PATCH `api/Admin/users/{deletedRenterId}/restore`                                   |
|      |                                                                                                            |                                                                                     |
| 68  | **View Detailed Contract Statistics** — status breakdown, total value, paged table                       | GET `api/Admin/stats/contracts`                                                     |
| 69  | **Download a contract PDF** from admin                                                                   | GET `api/Admin/stats/contracts/{contractId}/download`                               |
| 70  | Open the Contract                                                                                        |                                                                                     |
| Oral | Explain the contract content                                                                               |                                                                                     |
| 71  | **Terminate a contract** from admin                                                                      | PATCH `api/Admin/stats/contracts/{contractId}/cancel`                               |
| Oral | Explain the contract termination pro                                                                       |                                                                                     |
|      |                                                                                                            |                                                                                     |
| 72  | **View Detailed Revenue Statistics** — payments, platform revenue, owner payouts, time series            | GET `api/Admin/stats/revenue`                                                       |
| 73  | **View the Moderation Reports queue** — see report statuses and types                                    | GET `api/Admin/reports`                                                             |
| 74  | **View a report's full details**                                                                         | GET `api/Admin/reports/{reportId}`                                                  |
| 75  | **Review a report** — approve with action (e.g., ban user, hide message, deactivate property)            | PATCH `api/Admin/reports/{reportId}/review`                                         |
| 76  | **Generate an Analytics Report** (scope: Full, format: PDF, period: ThisMonth)                           | POST `api/Admin/analytics-reports/generate`                                         |
| 77  | **View the report history**                                                                              | GET `api/Admin/analytics-reports`                                                   |
| 78  | **Download the generated analytics report**                                                              | GET `api/Admin/analytics-reports/{reportId}/download`                               |
| 79  | **Log out**                                                                                              |                                                                                     |

---
## Act 4 — Property Owner Journey (via web)

> **Goal:** Show becoming an owner, adding/editing/deleting properties, handling booking requests, creating contracts, and the owner dashboard with earnings.

| #   | Step                                                                                                                     | Account / Details                                                          |
| --- | ------------------------------------------------------------------------------------------------------------------------ | -------------------------------------------------------------------------- |
| 80  | **Log in as the verified new user**                                                                      |                                                                            |
| 81  | **View the Admin Dashboard**                                                                             |                                                                            |
| 82  | **View the Renter Dashboard**                                                                            |                                                                            |
| 83  | **Submit a booking request** for the  saved property                                                     | POST `api/BookingRequest/add` with start date, end date, payment frequency |
|     |                                                                                                                          |                                                                            |
| 84  | **Become an Owner** — add the Owner role to the account                                                  | POST `api/Property/become-owner` — returns new JWT with Owner role         |
| 85  | **Add a new property** — fill in title, description, type, price, images, amenities, rules, proof of ownership, location | POST `api/Property/add` (multipart form)                                   |
| 86  | **View the Owner Dashboard** — show properties count, vacant/occupied, earnings summary, pending booking requests | GET `api/Profile/owner-dashboard`                                          |
| 87  | **Edit the property** — change title, add/remove images, update price                                    | PUT `api/Property/edit/{propertyId}`                                       |
| 88  | **Deactivate the property** — make it invisible in search                                                | PUT `api/Property/deactivate/{propertyId}`                                 |
| 89  | **Reactivate the property** — toggle back to searchable                                                  | PUT `api/Property/deactivate/{propertyId}` (toggle)                        |
| 90  | **Log out**                                                                                              |                                                                            |
|     |                                                                                                                          |                                                                            |
| 91  | **Log in as the renter** (`renter.a@example.com` / `Password123!`)                                       |                                                                            |
| 92  | **View the Renter Dashboard** - Show data                                                                |                                                                            |
| 93  | **Unsave any property** (toggle again)                                                                   | POST `api/Property/save/2001` — demonstrates toggle behavior               |
| 94  | **Choose saved property that** (`Nile View Apartment` [ID: 2001])                                        |                                                                            |
| 95  | **Submit a booking request** for the property                                                            | POST `api/BookingRequest/add` with start date, end date, payment frequency |
| 96  | **Start a chat** about the booking request with the owner                                                | POST `api/BookingRequest/{bookingRequestId}/start-chat`                    |
| 97  | **View the chat users list**                                                                             | GET `api/Chat/users`                                                       |
| 98  | **Search for** (`Banned Renter`) and **open his chat**                                                   | GET `api/Chat/history/{ownerUserId}`                                       |
| 99  | **Submit a report** against (`Banned Renter`)                                                            | POST `api/Reports` — type: User, reason: "Suspicious profile"              |
| 100 | **Log out**                                                                                              |                                                                            |

---
## Act 5 — Contracts, Renter Payments, & Blockchain Anchoring (via mobile)

> **Goal:** Show the full contract lifecycle — creation, signing, PDF generation, blockchain anchoring (Open Timestamps), payment via Stripe, and contract verification.

| #    | Step                                                                                                                | Account / Details                                                                      |
| ---- | ------------------------------------------------------------------------------------------------------------------- | -------------------------------------------------------------------------------------- |
| 101 | **Log in as the property owner** (`owner.x@example.com` / `Password123!`)  <br>(via mobile 1)            |                                                                                        |
| 102 | **Log in as the renter** (`renter.a@example.com` / `Password123!`) <br>(via mobile 2)<br>then close the app |                                                                                        |
| 103 | **View the Owner Dashboard** — see the pending booking request <br>(via mobile 1)                        | GET `api/Profile/owner-dashboard`                                                      |
| 104 | **Replay on renter message**<br>(via mobile 1)                                                           |                                                                                        |
| 105 | **Show FCM notification on renter mobile**<br>(via mobile 2)                                             |                                                                                        |
| 106 | **Replay on owner message**<br>(via mobile 2)                                                            |                                                                                        |
| 107 | **Create a contract** from the booking request <br>(via mobile 1)                                        | POST `api/contracts/create/{bookingRequestId}` — contract is created in Pending status |
| 108 | **View the contract details** <br>(via mobile 1)                                                         | GET `api/contracts/{contractId}` — show parties, lease dates, total value              |
| 109 | **Show Signal R notification on renter mobile**<br>(via mobile 2)                                        |                                                                                        |
| 110 | **View the Renter Dashboard** — see the pending contract <br>(via mobile 2)                              | GET `api/Profile/renter-dashboard`                                                     |
| 111 | **View the contract details** <br>(via mobile 2)                                                         | GET `api/contracts/{contractId}` — show parties, lease dates, total value              |
| 112 | **Sign the contract** — triggers PDF generation, SHA-256 hashing, and Open Timestamps submission <br>(via mobile 2) | POST `api/contracts/{contractId}/sign` — contract becomes Active                       |
| Oral | Explain that if the contract still pending the renter or the owner can cancel its progress                          |                                                                                        |
| 113 | **Show Signal R notification on owner mobile**<br>(via mobile 1)                                         |                                                                                        |
| 114 | **Download the contract PDF** <br>(via mobile 2)                                                         | GET `api/contracts/{contractId}/download` — show the bilingual PDF                     |
| Oral | Explain the Blockchain process                                                                                      |                                                                                        |
| 115 | **Download the OpenTimestamps proof file** (.ots) <br>(via mobile 2)                                     | GET `api/contracts/{contractId}/proof`                                                 |
| 116 | **Verify the contract** — upload the PDF to validate its hash matches the stored hash <br>(via mobile 2) | POST `api/contracts/verify?contractId={id}`                                            |
|      |                                                                                                                     |                                                                                        |
| 117 | **Go to the property** from the contract page<br>(via mobile 2)                                          |                                                                                        |
| 118 | **Rate the property** (e.g., 4 stars)<br>(via mobile 2)                                                  | POST `api/properties/1001/ratings`                                                     |
| 119 | **Update the rating** (change to 5 stars)<br>(via mobile 2)                                              | PUT `api/properties/1001/ratings/me`                                                   |
| 120 | **Delete the rating**<br>(via mobile 2)                                                                  | DELETE `api/properties/1001/ratings/me`                                                |
|      |                                                                                                                     |                                                                                        |
| 121 | **View the Renter Dashboard** — see the Next payment<br>(via mobile 2)                                   | GET `api/Profile/renter-dashboard`                                                     |
| 122 | **Make a payment** via Stripe<br>(via mobile 2)                                                          | POST `api/Payment/start-payment?paymentScheduleId={id}` — returns Stripe client secret |
| 123 | **View notifications** — check for payment confirmation notification<br>(via mobile 2)                   | GET `api/Notification/notifications-get`                                               |
| 124 | **Show Signal R notification on owner mobile**<br>(via mobile 1)                                         |                                                                                        |


---
## Act 6 — Owner Payouts & Stripe Connect (via web)

> **Goal:** Show Stripe Connect onboarding for owners and the withdrawal flow.

| #   | Step                                                                                          | Account / Details                        |
| --- | --------------------------------------------------------------------------------------------- | ---------------------------------------- |
| 125 | **Log in as the property owner** (`owner.x@example.com` / `Password123!`)                                | Has existing contracts with payments     |
| 126 | **View all notifications** - received and available payments                                             | GET `api/Notification/notifications-get` |
| 127 | **View the Owner Dashboard** — show monthly/yearly earnings, withdrawable vs. on-hold amounts            | GET `api/Profile/owner-dashboard`        |
| 128 | **Create a Stripe Connect account** — get the onboarding URL                                             | POST `api/Payment/connect-account`       |
| 129 | **Complete Stripe onboarding** (follow the returned URL)                                                 | Stripe handles KYC for the owner         |
| 130 | **Withdraw available funds** to the connected bank account                                               | POST `api/Payment/withdraw`              |

---
## Quick Reference: Seeded Data

| Role            | Email                        | Password       | Notes                                                                                                                      |
| --------------- | ---------------------------- | -------------- | -------------------------------------------------------------------------------------------------------------------------- |
| Admin           | `admin@marn.com`             | `Password123!` | Full admin access                                                                                                          |
| Pending Renter  | `pending.renter@example.com` | `Password123!` | Pending verification                                                                                                       |
| Banned Renter   | `banned.renter@example.com`  | `Password123!` | User have active contract with an available payment in day 6/23<br>Have At least one saved property                        |
| Renter A        | `renter.a@example.com`       | `Password123!` | Has active contracts & roommate preferences<br>Saving `Property 4` with no active contract <br>Saving any another property |
| Owner X         | `owner.x@example.com`        | `Password123!` | Owns properties 1001, 1003, etc.<br>Has existing contracts with Available payments                                         |

| ID   | Name                      | Owner     | Notes                                                                  |
| ---- | ------------------------- | --------- | ---------------------------------------------------------------------- |
| 1100 | **Agouza Shared House**     | `Owner X` | Is shared<br>Have at least one active renter with roommate preferences |
| 2004 | **Nasr City Shared Loft** | `Owner Z` | Pending Property with good data                                        |
| 2009 | **Miami Beach Studio**    | `Owner X` | Pending Property with wrong data                                       |
| 2001 | **Nile View Apartment**   | `Owner X` | that is **Owner X unshared property**                                  |
