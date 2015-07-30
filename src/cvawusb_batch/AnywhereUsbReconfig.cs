using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace cvawusb_batch
{
    public class AnywhereUsbReconfig
    {
        public const int GROUP_COUNT = 14;
        public const int GROUP_UNASSIGNED = 0;
        private const string PROTO_FORMAT = "http://{0}";
        private const string REALPORT_CONFIG_URL = "/config/applications/realport_usb_config.htm";
        private const string REALPORT_POST_CONFIG_URL = "/Forms/realport_usb_config_1";
        private const string PORT_GROUP_REGEX = @"<select.*?(RpUsbGroupList\?\d+).*?>.*?</select>";
        private const string PORT_GROUP_SELECTED_REGEX = @"option value=(.*?) selected";
        private const string PORT_FORMAT = "RpUsbGroupList?{0}";
        private const string GROUP_FORMAT = "{0:X2}000000";
        private KeyValuePair<string, string> DGA_PARAM = new KeyValuePair<string, string>("dga_enabled", "on");
        private KeyValuePair<string, string> SUBMIT_PARAM = new KeyValuePair<string, string>("Submit", "Apply");

        private string UsbHostAddress = "";

        private Dictionary<string, string> UsbHostParams = new Dictionary<string, string>();

        public AnywhereUsbReconfig(string address)
        {
            UsbHostAddress = address;
        }

        public string FormatPort(int port)
        {
            return String.Format(PORT_FORMAT, port);
        }

        public string FormatGroupNumber(int value)
        {
            return String.Format(GROUP_FORMAT, value);
        }

        public void SetParam(int port, int value)
        {
            UsbHostParams[FormatPort(port)] = FormatGroupNumber(value);
        }

        public int PortGroupToInt(string value)
        {
            value = value.TrimEnd('0');
            return String.IsNullOrEmpty(value) ? 0 : Convert.ToInt32(value, 16);
        }

        public int GetParam(int port)
        {
            var portName = FormatPort(port);

            if (UsbHostParams.ContainsKey(portName))
            {
                return PortGroupToInt(UsbHostParams[portName]);
            }

            return -1;
        }

        public void LoadConfig()
        {
            using (var client = new WebClient())
            {
                //var dataToPost = Encoding.Default.GetBytes("param1=value1&param2=value2");
                var result = client.DownloadString(String.Format(PROTO_FORMAT, UsbHostAddress) + REALPORT_CONFIG_URL);
                var regex = new Regex(PORT_GROUP_REGEX,
                    RegexOptions.Singleline | RegexOptions.IgnoreCase);
                var matches = regex.Matches(result);

                UsbHostParams.Clear();

                foreach (Match m in matches)
                {
                    var realPortGroupName = m.Groups[1].Value;
                    var realPortSelected = "";
                    var innerMatch = new Regex(PORT_GROUP_SELECTED_REGEX,
                        RegexOptions.IgnoreCase | RegexOptions.Multiline);
                    var innerMatchResult = innerMatch.Match(m.Groups[0].Value);
                    if (innerMatchResult.Success)
                    {
                        realPortSelected = innerMatchResult.Groups[1].Value;
                    }
                    Console.WriteLine("{0} - {1}", realPortGroupName, realPortSelected);
                    UsbHostParams[realPortGroupName] = realPortSelected;
                }
            }
        }

        public void SaveConfig()
        {
            using (var client = new WebClient())
            {
                var requestParams = new System.Collections.Specialized.NameValueCollection();

                foreach (var item in UsbHostParams)
                {
                    requestParams.Add(item.Key, item.Value);
                }

                requestParams.Add(DGA_PARAM.Key, DGA_PARAM.Value);
                requestParams.Add(SUBMIT_PARAM.Key, SUBMIT_PARAM.Value);

                byte[] responseBytes = client.UploadValues(String.Format(PROTO_FORMAT, UsbHostAddress) + REALPORT_POST_CONFIG_URL, "POST", requestParams);

                //string responsebody = Encoding.UTF8.GetString(responsebytes);
            }
        }

        void ReadSettings()
        {
            
        }
    }
}
