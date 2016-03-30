using PortableDeviceApiLib;
using System;
using System.Runtime.InteropServices;

namespace WpdMtpLib
{
    public class MtpCommand
    {
        /// <summary>
        /// デバイス
        /// </summary>
        private PortableDevice device;

        /// <summary>
        /// イベント識別子
        /// </summary>
        private string eventCookie;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MtpCommand()
        {
            device = null;
        }

        /// <summary>
        /// 接続されているデバイスのID配列を取得する
        /// </summary>
        /// <returns></returns>
        public string[] GetDeviceIds()
        {
            // デバイスマネージャ
            PortableDeviceManager manager = new PortableDeviceManager();
            manager.RefreshDeviceList();
            // 接続されているデバイス数を取得する
            uint deviceNum = 1;
            manager.GetDevices(null, ref deviceNum);
            // デバイスIDを取得する
            string[] deviceIds = new string[deviceNum];
            if (deviceNum > 0) { manager.GetDevices(ref deviceIds[0], ref deviceNum); }
            // 解放
            Marshal.ReleaseComObject(manager);

            return deviceIds;
        }

        /// <summary>
        /// デバイスに接続する
        /// </summary>
        /// <param name="deviceId">デバイスID</param>
        public void Open(string deviceId)
        {
            if (device != null) { return; }
            device = new PortableDevice();
            IPortableDeviceValues clientInfo = (IPortableDeviceValues)new PortableDeviceTypesLib.PortableDeviceValuesClass();
            device.Open(deviceId, clientInfo);
            Marshal.ReleaseComObject(clientInfo);

            // eventを受信できるようにする
            WpdEvent wpdEvent = new WpdEvent();
            IPortableDeviceValues eventParameter = (IPortableDeviceValues)new PortableDeviceTypesLib.PortableDeviceValuesClass();
            device.Advise(0, wpdEvent, eventParameter, out eventCookie);
        }

        /// <summary>
        /// デバイスを切断する
        /// </summary>
        public void Close()
        {
            if (device == null) { return; }
            device.Unadvise(eventCookie);
            device.Close();
            Marshal.ReleaseComObject(device);
            device = null;
        }

        /// <summary>
        /// MTPオペレーションを実行する
        /// </summary>
        /// <param name="code"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public MtpResponse Execute(MtpOperationCode code, uint[] param, byte[] data = null)
        {
            if (param == null) { param = new uint[5]; }
            return MtpOperation.ExecuteCommand(device, code, param, data);
        }

        /// <summary>
        /// WPDのイベントを受信するためのクラス
        /// </summary>
        private class WpdEvent : IPortableDeviceEventCallback
        {
            public void OnEvent(IPortableDeviceValues pEventParameters)
            {
                Guid eventId;
                if (pEventParameters == null) { return; }
                pEventParameters.GetGuidValue(ref WpdProperty.WPD_EVENT_PARAMETER_EVENT_ID, out eventId);
                Console.WriteLine(eventId);
            }
        }
    }
}
