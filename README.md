# Professional Learning Portal — Syncfusion Blazor .NET 10 POC

A proof-of-concept web application demonstrating how **Syncfusion Blazor components** running on **.NET 10** can replace an existing ASP.NET Web Forms (.NET Framework 4.8) website for an online professional-learning provider serving educators and staff.

## Who it serves

The portal covers the four user groups from the migration requirements:

| User group | Page | Syncfusion components |
|---|---|---|
| **Content Team** | Content Management | `SfRichTextEditor` (advanced toolbar, image upload & browse, tables, links, fonts, colors), `SfGrid`, `SfTextBox`, `SfDropDownList`, `SfButton` |
| **Learners & Professors** | Learning Content | `SfRichTextEditor` (simplified toolbar) + server-side HTML-to-PDF (Blink engine) |
| **Client Managers** | Reports | `SfGrid` (filtering, paging, sorting), `SfChart`, `SfAccumulationChart`, `SfPivotView` |
| **Administrative Team** | Course Scheduling, Query Builder | `SfSchedule` (Day/Week/Month, CRUD, recurrence), `SfQueryBuilder` wired to `SfGrid` |

A shared **Dashboard** page gives an at-a-glance overview with KPI cards and charts.

## Project structure

```
LearningPortal.sln
└── src/LearningPortal/
    ├── Program.cs                  # DI, endpoints (image upload, HTML→PDF), middleware
    ├── Models/CourseModels.cs      # Course, Enrollment, CourseSectionEvent, LearningContentItem, …
    ├── Services/
    │   ├── SampleDataService.cs    # In-memory realistic seed data + CRUD
    │   ├── ImageStorageService.cs  # RTE image upload storage (wwwroot/uploads)
    │   └── PdfExportService.cs     # Syncfusion Blink HTML-to-PDF conversion
    ├── Components/
    │   ├── App.razor               # Host page — Syncfusion bootstrap5 theme + download helper
    │   ├── Routes.razor
    │   ├── _Imports.razor          # Syncfusion namespaces
    │   ├── Layout/MainLayout.razor # Responsive dashboard shell (sidebar + topbar)
    │   └── Pages/
    │       ├── Home.razor              # Dashboard (KPIs, charts, recent enrollments)
    │       ├── ContentManagement.razor # Advanced RTE + article library grid
    │       ├── LearningContent.razor   # Basic RTE + PDF export + drafts
    │       ├── Reports.razor           # Grid + pie/bar charts + pivot table
    │       ├── CourseScheduling.razor  # Schedule with CRUD + recurrence
    │       └── QueryBuilderPage.razor  # Query Builder → filtered grid
    └── wwwroot/
        ├── app.css                # Shared page styles
        └── uploads/               # RTE-uploaded images (created at runtime)
```

## Prerequisites

- **.NET 10 SDK** (or newer SDK that can target `net10.0` — the SDK 11.0.100 used during development works)
- **Windows** (the HTML-to-PDF converter ships Windows Blink binaries)
- A Syncfusion Blazor license key (free community license available) — see *Licensing* below

### NuGet packages (Syncfusion 35.1.46)

Since version 35, Syncfusion ships **modular packages** instead of the monolithic `Syncfusion.Blazor` package. This project references only the modules it uses:

| Package | Used for |
|---|---|
| `Syncfusion.Blazor.Core` | Base component infrastructure & `AddSyncfusionBlazor()` |
| `Syncfusion.Blazor.Grid` | Dashboard & reports grids, query-builder results |
| `Syncfusion.Blazor.Charts` | Dashboard & reports charts |
| `Syncfusion.Blazor.PivotTable` | Pivot analysis report |
| `Syncfusion.Blazor.Schedule` | Course scheduling calendar |
| `Syncfusion.Blazor.QueryBuilder` | Dynamic query building |
| `Syncfusion.Blazor.RichTextEditor` | Both rich text editors |
| `Syncfusion.Blazor.Inputs` / `Buttons` / `Dropdowns` / `Popups` / `Navigations` / `Lists` / `Data` / `Spinner` / `SplitButtons` / `Calendars` / `Layouts` | Support infrastructure |
| `Syncfusion.HtmlToPdfConverter.Net.Windows` | Server-side HTML→PDF (Blink engine) |

All at version **35.1.46**, declared in `src/LearningPortal/LearningPortal.csproj`.

## Setup & run

```bash
cd "D:/POC/Blazor NET10"
dotnet build src/LearningPortal

dotnet run --project src/LearningPortal --urls "http://localhost:5080"
```

Then open **http://localhost:5080**.

> **Note:** The first `dotnet build` copies the Blink rendering-engine binaries to `bin/…/runtimes/win-x64/native` via a NuGet build target. The PDF export requires these; if PDF export reports missing binaries, rebuild once.

### Syncfusion license key (required for production, optional in development)

