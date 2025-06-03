using Shopify.IO.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ShopifyHelper.IO.ODAL
{
    public class Variants_DAL
    {
        public Variants_DAL()
        {

        }

        public variantStatus CheckVariant(Variant variant)
        {
            //check if variant exist.
            //if not exit, add the variant to the database.
            //if exist, then check if it is changed.
            //for deleted variants from shopify, it will be handelled in the sync operation.
            return variantStatus.unknown;
        }

        public variantStatus AddVariant(Variant variant)
        {

            SqlCommand insertCMD = Fields.CachingDB.Connection.CreateCommand();

            string insertVariantCmdTxt = @"INSERT  INTO dbo.Variants ( id ,product_id ,title ,price ,sku ,position ,grams ,inventory_policy ,compare_at_price ,
                        fulfillment_service ,inventory_management ,option1 ,option2 ,option3 ,created_at ,updated_at ,requires_shipping ,
                        taxable ,barcode ,inventory_quantity ,old_inventory_quantity ,image_id ,weight ,weight_unit,inventory_item_id)
                        VALUES  ( @id ,@product_id ,@title ,@price ,@sku ,@position ,@grams ,@inventory_policy ,@compare_at_price ,@fulfillment_service ,
                        @inventory_management ,@option1 ,@option2 ,@option3 ,@created_at ,@updated_at ,@requires_shipping ,@taxable ,@barcode ,
                        @inventory_quantity ,@old_inventory_quantity ,@image_id ,@weight ,@weight_unit,@inventory_item_id);";


            //insert variant row.

            insertCMD.CommandText = insertVariantCmdTxt;

            insertCMD.Parameters.Add("@id", SqlDbType.BigInt).Value = variant.id;
            insertCMD.Parameters.Add("@product_id", SqlDbType.BigInt).Value = variant.product_id;
            if (variant.title != null)
                insertCMD.Parameters.Add("@title", SqlDbType.NVarChar).Value = variant.title;
            else
                insertCMD.Parameters.Add("@title", SqlDbType.NVarChar).Value = DBNull.Value;

            insertCMD.Parameters.Add("@price", SqlDbType.NVarChar).Value = variant.price;
            insertCMD.Parameters.Add("@sku", SqlDbType.NVarChar).Value = variant.sku;
            insertCMD.Parameters.Add("@position", SqlDbType.Int).Value = variant.position;
            insertCMD.Parameters.Add("@grams", SqlDbType.NVarChar).Value = variant.grams;
            insertCMD.Parameters.Add("@inventory_policy", SqlDbType.NVarChar).Value = variant.inventory_policy;

            if (variant.compare_at_price != null)
                insertCMD.Parameters.Add("@compare_at_price", SqlDbType.NVarChar).Value = variant.compare_at_price;
            else
                insertCMD.Parameters.Add("@compare_at_price", SqlDbType.NVarChar).Value = DBNull.Value;

            if (variant.fulfillment_service != null)
                insertCMD.Parameters.Add("@fulfillment_service", SqlDbType.NVarChar).Value = variant.fulfillment_service;
            else
                insertCMD.Parameters.Add("@fulfillment_service", SqlDbType.NVarChar).Value = DBNull.Value;

            if (variant.inventory_management != null)
                insertCMD.Parameters.Add("@inventory_management", SqlDbType.NVarChar).Value = variant.inventory_management;
            else
                insertCMD.Parameters.Add("@inventory_management", SqlDbType.NVarChar).Value = DBNull.Value;

            if (variant.option1 != null)
                insertCMD.Parameters.Add("@option1", SqlDbType.NVarChar).Value = variant.option1;
            else
                insertCMD.Parameters.Add("@option1", SqlDbType.NVarChar).Value = DBNull.Value;

            if (variant.option2 != null)
                insertCMD.Parameters.Add("@option2", SqlDbType.NVarChar).Value = variant.option2;
            else
                insertCMD.Parameters.Add("@option2", SqlDbType.NVarChar).Value = DBNull.Value;

            if (variant.option3 != null)
                insertCMD.Parameters.Add("@option3", SqlDbType.NVarChar).Value = variant.option3;
            else
                insertCMD.Parameters.Add("@option3", SqlDbType.NVarChar).Value = DBNull.Value;

            insertCMD.Parameters.Add("@created_at", SqlDbType.DateTime).Value = Convert.ToDateTime(variant.created_at);
            insertCMD.Parameters.Add("@updated_at", SqlDbType.DateTime).Value = Convert.ToDateTime(variant.updated_at);
            insertCMD.Parameters.Add("@requires_shipping", SqlDbType.Bit).Value = variant.requires_shipping;
            insertCMD.Parameters.Add("@taxable", SqlDbType.Bit).Value = variant.taxable;

            if (variant.barcode != null)
                insertCMD.Parameters.Add("@barcode", SqlDbType.NVarChar).Value = variant.barcode;
            else
                insertCMD.Parameters.Add("@barcode", SqlDbType.NVarChar).Value = DBNull.Value;

            insertCMD.Parameters.Add("@inventory_quantity", SqlDbType.Int).Value = variant.inventory_quantity;
            insertCMD.Parameters.Add("@old_inventory_quantity", SqlDbType.NVarChar).Value = variant.old_inventory_quantity;

            if (variant.image_id != null)
                insertCMD.Parameters.Add("@image_id", SqlDbType.BigInt).Value = variant.image_id;
            else
                insertCMD.Parameters.Add("@image_id", SqlDbType.BigInt).Value = DBNull.Value;

            insertCMD.Parameters.Add("@weight", SqlDbType.Decimal).Value = variant.weight;
            insertCMD.Parameters.Add("@weight_unit", SqlDbType.NVarChar).Value = variant.weight_unit;
            insertCMD.Parameters.Add("@inventory_item_id",SqlDbType.BigInt).Value = variant.inventory_item_id;

            try
            {
                int affectedRows = insertCMD.ExecuteNonQuery();
                if (affectedRows > 0)
                    return variantStatus.addCompleated;
                else
                    return variantStatus.unknown;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public variantStatus UpdateVariant(Variant variant)
        {
            return variantStatus.unknown;
        }

        public variantStatus DeleteVariant(Variant variant)
        {
            string deleteCMDtxt = @"";


            SqlCommand deleteCMD = Fields.CachingDB.Connection.CreateCommand();

            deleteCMD.CommandText = deleteCMDtxt;

            deleteCMD.Parameters.Add("@id", SqlDbType.BigInt).Value = "";

            try
            {
                int affectedRows = deleteCMD.ExecuteNonQuery();
                if (affectedRows > 0)
                    return variantStatus.deleteCompleated;
                else
                    return variantStatus.unknown;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public int GetVariantQtyFromOcean(string OceanBarcode, LocationConfig lc)
        {
            using (SqlCommand cmd = Fields.OceanDB.Connection.CreateCommand()) 
            //if (Fields.DbConnection.State != System.Data.ConnectionState.Open)
            //    Fields.openConnection();

            //if (Fields.cm == null)
            //    Fields.cm = Fields.DbConnection.CreateCommand();
            {
                string b = "";
                foreach (string s in lc.LocalDBBranch)
                    b += s + ",";

                b = b.Remove(b.Length - 1, 1);

                cmd.CommandText = "Select SUM(ISNULL(Qty, 0)) AS Qty from QMTS where barcode = '" + OceanBarcode + "' and branchID in (" + b + ")";

                var tmpqty = cmd.ExecuteScalar();

                try
                {
                    if (tmpqty == null)
                        return -1;
                    else
                    {
                        int maskedStock = Convert.ToInt32(ApplyMaskingRules((int)tmpqty, lc.MaskingRules));
                        return maskedStock;
                    }
                }
                catch (Exception)
                {
                    return -1;
                }
            }
        }

        private int? LegacyApplyMaskingRules(int totalStock, List<StockMaskingRule> rules)
        {
            foreach (var rule in rules)
            {
                // If RangeTo is null, treat it as a single-value range: [RangeFrom, RangeFrom]
                int effectiveTo = rule.RangeTo ?? rule.RangeFrom;

                // Check if totalStock is within [RangeFrom, effectiveTo].
                if (totalStock >= rule.RangeFrom && totalStock <= effectiveTo)
                {
                    // If the rule's MaskedStock is null, that might mean "Hide" or "Return 0" 
                    // or you might interpret it differently. Here, let's interpret null => zero.
                    return rule.MaskedStock ?? 0;
                }
            }

            // If no rule matched, return the original total (no masking).
            return totalStock;
        }

        private int? ApplyMaskingRules(int totalStock, List<StockMaskingRule> rules)
        {
            foreach (var rule in rules)
            {
                // If RangeTo is null, this is an open-ended range (>= RangeFrom)
                if (rule.RangeTo == null)
                {
                    if (totalStock >= rule.RangeFrom)
                    {
                        return rule.MaskedStock ?? 0;
                    }
                }
                else
                {
                    if (totalStock >= rule.RangeFrom && totalStock <= rule.RangeTo.Value)
                    {
                        return rule.MaskedStock ?? 0;
                    }
                }
            }

            // If no rule matched, return the original value
            return totalStock;
        }



        public decimal GetVariantPriceFromOcean(Variant variant)
        {
            SqlCommand cmd = Fields.OceanDB.Connection.CreateCommand();
            //if (Fields.DbConnection.State != System.Data.ConnectionState.Open)
            //    Fields.openConnection();

            //if (Fields.cm == null)
            //    Fields.cm = Fields.DbConnection.CreateCommand();

            cmd.CommandText = "SELECT  CASE WHEN Sale = 0 THEN Price ELSE Sale END AS Price FROM shopify.VariantPrices where barcode = '" + variant.barcode + "'";

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

        public decimal GetVariantComparedAtPriceFromOcean(Variant variant)
        {
            SqlCommand cmd = Fields.OceanDB.Connection.CreateCommand();

            cmd.CommandText = "SELECT CASE WHEN Sale <> 0 THEN Price ELSE 0 END AS compare_at_price  FROM shopify.VariantPrices where barcode = '" + variant.barcode + "'";

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

        public string ColorNameAlternative(string ColorName)
        {
            SqlCommand cmd = Fields.CachingDB.Connection.CreateCommand();

            cmd.CommandText = "SELECT NewColorName FROM dbo.ColorDic WHERE OldColorName = '" + ColorName + "'";

            object newColorName = cmd.ExecuteScalar();

            try
            {
                if (newColorName == null)
                    return ColorName;
                else
                    return (string)newColorName;
            }
            catch (Exception)
            {
                return ColorName;
            }

        }


        public enum variantStatus
        {
            notexist,
            alreadyexist,
            deleteCompleated,
            updateCompleated,
            addCompleated,
            unknown
        }

    }
}
