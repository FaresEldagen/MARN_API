# Migrated Features Frontend Guide

This document explains the newly migrated rental workflow features in `MARN_API` so the frontend team can integrate them confidently.

It focuses on:
- what the new features do
- how the user flow works from start to finish
- which endpoints the frontend will call
- which response fields matter for UI decisions
- which parts are backend-only

## Overview

The migrated feature set adds a full rental payment and contract pipeline:

1. Owner creates a Stripe connected account
2. Owner completes Stripe onboarding
3. Renter starts checkout for a property
4. Renter pays through Stripe Checkout
5. Backend webhook confirms payment
6. Backend generates the rental contract PDF
7. Backend stores the contract and OpenTimestamps proof
8. Background service later upgrades the proof and marks the contract as anchored

This means the frontend does not generate contracts or handle anchoring itself. The frontend mainly:
- starts the flow
- shows statuses
- displays records
- downloads contract/proof files

## Main Concepts

### Connected Account

Represents the Stripe payout account for an owner.

Why frontend cares:
- owner cannot receive rental payments until onboarding is complete
- checkout should fail if the owner has no connected account or onboarding is incomplete

### Payment

Represents the Stripe payment record for a rental checkout attempt.

Why frontend cares:
- useful for renter/owner payment history
- useful for showing payment progress and receipt links

### Rental Transaction

Represents the rental workflow state machine.

This is the best record for tracking the full rental process.

Why frontend cares:
- tells you whether the flow is just initiated, paid, pending anchoring, anchored, expired, etc.

### Contract

Represents the generated contract and its anchoring state.

Why frontend cares:
- contract existence means payment fulfillment completed
- anchoring status tells whether blockchain proof is still pending or fully anchored
- contract download and proof download come from this record

## Full Flow A to Z

### 1. Owner becomes owner

Endpoint:
- `POST /api/Account/become-owner`

Purpose:
- adds the `Owner` role to the authenticated user

Frontend note:
- after success, the user should log in again to refresh JWT claims

### 2. Owner creates connected account

Endpoint:
- `POST /api/connected-accounts`

Auth:
- owner JWT required

Returns:
- connected account identifiers
- `onboardingUrl`

Frontend action:
- redirect the owner to `onboardingUrl`

### 3. Owner checks onboarding status

Endpoint:
- `GET /api/connected-accounts/me/onboarding-status`

Important field:
- `isOnboardingComplete`

Frontend decision:
- if `false`, show owner that payouts are not ready yet
- if `true`, owner is ready to receive rental payments

### 4. Renter starts checkout

Endpoint:
- `POST /api/rentalworkflow/checkout`

Auth:
- renter JWT required

Request body:

```json
{
  "propertyId": 1001
}
```

Backend behavior:
- renter ID comes from JWT
- owner is derived from property
- backend validates owner connected account
- backend checks whether the property is already rented for the generated lease window
- backend creates Stripe checkout session
- backend creates payment record
- backend creates rental transaction record

Response:

```json
{
  "url": "https://checkout.stripe.com/..."
}
```

Frontend action:
- redirect user to `url`

### 5. Stripe payment happens

Frontend does not confirm payment directly.

Backend-only:
- Stripe calls `POST /api/payments/webhooks/stripe`
- backend updates payment
- backend generates contract
- backend updates rental transaction

### 6. Contract is created

After successful webhook processing:
- payment becomes succeeded
- contract is generated and saved
- rental transaction moves to `PendingAnchoring`
- contract anchoring status becomes `Pending`

### 7. Proof anchoring completes later

Backend-only:
- background service checks pending contracts
- if proof upgrade succeeds, contract becomes anchored
- rental transaction becomes anchored

Frontend implication:
- anchoring is asynchronous
- do not expect anchored status immediately after payment success

## Statuses the Frontend Should Care About

This section now lists the actual enum values currently used in the backend, not just the most common examples.

### Rental Transaction Status

Used for overall workflow progress.

All current values:
- `Initiated`
  - checkout session created but payment not completed
- `Paid`
  - payment succeeded and backend started fulfillment
- `ContractGenerated`
  - contract PDF generated during fulfillment
- `PendingAnchoring`
  - contract exists and proof is waiting for final anchoring upgrade
- `Expired`
  - checkout session expired without successful payment
- `Anchored`
  - proof upgrade completed
- `Failed`
  - workflow failed unexpectedly

Recommended frontend interpretation:
- `Initiated`: payment in progress / awaiting checkout completion
- `Paid`: payment succeeded and fulfillment is in progress
- `ContractGenerated`: contract PDF was produced and the workflow is still moving forward
- `PendingAnchoring`: payment done, contract created, proof still processing
- `Anchored`: final success state
- `Expired`: allow retry
- `Failed`: show support/retry style message

### Rental Payment Status

Used inside rental workflow records.

