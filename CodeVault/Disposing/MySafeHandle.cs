using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32.SafeHandles;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using System.Runtime.InteropServices;

// This is an example of implementing your own SafeHandle class. There is a lot
// of stuff here, but it is mainly baggage to show a complete working implementation.
// The actual SafeFindHandle class is very small.

// Richter, p470.
// See namespace Microsoft.Win32.SafeHandles
// Only 2 classes are exposed at the moment, but there are many others which
// can be examined in Reflector and copied, e.g. SafeRegistryHandle, SafeProcessHandle.
// Also see "FileSystemEnumerator" for example code.
//
// SafeHandle (abstract)
//   SafeHandleMinusOneIsInvalid (abstract)
//   SafeHandleZeroOrMinusOneIsInvalid (abstract)
//     SafeFileHandle (concrete)
//     SafeWaitHandle (concrete)

namespace Disposing {

    /// <summary>
    /// Probably best to seal this class by default.
    /// </summary>
    sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid {
        public SafeFindHandle()
            : base(true) {
        }

        /// <summary>
        /// Release the handle.
        /// </summary>
        /// <returns>True if the handle was released.</returns>
        [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
        protected override bool ReleaseHandle() {
            return SafeNativeMethods.FindClose(handle);
        }

        // Note you can also override IsInvalid if 0/-1 is not enough.
        // STRONGLY RECOMMENDED YOU DO NOT OVERRIDE Dispose(). (Richter, p471).
    }


    /// <summary>
    /// Wrapper for P/Invoke methods used by FileSystemEnumerator
    /// </summary>
    [SecurityPermissionAttribute(SecurityAction.Assert, UnmanagedCode = true)]
    internal static class SafeNativeMethods {
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        public static extern SafeFindHandle FindFirstFile(String fileName, [In, Out] FindData findFileData);

        [DllImport("kernel32", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FindNextFile(SafeFindHandle hFindFile, [In, Out] FindData lpFindFileData);

        [DllImport("kernel32", CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FindClose(IntPtr hFindFile);
    }

    /// <summary>
    /// Structure that maps to WIN32_FIND_DATA
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    internal sealed class FindData {
        public int fileAttributes;
        public int creationTime_lowDateTime;
        public int creationTime_highDateTime;
        public int lastAccessTime_lowDateTime;
        public int lastAccessTime_highDateTime;
        public int lastWriteTime_lowDateTime;
        public int lastWriteTime_highDateTime;
        public int nFileSizeHigh;
        public int nFileSizeLow;
        public int dwReserved0;
        public int dwReserved1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public String fileName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
        public String alternateFileName;
    }

}
