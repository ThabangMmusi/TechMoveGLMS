using System.Text;

namespace TechMoveGLMS.Services
{
    public class FileValidationService : IFileValidationService
    {
        private static readonly string[] AllowedExtensions = { ".pdf" };
        private const int MaxFileSize = 10 * 1024 * 1024; // 10MB

        public bool IsValidPdfFile(string fileName, byte[] fileContent)
        {
            var validationResult = ValidateFile(fileName, fileContent);
            return validationResult == "Valid";
        }

        public string ValidateFile(string fileName, byte[] fileContent)
        {
            if (string.IsNullOrEmpty(fileName))
                return "File name cannot be empty";

            if (fileContent == null || fileContent.Length == 0)
                return "File content cannot be empty";

            var extension = Path.GetExtension(fileName).ToLower();

            if (!AllowedExtensions.Contains(extension))
                return $"Only {string.Join(", ", AllowedExtensions)} files are allowed. Uploaded: {extension}";

            if (fileContent.Length > MaxFileSize)
                return $"File size cannot exceed {MaxFileSize / 1024 / 1024}MB. Current size: {fileContent.Length / 1024 / 1024}MB";

            // Check PDF header (%PDF)
            if (fileContent.Length >= 4)
            {
                var header = Encoding.ASCII.GetString(fileContent.Take(4).ToArray());
                if (header != "%PDF")
                    return "Invalid PDF file format - File doesn't appear to be a valid PDF";
            }

            return "Valid";
        }
    }
}
