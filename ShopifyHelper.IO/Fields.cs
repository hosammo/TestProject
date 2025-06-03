using Shopify.IO;
using Shopify.IO.Operations;
using Shopify.IO.Types;
using ShopifyHelper.IO.ODAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ShopifyHelper.IO
{
    public class Fields
    {
        //APIAccess shopifyAccess = 
        //public static StoreManager montaniashop = new StoreManager(new APIAccess("7eda9690da448468cb9217736c5c9bf3", "189184881f1261b2d16ccf95564b5311", "4ded2223907d68d561d7ae1bae2b3525", "montania-fashion.myshopify.com"));

        //connection to sql
        public static DBContext OceanDB;
        public static DBContext CachingDB;

        //APIAccess shopifyAccess = 
        public static StoreManager CurrentStore;
        public static void GetNewProductsFromShopify()
        {
            SqlCommand cmd = Fields.CachingDB.Connection.CreateCommand();

            Products_DAL pdal = new Products_DAL();

            cmd.CommandText = "Select ISNULL(Max(ID),0) from Products";

            long product_id = (Int64)cmd.ExecuteScalar(); //5328679750;

            Products po = new Products(CurrentStore.CurrentSroreAPIAccess);
            List<Product> lop = po.GetListWithEndPoint(new ProductEndPoint { EndPointType = ProductEndPointTypes.since_id, EndPointValue = product_id.ToString() });

            foreach (Product p in lop)
            {
                pdal.AddProduct(p);
            }
        }

        public static void SyncLocations()
        {

            SqlCommand cmd = Fields.CachingDB.Connection.CreateCommand();

            Locations_DAL ldal = new Locations_DAL();

            DataTable lls = Fields.CachingDB.ExecuteDatatable("Select ISNULL(ID,0) id from Locations");

            Locations lo = new Locations(CurrentStore.CurrentSroreAPIAccess);
            List<Location> lol = lo.GetList();

            if (lls.Rows.Count == 0)
            {
                //fill for first time


                foreach (Location l in lol)
                {
                    ldal.AddLocation(l);
                }

            } 
            else if (lls.Rows.Count < lol.Count)
            {
                //sync
                // since the locations are limited and not going to be a big number, more oger there will be a special maping with ocen brancehes
                foreach (Location l in lol)
                {
                    //check each location and add it if not exist,
                    if (ldal.CheckLocation(l) == Locations_DAL.LocationsStatus.notexist)
                    {
                        ldal.AddLocation(l);
                    }

                }

            }
            else if (lls.Rows.Count > lol.Count)
            {
                foreach (DataRow dr in lls.Rows)
                {

                    Location foundLocation = lol.FirstOrDefault(loc => loc.id == Convert.ToInt64(dr["id"]));

                    if (foundLocation == null)
                    {
                        ldal.DeleteLocation(Convert.ToInt64(dr["id"]));
                    }    
                }
            }


            //update status from shopify

            foreach (Location l in lol)
            {
                //check each location and add it if not exist,
                ldal.UpdateLocationStatus(l);

            }
        }

        public static void ClearCachingDB()
        {

            SqlCommand cmd = Fields.CachingDB.Connection.CreateCommand();

            Products_DAL pdal = new Products_DAL();

            cmd.CommandText = @"DELETE FROM dbo.OptionsValues;
                                DELETE FROM dbo.Options;
                                DELETE FROM dbo.Variants;
                                DELETE FROM dbo.Images;
                                DELETE FROM dbo.RecommendedProducts;
                                DELETE FROM dbo.Products";

            cmd.ExecuteNonQuery();


        }

        #region oldcode
        //public static int GetBarcodeQty(string barcode, string sourceDatabase = "")
        //{
        //    if (General.cn.State != System.Data.ConnectionState.Open)
        //        openConnection();

        //    if (General.cm == null)
        //        General.cm = General.cn.CreateCommand();

        //    General.cm.CommandText = "Select Qty from " + sourceDatabase + ".dbo.QtyByVariant where barcode = '" + barcode + "'";

        //    var tmpqty = General.cm.ExecuteScalar();

        //    try
        //    {
        //        return (int)tmpqty;
        //    }
        //    catch (Exception)
        //    {
        //        return -1;
        //    }
        //}

        //public static decimal GetVariantPrice(string barcode, string sourceDatabase = "")
        //{
        //    if (General.cn.State != System.Data.ConnectionState.Open)
        //        openConnection();

        //    if (General.cm == null)
        //        General.cm = General.cn.CreateCommand();

        //    General.cm.CommandText = "SELECT  CASE WHEN Sale = 0 THEN Price ELSE Sale END AS Price FROM " + sourceDatabase + ".shopify.VariantPrices where barcode = '" + barcode + "'";

        //    var tmpqty = General.cm.ExecuteScalar();

        //    try
        //    {
        //        if (tmpqty != null)
        //            return Convert.ToDecimal(tmpqty);
        //        else
        //            return -1;
        //    }
        //    catch (Exception)
        //    {
        //        return -1;
        //    }
        //}

        //public static decimal GetVariantComparedAtPrice(string barcode, string sourceDatabase = "")
        //{
        //    if (General.cn.State != System.Data.ConnectionState.Open)
        //        openConnection();

        //    if (General.cm == null)
        //        General.cm = General.cn.CreateCommand();

        //    General.cm.CommandText = "SELECT CASE WHEN Sale <> 0 THEN Price ELSE 0 END AS compare_at_price  FROM " + sourceDatabase + ".shopify.VariantPrices where barcode = '" + barcode + "'";

        //    var tmpqty = General.cm.ExecuteScalar();

        //    try
        //    {
        //        return Convert.ToDecimal(tmpqty);
        //    }
        //    catch (Exception)
        //    {
        //        return -1;
        //    }
        //}

        //public static decimal GetProductPrice(string ComputerNo, string sourceDatabase = "")
        //{
        //    if (General.cn.State != System.Data.ConnectionState.Open)
        //        openConnection();

        //    if (General.cm == null)
        //        General.cm = General.cn.CreateCommand();

        //    General.cm.CommandText = "SELECT TOP 1 CASE WHEN Sale = 0 THEN Price ELSE Sale END AS Price FROM " + sourceDatabase + ".shopify.VariantPrices where ComputerNo = '" + ComputerNo + "'";

        //    var tmpqty = General.cm.ExecuteScalar();

        //    try
        //    {
        //        if (tmpqty != null)
        //            return Convert.ToDecimal(tmpqty);
        //        else
        //            return -1;
        //    }
        //    catch (Exception)
        //    {
        //        return -1;
        //    }
        //}

        //public static Decimal GetProductComparedAtPrice(string ComputerNo, string sourceDatabase = "")
        //{
        //    if (General.cn.State != System.Data.ConnectionState.Open)
        //        openConnection();

        //    if (General.cm == null)
        //        General.cm = General.cn.CreateCommand();

        //    General.cm.CommandText = "SELECT TOP 1 CASE WHEN Sale <> 0 THEN Price ELSE 0 END AS compare_at_price  FROM " + sourceDatabase + ".shopify.VariantPrices where ComputerNo = '" + ComputerNo + "'";

        //    var tmpqty = General.cm.ExecuteScalar();

        //    try
        //    {
        //        return Convert.ToDecimal(tmpqty);
        //    }
        //    catch (Exception)
        //    {
        //        return -1;
        //    }
        //}

        //public static DataTable ExecuteDatatable(string sqlCommand)
        //{
        //    if (General.cn.State != System.Data.ConnectionState.Open)
        //        openConnection();

        //    if (General.cm == null)
        //        General.cm = General.cn.CreateCommand();

        //    SqlCommand OceanCM = General.cn.CreateCommand();
        //    DataTable dt = new DataTable();
        //    using (OceanCM)
        //    {
        //        OceanCM.CommandText = sqlCommand;

        //        SqlDataAdapter sda = new SqlDataAdapter(OceanCM);


        //        sda.Fill(dt);
        //    }
        //    return dt;

        //}

        //public static DataSet ExecuteDataSet(string sqlCommand)
        //{
        //    if (General.cn.State != System.Data.ConnectionState.Open)
        //        openConnection();

        //    if (General.cm == null)
        //        General.cm = General.cn.CreateCommand();

        //    SqlCommand OceanCM = General.cn.CreateCommand();
        //    DataSet ds = new DataSet();
        //    using (OceanCM)
        //    {

        //        OceanCM.CommandText = sqlCommand;

        //        SqlDataAdapter sda = new SqlDataAdapter(OceanCM);

        //        sda.Fill(ds);

        //    }
        //    return ds;

        //} 
        #endregion
    }
}
