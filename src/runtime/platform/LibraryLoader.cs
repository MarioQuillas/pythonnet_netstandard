using System;

namespace Python.Runtime.Platform
{
    static class LibraryLoader
    {
        public static ILibraryLoader Get(OperatingSystemType os)
        {
            switch (os)
            {
                case OperatingSystemType.Windows:
                    return new WindowsLoader();
                case OperatingSystemType.Darwin:
                    return new DarwinLoader();
                case OperatingSystemType.Linux:
                    return new LinuxLoader();
                default:
                    throw new PlatformNotSupportedException($"This operating system ({os}) is not supported");
            }
        }
    }
}
