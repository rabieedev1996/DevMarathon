using DevMarathon.Domain.Enums;

namespace DevMarathon.Application.Contract.Services;

public interface IImageService:IFileService
{
     void Compress(CompresstionLevel level);
     public void Resize(int width, int height);
     public void Resize(double factor);
}