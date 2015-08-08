using System;

namespace cvawusb_batch
{
    public partial class FlowItemSequenceCall : IFlowItemSequenceItemBase
    {
        public bool Execute(IExecutionControl executionControl)
        {
            Console.WriteLine("Calling {0}", id);
            return executionControl.Call(id);
        }
    }
}