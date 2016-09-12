using System;
using WpdMtpLib.DeviceProperty;

namespace WpdMtpLib
{
    public class DevicePropDesc
    {
        public MtpDevicePropCode DevicePropCode { get; private set; }
        public DataType DataType { get; private set; }
        public byte GetSet { get; private set; }
        public dynamic FactoryDefaultValue { get; private set; }
        public dynamic CurrentValue { get; private set; }
        public byte FormFlag { get; private set; }
        public dynamic Form { get; private set; }

        public DevicePropDesc(byte[] data)
        {
            int pos = 0;
            DevicePropCode = (MtpDevicePropCode)BitConverter.ToUInt16(data, pos); pos += 2;
            DataType = (DataType)BitConverter.ToUInt16(data, pos); pos += 2;
            GetSet = data[pos]; pos++;
            FactoryDefaultValue = getValue(data, ref pos, DataType);
            CurrentValue = getValue(data, ref pos, DataType);
            FormFlag = data[pos]; pos++;
            if (FormFlag == 0x02)
            {   // 配列
                Form = getForm(data, ref pos, DataType);
            }
            else if (FormFlag == 0x01)
            {   // 範囲
                Form = getRangeForm(data, ref pos, DataType, 3);
            }
        }

        private static dynamic getRangeForm(byte[] data, ref int pos, DataType type, ushort arraySize)
        {
            dynamic value = null;
            switch (type)
            {
                case DataType.INT8:
                    value = new sbyte[arraySize];
                    for (int i = 0; i < arraySize; i++)
                    {
                        value[i] = (sbyte)data[pos]; pos++;
                    }
                    break;
                case DataType.UINT8:
                    value = new byte[arraySize];
                    for (int i = 0; i < arraySize; i++)
                    {
                        value[i] = (byte)data[pos]; pos++;
                    }
                    break;
                case DataType.INT16:
                    value = new short[arraySize];
                    for (int i = 0; i < arraySize; i++)
                    {
                        value[i] = BitConverter.ToInt16(data, pos); pos += 2;
                    }
                    break;
                case DataType.UINT16:
                    value = new ushort[arraySize];
                    for (int i = 0; i < arraySize; i++)
                    {
                        value[i] = BitConverter.ToUInt16(data, pos); pos += 2;
                    }
                    break;
                case DataType.INT32:
                    value = new int[arraySize];
                    for (int i = 0; i < arraySize; i++)
                    {
                        value[i] = BitConverter.ToInt32(data, pos); pos += 4;
                    }
                    break;
                case DataType.UINT32:
                    value = new uint[arraySize];
                    for (int i = 0; i < arraySize; i++)
                    {
                        value[i] = BitConverter.ToUInt32(data, pos); pos += 4;
                    }
                    break;
                case DataType.INT64:
                    value = new long[arraySize];
                    for (int i = 0; i < arraySize; i++)
                    {
                        value[i] = BitConverter.ToInt64(data, pos); pos += 8;
                    }
                    break;
                case DataType.UINT64:
                    value = new ulong[arraySize];
                    for (int i = 0; i < arraySize; i++)
                    {
                        value[i] = BitConverter.ToUInt64(data, pos); pos += 8;
                    }
                    break;
                case DataType.STR:
                    value = new string[3];
                    for (int i = 0; i < arraySize; i++)
                    {
                        value[i] = Utils.GetString(data, ref pos);
                    }
                    break;
                default:
                    break;
            }

            return value;
        }

        /// <summary>
        /// 型に応じた配列を取得します
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pos"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static dynamic getForm(byte[] data, ref int pos, DataType type)
        {
            ushort arraySize = BitConverter.ToUInt16(data, pos); pos += 2;
            return getRangeForm(data, ref pos, type, arraySize);
        }

        /// <summary>
        /// 型に応じて値を取得する
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pos"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static dynamic getValue(byte[] data, ref int pos, DataType type)
        {
            dynamic value = null;

            switch (type)
            {
                case DataType.INT8:
                    value = (char)data[pos]; pos++;
                    break;
                case DataType.UINT8:
                    value = data[pos]; pos++;
                    break;
                case DataType.INT16:
                    value = BitConverter.ToInt16(data, pos); pos += 2;
                    break;
                case DataType.UINT16:
                    value = BitConverter.ToUInt16(data, pos); pos += 2;
                    break;
                case DataType.INT32:
                    value = BitConverter.ToInt32(data, pos); pos += 4;
                    break;
                case DataType.UINT32:
                    value = BitConverter.ToUInt32(data, pos); pos += 4;
                    break;
                case DataType.INT64:
                    value = BitConverter.ToInt64(data, pos); pos += 8;
                    break;
                case DataType.UINT64:
                    value = BitConverter.ToUInt64(data, pos); pos += 8;
                    break;
                case DataType.STR:
                    value = Utils.GetString(data, ref pos);
                    break;
                default:
                    break;
            }

            return value;
        }
    }
}
