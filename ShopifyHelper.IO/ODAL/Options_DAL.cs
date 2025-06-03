using Shopify.IO.Types;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ShopifyHelper.IO.ODAL
{
    public class Options_DAL
    {
        public optionStatus CheckOption(Option option)
        {
            //check if option exist.
            //if not exit, add the option to the database.
            //if exist, then check if it is changed.
            //for deleted products from shopify, it will be handelled in the sync operation.
            return optionStatus.unknown;
        }

        public optionStatus AddOption(Option option)
        {

            SqlCommand insertCMD = Fields.CachingDB.Connection.CreateCommand();

            string insertOptionCmdTxt = @"INSERT  INTO dbo.Options ( id, product_id, name, position)
                        VALUES  (@id, @product_id, @name, @position);";

            //insert option row.

            insertCMD.CommandText = insertOptionCmdTxt;

            insertCMD.Parameters.Add("@id", SqlDbType.BigInt).Value = option.id;
            insertCMD.Parameters.Add("@name", SqlDbType.NVarChar, 50).Value = option.name;
            insertCMD.Parameters.Add("@product_id", SqlDbType.BigInt).Value = option.product_id;
            insertCMD.Parameters.Add("@position", SqlDbType.Int).Value = option.position;

            try
            {
                int affectedRows = insertCMD.ExecuteNonQuery();
                if (affectedRows > 0)
                {
                    foreach (string s in option.values)
                    {
                        string insertOptionValueCmdTxt = @"INSERT  INTO dbo.OptionsValues ( option_id, value )
                        VALUES  (@option_id, @value);";

                        insertCMD = Fields.CachingDB.Connection.CreateCommand();
                        insertCMD.CommandText = insertOptionValueCmdTxt;

                        insertCMD.Parameters.Add("@option_id", SqlDbType.BigInt).Value = option.id;
                        insertCMD.Parameters.Add("@value", SqlDbType.NVarChar, 50).Value = s;

                        affectedRows = insertCMD.ExecuteNonQuery();

                    }
                    return optionStatus.addCompleated;
                }
                else
                    return optionStatus.unknown;
            }
            catch 
            {
                throw;
            }
        }

        public optionStatus UpdateOption(Option option)
        {
            return optionStatus.unknown;
        }

        public optionStatus DeleteOption(Option option)
        {
            string deleteCMDtxt = "";


            SqlCommand deleteCMD = Fields.CachingDB.Connection.CreateCommand();

            deleteCMD.CommandText = deleteCMDtxt;

            deleteCMD.Parameters.Add("@id", SqlDbType.BigInt).Value = "";

            try
            {
                int affectedRows = deleteCMD.ExecuteNonQuery();
                if (affectedRows > 0)
                    return optionStatus.deleteCompleated;
                else
                    return optionStatus.unknown;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public enum optionStatus
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
