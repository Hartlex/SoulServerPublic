using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Console.SetOut(new TextBoxOutputter(ConsoleOutput));
            stopServerButton.Enabled = false;
            restartServerButton.Enabled = false;
        }

        private void StopServerButton_Click(object sender, EventArgs e)
        {
            Server.Stop();
            restartServerButton.Enabled = false;
            startServerButton.Enabled = true;
        }

        private void StartServerButton_Click(object sender, EventArgs e)
        {
            Server.Start();
            startServerButton.Enabled = false;
            restartServerButton.Enabled = true;
            stopServerButton.Enabled = true;
        }

        private void ConsoleOutput_TextChanged(object sender, EventArgs e)
        {
            ConsoleOutput.SelectionStart = ConsoleOutput.Text.Length;
            ConsoleOutput.ScrollToCaret();
        }

        private void restartServerButton_Click(object sender, EventArgs e)
        {
            Server.Restart();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void mainServerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainServerBox.Visible=!MainServerBox.Visible;
        }

        public void AddServerBox(string name,List<SunChannel> channels)
        {
            var menuItem = new ToolStripMenuItem(name);
            menuItem.Click += (sender, e) => { };
            menuStrip1.Items.Add(menuItem);
        }
    }
    public class TextBoxOutputter : TextWriter
    {
        RichTextBox textBox = null;

        public TextBoxOutputter(RichTextBox output)
        {
            textBox = output;
        }

        //public override void Write(char value)
        //{
        //    base.Write(value);
        //    textBox.BeginInvoke(new Action(() =>
        //    {
        //        textBox.AppendText(value.ToString());
        //    }));
        //}
        public override void WriteLine(string value)
        {
            base.Write(value);
            textBox.BeginInvoke(new Action(() =>
            {
                textBox.AppendText(value.ToString()+"\n");
            }));
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
