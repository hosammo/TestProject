using Shopify.IO.Types;
using ShopifyHelper.IO;
using ShopifyHelper.IO.ODAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shopify_Manager.UI
{
    public partial class ManageProducts_frm : Form
    {
        Products_DAL pdal = new Products_DAL();
        Variants_DAL vdal = new Variants_DAL();
        DataSet CurrentOceanItem;
        List<Product> productsOnShopify = new List<Product>();
        //Product currentProduct = new Product();
        //List<string> barcodes = new List<string>();
        //List<string> ComputerNos = new List<string>();
        object obj_tempGridCellValue = null;
        public ManageProducts_frm()
        {
            InitializeComponent();
        }
        private void GetOceanItem(string ComputerNo)
        {

            CurrentOceanItem = Fields.OceanDB.ExecuteDataSet(@"SELECT  ComputerNo, ModelNo, MTName, MTEName, ItemName, ItemEName, 
	                            BrandName, BrandEName, DeptName, DeptEName, CountryName,
	                            CountryEName, FabricName, FabricEName, SeasonName, SeasonEName, 
	                            TypeName, TypeEName, ItemYear, Des, Whole, Special, Sale, 
	                            Clearance, EndUser FROM dbo.QMTI Where ComputerNo = '" + ComputerNo + @"'; 
                                SELECT Code,ComputerNo, SUM(Qty) Qty, BarCode, SizeName, ColorEName,ColorID, OrderIndex FROM dbo.QMTS WHERE ComputerNo = '" + ComputerNo + @"' GROUP BY Code , BarCode, SizeName, ColorEName,ComputerNo,ColorID, OrderIndex ORDER BY ColorID, OrderIndex
                                Select Distinct ColorEName, ColorID from qmtd where computerNo = '" + ComputerNo + "'");
        }
        private void Search_btn_Click(object sender, EventArgs e)
        {
            productsOnShopify.Clear();
            productsOnShopify_lst.DataSource = null;
            productsOnShopify_lst.Items.Clear();

            DataRow odr;

            GetOceanItem(search_txt.Text);

            if (CurrentOceanItem.Tables.Count == 0 | CurrentOceanItem.Tables[0].Rows.Count <= 0)
            {
                MessageBox.Show(this, "Item does not exist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                return;
            }

            odr = CurrentOceanItem.Tables[0].Rows[0];

            ComputerNo_txt.Text = odr["ComputerNo"].ToString();
            dept_txt.Text = odr["DeptEName"].ToString();
            Item_txt.Text = odr["ItemEName"].ToString();
            brand_txt.Text = odr["BrandEName"].ToString();
            endUser_txt.Text = odr["EndUser"].ToString();
            sales_txt.Text = odr["Sale"].ToString();
            clearance_txt.Text = odr["Clearance"].ToString();
            ModelNo_txt.Text = odr["ModelNo"].ToString();

            shopifyComparedTo_txt.Text = pdal.GetProductComparedAtPriceFromOcean(odr["ComputerNo"].ToString(),General.CurrentProfile.Currency).ToString();
            shopifyPrice_txt.Text = pdal.GetProductPriceFromOcean(odr["ComputerNo"].ToString(), General.CurrentProfile.Currency).ToString();


            //foreach (Product p in productsOnShopify)
            //{
            //    if (p.variants.Find(x => x.sku.Contains(search_txt.Text)) != null)
            //    {
            //        currentProduct.Add(p);
            //    }
            //}

            DataTable t = Fields.CachingDB.ExecuteDatatable("Select id,OceanComputerNo from products where OceanComputerNo like '%" + odr["ComputerNo"].ToString() + "%'");

            if (t.Rows.Count > 0)
            {
                foreach (DataRow r in t.Rows)
                {
                    Product p = Fields.CurrentStore.Products.GetProduct((long)r["id"]);
                    if (p != null)
                        productsOnShopify.Add(p);
                }
                productsOnShopify_lst.DataSource = productsOnShopify;
                productsOnShopify_lst.DisplayMember = "sku";
            }


            //show product status to user.
            if (productsOnShopify.Count <= 0)
            {
                shopifyExist_lbl.Text = "This product is not exist on shopify";
                shopifyExist_lbl.ForeColor = Color.Orange;
                variantsOnShopify_lbl.Text = "";
            }

            else if (t.Rows.Count == 1)
            {
                shopifyExist_lbl.Text = "This product is available on shopify";
                shopifyExist_lbl.ForeColor = Color.Green;
                if (productsOnShopify[0].variants.Count == CurrentOceanItem.Tables[1].Rows.Count)
                {
                    variantsOnShopify_lbl.Text = "All variants are available on shopify.";
                    variantsOnShopify_lbl.ForeColor = Color.Green;
                }
                else if (productsOnShopify[0].variants.Count > CurrentOceanItem.Tables[1].Rows.Count)
                {
                    variantsOnShopify_lbl.Text = "Product on shopify has more variants than product on ocean.";
                    variantsOnShopify_lbl.ForeColor = Color.Red;
                }
                else if (productsOnShopify[0].variants.Count < CurrentOceanItem.Tables[1].Rows.Count)
                {
                    variantsOnShopify_lbl.Text = "There is " + (CurrentOceanItem.Tables[1].Rows.Count - productsOnShopify[0].variants.Count).ToString() + " variants not available on shopify.";
                    variantsOnShopify_lbl.ForeColor = Color.Red;
                }
                shopifyTotalVariants_lbl.Text = "Total variants on Shopify = " + productsOnShopify[0].variants.Count.ToString();

            }

            else if (productsOnShopify.Count > 1)
            {
                if (isProductDuplicated(productsOnShopify))
                {
                    shopifyExist_lbl.Text = "This product is duplicated on shopify";
                    shopifyExist_lbl.ForeColor = Color.Red;
                    MessageBox.Show(this, "Please resolve duplicate first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    shopifyExist_lbl.Text = "This product is available on shopify";
                    foreach (Product p in productsOnShopify)
                    {
                        shopifyExist_lbl.ForeColor = Color.Green;
                        if (p.variants.Count == CurrentOceanItem.Tables[1].Rows.Count)
                        {
                            variantsOnShopify_lbl.Text = "All variants are available on shopify.";
                            variantsOnShopify_lbl.ForeColor = Color.Green;
                        }
                        else if (p.variants.Count > CurrentOceanItem.Tables[1].Rows.Count)
                        {
                            variantsOnShopify_lbl.Text = "Product on shopify has more variants than product on ocean.";
                            variantsOnShopify_lbl.ForeColor = Color.Red;
                        }
                        else if (p.variants.Count < CurrentOceanItem.Tables[1].Rows.Count)
                        {
                            variantsOnShopify_lbl.Text = "There is " + (CurrentOceanItem.Tables[1].Rows.Count - p.variants.Count).ToString() + " variants not available on shopify.";
                            variantsOnShopify_lbl.ForeColor = Color.Red;
                        }
                    }
                    shopifyTotalVariants_lbl.Text = "Total variants on Shopify = " + productsOnShopify[0].variants.Count.ToString();

                }
            }

            oceanTotalVariants_lbl.Text = "Total variants on Ocean = " + CurrentOceanItem.Tables[1].Rows.Count.ToString();

            variants_dgv.Rows.Clear();
            foreach (DataRow dr in CurrentOceanItem.Tables[1].Rows)
            {
                bool f = false;
                variants_dgv.Rows.Add(false, dr["ColorEname"].ToString(), dr["SizeName"].ToString(), dr["barcode"].ToString(), 0, dr["ColorID"].ToString());

                foreach (Product p in productsOnShopify)
                {
                    if (p.variants.Find(x => x.barcode.Contains(dr["barcode"].ToString())) != null)
                    {
                        f = true;
                        break;
                    }
                }
                
                if (productsOnShopify.Count > 0 && f)
                {
                    variants_dgv.Rows[variants_dgv.Rows.Count - 1].ReadOnly = true;
                    variants_dgv.Rows[variants_dgv.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Gray;
                }
            }

            //if (databaseSelector_cmb.SelectedIndex == 0)
            //    shopifyVendor_txt.Text = "Montania";
            //else
            //    shopifyVendor_txt.Text = brand_txt.Text;

            shopifyTitle_txt.Text = Item_txt.Text;


            tags_lbl.Text = generateTags();
        }
        private void NewProduct_frm_Load(object sender, EventArgs e)
        {
            //databaseSelector_cmb.SelectedIndex = 0;
            //EnableDisableSearch();
            Fields.GetNewProductsFromShopify();

            //Fill Lists
            //SKUs
            //Barcodes
            //Product Types
            //foreach (Product p in productsOnShopify)
            //{
            //    if (!shopifyProductType_cmd.Items.Contains(p.product_type))
            //    {
            //        shopifyProductType_cmd.Items.Add(p.product_type);
            //    }

            //    //fill computer nos List (SKUs) and barcodes
            //    //foreach (Variant v in p.variants)
            //    //{
            //    //    if (!ComputerNos.Contains(v.sku))
            //    //        ComputerNos.Add(v.sku);

            //    //    if (v.barcode != null && !barcodes.Contains(v.barcode))
            //    //        barcodes.Add(v.barcode);
            //    //    else if (v.barcode != null)
            //    //        barcodedublicates_dgv.Rows.Add(v.barcode);
            //    //}
            //}
            DataTable d = Fields.CachingDB.ExecuteDatatable("SELECT DISTINCT product_type FROM dbo.Products");
            shopifyProductType_cmb.DataSource = d;
            shopifyProductType_cmb.DisplayMember = "product_type";
        }
        private void SaveToShopify_btn_Click(object sender, EventArgs e)
        {
            string fulfillment_service = "manual";

            if (productsOnShopify.Count <= 0)
            {
                //the product is not exist on shopify at all, a new product(s) will be created.
                //list of options for the product.

                if (!productByColor_chk.Checked)
                {
                    List<Option> loo = new List<Option>();

                    //Color
                    Option color = new Option();
                    color.name = "Color";
                    color.values = new List<string>();

                    //Size
                    Option size = new Option();
                    size.name = "Size";
                    size.values = new List<string>();


                    // create variants
                    int position = 1;
                    List<Variant> lnv = new List<Variant>();
                    foreach (DataGridViewRow gr in variants_dgv.Rows)
                    {
                        DataGridViewCheckBoxCell cbs = (DataGridViewCheckBoxCell)gr.Cells["select_col"];

                        string sku = gr.Cells["barcode_col"].Value.ToString();

                        if (cbs.Value != null && (bool)cbs.Value == true)
                        {
                            Variant nv = new Variant(gr.Cells["Color_col"].Value.ToString() + " / " + gr.Cells["size_col"].Value.ToString(),
                                shopifyPrice_txt.Text,sku , position, "1000", "deny",
                                shopifyComparedTo_txt.Text,fulfillment_service, "shopify", gr.Cells["Color_col"].Value.ToString(), gr.Cells["size_col"].Value.ToString(),
                                null, true,chk_isTaxable.Checked, gr.Cells["barcode_col"].Value.ToString(), Convert.ToInt16(gr.Cells["qty_col"].Value));
                            lnv.Add(nv);

                            //fill list of color in this product.
                            if (!color.values.Contains(gr.Cells["Color_col"].Value.ToString()))
                                color.values.Add(gr.Cells["Color_col"].Value.ToString());

                            //fill list of sizes in this product.
                            if (!size.values.Contains(gr.Cells["size_col"].Value.ToString()))
                                size.values.Add(gr.Cells["size_col"].Value.ToString());

                            position += 1;
                        }
                    }

                    //fill the list of options
                    loo.Add(color); loo.Add(size);

                    //genarate available at list


                    Product np = new Product(shopifyTitle_txt.Text, "New!", shopifyVendor_txt.Text, shopifyProductType_cmb.Text, false, lnv, loo, tags_lbl.Text);

                    RootObject addedProduct = Fields.CurrentStore.Products.AddProduct(np);

                    if (addedProduct.product != null)
                    {
                        MessageBox.Show(this, "The product " + ComputerNo_txt.Text + " has been added and had this id: " + addedProduct.product.id + ".", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                        pdal.AddProduct(addedProduct.product);
                    }
                    else
                    {
                        MessageBox.Show(this, "Failed to add product, \n" + addedProduct.errors.title, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    }

                }
                else
                {
                    //create list of selected colors
                    List<string> s = SelectedColors();

                    foreach (string si in s)
                    {
                        List<Option> loo = new List<Option>();

                        //Color
                        Option color = new Option();
                        color.name = "Color";
                        color.values = new List<string>();

                        //Size
                        Option size = new Option();
                        size.name = "Size";
                        size.values = new List<string>();


                        // create variants
                        int position = 1;
                        List<Variant> lnv = new List<Variant>();
                        foreach (DataGridViewRow gr in variants_dgv.Rows)
                        {
                            DataGridViewCheckBoxCell cbs = (DataGridViewCheckBoxCell)gr.Cells["select_col"];

                            string sku = gr.Cells["barcode_col"].Value.ToString();
                            if (cbs.Value != null && (bool)cbs.Value == true && gr.Cells["Color_col"].Value.ToString() == si)
                            {
                                Variant nv = new Variant(gr.Cells["Color_col"].Value.ToString() + " / " + gr.Cells["size_col"].Value.ToString(),
                                    shopifyPrice_txt.Text, sku, position, "1000", "deny",
                                    shopifyComparedTo_txt.Text, fulfillment_service, "shopify", gr.Cells["Color_col"].Value.ToString(), gr.Cells["size_col"].Value.ToString(),
                                    null, true, chk_isTaxable.Checked, gr.Cells["barcode_col"].Value.ToString(), Convert.ToInt16(gr.Cells["qty_col"].Value));
                                lnv.Add(nv);

                                //fill list of color in this product.
                                if (!color.values.Contains(gr.Cells["Color_col"].Value.ToString()))
                                    color.values.Add(gr.Cells["Color_col"].Value.ToString());

                                //fill list of sizes in this product.
                                if (!size.values.Contains(gr.Cells["size_col"].Value.ToString()))
                                    size.values.Add(gr.Cells["size_col"].Value.ToString());

                                position += 1;
                            }
                        }

                        //fill the list of options
                        loo.Add(color); loo.Add(size);

                        Product np = new Product(shopifyTitle_txt.Text, "New!", shopifyVendor_txt.Text, shopifyProductType_cmb.Text, false, lnv, loo, tags_lbl.Text);

                        RootObject addedProduct = Fields.CurrentStore.Products.AddProduct(np);

                        if (addedProduct.product != null)
                        {
                            MessageBox.Show(this, "The product " + ComputerNo_txt.Text + " has been added and had this id: " + addedProduct.product.id + ".", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            pdal.AddProduct(addedProduct.product);
                        }
                        else
                        {
                            MessageBox.Show(this, "Failed to add product, \n" + addedProduct.errors.title, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        }
                    }

                }
            }
            //when the product is exist but some of it's variant not.
            //when only one product available on shopify and has more than one color in options.
            else if (productsOnShopify.Count < CurrentOceanItem.Tables[2].Rows.Count /*&& productsOnShopify[0].options.Find(o => o.name == "Color").values.Count > 1*/)
            {

                if(!productByColor_chk.Checked)
                {
                    foreach (DataGridViewRow gr in variants_dgv.Rows)
                    {
                        //List<Variant> savedVarians = new List<Variant>();
                        DataGridViewCheckBoxCell cbs = (DataGridViewCheckBoxCell)gr.Cells["select_col"];
                        if (cbs.Value != null && (bool)cbs.Value == true)
                        {
                            Variant nv = new Variant(gr.Cells["Color_col"].Value.ToString() + " / " + gr.Cells["size_col"].Value.ToString(),
                                shopifyPrice_txt.Text, ComputerNo_txt.Text, 0, "1000", "deny",
                                shopifyComparedTo_txt.Text, fulfillment_service, "shopify", gr.Cells["Color_col"].Value.ToString(), gr.Cells["size_col"].Value.ToString(),
                                null, true, chk_isTaxable.Checked, gr.Cells["barcode_col"].Value.ToString(), Convert.ToInt16(gr.Cells["qty_col"].Value));

                            RootObject addedVariant = Fields.CurrentStore.Products.AddProductVariant(productsOnShopify[0], nv);
                            if (addedVariant.variant != null)
                            {
                                MessageBox.Show(this, "The variant " + gr.Cells["barcode_col"].Value.ToString() + " has been added and had this id: " + addedVariant.variant.id + ".", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                                vdal.AddVariant(addedVariant.variant);
                            }
                            else
                            {
                                MessageBox.Show(this, "Failed to add product, \n" + addedVariant.errors.title, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                            }
                        }
                    }
                }
                else
                {
                    //create list of selected colors
                    List<string> s = SelectedColors();

                    foreach (string si in s)
                    {
                        List<Option> loo = new List<Option>();

                        //Color
                        Option color = new Option();
                        color.name = "Color";
                        color.values = new List<string>();

                        //Size
                        Option size = new Option();
                        size.name = "Size";
                        size.values = new List<string>();


                        // create variants
                        int position = 1;
                        List<Variant> lnv = new List<Variant>();
                        foreach (DataGridViewRow gr in variants_dgv.Rows)
                        {
                            DataGridViewCheckBoxCell cbs = (DataGridViewCheckBoxCell)gr.Cells["select_col"];

                            if (cbs.Value != null && (bool)cbs.Value == true && gr.Cells["Color_col"].Value.ToString() == si)
                            {
                                Variant nv = new Variant(gr.Cells["Color_col"].Value.ToString() + " / " + gr.Cells["size_col"].Value.ToString(),
                                    shopifyPrice_txt.Text, ComputerNo_txt.Text /*+ "." + gr.Cells["colorid_col"].Value.ToString()*/, position, "1000", "deny",
                                    shopifyComparedTo_txt.Text, fulfillment_service, "shopify", gr.Cells["Color_col"].Value.ToString(), gr.Cells["size_col"].Value.ToString(),
                                    null, true, chk_isTaxable.Checked, gr.Cells["barcode_col"].Value.ToString(), Convert.ToInt16(gr.Cells["qty_col"].Value));
                                lnv.Add(nv);

                                //fill list of color in this product.
                                if (!color.values.Contains(gr.Cells["Color_col"].Value.ToString()))
                                    color.values.Add(gr.Cells["Color_col"].Value.ToString());

                                //fill list of sizes in this product.
                                if (!size.values.Contains(gr.Cells["size_col"].Value.ToString()))
                                    size.values.Add(gr.Cells["size_col"].Value.ToString());

                                position += 1;
                            }
                        }

                        //fill the list of options
                        loo.Add(color); loo.Add(size);

                        Product np = new Product(shopifyTitle_txt.Text, "New!", shopifyVendor_txt.Text, shopifyProductType_cmb.Text, false, lnv, loo, tags_lbl.Text);

                        RootObject addedProduct = Fields.CurrentStore.Products.AddProduct(np);

                        if (addedProduct.product != null)
                        {
                            MessageBox.Show(this, "The product " + ComputerNo_txt.Text + " has been added and had this id: " + addedProduct.product.id + ".", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                            pdal.AddProduct(addedProduct.product);
                        }
                        else
                        {
                            MessageBox.Show(this, "Failed to add product, \n" + addedProduct.errors.title, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                        }
                    }

                }
            }
            else if (productsOnShopify.Count > 1)
            {
                //list of options for the product.
                List<Option> loo = new List<Option>();

                //Color
                Option color = new Option();
                color.name = "Color";
                color.values = new List<string>();

                //Size
                Option size = new Option();
                size.name = "Size";
                size.values = new List<string>();


                // create variants
                int position = 1;
                List<Variant> lnv = new List<Variant>();
                foreach (DataGridViewRow gr in variants_dgv.Rows)
                {
                    DataGridViewCheckBoxCell cbs = (DataGridViewCheckBoxCell)gr.Cells["select_col"];

                    if (cbs.Value != null && (bool)cbs.Value == true)
                    {
                        Variant nv = new Variant(gr.Cells["Color_col"].Value.ToString() + " / " + gr.Cells["size_col"].Value.ToString(),
                            shopifyPrice_txt.Text, ComputerNo_txt.Text, position, "1000", "deny",
                            shopifyComparedTo_txt.Text, fulfillment_service, "shopify", gr.Cells["Color_col"].Value.ToString(), gr.Cells["size_col"].Value.ToString(),
                            null, true, chk_isTaxable.Checked, gr.Cells["barcode_col"].Value.ToString(), Convert.ToInt16(gr.Cells["qty_col"].Value));
                        lnv.Add(nv);

                        //fill list of color in this product.
                        if (!color.values.Contains(gr.Cells["Color_col"].Value.ToString()))
                            color.values.Add(gr.Cells["Color_col"].Value.ToString());

                        //fill list of sizes in this product.
                        if (!size.values.Contains(gr.Cells["size_col"].Value.ToString()))
                            size.values.Add(gr.Cells["size_col"].Value.ToString());

                        position += 1;
                    }
                }

                //fill the list of options
                loo.Add(color); loo.Add(size);

                Product np = new Product(shopifyTitle_txt.Text, "New!", shopifyVendor_txt.Text, shopifyProductType_cmb.Text, false, lnv, loo, tags_lbl.Text);

                RootObject addedProduct = Fields.CurrentStore.Products.AddProduct(np);

                if (addedProduct.product != null)
                {
                    MessageBox.Show(this, "The product " + ComputerNo_txt.Text + " has been added and had this id: " + addedProduct.product.id + ".", "Done!", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    pdal.AddProduct(addedProduct.product);
                }
                else
                {
                    MessageBox.Show(this, "Failed to add product, \n" + addedProduct.errors.title, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
        }
        private string generateTags()
        {
            string tags = "";

            if (addToTags_lst.CheckedItems.Contains("ShowInApp"))
            {
                tags += "ShowInApp" + ",";
            }


            if (addToTags_lst.CheckedItems.Contains("ComputerNo"))
            {
                tags += ComputerNo_txt.Text + ",";
            }

            if (addToTags_lst.CheckedItems.Contains("Color"))
            {
                foreach (DataGridViewRow gr in variants_dgv.Rows)
                {
                    //List<Variant> savedVarians = new List<Variant>();
                    DataGridViewCheckBoxCell cbs = (DataGridViewCheckBoxCell)gr.Cells["select_col"];
                    if (cbs.Value != null && (bool)cbs.Value == true)
                    {
                        if (!tags.Contains(gr.Cells["Color_col"].Value.ToString()))
                            tags += gr.Cells["Color_col"].Value.ToString() + ",";
                    }
                }
            }

            if (addToTags_lst.CheckedItems.Contains("Size"))
            {
                foreach (DataGridViewRow gr in variants_dgv.Rows)
                {
                    //List<Variant> savedVarians = new List<Variant>();
                    DataGridViewCheckBoxCell cbs = (DataGridViewCheckBoxCell)gr.Cells["select_col"];
                    if (cbs.Value != null && (bool)cbs.Value == true)
                    {
                        if (!tags.Contains(gr.Cells["size_col"].Value.ToString()))
                            tags += gr.Cells["size_col"].Value.ToString() + ",";
                    }
                }

            }

            if (addToTags_lst.CheckedItems.Contains("Dept"))
            {
                tags += CurrentOceanItem.Tables[0].Rows[0]["DeptEName"].ToString() + ",";
            }

            if (addToTags_lst.CheckedItems.Contains("Brand"))
            {
                tags += shopifyVendor_txt.Text + ",";
            }

            if (addToTags_lst.CheckedItems.Contains("Season"))
            {
                tags += CurrentOceanItem.Tables[0].Rows[0]["SeasonEName"].ToString() + ",";
            }

            if (addToTags_lst.CheckedItems.Contains("Year"))
            {
                tags += CurrentOceanItem.Tables[0].Rows[0]["ItemYear"].ToString() + ",";
            }



            if (tags.Length > 0)
                return tags.Remove(tags.Length - 1, 1);
            else
                return "";

        }
        private void checkAll_chk_CheckedChanged(object sender, EventArgs e)
        {
            if (variants_dgv.Rows.Count > 0)
            {
                foreach (DataGridViewRow gr in variants_dgv.Rows)
                {
                    if (gr.ReadOnly != true)
                        ((DataGridViewCheckBoxCell)gr.Cells["select_col"]).Value = checkAll_chk.Checked;
                }
            }
            tags_lbl.Text = generateTags();
        }
        private void variants_dgv_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                DataGridView dg = (DataGridView)sender;
                string newValue = dg.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                foreach (DataGridViewRow gr in variants_dgv.Rows)
                {
                    if (gr.Cells[e.ColumnIndex].Value.ToString() == (string)obj_tempGridCellValue)
                        gr.Cells[e.ColumnIndex].Value = newValue;
                }
            }
            if (e.ColumnIndex != -1)
            {
                tags_lbl.Text = generateTags();
            }
        }
        //private void EnableDisableSearch()
        //{
        //    if (databaseSelector_cmb.Text == "")
        //        Search_btn.Enabled = false;
        //    else
        //        Search_btn.Enabled = true;
        //}
        private void variants_dgv_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            obj_tempGridCellValue = variants_dgv.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        }
        //private void databaseSelector_cmb_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    EnableDisableSearch();
        //}
        private void variants_dgv_Leave(object sender, EventArgs e)
        {
            tags_lbl.Text = generateTags();
        }
        List<string> SelectedColors()
        {
            List<string> s = new List<string>();
            foreach (DataGridViewRow gr in variants_dgv.Rows)
            {
                DataGridViewCheckBoxCell cbs = (DataGridViewCheckBoxCell)gr.Cells["select_col"];
                if (cbs.Value != null && (bool)cbs.Value == true)
                {
                    if (!s.Contains(gr.Cells["Color_col"].Value.ToString()))
                        s.Add(gr.Cells["Color_col"].Value.ToString());
                }
            }
            return s;
        }
        bool isProductDuplicated(List<Product> listOfProducts)
        {
            foreach (Product p in listOfProducts)
            {
                if (listOfProducts.FindAll(pa => pa.sku == p.sku).Count > 1)
                    return true;
            }
            return false;
        }
        List<Variant> GetVariantsList(List<Product> ListOfProducts)
        {
            List<Variant> v = new List<Variant>();
            foreach (Product p in ListOfProducts)
            {
                foreach (Variant sv in p.variants)
                {
                    v.Add(sv);
                }
            }
            return v;
        }
        private void SaveRecommended_btn_Click(object sender, EventArgs e)
        {

            DataTable dt = Fields.CachingDB.ExecuteDatatable("Select handle from products where OceanComputerNo = '" + computerno_tometa_txt.Text + "'");

            if (dt.Rows.Count > 0)
            {
                List<Metafield> lom = Fields.CurrentStore.Metafields.GetProductMetafields((Product)productsOnShopify_lst.SelectedItem);
                Metafield m = lom.Find(p => p.@namespace == "recommendations" && p.key == "productHandles");
                if (m != null)
                {
                    m.value += dt.Rows[0]["handle"];
                    RootObject ro = Fields.CurrentStore.Metafields.UpdateProductMetafield((Product)productsOnShopify_lst.SelectedItem, m);
                }
                else
                {
                    Metafield nm = new Metafield();
                    nm.@namespace = "recommendations"; nm.key = "productHandles"; nm.value += dt.Rows[0]["handle"]; nm.value_type = "string";
                    RootObject ro = Fields.CurrentStore.Metafields.AddProductMetafield((Product)productsOnShopify_lst.SelectedItem, nm);
                }
                recommended_lst.Items.Add(computerno_tometa_txt.Text);
            }
        }
        private void productsOnShopify_lst_SelectedIndexChanged(object sender, EventArgs e)
        {
            recommended_lst.Items.Clear();
            Metafield m = Fields.CurrentStore.Metafields.GetProductMetafields((Product)productsOnShopify_lst.SelectedItem).Find(p => p.@namespace == "recommendations" && p.key == "productHandles");

            if (m != null)
            {
                string[] s = m.value.Split(',');
                foreach (string si in s)
                {
                    DataTable dt = Fields.CachingDB.ExecuteDatatable("Select OceanComputerNo from products where handle = '" + si + "'");
                    recommended_lst.Items.Add(dt.Rows[0]["OceanComputerNo"]);
                }
            }


        }

        private string getSizeChart(string computerNo)
        {
            StringBuilder htmlString = new StringBuilder();
            //bool isFirstRow = true;
            DataTable SizeList = Fields.OceanDB.ExecuteDatatable(@"SELECT  SizeName
FROM    ( SELECT DISTINCT
                    Sizes.SizeName ,
                    Sizes.OrderIndex
          FROM      SizeChart
                    INNER JOIN SizeChartMeasurements ON SizeChart.MeasurementID = SizeChartMeasurements.MeasurementID
                    INNER JOIN Sizes ON SizeChart.SizeID = Sizes.SizeID
          WHERE     ( SizeChart.ComputerNo = '1705046' )
        ) AS SizeList
ORDER BY SizeList.OrderIndex;");

            string pivotStr = "PIVOT ( SUM(MeasurementValue) FOR SizeName IN ( ";
            foreach (DataRow item in SizeList.Rows)
            {
                pivotStr += "[" + item[0].ToString() + "],";
            }
            pivotStr = pivotStr.Remove(pivotStr.Length - 1, 1);
            pivotStr += ")) AS pivotTable";

            DataTable PivotDt = Fields.OceanDB.ExecuteDatatable(@"SELECT *
FROM    ( SELECT    SizeChart.MeasurementValue ,
                    SizeChartMeasurements.MeasurementName [المقاس] ,
                    SizeChartMeasurements.MeasurementEName [Size] ,
                    Sizes.SizeName
          FROM      SizeChart
                    INNER JOIN SizeChartMeasurements ON SizeChart.MeasurementID = SizeChartMeasurements.MeasurementID
                    INNER JOIN Sizes ON SizeChart.SizeID = Sizes.SizeID
          WHERE     ComputerNo = '" + computerNo + "') AS pvt " + pivotStr);

            PivotDt.Columns[0].SetOrdinal(PivotDt.Columns.Count - 1);



            return ConvertDataTableToHTML(PivotDt);
        }

        private string getProductAvailability(string computerNo, string colorID)
        {
            return "";
        }

        private string getProductAvailability(string computerNo)
        {
            StringBuilder htmlString = new StringBuilder();
            int countyCounter = 0;
            DataTable dt = Fields.OceanDB.ExecuteDatatable(@" QMTS.BrancheName, BranchesGroup.GroupName
FROM QMTS INNER JOIN Branches ON QMTS.BranchID = Branches.BranchID INNER JOIN BranchesGroup ON Branches.BranchID = BranchesGroup.BranchID
WHERE (QMTS.ComputerNo = '" + computerNo + "') AND (QMTS.Qty > 0) AND (Branches.TypeID = 2)");

            htmlString.Append("<h2>Available At:</h2>");

            foreach (DataRow r in dt.Rows)
            {
                if (countyCounter == 0)
                {
                    htmlString.Append("<p><span><strong>" + r[""] + "</strong></span></p>");
                }
                //htmlString.AppendLine()
            }


            return "";
        }

        public static string ConvertDataTableToHTML(DataTable dt)
        {
            string html = "<table>";
            //add header row
            html += "<tr>";
            for (int i = 0; i < dt.Columns.Count; i++)
                html += "<td>" + dt.Columns[i].ColumnName + "</td>";
            html += "</tr>";
            //add rows
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                    html += "<td>" + dt.Rows[i][j].ToString() + "</td>";
                html += "</tr>";
            }
            html += "</table>";
            return html;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string str = getSizeChart(ComputerNo_txt.Text);
        }

        private void addToTags_lst_SelectedIndexChanged(object sender, EventArgs e)
        {
            tags_lbl.Text = generateTags();
        }
    }
}
