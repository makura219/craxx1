using craxx_plus.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace craxx_plus.Room
{
    public static class roomData
    {
        public static List<CheckBox> getRoomItemCollection()
        {
            List<CheckBox> ret = new List<CheckBox>();

            byte[] casino = new byte[] { 0x63, 0x61, 0x73, 0x69, 0x6E, 0x6F };


            //Casino-BJ----------------------------------------
            byte[] normal = new byte[] { 0x62, 0x6C, 0x61, 0x63, 0x6B, 0x6A, 0x61, 0x63, 0x6B, 0x5F, 0x6E, 0x6F, 0x72, 0x6D, 0x61, 0x6C };//blackjack_normal
            byte[] vip = new byte[] { 0x62, 0x6C, 0x61, 0x63, 0x6B, 0x6A, 0x61, 0x63, 0x6B, 0x5F, 0x76, 0x69, 0x70, 0x00, 0x00, 0x00 };//blackjack_vip

            return ret;
        }
    }
}
