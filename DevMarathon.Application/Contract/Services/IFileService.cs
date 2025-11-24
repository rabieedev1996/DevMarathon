using DevMarathon.Domain.Enums;

namespace DevMarathon.Application.Contract.Services;

public interface IFileService
{
    void LoadFileFromStorage(string path);
    void LoadFileFromStream(Stream stream, string? fileType = null, string? contentType = null);
    void LoadFileFromByteArray(byte[] byteArray, string? fileType = null, string? contentType = null);
    string SaveToFile(string destinationPath);
    string SaveToAWS(string directory = null);
}