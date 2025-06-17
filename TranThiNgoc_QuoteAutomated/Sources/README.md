# Quote Solution - Test Execution Guide

## Overview

This project includes acceptance tests for the Quote service, built with:
- .NET 8.0
- xUnit
- Reqnroll (SpecFlow-compatible)

## Prerequisites

Ensure you have the following installed:

- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- A code editor: VS Code

---

## Folder Structure

Quote.Solution/
├── Models/
├── Quote/
├── QuoteService/
├── QuoteAcceptanceTests/
│ ├── StepDefinitions/
│ └── Features/


---

## How to Run the Tests

1. **Navigate to project root:**

```bash
1. Open terminal and move into solution directory:
cd Sources/Quote.Solution

2. Start the API service (QuoteService)
dotnet run --project ./Quote/Quote.csproj                                                                   


3. Build the solution:
dotnet build

4. Open a new terminal tab/window, then Run all acceptance tests:
cd Sources/Quote.Solution
dotnet test ./QuoteAcceptanceTests/QuoteAcceptanceTests.csproj

You should see an output like: Passed!  - Passed: 17, Failed: 1, Skipped: 0

