using Shopify.IO.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ShopifyHelper.IO.ODAL
{
    public class Locations_DAL
    {
        public Locations_DAL()
        {

        }

        public LocationsStatus CheckLocation(Location location)
        {
            //check if product exist.
            //if not exit, add the product to the database.
            //if exist, then check if it is changed.
            //for deleted Locations from shopify, it will be handelled in the sync operation.
            string selectCMDtxt = @"select isnull(id, 0) from locations where id = @id";

            SqlCommand selectCMD = Fields.CachingDB.Connection.CreateCommand();

            selectCMD.CommandText = selectCMDtxt;

            selectCMD.Parameters.Add("@id", SqlDbType.BigInt).Value = location.id;

            try
            {
                long affectedRows = Convert.ToInt64(selectCMD.ExecuteScalar());
                if (affectedRows > 0)
                    return LocationsStatus.alreadyexist;
                else
                    return LocationsStatus.notexist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public LocationsStatus AddLocation(Location location)
        {

            SqlCommand insertCMD = Fields.CachingDB.Connection.CreateCommand();

            string insertLocationCmdTxt = @"INSERT INTO Locations
(id,name,address1,address2,city,zip,province,country,phone,created_at,
updated_at,country_code,country_name,province_code,legacy,active,
admin_graphql_api_id,localized_country_name,localized_province_name) 
VALUES
(@id,@name,@address1,@address2,@city,@zip,@province,@country,@phone,@created_at,
@updated_at,@country_code,@country_name,@province_code,@legacy,@active,
@admin_graphql_api_id,@localized_country_name,@localized_province_name);";



            //insert product row.

            //,@,@,@,@,@zip,@,@,@,@created_at,@updated_at,@country_code,@country_name,@province_code,@legacy,@active,@admin_graphql_api_id,@localized_country_name,@localized_province_name

            insertCMD.CommandText = insertLocationCmdTxt;

            insertCMD.Parameters.Add("@id", SqlDbType.BigInt).Value = location.id;
            insertCMD.Parameters.Add("@name", SqlDbType.NVarChar, 255).Value = location.name;

            if (location.address1 != null)
            {
                insertCMD.Parameters.Add("@address1", SqlDbType.NVarChar, 255).Value = location.address1;
            }
            else
            {
                insertCMD.Parameters.Add("@address1", SqlDbType.NVarChar, 255).Value = "not set";
            }

            insertCMD.Parameters.Add("@address2", SqlDbType.NVarChar, 255).Value = location.address2 ?? (object)DBNull.Value;

            if (location.city !=null)
            {
                insertCMD.Parameters.Add("@city", SqlDbType.NVarChar, 100).Value = location.city;
            }
            else
            {
                insertCMD.Parameters.Add("@city", SqlDbType.NVarChar, 100).Value = "not set";
            }

            if (location.zip != null)
            {
                insertCMD.Parameters.Add("@zip", SqlDbType.NVarChar, 20).Value = location.zip;
            }
            else
            {
                insertCMD.Parameters.Add("@zip", SqlDbType.NVarChar, 20).Value = "not set";
            }

            if (location.province != null)
            {
                insertCMD.Parameters.Add("@province", SqlDbType.NVarChar, 100).Value = location.province;
            }
            else
            {
                insertCMD.Parameters.Add("@province", SqlDbType.NVarChar, 100).Value = "not set";
            }
            insertCMD.Parameters.Add("@country", SqlDbType.NVarChar, 10).Value = location.country;
            insertCMD.Parameters.Add("@phone", SqlDbType.NVarChar, 50).Value = location.phone ?? (object)DBNull.Value;
            insertCMD.Parameters.Add("@created_at", SqlDbType.DateTimeOffset).Value = location.created_at;
            insertCMD.Parameters.Add("@updated_at", SqlDbType.DateTimeOffset).Value = location.updated_at;
            insertCMD.Parameters.Add("@country_code", SqlDbType.NVarChar, 10).Value = location.country_code;
            insertCMD.Parameters.Add("@country_name", SqlDbType.NVarChar, 100).Value = location.country_name;

            if (location.province_code != null)
            {
                insertCMD.Parameters.Add("@province_code", SqlDbType.NVarChar, 10).Value = location.province_code;
            }
            else
            {
                insertCMD.Parameters.Add("@province_code", SqlDbType.NVarChar, 10).Value = "not set";
            }

            insertCMD.Parameters.Add("@legacy", SqlDbType.Bit).Value = location.legacy;
            insertCMD.Parameters.Add("@active", SqlDbType.Bit).Value = location.active;
            insertCMD.Parameters.Add("@admin_graphql_api_id", SqlDbType.NVarChar, 255).Value = location.admin_graphql_api_id;

            if (location.localized_country_name != null)
            {
                insertCMD.Parameters.Add("@localized_country_name", SqlDbType.NVarChar, 100).Value = location.localized_country_name;
            }
            else
            {
                insertCMD.Parameters.Add("@localized_country_name", SqlDbType.NVarChar, 100).Value = "not set";
            }

            if (location.localized_province_name != null)
            {
                insertCMD.Parameters.Add("@localized_province_name", SqlDbType.NVarChar, 100).Value = location.localized_province_name;
            }
            else
            {
                insertCMD.Parameters.Add("@localized_province_name", SqlDbType.NVarChar, 100).Value = "not set";
            }

            try
            {
                long affectedRows = Convert.ToInt64(insertCMD.ExecuteScalar());
                if (affectedRows > 0)
                {
                    return LocationsStatus.addCompleated;
                }

                else
                    return LocationsStatus.unknown;
            }
            catch ( Exception e)
            {
                throw;
            }
        }

        public LocationsStatus UpdateLocation(Location location)
        {
            SqlCommand UpdateCMD = Fields.CachingDB.Connection.CreateCommand();

            string UpdateLocationCmdTxt = @"UPDATE dbo.Locations
SET
    name = @name,
    address1 = @address1,
    address2 = @address2,
    city = @city,
    zip = @zip,
    province = @province,
    country = @country,
    phone = @phone,
    created_at = @created_at,
    updated_at = @updated_at,
    country_code = @country_code,
    country_name = @country_name,
    province_code = @province_code,
    legacy = @legacy,
    active = @active,
    admin_graphql_api_id = @admin_graphql_api_id,
    localized_country_name = @localized_country_name,
    localized_province_name = @localized_province_name
WHERE id = @id;";

            UpdateCMD.CommandText = UpdateLocationCmdTxt;

            UpdateCMD.Parameters.Add("@id", SqlDbType.BigInt).Value = location.id;
            UpdateCMD.Parameters.Add("@name", SqlDbType.NVarChar, 255).Value = location.name;

            if (location.address1 != null)
            {
                UpdateCMD.Parameters.Add("@address1", SqlDbType.NVarChar, 255).Value = location.address1;
            }
            else
            {
                UpdateCMD.Parameters.Add("@address1", SqlDbType.NVarChar, 255).Value = "not set";
            }

            UpdateCMD.Parameters.Add("@address2", SqlDbType.NVarChar, 255).Value = location.address2 ?? (object)DBNull.Value;

            if (location.city != null)
            {
                UpdateCMD.Parameters.Add("@city", SqlDbType.NVarChar, 100).Value = location.city;
            }
            else
            {
                UpdateCMD.Parameters.Add("@city", SqlDbType.NVarChar, 100).Value = "not set";
            }

            if (location.zip != null)
            {
                UpdateCMD.Parameters.Add("@zip", SqlDbType.NVarChar, 20).Value = location.zip;
            }
            else
            {
                UpdateCMD.Parameters.Add("@zip", SqlDbType.NVarChar, 20).Value = "not set";
            }

            if (location.province != null)
            {
                UpdateCMD.Parameters.Add("@province", SqlDbType.NVarChar, 100).Value = location.province;
            }
            else
            {
                UpdateCMD.Parameters.Add("@province", SqlDbType.NVarChar, 100).Value = "not set";
            }
            UpdateCMD.Parameters.Add("@country", SqlDbType.NVarChar, 10).Value = location.country;
            UpdateCMD.Parameters.Add("@phone", SqlDbType.NVarChar, 50).Value = location.phone ?? (object)DBNull.Value;
            UpdateCMD.Parameters.Add("@created_at", SqlDbType.DateTimeOffset).Value = location.created_at;
            UpdateCMD.Parameters.Add("@updated_at", SqlDbType.DateTimeOffset).Value = location.updated_at;
            UpdateCMD.Parameters.Add("@country_code", SqlDbType.NVarChar, 10).Value = location.country_code;
            UpdateCMD.Parameters.Add("@country_name", SqlDbType.NVarChar, 100).Value = location.country_name;

            if (location.province_code != null)
            {
                UpdateCMD.Parameters.Add("@province_code", SqlDbType.NVarChar, 10).Value = location.province_code;
            }
            else
            {
                UpdateCMD.Parameters.Add("@province_code", SqlDbType.NVarChar, 10).Value = "not set";
            }

            UpdateCMD.Parameters.Add("@legacy", SqlDbType.Bit).Value = location.legacy;
            UpdateCMD.Parameters.Add("@active", SqlDbType.Bit).Value = location.active;
            UpdateCMD.Parameters.Add("@admin_graphql_api_id", SqlDbType.NVarChar, 255).Value = location.admin_graphql_api_id;

            if (location.localized_country_name != null)
            {
                UpdateCMD.Parameters.Add("@localized_country_name", SqlDbType.NVarChar, 100).Value = location.localized_country_name;
            }
            else
            {
                UpdateCMD.Parameters.Add("@localized_country_name", SqlDbType.NVarChar, 100).Value = "not set";
            }

            if (location.localized_province_name != null)
            {
                UpdateCMD.Parameters.Add("@localized_province_name", SqlDbType.NVarChar, 100).Value = location.localized_province_name;
            }
            else
            {
                UpdateCMD.Parameters.Add("@localized_province_name", SqlDbType.NVarChar, 100).Value = "not set";
            }

            try
            {
                int affectedRows = Convert.ToInt32(UpdateCMD.ExecuteNonQuery());
                if (affectedRows > 0)
                {
                    return LocationsStatus.updateCompleated;
                }

                else
                    return LocationsStatus.unknown;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public LocationsStatus UpdateLocationStatus(Location location)
        {
            SqlCommand UpdateCMD = Fields.CachingDB.Connection.CreateCommand();

            string UpdateLocationCmdTxt = @"UPDATE dbo.Locations
SET
    created_at = @created_at,
    updated_at = @updated_at,
    active = @active
WHERE id = @id;";

            UpdateCMD.CommandText = UpdateLocationCmdTxt;

            UpdateCMD.Parameters.Add("@id", SqlDbType.BigInt).Value = location.id;
            UpdateCMD.Parameters.Add("@created_at", SqlDbType.DateTimeOffset).Value = location.created_at;
            UpdateCMD.Parameters.Add("@updated_at", SqlDbType.DateTimeOffset).Value = location.updated_at;

            UpdateCMD.Parameters.Add("@active", SqlDbType.Bit).Value = location.active;


            try
            {
                int affectedRows = Convert.ToInt32(UpdateCMD.ExecuteNonQuery());
                if (affectedRows > 0)
                {
                    return LocationsStatus.updateCompleated;
                }

                else
                    return LocationsStatus.unknown;
            }
            catch (Exception)
            {
                throw;
            }

        }



        public LocationsStatus DeleteLocation(Location location)
        {

            string deleteCMDtxt = @"delete from locations where id = @id";

            SqlCommand deleteCMD = Fields.CachingDB.Connection.CreateCommand();

            deleteCMD.CommandText = deleteCMDtxt;

            deleteCMD.Parameters.Add("@id", SqlDbType.BigInt).Value = location.id;

            try
            {
                int affectedRows = deleteCMD.ExecuteNonQuery();
                if (affectedRows > 0)
                    return LocationsStatus.deleteCompleated;
                else
                    return LocationsStatus.unknown;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public LocationsStatus DeleteLocation(long location_id)
        {

            string deleteCMDtxt = @"delete from locations where id = @id";

            SqlCommand deleteCMD = Fields.CachingDB.Connection.CreateCommand();

            deleteCMD.CommandText = deleteCMDtxt;

            deleteCMD.Parameters.Add("@id", SqlDbType.BigInt).Value = location_id;

            try
            {
                int affectedRows = deleteCMD.ExecuteNonQuery();
                if (affectedRows > 0)
                    return LocationsStatus.deleteCompleated;
                else
                    return LocationsStatus.unknown;
            }
            catch (Exception)
            {
                throw;
            }

        }


        public LocationsStatus UpdateLinkInfo(string OceanBranchIds, Int64 id)
        {
            SqlCommand UpdateCMD = Fields.CachingDB.Connection.CreateCommand();

            string UpdateLocationCmdTxt = "UPDATE dbo.Locations SET ocean_stock_location_ids = @localDBBranch WHERE id = @id";

            UpdateCMD.CommandText = UpdateLocationCmdTxt;

            if (OceanBranchIds.Trim().Length > 0)
                UpdateCMD.Parameters.AddWithValue("@localDBBranch", OceanBranchIds);
            else
                UpdateCMD.Parameters.AddWithValue("@localDBBranch", DBNull.Value);

            UpdateCMD.Parameters.AddWithValue("@id", id);

            try
            {
                int affectedRows = Convert.ToInt32(UpdateCMD.ExecuteNonQuery());
                if (affectedRows > 0)
                {
                    return LocationsStatus.updateCompleated;
                }

                else
                    return LocationsStatus.unknown;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateMaskingRulesForLocation(long locationId, List<StockMaskingRule> rules)
        {
            try
            {
                using (SqlTransaction tran = Fields.CachingDB.Connection.BeginTransaction())
                {
                    // First delete any existing rules for this location.
                   string deleteQuery = "DELETE FROM dbo.StockMaskingRules WHERE ShopifyLocationId = @locId";
                    using (SqlCommand cmdDelete = Fields.CachingDB.Connection.CreateCommand())
                    {
                        cmdDelete.Transaction = tran;
                        cmdDelete.CommandText = deleteQuery;
                        cmdDelete.Parameters.AddWithValue("@locId", locationId);
                        cmdDelete.ExecuteNonQuery();
                    }
                    // Then insert the new rules.
                    foreach (var rule in rules)
                    {
                        string insertQuery = @"INSERT INTO dbo.StockMaskingRules 
                                           (ShopifyLocationId, RangeFrom, RangeTo, MaskedStock)
                                           VALUES (@locId, @rangeFrom, @rangeTo, @maskedStock)";
                        using (SqlCommand cmdInsert = Fields.CachingDB.Connection.CreateCommand())
                        {
                            cmdInsert.Transaction = tran;
                            cmdInsert.CommandText=insertQuery;

                            cmdInsert.Parameters.AddWithValue("@locId", locationId);
                            cmdInsert.Parameters.AddWithValue("@rangeFrom", rule.RangeFrom);
                            cmdInsert.Parameters.AddWithValue("@rangeTo", rule.RangeTo.HasValue ? (object)rule.RangeTo.Value : DBNull.Value);
                            cmdInsert.Parameters.AddWithValue("@maskedStock", rule.MaskedStock.HasValue ? (object)rule.MaskedStock.Value : DBNull.Value);
                            cmdInsert.ExecuteNonQuery();
                        }
                    }
                    tran.Commit();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public DataTable ListCacheLocations()
        {
            string selectCMDtxt = "selet * from Locations";


            try
            {
                DataTable loc = Fields.CachingDB.ExecuteDatatable(selectCMDtxt);
                return loc;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public DataTable ListOceanBranches()
        {
            string selectCMDtxt = "select * from branches";


            try
            {
                DataTable loc = Fields.OceanDB.ExecuteDatatable(selectCMDtxt);
                return loc;

            }
            catch (Exception)
            {
                throw;
            }
        }
        public enum LocationsStatus
        {
            notexist,
            alreadyexist,
            deleteCompleated,
            updateCompleated,
            addCompleated,
            unknown
        }

        public List<StockMaskingRule> GetMaskingRulesForLocation(long locationId)
        {
            List<StockMaskingRule> rules = new List<StockMaskingRule>();
            {
                string query = "SELECT RangeFrom, RangeTo, MaskedStock FROM dbo.StockMaskingRules WHERE ShopifyLocationId = " + locationId.ToString() + " ORDER BY RangeFrom";


                DataTable dt = Fields.CachingDB.ExecuteDatatable(query);

                foreach (DataRow row in dt.Rows)
                {
                    StockMaskingRule rule = new StockMaskingRule
                    {
                        RangeFrom = Convert.ToInt32(row["RangeFrom"]),
                        RangeTo = row["RangeTo"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["RangeTo"]),
                        MaskedStock = row["MaskedStock"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["MaskedStock"])
                    };
                    rules.Add(rule);
                }
            }
            return rules;
        }

        public List<LocationConfig> GetLocationConfig()
        {

            List<LocationConfig> lc = new List<LocationConfig>();

            string selectCMDtxt = "select * from Locations where active = 'true' and ocean_stock_location_ids is not null";


            try
            {
                DataTable loc = Fields.CachingDB.ExecuteDatatable(selectCMDtxt);

                foreach (DataRow r in loc.Rows)
                {
                    LocationConfig l = new LocationConfig();

                    l.Id = Convert.ToInt64(r["id"]); l.Name = r["name"].ToString();


                    string[] ob = r["ocean_stock_location_ids"].ToString().Split(',');
                    l.LocalDBBranch = new List<string>();

                    foreach (string s in ob)
                        l.LocalDBBranch.Add(s);

                    l.MaskingRules = GetMaskingRulesForLocation(l.Id);

                    lc.Add(l);

                }
                return lc;

            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
