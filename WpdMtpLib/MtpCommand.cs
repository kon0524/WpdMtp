using PortableDeviceApiLib;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;

namespace WpdMtpLib
{
    public class MtpCommand
    {
        /// <summary>
        /// MTPイベント
        /// </summary>
        public event Action<ushort, object> MtpEvent;

        /// <summary>
        /// デバイスイベント
        /// </summary>
        public event Action<DeviceEvent, object> DeviceEvent;

        /// <summary>
        /// デバイス
        /// </summary>
        private PortableDevice device;

        /// <summary>
        /// イベント識別子
        /// </summary>
        private string eventCookie;

        /// <summary>
        /// MTPイベントのGUID
        /// </summary>
        private static Guid WPD_EVENT_MTP_VENDOR_EXTENDED_EVENTS
            = new Guid(0x00000000, 0x5738, 0x4ff2, 0x84, 0x45, 0xbe, 0x31, 0x26, 0x69, 0x10, 0x59);
        private static Guid WPD_EVENT_OBJECT_ADDED
            = new Guid(0xA726DA95, 0xE207, 0x4B02, 0x8D, 0x44, 0xBE, 0xF2, 0xE8, 0x6C, 0xBF, 0xFC);
        private static Guid WPD_EVENT_DEVICE_REMOVED
            = new Guid(0xE4CBCA1B, 0x6918, 0x48B9, 0x85, 0xEE, 0x02, 0xBE, 0x7C, 0x85, 0x0A, 0xF9);
        private static Guid WPD_EVENT_OBJECT_UPDATED
            = new Guid(0x1445A759, 0x2E01, 0x485D, 0x9F, 0x27, 0xFF, 0x07, 0xDA, 0xE6, 0x97, 0xAB);
        private static Guid WPD_EVENT_DEVICE_CAPABILITIES_UPDATED
            = new Guid(0x36885AA1, 0xCD54, 0x4DAA, 0xB3, 0xD0, 0xAF, 0xB3, 0xE0, 0x3F, 0x59, 0x99);

        /// <summary>
        /// MTP排他制御用セマフォ
        /// </summary>
        private Semaphore sem = null;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MtpCommand()
        {
            device = null;
        }

        /// <summary>
        /// デストラクター
        /// </summary>
        ~MtpCommand()
        {
            if (sem != null)
            {
                sem.Close();
                sem = null;
            }
        }

        /// <summary>
        /// 接続されているデバイスのID配列を取得する
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetDeviceIds()
        {
            // デバイスマネージャ
            PortableDeviceManager manager = new PortableDeviceManager();
            manager.RefreshDeviceList();
            // 接続されているデバイス数を取得する
            uint deviceNum = 1;
            manager.GetDevices(null, ref deviceNum);
            // デバイスIDを取得する
            string[] deviceIds = new string[deviceNum];
            if (deviceNum > 0) { manager.GetDevices(deviceIds, ref deviceNum); }
            // 解放
            Marshal.ReleaseComObject(manager);

            return deviceIds;
        }

        /// <summary>
        /// 接続したデバイスを取得する
        /// </summary>
        /// <returns></returns>
        public object GetDevice()
        {
            return device;
        }

        /// <summary>
        /// デバイスの接続状況
        /// </summary>
        /// <returns></returns>
        public bool IsOpened()
        {
            return device != null;
        }

        /// <summary>
        /// ushort配列からstringへの変換
        /// </summary>
        private string ushortArrayToString(ushort[] usArray)
        {
            string str = String.Empty;
            foreach (ushort letter in usArray) {
                if (letter != 0) { str += (char)letter; }
            }
            return str;
        }

        /// <summary>
        /// デバイスプロトコルを取得する
        /// </summary>
        /// <param name="deviceId">デバイスID</param>
        public virtual string GetDeviceProtocol(string deviceId)
        {
            PortableDeviceManager manager = new PortableDeviceManager();
            string deviceProtocol = string.Empty;
            try
            {
                var isOpened = IsOpened();
                if (!isOpened) { Open(deviceId); }
                IPortableDeviceContent content;
                IPortableDeviceProperties properties;
                device.Content(out content);
                content.Properties(out properties);
                PortableDeviceApiLib.IPortableDeviceValues propertyValues;
                properties.GetValues("DEVICE", null, out propertyValues);
                propertyValues.GetStringValue(WpdProperty.WPD_DEVICE_PROTOCOL, out deviceProtocol);
                if (Marshal.IsComObject(propertyValues)) { Marshal.ReleaseComObject(propertyValues); }
                if (Marshal.IsComObject(properties)) { Marshal.ReleaseComObject(properties); }
                if (Marshal.IsComObject(content)) { Marshal.ReleaseComObject(content); }
                if (!isOpened) { Close(); }
            }
            catch (Exception)
            {
                deviceProtocol = string.Empty;
            }
            Marshal.ReleaseComObject(manager);

            return deviceProtocol;
        }
        /// <summary>
        /// デバイス種別を取得する
        /// </summary>
        /// <param name="deviceId">デバイスID</param>
        public virtual DeviceType GetDeviceType(string deviceId)
        {
            PortableDeviceManager manager = new PortableDeviceManager();
            uint deviceType = (uint)DeviceType.Unknown;
            try {
                var isOpened = IsOpened();
                if (!isOpened) { Open(deviceId); }
                IPortableDeviceContent content;
                IPortableDeviceProperties properties;
                device.Content(out content);
                content.Properties(out properties);
                PortableDeviceApiLib.IPortableDeviceValues propertyValues;
                properties.GetValues("DEVICE", null, out propertyValues);
                propertyValues.GetUnsignedIntegerValue(WpdProperty.WPD_DEVICE_TYPE, out deviceType);
                if (Marshal.IsComObject(propertyValues)) { Marshal.ReleaseComObject(propertyValues); }
                if (Marshal.IsComObject(properties)) { Marshal.ReleaseComObject(properties); }
                if (Marshal.IsComObject(content)) { Marshal.ReleaseComObject(content); }
                if (!isOpened) { Close(); }
            }
            catch (Exception)
            {
                deviceType = (uint)DeviceType.Unknown;
            }
            Marshal.ReleaseComObject(manager);

            return (DeviceType)deviceType;
        }

