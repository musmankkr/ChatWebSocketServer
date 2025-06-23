# ChatWebSocketServer

A real-time chatbot server built with **ASP.NET Core**, using **WebSockets** to communicate with a frontend and **Dialogflow ES** to handle natural language understanding.

---

## Features

- Real-time chat via WebSocket
- Static frontend UI (`index.html`) served from `wwwroot/`
- Dialogflow ES integration using service account authentication
- HTTP-only setup (no HTTPS)
- Clean project structure with Dependency Injection

---

## Project Structure

```
ChatWebSocketServer/
│
├── wwwroot/                # Static files (index.html)
├── Services/               # DialogflowService.cs
├── Handlers/               # WebSocketHandler.cs
├── Configurations/         # DialogflowSettings.cs
├── Program.cs              # App entry point
├── appsettings.json        # Configuration file
└── README.md               # Project info
```

---

## Prerequisites

- [.NET SDK 8.0](https://dotnet.microsoft.com/download/dotnet/8.0)
- A **Dialogflow ES Agent** set up in your Google Cloud project
- A **service account JSON key file**

---

## Dialogflow Setup

1. Go to [Dialogflow Console](https://dialogflow.cloud.google.com/)
2. Create or use an existing agent
3. Create a service account in Google Cloud with the role:
   - `Dialogflow API Client`
4. Download the JSON key and place it in your project root:
   - e.g., `optimum-shape-349908-xxxxxx.json`
5. Configure `appsettings.json`:

```json
"Dialogflow": {
  "ProjectId": "optimum-shape-349908",
  "LanguageCode": "en",
  "CredentialsPath": "optimum-shape-349908-xxxxxx.json"
}
```

---

## Running the App

```bash
dotnet run
```

Once running:

- 🔗 Frontend UI: [http://localhost:5214/index.html](http://localhost:5214/index.html)
- 🔌 WebSocket endpoint: `ws://localhost:5214/ws`

---

## Developer Notes

- The app runs **on HTTP only**
- Ensure `app.UseHttpsRedirection()` is **not** in `Program.cs`
- WebSocket handler and Dialogflow service are injected via **Dependency Injection**
- `launchSettings.json` configures the app to start on port `5214` over HTTP

---

## Example Message Flow

1. User types a message in the chat UI
2. Message is sent via WebSocket to the backend
3. Backend sends it to Dialogflow
4. Dialogflow returns a response
5. Response is sent back to the frontend via WebSocket

