using craxx;
using craxx_plus.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace craxx_plus.Room
{
    public class piggRoom
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
        byte[] adaptDifference(byte[] basecode, byte[] code)
        {

            int b = byteCount(basecode);
            int c = byteCount(code);
            if (b - c >= 0)
            {
                byte[] zero = { 0x00 };
                int result = b - c;
                for (int i = result; i != 0; i--)
                {
                    byte[] n1 = { 0x00 };
                    zero = Enumerable.Concat(zero, n1).ToArray();
                }
                return zero;
            }
            return null;

        }

        public bool isWrite=true;
        public void WriteRoom_mode1m(Process process, byte[] beforeByte, byte[] afterByte, ListBox addressList, bool newsearch)
        {
            _adressList = new List<IntPtr>();
            Scan scan = new Scan(Convert.ToUInt32(process.Id));
            _adressList = scan.AobScan(beforeByte);

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
                byte[] buffer;
                try
                {
                    byte[] zero = adaptDifference(beforeByte, afterByte);
                    buffer = Enumerable.Concat(afterByte, zero).ToArray();
                }
                catch
                {
                    buffer = afterByte;
                }

                Task.Run(() =>
                {
                    while (isWrite)
                    {
                        for (int c = 0; _adressList.Count() > c; c++)
                        {
                            byte[] readmem = readMemory(process, _adressList[c], buffer.Length);

                            string a = BitConverter.ToString(buffer);
                            string b = BitConverter.ToString(readmem);
                            if (a != b)
                                mem.WriteMemory(_adressList[c], buffer, byteCount(buffer));
                            else
                                Console.WriteLine("");
                        }
                    }
                });


            }
            catch
            {
                throw new Exception("WriteError!!");
            }
        }
        public void WriteRoom(Process process, byte[] baseByte, byte[] writeByte, ListBox listbox)
        {
            _adressList = new List<IntPtr>();
            Scan scan = new Scan(Convert.ToUInt32(process.Id));
            _adressList = scan.AobScan(baseByte);


            IntPtr processHandle = OpenProcess(0x1F0FF, false, process.Id);
            listbox.Dispatcher.BeginInvoke(new Action(() =>
            {
                listbox.Items.Clear();
                foreach (var ad in _adressList)
                {
                    var lbl = new Label().Content = "0x" + ad.ToString("X");
                    listbox.Items.Add(lbl);
                }
            }));
            try
            {
                byte[] buffer = writeByte;
                MemoryUtil mem = new MemoryUtil(process);
                var ad1 = _adressList;
                Task.Run(() =>
                {
                    while (isWrite)
                    {
                        for (int c = 0; _adressList.Count() > c; c++)
                        {
                            byte[] readmem = readMemory(process,_adressList[c], buffer.Length);
                            string a = BitConverter.ToString(buffer);
                            string b = BitConverter.ToString(readmem);
                            if (a != b)
                                mem.WriteMemory(_adressList[c], buffer, byteCount(buffer));
                        }
                    }
                });
            }
            catch
            {
                throw new Exception("WriteError!!");
            }
        }

        public static byte[] readMemory(Process process ,IntPtr addres, int length)
        {
            MemoryUtil mem = new MemoryUtil(process);
            var bytes = new byte[length];

            mem.ReadMemory(addres, bytes, length);
            string s = System.Text.Encoding.ASCII.GetString(bytes);

            return bytes;
        }
        public string byteArrayToAscii(byte[] bytes)
        {
            return System.Text.Encoding.ASCII.GetString(bytes);
        }
    }
}