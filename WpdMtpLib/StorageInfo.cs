using System;

namespace WpdMtpLib
{
    public class StorageInfo
    {
        public ushort StorageType { get; private set; }
        public ushort FilesystemType { get; private set; }
        public ushort AccessCapability { get; private set; }
        public ulong MaxCapacity { get; private set; }
        public ulong FreeSpaceInBytes { get; private set; }
        public uint FreeSpaceInObjects { get; private set; }
        public string StorageDescription { get; private set; }
        public string VolumeIdentifier { get; private set; }

        public StorageInfo(byte[] data)
        {
            int pos = 0;
            StorageType = BitConverter.ToUInt16(data, pos); pos += 2;
            FilesystemType = BitConverter.ToUInt16(data, pos); pos += 2;
            AccessCapability = BitConverter.ToUInt16(data, pos); pos += 2;
            MaxCapacity = BitConverter.ToUInt64(data, pos); pos += 8;
            FreeSpaceInBytes = BitConverter.ToUInt64(data, pos); pos += 8;
            FreeSpaceInObjects = BitConverter.ToUInt32(data, pos); pos += 4;
            StorageDescription = Utils.GetString(data, ref pos);
            VolumeIdentifier = Utils.GetString(data, ref pos);
        }
    }
}
