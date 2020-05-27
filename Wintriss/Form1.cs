using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Reflection;
using System.Threading;

namespace Wintriss
{
    public partial class Form1 : Form
    {

        #region " DB參數 "

        // DB connect string 
        public string con = "Data Source=10.10.37.91;Initial Catalog=WebDB2;User ID=sa;Password=wecoweb";

        // 1st command without iImage
        //public string strcmd_1 = "select j.klKey,j.JobID, fc.lID,fc.sName,f.pklFlawKey from[WebDB2].[dbo].[Image] img,[WebDB2].[dbo].[Flaw] f ,[WebDB2].[dbo].[Jobs] j,[WebDB2].[dbo].[FlawClass] fc where img.klFlawKey = f.pklFlawKey and f.klJobKey=j.klKey and fc.fkJobKey = j.klKey and fc.lID=f.lFlawClassType and j.JobID='R40410-15'";

        // 2nd command with iImage
        //public string strcmd_2 = "select j.klKey,j.JobID, fc.lID,fc.sName,f.pklFlawKey,img.iImage from[WebDB2].[dbo].[Image] img,[WebDB2].[dbo].[Flaw] f ,[WebDB2].[dbo].[Jobs] j,[WebDB2].[dbo].[FlawClass] fc where img.klFlawKey = f.pklFlawKey and f.klJobKey=j.klKey and fc.fkJobKey = j.klKey and fc.lID=f.lFlawClassType and j.JobID='R40410-15'";

        //正式上線要用的字串 1st
        public string strcmd_3 = "select j.klKey,j.JobID, fc.lID,fc.sName,f.pklFlawKey from[WebDB2].[dbo].[Image] img,[WebDB2].[dbo].[Flaw] f ,[WebDB2].[dbo].[Jobs] j,[WebDB2].[dbo].[FlawClass] fc where img.klFlawKey = f.pklFlawKey and f.klJobKey=j.klKey and fc.fkJobKey = j.klKey and fc.lID=f.lFlawClassType and j.JobID=";

        public string strcmd_4 = "select j.klKey,j.JobID, fc.lID,fc.sName,f.pklFlawKey,img.iImage from[WebDB2].[dbo].[Image] img,[WebDB2].[dbo].[Flaw] f ,[WebDB2].[dbo].[Jobs] j,[WebDB2].[dbo].[FlawClass] fc where img.klFlawKey = f.pklFlawKey and f.klJobKey=j.klKey and fc.fkJobKey = j.klKey and fc.lID=f.lFlawClassType and j.JobID=";

        public string strcmd_5 = "  SELECT  [JobID] FROM[WebDB2].[dbo].[Jobs]";

        #endregion

        #region "物件宣告"
        SqlCommand sqlcmd = new SqlCommand();
        Int32 iName;
        String strID;
        #endregion

        #region "DB欄位物件轉字串用到的參數"
        object obj;
        object obj_int;
        object obj_name;
        object obj_ID;
        //string s;
        #endregion

        #region "資料夾參數"
        string defectType = "";
        string strFolderName = ""; //資料夾名稱
        string strPath = @"C:\test_dest\"; //資料夾位子
        string strFolderPath ;
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //加入textbox 以後正式上線要用
            strcmd_3 = strcmd_3 + "'" + textBox1.Text.ToUpper().Trim() + "'";
            strcmd_4 = strcmd_4 + "'" + textBox1.Text.ToUpper().Trim() + "'";
            //Thread t1 = new Thread(sql);
            //t1.Start();
            sql();
            MessageBox.Show("分類完成");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //public string con = "Data Source=10.10.37.91;Initial Catalog=WebDB2;User ID=sa;Password=wecoweb";
            string strDBIP = txtDBIP.Text;
            con = "Data Source=" + strDBIP + ";Initial Catalog=WebDB2;User ID=sa;Password=wecoweb";
            selectJobID();
        }

        #region " Method " 

        private void selectJobID()
        {
            using(SqlConnection cn = new SqlConnection(con))
            {
                using(SqlCommand cmd = new SqlCommand(strcmd_5,cn))
                {
                    SqlDataAdapter sd = new SqlDataAdapter(strcmd_5, con);
                    DataSet ds = new DataSet();
                    sd.Fill(ds, "JobID");
                    BindingSource bs = new BindingSource(ds, "JobID");
                    dataGridView2.DataSource = bs;
                        
                }
            }
        }

