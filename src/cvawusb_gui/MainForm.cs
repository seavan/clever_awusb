using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cvawusb_batch;

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
        public MainForm()
        {
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
                MessageBoxEx.Show(this, "Операция успешно выполнена", name);    
            }
            else
            {
                MessageBoxEx.Show(this, "Ошибка выполнения операции", name);    
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
    }
}
