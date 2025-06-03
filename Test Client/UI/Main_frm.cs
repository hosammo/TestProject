using Shopify.IO.Operations;
using Shopify.IO.Types;
using Shopify_Manager.SettingsTypes;
using ShopifyHelper.IO;
using ShopifyHelper.IO.ODAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Shopify_Manager.UI
{
    public partial class Main_frm : Form
    {
        

        public Main_frm()
        {
            InitializeComponent();
        }

        private void Main_frm_Load(object sender, EventArgs e)
        {
            //load settings
            General.SettingsObj = new Settings();
            General.SettingsObj.LoadSettings();

            ProfileSelector_frm selector = new ProfileSelector_frm();
            selector.ShowDialog(this);

            Fields.CachingDB = new DBContext(General.CurrentProfile.CachingDB, Properties.Settings.Default.OceanSQL);
            Fields.OceanDB = new DBContext(General.CurrentProfile.OceanDB, Properties.Settings.Default.OceanSQL);

            version_lbl.Text = version_lbl.Text.Replace("{version}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
            OceanDB_TSSL.Text = General.CurrentProfile.OceanDB;
            Profile_TSSL.Text = General.CurrentProfile.ProfileName;
            CachingDB_TSSL.Text = General.CurrentProfile.CachingDB;
        }

        private void DownloadProducts_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            Fields.ClearCachingDB();
            Fields.GetNewProductsFromShopify();
        }

        private void ManageProducts_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageProducts_frm obj = new ManageProducts_frm();
            obj.MdiParent = this;
            obj.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            test obj = new test();
            obj.Show();
        }

        private void bulkCreate_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            BulkProductCreate_frm obj = new BulkProductCreate_frm();
            obj.Show(this);
        }

        private void StockUpdate_toolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShopifyQtyUpdate_frm obj = new ShopifyQtyUpdate_frm();
            obj.MdiParent = this;
            obj.Show();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ProductSplitter_frm obj = new ProductSplitter_frm();
            obj.Show(this);
        }

        private void grandMallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GrandMall_frm gm = new GrandMall_frm();
            gm.Show(this);
        }

        private void specialOpsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SpecialOps_frm obj = new SpecialOps_frm();
            obj.Show(this);

        }

        private void locationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ManageLocations_frm obj = new ManageLocations_frm();
            obj.MdiParent = this;
            obj.Show();
        }
    }
}
