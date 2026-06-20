namespace TechMoveGLMS.Services
{
    public interface IFileValidationService
    {
        bool IsValidPdfFile(string fileName, byte[] fileContent);
        string ValidateFile(string fileName, byte[] fileContent);
    }
}