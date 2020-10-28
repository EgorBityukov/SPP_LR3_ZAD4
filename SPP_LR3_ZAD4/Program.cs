using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace SPP_LR3_ZAD4
{
    class Program
    {
        static void Main(string[] args)
        {
            int id;
            OsHandle handle = new OsHandle();
            Process[] processes = Process.GetProcesses();

            foreach (var p in processes)
            {
                Console.WriteLine("Process id: " + p.Id + " Name: " + p.ProcessName.ToString());
            }

            Console.WriteLine("Input id: ");
            id = Convert.ToInt32(Console.ReadLine());

            foreach (var p in processes)
            {
                if (p.Id == id)
                {
                    handle.Handle = p.Handle;
                    Console.WriteLine(p.ProcessName + "Handle count:  " + p.HandleCount);
                    p.Kill();
                }
            }

            handle.Dispose();
            Thread.Sleep(1000);

            try
            {
                Console.WriteLine(Process.GetProcessById(id).HandleCount);
            }
            catch
            {
                Console.WriteLine("Handle close");
            }
        }
    }
    class OsHandle : IDisposable
    {
        [DllImport("Kernel32.dll")]
        private static extern bool CloseHandle(IntPtr handle);

        private bool _disposed = false;
        public IntPtr Handle { get; set; }

        public OsHandle()
        {
            Handle = IntPtr.Zero;
        }

        ~OsHandle()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (!_disposed && Handle != IntPtr.Zero)
                {
                    CloseHandle(Handle);
                    Handle = IntPtr.Zero;
                }
                _disposed = true;
            }
        }
    }
}
