using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cvawusb_batch
{
    class Program
    {
        static void Test1()
        {
            while (true)
            {
                var searchDevice = "VID_2022&PID_0139&MI_00";
                var awUsbReconfig = new AnywhereUsbReconfig("192.168.11.122");
                awUsbReconfig.LoadConfig();

                for (int i = 1; i < AnywhereUsbReconfig.GROUP_COUNT; ++i)
                {
                    Console.WriteLine("Port {0} has group {1}", i, awUsbReconfig.GetParam(i));
                }

                var usbDeviceLookup = new UsbDeviceLookup();
                usbDeviceLookup.Test();

                usbDeviceLookup.WaitForConnection(searchDevice);

                Console.WriteLine("Enter port:");
                var portString = Console.ReadLine();
                Console.WriteLine("Enter group:");
                var groupString = Console.ReadLine();

                if (!String.IsNullOrEmpty(portString) && !String.IsNullOrEmpty(groupString))
                {
                    var portInt = Convert.ToInt32(portString);
                    var groupInt = Convert.ToInt32(groupString);

                    awUsbReconfig.SetParam(portInt, groupInt);

                    awUsbReconfig.SaveConfig();
                }
            }            
        }
        static void Main(string[] args)
        {
            var writer = new DuplicateWriter();
            Console.SetOut(writer);
            Console.WriteLine("Clever AnywhereUSB-14 multi-host remote control tool");
            Console.WriteLine("Usage: cvawusb_batch [group name] [--list] [--alert]");
            Console.WriteLine("Loading flow");

            var flow = FlowConfigReader.Read("Flow.xml");


            foreach (var item in flow.Item)
            {
                Console.WriteLine("{0} - {1}", item.title, item.id);
            }

            string testGroup = null;

            if (args.Length > 0)
            {
                testGroup = args.FirstOrDefault(s => !s.StartsWith("--"));
            }

            bool listDevices = args.Any(s => s == "--list");

            if (listDevices)
            {
                var lookup = new UsbDeviceLookup();
                var devices = lookup.GetUSBDevices();
                foreach (var d in devices)
                {
                    Console.WriteLine(d.DeviceID);
                }    
            }

            bool result = false;

            if (testGroup != null)
            {
                if (!flow.Exists(testGroup))
                {
                    Console.WriteLine("Flow item id not found in config file");
                    return;
                }
                var selectedItem = flow.Find(testGroup);
                Console.WriteLine("Executing {0}", selectedItem.id);
                result = flow.Execute(selectedItem.id);
            }

            bool alert = args.Any(s => s == "--alert");

            if (alert && !result)
            {
                Console.WriteLine("Could not complete the sequence. Sending alert.");
                AlertMailer.Send(writer.GetAll());
            }
        }
    }
}
