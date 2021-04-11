namespace Python.Runtime.Platform
{
    using System;

    interface ILibraryLoader
    {
        IntPtr Load(string dllToLoad);

        IntPtr GetFunction(IntPtr hModule, string procedureName);

        void Free(IntPtr hModule);
    }
}