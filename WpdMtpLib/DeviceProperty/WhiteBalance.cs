
namespace WpdMtpLib.DeviceProperty
{
    public enum WhiteBalance : ushort
    {
        Automatic = 0x0002,
        Daylight = 0x0004,
        Shade = 0x8001,
        Cloudy = 0x8002,
        Tungsten1 = 0x0006,
        Tungsten2 = 0x8020,
        FluorescentLamp1_DaylightColor = 0x8003,
        FluorescentLamp2_NeutralWhiteColor = 0x8004,
        FluorescentLamp3_White = 0x8005,
        FluorescentLamp4_LightBulbColor = 0x8006
    }
}
