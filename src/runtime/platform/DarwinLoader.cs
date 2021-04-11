namespace Python.Runtime.Platform
{
    using System;
    using System.Runtime.InteropServices;

    class DarwinLoader : ILibraryLoader
    {
        private static int RTLD_NOW = 0x2;
        private static int RTLD_GLOBAL = 0x8;
        private const string NativeDll = "/usr/lib/libSystem.dylib";
        private static IntPtr RTLD_DEFAULT = new IntPtr(-2);

        public IntPtr Load(string dllToLoad)
        {
            var filename = $"lib{dllToLoad}.dylib";
            this.ClearError();
            var res = dlopen(filename, RTLD_NOW | RTLD_GLOBAL);
            if (res == IntPtr.Zero)
            {
                var err = this.GetError();
                throw new DllNotFoundException($"Could not load {filename} with flags RTLD_NOW | RTLD_GLOBAL: {err}");
            }

            return res;
        }

        public void Free(IntPtr handle)
        {
            dlclose(handle);
        }

        public IntPtr GetFunction(IntPtr dllHandle, string name)
        {
            // look in the exe if dllHandle is NULL
            if (dllHandle == IntPtr.Zero)
            {
                dllHandle = RTLD_DEFAULT;
            }

            this.ClearError();
            IntPtr res = dlsym(dllHandle, name);
            if (res == IntPtr.Zero)
            {
                var err = this.GetError();
                throw new MissingMethodException($"Failed to load symbol {name}: {err}");
            }
            return res;
        }

        void ClearError()
        {
            dlerror();
        }

        string GetError()
        {
            var res = dlerror();
            if (res != IntPtr.Zero)
                return Marshal.PtrToStringAnsi(res);
            else
                return null;
        }

        [DllImport(NativeDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public static extern IntPtr dlopen(String fileName, int flags);

        [DllImport(NativeDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern IntPtr dlsym(IntPtr handle, String symbol);

        [DllImport(NativeDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern int dlclose(IntPtr handle);

        [DllImport(NativeDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr dlerror();
    }
}