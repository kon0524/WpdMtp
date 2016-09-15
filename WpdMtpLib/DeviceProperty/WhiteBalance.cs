
namespace WpdMtpLib.DeviceProperty
{
    public enum WhiteBalance : ushort
    {
        Undefined = 0x0000,
        Manual = 0x0001,
        Automatic = 0x0002,
        OnePushAutomatic = 0x0003,
        Daylight = 0x0004,
        Florescent = 0x0005,
        Flash = 0x0007,
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
