using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craxx_plus.IO
{
    public static class File
    {
        public static string DocumentPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        public static string saveDir = DocumentPath + @"\Artemis\config";

        public static Config readSetting()
        {
            try
            {


                //保存元のファイル名
                string fileName = saveDir;

                //XmlSerializerオブジェクトを作成
                System.Xml.Serialization.XmlSerializer serializer =
                    new System.Xml.Serialization.XmlSerializer(typeof(Config));
                //読み込むファイルを開く
                System.IO.StreamReader sr = new System.IO.StreamReader(
                    fileName, new System.Text.UTF8Encoding(false));
                //XMLファイルから読み込み、逆シリアル化する
                Config obj = (Config)serializer.Deserialize(sr);
                //ファイルを閉じる
                sr.Close();
                return obj;
            }
            catch
            {
                return null;
            }
        }
        public static void saveSetting(Config settingsProperty)
        {
            try
            {
                System.IO.DirectoryInfo di =
    System.IO.Directory.CreateDirectory(DocumentPath + @"\Artemis");


                //保存先のファイル名
                string fileName = saveDir;

                //保存するクラス(SampleClass)のインスタンスを作成
                Config obj = new Config();
                obj = settingsProperty;

                //XmlSerializerオブジェクトを作成
                //オブジェクトの型を指定する
                System.Xml.Serialization.XmlSerializer serializer =
                    new System.Xml.Serialization.XmlSerializer(typeof(Config));
                //書き込むファイルを開く（UTF-8 BOM無し）
                System.IO.StreamWriter sw = new System.IO.StreamWriter(
                    fileName, false, new System.Text.UTF8Encoding(false));
                //シリアル化し、XMLファイルに保存する
                serializer.Serialize(sw, obj);
                //ファイルを閉じる
                sw.Close();
            }
            catch {
                throw new Exception("データのセーブに失敗");

            }
        }
    }
}