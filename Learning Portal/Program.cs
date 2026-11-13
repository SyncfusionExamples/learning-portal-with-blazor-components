using LearningPortal.Components;
using LearningPortal.Services;
using Syncfusion.Blazor;

// Register the Syncfusion license
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("YOUR LICENSE KEY");

var builder = WebApplication.CreateBuilder(args);

// Syncfusion Blazor component registration (registers all Sf* components).
builder.Services.AddSyncfusionBlazor();

// Application sample-data services (singleton keeps in-memory CRUD state for the demo).
builder.Services.AddSingleton<SampleDataService>();

// Images uploaded through the Rich Text Editor are stored under wwwroot/uploads
// so they are served back as static files.
builder.Services.AddSingleton<ImageStorageService>();

// HttpClient for server-side calls back to our own endpoints (PDF export).
builder.Services.AddHttpClient("portal", client => { });

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// --- Base path (hosting under /learning-portal on Azure with a custom domain) ---
// Strips the /learning-portal prefix from incoming requests so Blazor routes,
// MapRazorComponents and MapStaticAssets all resolve correctly. The matching
// <base href="/learning-portal/"> is set in Components/App.razor.
const string basePath = "/learning-portal";

// Requests outside the base path (e.g. https://host/) would load the page with
// <base href="/learning-portal/"> but a mismatching URL, which makes the Blazor
// circuit fail to initialize. Redirect them into the base path instead.
app.Use(async (context, next) =>
{
    if (!context.Request.Path.StartsWithSegments(basePath, StringComparison.OrdinalIgnoreCase))
    {
        context.Response.Redirect($"{basePath}{context.Request.Path}{context.Request.QueryString}");
        return;
    }
    await next();
});

app.UsePathBase(basePath);

// Explicit routing AFTER UsePathBase: WebApplication auto-inserts UseRouting at
// the very start of the pipeline, which would match routes against the full
// /learning-portal/... path before the prefix is stripped (causing 405 on
// _blazor/negotiate). Calling UseRouting here pins it to the right position.
app.UseRouting();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}
//app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();

// MapStaticAssets only serves files known at build time. Images uploaded at
// runtime (wwwroot/uploads) must be served by the regular static files middleware.
app.UseStaticFiles();

// --- Rich Text Editor image upload endpoint (SaveUrl) ---------------------
// Receives multipart/form-data posted by the RTE image uploader, stores the
// file on disk and returns the file metadata the editor expects.
app.MapPost("/api/upload/rte-image", async (HttpContext ctx, ImageStorageService storage) =>
{
    if (!ctx.Request.HasFormContentType || ctx.Request.Form.Files.Count == 0)
        return Results.BadRequest(new { error = "No file received." });

    var file = ctx.Request.Form.Files[0];
    var (ok, urlOrError) = await storage.SaveRteImageAsync(file);
    return ok
        ? Results.Ok(new { name = file.FileName, url = urlOrError })
        : Results.BadRequest(new { error = urlOrError });
}).DisableAntiforgery();



app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
