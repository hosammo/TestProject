using ShopifyHelper.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shopify_Manager.UI
{
    public partial class test : Form
    {
        public test()
        {
            InitializeComponent();
        }

        private void test_Load(object sender, EventArgs e)
        {



        }

        private void button1_Click(object sender, EventArgs e)
        {
            //DataTable t = Fields.CachingDB.ExecuteDatatable(@"SELECT (SELECT TOP (1) sku FROM Variants WHERE (product_id = Images.product_id)) AS sku, Images.src, Products.tags FROM Images INNER JOIN Products ON Images.product_id = Products.id WHERE (Images.product_id IN (SELECT id FROM Products AS Products_1 WHERE (vendor = 'montania'))) AND (Products.tags LIKE N'%2016%')");
            DataTable t = Fields.CachingDB.ExecuteDatatable("SELECT ComputerNo FROM NeckDB0001..MTI WHERE ItemYear = 2016 AND ComputerNo NOT IN (SELECT DISTINCT SUBSTRING(OceanComputerNo,1,8) FROM dbo.Products )");
            WebClient w = new WebClient();
            int ModelImageCounter = 1;
            string psku = "";
            foreach (DataRow r in t.Rows)
            {
                if(psku == "" | psku != r["sku"].ToString())
                {
                    ModelImageCounter = 1;
                }
                else if(psku == r["sku"].ToString())
                {
                    ModelImageCounter += 1;
                }


                //w.DownloadFile(r["src"].ToString(), "E:\\OneDrive\\Ahmad Al-Mosawi\\Montania2016\\" + r["sku"].ToString() + "-" + ModelImageCounter.ToString() + ".jpg");
                psku = r["sku"].ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            DataTable t = Fields.CachingDB.ExecuteDatatable("SELECT DISTINCT ComputerNo + '.' + ColorID FROM NeckDB0001..QMTD WHERE ItemYear = 2016 AND  SeasonID = 'W' And BrandID = 1 --and Substring(ComputerNo,1,2) = 16 --ComputerNo NOT IN (SELECT DISTINCT SUBSTRING(OceanComputerNo,1,8) FROM dbo.Products )");

            string s = "";
            WebClient w = new WebClient();
            foreach (DataRow dr in t.Rows)
            {
                s = dr[0].ToString();
                if (!System.IO.File.Exists("E:\\Neck&Neck Photos\\Winter2016\\" + s + "-A1.jpg"))
                    w.DownloadFile("http://www.neckandneck.com/img/productos/" + s + "-A1.jpg", "E:\\Neck&Neck Photos\\Winter2016\\" + s + "-A1.jpg");
                if (!System.IO.File.Exists("E:\\Neck&Neck Photos\\Winter2016\\" + s + "-B1.jpg"))
                    w.DownloadFile("http://www.neckandneck.com/img/productos/" + s + "-B1.jpg", "E:\\Neck&Neck Photos\\Winter2016\\" + s + "-B1.jpg");
                if (!System.IO.File.Exists("E:\\Neck&Neck Photos\\Winter2016\\" + s + "-C1.jpg"))
                    w.DownloadFile("http://www.neckandneck.com/img/productos/" + s + "-C1.jpg", "E:\\Neck&Neck Photos\\Winter2016\\" + s + "-C1.jpg");
                if (!System.IO.File.Exists("E:\\Neck&Neck Photos\\Winter2016\\" + s + "-D1.jpg"))
                    w.DownloadFile("http://www.neckandneck.com/img/productos/" + s + "-D1.jpg", "E:\\Neck&Neck Photos\\Winter2016\\" + s + "-D1.jpg");
                if (!System.IO.File.Exists("E:\\Neck&Neck Photos\\Winter2016\\" + s + "-E1.jpg"))
                    w.DownloadFile("http://www.neckandneck.com/img/productos/" + s + "-E1.jpg", "E:\\Neck&Neck Photos\\Winter2016\\" + s + "-E1.jpg");
            }
        }

    }
}
