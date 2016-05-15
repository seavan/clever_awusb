using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvawusb_batch
{
    public partial class FlowItemSequenceConnect : IFlowItemSequenceItemBase
    {
        public bool Execute(IExecutionControl executionControl)
        {
            Console.WriteLine("Connecting to device {0}", device);
            var usbDeviceLookup = new UsbDeviceLookup();
            usbDeviceLookup.Test();
            if (!usbDeviceLookup.WaitForConnection(device))
            {
                Console.WriteLine("Error connecting to device {0}", device);
                return false;
            }

            return true;
        }
    }
}
