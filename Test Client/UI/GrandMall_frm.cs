using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ShopifyHelper.IO;
using System.Net;

namespace Shopify_Manager.UI
{
    public partial class GrandMall_frm : Form
    {

        public GrandMall_frm()
        {
            InitializeComponent();
        }

        private void GenerateImagesExcel_btn_Click(object sender, EventArgs e)
        {
            //if (System.IO.Directory.Exists("E:\\OneDrive\\Grand Mall\\Latest Data\\AllPhotos"))
            //{
            //    System.IO.Directory.Move("E:\\OneDrive\\Grand Mall\\Latest Data", "E:\\OneDrive\\Grand Mall\\Latest Data History\\Before_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString());
            //}

            DataTable t = Fields.CachingDB.ExecuteDatatable(@"SELECT  derivedtbl_1.model ,
        derivedtbl_1.title_AR ,
        derivedtbl_1.title_EN ,
        derivedtbl_1.body_html_AR ,
        derivedtbl_1.body_html_EN ,
        Variants.option1 AS Color ,
        Variants.option2 AS Size ,
        Variants.inventory_quantity ,
        product_type ,
        derivedtbl_1.published_scope ,
        Images.src ,

        --Variants.id ,
        --derivedtbl_1.image ,
        --derivedtbl_1.handle,
        ( SELECT    EndUser
          FROM      ODB0001..MTI
          WHERE     ComputerNo = derivedtbl_1.model
        ) OriginalPrice ,
        ( SELECT    Sale
          FROM      ODB0001..MTI
          WHERE     ComputerNo = derivedtbl_1.model
        ) SalePrice
FROM    Variants
        INNER JOIN ( SELECT id ,
                            LEFT(OceanComputerNo, 7) AS model ,
                            title title_AR ,
                            title AS title_EN ,
                            (body_html
                            ) AS body_html_AR ,
                            (body_html
                            ) AS body_html_EN ,
                            handle ,
                            product_type ,
                            published_scope
                     FROM   Products
                     WHERE  ( vendor = 'Montania' /*and tags LIKE '%" + txt_tag.Text + @"%'*/ and published_at is not null)
                   ) AS derivedtbl_1 ON Variants.product_id = derivedtbl_1.id
        INNER JOIN Images ON Variants.image_id = Images.id
WHERE   ( Variants.inventory_quantity > 2 );");

            //create image folder.
            string lastComputerNo = "";
            string currentComputerNo = "";
            WebClient w = new WebClient();

            if (!System.IO.Directory.Exists("C:\\Grand Mall\\Latest Data\\AllPhotos"))
            {
                System.IO.Directory.CreateDirectory("C:\\Grand Mall\\Latest Data\\AllPhotos");
            }

            foreach (DataRow dr in t.Rows)
            {
                //set current computerno
                currentComputerNo = dr["model"].ToString();
                //----------------------

                if (!System.IO.Directory.Exists("C:\\Grand Mall\\Latest Data\\AllPhotos\\" + currentComputerNo))
                {
                    System.IO.Directory.CreateDirectory("C:\\Grand Mall\\Latest Data\\AllPhotos\\" + currentComputerNo);
                }

                //set master photo for the model.
                if (!System.IO.File.Exists("C:\\Grand Mall\\Latest Data\\AllPhotos\\" + currentComputerNo + "\\1.jpg"))
                {
                    w.DownloadFile(dr["src"].ToString(), "C:\\Grand Mall\\Latest Data\\AllPhotos\\" + currentComputerNo + "\\1.jpg");
                }

                //set photos for colors
                if (!System.IO.File.Exists("C:\\Grand Mall\\Latest Data\\AllPhotos\\" + currentComputerNo + "\\" + dr["Color"] + ".jpg"))
                {
                    w.DownloadFile(dr["src"].ToString(), "C:\\Grand Mall\\Latest Data\\AllPhotos\\" + currentComputerNo + "\\" + dr["Color"] + ".jpg");
                }
                //else if (!System.IO.File.Exists("E:\\OneDrive\\Grand Mall\\Latest Data\\AllPhotos\\" + currentComputerNo + "\\" + dr["Color"] + "_1.jpg"))
                //{
                //    w.DownloadFile(dr["src"].ToString(), "E:\\OneDrive\\Grand Mall\\Latest Data\\AllPhotos\\" + currentComputerNo + "\\" + dr["Color"] + "_1.jpg");
                //}
                //else if (!System.IO.File.Exists("E:\\OneDrive\\Grand Mall\\Latest Data\\AllPhotos\\" + currentComputerNo + "\\" + dr["Color"] + "_2.jpg"))
                //{
                //    w.DownloadFile(dr["src"].ToString(), "E:\\OneDrive\\Grand Mall\\Latest Data\\AllPhotos\\" + currentComputerNo + "\\" + dr["Color"] + "_2.jpg");
                //}

                //set last computerno
                lastComputerNo = currentComputerNo;
                //-------------------
            }

            //create excel file.
            DataSet ds = new DataSet();

            t.Columns.Remove("published_scope");
            t.Columns.Remove("src");


            ds.Tables.Add(t);

            

            ExportDataSetToExcel(ds);

        }
        private void ExportDataSetToExcel(DataSet ds)
        {
            ////Creae an Excel application instance
            //Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();

            //excelApp.Visible = true;
            ////Create an Excel workbook instance and open it from the predefined location
            //Microsoft.Office.Interop.Excel.Workbook excelWorkBook = excelApp.Workbooks.Add();//excelApp.Workbooks.Open("C:\\Grand Mall\\Latest Data\\ProductsData.xlsx");



            //foreach (DataTable table in ds.Tables)
            //{


            //    //Add a new worksheet to workbook with the Datatable name
            //    Microsoft.Office.Interop.Excel.Worksheet excelWorkSheet = excelWorkBook.Sheets.Add();

            //    excelWorkSheet.Name = table.TableName;

            //    for (int i = 1; i < table.Columns.Count + 1; i++)
            //    {
            //        excelWorkSheet.Cells[1, i] = table.Columns[i - 1].ColumnName;
            //    }

            //    for (int j = 0; j < table.Rows.Count; j++)
            //    {
            //        for (int k = 0; k < table.Columns.Count; k++)
            //        {
            //            excelWorkSheet.Cells[j + 2, k + 1] = table.Rows[j].ItemArray[k].ToString();
            //        }
            //    }
            //}
        }

        private void qmallExport_btn_Click(object sender, EventArgs e)
        {
            //if (System.IO.Directory.Exists("E:\\OneDrive\\QMALL\\Latest Data\\AllPhotos"))
            //{
            //    System.IO.Directory.Move("E:\\OneDrive\\QMALL\\Latest Data", "E:\\OneDrive\\QMALL\\Latest Data History\\Before_" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString());
            //}

            DataTable t = Fields.CachingDB.ExecuteDatatable("Exec prc_prepQmallData '" + txt_tag.Text.Trim());

            //create image folder.
            string lastComputerNo = "";
            string currentComputerNo = "";

            string foldername = "";

            foldername = DateTime.Now.Date.Day.ToString() + DateTime.Now.Date.Month.ToString() + DateTime.Now.Date.Year.ToString();


            WebClient w = new WebClient();

            if (!System.IO.Directory.Exists("C:\\QMALL\\Latest Data\\AllPhotos"))
            {
                System.IO.Directory.CreateDirectory("C:\\QMALL\\Latest Data\\" );
            }

            foreach (DataRow dr in t.Rows)
            {
                //set current computerno
                currentComputerNo = dr["ProductID"].ToString();
                //----------------------

                if (!System.IO.Directory.Exists("C:\\QMALL\\Latest Data\\AllPhotos\\" + currentComputerNo))
                {
                    System.IO.Directory.CreateDirectory("C:\\QMALL\\Latest Data\\AllPhotos\\" + currentComputerNo);
                }

                //set master photo for the model.
                if (!System.IO.File.Exists("C:\\QMALL\\Latest Data\\AllPhotos\\" + currentComputerNo + "\\1.jpg"))
                {
                    w.DownloadFile(dr["src"].ToString(), "C:\\QMALL\\Latest Data\\AllPhotos\\" + currentComputerNo + "\\1.jpg");
                }

                //set photos for colors
                if (!System.IO.File.Exists("C:\\QMALL\\Latest Data\\AllPhotos\\" + currentComputerNo + "\\" + dr["AttributeValue1"] + ".jpg"))
                {
                    w.DownloadFile(dr["src"].ToString(), "C:\\QMALL\\Latest Data\\AllPhotos\\" + currentComputerNo + "\\" + dr["AttributeValue1"] + ".jpg");
                }
                //else if (!System.IO.File.Exists("E:\\OneDrive\\QMALL\\Latest Data\\AllPhotos\\" + currentComputerNo + "\\" + dr["Color"] + "_1.jpg"))
                //{
                //    w.DownloadFile(dr["src"].ToString(), "E:\\OneDrive\\QMALL\\Latest Data\\AllPhotos\\" + currentComputerNo + "\\" + dr["Color"] + "_1.jpg");
                //}
                //else if (!System.IO.File.Exists("E:\\OneDrive\\QMALL\\Latest Data\\AllPhotos\\" + currentComputerNo + "\\" + dr["Color"] + "_2.jpg"))
                //{
                //    w.DownloadFile(dr["src"].ToString(), "E:\\OneDrive\\QMALL\\Latest Data\\AllPhotos\\" + currentComputerNo + "\\" + dr["Color"] + "_2.jpg");
                //}

                //set last computerno
                lastComputerNo = currentComputerNo;
                //-------------------
            }

            //create excel file.
            DataSet ds = new DataSet();

            t.Columns.Remove("published_scope");
            t.Columns.Remove("src");


            ds.Tables.Add(t);



            ExportDataSetToExcel(ds);
        }
    }
}
