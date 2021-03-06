﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cvawusb_batch;
using cvawusb_gui.Properties;

namespace cvawusb_gui
{
    public partial class MainForm : Form
    {
        public class TextBoxWriter : TextWriter
        {
            public TextBoxWriter(TextBox output)
            {
                textBox = output;
                textBox.ScrollBars = ScrollBars.Both;
                
            }

            private TextBox textBox;

            public override void Write(string value)
            {
                textBox.Text += value;
                textBox.Select(textBox.TextLength - 1, 0);
                textBox.ScrollToCaret();
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


        [DllImport("user32.dll")]
        public static extern bool ShowWindowAsync(HandleRef hWnd, int nCmdShow);
        public const int SW_RESTORE = 9;

        public bool SwitchToCurrent()
        {
            IntPtr hWnd = IntPtr.Zero;
            Process process = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(process.ProcessName);
            foreach (Process _process in processes)
            {
                // Get the first instance that is not this instance, has the
                // same process name and was started from the same file name
                // and location. Also check that the process has a valid
                // window handle in this session to filter out other user's
                // processes.

                if (_process.Id != process.Id &&
                  _process.MainModule.FileName == process.MainModule.FileName &&
                  _process.MainWindowHandle != IntPtr.Zero)
                {
                    hWnd = _process.MainWindowHandle;
                    return true;
                }
            }

            return false;
        }

        public MainForm()
        {
            if (SwitchToCurrent())
            {
                MessageBox.Show(Resources.MainForm_ALreadyLaunched,
                    Resources.MainForm_AppTitle);
                Application.Exit();
                Close();
                return;
            }

            InitializeComponent();
            Load += (sender, args) =>
            {
                Console.SetOut(new TextBoxWriter(this.textBox1));
                Console.WriteLine("Loading flow");

                MyFlow = FlowConfigReader.Read("Flow.xml");

                foreach (var flowItem in MyFlow.Item.Where(s => !String.IsNullOrEmpty(s.title)))
                {
                    AddTaskButton(new ButtonInfo() { Title = flowItem.title, Task = flowItem.id });
                }

                ExpandedLogHeight = textBox1.Height;
                ContractedLogHeight = logToggle.Height;
                logToggle_Click(null, null);
                SetStatusText("Выберите ключ для подключения:");
            };

        }

        private Flow MyFlow;
        private const int MARGIN = 10;
        private const int SIZE = 80;

        private List<Button> Buttons = new List<Button>();

        public void ExecuteTask(string name)
        {
            if (MyFlow.Execute(name))
            {
                SetStatusText(String.Format("Ключ \"{0}\" успешно подключен: ", MyFlow.Find(name).title));
            }
            else
            {
                SetStatusText(String.Format("Ошибка подключения ключа \"{0}\"", MyFlow.Find(name).title, false));
            }
            
        }

        public void AddTaskButton(ButtonInfo bi)
        {
            var button = new Button();
            button.Width = SIZE;
            button.Height = SIZE;
            button.Text = bi.Title;
            button.Left =
                Buttons.Count > 0 ? Buttons.Max(s => s.Left + s.Width + MARGIN) : 0;
            button.Top = MARGIN;
            Container.Controls.Add(button);

            button.Click += (sender, args) => ExecuteTask(bi.Task);

            Buttons.Add(button);
        }

        public class ButtonInfo
        {
            public string Title { get; set; }
            public string Task { get; set; }
        }

        private void logToggle_Click(object sender, EventArgs e)
        {
            var bottom = textBox1.Bottom;
            if (textBox1.Height < ExpandedLogHeight)
            {
                textBox1.Height = ExpandedLogHeight;
                textBox1.Width += logToggle.Width;
                textBox1.Left -= logToggle.Width;
                //textBox1.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            }
            else
            {
                textBox1.Height = ContractedLogHeight;
                textBox1.Width -= logToggle.Width;
                textBox1.Left += logToggle.Width;
                //textBox1.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
            }
            textBox1.Top = bottom - textBox1.Height;
            logToggle.Top = textBox1.Top;
        }

        public void SetStatusText(string text, bool alert = false)
        {
            hintLabel.Text = text;
            if (alert)
            {
                hintLabel.ForeColor = Color.Red;
            }
            else
            {
                hintLabel.ForeColor = Color.Green;
            }
        }

        private int ExpandedLogHeight = 0;
        private int ContractedLogHeight = 0;

    }
}
