using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpdMtpLib.DeviceProperty
{
    public class ShutterSpeed
    {
        /// <summary>
        /// シャッター速度
        /// </summary>
        public static ulong SS_1_8000 = 0x00001F4000000001; // 1/8000
        public static ulong SS_1_6400 = 0x0000190000000001; // 1/6400
        public static ulong SS_1_6000 = 0x0000177000000001; // 1/6000
        public static ulong SS_1_5000 = 0x0000138800000001; // 1/5000
        public static ulong SS_1_4000 = 0x00000FA000000001; // 1/4000
        public static ulong SS_1_3200 = 0x00000C8000000001; // 1/3200
        public static ulong SS_1_3000 = 0x00000BB800000001; // 1/3000
        public static ulong SS_1_2500 = 0x000009C400000001; // 1/2500
        public static ulong SS_1_2000 = 0x000007D000000001; // 1/2000
        public static ulong SS_1_1600 = 0x0000064000000001; // 1/1600
        public static ulong SS_1_1500 = 0x000005DC00000001; // 1/1500
        public static ulong SS_1_1250 = 0x000004E200000001; // 1/1250
        public static ulong SS_1_1000 = 0x000003E800000001; // 1/1000
        public static ulong SS_1_800  = 0x0000032000000001; // 1/800
        public static ulong SS_1_750  = 0x000002EE00000001; // 1/750
        public static ulong SS_1_640  = 0x0000028000000001; // 1/640
        public static ulong SS_1_500  = 0x000001F400000001; // 1/500
        public static ulong SS_1_400  = 0x0000019000000001; // 1/400
        public static ulong SS_1_350  = 0x0000015E00000001; // 1/350
        public static ulong SS_1_320  = 0x0000014000000001; // 1/320
        public static ulong SS_1_250  = 0x000000FA00000001; // 1/250
        public static ulong SS_1_200  = 0x000000C800000001; // 1/200
        public static ulong SS_1_180  = 0x000000B400000001; // 1/180
        public static ulong SS_1_160  = 0x000000A000000001; // 1/160
        public static ulong SS_1_125  = 0x0000007D00000001; // 1/125
        public static ulong SS_1_100  = 0x0000006400000001; // 1/100
        public static ulong SS_1_90   = 0x0000005A00000001; // 1/90
        public static ulong SS_1_80   = 0x0000005000000001; // 1/80
        public static ulong SS_1_60   = 0x0000003C00000001; // 1/60
        public static ulong SS_1_50   = 0x0000003200000001; // 1/50
        public static ulong SS_1_45   = 0x0000002D00000001; // 1/45
        public static ulong SS_1_40   = 0x0000002800000001; // 1/40
        public static ulong SS_1_30   = 0x0000001E00000001; // 1/30
        public static ulong SS_1_25   = 0x0000001900000001; // 1/25
        public static ulong SS_1_20   = 0x0000001400000001; // 1/20
        public static ulong SS_1_15   = 0x0000000F00000001; // 1/15
        public static ulong SS_1_13   = 0x0000000D00000001; // 1/13
        public static ulong SS_1_10   = 0x0000000A00000001; // 1/10
        public static ulong SS_1_8    = 0x0000000800000001; // 1/8
        public static ulong SS_1_6    = 0x0000000600000001; // 1/6
        public static ulong SS_1_5    = 0x0000000500000001; // 1/5
        public static ulong SS_1_4    = 0x0000000400000001; // 1/4
        public static ulong SS_1_3    = 0x0000000300000001; // 1/3
        public static ulong SS_3_10   = 0x0000000A00000003; // 0.3(3/10)
        public static ulong SS_10_25  = 0x000000190000000A; // 1/2.5(10/25)
        public static ulong SS_4_10   = 0x0000000A00000004; // 0.4(4/10)
        public static ulong SS_1_2    = 0x0000000200000001; // 1/2
        public static ulong SS_5_10   = 0x0000000A00000005; // 0.5(5/10)
        public static ulong SS_6_10   = 0x0000000A00000006; // 0.6(6/10)
        public static ulong SS_10_16  = 0x000000100000000A; // 1/1.6(10/16)
        public static ulong SS_7_10   = 0x0000000A00000007; // 0.7(7/10)
        public static ulong SS_10_13  = 0x0000000D0000000A; // 1/1.3(10/13)
        public static ulong SS_8_10   = 0x0000000A00000008; // 0.8(8/10)
        public static ulong SS_1_1    = 0x0000000100000001; // 1/1
        public static ulong SS_13_10  = 0x0000000A0000000D; // 1.3(13/10)
        public static ulong SS_15_10  = 0x0000000A0000000F; // 1.5(15/10)
        public static ulong SS_16_10  = 0x0000000A00000010; // 1.6(16/10)
        public static ulong SS_2_1    = 0x0000000100000002; // 2
        public static ulong SS_25_10  = 0x0000000A00000019; // 2.5(25/10)
        public static ulong SS_3_1    = 0x0000000100000003; // 3
        public static ulong SS_32_10  = 0x0000000A00000020; // 3.2(32/10)
        public static ulong SS_4_1    = 0x0000000100000004; // 4
        public static ulong SS_5_1    = 0x0000000100000005; // 5
        public static ulong SS_6_1    = 0x0000000100000006; // 6
        public static ulong SS_8_1    = 0x0000000100000008; // 8
        public static ulong SS_10_1   = 0x000000010000000A; // 10
        public static ulong SS_13_1   = 0x000000010000000D; // 13
        public static ulong SS_15_1   = 0x000000010000000F; // 15
        public static ulong SS_20_1   = 0x0000000100000014; // 20
        public static ulong SS_25_1   = 0x0000000100000019; // 25
        public static ulong SS_30_1   = 0x000000010000001E; // 30
        public static ulong SS_60_1   = 0x000000010000003C; // 60
        public static ulong SS_0_1    = 0x0000000100000000; // AUTO(0/1)
        public static ulong SS_AUTO   = 0x0000000000000000; // AUTO

        /// <summary>
        /// 現在値
        /// </summary>
        public ulong Current { get; private set; }

        /// <summary>
        /// 分子
        /// </summary>
        public uint Numer { get; private set; }

        /// <summary>
        /// 分母
        /// </summary>
        public uint Denom { get; private set; }

        /// <summary>
        /// データ
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// バイト列から生成する
        /// </summary>
        /// <param name="data"></param>
        public ShutterSpeed(byte[] data)
        {
            Data = data;
            Numer = BitConverter.ToUInt32(data, 0);
            Denom = BitConverter.ToUInt32(data, 4);
            Current = BitConverter.ToUInt64(data, 0);
        }

        /// <summary>
        /// 分子分母から生成する
        /// </summary>
        /// <param name="numer"></param>
        /// <param name="denom"></param>
        public ShutterSpeed(uint numer, uint denom)
        {
            Numer = numer;
            Denom = denom;
            byte[] data = new byte[8];
            Array.Copy(BitConverter.GetBytes(numer), 0, data, 0, 4);
            Array.Copy(BitConverter.GetBytes(denom), 0, data, 4, 4);
            Data = data;
            Current = BitConverter.ToUInt64(data, 0);
        }

        /// <summary>
        /// 定義から生成する
        /// </summary>
        /// <param name="speed"></param>
        public ShutterSpeed(ulong speed)
        {
            Current = speed;
            Numer = (uint)(speed & 0x00000000FFFFFFFF);
            Denom = (uint)(speed >> 32);
            Data = BitConverter.GetBytes(speed);
        }
    }
}
