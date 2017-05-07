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

namespace craxx_plus.Controls
{
    /// <summary>
    /// roomItem.xaml の相互作用ロジック
    /// </summary>
    public partial class roomItem : UserControl
    {
        public roomItem()
        {
            InitializeComponent();
        }

        public byte[] roomCategory;
        public byte[] roomCode;

        public byte[] base_roomCategory;
        public byte[] base_roomCode;

        private string _beforeRoomName;
        private string _afterRoomName;
        public string beforeRoomName
        {
            get
            {
                return _beforeRoomName;
            }
            set
            {
                _beforeRoomName = value;
                label_BEFORE.Content = _beforeRoomName;
            }
        }

        public string afterRoomName
        {
            get
            {
                return _afterRoomName;
            }
            set
            {
                _afterRoomName = value;
                label_AFTER.Content = _afterRoomName;
            }
        }
    }
}
