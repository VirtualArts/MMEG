using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{

    [StructLayout(LayoutKind.Sequential, Size = 512)]
    internal struct HeaderBlock
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 100)]
        public byte[] name;    // name of file. 

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] mode;    // file mode

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] uid;     // owner user ID

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] gid;     // owner group ID

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] size;    // length of file in bytes

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
        public byte[] mtime;   // modify time of file

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] chksum;  // checksum for header
    }

    internal class RawSerializer<T>
    {
        public T RawDeserialize(byte[] rawData)
        {
            return RawDeserialize(rawData, 0);
        }

        public T RawDeserialize(byte[] rawData, int position)
        {
            int rawsize = Marshal.SizeOf(typeof(T));
            if (rawsize > rawData.Length)
                return default(T);

            IntPtr buffer = Marshal.AllocHGlobal(rawsize);
            Marshal.Copy(rawData, position, buffer, rawsize);
            T obj = (T)Marshal.PtrToStructure(buffer, typeof(T));
            Marshal.FreeHGlobal(buffer);
            return obj;
        }

        public byte[] RawSerialize(T item)
        {
            int rawSize = Marshal.SizeOf(typeof(T));
            IntPtr buffer = Marshal.AllocHGlobal(rawSize);
            Marshal.StructureToPtr(item, buffer, false);
            byte[] rawData = new byte[rawSize];
            Marshal.Copy(buffer, rawData, 0, rawSize);
            Marshal.FreeHGlobal(buffer);
            return rawData;
        }
    }
}
