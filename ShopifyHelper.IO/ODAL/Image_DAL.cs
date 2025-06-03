using Shopify.IO.Types;
using System;
using System.Data;
using System.Data.SqlClient;

namespace ShopifyHelper.IO.ODAL
{
    public class Image_DAL
    {
        public imageStatus CheckImage(Image image)
        {
            //check if image exist.
            //if not exit, add the image to the database.
            //if exist, then check if it is changed.
            //for deleted images from shopify, it will be handelled in the sync operation.
            return imageStatus.unknown;
        }

        public imageStatus AddImage(Image image)
        {

            SqlCommand insertCMD = Fields.CachingDB.Connection.CreateCommand();

            string insertImageCmdTxt = @"IF NOT EXISTS (SELECT 1 FROM dbo.Images WHERE id = @id)
BEGIN
    INSERT INTO dbo.Images (id, product_id, position, created_at, updated_at, src)
    VALUES (@id, @product_id, @position, @created_at, @updated_at, @src);
END";

            //insert image row.

            insertCMD.CommandText = insertImageCmdTxt;

            insertCMD.Parameters.Add("@id", SqlDbType.BigInt).Value = image.id;
            insertCMD.Parameters.Add("@product_id", SqlDbType.BigInt).Value = image.product_id;
            insertCMD.Parameters.Add("@position", SqlDbType.Int).Value = image.position;
            insertCMD.Parameters.Add("@created_at", SqlDbType.DateTime).Value = Convert.ToDateTime(image.created_at);
            insertCMD.Parameters.Add("@updated_at", SqlDbType.DateTime).Value = Convert.ToDateTime(image.updated_at);
            insertCMD.Parameters.Add("@src", SqlDbType.NVarChar).Value = image.src;

            try
            {
                int affectedRows = insertCMD.ExecuteNonQuery();
                if (affectedRows > 0)
                    return imageStatus.addCompleated;
                else
                    return imageStatus.unknown;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public imageStatus UpdateImage(Image image)
        {
            return imageStatus.unknown;
        }

        public imageStatus DeleteImage(Image image)
        {
            string deleteCMDtxt = @"";


            SqlCommand deleteCMD = Fields.CachingDB.Connection.CreateCommand();

            deleteCMD.CommandText = deleteCMDtxt;

            deleteCMD.Parameters.Add("@id", SqlDbType.BigInt).Value = "";

            try
            {
                int affectedRows = deleteCMD.ExecuteNonQuery();
                if (affectedRows > 0)
                    return imageStatus.deleteCompleated;
                else
                    return imageStatus.unknown;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public enum imageStatus
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
