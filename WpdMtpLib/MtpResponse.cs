
namespace WpdMtpLib
{
    public class MtpResponse
    {
        /// <summary>
        /// レスポンスコード
        /// </summary>
        public MtpResponseCode ResponseCode { get; private set; }

        /// <summary>
        /// パラメータ1
        /// </summary>
        public uint Parameter1 { get; private set; }

        /// <summary>
        /// パラメータ2
        /// </summary>
        public uint Parameter2 { get; private set; }

        /// <summary>
        /// パラメータ3
        /// </summary>
        public uint Parameter3 { get; private set; }

        /// <summary>
        /// パラメータ4
        /// </summary>
        public uint Parameter4 { get; private set; }

        /// <summary>
        /// パラメータ5
        /// </summary>
        public uint Parameter5 { get; private set; }

        /// <summary>
        /// データ(R->Iの場合のみ)
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="responseCode"></param>
        /// <param name="parameter"></param>
        /// <param name="data"></param>
        public MtpResponse(ushort responseCode, uint[] parameter, byte[] data)
        {
            ResponseCode = (MtpResponseCode)responseCode;
            if (parameter != null)
            {
                if (parameter.Length > 0) { Parameter1 = parameter[0]; }
                if (parameter.Length > 1) { Parameter2 = parameter[1]; }
                if (parameter.Length > 2) { Parameter3 = parameter[2]; }
                if (parameter.Length > 3) { Parameter4 = parameter[3]; }
                if (parameter.Length > 4) { Parameter5 = parameter[4]; }
            }
            Data = data;
        }
    }
}
