using craxx;
using craxx_plus.Action_;
using craxx_plus.Controls;
using craxx_plus.Controls.rainbow;
using craxx_plus.Room;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace craxx_plus
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Process _process;
        public static string _basecode;
        public static piggAction _piggAction;
        public static piggRoom _piggRoomCategory;
        public static piggRoom _piggRoomCode;
        public static bool _newsearch = true;
        public static bool _roomNewsearch = true;

        private long useLimit = 1493556301;//2017-04-30
        public MainWindow()
        {
            InitializeComponent();

            DateTime targetTime = DateTime.Now;
            var nowTimeUnixTimeStump = TimeUtil.GetUnixTime(targetTime);
            if (useLimit < nowTimeUnixTimeStump)
            {
                MessageBox.Show("試用期間が過ぎています。\r\n新しいバージョンをダウンロードしてください");
                Close();
            }


            grid_titleBar.MouseLeftButtonDown += (sender, e) => DragMove();
            rainbowBorder();
            loadConfig();
            adaptCursor();
            _piggAction = new piggAction();
            _piggRoomCategory = new piggRoom();
            _piggRoomCode = new piggRoom();
        }

        int rainbowIndex = 1;
        void rainbowBorder()
        {
            var brush = new SolidColorBrush(rainbowColors.getRainbow(rainbowIndex));
            var animation = new BrushAnimation
            {
                From = brush,
                To = new LinearGradientBrush(rainbowColors.getRainbow(rainbowIndex), rainbowColors.getRainbow(rainbowIndex + 1), 45),
                Duration = new Duration(TimeSpan.FromSeconds(5)),
            };

            animation.Completed += Animation_Completed;
            Storyboard.SetTarget(animation, border);
            Storyboard.SetTargetProperty(animation, new PropertyPath("BorderBrush"));//BorderBrush

            var sb = new Storyboard();
            sb.Children.Add(animation);
            sb.Begin();

            if (rainbowIndex != 6)
                rainbowIndex += 1;
            else if (rainbowIndex == 6)
                rainbowIndex = 1;
        }

        void loadConfig()
        {
            try
            {
                var conf = IO.File.readSetting();
                textBox_basecode.Text = conf.BASECODE;
            }
            catch { }
        }
        void adaptCursor()
        {
            Cursor cur = ((TextBlock)this.Resources["pointer"]).Cursor;
            Cursor = cur;

        }

        Visibility Show = Visibility.Visible;
        Visibility Hide = Visibility.Hidden;
        void gridChange(Grid showGrid)
        {
            grid_Action.Visibility = Hide;
            grid_attachProcess.Visibility = Hide;
            grid_Room.Visibility = Hide;
            grid_settings.Visibility = Hide;
            grid_home.Visibility = Hide;

            showGrid.Visibility = Show;

        }
        private void Animation_Completed(object sender, EventArgs e)
        {
            rainbowBorder();
        }

        private void menu_selectProcess_Click(object sender, RoutedEventArgs e)
        {
            LoadProcessesList();
            gridChange(grid_attachProcess);
        }
        void LoadProcessesList()
        {
            listBox_processes.Items.Clear();
            var processes = getProcesses();
            List<processListItem> list = new List<processListItem>();
            foreach (System.Diagnostics.Process p in processes)
            {
                try
                {
                    processListItem item = new processListItem();
                    item.lbl_pName.Content = p.ProcessName;
                    item.lbl_pID.Content = p.Id;
                    item.lbl_useingPersent.Content = p.WorkingSet64;
                    list.Add(item);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "エラー");
                }
            }
            list.Sort(delegate (processListItem mca1, processListItem mca2) { return string.Compare((string)mca1.lbl_pName.Content, (string)mca2.lbl_pName.Content); });
            foreach (var item in list)
            {
                listBox_processes.Items.Add(item);
            }
        }
        Process[] getProcesses()
        {
            return Process.GetProcesses();
        }
        void attachProcess()
        {
            processListItem item = (processListItem)listBox_processes.SelectedItem;
            try
            {
                _process = System.Diagnostics.Process.GetProcessById(Convert.ToInt32(item.lbl_pID.Content));
                label_isAttached.Content = "設定しました";
                label_attachedProcessName.Content = "アタッチ中のプロセス - " + _process.ProcessName;
            }
            catch
            {
                label_isAttached.Content = "設定に失敗しました";
            }
        }
        private void button_processAttach_Click(object sender, RoutedEventArgs e)
        {
            attachProcess();
        }

        List<actionItem> Filter(List<actionItem> items)
        {
            List<actionItem> ret = new List<actionItem>();
            foreach (actionItem item in items)
            {
                if (item.actioncode.Length > textBox_basecode.Text.Length - 9)
                {
                    Color color = new Color()
                    {
                        A = 255,
                        R = 222,
                        G = 0,
                        B = 0
                    };
                    item.lbl_name.Foreground = new SolidColorBrush(color);
                }

                ret.Add(item);
            }
            return ret;
        }

        private void grid_Action_Loaded(object sender, RoutedEventArgs e)
        {
            LoadActionCodeList();
        }
        List<actionItem> ActionItemSort(List<actionItem> targetList)
        {
            var list = targetList;
            list.Sort(delegate (actionItem mca1, actionItem mca2) { return string.Compare((string)mca1.actioncode, (string)mca2.actioncode); });
            return list;
        }
        void LoadActionCodeList()
        {
            var List = actionData.getActionItemCollection();
            List = Filter(List);
            List = ActionItemSort(List);
            string text = "";
            foreach (var data in List)
            {
                listBox_actionList.Items.Add(data);
            }
            label_actionColunt.Content = "action count : " + List.Count;
        }

        void searchActionCode()
        {
            string beforeText = textBox_searchBox.Text;
            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(1000);
                listBox_actionList.Dispatcher.BeginInvoke(new Action(() =>
                {
                    if (textBox_searchBox.Text == beforeText)
                    {
                        listBox_actionList.Items.Clear();
                        var collection = actionData.getFilterActionItemCollection(textBox_searchBox.Text);
                        collection = Filter(collection);
                        collection = ActionItemSort(collection);
                        foreach (var item in collection)
                        {
                            listBox_actionList.Items.Add(item);
                        }
                    }
                }));
            });
        }

        private void textBox_searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchActionCode();
        }
        void adaptBaseCode(string basecode)
        {
            _basecode = basecode;
            label_baseCodeLength.Content = "base code Length : " + _basecode.Length;
            label_possibleCodeLength.Content = "possible code Length : " + (_basecode.Length - 9);
        }
        private void textBox_basecode_TextChanged(object sender, TextChangedEventArgs e)
        {
            string a = textBox_basecode.Text.Substring(0,4);
            if (textBox_basecode.Text.Substring(0,4) == "cry:")
            {
                string crytext = Util.Decypter.Decrypt(textBox_basecode.Text.Remove(0, 4));
                adaptBaseCode(crytext);
            }
            else
            {
                adaptBaseCode(textBox_basecode.Text);
            }

        }
        void selectActionList(ListBox actionlist)
        {
            actionItem item = (actionItem)listBox_actionList.SelectedItem;
            adaptBaseCode(item.actioncode);
            textBox_basecode.Text = "cry:"+Util.Decypter.Crypt(_basecode);

            checkBox_selectActionList.IsChecked = false;
        }

        void writeAction(string code)
        {
            Task.Run(() =>
            {
                if (_basecode != "" | _basecode != null)
                {
                    if (code != null)
                    {
                        try
                        {
                            label_selectedCodeLength.Dispatcher.BeginInvoke(new Action(() =>
                            {
                                label_selectedCodeLength.Content = "selected code Length : " + code.Length;
                            }));

                            _piggAction.WriteAction(_process, _basecode, code, listBox_action_debug_addressList, _newsearch);
                        }
                        catch
                        {
                            Invoke_ActionListMessageBox("検索・書き込みに失敗しました。\r\n正しいプロセス及び使用可能なコード、newSearch等を再度ご確認下さい。", "Write Error");
                        }
                        checkBox_newSearch.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            checkBox_newSearch.IsChecked = _newsearch;
                        }));

                    }

                }
                else
                {
                    Invoke_ActionListMessageBox("ベースコードを設定してください");
                }
            });
        }
        void Invoke_ActionListMessageBox(string message, string title = "")
        {
            listBox_actionList.Dispatcher.BeginInvoke(new Action(() =>
            {
                MessageBox.Show(message, title);
            }));
        }
        private void listBox_actionList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((bool)checkBox_selectActionList.IsChecked)
            {
                selectActionList(listBox_actionList);
            }
            else
            {
                actionItem item = (actionItem)listBox_actionList.SelectedItem;
                try
                {
                    writeAction(item.actioncode);
                }
                catch { }
            }
        }

        private void menu_action_Click(object sender, RoutedEventArgs e)
        {
            gridChange(grid_Action);
        }

        private void checkBox_enableDebug_Click(object sender, RoutedEventArgs e)
        {
            debugmenu();
        }
        void debugmenu()
        {
            if ((bool)checkBox_enableDebug.IsChecked)
            {
                groupBox_debugMenu.Visibility = Visibility.Visible;
            }
            else
            {
                groupBox_debugMenu.Visibility = Visibility.Hidden;
            }
        }

        private void button_debug_write_Click(object sender, RoutedEventArgs e)
        {
            writeAction(textBox_debug_writeActionCode.Text);
        }

        private void checkBox_newSearch_Click(object sender, RoutedEventArgs e)
        {
            _newsearch = (bool)checkBox_newSearch.IsChecked;
        }

        private void menu_room_Click(object sender, RoutedEventArgs e)
        {
            gridChange(grid_Room);
        }
        void LoadRoomItemsList()
        {
            var items = roomData.getRoomItemCollection();
            foreach (CheckBox item in items)
            {
                listBox_Room.Items.Add(item);
            }
        }
        private void grid_Room_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRoomItemsList();
        }
        void writeRoom(byte[] before, byte[] after, string message)
        {
            Task.Run(() =>
            {
                try
                {
                    _piggRoomCode.WriteRoom(_process, before, after, listBox_roomAddressList);
                    label_room_status.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        label_room_status.Content = message;
                    }));
                }
                catch
                {
                    label_room_status.Dispatcher.BeginInvoke(new Action(() =>
                                    {
                                        label_room_status.Content = "失敗";
                                    }));
                }

            });

        }
        void roomDebug()
        {
            if ((bool)checkBox_room_enableDebug.IsChecked)
            {
                listBox_roomAddressList.Visibility = Visibility.Visible;
            }
            else
            {
                listBox_roomAddressList.Visibility = Visibility.Hidden;
            }
        }
        private void checkBox_room_enableDebug_Click(object sender, RoutedEventArgs e)
        {
            roomDebug();
        }
        private void BJ_normalToVIP_Click(object sender, RoutedEventArgs e)
        {
            byte[] normal = new byte[] { 0x62, 0x6C, 0x61, 0x63, 0x6B, 0x6A, 0x61, 0x63, 0x6B, 0x5F, 0x6E, 0x6F, 0x72, 0x6D, 0x61, 0x6C };//blackjack_normal
            byte[] vip = new byte[] { 0x62, 0x6C, 0x61, 0x63, 0x6B, 0x6A, 0x61, 0x63, 0x6B, 0x5F, 0x76, 0x69, 0x70, 0x00, 0x00, 0x00 };//blackjack_vip

            CheckBox checkbox = (CheckBox)sender;
            if ((bool)checkbox.IsChecked)
            {
                _piggRoomCode.isWrite = true;
                writeRoom(normal, vip, "ブラックジャックVIPに変更しました");
            }
            else
            {
                writeRoom(vip, normal, "ブラックジャックノーマルに変更しました");
                _piggRoomCode.isWrite = false;
            }
        }

        private void roulette_normalToVIP_Click(object sender, RoutedEventArgs e)
        {
            byte[] normal = new byte[] { 0x72, 0x6F, 0x75, 0x6C, 0x65, 0x74, 0x74, 0x65, 0x5F, 0x6E, 0x6F, 0x72, 0x6D, 0x61, 0x6C };//roulette_normal
            byte[] vip = new byte[] { 0x72, 0x6F, 0x75, 0x6C, 0x65, 0x74, 0x74, 0x65, 0x5F, 0x76, 0x69, 0x70, 0x00, 0x00, 0x00 };//roulette_vip

            CheckBox checkbox = (CheckBox)sender;
            if ((bool)checkbox.IsChecked)
            {
                _piggRoomCode.isWrite = true;
                writeRoom(normal, vip, "ルーレットVIPに変更しました");
            }
            else
            {
                writeRoom(vip, normal, "ルーレットノーマルに変更しました");
                _piggRoomCode.isWrite = false;
            }
        }

        private void minibaccarat_normalToVIP_Click(object sender, RoutedEventArgs e)
        {
            byte[] normal = new byte[] { 0x6D, 0x69, 0x6E, 0x69, 0x62, 0x61, 0x63, 0x63, 0x61, 0x72, 0x61, 0x74, 0x5F, 0x6E, 0x6F, 0x72, 0x6D, 0x61, 0x6C };//minibaccarat_normal
            byte[] vip = new byte[] { 0x6D, 0x69, 0x6E, 0x69, 0x62, 0x61, 0x63, 0x63, 0x61, 0x72, 0x61, 0x74, 0x5F, 0x76, 0x69, 0x70, 0x00, 0x00, 0x00 };//minibaccarat_vip

            CheckBox checkbox = (CheckBox)sender;
            if ((bool)checkbox.IsChecked)
            {
                _piggRoomCode.isWrite = true;
                writeRoom(normal, vip, "ミニバカラVIPに変更しました");
            }
            else
            {
                writeRoom(vip, normal, "ミニバカラノーマルに変更しました");
                _piggRoomCode.isWrite = false;
            }
        }

        private void menu_settings_Click(object sender, RoutedEventArgs e)
        {
            gridChange(grid_settings);
        }
        void fileDelete(string path)
        {
            System.IO.Directory.Delete(path, true);
        }
        void deleteLicense()
        {
            if (MessageBox.Show("ライセンスを破棄するとCraxxに関する(本体以外の)データが全て削除されます。\r\nよろしいですか？",
               "ライセンスの破棄",
               MessageBoxButton.YesNo,
               MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                fileDelete(IO.File.saveDir);
                MessageBox.Show("ライセンスを削除しました\r\nアプリケーションを終了します。");
                Close();
            }
        }
        private void button_deleteLicense_Click(object sender, RoutedEventArgs e)
        {
            deleteLicense();
        }
        void saveConfig()
        {
            IO.Config conf = new IO.Config();
            if (textBox_basecode.Text.Substring(0, 4) == "cry:")
                conf.BASECODE = textBox_basecode.Text;
            else
                conf.BASECODE = _basecode;
            IO.File.saveSetting(conf);
        }
        private void menu_exit_Click(object sender, RoutedEventArgs e)
        {
            saveConfig();
            Close();
        }
        private void menu_tools_MouseEnter(object sender, MouseEventArgs e)
        {
            if (_process != null)
            {
                menu_action.IsEnabled = true;
                menu_room.IsEnabled = true;
            }
        }
        public static bool _isAdmin = false;
        private void textBox_admin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox_admin.Text == "wowow")
                _isAdmin = true;
        }

        private void label2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gridChange(grid_home);
        }
    }
}