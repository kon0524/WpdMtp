using System;
using System.Text;

namespace WpdMtpLib
{
    public static class MtpData
    {
        /// <summary>
        /// ObjectInfo構造体
        /// </summary>
        public struct ObjectInfo
        {
            public uint StorageID;
            public ushort ObjectFormat;
            public ushort ProtectionStatus;
            public uint ObjectCompressedSize;
            public ushort ThumbFormat;
            public uint ThumbCompressedSize;
            public uint ThumbPixWidth;
            public uint ThumbPixHeight;
            public uint ImagePixWidth;
            public uint ImagePixHeight;
            public uint ImageBitDepth;
            public uint ParentObject;
            public ushort AssociationType;
            public uint AssociationDescription;
            public uint SequenceNumber;
            public string Filename;
            public string DateCreated;
            public string DateModified;
            public string Keyword;
        }

        /// <summary>
        /// DeviceInfo構造体
        /// </summary>
        public struct DeviceInfo
        {
            public ushort StandardVersion;
            public uint MtpVenderExtensionID;
            public ushort MtpVersion;
            public string MtpExtensions;
            public ushort FunctionalMode;
            public ushort[] OperationsSupported;
            public ushort[] EventsSupported;
            public ushort[] DevicePropertiesSupport;
            public ushort[] CaptureFormats;
            public ushort[] PlaybackFormats;
            public string Manufacturer;
            public string Model;
            public string DeviceVersion;
            public string SerialNumber;
        }

        /// <summary>
        /// StorageInfo構造体
        /// </summary>
        public struct StorageInfo
        {
            public ushort StorageType;
            public ushort FilesystemType;
            public ushort AccessCapability;
            public ulong MaxCapacity;
            public ulong FreeSpaceInBytes;
            public uint FreeSpaceInObjects;
            public string StorageDescription;
            public string VolumeIdentifier;
        }

        /// <summary>
        /// MtpResponseからuint型の配列を取得します
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static uint[] GetUInt32Array(MtpResponse response)
        {
            uint[] ret = null;
            if (response.ResponseCode != MtpResponseCode.OK || response.Data == null) { return ret; }
            int num = BitConverter.ToInt32(response.Data, 0);
            ret = new uint[num];
            for (int i = 0; i < num; i++)
            {
                ret[i] = BitConverter.ToUInt32(response.Data, (i + 1) * 4);
            }

            return ret;
        }

        /// <summary>
        /// MtpResponseからuint型の数値を取得します
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static uint GetUint32Value(MtpResponse response)
        {
            uint ret = 0;
            if (response.ResponseCode != MtpResponseCode.OK || response.Data == null || response.Data.Length != 4) { return ret; }
            ret = BitConverter.ToUInt32(response.Data, 0);

            return ret;
        }

        /// <summary>
        /// MtpResponseからObjectInfo構造体を取得します
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static ObjectInfo GetObjectInfoDataset(MtpResponse response)
        {
            ObjectInfo objectInfo = new ObjectInfo();
            if (response.ResponseCode != MtpResponseCode.OK || response.Data == null) { return objectInfo; }

            int pos = 0;
            objectInfo.StorageID = BitConverter.ToUInt32(response.Data, pos); pos += 4;
            objectInfo.ObjectFormat = BitConverter.ToUInt16(response.Data, pos); pos += 2;
            objectInfo.ProtectionStatus = BitConverter.ToUInt16(response.Data, pos); pos += 2;
            objectInfo.ObjectCompressedSize = BitConverter.ToUInt32(response.Data, pos); pos += 4;
            objectInfo.ThumbFormat = BitConverter.ToUInt16(response.Data, pos); pos += 2;
            objectInfo.ThumbCompressedSize = BitConverter.ToUInt32(response.Data, pos); pos += 4;
            objectInfo.ThumbPixWidth = BitConverter.ToUInt32(response.Data, pos); pos += 4;
            objectInfo.ThumbPixHeight = BitConverter.ToUInt32(response.Data, pos); pos += 4;
            objectInfo.ImagePixWidth = BitConverter.ToUInt32(response.Data, pos); pos += 4;
            objectInfo.ImagePixHeight = BitConverter.ToUInt32(response.Data, pos); pos += 4;
            objectInfo.ImageBitDepth = BitConverter.ToUInt32(response.Data, pos); pos += 4;
            objectInfo.ParentObject = BitConverter.ToUInt32(response.Data, pos); pos += 4;
            objectInfo.AssociationType = BitConverter.ToUInt16(response.Data, pos); pos += 2;
            objectInfo.AssociationDescription = BitConverter.ToUInt32(response.Data, pos); pos += 4;
            objectInfo.SequenceNumber = BitConverter.ToUInt32(response.Data, pos); pos += 4;
            objectInfo.Filename = getString(response.Data, ref pos);
            objectInfo.DateCreated = getString(response.Data, ref pos);
            objectInfo.DateModified = getString(response.Data, ref pos);
            objectInfo.Keyword = getString(response.Data, ref pos);

            return objectInfo;
        }

