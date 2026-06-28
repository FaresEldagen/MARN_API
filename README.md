# MARN API — Property Rental, Roommate Matching, & Blockchain-Anchored Contracting Platform

[![.NET 8.0](https://img.shields.io/badge/.NET-8.0-blueviolet.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![Stripe](https://img.shields.io/badge/Payments-Stripe-008cdd.svg)](https://stripe.com)
[![OpenTimestamps](https://img.shields.io/badge/Blockchain-Bitcoin%20OTS-orange.svg)](https://opentimestamps.org)
[![Hangfire](https://img.shields.io/badge/Scheduler-Hangfire-red.svg)](https://hangfire.io)
[![SignalR](https://img.shields.io/badge/WebSockets-SignalR-blue.svg)](https://learn.microsoft.com/en-us/aspnet/core/signalr/introduction)

**MARN** is a professional, enterprise-grade, bilingual (English/Arabic) backend API built with **ASP.NET Core 8.0**. It serves as a comprehensive property rental and management system designed to streamline landlord-tenant relations, provide compatibility-driven roommate matching, secure lease agreements using Bitcoin blockchain time-stamping, automate billing via Stripe Connect, and deliver personalized recommendation feeds powered by AI.

---

## 🚀 Core Subsystems & Key Features

### 1. Identity & Security Suite
* **ASP.NET Core Identity**: Managed registration, login, and secure user states.
* **Two-Factor Authentication (2FA)**: High-security MFA policies with custom verification endpoints.
* **JWT Bearer Authentication**: Signed JSON Web Tokens containing user roles and claims (e.g., Owner, Admin, Renter).
* **Google OAuth**: Integrated Google authentication endpoint (`api/Account/external/google`) for seamless third-party login.
* **Banned Account Access Filter**: Custom Action Filter (`BannedAccountAccessFilter`) enforcing restrictions on suspended accounts system-wide.
* **IP Partitioned Rate Limiting**: Multi-tier rate limiting policies (Global, StrictAuth, Moderate) utilizing `PartitionedRateLimiter` to mitigate DDoS and brute-force attacks.

### 2. User Profiles & KYC Verification
* **Roommate Preferences Profile**: Tracks user habits (e.g., smoking tolerance, pets, sleep schedules, gender constraints, budgets).
* **KYC (Know Your Customer) Flow**: Verification cycle shifting users from **Unverified** to **Pending** upon uploading official documents (ID photos, Arabic name, national ID number) for Admin approval.
* **Roommate Matching Engine**: Evaluates roommate profiles and ranks matches by compatibility score. Compares active renters in shared properties (`Agouza Shared House` [ID: 1100]) to show compatibility rates.

### 3. Property Management & Search Engine
* **Multimodal Listings**: Support for pricing configurations, rules, amenities, geographical coordinates, images, and proof-of-ownership documentation.
* **Advanced Search Filters**: Geospatial and feature-rich queries filtering by type, city, price range, and occupancy status.
* **Listing Lifecycle**: Ability for verified owners to activate, edit, deactivate, or reactivate properties.

### 4. Bilingual Contracting & Blockchain Integrity
* **QuestPDF Engine**: Automatically generates professional, bilingually structured (English/Arabic) lease agreement PDFs.
* **Bitcoin Anchoring (OpenTimestamps)**: Hashes the signed PDF file (SHA-256) and submits it to public Bitcoin calendar servers. Users receive an `.ots` proof file.
* **Proof Verification Endpoint**: Allows upload of a contract PDF to verify that its cryptographic hash matches the original signature on the database and blockchain records.

### 5. Automated Payments & Stripe Connect
* **Stripe Payment Gateway**: Enables tenants to pay rent directly via Stripe Checkout/Payment Intents.
* **Stripe Connect Integration**: Custom onboarding for landlords. Seamlessly splits fees (10% platform fee, 90% owner payout) and routes payouts to the owner's bank account.
* **Escrow Holding Logic**: Platform-configured hold periods (e.g., 10 days) on transaction funds.

### 6. Real-Time Interactions & Chatbot AI
* **SignalR WebSocket Hubs**: Direct channels for instant notifications (`NotificationHub`) and peer-to-peer chats (`ChatHub`) with online/offline presence tracking (`ConnectionTracker`).
* **Firebase Cloud Messaging (FCM)**: Native push notification triggers for mobile applications.
* **AI Chat Assistant & Recommendations**: Interfaces with external AI clients (`ExternalPropertyAiClient`, `AssistantAiClient`) to provide personalized recommendation feeds and interactive chat sessions.

### 7. Administration & Moderation Console
* **Admin Dashboard**: System-wide health monitoring displaying active counts, total monthly revenue, and user verification queues.
* **Moderation Center**: A reporting system tracking flagged items (users, properties, comments). Supports actions such as banning users, hiding messages, or deactivating properties.
* **Soft-Delete Procedures**: System-wide soft deletion for users and properties allowing administrators to restore accounts while protecting databases from cascading dependency breaks.
* **Automated PDF Reports**: On-demand Generation of system performance summaries and analytics files.

---

## 🛠 Tech Stack

* **Framework**: .NET 8.0 (ASP.NET Core Web API)
* **Database**: Microsoft SQL Server with Entity Framework Core (EF Core)
* **Identity & Auth**: Microsoft.AspNetCore.Identity, JwtBearer, Google OAuth API
* **Task Scheduling**: Hangfire (using SQL Server persistence)
* **Real-time WebSockets**: SignalR
* **Billing System**: Stripe SDK (`Stripe.net`)
* **Document Generation**: QuestPDF
* **AI Clients**: System.Net.Http (integrations with custom Python recommendation/chatbot microservices)
* **Push Notifications**: Firebase Admin SDK
* **Testing & Tools**: Swashbuckle (Swagger/OpenAPI), HealthChecks
