using PortableDeviceApiLib;
using System;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace WpdMtpLib
{
    /// <summary>
    /// MTPオペレーションを実行する
    /// </summary>
    internal static class MtpOperation
    {
        /// <summary>
        /// MtpOperationCodeとDataPhaseの一覧
        /// </summary>
        private static Dictionary<MtpOperationCode, DataPhase> OperationCode2DataPhase
            = new Dictionary<MtpOperationCode, DataPhase>()
            {
                // データフェーズのないオペレーション
                {MtpOperationCode.OpenSession, DataPhase.NoDataPhase},
                {MtpOperationCode.CloseSession, DataPhase.NoDataPhase},
                {MtpOperationCode.GetNumObjects, DataPhase.NoDataPhase},
                {MtpOperationCode.DeleteObject, DataPhase.NoDataPhase},
                {MtpOperationCode.InitiateCapture, DataPhase.NoDataPhase},
                {MtpOperationCode.TerminateOpenCapture, DataPhase.NoDataPhase},
                {MtpOperationCode.InitiateOpenCapture, DataPhase.NoDataPhase},
                {MtpOperationCode.StopSelfTimer, DataPhase.NoDataPhase},
                // R->Iのデータフェーズがあるオペレーション
                {MtpOperationCode.GetDeviceInfo, DataPhase.DataReadPhase},
                {MtpOperationCode.GetStorageIDs, DataPhase.DataReadPhase},
                {MtpOperationCode.GetStorageInfo, DataPhase.DataReadPhase},
                {MtpOperationCode.GetObjectHandles, DataPhase.DataReadPhase},
                {MtpOperationCode.GetObjectInfo, DataPhase.DataReadPhase},
                {MtpOperationCode.GetObject, DataPhase.DataReadPhase},
                {MtpOperationCode.GetThumb, DataPhase.DataReadPhase},
                {MtpOperationCode.GetDevicePropDesc, DataPhase.DataReadPhase},
                {MtpOperationCode.GetDevicePropValue, DataPhase.DataReadPhase},
                {MtpOperationCode.GetPartialObject, DataPhase.DataReadPhase},
                // I->Rのデータフェーズがあるオペレーション
                {MtpOperationCode.SetDevicePropValue, DataPhase.DataWritePhase}
            };

        /// <summary>
        /// オペレーションを実行する
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        internal static MtpResponse ExecuteCommand(PortableDevice device, MtpOperationCode code, uint[] param, byte[] sendData)
        {
            DataPhase dataPhaseInfo = OperationCode2DataPhase[code];

            if (dataPhaseInfo == DataPhase.NoDataPhase)
            {
                return executeNoDataCommand(device, (ushort)code, param);
            }
            else if (dataPhaseInfo == DataPhase.DataReadPhase)
            {
                return executeDataReadCommand(device, (ushort)code, param);
            }
            else
            {
                return executeDataWriteCommand(device, (ushort)code, param, sendData);
            }
        }

        /// <summary>
        /// オペレーションを実行する
        /// </summary>
        /// <param name="device"></param>
        /// <returns></returns>
        internal static MtpResponse ExecuteCommand(PortableDevice device, ushort code, DataPhase dataPhase, uint[] param, byte[] sendData)
        {
            if (dataPhase == DataPhase.NoDataPhase)
            {
                return executeNoDataCommand(device, code, param);
            }
            else if (dataPhase == DataPhase.DataReadPhase)
            {
                return executeDataReadCommand(device, code, param);
            }
            else
            {
                return executeDataWriteCommand(device, code, param, sendData);
            }
        }

        /// <summary>
        /// データ転送が無いオペレーションを実行する
        /// </summary>
        /// <param name="device"></param>
        /// <param name="code"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private static MtpResponse executeNoDataCommand(PortableDevice device, ushort code, uint[] param)
        {
            IPortableDeviceValues spResults;
            int ret;
            uint responseCode;
            uint[] responseParam;

            // MTPコマンドとパラメータを構築する
            IPortableDeviceValues mtpCommand = createMtpCommand(code, WpdProperty.WPD_COMMAND_MTP_EXT_EXECUTE_COMMAND_WITHOUT_DATA_PHASE);
            IPortableDevicePropVariantCollection mtpCommandParam = null;
            if (param != null)
            {
                mtpCommandParam = createMtpCommandParameter(param);
                mtpCommand.SetIPortableDevicePropVariantCollectionValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_OPERATION_PARAMS, mtpCommandParam);
            }
            // リクエストを送信
            device.SendCommand(0, mtpCommand, out spResults);
            // コマンドとパラメータは以後不要なので解放
            if (mtpCommandParam != null) { Marshal.ReleaseComObject(mtpCommandParam); }
            Marshal.ReleaseComObject(mtpCommand);
            // リクエストの結果を取得する
            spResults.GetErrorValue(ref WpdProperty.WPD_PROPERTY_COMMON_HRESULT, out ret);
            if (ret != 0)
            {   // エラーなら終了
                Marshal.ReleaseComObject(spResults);
                return new MtpResponse(0, null, null);
            }
            // レスポンスコードを取得する
            spResults.GetUnsignedIntegerValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_RESPONSE_CODE, out responseCode);
            // レスポンスパラメータを取得する
            responseParam = null;
            if (responseCode == 0x2001)
            {
                IPortableDevicePropVariantCollection resultValues
                    = (IPortableDevicePropVariantCollection)new PortableDeviceTypesLib.PortableDevicePropVariantCollection();
                spResults.GetIPortableDevicePropVariantCollectionValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_RESPONSE_PARAMS, out resultValues);

                uint count = 1;
                resultValues.GetCount(ref count);
                responseParam = new uint[count];
                for (uint i = 0; i < count; i++)
                {
                    tag_inner_PROPVARIANT value = new tag_inner_PROPVARIANT();
                    resultValues.GetAt(i, ref value);
                    responseParam[i] = getUintValue(value);
                }
                Marshal.ReleaseComObject(resultValues);
            }
            Marshal.ReleaseComObject(spResults);

            return new MtpResponse((ushort)responseCode, responseParam, null);
        }

        /// <summary>
        /// データ受信を伴うMTPオペレーションを実行する
        /// </summary>
        /// <param name="device"></param>
        /// <param name="code"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private static MtpResponse executeDataReadCommand(PortableDevice device, ushort code, uint[] param)
        {
            IPortableDeviceValues spResults;
            int ret;
            string context;
            uint size;
            byte[] bufferIn;
            IntPtr dataAddress;
            uint readedSize;
            IntPtr dataPtr;
            byte[] bufferOut = null;
            uint responseCode;
            uint[] responseParam;

            // MTPコマンドとパラメータを構築する
            IPortableDeviceValues mtpCommand = createMtpCommand(code, WpdProperty.WPD_COMMAND_MTP_EXT_EXECUTE_COMMAND_WITH_DATA_TO_READ);
            IPortableDevicePropVariantCollection mtpCommandParam = null;
            if (param != null)
            {
                mtpCommandParam = createMtpCommandParameter(param);
                mtpCommand.SetIPortableDevicePropVariantCollectionValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_OPERATION_PARAMS, mtpCommandParam);
            }
            // リクエストを送信
            device.SendCommand(0, mtpCommand, out spResults);
            // コマンドとパラメータは以後不要なので解放
            if (mtpCommandParam != null) { Marshal.ReleaseComObject(mtpCommandParam); }
            Marshal.ReleaseComObject(mtpCommand);
            // リクエストの結果を取得する
            spResults.GetErrorValue(ref WpdProperty.WPD_PROPERTY_COMMON_HRESULT, out ret);
            if (ret != 0)
            {   // エラーなら終了
                Marshal.ReleaseComObject(spResults);
                return new MtpResponse(0, null, null);
            }
            // コンテキストを取得する
            spResults.GetStringValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_TRANSFER_CONTEXT, out context);
            // データサイズを取得する
            spResults.GetUnsignedIntegerValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_TRANSFER_TOTAL_DATA_SIZE, out size);
            Marshal.ReleaseComObject(spResults);

            if (size > 0)
            {
                // データ受信用のコマンドを構築する
                bufferIn = new byte[size];
                mtpCommand = (IPortableDeviceValues)new PortableDeviceTypesLib.PortableDeviceValues();
                mtpCommand.SetGuidValue(ref WpdProperty.WPD_PROPERTY_COMMON_COMMAND_CATEGORY, ref WpdProperty.WPD_COMMAND_MTP_EXT_READ_DATA.fmtid);
                mtpCommand.SetUnsignedIntegerValue(ref WpdProperty.WPD_PROPERTY_COMMON_COMMAND_ID, WpdProperty.WPD_COMMAND_MTP_EXT_READ_DATA.pid);
                mtpCommand.SetStringValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_TRANSFER_CONTEXT, context);
                mtpCommand.SetBufferValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_TRANSFER_DATA, ref bufferIn[0], size);
                mtpCommand.SetUnsignedIntegerValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_TRANSFER_NUM_BYTES_TO_READ, size);
                // データを受信する
                device.SendCommand(0, mtpCommand, out spResults);
                Marshal.ReleaseComObject(mtpCommand);
                spResults.GetErrorValue(ref WpdProperty.WPD_PROPERTY_COMMON_HRESULT, out ret);
                if (ret != 0)
                {   // エラーなら終了
                    Marshal.ReleaseComObject(spResults);
                    return new MtpResponse(0, null, null);
                }
                // 受信データのアドレスを取得する
                dataAddress = Marshal.AllocCoTaskMem(4);
                spResults.GetBufferValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_TRANSFER_DATA, dataAddress, out readedSize);
                dataPtr = new IntPtr(Marshal.ReadInt32(dataAddress));
                Marshal.FreeCoTaskMem(dataAddress);
                // 受信データを取得する
                bufferOut = new byte[size];
                Marshal.Copy(dataPtr, bufferOut, 0, bufferOut.Length);
                Marshal.ReleaseComObject(spResults);
            }

            // DataEndTransfer(レスポンス)を送信する
            sendEndDataTransfer(device, context, out responseCode, out responseParam);

            return new MtpResponse((ushort)responseCode, responseParam, bufferOut);
        }
           
        /// <summary>
        /// データ送信を伴うMTPオペレーションを実行する
        /// </summary>
        /// <param name="device"></param>
        /// <param name="code"></param>
        /// <param name="param"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static MtpResponse executeDataWriteCommand(PortableDevice device, ushort code, uint[] param, byte[] data)
        {
            int ret = 1;
            string context;
            uint writtenSize;
            uint responseCode;
            uint[] responseParam;
            IPortableDeviceValues spResults;

            // MTPコマンドとパラメータを構築する
            IPortableDeviceValues mtpCommand = createMtpCommand(code, WpdProperty.WPD_COMMAND_MTP_EXT_EXECUTE_COMMAND_WITH_DATA_TO_WRITE);
            IPortableDevicePropVariantCollection mtpCommandParam = null;
            if (param != null)
            {
                mtpCommandParam = createMtpCommandParameter(param);
                mtpCommand.SetIPortableDevicePropVariantCollectionValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_OPERATION_PARAMS, mtpCommandParam);
            }
            // 送信するデータのサイズを設定する
            mtpCommand.SetUnsignedLargeIntegerValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_TRANSFER_TOTAL_DATA_SIZE, (ulong)data.Length);
            // リクエストを送信
            device.SendCommand(0, mtpCommand, out spResults);
            // コマンドとパラメータは以後不要なので解放
            if (mtpCommandParam != null) { Marshal.ReleaseComObject(mtpCommandParam); }
            Marshal.ReleaseComObject(mtpCommand);
            // リクエストの結果を取得する
            spResults.GetErrorValue(ref WpdProperty.WPD_PROPERTY_COMMON_HRESULT, out ret);
            if (ret != 0)
            {   // エラーなら終了
                Marshal.ReleaseComObject(spResults);
                return new MtpResponse(0, null, null);
            }
            // contextを取得する
            spResults.GetStringValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_TRANSFER_CONTEXT, out context);
            Marshal.ReleaseComObject(spResults);

            // データ送信用のコマンドを構築する
            mtpCommand = (IPortableDeviceValues)new PortableDeviceTypesLib.PortableDeviceValues();
            mtpCommand.SetGuidValue(ref WpdProperty.WPD_PROPERTY_COMMON_COMMAND_CATEGORY, ref WpdProperty.WPD_COMMAND_MTP_EXT_WRITE_DATA.fmtid);
            mtpCommand.SetUnsignedIntegerValue(ref WpdProperty.WPD_PROPERTY_COMMON_COMMAND_ID, WpdProperty.WPD_COMMAND_MTP_EXT_WRITE_DATA.pid);
            mtpCommand.SetStringValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_TRANSFER_CONTEXT, context);
            mtpCommand.SetUnsignedIntegerValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_TRANSFER_NUM_BYTES_TO_WRITE, (uint)data.Length);
            mtpCommand.SetBufferValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_TRANSFER_DATA, ref data[0], (uint)data.Length);
            // データを送信する
            device.SendCommand(0, mtpCommand, out spResults);
            Marshal.ReleaseComObject(mtpCommand);
            // データ送信の結果を取得する
            spResults.GetErrorValue(ref WpdProperty.WPD_PROPERTY_COMMON_HRESULT, out ret);
            if (ret != 0)
            {   // エラーなら終了
                Marshal.ReleaseComObject(spResults);
                if (mtpCommandParam != null) { Marshal.ReleaseComObject(mtpCommandParam); }
                return new MtpResponse(0, null, null);
            }
            // デバイスが受け取ったデータサイズを取得する
            spResults.GetUnsignedIntegerValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_TRANSFER_NUM_BYTES_WRITTEN, out writtenSize);
            Marshal.ReleaseComObject(spResults);

            // DataEndTransfer(レスポンス)を送信する
            sendEndDataTransfer(device, context, out responseCode, out responseParam);

            return new MtpResponse((ushort)responseCode, responseParam, null);
        }
        
        /// <summary>
        /// EndDataTransferを送信する
        /// </summary>
        /// <param name="device"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        private static void sendEndDataTransfer(PortableDevice device, string context, out uint responseCode, out uint[] responseParam)
        {
            IPortableDeviceValues mtpEndDataTransfer = (IPortableDeviceValues)new PortableDeviceTypesLib.PortableDeviceValues();
            mtpEndDataTransfer.SetGuidValue(ref WpdProperty.WPD_PROPERTY_COMMON_COMMAND_CATEGORY, ref WpdProperty.WPD_COMMAND_MTP_EXT_END_DATA_TRANSFER.fmtid);
            mtpEndDataTransfer.SetUnsignedIntegerValue(ref WpdProperty.WPD_PROPERTY_COMMON_COMMAND_ID, WpdProperty.WPD_COMMAND_MTP_EXT_END_DATA_TRANSFER.pid);
            mtpEndDataTransfer.SetStringValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_TRANSFER_CONTEXT, context);
            
            IPortableDeviceValues spResults;
            device.SendCommand(0, mtpEndDataTransfer, out spResults);
            Marshal.ReleaseComObject(mtpEndDataTransfer);

            int ret = 1;
            spResults.GetErrorValue(ref WpdProperty.WPD_PROPERTY_COMMON_HRESULT, out ret);
            if (ret != 0)
            {
                Marshal.ReleaseComObject(spResults);
                responseCode = 0;
                responseParam = null;
                return;
            }

            // レスポンスコード
            spResults.GetUnsignedIntegerValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_RESPONSE_CODE, out responseCode);

            // パラメータ
            responseParam = null;
            if (responseCode == 0x2001)
            {
                IPortableDevicePropVariantCollection resultValues
                    = (IPortableDevicePropVariantCollection)new PortableDeviceTypesLib.PortableDevicePropVariantCollection();
                spResults.GetIPortableDevicePropVariantCollectionValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_RESPONSE_PARAMS, out resultValues);

                uint count = 1;
                resultValues.GetCount(ref count);
                responseParam = new uint[count];
                for (uint i = 0; i < count; i++)
                {
                    tag_inner_PROPVARIANT value = new tag_inner_PROPVARIANT();
                    resultValues.GetAt(i, ref value);
                    responseParam[i] = getUintValue(value);
                }
                Marshal.ReleaseComObject(resultValues);
            }
            
            Marshal.ReleaseComObject(spResults);
        }


        /// <summary>
        /// MTPコマンドを構築する
        /// </summary>
        /// <param name="code">オペレーションコード</param>
        /// <returns></returns>
        private static IPortableDeviceValues createMtpCommand(ushort code, _tagpropertykey dataPhase)
        {
            IPortableDeviceValues mtpCommand = (IPortableDeviceValues)new PortableDeviceTypesLib.PortableDeviceValues();
            mtpCommand.SetGuidValue(ref WpdProperty.WPD_PROPERTY_COMMON_COMMAND_CATEGORY, ref dataPhase.fmtid);
            mtpCommand.SetUnsignedIntegerValue(ref WpdProperty.WPD_PROPERTY_COMMON_COMMAND_ID, dataPhase.pid);
            mtpCommand.SetUnsignedIntegerValue(ref WpdProperty.WPD_PROPERTY_MTP_EXT_OPERATION_CODE, (uint)code);

            return mtpCommand;
        }

        /// <summary>
        /// MTPコマンドのパラメータを構築する
        /// </summary>
        /// <param name="param">パラメータ</param>
        /// <returns></returns>
        private static IPortableDevicePropVariantCollection createMtpCommandParameter(uint[] param)
        {
            IPortableDevicePropVariantCollection mtpCommandParameter
                = (IPortableDevicePropVariantCollection)new PortableDeviceTypesLib.PortableDevicePropVariantCollection();
            foreach (uint p in param)
            {
                tag_inner_PROPVARIANT propValiant = createPropVariant(p);
                mtpCommandParameter.Add(ref propValiant);
            }

            return mtpCommandParameter;
        }

        /// <summary>
        /// uint型のtag_inner_PROPVARIANTを生成する
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static tag_inner_PROPVARIANT createPropVariant(uint value)
        {
            tag_inner_PROPVARIANT propVariant = new tag_inner_PROPVARIANT();

            IPortableDeviceValues pdValues = (IPortableDeviceValues)new PortableDeviceTypesLib.PortableDeviceValues();
            pdValues.SetUnsignedIntegerValue(ref WpdProperty.WPD_OBJECT_ID, value);
            pdValues.GetValue(ref WpdProperty.WPD_OBJECT_ID, out propVariant);
            Marshal.ReleaseComObject(pdValues);

            return propVariant;
        }

        /// <summary>
        /// tag_inner_PROPVARIANT型からuintの値を取得する
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static uint getUintValue(tag_inner_PROPVARIANT value)
        {
            uint ret = 1;

            IPortableDeviceValues pdValues = (IPortableDeviceValues)new PortableDeviceTypesLib.PortableDeviceValues();
            pdValues.SetValue(ref WpdProperty.WPD_OBJECT_ID, ref value);
            pdValues.GetUnsignedIntegerValue(ref WpdProperty.WPD_OBJECT_ID, out ret);
            Marshal.ReleaseComObject(pdValues);

            return ret;
        }

    }
}
