using DevMarathon.Domain.Enums;

namespace DevMarathon.Infrastructure.ServiceImpls.FileHelperImpl;

public interface IImageHelper
{
    byte[] Compress(byte[] bytes, CompresstionLevel level);
}