# Assistant Chatbot Backend Handoff

This document explains the backend assistant-chat feature and the contract expected from the AI service.

## Overview

The backend owns user-facing assistant chat sessions and visible messages. The AI service receives only a `sessionId`, reads the conversation context directly from the database, generates the assistant response, and returns that response to the backend.

The backend then stores the assistant response in the database and returns it to the frontend.

There are two new tables:

- `assistantSessions`: one row per chat session.
- `assistantMessages`: one row per message in a session.

Users can have multiple assistant chat sessions.

## Database Tables

### `assistantSessions`

Stores the list of chats for each user.

| Column | Type | Notes |
| --- | --- | --- |
| `SessionId` | `uniqueidentifier` | Primary key. |
| `UserId` | `uniqueidentifier` | User who owns the session. FK to `AspNetUsers.Id`. |
| `SessionName` | `nvarchar(200)` | Defaults to the session id string when created. Can be renamed later. |
| `CreatedAt` | `datetime2` | Created timestamp in UTC. |
| `UpdatedAt` | `datetime2` | Updated when messages are added or session is renamed. |

### `assistantMessages`

Stores all messages for an assistant session.

| Column | Type | Notes |
| --- | --- | --- |
| `MessageId` | `uniqueidentifier` | Primary key. |
| `UserId` | `uniqueidentifier` | User who owns the message/session. FK to `AspNetUsers.Id`. |
| `SessionId` | `uniqueidentifier` | FK to `assistantSessions.SessionId`. |
| `Role` | `nvarchar(20)` | Must be one of: `user`, `assistant`, `tool`. |
| `ToolOnly` | `bit` | `false` for user-visible messages, `true` for tool/internal AI messages. |
| `Content` | `nvarchar(max)` | Plaintext message content. |
| `ImagePathsJson` | `nvarchar(max)` | Nullable JSON array of image path strings for assistant messages. |
| `CreatedAt` | `datetime2` | Created timestamp in UTC. |

Important:

- User-visible chat history is only messages where `ToolOnly = false`.
- The frontend should normally see only `Role = user` and `Role = assistant`.
- AI/tool internals can be stored with `Role = tool` and `ToolOnly = true`.
- Content is plaintext so the AI service can read session history directly from the database.
- Assistant messages can include image paths in `ImagePathsJson`; old rows or text-only messages can leave it `null`.

## Backend Endpoints

All endpoints require authenticated users.

Base route:

```text
/api/assistant
```

### Send Message

```http
POST /api/assistant/messages
```

Request body:

```json
{
  "sessionId": null,
  "content": "I need help finding an apartment near Maadi."
}
```

For an existing chat:

```json
{
  "sessionId": "11111111-2222-3333-4444-555555555555",
  "content": "Can you make it cheaper?"
}
```

Backend behavior:

1. If `sessionId` is `null`, backend creates a new assistant session.
2. If `sessionId` is provided, backend validates that the session belongs to the authenticated user.
3. Backend stores the user message:
   - `Role = user`
   - `ToolOnly = false`
4. Backend calls the AI endpoint with only the `sessionId`.
5. AI returns the assistant response as text.
6. Backend stores the assistant response:
   - `Role = assistant`
   - `ToolOnly = false`
7. Backend returns the assistant message to the frontend.

Response data shape:

```json
{
  "sessionId": "11111111-2222-3333-4444-555555555555",
  "assistantMessage": {
    "messageId": "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee",
    "sessionId": "11111111-2222-3333-4444-555555555555",
    "role": "assistant",
    "content": "Sure, I can help with that.",
    "imagePaths": [],
    "createdAt": "2026-05-28T18:52:00Z"
  }
}
```

If the AI endpoint fails:

- The user message remains saved.
- No assistant message is saved.
- Backend returns an error saying the assistant response could not be generated.

### List Sessions

```http
GET /api/assistant/sessions
```

Returns the current user's assistant sessions, newest updated first.

Response item shape:

```json
{
  "sessionId": "11111111-2222-3333-4444-555555555555",
  "sessionName": "Apartment search",
  "createdAt": "2026-05-28T18:50:00Z",
  "updatedAt": "2026-05-28T18:52:00Z",
  "lastMessagePreview": "Sure, I can help with that.",
  "lastMessageAt": "2026-05-28T18:52:00Z"
}
```

### Get Session Messages

