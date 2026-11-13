# Professional Learning Portal — Syncfusion Blazor .NET 10 POC

A proof-of-concept web application demonstrating how **Syncfusion Blazor components** running on **.NET 10** can replace an existing website for an online professional-learning provider serving educators and staff.

## Who it serves

The portal covers the four user groups from the migration requirements:

| User group | Page | Syncfusion components |
|---|---|---|
| **Content Team** | Content Management | `SfRichTextEditor` (advanced toolbar, image upload & browse, tables, links, fonts, colors), `SfGrid`, `SfTextBox`, `SfDropDownList`, `SfButton` |
| **Learners & Professors** | Learning Content | `SfRichTextEditor` (simplified toolbar) + server-side HTML-to-PDF (Blink engine) |
| **Client Managers** | Reports | `SfGrid` (filtering, paging, sorting), `SfChart`, `SfAccumulationChart`, `SfPivotView` |
| **Administrative Team** | Course Scheduling, Query Builder | `SfSchedule` (Day/Week/Month, CRUD, recurrence), `SfQueryBuilder` wired to `SfGrid` |

A shared **Dashboard** page gives an at-a-glance overview with KPI cards and charts.

## Prerequisites

- **.NET 10 SDK** (or newer SDK that can target `net10.0` — the SDK 11.0.100 used during development works)
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
| `Syncfusion.Blazor.Inputs` / `Buttons` / `Dropdowns` / `Popups` / `Navigations` / `Lists` / `Data` / `Spinner` / `SplitButtons` / `Calendars` / `Layouts` | Support components |
| `Syncfusion.Blazor.Themes` | `bootstrap5.css` theme |

All at version **35.1.46**

## Setup & run

From this folder:

```bash
dotnet build
dotnet run
```
With the default launch profile the app listens on `http://localhost:5083` (and `https://localhost:7288`).

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
- **Export to PDF** uses the RTE's built-in `ExportPdf` toolbar command (`RichTextEditorExportPdf`), which calls the Syncfusion-hosted service URL configured on the page. The server needs outbound internet access, and editor content is sent to that service. For production or sensitive content, point `ServiceUrl` at a self-hosted service.

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

