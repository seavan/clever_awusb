using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvawusb_batch
{
    public partial class FlowItemSequenceSetPort : IFlowItemSequenceItemBase
    {
        public bool Execute(IExecutionControl executionControl)
        {
            Console.WriteLine("Reconfiguring {0}", ip);
            var awUsbReconfig = new AnywhereUsbReconfig(ip);
            awUsbReconfig.LoadConfig();
            var portToSet = Convert.ToInt32(port);
            var valueToSet = Convert.ToInt32(value);

            Console.WriteLine("Current {0} value {1}", portToSet, awUsbReconfig.GetParam(portToSet));
            Console.WriteLine("Setting {0} value to {1}", portToSet, valueToSet);
            awUsbReconfig.SetParam(portToSet, valueToSet);
            awUsbReconfig.SaveConfig();
            Console.WriteLine("Checking");
            awUsbReconfig.LoadConfig();
            var newParam = awUsbReconfig.GetParam(portToSet);
            Console.WriteLine("Current {0} value {1}", portToSet, newParam);
            if (newParam == valueToSet)
            {
                Console.WriteLine("Change successfull");
            }
            else
            {
                Console.WriteLine("Unsuccessfull. Aborting");
                return false;
            }

            return true;
        }
    }
}