All current values:
- `NotPaid`
- `Paid`

Recommended frontend interpretation:
- `NotPaid`: renter has not completed a successful payment for this workflow record
- `Paid`: payment portion of the workflow completed successfully

### Payment Status

Used in payment records.

All current values:
- `Pending`
- `Succeeded`
- `Failed`
- `Refunded`
- `Expired`

Recommended frontend interpretation:
- `Pending`: payment not finalized yet
- `Succeeded`: payment completed
- `Failed`: payment attempt failed
- `Refunded`: payment was completed previously but later refunded
- `Expired`: user did not finish checkout in time

### Contract Status

Used in contract records for the business lifecycle of the contract itself.

All current values:
- `Pending`
- `Active`
- `Cancelled`
- `Expired`

Recommended frontend interpretation:
- `Pending`: contract row exists but final business activation is still in progress
- `Active`: normal usable contract state
- `Cancelled`: contract was cancelled
- `Expired`: contract term ended or was marked expired by business logic

### Contract Anchoring Status

Used in contract records.

All current values:
- `Pending`
- `Anchored`
- `Failed`

Recommended frontend interpretation:
- `Pending`: contract is valid and generated, but blockchain proof is still processing
- `Anchored`: proof is complete
- `Failed`: anchoring did not complete successfully and may require retry/support handling

## Frontend-Relevant Endpoints

## Connected Accounts

### `GET /api/connected-accounts`

Testing/list endpoint.

Returns:
- list of connected account records

### `GET /api/connected-accounts/me`

Owner-only.

Returns:
- owner connected account details

Useful fields:
- `connectedAccountId`
- `applicationUserId`
- `stripeAccountId`
- `isOnboardingComplete`
- `createdAt`
- `updatedAt`

### `GET /api/connected-accounts/me/onboarding-status`

Owner-only.

Useful fields:
- `isOnboardingComplete`

### `POST /api/connected-accounts`

Owner-only.

Useful response fields:
- `connectedAccountId`
- `stripeAccountId`
- `onboardingUrl`

### `POST /api/connected-accounts/me/onboarding-link`

Owner-only.

Use when:
- owner needs a fresh onboarding link

## Rental Workflow

### `POST /api/rentalworkflow/checkout`

Renter-only flow starter.

Response field:
- `url`

### `GET /api/rentalworkflow`

Testing/list endpoint.

### `GET /api/rentalworkflow/my`

User-scoped list for authenticated user.

### `GET /api/rentalworkflow/by-property/{propertyId}`

Open property-scoped list.

### `GET /api/rentalworkflow/{id}`

Open single workflow record.

### `GET /api/rentalworkflow/by-payment/{paymentRecordId}`

Open lookup by payment record.
Useful if UI starts from payment record and needs workflow status.

### `GET /api/rentalworkflow/by-contract/{contractId}`

Open lookup by contract record.
Useful if UI starts from contract record and needs workflow status.

Important response fields:
- `id`
- `renterId`
- `ownerId`
- `propertyId`
- `stripeSessionId`
- `paymentId`
- `contractId`
- `status`
- `paymentStatus`
- `createdAt`
- `completedAt`

Frontend decisions:
- if `contractId` exists, contract section can be shown
- if `status == PendingAnchoring`, show contract as created but proof still pending
- if `status == Anchored`, show final verified state

## Payments

### `GET /api/payments`

Testing/list endpoint.

### `GET /api/payments/my`

Authenticated user payment list.

### `GET /api/payments/by-property/{propertyId}`

Open property payment list.

### `GET /api/payments/by-user/{userId}`

Open payment list for a specific user ID, whether that user is owner or renter.

### `GET /api/payments/{paymentId}`

Open single payment record.

Important response fields:
- `id`
- `contractId`
- `stripeSessionId`
- `amountTotal`
- `platformFee`
- `ownerAmount`
- `renterId`
- `renterEmail`
- `ownerId`
- `propertyId`
- `ownerStripeAccountId`
- `paidAt`
- `paymentIntentId`
- `receiptUrl`
- `currency`
- `status`
- `createdAt`

Frontend decisions:
- if `receiptUrl` exists, show receipt action
- if `contractId` exists, user can navigate to contract details/download
- if `status == Succeeded`, payment is complete
- if `status == Expired`, allow retry path

## Contracts

### `GET /api/contracts`

Testing/list endpoint.

### `GET /api/contracts/my`

Authenticated user contract list.

### `GET /api/contracts/by-property/{propertyId}`

Open property contract list.

### `GET /api/contracts/by-user/{userId}`

Open contract list for a specific user ID, whether that user is owner or renter.

### `GET /api/contracts/{contractId}`

Open single contract record.

