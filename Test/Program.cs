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

            // イベントを受け取れるようにする
            command.MtpEvent += MtpEventListener;

            // DeviceInfo
            res = command.Execute(MtpOperationCode.GetDeviceInfo, null, null);
            DeviceInfo deviceInfo = new DeviceInfo(res.Data);

            // DevicePropDesc(StillCaptureMode)
            res = command.Execute(MtpOperationCode.GetDevicePropDesc, new uint[1] { (uint)MtpDevicePropCode.StillCaptureMode }, null);
            DevicePropDesc dpd = new DevicePropDesc(res.Data);

            // シャッター優先
            command.Execute(MtpOperationCode.SetDevicePropValue, new uint[1] { (uint)MtpDevicePropCode.ExposureProgramMode }, BitConverter.GetBytes((ushort)ExposureProgramMode.ShutterPriorityProgram));

            // シャッター速度(Get)
            res = command.Execute(MtpOperationCode.GetDevicePropValue, new uint[1] { (uint)MtpDevicePropCode.ShutterSpeed }, null);
            ShutterSpeed ss = new ShutterSpeed(res.Data);
            
            // シャッター速度(Set)
            ss = new ShutterSpeed(1, 100); // 1/100
            res = command.Execute(MtpOperationCode.SetDevicePropValue, new uint[1] { (uint)MtpDevicePropCode.ShutterSpeed }, ss.Data);

            // シャッター速度(Get)
            res = command.Execute(MtpOperationCode.GetDevicePropValue, new uint[1] { (uint)MtpDevicePropCode.ShutterSpeed }, null);
            ss = new ShutterSpeed(res.Data);

            // DevicePropDesc(ExposureIndex)
            res = command.Execute(MtpOperationCode.GetDevicePropDesc, new uint[1] { (uint)MtpDevicePropCode.ExposureIndex }, null);
            dpd = new DevicePropDesc(res.Data);

            // StillCaptureMode
            res = command.Execute(MtpOperationCode.GetDevicePropValue, new uint[1] { (uint)MtpDevicePropCode.StillCaptureMode }, null);
            StillCaptureMode mode = (StillCaptureMode)BitConverter.ToUInt16(res.Data, 0);

            // ストレージIDをとる
            res = command.Execute(MtpOperationCode.GetStorageIDs, null, null);
            uint[] storageIds = Utils.GetUIntArray(res.Data);

            // ストレージ情報をとる
            res = command.Execute(MtpOperationCode.GetStorageInfo, new uint[1] { storageIds[0] }, null);
            StorageInfo storageInfo = new StorageInfo(res.Data);

            // オブジェクト数をとる
            res = command.Execute(MtpOperationCode.GetNumObjects, new uint[3] { storageIds[0], 0, 0 }, null);
            uint num = res.Parameter1;

            // GetObjectHandles
            res = command.Execute(MtpOperationCode.GetObjectHandles, new uint[3] { storageIds[0], 0, 0 }, null);
            uint[] objectHandles = Utils.GetUIntArray(res.Data);

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

        /// <summary>
        /// イベント用コールバック
        /// </summary>
        /// <param name="eventCode"></param>
        static void MtpEventListener(ushort eventCode, object eventValue)
        {
            Console.WriteLine("Event : " + eventCode);
            switch (eventCode)
            {
                case MtpEvent.ObjectAdded:
                    Console.WriteLine("ObjectAdded. ObjectID: {0}", (uint)eventValue);
                    break;
                case MtpEvent.DevicePropChanged:
                    Console.WriteLine("DevicePropChanged.");
                    break;
                case MtpEvent.DeviceInfoChanged:
                    Console.WriteLine("DeviceInfoChanged.");
                    break;
                case MtpEvent.StoreFull:
                    Console.WriteLine("StoreFull.");
                    break;
                case MtpEvent.StorageInfoChanged:
                    Console.WriteLine("StorageInfoChanged.");
                    break;
                case MtpEvent.CaptureComplete:
                    Console.WriteLine("CaptureComplete.");
                    break;
                default:
                    Console.WriteLine("Unknown Event");
                    break;
            }
        }
    }
}
