using System.Security.Cryptography;

namespace MyApp.Common;

public static class GuidFactory
{
    public static Guid CreateVersion7()
    {
        var unixTimeMilliseconds = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var bytes = new byte[16];

        bytes[0] = (byte)((unixTimeMilliseconds >> 40) & 0xFF);
        bytes[1] = (byte)((unixTimeMilliseconds >> 32) & 0xFF);
        bytes[2] = (byte)((unixTimeMilliseconds >> 24) & 0xFF);
        bytes[3] = (byte)((unixTimeMilliseconds >> 16) & 0xFF);
        bytes[4] = (byte)((unixTimeMilliseconds >> 8) & 0xFF);
        bytes[5] = (byte)(unixTimeMilliseconds & 0xFF);

        RandomNumberGenerator.Fill(bytes.AsSpan(6, 10));

        bytes[6] = (byte)((bytes[6] & 0x0F) | 0x70);
        bytes[8] = (byte)((bytes[8] & 0x3F) | 0x80);

        return new Guid(bytes);
    }
}
