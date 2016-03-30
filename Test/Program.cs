using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpdMtpLib;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            MtpResponse res;
            MtpCommand command = new MtpCommand();

            // 接続されているデバイスIDを取得する
            string[] deviceIds = command.GetDeviceIds();
            if (deviceIds.Length == 0) { return; }

            // 運よく最初のデバイスIDが所望のデバイスでありますように...(なんとかしたい)
            command.Open(deviceIds[0]);

            // ストレージIDをとる
            res = command.Execute(MtpOperationCode.GetStorageIDs, null, null);
            uint[] storageIds = MtpData.GetUInt32Array(res);

            // オブジェクト数をとる
            res = command.Execute(MtpOperationCode.GetNumObjects, new uint[3] { storageIds[0], 0, 0 }, null);
            uint num = res.Parameter1;

            // 撮影する
            res = command.Execute(MtpOperationCode.InitiateCapture, new uint[2] { 0, 0 }, null);

            // デバイスよさようなら
            command.Close();
        }
    }
}
