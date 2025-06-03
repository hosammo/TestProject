using Shopify.IO.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ShopifyHelper.IO.ODAL
{
    public class Products_DAL
    {
        public Products_DAL()
        {

        }

        public productStatus CheckProduct(Product product)
        {
            //check if product exist.
            //if not exit, add the product to the database.
            //if exist, then check if it is changed.
            //for deleted products from shopify, it will be handelled in the sync operation.
            return productStatus.unknown;
        }

        public productStatus AddProduct(Product product)
        {

            SqlCommand insertCMD = Fields.CachingDB.Connection.CreateCommand();

            string insertProductCmdTxt = @"ALTER TABLE dbo.Products NOCHECK CONSTRAINT FK_Products_Images;INSERT INTO Products 
                        (id,title,body_html,vendor,product_type,created_at,
                        handle,updated_at,published_at,template_suffix,published_scope,
                        tags,published,OceanComputerNo) 
                        VALUES 
                        (@id,@title,@body_html,@vendor,@product_type,@created_at,@handle,
                        @updated_at,@published_at,@template_suffix,@published_scope,@tags,
                        @published,@OceanComputerNo);
                        ALTER TABLE dbo.Products CHECK CONSTRAINT FK_Products_Images;";



            //insert product row.

            insertCMD.CommandText = insertProductCmdTxt;

            insertCMD.Parameters.Add("@id", SqlDbType.BigInt).Value = product.id;
            insertCMD.Parameters.Add("@title", SqlDbType.NVarChar).Value = product.title;

            if (product.body_html != null)

                insertCMD.Parameters.Add("@body_html", SqlDbType.NVarChar).Value = product.body_html;
            else
                insertCMD.Parameters.Add("@body_html", SqlDbType.NVarChar).Value = " ";



            insertCMD.Parameters.Add("@vendor", SqlDbType.NVarChar).Value = product.vendor;
            insertCMD.Parameters.Add("@product_type", SqlDbType.NVarChar).Value = product.product_type;
            insertCMD.Parameters.Add("@created_at", SqlDbType.DateTime).Value = Convert.ToDateTime(product.created_at);
            insertCMD.Parameters.Add("@handle", SqlDbType.NVarChar).Value = product.handle;
            insertCMD.Parameters.Add("@updated_at", SqlDbType.DateTime).Value = Convert.ToDateTime(product.updated_at);

            if (product.published_at != null)
                insertCMD.Parameters.Add("@published_at", SqlDbType.DateTime).Value = Convert.ToDateTime(product.published_at);
            else
                insertCMD.Parameters.Add("@published_at", SqlDbType.DateTime).Value = DBNull.Value;

            if (product.template_suffix != null)
                insertCMD.Parameters.Add("@template_suffix", SqlDbType.NVarChar).Value = product.template_suffix;
            else
                insertCMD.Parameters.Add("@template_suffix", SqlDbType.NVarChar).Value = DBNull.Value;

            insertCMD.Parameters.Add("@published_scope", SqlDbType.NVarChar).Value = product.published_scope;
            insertCMD.Parameters.Add("@tags", SqlDbType.NVarChar).Value = product.tags;

            if (product.image != null)
                insertCMD.Parameters.Add("@image", SqlDbType.NVarChar).Value = product.image.id;
            else
                insertCMD.Parameters.Add("@image", SqlDbType.NVarChar).Value = DBNull.Value;

            insertCMD.Parameters.Add("@published", SqlDbType.NVarChar).Value = product.published_scope;
            if(product.variants !=null && product.variants[0].sku != null)
                insertCMD.Parameters.Add("@OceanComputerNo", SqlDbType.NVarChar).Value = product.variants[0].sku;
            else
                insertCMD.Parameters.Add("@OceanComputerNo", SqlDbType.NVarChar).Value = DBNull.Value;

            try
            {
                int affectedRows = insertCMD.ExecuteNonQuery();
                if (affectedRows > 0)
                {

                    //insert images
                    Image_DAL idal = new Image_DAL();
                    foreach (Image i in product.images)
                    {
                        idal.AddImage(i);
                    }

                    //insert Variants
                    Variants_DAL vdal = new Variants_DAL();
                    foreach (Variant v in product.variants)
                    {
                        vdal.AddVariant(v);
                    }

                    //insert options
                    Options_DAL odal = new Options_DAL();
                    foreach (Option o in product.options)
                    {
                        odal.AddOption(o);
                    }



                    return productStatus.addCompleated;
                }

                else
                    return productStatus.unknown;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public productStatus UpdateProduct(Product product)
        {
            return productStatus.unknown;
        }


        public productStatus DeleteProduct(Product product)
        {
            string deleteCMDtxt = @"INSERT  INTO dbo.Products 
                        (id,title,body_html,vendor,product_type,created_at,
                        handle,updated_at,published_at,template_suffix,published_scope,
                        tags,image,published,OceanComputerNo) 
                        VALUES 
                        (@id,@title,@body_html,@vendor,@product_type,@created_at,@handle,
                        @updated_at,@published_at,@template_suffix,@published_scope,@tags,
                        @image,@published,@OceanComputerNo);";


            SqlCommand deleteCMD = Fields.CachingDB.Connection.CreateCommand();

            deleteCMD.CommandText = deleteCMDtxt;

            deleteCMD.Parameters.Add("@id", SqlDbType.BigInt).Value = "";

            try
            {
                int affectedRows = deleteCMD.ExecuteNonQuery();
                if (affectedRows > 0)
                    return productStatus.deleteCompleated;
                else
                    return productStatus.unknown;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public decimal GetProductPriceFromOcean(string OceanComputerNo, string CurrencyIsoCode)
        {
            SqlCommand cmd = Fields.OceanDB.Connection.CreateCommand();
            //if (Fields.OceanDB.Connection.State != System.Data.ConnectionState.Open)
            //    Fields.openConnection();

            cmd.CommandText = "SELECT TOP 1 CASE WHEN Sale = 0 THEN Price ELSE Sale END AS Price FROM shopify.VariantPricesWithCurrency where ComputerNo = '" + OceanComputerNo + "' and Currency = '" + CurrencyIsoCode + "'";

            var tmpqty = cmd.ExecuteScalar();

            try
            {
                if (tmpqty != null)
                    return Convert.ToDecimal(tmpqty);
                else
                    return -1;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public decimal GetProductComparedAtPriceFromOcean(string OceanComputerNo, string CurrencyIsoCode)
        {
            SqlCommand cmd = Fields.OceanDB.Connection.CreateCommand();
            //if (Fields.DbConnection.State != System.Data.ConnectionState.Open)
            //    Fields.openConnection();

            //if (Fields.cm == null)
            //    Fields.cm = Fields.DbConnection.CreateCommand();

            cmd.CommandText = "SELECT TOP 1 CASE WHEN Sale <> 0 THEN Price ELSE 0 END AS compare_at_price  FROM shopify.VariantPricesWithCurrency where ComputerNo = '" + OceanComputerNo + "' and Currency = '" + CurrencyIsoCode + "'";

            var tmpqty = cmd.ExecuteScalar();

            try
            {
                return Convert.ToDecimal(tmpqty);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public long GetProductIDByComputerNo(string OceanComputerNo)
        {
            DataTable d = Fields.CachingDB.ExecuteDatatable("Select id from products where OceanComputerNo ='" + OceanComputerNo + "'");

            if (d.Rows.Count > 0)
                return Convert.ToInt64(d.Rows[0]["id"]);
            else
                return 0;

        }

        public void SyncLocalDataFromShopify(List<Product> ShopifyProducts)
        {

        }

        public string VendorNameAlternative(string VendorName)
        {
            SqlCommand cmd = Fields.CachingDB.Connection.CreateCommand();

            cmd.CommandText = "SELECT NewBrandName FROM dbo.VendorDic WHERE OldBrandName = '" + VendorName + "'";

            object newVendorName = cmd.ExecuteScalar();

            try
            {
                if (newVendorName == null)
                    return VendorName;
                else
                    return (string)newVendorName;
            }
            catch (Exception)
            {
                return VendorName;
            }

        }

        public string ItemNameAlternative(string ItemName)
        {
            SqlCommand cmd = Fields.CachingDB.Connection.CreateCommand();

            cmd.CommandText = "SELECT NewItemName FROM dbo.ProductTypeDic WHERE OldItemName = '" + ItemName + "'";

            object newItemName = cmd.ExecuteScalar();

            try
            {
                if (newItemName == null)
                    return ItemName;
                else
                    return (string)newItemName;
            }
            catch (Exception)
            {
                return ItemName;
            }
        }

        public enum productStatus
        {
            notexist,
            alreadyexist,
            deleteCompleated,
            updateCompleated,
            addCompleated,
            unknown
        }

        public string DeptNameAlternative(string DeptName)
        {
            SqlCommand cmd = Fields.CachingDB.Connection.CreateCommand();

            cmd.CommandText = "SELECT NewDeptName FROM dbo.DeptDic WHERE OldDeptName = '" + DeptName + "'";

            object newDeptName = cmd.ExecuteScalar();

            try
            {
                if (newDeptName == null)
                    return DeptName;
                else
                    return (string)newDeptName;
            }
            catch (Exception)
            {
                return DeptName;
            }
        }

        public string SeasonNameAlternative(string SeasonName)
        {
            SqlCommand cmd = Fields.CachingDB.Connection.CreateCommand();

            cmd.CommandText = "SELECT NewSeasonName FROM dbo.SeasonDic WHERE OldSeasonName = '" + SeasonName + "'";

            object newSeasonName = cmd.ExecuteScalar();

            try
            {
                if (newSeasonName == null)
                    return SeasonName;
                else
                    return (string)newSeasonName;
            }
            catch (Exception)
            {
                return SeasonName;
            }
        }
    }
}
