using System;
using System.Runtime.InteropServices;

namespace SmScanner
{
    public interface ICoreFunction
    {
        public abstract bool Attach();

        public abstract void Detach();
    }
    public class CoreFunctions32 : ICoreFunction
    {
        [DllImport("Smdkddll32.dll", EntryPoint = "Attach")] // PUBLIC
        private static extern bool attach();

        [DllImport("Smdkddll32.dll", EntryPoint = "Detach")] // PUBLIC
        private static extern void detach();

        public bool Attach() => attach();

        public void Detach() => detach();
    }
    public class CoreFunctions64 : ICoreFunction
    {
        [DllImport("Smdkddll.dll", EntryPoint = "Attach")] // PUBLIC
        private static extern bool attach();

        [DllImport("Smdkddll.dll", EntryPoint = "Detach")] // PUBLIC
        private static extern void detach();

        public bool Attach() => attach();

        public void Detach() => detach();
    }
    //public static class CoreFunctions
    //{
    //    public static ICoreFunction CoreFunction { get; }
    //    static CoreFunctions()
    //    {
    //        if (Environment.Is64BitProcess)
    //            CoreFunction = new CoreFunctions64();
    //        else
    //            CoreFunction = new CoreFunctions32();
    //    }
    //}
}
