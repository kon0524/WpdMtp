using System;

namespace WpdMtpLib
{
    public class ObjectInfo
    {
        public uint StorageID { get; private set; }
        public ushort ObjectFormat { get; private set; }
        public ushort ProtectionStatus { get; private set; }
        public uint ObjectCompressedSize { get; private set; }
        public ushort ThumbFormat { get; private set; }
        public uint ThumbCompressedSize { get; private set; }
        public uint ThumbPixWidth { get; private set; }
        public uint ThumbPixHeight { get; private set; }
        public uint ImagePixWidth { get; private set; }
        public uint ImagePixHeight { get; private set; }
        public uint ImageBitDepth { get; private set; }
        public uint ParentObject { get; private set; }
        public ushort AssociationType { get; private set; }
        public uint AssociationDescription { get; private set; }
        public uint SequenceNumber { get; private set; }
        public string Filename { get; private set; }
        public string DateCreated { get; private set; }
        public string DateModified { get; private set; }
        public string Keyword { get; private set; }

        public ObjectInfo(byte[] data)
        {
            int pos = 0;
            StorageID = BitConverter.ToUInt32(data, pos); pos += 4;
            ObjectFormat = BitConverter.ToUInt16(data, pos); pos += 2;
            ProtectionStatus = BitConverter.ToUInt16(data, pos); pos += 2;
            ObjectCompressedSize = BitConverter.ToUInt32(data, pos); pos += 4;
            ThumbFormat = BitConverter.ToUInt16(data, pos); pos += 2;
            ThumbCompressedSize = BitConverter.ToUInt32(data, pos); pos += 4;
            ThumbPixWidth = BitConverter.ToUInt32(data, pos); pos += 4;
            ThumbPixHeight = BitConverter.ToUInt32(data, pos); pos += 4;
            ImagePixWidth = BitConverter.ToUInt32(data, pos); pos += 4;
            ImagePixHeight = BitConverter.ToUInt32(data, pos); pos += 4;
            ImageBitDepth = BitConverter.ToUInt32(data, pos); pos += 4;
            ParentObject = BitConverter.ToUInt32(data, pos); pos += 4;
            AssociationType = BitConverter.ToUInt16(data, pos); pos += 2;
            AssociationDescription = BitConverter.ToUInt32(data, pos); pos += 4;
            SequenceNumber = BitConverter.ToUInt32(data, pos); pos += 4;
            Filename = Utils.GetString(data, ref pos);
            DateCreated = Utils.GetString(data, ref pos);
            DateModified = Utils.GetString(data, ref pos);
            Keyword = Utils.GetString(data, ref pos);
        }
    }
}