        /// <summary>
        /// デバイス名を取得する
        /// </summary>
        /// <param name="deviceId">デバイスID</param>
        public virtual string GetDeviceFriendlyName(string deviceId)
        {
            PortableDeviceManager manager = new PortableDeviceManager();
            uint length = 0;
            string friendlyName = String.Empty;
            ushort[] usFriendlyName = null;
            try {
                manager.GetDeviceFriendlyName(deviceId, null, ref length);
            }
            catch (Exception) {
                return friendlyName;
            }

            if (length > 0)
            {
                usFriendlyName = new ushort[length];
                manager.GetDeviceFriendlyName(deviceId, usFriendlyName, ref length);
                friendlyName = ushortArrayToString(usFriendlyName);
            }
            Marshal.ReleaseComObject(manager);

            return friendlyName;
        }

        /// <summary>
        /// デバイス製造元を取得する
        /// </summary>
        /// <param name="deviceId">デバイスID</param>
        public virtual string GetDeviceManufacturer(string deviceId)
        {
            PortableDeviceManager manager = new PortableDeviceManager();
            uint length = 0;
            string manufacturer = String.Empty;
            ushort[] usManufacturerer = null;
            try
            {
                manager.GetDeviceManufacturer(deviceId, null, ref length);
            }
            catch (Exception)
            {
                return manufacturer;
            }

            if (length > 0)
            {
                usManufacturerer = new ushort[length];
                manager.GetDeviceManufacturer(deviceId, usManufacturerer, ref length);
                manufacturer = ushortArrayToString(usManufacturerer);
            }
            Marshal.ReleaseComObject(manager);

            return manufacturer;
        }

        /// <summary>
        /// デバイス説明を取得する
        /// </summary>
        /// <param name="deviceId">デバイスID</param>
        public virtual string GetDeviceDescription(string deviceId)
        {
            PortableDeviceManager manager = new PortableDeviceManager();
            uint length = 0;
            string description = String.Empty;
            ushort[] usDescription = null;
            try
            {
                manager.GetDeviceDescription(deviceId, null, ref length);
            }
            catch (Exception)
            {
                return description;
            }

            if (length > 0)
            {
                usDescription = new ushort[length];
                manager.GetDeviceDescription(deviceId, usDescription, ref length);
                description = ushortArrayToString(usDescription);
            }
            Marshal.ReleaseComObject(manager);

            return description;
        }

        /// <summary>
        /// デバイスに接続する
        /// </summary>
        /// <param name="deviceId">デバイスID</param>
        public virtual void Open(string deviceId)
        {
            if (device != null) { return; }

            // 排他制御用のセマフォを生成する
            if (sem == null)
            {
                var semName = Regex.Replace("WpdMtpSem" + deviceId, "[^0-9a-zA-Z]", "", RegexOptions.Singleline);
                sem = new Semaphore(1, 1, semName);
            }

            device = new PortableDevice();
            IPortableDeviceValues clientInfo = (IPortableDeviceValues)new PortableDeviceTypesLib.PortableDeviceValues();
            device.Open(deviceId, clientInfo);
            Marshal.ReleaseComObject(clientInfo);

            // eventを受信できるようにする
            WpdEvent wpdEvent = new WpdEvent(this);
            IPortableDeviceValues eventParameter = (IPortableDeviceValues)new PortableDeviceTypesLib.PortableDeviceValues();
            device.Advise(0, wpdEvent, eventParameter, out eventCookie);
        }

        /// <summary>
        /// デバイスを切断する
        /// </summary>
        public virtual void Close()
        {
            if (device == null) { return; }
            device.Unadvise(eventCookie);
            device.Close();
            Marshal.ReleaseComObject(device);
            device = null;

            // 排他制御用セマフォを解放する
            sem.Close();
            sem = null;
        }

