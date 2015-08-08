using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvawusb_batch
{
    public partial class FlowItem
    {
        public bool ExecuteSequence(IExecutionControl executionControl)
        {
            foreach (IFlowItemSequenceItemBase seq in Sequence.Items)
            {
                if (seq.Execute(executionControl))
                {
                    Console.WriteLine("Step executed");
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
