using System;
using System.IO;
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

            // RICOH THETA S デバイスを取得する
            string targetDeviceId = String.Empty;
            foreach (string deviceId in deviceIds)
            {
                if ("RICOH THETA S".Equals(command.GetDeviceFriendlyName(deviceId)))
                {
                    targetDeviceId = deviceId;
                    break;
                }
            }
            if (targetDeviceId.Length == 0) { return; }
            command.Open(targetDeviceId);

            // DeviceInfo
            res = command.Execute(MtpOperationCode.GetDeviceInfo, null, null);
            MtpData.DeviceInfo dInfo = MtpData.GetDeviceInfoDataset(res);

            // DevicePropDesc(StillCaptureMode)
            res = command.Execute(MtpOperationCode.GetDevicePropDesc, new uint[1] { (uint)MtpDevicePropCode.StillCaptureMode }, null);
            MtpData.DevicePropDesc dpd = MtpData.GetDevicePropDesc(res);

            // ISO優先
            command.Execute(MtpOperationCode.SetDevicePropValue, new uint[1] { (uint)MtpDevicePropCode.ExposureProgramMode }, BitConverter.GetBytes((ushort)ExposureProgramMode.IsoPriorityProgram));

            // DevicePropDesc(ExposureIndex)
            res = command.Execute(MtpOperationCode.GetDevicePropDesc, new uint[1] { (uint)MtpDevicePropCode.ExposureIndex }, null);
            dpd = MtpData.GetDevicePropDesc(res);

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

            // GetObjectHandles
            res = command.Execute(MtpOperationCode.GetObjectHandles, new uint[3] { storageIds[0], 0, 0 }, null);
            uint[] objectHandles = MtpData.GetUInt32Array(res);

            // 静止画か動画をデスクトップに保存する
            // objectHandlesの最初の3つはフォルダのようなので4つ目を取得する
            if (objectHandles.Length > 3)
            {
                // ファイル名を取得する
                res = command.Execute(MtpOperationCode.GetObjectInfo, new uint[1] { objectHandles[3] }, null);
                ObjectInfo objectInfo = new ObjectInfo(res.Data);

                // ファイルを取得する
                res = command.Execute(MtpOperationCode.GetObject, new uint[1] { objectHandles[3] }, null);
                if (res.ResponseCode == MtpResponseCode.OK)
                {
                    // デスクトップへ保存する
                    using (FileStream fs = new FileStream(
                        Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\" + objectInfo.Filename, // ファイル名
                        FileMode.Create, FileAccess.Write))
                    {
                        fs.Write(res.Data, 0, res.Data.Length);
                    }
                }
            }

            // 撮影する
            res = command.Execute(MtpOperationCode.InitiateCapture, new uint[2] { 0, 0 }, null);


            // デバイスよさようなら
            command.Close();
        }
    }
}
