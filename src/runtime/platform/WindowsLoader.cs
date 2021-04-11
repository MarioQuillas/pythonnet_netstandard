namespace Python.Runtime.Platform
{
    using System;
    using System.ComponentModel;
    using System.Runtime.InteropServices;

    class WindowsLoader : ILibraryLoader
    {
        private const string NativeDll = "kernel32.dll";


        public IntPtr Load(string dllToLoad)
        {
            var res = WindowsLoader.LoadLibrary(dllToLoad);
            if (res == IntPtr.Zero)
                throw new DllNotFoundException($"Could not load {dllToLoad}", new Win32Exception());
            return res;
        }

        public IntPtr GetFunction(IntPtr hModule, string procedureName)
        {
            var res = WindowsLoader.GetProcAddress(hModule, procedureName);
            if (res == IntPtr.Zero)
                throw new MissingMethodException($"Failed to load symbol {procedureName}", new Win32Exception());
            return res;
        }

        public void Free(IntPtr hModule) => WindowsLoader.FreeLibrary(hModule);

        [DllImport(NativeDll, SetLastError = true)]
        static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport(NativeDll, SetLastError = true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        [DllImport(NativeDll)]
        static extern bool FreeLibrary(IntPtr hModule);
    }
}
