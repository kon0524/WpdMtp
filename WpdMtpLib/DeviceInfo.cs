using System;

namespace WpdMtpLib
{
    public class DeviceInfo
    {
        public ushort StandardVersion { get; private set; }
        public uint MtpVenderExtensionID { get; private set; }
        public ushort MtpVersion { get; private set; }
        public string MtpExtensions { get; private set; }
        public ushort FunctionalMode { get; private set; }
        public ushort[] OperationsSupported { get; private set; }
        public ushort[] EventsSupported { get; private set; }
        public ushort[] DevicePropertiesSupport { get; private set; }
        public ushort[] CaptureFormats { get; private set; }
        public ushort[] PlaybackFormats { get; private set; }
        public string Manufacturer { get; private set; }
        public string Model { get; private set; }
        public string DeviceVersion { get; private set; }
        public string SerialNumber { get; private set; }

        public DeviceInfo(byte[] data)
        {
            int pos = 0;
            StandardVersion = BitConverter.ToUInt16(data, pos); pos += 2;
            MtpVenderExtensionID = BitConverter.ToUInt32(data, pos); pos += 4;
            MtpVersion = BitConverter.ToUInt16(data, pos); pos += 2;
            MtpExtensions = Utils.GetString(data, ref pos);
            FunctionalMode = BitConverter.ToUInt16(data, pos); pos += 2;
            OperationsSupported = Utils.GetUShortArray(data, ref pos);
            EventsSupported = Utils.GetUShortArray(data, ref pos);
            DevicePropertiesSupport = Utils.GetUShortArray(data, ref pos);
            CaptureFormats = Utils.GetUShortArray(data, ref pos);
            PlaybackFormats = Utils.GetUShortArray(data, ref pos);
            Manufacturer = Utils.GetString(data, ref pos);
            Model = Utils.GetString(data, ref pos);
            DeviceVersion = Utils.GetString(data, ref pos);
            SerialNumber = Utils.GetString(data, ref pos);
        }
    }
}
