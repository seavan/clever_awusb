using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvawusb_batch
{
    public partial class FlowItemSequenceDisconnect : IFlowItemSequenceItemBase
    {
        public bool Execute(IExecutionControl executionControl)
        {
            Console.WriteLine("Disconnecting device {0}", device);
            var usbDeviceLookup = new UsbDeviceLookup();
            usbDeviceLookup.Test();
            if (!usbDeviceLookup.WaitForDisconnection(device))
            {
                return false;
            }

            return true;
        }
    }
}
