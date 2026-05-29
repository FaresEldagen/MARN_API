# Assistant Chatbot Frontend Guide

This document explains how the frontend should use the assistant chatbot backend endpoints.

## Base Route

All assistant endpoints are under:

```text
/api/Assistant
```

All endpoints require authentication:

```http
Authorization: Bearer {accessToken}
Accept-Language: en
```

## Main Concepts

The chatbot has two main resources:

- Session: one chat thread.
- Message: one user or assistant message inside a session.

A user can have multiple sessions. For a new chat, send `sessionId: null` or omit `sessionId`. For an existing chat, send the existing `sessionId`.

Frontend-visible messages are only:

- `role: "user"`
- `role: "assistant"`

Internal AI/tool messages are hidden by the backend.

Assistant messages may include optional image paths in `imagePaths`.

## Send Message

Use this endpoint when the user sends a message.

```http
POST /api/Assistant/messages
Content-Type: application/json
```

### New Chat Request

```json
{
  "sessionId": null,
  "content": "hello, are there any properties in alexandria?"
}
```

You can also omit `sessionId`:

```json
{
  "content": "hello, are there any properties in alexandria?"
}
```

### Existing Chat Request

```json
{
  "sessionId": "11111111-2222-3333-4444-555555555555",
  "content": "can you show cheaper options?"
}
```

### Success Response

The backend returns the session id and the assistant response.

```json
{
  "code": "ZZ_ASSISTANT_MESSAGE_SENT_SUCCESSFULLY",
  "message": "Success",
  "data": {
    "sessionId": "11111111-2222-3333-4444-555555555555",
    "assistantMessage": {
      "messageId": "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee",
      "sessionId": "11111111-2222-3333-4444-555555555555",
      "role": "assistant",
      "content": "Yes, I found some properties in Alexandria.",
      "imagePaths": [
        "/images/properties/example-1.jpg"
      ],
      "createdAt": "2026-05-29T12:00:00Z"
    }
  }
}
```

### Frontend Behavior

Recommended UI flow:

1. User types a message.
2. Immediately render the user's message locally in the chat UI.
3. Call `POST /api/Assistant/messages`.
4. If this is a new chat, save `data.sessionId` in frontend state.
5. Append `data.assistantMessage` to the chat UI.
6. Refresh the sessions sidebar/list if needed.

The response does not include the user message because the frontend already has it.

If `assistantMessage.imagePaths` has values, render them as images under/inside the assistant bubble. If it is empty, render the message as normal text-only chat.

### AI Failure Behavior

If the backend saves the user message but the AI service fails, the backend returns an error. The user message remains saved in the database.

Frontend can show a retry/error state such as:

```text
Assistant response could not be generated. Try again.
```

If the user refreshes history, their saved user message will still appear.

## List Chat Sessions

Use this endpoint for the chat sidebar or previous-chat screen.

```http
GET /api/Assistant/sessions
```

### Success Response

```json
{
  "code": "SUCCESS",
  "message": "Success",
  "data": [
    {
      "sessionId": "11111111-2222-3333-4444-555555555555",
      "sessionName": "Apartment search",
      "createdAt": "2026-05-29T11:55:00Z",
      "updatedAt": "2026-05-29T12:00:00Z",
      "lastMessagePreview": "Yes, I found some properties in Alexandria.",
      "lastMessageAt": "2026-05-29T12:00:00Z"
    }
  ]
}
```

Sessions are returned ordered by latest update first.

## Get Chat History

Use this endpoint when opening an existing session.

```http
GET /api/Assistant/sessions/{sessionId}/messages
```

Example:

```http
GET /api/Assistant/sessions/11111111-2222-3333-4444-555555555555/messages
```

### Success Response

