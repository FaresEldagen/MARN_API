# Assistant Chatbot Image Paths Update for AI

This is an update to the assistant chatbot integration. The AI service can now return image paths with its final assistant response.

## What Changed

Assistant messages now support optional image paths.

New database column:

```text
assistantMessages.ImagePathsJson
```

Column details:

| Column | Type | Notes |
| --- | --- | --- |
| `ImagePathsJson` | `nvarchar(max)` | Nullable JSON array of image path strings. |

Text-only messages can leave this column as `NULL`.

## Backend-to-AI Request

No change.

The backend still calls the AI service with only:

```json
{
  "sessionId": "11111111-2222-3333-4444-555555555555"
}
```

## AI-to-Backend Response

The AI service can still return plain text:

```text
I found a few properties in Alexandria.
```

It can also still return a JSON string:

```json
"I found a few properties in Alexandria."
```

New supported format:

```json
{
  "content": "I found a few properties in Alexandria.",
  "imagePaths": [
    "/images/properties/example-1.jpg",
    "/images/properties/example-2.jpg"
  ]
}
```

The backend also accepts `message` or `response` instead of `content`:

```json
{
  "message": "I found a few properties in Alexandria.",
  "imagePaths": [
    "/images/properties/example-1.jpg"
  ]
}
```

## Image Path Rules

The backend filters `imagePaths` before saving.

Accepted paths:

- non-empty strings;
- start with `/`;
- relative/static backend paths, for example `/images/...`;
- max 20 paths.

Rejected paths:

- external URLs like `https://example.com/image.jpg`;
- Windows paths like `C:\images\a.jpg`;
- paths containing backslashes `\`;
- paths containing `..`;
- empty strings;
- more than 20 paths.

If some paths are invalid, the backend saves only the valid ones.

If `content` is empty or missing, the response is treated as an AI failure and no assistant message is saved.

## How It Is Stored

When the AI returns:

```json
{
  "content": "Here are some options.",
  "imagePaths": [
    "/images/properties/a.jpg",
    "/images/properties/b.jpg"
  ]
}
```

The backend stores:

```text
assistantMessages.Role = assistant
assistantMessages.ToolOnly = false
assistantMessages.Content = Here are some options.
assistantMessages.ImagePathsJson = ["/images/properties/a.jpg","/images/properties/b.jpg"]
```

For text-only assistant messages:

```text
assistantMessages.ImagePathsJson = NULL
```

## Reading History From DB

When reading assistant history directly, include the new column:

```sql
SELECT MessageId, UserId, SessionId, Role, ToolOnly, Content, ImagePathsJson, CreatedAt
FROM assistantMessages
WHERE SessionId = @sessionId
ORDER BY CreatedAt ASC;
```

For visible user-facing messages only:

```sql
SELECT MessageId, UserId, SessionId, Role, Content, ImagePathsJson, CreatedAt
FROM assistantMessages
WHERE SessionId = @sessionId
  AND ToolOnly = 0
ORDER BY CreatedAt ASC;
```

## Recommended AI Behavior

When recommending properties, return:

```json
{
  "content": "I found these properties that match your request.",
  "imagePaths": [
    "/images/properties/property-1-main.jpg",
    "/images/properties/property-2-main.jpg"
  ]
}
```

Only include paths that already exist in the backend/static files or property media records.

Do not return private local file paths or generated external links.

## Frontend Response Shape

The backend now returns `imagePaths` to the frontend:

```json
{
  "sessionId": "11111111-2222-3333-4444-555555555555",
  "assistantMessage": {
    "messageId": "aaaaaaaa-bbbb-cccc-dddd-eeeeeeeeeeee",
    "sessionId": "11111111-2222-3333-4444-555555555555",
    "role": "assistant",
    "content": "I found these properties that match your request.",
    "imagePaths": [
      "/images/properties/property-1-main.jpg"
    ],
    "createdAt": "2026-05-29T12:00:00Z"
  }
}
```

If there are no images, frontend receives:

```json
"imagePaths": []
```
