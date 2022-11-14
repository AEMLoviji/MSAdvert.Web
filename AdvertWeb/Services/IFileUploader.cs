namespace AdvertWeb.Services;

public interface IFileUploader
{
    Task<bool> UploadFileAsync(string fileName, Stream storageStream);
}
