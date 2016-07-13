
namespace WpdMtpLib
{
    public static class MtpEvent
    {
        public const ushort ObjectAdded = 0x4002;
        public const ushort DevicePropChanged = 0x4006;
        public const ushort DeviceInfoChanged = 0x4008;
        public const ushort StoreFull = 0x400A;
        public const ushort StorageInfoChanged = 0x400C;
        public const ushort CaptureComplete = 0x400D;
    }
}
