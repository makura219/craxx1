using craxx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craxx_plus.IO
{
   public static class File
    {
        public static string DocumentPath = @"C:\Users\9ketsuki\Desktop\CraxxAction_1227.xaml";
        public static string saveDir = @"C:\Users\9ketsuki\Desktop\CraxxAction_1227.xaml";

        public static List<ACTION_ITEM> readSetting()
        {
            //保存元のファイル名
            string fileName = saveDir;

            //XmlSerializerオブジェクトを作成
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(List<ACTION_ITEM>));
            //読み込むファイルを開く
            System.IO.StreamReader sr = new System.IO.StreamReader(
                fileName, new System.Text.UTF8Encoding(false));
            //XMLファイルから読み込み、逆シリアル化する
            List<ACTION_ITEM> obj = (List<ACTION_ITEM>)serializer.Deserialize(sr);
            //ファイルを閉じる
            sr.Close();

            return obj;
        }
        public static void saveSetting(ACTION_ITEM[] settingsProperty)
        {


            //保存先のファイル名
            string fileName = saveDir;

            //保存するクラス(SampleClass)のインスタンスを作成
            ACTION_ITEM[] obj = settingsProperty;
            obj = settingsProperty;

            //XmlSerializerオブジェクトを作成
            //オブジェクトの型を指定する
            System.Xml.Serialization.XmlSerializer serializer =
                new System.Xml.Serialization.XmlSerializer(typeof(ACTION_ITEM[]));
            //書き込むファイルを開く（UTF-8 BOM無し）
            System.IO.StreamWriter sw = new System.IO.StreamWriter(
                fileName, false, new System.Text.UTF8Encoding(false));
            //シリアル化し、XMLファイルに保存する
            serializer.Serialize(sw, obj);
            //ファイルを閉じる
            sw.Close();
        }
    }
    public class Person
    {
        public string ActionName;
        public string ActionCode;
    }
}