Important response fields:
- `id`
- `propertyId`
- `renterId`
- `ownerId`
- `leaseStartDate`
- `leaseEndDate`
- `fileName`
- `hash`
- `submittedAt`
- `anchoredAt`
- `transactionId`
- `merkleRoot`
- `status`
- `anchoringStatus`
- `createdAt`
- `updatedAt`

Frontend decisions:
- if `anchoringStatus == Pending`, show "proof processing"
- if `anchoringStatus == Anchored`, show verified/anchored state
- if `transactionId` exists, you can optionally expose blockchain proof info in UI

### `GET /api/contracts/{contractId}/download`

Downloads the PDF contract.

### `GET /api/contracts/{contractId}/proof`

Downloads the `.ots` proof file.

### `POST /api/contracts/verify`

Uploads a file and checks whether it matches stored contract hash.

Important response fields:
- `match`
- `message`
- `status`
- `anchoringStatus`
- `anchoredAt`

### `POST /api/contracts/hash`

Returns SHA-256 hash for an uploaded file.

### `POST /api/contracts/generate-pdf`

Manual utility endpoint.

Usually not needed by the normal frontend rental flow.

### `POST /api/contracts/extract-proof-data`

Manual utility endpoint for proof inspection.

Usually not needed by the normal frontend rental flow.

## Recommended Frontend UX Logic

## Owner Side

### Before accepting rental monetization

Check:
- owner connected account exists
- `isOnboardingComplete == true`

If not complete:
- show CTA to continue onboarding

### Owner dashboard ideas

Show:
- connected account onboarding state
- rental workflow state for each paid/active rental
- contract download once `contractId` exists
- anchored badge only when `anchoringStatus == Anchored`

## Renter Side

### Before checkout

Use property details as usual.

On checkout:
- call `POST /api/rentalworkflow/checkout`
- redirect to returned Stripe URL

### After returning from Stripe

Do not rely only on the success page text.

Instead:
- query workflow records or payment records
- wait for backend webhook fulfillment

Recommended UI states:
- payment submitted
- payment confirmed
- contract generated
- proof pending
- proof anchored

## Important Integration Notes

### Checkout is property-based

Frontend only sends:
- `propertyId`

Do not send:
- owner ID
- renter ID

Those are derived by the backend.

### Webhook is backend-only

Frontend should never call:
- `POST /api/payments/webhooks/stripe`

### Anchoring is delayed

The contract may exist before blockchain proof is fully anchored.

That is expected.

### Open and user-scoped endpoint rules

Open/testing endpoints currently include:
- `GET /api/contracts`
- `GET /api/contracts/{contractId}`
- `GET /api/contracts/by-property/{propertyId}`
- `GET /api/contracts/by-user/{userId}`
- `GET /api/payments`
- `GET /api/payments/{paymentId}`
- `GET /api/payments/by-property/{propertyId}`
- `GET /api/payments/by-user/{userId}`
- `GET /api/rentalworkflow`
- `GET /api/rentalworkflow/{id}`
- `GET /api/rentalworkflow/by-property/{propertyId}`
- `GET /api/rentalworkflow/by-payment/{paymentRecordId}`
- `GET /api/rentalworkflow/by-contract/{contractId}`
- `GET /api/connected-accounts`

User-scoped/protected endpoints still include:
- `GET /api/contracts/my`
- `GET /api/payments/my`
- `GET /api/rentalworkflow/my`
- owner connected-account endpoints like `me`, `me/onboarding-status`, and onboarding-link creation

Frontend recommendation:
- for normal user dashboards, prefer `my` endpoints
- use open lookup endpoints only when the product flow intentionally needs direct lookup by ID or property

## Error Handling Expectations

The migrated endpoints now return clearer error payloads than before.

Common patterns:
- business-rule failures return `400`
  - example: owner not onboarded
  - example: property not found
  - example: property already rented for the requested period
- unauthorized access returns `401`
- forbidden access returns `403`
- missing records return `404`
- unexpected server problems return `500`

Global error responses now include:
- `message`
- `statusCode`
- `path`
- `traceId`
- `timestamp`

Validation errors may also include:
- `errors`

Frontend recommendation:
- show user-friendly message from `message`
- log `traceId` for support/debugging

## Frontend Decision Rules

This section is the quickest way for the frontend team to translate backend data into UI behavior.

## Owner UI Rules

### Connected account CTA

If owner has no connected account:
- show `Create payout account`

If owner has connected account and `isOnboardingComplete == false`:
- show `Continue Stripe onboarding`

If owner has connected account and `isOnboardingComplete == true`:
- show `Payouts ready`

### Owner rental card state

If rental workflow `status == Initiated`:
- show `Checkout created`

If rental workflow `status == PendingAnchoring`:
- show `Paid`
- show `Contract available`
- show `Proof pending`

If rental workflow `status == Anchored`:
- show `Paid`
- show `Contract available`
- show `Blockchain proof verified`

