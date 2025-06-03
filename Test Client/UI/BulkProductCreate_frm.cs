using Shopify.IO.Types;
using ShopifyHelper.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shopify_Manager.UI
{
    public partial class BulkProductCreate_frm : Form
    {
        List<Product> addedProducts = new List<Product>();
        List<string> FailureProducts = new List<string>();
        public BulkProductCreate_frm()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            listBox1.Refresh();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BulkProductInsert b = new BulkProductInsert();
            List<string> s = new List<string>();

            foreach (string c in addToTags_lst.CheckedItems)
            {
                s.Add(c);
            }


            b.uploadProducts(Fields.CurrentStore.CurrentSroreAPIAccess, productByColor_chk.Checked, PhotosFolder_txt.Text, delimiter1_txt.Text[0], delimiter2_txt.Text[0], s, ref addedProducts, ref FailureProducts);


            File.WriteAllLines("C:\\AddLog", FailureProducts);

        }

        private void start_btn_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void browse_btn_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog n = new FolderBrowserDialog();

            n.ShowNewFolderButton = false;

            n.ShowDialog(this);

            PhotosFolder_txt.Text = n.SelectedPath;
        }

        private void BulkProductCreate_frm_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = addedProducts;
            listBox1.DataSource = FailureProducts;

        }
    }
}
