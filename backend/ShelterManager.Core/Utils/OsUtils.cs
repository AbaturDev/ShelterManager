using System.Runtime.InteropServices;

namespace ShelterManager.Core.Utils;

public static class OsUtils
{
    public static string GetWkhtmlExtension()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return ".dll";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            return ".so";
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
        {
            return ".dylib";
        }
        
        throw new PlatformNotSupportedException("Operating system is not supported.");
    }
}