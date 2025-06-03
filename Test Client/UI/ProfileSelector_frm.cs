using Shopify.IO;
using Shopify.IO.helpers;
using Shopify_Manager.SettingsTypes;
using ShopifyHelper.IO;
using System;
using System.Windows.Forms;

namespace Shopify_Manager.UI
{
    public partial class ProfileSelector_frm : Form
    {
        public ProfileSelector_frm()
        {
            InitializeComponent();
        }

        private void ProfileSelector_frm_Load(object sender, EventArgs e)
        {
            profiles_lst.DataSource = General.SettingsObj.Profiles;
            profiles_lst.DisplayMember = "ProfileName";
        }

        private void ok_btn_Click(object sender, EventArgs e)
        {
            selectProfile();
        }

        private void profiles_lst_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                selectProfile();
            }
        }

        private void selectProfile()
        {
            Profile pr = (Profile)profiles_lst.SelectedItem;
            General.CurrentProfile = pr;

            Fields.CurrentStore = new StoreManager(new APIAccess(pr.ApiKey, pr.Password, pr.SharedSecret, pr.HostName, pr.ApiVersion));

            this.Close();

         }
    }
}