1. Get a key from [syncfusion.com](https://syncfusion.com) (free Community license if you qualify).
2. Register it in `Program.cs` **before** `builder.Services.AddSyncfusionBlazor()`:

```csharp
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR_LICENSE_KEY");
```

Without a key the components still render in development, but pages show a license banner.

## Feature walkthrough

### 1. Dashboard (`/`)
KPI cards (total enrollments, completions, active courses, average progress), a column/line combo chart of monthly enrollments vs completions, a course-completion progress chart, and a recent-enrollment grid — all fed from the seeded sample data.

### 2. Content Management (`/content-management`) — Content Team
- **Article library grid** — select a row to load an article into the editor.
- **Advanced RTE toolbar** — bold/italic/underline/strikethrough, font name/size, font & background colors, formats, alignments, ordered/bulleted lists, tables, hyperlinks, images, source-code view, clear-format, undo/redo, full screen.
- **Image upload & browse** — images POST to `/api/upload/rte-image`, are stored in `wwwroot/uploads/` (5 MB limit, extension allow-list), and are served back as static files for browsing/insertion.
- **Quick toolbar on images** — replace, align, caption, alt text, dimension, link, display, remove.
- Article details (title, author, status) save back into the in-memory service — full CRUD without a database, ready to swap for EF Core.
- **Extensibility** — additional toolbar items can be appended to `AdvancedToolbar` (`List<ToolbarItemModel>`) or a custom `RichTextEditorToolbarSettings` item without touching other code.

### 3. Learning Content (`/learning-content`) — Learners & Professors
- **Simplified RTE toolbar** — bold, italic, underline, formats, alignments, lists, link, image, table, undo/redo, clear format.
- **Save Draft** — stores content drafts in the sample-data service; click a draft to reload it into the editor.
- **Download as PDF** — the editor HTML is POSTed to `/api/export/html-to-pdf`; the Syncfusion Blink engine renders it server-side into a pixel-perfect PDF that preserves formatting, images, and tables (verified: tables render, uploaded images embed as PDF image XObjects). The file downloads via a small JS helper (`lpDownloadFile`).

### 4. Reports (`/reports`) — Client Managers
Three switchable views:
- **Enrollment Detail** — `SfGrid` with menu filtering, sorting, and paging over the enrollment dataset.
- **Graphical Reports** — pie chart of enrollments by category and bar chart of average progress by category.
- **Pivot Analysis** — `SfPivotView` aggregating average progress and completion counts by district (rows) and category (columns).

### 5. Course Scheduling (`/course-scheduling`) — Administrative Team
- `SfSchedule` with **Day, Week, and Month** views.
- Create/edit/delete course sections via the built-in editor popup; changes persist through the sample-data service (`OnActionBegin` intercepts create/change/remove).
- **Recurrence** — the editor popup supports repeat rules; seeded events include weekly-recurring course sections (e.g. `FREQ=WEEKLY;BYDAY=MO,WE;COUNT=12`).
- Fields mapped: subject (course section), location (room/platform), description (notes), start/end, all-day, recurrence.

### 6. Query Builder (`/query-builder`) — Administrative Team
- `SfQueryBuilder` over the enrollment dataset with fields for learner, course, category, instructor, district, progress %, enrolled date, and completed (typed string/number/date/boolean columns → appropriate operators).
- **AND/OR groups** with nested condition groups.
- **Run Query** applies the built predicate (`GetFilteredRecordsAsync`) and streams the filtered rows into the results `SfGrid`; **Reset** clears the query.

## Architecture notes

- **Render mode:** Blazor Web App with Interactive Server render mode (`@rendermode InteractiveServer` per page) — a natural Web Forms migration path since state lives on the server.
- **Dependency injection:** `SampleDataService` (singleton — in-memory CRUD state), `ImageStorageService`, `PdfExportService`, and a named `HttpClient` for internal calls.
- **Realistic sample data:** 10 courses, 48 enrollments across 8 districts, seeded article library, and ~10 calendar events including recurring series — all in `SampleDataService`, swappable for a real data layer.
- **Responsive design:** the sidebar collapses to a slide-over drawer under 960px; grid/card layouts are fluid.
- **Security:** upload endpoint enforces extension allow-list + 5 MB size cap; minimal APIs return typed `Results` responses; antiforgery stays on for Razor components.

## Migration mapping (Web Forms → Blazor)

| Web Forms pattern | Blazor / Syncfusion replacement |
|---|---|
| ASPX page + code-behind | Razor component with `@code` block |
| `<asp:GridView>` | `SfGrid` |
| `<asp:Calendar>` | `SfSchedule` |
| Custom HTML editor ( Telerik / CKEditor ) | `SfRichTextEditor` |
| SSRS / RDLC reports | `SfChart`, `SfPivotView`, `SfGrid` exports |
| Postback events | Blazor EventCallbacks (`GridEvents`, `ScheduleEvents`) |
| Session state | DI services (scoped/singleton) |
| Master pages | `MainLayout` + nested layouts |

## Verified in this build

- All six pages return HTTP 200 and render their Syncfusion components.
- HTML→PDF endpoint produces valid PDFs (`%PDF` header, `%%EOF` trailer), including embedded images (image XObject present) and tables.
- Image upload endpoint stores files, returns editor-compatible JSON, and files are served back at `/uploads/…`.
- Full solution builds with **0 warnings, 0 errors** on `net10.0`.