        /// <summary>
        /// MTPオペレーションを実行する
        /// </summary>
        /// <param name="code"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual MtpResponse Execute(MtpOperationCode code, uint[] param, byte[] data = null)
        {
            if (param == null) { param = new uint[5]; }
            sem.WaitOne();
            MtpResponse res;
            try
            {
                res = MtpOperation.ExecuteCommand(device, code, param, data);
            }
            catch (COMException e)
            {
                Debug.WriteLine("[WpdCommand.Execute] COM Error occured. ErrorCode: 0x" + e.ErrorCode.ToString("x"));
                res = new MtpResponse((ushort)MtpResponseCode.Error, param, data);
            }
            sem.Release();
            return res;
        }

        /// <summary>
        /// MTPオペレーションを実行する
        /// </summary>
        /// <param name="code"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public virtual MtpResponse Execute(ushort code, DataPhase dataPhase, uint[] param, byte[] data = null)
        {
            if (param == null) { param = new uint[5]; }
            sem.WaitOne();
            MtpResponse res;
            try
            {
                res = MtpOperation.ExecuteCommand(device, code, dataPhase, param, data);
            }
            catch (COMException e)
            {
                Debug.WriteLine("[WpdCommand.Execute] COM Error occured. ErrorCode: 0x" + e.ErrorCode.ToString("x"));
                res = new MtpResponse((ushort)MtpResponseCode.Error, param, data);
            }
            sem.Release();
            return res;
        }

        /// <summary>
        /// WPDのイベントを受信するためのクラス
        /// </summary>
        private class WpdEvent : IPortableDeviceEventCallback
        {
            MtpCommand mtpCommand;

            public WpdEvent(MtpCommand mtpCommand)
            {
                this.mtpCommand = mtpCommand;
            }

            public void OnEvent(IPortableDeviceValues pEventParameters)
            {
                Guid eventId;
                if (pEventParameters == null) { return; }
                pEventParameters.GetGuidValue(ref WpdProperty.WPD_EVENT_PARAMETER_EVENT_ID, out eventId);
                Debug.WriteLine("[WpdEvent] Guid: " + eventId.ToString());
                
                ushort mtpEventCode = 0;
                object eventValue = null;
                if (eventId.Equals(WPD_EVENT_OBJECT_ADDED))
                {
                    mtpEventCode = WpdMtpLib.MtpEvent.ObjectAdded;
                    string objectIdStr;
                    pEventParameters.GetStringValue(WpdProperty.WPD_OBJECT_ID, out objectIdStr);
                    Debug.WriteLine("[WpdEvent][ObjectAdded] ObjectID: " + objectIdStr);
                    uint objectId = uint.Parse(objectIdStr.Trim('o'), NumberStyles.HexNumber);
                    eventValue = objectId;
                }
                else if (eventId.Equals(WPD_EVENT_DEVICE_REMOVED))
                {
                    Debug.WriteLine("[WpdEvent] Device Removed. Terminate.");
                    mtpCommand.device.Unadvise(mtpCommand.eventCookie);
                    mtpCommand.device = null;
                    mtpCommand.DeviceEvent(WpdMtpLib.DeviceEvent.Removed, eventValue);
                }
                else if (eventId.Equals(WPD_EVENT_OBJECT_UPDATED) || eventId.Equals(WPD_EVENT_DEVICE_CAPABILITIES_UPDATED))
                {
                    string objectIdStr;
                    pEventParameters.GetStringValue(WpdProperty.WPD_OBJECT_ID, out objectIdStr);
                    Debug.WriteLine("[WpdEvent][ObjectUpdated] ObjectID                : " + objectIdStr);
                    string objectNameStr;
                    pEventParameters.GetStringValue(WpdProperty.WPD_OBJECT_NAME, out objectNameStr);
                    Debug.WriteLine("[WpdEvent][ObjectUpdated] ObjectName              : " + objectNameStr);
                    if (objectIdStr == "DEVICE") {
                        mtpEventCode = WpdMtpLib.MtpEvent.DevicePropChanged;
                    }
                }
                else if (isGuidMtpVendorExtendedEvents(eventId))
                {
                    // MtpEventコードを取得する
                    mtpEventCode = BitConverter.ToUInt16(eventId.ToByteArray(), 2);
                }

                if (mtpEventCode != 0 && mtpCommand.MtpEvent != null)
                {
                    Debug.WriteLine("[WpdEvent] eventCode: 0x" + mtpEventCode.ToString("x4"));
                    mtpCommand.MtpEvent(mtpEventCode, eventValue);
                }
            }
            private bool isGuidMtpVendorExtendedEvents(Guid guid) {
                if (guid == null) { return false; }
                byte[] guidBytes = guid.ToByteArray();
                byte[] mtpVendorExtendedEventsBytes = WPD_EVENT_MTP_VENDOR_EXTENDED_EVENTS.ToByteArray();
                for (int i = 4; i < mtpVendorExtendedEventsBytes.Length; i++)
                {
                    if (guidBytes[i] != mtpVendorExtendedEventsBytes[i]) { return false; }
                }
                return true;
            }
        }
    }
}
