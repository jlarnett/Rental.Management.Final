namespace Rental.Management.Final.Services.FileExtensionValidator;

public interface IFileExtensionValidator
{
    bool CheckValidImageExtensions(string uploadedFileName);
}
