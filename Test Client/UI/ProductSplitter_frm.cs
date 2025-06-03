using Shopify.IO.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using ShopifyHelper.IO;

namespace Shopify_Manager.UI
{
    public partial class ProductSplitter_frm : Form
    {
        public ProductSplitter_frm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SplitProducts();
        }

        private void SplitProducts()
        {
            List<Product> cp = new List<Product>();

            cp = Fields.CurrentStore.Products.GetList();
            //go on all products in the site.
            foreach (Product p in cp)
            {
                //if (p.tags.Contains("summer") && p.tags.Contains("2016"))
                //{
                var o = p.options.Find(x => x.name == "Color");
                if (o != null && o.values.Count > 1)
                {
                    for (int i = 1; i <= o.values.Count - 1; i++)
                    {
                        //find all variants with the same color.
                        List<Variant> lv = p.variants.FindAll(x => x.option1 == o.values[i] || x.option2 == o.values[i]);
                        //create the new product and set all it's properties.
                        //===================================================
                        if (lv.Count > 0)
                        {
                            Product NP = new Product();
                            NP = Fields.CurrentStore.Products.GetProduct(p.id);
                            NP.tags = NP.tags + ", " + lv[0].sku;


                            //update variants sku by adding the color id to the existing computerNo.
                            //===================================================
                            string cid;
                            SqlCommand c = Fields.OceanDB.Connection.CreateCommand();
                            c.CommandText = "Select ColorID from MTD Where barcode = '" + lv[0].barcode + "'";
                            cid = (string)c.ExecuteScalar();

                            foreach (Variant v in lv)
                            {
                                v.sku = v.sku + "." + cid;
                            }


                            NP.variants = lv;
                            //===================================================
                            //remove tags that represents other coloers.
                            //===================================================
                            foreach (string s in o.values)
                            {
                                if (o.values[i] != s)
                                    NP.tags = NP.tags.Replace(s, "").Replace(", ,", ",");
                            }

                            NP.tags = NP.tags + ", split";
                            //remove all images, but keep the one related to the current color.

                            List<Image> ims = new List<Image>();
                            foreach (Image im in p.images)
                            {
                                if (im.id == lv[0].image_id)
                                {
                                    ims.Add(im);
                                }
                            }

                            NP.images = ims;
                            //===================================================
                            //create the new product with the select variants.
                            RootObject ap = Fields.CurrentStore.Products.AddProduct(NP);
                            if (ap.product != null)
                            {
                                //start deleting the saved variants from the master product.
                                foreach (Variant v in lv)
                                {
                                    //delete from shopify
                                    Fields.CurrentStore.Products.DeleteProductVariant(p, v);
                                    //delete from loop collections
                                    p.variants.Remove(v);

                                }
                            }
                            //===================================================
                        }
                        //===================================================
                    }
                    //update the original product
                    //===================================================
                    Fields.CurrentStore.Products.UpdateTags(p, p.tags + ", split");
                    //update variants sku by adding the color id to the existing computerNo.
                    //===================================================
                    string ocid;
                    SqlCommand oc = Fields.OceanDB.Connection.CreateCommand();
                    oc.CommandText = "Select ColorID from MTD Where barcode = '" + p.variants[0].barcode + "'";
                    ocid = (string)oc.ExecuteScalar();

                    foreach (Variant v in p.variants)
                    {
                        Fields.CurrentStore.Products.UpdateVariantSKU(v, v.sku + "." + ocid);
                    }


                    //===================================================
                }

                //}
            }
        }
    }
}
