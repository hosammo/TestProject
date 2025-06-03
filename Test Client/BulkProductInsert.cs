using Shopify.IO.Types;
using ShopifyHelper.IO;
using ShopifyHelper.IO.ODAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shopify_Manager
{
    public class BulkProductInsert
    {
        List<Product> productsOnShopify = new List<Product>();
        DataSet CurrentOceanItem;
        Products_DAL pdal = new Products_DAL();
        Variants_DAL vdal = new Variants_DAL();

        public void uploadProducts(Shopify.IO.helpers.APIAccess store, bool splitToColors, string photosFolder, char delimiter1, char delimiter2, List<string> SelectedTags, ref List<Product> addedProducts, ref List<string> failureLog)
        {

            string[] images = Directory.GetFiles(photosFolder, "*.JPG").Select(path => Path.GetFileName(path)).ToArray();
            List<ComputerNoDetails> modelList = new List<ComputerNoDetails>();
            string productTags; string productItemName;
            string shopify_price; string shopify_compareatprice; string shopify_vendorName; string shopify_productType;
            //create lists from file names
            //ComputerNo List
            //15I10105.36-A1.jpg
            //ComputerNo.ColorID-ImageSize+Sequence.FileType
            foreach (string s in images)
            {
                string[] ss = s.Split(delimiter1);
                string computerno = ss[0];

                string colorid;
                string seq;

                if (delimiter1 == delimiter2)
                {
                    colorid = ss[1];
                    seq = ss[2];
                }
                else
                {
                    colorid = ss[1].Split(delimiter2)[0];
                    seq = ss[1].Split(delimiter2)[1];
                }

                modelList.Add(new ComputerNoDetails { computerno = computerno, colorid = colorid, sequence = seq, filename = s });
            }

            foreach (string computerno in modelList.Select(x => x.computerno).Distinct())
            {
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////this code is used from ManageProducts_frm   /////////////////////////////////////////////////
                ///////this code will search for a product in ocean////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////

                //create colorid's list
                string colorlist = "";
                foreach (string C in modelList.Where(x => x.computerno == computerno).Select(y => y.colorid).Distinct())
                {
                    colorlist += "'" + C + "',";
                }

                DataRow odr;

                GetOceanItem(computerno, colorlist.Remove(colorlist.Length - 1, 1));

                if (CurrentOceanItem.Tables.Count == 0 | CurrentOceanItem.Tables[0].Rows.Count <= 0)
                {
                    failureLog.Add("Error, Item does not exist" + " ComputerNo = " + computerno);
                    continue;
                }

                odr = CurrentOceanItem.Tables[0].Rows[0];

                //ComputerNo_txt.Text = odr["ComputerNo"].ToString();
                //dept_txt.Text = odr["DeptEName"].ToString();
                //Item_txt.Text = odr["ItemEName"].ToString();

                //endUser_txt.Text = odr["EndUser"].ToString();
                //sales_txt.Text = odr["Sale"].ToString();
                //clearance_txt.Text = odr["Clearance"].ToString();
                //ModelNo_txt.Text = odr["ModelNo"].ToString();
                shopify_productType = pdal.ItemNameAlternative(odr["ItemEName"].ToString());
                shopify_vendorName = pdal.VendorNameAlternative(odr["BrandEName"].ToString());
                shopify_compareatprice = pdal.GetProductComparedAtPriceFromOcean(odr["ComputerNo"].ToString(), General.CurrentProfile.Currency).ToString();
                shopify_price = pdal.GetProductPriceFromOcean(odr["ComputerNo"].ToString(), General.CurrentProfile.Currency).ToString();

                DataTable t = Fields.CachingDB.ExecuteDatatable("Select id,OceanComputerNo from products where OceanComputerNo = '" + odr["ComputerNo"].ToString() +  "'");

                if (t.Rows.Count > 0)
                {
                    foreach (DataRow r in t.Rows)
                    {
                        Product p = Fields.CurrentStore.Products.GetProduct((long)r["id"]);
                        if (p != null)
                            productsOnShopify.Add(p);
                    }
                    //productsOnShopify_lst.DataSource = productsOnShopify;
                    //productsOnShopify_lst.DisplayMember = "sku";
                }


                //show product status to user.
                if (productsOnShopify.Count <= 0)
                {
                    failureLog.Add("This product is not exist on shopify, " + computerno);
                }

                else if (t.Rows.Count == 1)
                {
                    failureLog.Add("This product is available on shopify, " + computerno);
                    if (productsOnShopify[0].variants.Count == CurrentOceanItem.Tables[1].Rows.Count)
                    {
                        failureLog.Add("All variants are available on shopify, " + computerno);
                    }
                    else if (productsOnShopify[0].variants.Count > CurrentOceanItem.Tables[1].Rows.Count)
                    {
                        failureLog.Add("Product on shopify has more variants than product on ocean, " + computerno);
                    }
                    else if (productsOnShopify[0].variants.Count < CurrentOceanItem.Tables[1].Rows.Count)
                    {
                        failureLog.Add("There is " + (CurrentOceanItem.Tables[1].Rows.Count - productsOnShopify[0].variants.Count).ToString() + " variants not available on shopify, " + computerno);
                    }
                    failureLog.Add("Total variants on Shopify = " + productsOnShopify[0].variants.Count.ToString() + ", " + computerno);

                }

                else if (productsOnShopify.Count > 1)
                {
                    if (isProductDuplicated(productsOnShopify))
                    {
                        failureLog.Add("This product is duplicated on shopify, " + computerno + ", Aborted, starting next product.");
                        continue;
                    }
                    else
                    {
                        failureLog.Add("This product is available on shopify");
                        foreach (Product p in productsOnShopify)
                        {
                            if (p.variants.Count == CurrentOceanItem.Tables[1].Rows.Count)
                            {
                                failureLog.Add("All variants are available on shopify, " + computerno);
                            }
                            else if (productsOnShopify[0].variants.Count > CurrentOceanItem.Tables[1].Rows.Count)
                            {
                                failureLog.Add("Product on shopify has more variants than product on ocean, " + computerno);
                            }
                            else if (productsOnShopify[0].variants.Count < CurrentOceanItem.Tables[1].Rows.Count)
                            {
                                failureLog.Add("There is " + (CurrentOceanItem.Tables[1].Rows.Count - productsOnShopify[0].variants.Count).ToString() + " variants not available on shopify, " + computerno);
                            }
                        }
                        failureLog.Add("Total variants on Shopify = " + productsOnShopify[0].variants.Count.ToString() + ", " + computerno);

                    }
                }

                failureLog.Add("Total variants on Ocean = " + CurrentOceanItem.Tables[1].Rows.Count.ToString() + ", " + computerno);

                //variants_dgv.Rows.Clear();
                //foreach (DataRow dr in CurrentOceanItem.Tables[1].Rows)
                //{
                //    variants_dgv.Rows.Add(false, dr["ColorEname"].ToString(), dr["SizeName"].ToString(), dr["barcode"].ToString(), vdal.GetVariantQtyFromOcean(dr["barcode"].ToString()), dr["ColorID"].ToString());

                //    if (productsOnShopify.Count > 0 && productsOnShopify[0].variants.Find(x => x.barcode.Contains(dr["barcode"].ToString())) != null)
                //    {
                //        variants_dgv.Rows[variants_dgv.Rows.Count - 1].ReadOnly = true;
                //        variants_dgv.Rows[variants_dgv.Rows.Count - 1].DefaultCellStyle.BackColor = Color.Gray;
                //    }
                //}


                productItemName = odr["ItemEName"].ToString();


                productTags = generateTags(SelectedTags);


                //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                //-------------------------------------------------------------------------------------------------------->
                ///////////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////////////
                ///////this code is used from ManageProducts_frm  //////////////////////////////////////////////////
                ///////this code will upload products from shopify/////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////////
                ////////////////////////////////////////////////////////////////////////////////////////////////
                ///////////////////////////////////////////////////////////////////////////////////////////////
                //////////////////////////////////////////////////////////////////////////////////////////////
                /////////////////////////////////////////////////////////////////////////////////////////////




                if (productsOnShopify.Count <= 0)
                {
                    //the product is not exist on shopify at all, a new product(s) will be created.
                    //list of options for the product.

                    if (!splitToColors)
                    {
                        //create images List
                        List<Image> productImages = new List<Image>();
                        List<string> filenames = new List<string>();
                        int imageposition = 1;
                        filenames = modelList.Where(x => x.computerno == computerno).OrderBy(z => z.filename).Select(y => y.filename).ToList();


                        foreach (string filename in filenames)
                        {
                            System.Drawing.Image img = System.Drawing.Image.FromFile(photosFolder + "\\" + filename);
                            byte[] arr;
                            using (MemoryStream ms = new MemoryStream())
                            {
                                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                arr = ms.ToArray();

                                Image shm = new Image();
                                shm.position = imageposition;
                                shm.attachment = Convert.ToBase64String(arr);

                                productImages.Add(shm);

                                imageposition += 1;
                            }
                        }

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
                        foreach (DataRow dr in CurrentOceanItem.Tables[1].Rows)
                        {
                            Variant nv = new Variant(vdal.ColorNameAlternative(dr["ColorEName"].ToString()) + " / " + dr["SizeName"].ToString(),
                                shopify_price, computerno, position, "1000", "deny",
                                shopify_compareatprice, "FedEx", "shopify", dr["ColorEName"].ToString(), dr["SizeName"].ToString(),
                                null, true, false, dr["barcode"].ToString(),0);
                            lnv.Add(nv);

                            //fill list of color in this product.
                            if (!color.values.Contains(vdal.ColorNameAlternative(dr["ColorEName"].ToString())))
                                color.values.Add(vdal.ColorNameAlternative(dr["ColorEName"].ToString()));


                            //fill list of sizes in this product.
                            if (!size.values.Contains(dr["SizeName"].ToString()))
                                size.values.Add(dr["SizeName"].ToString());

                        }

                        //fill the list of options
                        loo.Add(color); loo.Add(size);

                        Product np = new Product(productItemName, "New!", shopify_vendorName, shopify_productType, false, lnv, loo, productTags);
                        np.images = productImages;

                        RootObject addedProduct = Fields.CurrentStore.Products.AddProduct(np);

                        if (addedProduct.product != null)
                        {
                            failureLog.Add("The product " + computerno + " has been added and had this id: " + addedProduct.product.id + ".");
                            addedProducts.Add(addedProduct.product);
                            pdal.AddProduct(addedProduct.product);
                        }
                        else
                        {
                            failureLog.Add("Failed to add product, \n" + addedProduct.errors.title);
                        }

                    }
                    else
                    {
                        //create list of selected colors
                        List<string> s = SelectedColors(CurrentOceanItem.Tables[1]);

                        foreach (string si in s)
                        {

                            //create images List
                            List<Image> productImages = new List<Image>();
                            List<string> filenames = new List<string>();
                            int imageposition = 1;
                            filenames = modelList.Where(x => x.computerno == computerno & x.colorid == si).OrderBy(z => z.filename).Select(y => y.filename).ToList();


                            foreach (string filename in filenames)
                            {
                                System.Drawing.Image img = System.Drawing.Image.FromFile(photosFolder + "\\" + filename);
                                byte[] arr;
                                using (MemoryStream ms = new MemoryStream())
                                {
                                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    arr = ms.ToArray();

                                    Image shm = new Image();
                                    shm.position = imageposition;
                                    shm.attachment = Convert.ToBase64String(arr);

                                    productImages.Add(shm);

                                    imageposition += 1;
                                }
                            }


                            List<Option> loo = new List<Option>();

                            ////Color
                            //Option color = new Option();
                            //color.name = "Color";
                            //color.values = new List<string>();

                            //Size
                            Option size = new Option();
                            size.name = "Size";
                            size.values = new List<string>();


                            // create variants
                            int position = 1;
                            List<Variant> lnv = new List<Variant>();
                            foreach (DataRow dr in CurrentOceanItem.Tables[1].Select("ColorID = '" + si + "'"))
                            {

                                Variant nv = new Variant(/*vdal.ColorNameAlternative(dr["ColorEName"].ToString()) + " / " +*/ dr["SizeName"].ToString(),
                                    shopify_price, computerno + "." + dr["ColorID"].ToString(), position, "1000", "deny",
                                    shopify_compareatprice, "FedEx", "shopify"/*, vdal.ColorNameAlternative(dr["ColorEName"].ToString())*/, dr["SizeName"].ToString(),
                                    null,null, true, false, dr["barcode"].ToString(), 0);
                                lnv.Add(nv);

                                ////fill list of color in this product.
                                //if (!color.values.Contains(vdal.ColorNameAlternative(dr["ColorEName"].ToString())))
                                //    color.values.Add(vdal.ColorNameAlternative(dr["ColorEName"].ToString()));

                                //fill list of sizes in this product.
                                if (!size.values.Contains(dr["SizeName"].ToString()))
                                    size.values.Add(dr["SizeName"].ToString());

                                position += 1;
                            }

                            //fill the list of options
                            /*loo.Add(color);*/ loo.Add(size);

                            Product np = new Product(productItemName, "New!", pdal.VendorNameAlternative(shopify_vendorName), pdal.VendorNameAlternative(shopify_productType), false, lnv, loo, productTags);
                            np.images = productImages;
                            RootObject addedProduct = Fields.CurrentStore.Products.AddProduct(np);

                            if (addedProduct.product != null)
                            {
                                failureLog.Add("The product " + computerno + " has been added and had this id: " + addedProduct.product.id + ".");
                                addedProducts.Add(addedProduct.product);
                                pdal.AddProduct(addedProduct.product);
                            }
                            else
                            {
                                failureLog.Add("Failed to add product, \n" + addedProduct.errors.title);
                            }
                        }

                    }

                    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
                    //\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

                }

            }

        }
        private void GetOceanItem(string ComputerNo, string ColorList)
        {

            CurrentOceanItem = Fields.OceanDB.ExecuteDataSet(@"SELECT  ComputerNo, ModelNo, MTName, MTEName, ItemName, ItemEName, 
	                            BrandName, BrandEName, DeptName, DeptEName, CountryName,
	                            CountryEName, FabricName, FabricEName, SeasonName, SeasonEName, 
	                            TypeName, TypeEName, ItemYear, Des, Whole, Special, Sale, 
	                            Clearance, EndUser FROM dbo.QMTI Where ComputerNo = '" + ComputerNo + @"'; 
                                SELECT Code,ComputerNo, SUM(Qty) Qty, BarCode, SizeName, ColorEName,ColorID, 
                                OrderIndex FROM dbo.QMTS WHERE ComputerNo = '" + ComputerNo + @"' And 
                                ColorID in (" + ColorList + @") GROUP BY Code , BarCode, SizeName, 
                                ColorEName,ComputerNo,ColorID, OrderIndex ORDER BY ColorID, OrderIndex ");
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
        public class ComputerNoDetails
        {
            public string filename { get; set; }

            public string computerno { get; set; }

            public string colorid { get; set; }

            public string sequence { get; set; }

        }

        private string generateTags(List<string> includeInTags)
        {
            string tags = "";
            if (includeInTags.Contains("Color"))
            {
                foreach (DataRow dr in CurrentOceanItem.Tables[1].Rows)
                {
                    if (!tags.Contains(vdal.ColorNameAlternative(dr["ColorEName"].ToString())))
                        tags += vdal.ColorNameAlternative(dr["ColorEName"].ToString()) + ",";
                }
            }

            if (includeInTags.Contains("Size"))
            {
                foreach (DataRow dr in CurrentOceanItem.Tables[1].Rows)
                {
                    if (!tags.Contains(dr["SizeName"].ToString()))
                        tags += dr["SizeName"].ToString() + ",";
                }

            }

            if (includeInTags.Contains("Dept"))
            {
                tags += pdal.DeptNameAlternative(CurrentOceanItem.Tables[0].Rows[0]["DeptEName"].ToString()) + ",";
            }

            if (includeInTags.Contains("Season"))
            {
                tags += pdal.SeasonNameAlternative(CurrentOceanItem.Tables[0].Rows[0]["SeasonEName"].ToString()) + ",";
            }

            if (includeInTags.Contains("Year"))
            {
                tags += CurrentOceanItem.Tables[0].Rows[0]["ItemYear"].ToString() + ",";
            }

            if (includeInTags.Contains("Brand"))
            {
                tags += pdal.VendorNameAlternative(CurrentOceanItem.Tables[0].Rows[0]["BrandEName"].ToString() + ",");
            }

            if (tags.Length > 0)
                return tags.Remove(tags.Length - 1, 1);
            else
                return "";

        }

        List<string> SelectedColors(DataTable Variants)
        {
            List<string> s = new List<string>();
            foreach (DataRow dr in Variants.Rows)
            {
                if (!s.Contains(dr["ColorID"].ToString()))
                    s.Add(dr["ColorID"].ToString());
            }
            return s;
        }


    }
}
