# C# WinForms Classes

Reusable OOP helper classes for .NET Framework 4.7.2 Windows Forms applications.

## Start here

1. Read [docs/guide.md](docs/guide.md) for the complete Markdown setup, CRUD, method reference, helpers, and implementation guide.
2. Open [index.html](index.html) for the same guide in a browser.
3. Copy the files in `Classes/` into a WinForms project's Classes folder.
4. Run [docs/schema.sql](docs/schema.sql) for the sample database.
5. Open `Sample Project/OOP_Framework.sln` to see CRUD, reusable control helpers, charts, report preview, and printing.

## Included

- SQL Server access through `AppDb.Instance` and `IDatabase`
- Control helpers that accept WinForms `Control` instances
- Built-in chart binding with column, line, and pie series
- Paginated report preview and printing through `PrintDocument`
- A package-free .NET Framework test harness under `Tests/`

The library adds no NuGet dependencies. The sample uses SQL Server LocalDB and requires a configured `AppDB` connection string.
