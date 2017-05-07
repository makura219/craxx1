using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace craxx
{
    /// <summary>
    /// actionItem.xaml の相互作用ロジック
    /// </summary>
    public partial class actionItem : UserControl
    {
        private string _code;
        public string actioncode
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
                if(value.Split('_')[0].Contains("feel"))
                {
                    Color color = new Color()
                    {
                        A = 255,
                        R = 245,
                        G = 131,
                        B = 197
                    };

                    lbl_type.Foreground = new SolidColorBrush(color);
                }
                else if(value.Split('_')[0].Contains("commu"))
                {
                    Color color = new Color()
                    {
                        A = 255,
                        R = 153,
                        G = 204,
                        B = 70
                    };

                    lbl_type.Foreground = new SolidColorBrush(color);
                }
                else if (value.Split('_')[0].Contains("sports"))
                {
                    Color color = new Color()
                    {
                        A = 255,
                        R = 102,
                        G = 204,
                        B = 204
                    };

                    lbl_type.Foreground = new SolidColorBrush(color);
                }
                else if (value.Split('_')[0].Contains("dance"))
                {
                    Color color = new Color()
                    {
                        A = 255,
                        R = 222,
                        G = 209,
                        B = 84
                    };

                    lbl_type.Foreground = new SolidColorBrush(color);
                }
                else
                {
                    Color color = new Color()
                    {
                        A = 255,
                        R = 187,
                        G = 99,
                        B = 202
                    };

                    lbl_type.Foreground = new SolidColorBrush(color);
                }
            }
        }
        private string _Name;
        public string name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
                lbl_name.Content = value;
            }
        }

        public actionItem()
        {
            InitializeComponent();
        }
    }
}