If rental workflow `status == Expired`:
- show `Checkout expired`

If rental workflow `status == Failed`:
- show `Processing issue`

## Renter UI Rules

### Rent / pay button

If property is rentable and backend checkout endpoint is available:
- show `Pay now` or `Continue to payment`

If checkout returns `400` with owner onboarding-related message:
- show `This property is temporarily unavailable for payment`

If checkout returns `400` with already-rented message:
- show `This property is already rented for the requested rental period`

If checkout returns `400` with self-rental message:
- hide rent button for owner viewing own property

### Renter rental state

If payment exists and rental workflow is still `Initiated`:
- show `Payment started`

If payment `status == Succeeded` and rental workflow `status == PendingAnchoring`:
- show `Payment successful`
- show `Contract created`
- show `Proof pending`

If contract `anchoringStatus == Anchored`:
- show `Verified contract`

If payment `status == Expired` or workflow `status == Expired`:
- show `Payment session expired`
- show retry action

## Suggested Screen States

### Property details page

Can show:
- normal property data
- rent/pay CTA
- disabled CTA if owner onboarding is incomplete and checkout fails for that reason

### Payment return screen

After Stripe redirects back:
- show a temporary `Processing your rental...` state
- do not assume success page means contract is already created
- start polling workflow or payment status

### Renter contract area

When `contractId` exists:
- show `Download contract`

When contract proof exists:
- show `Download proof`

When `anchoringStatus == Pending`:
- show subtle `Blockchain proof is still processing`

When `anchoringStatus == Anchored`:
- show `Anchored on Bitcoin`

### Owner payout status panel

Can show:
- no connected account
- onboarding incomplete
- payout ready

That alone will prevent many support questions.

## Recommended Polling Strategy

The most important async point is right after Stripe payment success.

### After Stripe success redirect

Recommended frontend flow:

1. redirect user to your success screen
2. show `Processing payment and contract...`
3. poll one of these:
   - `GET /api/rentalworkflow/my`
   - or a specific payment/workflow detail endpoint if the frontend already knows the related IDs
4. stop polling when:
   - `contractId` appears
   - or workflow reaches `PendingAnchoring`
   - or workflow reaches `Anchored`
   - or workflow reaches `Failed`
   - or workflow reaches `Expired`

Recommended interval:
- every 3 to 5 seconds for the first 30 seconds
- then slower if needed

Recommended timeout:
- around 30 to 60 seconds for the "immediate fulfillment" expectation

If timeout happens:
- show `Payment received. Contract generation is still processing. Please refresh shortly.`

### For anchoring completion

Do not poll aggressively for anchoring.

Because anchoring is background-based and slower, a better UX is:
- show contract as usable immediately once it exists
- show `proof pending`
- refresh anchoring state when user revisits dashboard/contract page
- optionally poll slowly on the contract details page

## Suggested Error-to-UI Mapping

### `400 BadRequest`

Usually means business rule / invalid operation.

Examples:
- property not found
- owner not onboarded
- renter cannot rent own property

Frontend action:
- show `message`
- keep user on current page

### `401 Unauthorized`

Frontend action:
- redirect to login
- or show session expired state

### `403 Forbidden`

Frontend action:
- show access denied
- do not retry automatically

### `404 NotFound`

Frontend action:
- show missing resource state
- useful for deleted/unknown contract, payment, or workflow record

### `500 InternalServerError`

Frontend action:
- show generic fallback like `Something went wrong. Please try again.`
- log `traceId`

## Minimal Integration Checklist

For the frontend team, the smallest complete integration usually needs:

1. owner onboarding entry point
2. owner onboarding status display
3. renter checkout by property ID
4. post-payment processing screen
5. renter contract download
6. optional proof download
7. workflow/payment/contract status display

If those are done well, the migrated features will already feel complete to end users.

## Suggested Frontend Flow Summary

### Owner

1. become owner
2. create connected account
3. redirect to onboarding
4. poll/check onboarding status
5. once complete, owner is payout-ready

### Renter

1. open property
2. start checkout using property ID
3. redirect to Stripe
4. after payment return, poll workflow/payment/contract state
5. when contract exists, show download
6. when anchoring completes, show anchored badge/state

## Backend-Only Parts

The frontend does not need to implement these:
- Stripe webhook processing
- contract file generation internals
- OpenTimestamps submission
- OpenTimestamps upgrade polling
- workflow recovery on webhook retries

Those are already handled by the backend.

## Final Recommendation for Frontend Team

For UI state tracking, prefer this priority:

1. `RentalTransaction.status`
2. `Contract.anchoringStatus`
3. `Payment.status`

Why:
- `RentalTransaction` tells the overall business workflow state
- `Contract` tells proof/anchoring state
- `Payment` tells pure payment state

If the frontend uses those three together, it can represent the full lifecycle clearly without guessing.
