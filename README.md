# Speeron Guest Portal Automated Tests

## Features
- Menu item validation in **TV & Mobile preview**
- Full menu **navigation test**
- Markdown documentation included in `TestCases/`

## Setup & Run

### 1. Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) (for Playwright CLI)
- Run the following once to install Playwright browsers:
  ```bash
  playwright install
  ```

### 2. Navigate to project folder
```bash
cd Speeron
```

### 3. Run tests
```bash
dotnet test
```

---

## 📁 Folder Structure
```
Speeron/
├── Speeron.sln
├── Images/
│   └── kot.jpg
├── SpeeronPage/
│   ├── SubPages/
│   ├── Tests/
├── TestCases/
│   ├── E2E_001_MenuValidation.md
│   └── E2E_002_NavigationTest.md
├── Config.cs
├── BaseTest.cs
└── README.md
```

---

## Authentication
Make sure your token is valid in `Config.cs`:
```csharp
public static string AuthToken => "your-valid-token";
```

---

## Tech Stack
- Playwright for .NET (Browser Automation)
- NUnit (Test Framework)

---

## 👥 Access
Grant read-only access to your private GitHub repo for:
- `karol-gro`
- `tomaszzarzeczny-speeron`

---

## 📎 Notes
- `kot.jpg` must be present in `/Images/` and marked as **Copy if newer** in file properties.
- Tests run in **headful mode** (UI visible).
- Recommended to run in **staging** or **test** environment.
- Ensure UI elements haven't changed (keep selectors updated).

---
