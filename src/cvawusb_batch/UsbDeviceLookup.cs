using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Threading;
using System.Threading.Tasks;

namespace cvawusb_batch
{
    public class UsbDeviceLookup
    {
        public const int INITIAL_DELAY = 5000;
        public const int RESCAN_DELAY = 1000;
        public const int TIMEOUT = 30000;
        public const int STEP = 200;

        public void Delay(string message, int ms)
        {
            Console.Write(message);
            while (ms >= 0)
            {
                Console.Write(".");
                ms -= STEP;
                Thread.Sleep(STEP);
            }
            
        }

        public bool WaitFor(string deviceIdMask, Func<string, bool> scan, string success, string fail )
        {
            Delay("Initial delay", INITIAL_DELAY);
            Console.WriteLine();

            int timeout = TIMEOUT;
            Console.WriteLine("Looking for device {0}", deviceIdMask);
            while (timeout >= 0)
            {
                if (scan(deviceIdMask))
                {
                    Console.WriteLine(success);
                    return true;
                }
                Delay("", RESCAN_DELAY);
                timeout -= RESCAN_DELAY;
            }
            Console.WriteLine(fail);
            return false;
        }

        public bool WaitForConnection(string deviceIdMask)
        {
            return WaitFor(deviceIdMask, ScanDeviceExists,
                "Device found! Connection successfull", "Device not found. Connection failed.");
        }

        public bool WaitForDisconnection(string deviceIdMask)
        {
            return WaitFor(deviceIdMask, (a) => !ScanDeviceExists(a),
                "Device not found! Disconnection successfull", "Device found. Disconnection failed.");
        }

        public bool ScanDeviceExists(string deviceIdMask)
        {
            deviceIdMask = deviceIdMask.ToLower();
            var info = GetUSBDevices();
            return info.Any(s => s.DeviceID.ToLower().Contains(deviceIdMask));

        }
        public List<USBDeviceInfo> GetUSBDevices()
        {
            var devices = new List<USBDeviceInfo>();

            ManagementObjectCollection collection;
            using (var searcher = new ManagementObjectSearcher(@"Select * From Win32_PnPEntity"))
                collection = searcher.Get();

            foreach (var device in collection)
            {
                var deviceId = (string) device.GetPropertyValue("DeviceID");
                if(!deviceId.ToLower().Contains("usb")) continue;
                
                devices.Add(new USBDeviceInfo(
                deviceId,
                (string)device.GetPropertyValue("PNPDeviceID"),
                (string)device.GetPropertyValue("Description")
                ));
            }

            collection.Dispose();
            return devices;
        }
        public void Test()
        {
            var usbDevices = GetUSBDevices();

            foreach (var usbDevice in usbDevices)
            {
                Console.WriteLine("Device ID: {0}, PNP Device ID: {1}, Description: {2}",
                    usbDevice.DeviceID, usbDevice.PnpDeviceID, usbDevice.Description);
            }

        }

    }
}
