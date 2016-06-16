using System;

namespace SPRiggan
{
    public struct Frame
    {
        public UInt16 width;
        public UInt16 height;
        public UInt16 data_length;
        public byte[] frame_data;
    }
}
