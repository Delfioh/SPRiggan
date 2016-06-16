using System.Runtime.InteropServices;

namespace SPRiggan
{
    [StructLayout(LayoutKind.Sequential, Size = 1024)]
    public struct Palette
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 256)]
        public PaletteColor[] data;
    }

    [StructLayout(LayoutKind.Sequential, Size = 4)]
    public struct PaletteColor
    {
        public byte r;
        public byte g;
        public byte b;
        public byte a;
    }
}
