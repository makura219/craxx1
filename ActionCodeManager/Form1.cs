using craxx;
using craxx_plus.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActionCodeManager
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var list = actionData.getACTION_ITEMCollection();
            //list = ACTION_ITEMSort(list);

            foreach (var data in list)
            {
                textBox1.Text += $"{data.name} : {data.actioncode}\r\n";
            }
        }

        void overlapCheck(List<ACTION_ITEM> list)
        {
            var checkedList = new List<ACTION_ITEM>();

            list = ACTION_ITEMSort(list);

            for (int i = 0; i < list.Count - 1; i++)
            {
                if (list[i].actioncode == list[i + 1].actioncode)
                {
                    textBox2.Text += $"+ {list[i].name} : {list[i].actioncode}\r\n";


                }
                else
                {
                    textBox2.Text += $"{list[i].name} : {list[i].actioncode}\r\n";
                    ACTION_ITEM item = new ACTION_ITEM() { name = list[i].name, actioncode = list[i].actioncode };

                    checkedList.Add(item);
                }
            }
            _checkedList = checkedList;
        }
        private List<ACTION_ITEM> _checkedList;
        List<ACTION_ITEM> ACTION_ITEMSort(List<ACTION_ITEM> targetList)
        {
            var list = targetList;
            list.Sort(delegate (ACTION_ITEM mca1, ACTION_ITEM mca2) { return string.Compare((string)mca1.actioncode, (string)mca2.actioncode); });
            return list;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var list = actionData.getACTION_ITEMCollection();
            //overlapCheck(list);
            File.saveSetting(list);
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
           /* var List = new List<ACTION_ITEM>();
            var data = File.readSetting();
            foreach (var item in data)
            {
                var i = new ACTION_ITEM() { name = item.name, actioncode = item.actioncode };
                List.Add(i);
            }

            var a = actionData.getACTION_ITEMCollection();
            a.AddRange(List);
            a = ACTION_ITEMSort(a);

            foreach (var h in a)
            {
                textBox1.Text += $"{h.name} : {h.actioncode}\r\n";
            }

            overlapCheck(a);*/
        }

        private void button4_Click(object sender, EventArgs e)
        {
            /*textBox2.Text = "";
            foreach(var data in _checkedList)
            {
                textBox2.Text += $"ret.Add(new ACTION_ITEM {{ name = \"{data.name}\",actioncode = \"{data.actioncode}\" }});\r\n";
            }*/
        }

        private void button5_Click(object sender, EventArgs e)
        {
            textBox2.Text= craxx_plus.Util.Decypter.Crypt(textBox1.Text);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox2.Text = craxx_plus.Util.Decypter.Decrypt(textBox1.Text);
        }
    }
}