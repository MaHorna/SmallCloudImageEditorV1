using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace SmallCloudImageEditorV1
{
    internal class MySQLHandle
    {
        public static int lastseenID = 1;
        public static bool firsttimeGetShowedID = true;

        public static void SQLUploadInfo(int scale, string authorname, string imgname)
        {
            MySqlConnection connection = new MySqlConnection("SERVER=remotemysql.com; PORT = 3306;DATABASE=K9gCHIfNyT;UID=K9gCHIfNyT;PASSWORD=aZKnkqIjJ8;");
            connection.Open();
            new MySqlCommand("INSERT INTO `ImageLibrary` (`ID`, `Scale`, `Author`, `ImageName`, `DownloadedTimes`) VALUES(NULL, '" + scale.ToString() + "', '" + authorname + "', '" + imgname + "', '0');", connection).ExecuteReader();
            connection.Close();
        }

        public static void FTPUploadImage(Bitmap img)
        {
            string str = Application.StartupPath + "\\tmpimg.bmp";
            img.Save(str);
            new WebClient()
            {
                Credentials = ((ICredentials)new NetworkCredential("momoos", "kakavko1"))
            }.UploadFile("ftp://momoos:kakavko1@ftp.drivehq.com/" + MySQLHandle.getimgID().ToString() + ".bmp", str);
            System.IO.File.Delete(str);
        }

        public static int getimgID()
        {
            int int32;
            using (MySqlConnection connection = new MySqlConnection("SERVER=remotemysql.com; PORT = 3306;DATABASE=K9gCHIfNyT;UID=K9gCHIfNyT;PASSWORD=aZKnkqIjJ8;"))
            {
                MySqlCommand mySqlCommand = new MySqlCommand("SELECT MAX(ID) FROM ImageLibrary", connection);
                connection.Open();
                int32 = Convert.ToInt32(mySqlCommand.ExecuteScalar());
                connection.Close();
            }
            return Convert.ToInt32(int32) + 1;
        }

        public static int getshowedID()
        {
            --MySQLHandle.lastseenID;
            if (MySQLHandle.firsttimeGetShowedID)
            {
                MySQLHandle.lastseenID = MySQLHandle.getimgID() - 1;
                MySQLHandle.firsttimeGetShowedID = false;
            }
            return MySQLHandle.lastseenID;
        }

        public static Image getLastUnseenImage()
        {
            string str = "C:\\SmallCloudImageEditor\\image" + MySQLHandle.lastseenID.ToString() + ".bmp";
            string fileName = "C:\\SmallCloudImageEditor\\image" + MySQLHandle.lastseenID.ToString() + "occupied.bmp";
            WebClient webClient = new WebClient();
            webClient.Credentials = (ICredentials)new NetworkCredential("momoos", "kakavko1");
            try
            {
                webClient.DownloadFile("ftp://momoos:kakavko1@ftp.drivehq.com/" + MySQLHandle.lastseenID.ToString() + ".bmp", str);
            }
            catch (Exception ex)
            {
                webClient.DownloadFile("ftp://momoos:kakavko1@ftp.drivehq.com/" + MySQLHandle.lastseenID.ToString() + ".bmp", fileName);
            }
            return Image.FromFile(str);
        }

        public static string getInfoString()
        {
            MySqlConnection connection = new MySqlConnection("SERVER=remotemysql.com; PORT = 3306;DATABASE=K9gCHIfNyT;UID=K9gCHIfNyT;PASSWORD=aZKnkqIjJ8;");
            connection.Open();
            MySqlDataReader mySqlDataReader = new MySqlCommand("SELECT * FROM ImageLibrary WHERE ID='" + MySQLHandle.lastseenID.ToString() + "'", connection).ExecuteReader();
            mySqlDataReader.Read();
            string str = "" + (string)mySqlDataReader["ImageName"] + " by " + (string)mySqlDataReader["Author"] + " , downloaded " + mySqlDataReader["DownloadedTimes"].ToString() + " times ";
            connection.Close();
            return str;
        }

        public static Image getImagewithID(int imageid)
        {
            string str = Application.StartupPath + "\\image" + MySQLHandle.lastseenID.ToString() + ".bmp";
            new WebClient()
            {
                Credentials = ((ICredentials)new NetworkCredential("momoos", "kakavko1"))
            }.DownloadFile("ftp://momoos:kakavko1@ftp.drivehq.com/" + MySQLHandle.lastseenID.ToString() + ".bmp", str);
            Image image = Image.FromFile(str);
            System.IO.File.Delete(str);
            return image;
        }

        public static void AddDownloadto(string id)
        {
            MySqlConnection connection = new MySqlConnection("SERVER=remotemysql.com; PORT = 3306;DATABASE=K9gCHIfNyT;UID=K9gCHIfNyT;PASSWORD=aZKnkqIjJ8;");
            connection.Open();
            MySqlDataReader mySqlDataReader = new MySqlCommand("SELECT DownloadedTimes FROM ImageLibrary WHERE ID='" + id + "'", connection).ExecuteReader();
            mySqlDataReader.Read();
            int num = (int)mySqlDataReader["DownloadedTimes"] + 1;
            mySqlDataReader.Close();
            new MySqlCommand("UPDATE ImageLibrary SET DownloadedTimes='" + num.ToString() + "' WHERE ID='" + id + "'", connection).ExecuteNonQuery();
            connection.Close();
        }
    }
}
