using craxx;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace craxx_plus.Action_
{
    public class piggAction
    {
        public static List<IntPtr> _adressList;

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess,
              bool bInheritHandle, int dwProcessId);

        byte[] StringToByte(string text)
        {
            return System.Text.Encoding.GetEncoding("shift_jis").GetBytes(text);
        }
        int byteCount(byte[] target)
        {

            int count = 0;
            foreach (byte data in target)
            {
                count += 1;
            }
            return count;
        }
        byte[] convertActionCode(string code, int baseLenth)
        {
            byte[] c1 = StringToByte(code);
            if (c1.Length + 8 < baseLenth)
            {
                byte[] zero = { 0x00 };

                int po = baseLenth - (c1.Length + 8);
                for (int c = po; c != 0; c--)
                {
                    byte[] n1 = { 0x00 };
                    zero = Enumerable.Concat(zero, n1).ToArray();
                }

                byte[] _secret = { 0x5F, 0x73, 0x65, 0x63, 0x72, 0x65, 0x74 };
                byte[] tail = Enumerable.Concat(zero, _secret).ToArray();
                return Enumerable.Concat(c1, tail).ToArray();
            }
            return null;
        }
        public void WriteAction(Process process, string basecode, string actioncode, ListBox addressList, bool newsearch)
        {
            if (newsearch)
            {
                _adressList = new List<IntPtr>();
                Scan scan = new Scan(Convert.ToUInt32(process.Id));
                _adressList = scan.AobScan(StringToByte(basecode));
                MainWindow._newsearch = false;
            }
            IntPtr processHandle = OpenProcess(0x1F0FF, false, process.Id);

            addressList.Dispatcher.BeginInvoke(new Action(() =>
            {
                addressList.Items.Clear();
                foreach (var ad in _adressList)
                {
                    var lbl = new Label().Content = "0x" + ad.ToString("X");
                    addressList.Items.Add(lbl);
                }
            }));

            try
            {
                MemoryUtil mem = new MemoryUtil(process);
                byte[] buffer = convertActionCode(actioncode, basecode.Length);
                for (int c = 0; _adressList.Count() > c; c++)
                {
                    IntPtr ipr = (IntPtr)268435456;
                    int int1 = 0;
                    int int2 = 0;
                    try
                    {
                        int1 = _adressList[c].ToInt32();
                        int2 = ipr.ToInt32();
                    }
                    catch { }

                    mem.WriteMemory(_adressList[c], buffer, byteCount(buffer));
                }
            }
            catch
            {
                throw new Exception("WriteError!!");
            }
        }
    }
}