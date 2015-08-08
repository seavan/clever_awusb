using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace cvawusb_batch
{
    public partial class Flow : IExecutionControl
    {
        public bool Execute(string name)
        {
            var item = Find(name);
            stack.Push(item);
            return Find(name).ExecuteSequence(this);
        }

        public bool Exists(string name)
        {
            return Item.Any(s => Match(s, name));
        }

        public FlowItem Find(string name)
        {
            return Item.Single(s => Match(s, name));
        }

        public bool Match(FlowItem item, string name)
        {
            return String.Equals(item.id, name, StringComparison.CurrentCultureIgnoreCase);
        }

        public bool Call(string name)
        {
            var item = Find(name);
            if (stack.Contains(item))
            {
                Console.WriteLine("No loops allowed. Loop detected:");
                foreach (var i in stack)
                {
                    Console.WriteLine("{0}: {1}", i.id, i.title);
                }

                return false;
            }

            var result = Execute(item.id);
            stack.Pop();
            return result;
        }

        CallStack stack = new CallStack();
    }
}
