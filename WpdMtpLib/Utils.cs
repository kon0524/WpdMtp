using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpdMtpLib
{
    public static class Utils
    {
        /// <summary>
        /// 文字列を取得する
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static string GetString(byte[] data, ref int pos)
        {
            string retval = "";
            int len = (int)data[pos++];
            if (len > 0)
            {
                retval = Encoding.Unicode.GetString(data, pos, (len - 1) * 2);
                pos += (len * 2);
            }

            return retval;
        }

        /// <summary>
        /// ushort型の配列を取得する
        /// </summary>
        /// <param name="data"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public static ushort[] GetUShortArray(byte[] data, ref int pos)
        {
            uint num = BitConverter.ToUInt32(data, pos);
            pos += 4;
            ushort[] array = new ushort[num];
            for (int i = 0; i < num; i++)
            {
                array[i] = BitConverter.ToUInt16(data, pos);
                pos += 2;
            }

            return array;
        }

        /// <summary>
        /// MtpResponseからuint型の配列を取得します
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static uint[] GetUIntArray(byte[] data)
        {
            uint[] ret = null;
            int num = BitConverter.ToInt32(data, 0);
            ret = new uint[num];
            for (int i = 0; i < num; i++)
            {
                ret[i] = BitConverter.ToUInt32(data, (i + 1) * 4);
            }

            return ret;
        }
    }
}