        private void sql()
        {
            // 1. DB connect
            using (SqlConnection cn = new SqlConnection(con))
            {
                // 2. open DB
                cn.Open();

                // 3. SQL 1st command 
                using (SqlCommand cmd_1 = new SqlCommand(strcmd_3, cn))
                {
                    SqlDataAdapter sda = new SqlDataAdapter(strcmd_3, con);
                    DataSet DS = new DataSet();
                    sda.Fill(DS, "PM18");
                    BindingSource bs = new BindingSource(DS, "PM18");
                    dataGridView1.DataSource = bs;
                }

                // 4. SQL 1st command 
                using (SqlCommand cmd_2 = new SqlCommand(strcmd_4, cn))
                {
                    // 5. 搭配Sqlcommand使用SqlDataReader
                    using (SqlDataReader dr = cmd_2.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            // 6. 判斷資料列使否為空
                            if (!dr[5].Equals(DBNull.Value))
                            {
                                // 7. 分類
                                obj_ID = dr[1];
                                
                                obj_int = dr[2]; 
                                obj_name = dr[4];
                                obj = dr[5];  //圖片
                                Int32 iField = (Int32)obj_int;
                                iName = (Int32)obj_name;
                                strID = (String)obj_ID;
                                // create folde and filter image
                                switch (iField)
                                {
                                    case 1:
                                    case 2:
                                    case 3:
                                    case 4:
                                    case 5:
                                    case 6:
                                    case 7:
                                    case 8:
                                    case 9:
                                    case 10:
                                    case 11:
                                    case 12:
                                    case 13:
                                    case 14:
                                        //建立資料夾
                                        CreateFolder(iField);
                                        //轉圖
                                        byte[] strfalwImage = (byte[])obj;
                                        baseToBitmap(strfalwImage);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 建立資料夾
        /// </summary>
        /// <param name="iField"></param>
        private void CreateFolder(int iField)
        {

            int i = iField;

            if (i == 0)
                defectType = GetDescriptionText(eDefectList.Not_Classified.ToString());
            if (i == 1)
                defectType = GetDescriptionText(eDefectList.Little_Broken_hole.ToString());
            if (i == 2)
                defectType = GetDescriptionText(eDefectList.Mid_Broken_hole.ToString());
            if (i == 3)
                defectType = GetDescriptionText(eDefectList.Big_Broken_hole.ToString());
            if (i == 4)
                defectType = GetDescriptionText(eDefectList.Little_stain.ToString());
            if (i == 5)
                defectType = GetDescriptionText(eDefectList.Mid_stain.ToString());
            if (i == 6)
                defectType = GetDescriptionText(eDefectList.Big_stain.ToString());
            if (i == 7)
                defectType = GetDescriptionText(eDefectList.Little_Black_stain.ToString());
            if (i == 8)
                defectType = GetDescriptionText(eDefectList.Mid_Black_stain.ToString());
            if (i == 9)
                defectType = GetDescriptionText(eDefectList.Big_Black_stain.ToString());
            if (i == 10)
                defectType = GetDescriptionText(eDefectList.Little_Transparent.ToString());
            if (i == 11)
                defectType = GetDescriptionText(eDefectList.Mid_Transparent.ToString());
            if (i == 12)
                defectType = GetDescriptionText(eDefectList.Big_Transparent.ToString());
            if (i == 13)
                defectType = GetDescriptionText(eDefectList.Oil.ToString());
            if (i == 14)
                defectType = GetDescriptionText(eDefectList.Grain.ToString());

            strFolderName = defectType; //資料夾名稱
            strPath = @"C:\test_dest\"; //資料夾位子
            strFolderPath = strPath + strFolderName;
            if (Directory.Exists(strFolderPath))
            {
                return;
            }
            Directory.CreateDirectory(strFolderPath);
        }

        /// <summary>
        /// byte[] 轉換圖片並儲存圖片
        /// </summary>
        /// <param name="strImage"></param>
        /// <returns></returns>
        public Bitmap baseToBitmap(byte[] strImage)
        {
            byte[] a = strImage;
            Bitmap bmpShowImg;
            if (strImage != null)
            {
                int intW = a[0] + a[1] * 256;  //'width (byte flipped)
                int intH = a[4] + a[5] * 256;  //'height (byte flipped)
                if (intW != 0 && intH != 0)
                {
                    bmpShowImg = new Bitmap(intW, intH);
                    int intR;
                    int intPixel = 7;
                    int intYadj = 0;
                    int intHadj = 0;
                    for (int intY = 0; intY <= intH - 1; intY++)
                    {
                        for (int intX = 0; intX <= intW - 1; intX++)
                        {
                            intYadj = intY;
                            intHadj = intX;
                            intR = a[++intPixel];
                            bmpShowImg.SetPixel(intHadj, intYadj, Color.FromArgb(intR, intR, intR));
                        }
                    }
                    bmpShowImg.Save(strFolderPath + "\\"+strID+"_" + iName.ToString() + ".bmp");
                }
                else
                {
                    bmpShowImg = new Bitmap(Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "Img_No.jpg")));
                }
            }
            else
            {
                bmpShowImg = new Bitmap(Image.FromFile(Path.Combine(Directory.GetCurrentDirectory(), "Img_No.jpg")));
            }
            return bmpShowImg;
        }
    
        public enum eDefectList
        {
            [Description("Not_Classified")]
            Not_Classified = 0,

            [Description("小破孔")]
            Little_Broken_hole =1,

            [Description("中破孔")]
            Mid_Broken_hole = 2,

            [Description("大破孔")]
            Big_Broken_hole = 3,

            [Description("小淺汙點")]
            Little_stain = 4,

            [Description("中淺汙點")]
            Mid_stain = 5,

            [Description("大淺汙點")]
            Big_stain = 6,

            [Description("小黑汙點")]
            Little_Black_stain = 7,

            [Description("中黑汙點")]
            Mid_Black_stain = 8,

            [Description("大黑汙點")]
            Big_Black_stain = 9,

            //Transparent
            [Description("小透明點")]
            Little_Transparent = 10,

            [Description("中透明點")]
            Mid_Transparent = 11,

            [Description("大透明點")]
            Big_Transparent = 12,

            [Description("油點")]
            Oil = 13,

            [Description("死紋")]
            Grain = 14,
        }

        public static string GetDescriptionText(string value)
        {
            Type type = typeof(eDefectList);
            var name = Enum.GetNames(type).Where(f => f.Equals(value, StringComparison.CurrentCultureIgnoreCase)).Select(d => d).FirstOrDefault();

            // 找無相對應的列舉
            if (name == null)
            {
                return string.Empty;
            }

            // 利用反射找出相對應的欄位
            var field = type.GetField(name);
            // 取得欄位設定DescriptionAttribute的值
            var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            // 無設定Description Attribute, 回傳Enum欄位名稱
            if (customAttribute == null || customAttribute.Length == 0)
            {
                return name;
            }

            // 回傳Description Attribute的設定
            return ((DescriptionAttribute)customAttribute[0]).Description;
        }

        #endregion
    }
}
