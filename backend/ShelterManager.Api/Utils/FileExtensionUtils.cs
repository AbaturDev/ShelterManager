namespace ShelterManager.Api.Utils;

public static class FileExtensionUtils
{
    private static readonly HashSet<string> AllowedImageExtensions = new()
    {
        ".jpg", ".jpeg", ".png", ".webp"
    };
    
    public static string MapToContentType(string extension)
    {
        var contentType = extension switch
        {
            ".txt" => "text/plain",
            ".csv" => "text/csv",
            ".json" => "application/json",
            ".xml" => "application/xml",
            ".pdf" => "application/pdf",
            ".doc" => "application/msword",
            ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            ".xls" => "application/vnd.ms-excel",
            ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            ".ppt" => "application/vnd.ms-powerpoint",
            ".pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            ".jpg" => "image/jpg",
            ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".webp" => "image/webp",
            ".gif" => "image/gif",
            ".mp3" => "audio/mpeg",
            ".wav" => "audio/wav",
            ".mp4" => "video/mp4",
            ".mov" => "video/quicktime",
            _ => "application/octet-stream"
        };

        return contentType;
    }

    public static bool IsValidImageExtension(string extension)
    {
        if (string.IsNullOrWhiteSpace(extension))
        {
            return false;
        }
        
        return AllowedImageExtensions.Contains(extension.ToLowerInvariant());
    }
}