```json
{
  "code": "SUCCESS",
  "message": "Success",
  "data": [
    {
      "messageId": "11111111-aaaa-bbbb-cccc-111111111111",
      "sessionId": "11111111-2222-3333-4444-555555555555",
      "role": "user",
      "content": "hello, are there any properties in alexandria?",
      "imagePaths": [],
      "createdAt": "2026-05-29T11:59:00Z"
    },
    {
      "messageId": "22222222-aaaa-bbbb-cccc-222222222222",
      "sessionId": "11111111-2222-3333-4444-555555555555",
      "role": "assistant",
      "content": "Yes, I found some properties in Alexandria.",
      "imagePaths": [
        "/images/properties/example-1.jpg"
      ],
      "createdAt": "2026-05-29T12:00:00Z"
    }
  ]
}
```

Messages are returned oldest first.

## Rename Session

Use this endpoint when the user renames a chat.

```http
PATCH /api/Assistant/sessions/{sessionId}/name
Content-Type: application/json
```

Request:

```json
{
  "sessionName": "Alexandria apartment search"
}
```

Success response:

```json
{
  "code": "ZZ_ASSISTANT_SESSION_RENAMED_SUCCESSFULLY",
  "message": "Success",
  "data": {
    "sessionId": "11111111-2222-3333-4444-555555555555",
    "sessionName": "Alexandria apartment search",
    "createdAt": "2026-05-29T11:55:00Z",
    "updatedAt": "2026-05-29T12:05:00Z",
    "lastMessagePreview": null,
    "lastMessageAt": null
  }
}
```

## Suggested Frontend State Shape

```ts
type AssistantRole = "user" | "assistant";

type AssistantMessage = {
  messageId?: string;
  sessionId?: string;
  role: AssistantRole;
  content: string;
  imagePaths: string[];
  createdAt: string;
  pending?: boolean;
  failed?: boolean;
};

type AssistantSession = {
  sessionId: string;
  sessionName: string;
  createdAt: string;
  updatedAt: string;
  lastMessagePreview?: string | null;
  lastMessageAt?: string | null;
};
```

For a locally rendered user message before the API responds, `messageId` can be temporary or omitted.

## Example Flow

### Start New Chat

1. User opens chatbot.
2. Frontend has `currentSessionId = null`.
3. User sends `"hello"`.
4. Frontend renders local user message.
5. Frontend calls:

```json
{
  "sessionId": null,
  "content": "hello"
}
```

6. Backend returns a real `sessionId`.
7. Frontend stores that `sessionId` as the current session.
8. Frontend appends the assistant message.

### Continue Existing Chat

1. User selects a session from `/api/Assistant/sessions`.
2. Frontend calls `/api/Assistant/sessions/{sessionId}/messages`.
3. User sends a new message with that same `sessionId`.
4. Frontend appends the assistant response.

## Error Handling Notes

Common cases:

- `401`: user is not logged in or token is invalid.
- `403`: account is banned.
- `404`: session does not exist or does not belong to the current user.
- `400`: invalid content, empty session name, or AI response failure.
- `500`: unexpected server/database/config issue.

For `POST /messages`, if the assistant response fails, the user's message may already be saved. The frontend should not assume the whole send operation was rolled back.

## Curl Examples

### New Chat

```bash
curl -X POST "http://localhost:5254/api/Assistant/messages" \
  -H "Authorization: Bearer {token}" \
  -H "Accept-Language: en" \
  -H "Content-Type: application/json" \
  -d "{\"content\":\"hello, are there any properties in alexandria?\"}"
```

### Existing Chat

```bash
curl -X POST "http://localhost:5254/api/Assistant/messages" \
  -H "Authorization: Bearer {token}" \
  -H "Accept-Language: en" \
  -H "Content-Type: application/json" \
  -d "{\"sessionId\":\"11111111-2222-3333-4444-555555555555\",\"content\":\"show cheaper options\"}"
```

### Sessions

```bash
curl "http://localhost:5254/api/Assistant/sessions" \
  -H "Authorization: Bearer {token}" \
  -H "Accept-Language: en"
```

### Messages

```bash
curl "http://localhost:5254/api/Assistant/sessions/11111111-2222-3333-4444-555555555555/messages" \
  -H "Authorization: Bearer {token}" \
  -H "Accept-Language: en"
```
