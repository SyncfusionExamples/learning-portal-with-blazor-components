using Microsoft.AspNetCore.Http;

namespace LearningPortal.Services;

public class ImageStorageService
{
    private readonly string _root;
    private readonly string[] _allowedExtensions = [".png", ".jpg", ".jpeg", ".gif", ".webp", ".bmp"];
    private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

    public ImageStorageService(IWebHostEnvironment env)
    {
        _root = Path.Combine(env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot"), "uploads");
        Directory.CreateDirectory(_root);
    }

    public async Task<(bool Ok, string Result)> SaveRteImageAsync(IFormFile file)
    {
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_allowedExtensions.Contains(ext))
            return (false, "Only image files (png, jpg, jpeg, gif, webp, bmp) are allowed.");
        if (file.Length == 0 || file.Length > MaxFileSizeBytes)
            return (false, "File size must be between 1 byte and 5 MB.");

        var uniqueName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Guid.NewGuid():N}{ext}";
        var fullPath = Path.Combine(_root, uniqueName);
        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream);
        // Prefix with the app's base path so the RTE image URL resolves correctly
        // when the portal is hosted under /learning-portal.
        return (true, $"/learning-portal/uploads/{uniqueName}");
    }
}