        /// <summary>
        /// MtpResponseからDeviceInfo構造体を取得します
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static DeviceInfo GetDeviceInfoDataset(MtpResponse response)
        {
            int pos = 0;
            DeviceInfo deviceInfo = new DeviceInfo();
            if (response.ResponseCode != MtpResponseCode.OK || response.Data == null) { return deviceInfo; }

            deviceInfo.StandardVersion = BitConverter.ToUInt16(response.Data, pos); pos += 2;
            deviceInfo.MtpVenderExtensionID = BitConverter.ToUInt32(response.Data, pos); pos += 4;
            deviceInfo.MtpVersion = BitConverter.ToUInt16(response.Data, pos); pos += 2;
            deviceInfo.MtpExtensions = getString(response.Data, ref pos);
            deviceInfo.FunctionalMode = BitConverter.ToUInt16(response.Data, pos); pos += 2;
            deviceInfo.OperationsSupported = getUShortArray(response.Data, ref pos);
            deviceInfo.EventsSupported = getUShortArray(response.Data, ref pos);
            deviceInfo.DevicePropertiesSupport = getUShortArray(response.Data, ref pos);
            deviceInfo.CaptureFormats = getUShortArray(response.Data, ref pos);
            deviceInfo.PlaybackFormats = getUShortArray(response.Data, ref pos);
            deviceInfo.Manufacturer = getString(response.Data, ref pos);
            deviceInfo.Model = getString(response.Data, ref pos);
            deviceInfo.DeviceVersion = getString(response.Data, ref pos);
            deviceInfo.SerialNumber = getString(response.Data, ref pos);

            return deviceInfo;
        }

        public static StorageInfo GetStorageInfoDataset(MtpResponse response)
        {
            int pos = 0;
            StorageInfo storageInfo = new StorageInfo();
            if (response.ResponseCode != MtpResponseCode.OK || response.Data == null) { return storageInfo; }

            storageInfo.StorageType = BitConverter.ToUInt16(response.Data, pos); pos += 2;
            storageInfo.FilesystemType = BitConverter.ToUInt16(response.Data, pos); pos += 2;
            storageInfo.AccessCapability = BitConverter.ToUInt16(response.Data, pos); pos += 2;
            storageInfo.MaxCapacity = BitConverter.ToUInt64(response.Data, pos); pos += 8;
            storageInfo.FreeSpaceInBytes = BitConverter.ToUInt64(response.Data, pos); pos += 8;
            storageInfo.FreeSpaceInObjects = BitConverter.ToUInt32(response.Data, pos); pos += 4;
            storageInfo.StorageDescription = getString(response.Data, ref pos);
            storageInfo.VolumeIdentifier = getString(response.Data, ref pos);

            return storageInfo;
        }

        /// <summary>
        /// 文字列を取得する
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private static string getString(byte[] data, ref int pos)
        {
            string retval = "";
            int len = (int)data[pos++]; 
            if (len > 0)
            {
                retval = Encoding.Unicode.GetString(data, pos, (len - 1) * 2); 
                pos += (len * 2);
            }

            return retval;
        }

        /// <summary>
        /// ushort型の配列を取得する
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        private static ushort[] getUShortArray(byte[] data, ref int pos)
        {
            uint num = BitConverter.ToUInt32(data, pos);
            pos += 4;
            ushort[] array = new ushort[num];
            for (int i = 0; i < num; i++)
            {
                array[i] = BitConverter.ToUInt16(data, pos);
                pos += 2;
            }

            return array;
        }
    }
}
