namespace WpdMtpLib
{
    public enum DataType : ushort
    {
        UNDEF = 0x0000,
        INT8,
        UINT8,
        INT16,
        UINT16,
        INT32,
        UINT32,
        INT64,
        UINT64,
        INT128,
        UINT128,

        AINT8 = 0x4001,
        AUINT8,
        AINT16,
        AUINT16,
        AINT32,
        AUINT32,
        AINT64,
        AUINT64,
        AINT128,
        AUINT128,

        STR = 0xFFFF
    }
}
