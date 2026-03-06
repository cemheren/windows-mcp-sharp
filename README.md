# 🖥️ Windows MCP

A lightweight **Model Context Protocol (MCP) server** for interacting with the Windows desktop — enabling AI agents like Claude to see, click, type, and operate your PC on your behalf.

Built with .NET 9 and native Windows UI Automation, this server exposes **21 tools** that give MCP-compatible clients full desktop automation capabilities: from pixel-perfect mouse clicks to file management, registry access, and more.

---

## ✨ Features

- **Desktop Automation** — Click, type, scroll, drag, and send keyboard shortcuts at exact screen coordinates
- **Accessibility Tree** — Traverses the full UI hierarchy to find interactive elements with bounding-box coordinates
- **Screenshot Capture** — Captures and annotates desktop screenshots with element overlays via SkiaSharp
- **Browser DOM Extraction** — Extracts visible text from browser tabs through the accessibility tree
- **File System Operations** — Read, write, copy, move, delete, search, and inspect files and directories
- **System Control** — Manage processes, clipboard, registry, toast notifications, and more
- **Virtual Desktops** — Detects and monitors Windows 10+ virtual desktops via COM
- **Event Monitoring** — Real-time tracking of focus changes, structure updates, and property modifications

---

## 🛠️ Tools

### Desktop & Input

| Tool | Description |
|------|-------------|
| **Snapshot** | Capture complete desktop state — windows, interactive elements with coordinates, scrollable areas. Optionally includes an annotated screenshot. |
| **Click** | Mouse click at `[x, y]`. Supports left/right/middle button, single/double click, and hover. |
| **Type** | Type text at coordinates. Options to clear existing text, set caret position, and press Enter. |
| **Scroll** | Scroll vertically or horizontally at a location by wheel increments. |
| **Move** | Move mouse cursor; set `drag=true` for drag-and-drop. |
| **Shortcut** | Execute keyboard shortcuts (e.g. `ctrl+c`, `alt+tab`, `win+r`). |
| **App** | Launch, resize, or switch to applications by name. |
| **WindowHandles** | List all visible window handles and titles. |
| **Wait** | Pause execution for a specified duration. |

### Multi-Action

| Tool | Description |
|------|-------------|
| **MultiSelect** | Ctrl+click multiple items or perform batch clicks. |
| **MultiEdit** | Enter text into multiple input fields at once. |

### File System

| Tool | Description |
|------|-------------|
| **FileSystem** | 8 modes: `read`, `write`, `copy`, `move`, `delete`, `list`, `search`, `info`. Supports line offsets, recursive operations, and glob patterns. |

### Web

| Tool | Description |
|------|-------------|
| **Scrape** | Fetch a URL as markdown, or extract DOM text from the active browser tab. |

### System

| Tool | Description |
|------|-------------|
| **Clipboard** | Get or set Windows clipboard content. |
| **Process** | List running processes (filter/sort by memory, CPU, name) or kill by PID. |
| **SystemInfo** | CPU, memory, disk, network stats, and uptime. |
| **Notification** | Send Windows toast notifications. |
| **LockScreen** | Lock the workstation. |
| **Registry** | Read, write, delete, and list Windows Registry keys and values. |
| **PowerShell** | Execute arbitrary PowerShell commands. |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- Windows 10 or later

### Build

```bash
dotnet build WindowsMcp.sln -c Release
```

### Run

```bash
dotnet run --project src/WindowsMcp/WindowsMcp.csproj
```

The server starts in **stdio** transport mode by default, communicating over stdin/stdout using the MCP protocol.

### CLI Options

```
--transport <stdio|sse|streamable-http>   Transport mode (default: stdio)
--host <hostname>                         Host for HTTP transports (default: localhost)
--port <port>                             Port for HTTP transports (default: 8000)
```

> **Note:** Only `stdio` transport is currently supported. SSE and streamable-http are planned.

---

## ⚙️ Configuration

### MCP Client Setup

Add this to your MCP client configuration (e.g. Claude Desktop, GitHub Copilot):

```json
{
  "mcpServers": {
    "windows-mcp": {
      "command": "dotnet",
      "args": ["run", "--project", "C:\\path\\to\\src\\WindowsMcp\\WindowsMcp.csproj"]
    }
  }
}
```

Or if using a published build:

```json
{
  "mcpServers": {
    "windows-mcp": {
      "command": "C:\\path\\to\\WindowsMcp.exe"
    }
  }
}
```

---

## 🏗️ Project Structure

```
windows-mcp-sharp/
├── src/WindowsMcp/
│   ├── Program.cs              # Entry point, CLI args, MCP tool definitions
│   ├── Desktop/                # Desktop automation (click, type, screenshot, state)
│   ├── Tree/                   # UI Automation tree traversal and caching
│   ├── Uia/                    # COM interface definitions and P/Invoke declarations
│   ├── Vdm/                    # Virtual desktop management
│   ├── FileSystem/             # File system operations
│   ├── Hints/                  # Interactive element hint labeling
│   ├── Watchdog/               # Real-time UI event monitoring (STA thread)
│   └── Auth/                   # Remote mode authentication
├── tests/WindowsMcp.Tests/     # Unit tests
└── WindowsMcp.sln
```

---

## 🧪 Tests

```bash
dotnet test WindowsMcp.sln
```

---

## 📦 Key Dependencies

| Package | Purpose |
|---------|---------|
| [ModelContextProtocol](https://www.nuget.org/packages/ModelContextProtocol) | MCP server framework |
| [SkiaSharp](https://www.nuget.org/packages/SkiaSharp) | Screenshot capture and image annotation |
| [ReverseMarkdown](https://www.nuget.org/packages/ReverseMarkdown) | HTML-to-Markdown conversion for web scraping |
| [FuzzySharp](https://www.nuget.org/packages/FuzzySharp) | Fuzzy string matching |
| [System.CommandLine](https://www.nuget.org/packages/System.CommandLine) | CLI argument parsing |

---

## 🤝 Contributing

Contributions are welcome! Please open an issue or submit a pull request.

---

## 📄 License

This project is provided as-is. See the repository for license details.
