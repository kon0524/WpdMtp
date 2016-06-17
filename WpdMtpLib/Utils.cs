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
    }
}
