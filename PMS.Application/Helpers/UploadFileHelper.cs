using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace PMS.Application.Helpers;

public class UploadFileHelper(IWebHostEnvironment hostingEnvironment)
{
    public async Task<string> Upload(IFormFile formFile)
    {
        var webRootPath = hostingEnvironment.WebRootPath;

        // Define the relative path within wwwroot to save the file
        const string relativePath = "uploads";

        // Combine the wwwroot path and the relative path to get the upload directory
        var uploadDirectory = Path.Combine(webRootPath, relativePath);

        // Ensure the upload directory exists; create it if not
        if (!Directory.Exists(uploadDirectory))
        {
            Directory.CreateDirectory(uploadDirectory);
        }

        // Define a unique file name or use the original file name
        var uniqueFileName = $"{Guid.NewGuid().ToString()}_{formFile.FileName}";
        var filePath = Path.Combine(uploadDirectory, uniqueFileName);
        var fileUrl = Path.Combine(relativePath, uniqueFileName).Replace(@"\\", "/");
        
        await using var stream = new FileStream(filePath, FileMode.Create);
        await formFile.CopyToAsync(stream);

        return fileUrl;
    }

    public Task<bool> Delete(string path)
    {
        try
        {
            // Check if the file exists before attempting to delete it
            if (File.Exists(path))
            {
                // Delete the file
                File.Delete(path);
                Console.WriteLine("File deleted successfully.");
                return Task.FromResult(true);
            }

            Console.WriteLine("File does not exist.");
            return Task.FromResult(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
                return Task.FromResult(false);
        }
    }

}