```http
GET /api/assistant/sessions/{sessionId}/messages
```

Returns visible messages for one session.

Backend filters messages by:

```sql
ToolOnly = false
```

and only returns normal user-facing roles:

- `user`
- `assistant`

Response item shape:

```json
{
  "messageId": "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee",
  "sessionId": "11111111-2222-3333-4444-555555555555",
  "role": "assistant",
  "content": "Sure, I can help with that.",
  "imagePaths": [],
  "createdAt": "2026-05-28T18:52:00Z"
}
```

### Rename Session

```http
PATCH /api/assistant/sessions/{sessionId}/name
```

Request body:

```json
{
  "sessionName": "Apartment search"
}
```

Backend validates that the session belongs to the current user and that the name is not empty.

## AI Service Contract

The backend calls the AI service after saving the user's message.

Configured in backend:

```json
{
  "AssistantAi": {
    "ChatUrl": "https://your-ai-service/chat"
  }
}
```

Request from backend to AI:

```http
POST {AssistantAi:ChatUrl}
Content-Type: application/json
```

Body:

```json
{
  "sessionId": "11111111-2222-3333-4444-555555555555"
}
```

The AI service should:

1. Receive the `sessionId`.
2. Query `assistantMessages` for that session.
3. Use the full session context as needed.
4. Ignore or specially handle tool/internal records depending on its own logic.
5. Return the final assistant response to the backend.

Supported AI response formats:

Plain text:

```text
Sure, I can help with that.
```

JSON string:

```json
"Sure, I can help with that."
```

JSON object with one of these string fields:

```json
{
  "content": "Sure, I can help with that.",
  "imagePaths": [
    "/images/properties/example-1.jpg",
    "/images/properties/example-2.jpg"
  ]
}
```

Also accepted:

```json
{
  "message": "Sure, I can help with that."
}
```

or:

```json
{
  "response": "Sure, I can help with that."
}
```

The backend stores the extracted text as the visible assistant message.

`imagePaths` is optional. When present, it must be an array of strings. Backend stores only valid paths:

- non-empty strings;
- paths starting with `/`, for example `/images/...`;
- no external URLs;
- no Windows absolute paths;
- no backslashes;
- no `..` path traversal segments;
- max 20 image paths.

Invalid paths are ignored. If the response has valid text content and some invalid image paths, the assistant message is still saved with only the valid image paths.

## Tool/Internal Messages

If the AI service needs to store tool calls, tool results, or private reasoning/context records, it can insert rows into `assistantMessages`.

Recommended shape for tool-only messages:

```text
Role = tool
ToolOnly = true
```

Example:

```json
{
  "messageId": "bbbbbbbb-cccc-dddd-eeee-ffffffffffff",
  "userId": "99999999-8888-7777-6666-555555555555",
  "sessionId": "11111111-2222-3333-4444-555555555555",
  "role": "tool",
  "toolOnly": true,
  "content": "{ \"tool\": \"property_search\", \"resultCount\": 5 }",
  "imagePathsJson": null,
  "createdAt": "2026-05-28T18:51:00Z"
}
```

These records will not appear in the frontend chat history because the backend filters them out.

## Notes for Direct Database Access

When the AI service reads context, a typical query is:

```sql
SELECT MessageId, UserId, SessionId, Role, ToolOnly, Content, ImagePathsJson, CreatedAt
FROM assistantMessages
WHERE SessionId = @sessionId
ORDER BY CreatedAt ASC;
```

If the AI only wants user-visible messages:

```sql
SELECT MessageId, UserId, SessionId, Role, Content, ImagePathsJson, CreatedAt
FROM assistantMessages
WHERE SessionId = @sessionId
  AND ToolOnly = 0
ORDER BY CreatedAt ASC;
```

If the AI inserts tool messages directly, it must use the same `UserId` as the owning session.

To get the session owner:

```sql
SELECT UserId
FROM assistantSessions
WHERE SessionId = @sessionId;
```

## Current Backend Defaults

- New session name defaults to the `SessionId` string.
- Backend-visible messages are stored with `ToolOnly = false`.
- Backend-created roles are only `user` and `assistant`.
- AI/tool-created internal messages should use `tool` and `ToolOnly = true`.
- The backend does not encrypt assistant message content.
- Text-only assistant responses return `imagePaths: []` to the frontend.
