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
            Console.WriteLine("Loading flow");

            var flow = FlowConfigReader.Read("Flow.xml");


            foreach (var item in flow.Item)
            {
                Console.WriteLine("{0} - {1}", item.title, item.id);
            }

            var testGroup = "group3";

            if (args.Length > 0)
            {
                testGroup = args[0];
            }

            var selectedItem = flow.Item.SingleOrDefault(s => s.id == testGroup);

            if (selectedItem == null)
            {
                Console.WriteLine("Flow item id not found in config file");
            }

            Console.WriteLine("Executing {0}", selectedItem.title);

            foreach (var seq in selectedItem.Sequence.Items)
            {
                var connect = seq as FlowItemSequenceConnect;
                var disconnect = seq as FlowItemSequenceDisconnect;
                var port = seq as FlowItemSequenceSetPort;

                if (connect != null)
                {
                    Console.WriteLine("Connecting to device {0}", connect.device);
                    var usbDeviceLookup = new UsbDeviceLookup();
                    usbDeviceLookup.Test();
                    if (!usbDeviceLookup.WaitForConnection(connect.device))
                    {
                        break;
                    }
                }

                if (disconnect != null)
                {
                    Console.WriteLine("Disconnecting device {0}", disconnect.device);
                    var usbDeviceLookup = new UsbDeviceLookup();
                    usbDeviceLookup.Test();
                    if (!usbDeviceLookup.WaitForDisconnection(disconnect.device))
                    {
                        break;
                    }
                }

                if (port != null)
                {
                    Console.WriteLine("Reconfiguring {0}", port.ip);
                    var awUsbReconfig = new AnywhereUsbReconfig(port.ip);
                    awUsbReconfig.LoadConfig();
                    var portToSet = Convert.ToInt32(port.port);
                    var valueToSet = Convert.ToInt32(port.value);

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
                        break;
                    }
                }
            }
        }
    }
}
