using Shopify.IO.Types;
using ShopifyHelper.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shopify_Manager.UI
{
    public partial class SpecialOps_frm : Form
    {
        public SpecialOps_frm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Product> p = Fields.CurrentStore.Products.GetList();

            foreach (Product pr in p)
            {


                string oldtags;

                DataTable dt = Fields.CachingDB.ExecuteDatatable("Select tags from products where id = " + pr.id.ToString());

                if (dt.Rows.Count > 0)
                {
                    oldtags = dt.Rows[0][0].ToString();
                }
                else
                {
                    oldtags = "";
                }

                Fields.CurrentStore.Products.UpdateTags(pr, oldtags + "," + pr.sku);

            }
        }
    }
}
