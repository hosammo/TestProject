using Shopify.IO.helpers;
using Shopify.IO.Operations;
namespace Shopify.IO
{
    public class StoreManager
    {
        public APIAccess CurrentSroreAPIAccess;
        public StoreManager()
        {

        }

        public StoreManager(APIAccess APIAccessObject)
        {
            CurrentSroreAPIAccess = APIAccessObject;
        }

        public Products Products 

        { 
            get
            {
                return new Products(CurrentSroreAPIAccess);
            }
        }

        public Metafields Metafields
        {
            get
            {
                return new Metafields(CurrentSroreAPIAccess);
            }
        }

        public Variants Variants
        {
            get
            {
                return new Variants(CurrentSroreAPIAccess);
            }
        }
        public Locations Locations
        {
            get
            {
                return new Locations(CurrentSroreAPIAccess);
            }
        }
    }
}
