using Xunit;
using TechMoveGLMS.Services;

namespace TechMoveGLMS.Tests
{
    public class FileValidationServiceTests
    {
        private readonly FileValidationService _service;

        public FileValidationServiceTests()
        {
            _service = new FileValidationService();
        }

        [Fact]
        public void ValidateFile_ValidPdf_ReturnsValid()
        {
            // Arrange
            string fileName = "test.pdf";
            byte[] content = System.Text.Encoding.ASCII.GetBytes("%PDF-1.5");

            // Act
            var result = _service.ValidateFile(fileName, content);

            // Assert
            Assert.Equal("Valid", result);
        }

        [Fact]
        public void ValidateFile_InvalidExtension_ReturnsErrorMessage()
        {
            // Arrange
            string fileName = "test.exe";
            byte[] content = new byte[100];

            // Act
            var result = _service.ValidateFile(fileName, content);

            // Assert
            Assert.Contains("Only .pdf files are allowed", result);
        }

        [Fact]
        public void ValidateFile_FileTooLarge_ReturnsErrorMessage()
        {
            // Arrange
            string fileName = "large.pdf";
            byte[] content = new byte[11 * 1024 * 1024]; // 11MB (Max is 10MB)

            // Act
            var result = _service.ValidateFile(fileName, content);

            // Assert
            Assert.Contains("exceed 10MB", result);
        }
    }
}
