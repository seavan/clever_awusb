using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cvawusb_batch
{
    public class FlowConfigReader
    {
        public static Flow Read(string fileName)
        {
            var reader =
     new System.Xml.Serialization.XmlSerializer(typeof(Flow));
            using (var file = new System.IO.StreamReader(fileName))
            {
                return (Flow)reader.Deserialize(file);
            }
        }
    }
}
