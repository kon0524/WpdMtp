
namespace WpdMtpLib.DeviceProperty
{
    public enum WhiteBalance : ushort
    {
        Undefined                           = 0x0000,
        Manual,
        Automatic,
        OnePushAutomatic,
        Daylight,
        Florescent,
        Tungsten1,
        Flash,
        Shade                               = 0x8001,
        Cloudy,
        FluorescentLamp1_DaylightColor,
        FluorescentLamp2_NeutralWhiteColor,
        FluorescentLamp3_White,
        FluorescentLamp4_LightBulbColor,
        Tungsten2                           = 0x8020,
    }
}
