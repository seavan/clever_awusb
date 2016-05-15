using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace cvawusb_batch
{
    public class DuplicateWriter : TextWriter
    {
        public DuplicateWriter()
        {
            original = Console.Out;
        }

        public string[] GetAll()
        {
            return text.ToArray();
        }

        private TextWriter original;
        private List<string> text = new List<string>();

        public override void Write(string value)
        {
            text.Add(value);
            original.Write(value);
        }

        public override void WriteLine(string format)
        {
            this.Write(format + "\r\n");
        }

        public override void WriteLine(string format, params object[] arg)
        {
            this.Write(String.Format(format, arg) + "\r\n");
        }

        public override Encoding Encoding
        {
            get { return Encoding.Default; }
        }
    }
}