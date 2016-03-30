
namespace WpdMtpLib
{
    /// <summary>
    /// MTPオペレーションコード(Thetaでサポートしている値のみ)
    /// </summary>
    public enum MtpOperationCode : ushort
    {
        GetDeviceInfo           = 0x1001,
        OpenSession,
        CloseSession,
        GetStorageIDs,
        GetStorageInfo,
        GetNumObjects,
        GetObjectHandles,
        GetObjectInfo,
        GetObject,
        GetThumb,
        DeleteObject,
        InitiateCapture         = 0x100E,
        GetDevicePropDesc       = 0x1014,
        GetDevicePropValue,
        SetDevicePropValue,
        TerminateOpenCapture    = 0x1018,
        GetPartialObject        = 0x101B,
        InitiateOpenCapture,
        StopSelfTimer           = 0x99A2
    }
}
