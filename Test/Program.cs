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
            MtpCommand command = new MtpCommand();
            string[] deviceIds = command.GetDeviceIds();
            if (deviceIds.Length == 0) { return; }
            command.Open(deviceIds[0]);
            command.Close();
        }
    }
}
