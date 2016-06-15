using WpdMtpLib;
using WpdMtpLib.DeviceProperty;

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

            // DeviceInfo
            res = command.Execute(MtpOperationCode.GetDeviceInfo, null, null);
            MtpData.DeviceInfo dInfo = MtpData.GetDeviceInfoDataset(res);

            // DevicePropDesc(StillCaptureMode)
            res = command.Execute(MtpOperationCode.GetDevicePropDesc, new uint[1] { (uint)MtpDevicePropCode.StillCaptureMode }, null);
            MtpData.DevicePropDesc dpd = MtpData.GetDevicePropDesc(res);

            // StillCaptureMode
            res = command.Execute(MtpOperationCode.GetDevicePropValue, new uint[1] { (uint)MtpDevicePropCode.StillCaptureMode }, null);
            StillCaptureMode mode = (StillCaptureMode)MtpData.GetUint16Value(res);

            // ストレージIDをとる
            res = command.Execute(MtpOperationCode.GetStorageIDs, null, null);
            uint[] storageIds = MtpData.GetUInt32Array(res);

            // ストレージ情報をとる
            res = command.Execute(MtpOperationCode.GetStorageInfo, new uint[1] { storageIds[0] }, null);
            MtpData.StorageInfo storageInfo = MtpData.GetStorageInfoDataset(res);

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
