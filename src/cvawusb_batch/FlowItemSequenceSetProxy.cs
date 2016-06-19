using System;

namespace cvawusb_batch
{
    public partial class FlowItemSequenceSetProxy : IFlowItemSequenceItemBase
    {
        public bool Execute(IExecutionControl executionControl)
        {
            Console.WriteLine("Setting proxy to {0}, {1}", proxy, enabled);
            ProxyReconfig.SetProxy(proxy, enabled);
            return true;
        }
    }
}