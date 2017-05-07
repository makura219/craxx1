using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace actionDataCodeGenerator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] a = getTextLineArray(textBox1.Text);
            foreach(string txt in a)
            {
                if(txt=="")
                {

                }
                else
                textBox2.Text += $"ret.Add(new actionItem {{ name = \"{txt.Split(',')[0]}\",actioncode = \"{txt.Split(',')[1]}\" }});\r\n";
            }
        }

        string[] getTextLineArray(string text)
        {
            System.IO.StringReader rs = new System.IO.StringReader(text);
            string a = "";
            while (rs.Peek() > -1)
            {
                a+=rs.ReadLine()+":";
            }

            rs.Close();
            return a.Split(':');
        }
    }
}
