namespace WpdMtpLib
{
    public enum DeviceType : uint
    {
        Generic     = 0,
        Camera,
        MediaPlayer,
        Phone,
        Video,
        PersonalInformationManager,
        AudioRecoder,

        Unknown = 0xFFFFFFFF
    }
}
