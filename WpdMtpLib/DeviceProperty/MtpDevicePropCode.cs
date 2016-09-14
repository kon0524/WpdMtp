
namespace WpdMtpLib.DeviceProperty
{
    /// <summary>
    /// Device Prop Code
    /// </summary>
    public enum MtpDevicePropCode : ushort
    {
        BatteryLevel            = 0x5001,
        FunctionalMode,
        ImageSize,
        WhiteBalance            = 0x5005,
        FNumber                 = 0x5007,
        ExposureProgramMode     = 0x500E,
        ExposureIndex,
        ExposureBiasCompensation,
        DateTime,
        CaptureDelay,
        StillCaptureMode,
        TimelapseNumber         = 0x501A,
        TimelapseInterval,
        AudioVolume             = 0x502C,
        ErrorInfo               = 0xD006,
        ShutterSpeed            = 0xD00F,
        PerceivedDeviceType     = 0xD407,
        GpsInfo                 = 0xD801,
        AutoPowerOffDelay,
        SleepDelay,
        ChannelNumber           = 0xD807,
        CaptureStatus,
        RecordingTime,
        RemainingRecordingTime,
        Filter,
        BatteryStatus,
        RemainingVideos,
        SleepMode
    }
}
