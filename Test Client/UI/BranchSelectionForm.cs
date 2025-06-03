using ShopifyHelper.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static ShopifyHelper.IO.ODAL.Locations_DAL;

namespace Shopify_Manager.UI
{
    public partial class BranchSelectionForm : Form
    {
        public string SelectedBranchIds { get; set; }
        private List<Branch> branches;
        private string currentSelection; // comma separated branch IDs that are already assigned

        public BranchSelectionForm(List<Branch> branches, string currentSelection)
        {
            InitializeComponent();
            this.branches = branches;
            this.currentSelection = currentSelection;
        }

        private void BranchSelectionForm_Load(object sender, EventArgs e)
        {
            // Parse the current selection into a list for easy lookup.
            var selectedIds = new List<string>();
            if (!string.IsNullOrEmpty(currentSelection))
            {
                selectedIds = currentSelection.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }

            // Populate the CheckedListBox with branches.
            checkedListBoxBranches.Items.Clear();
            foreach (var branch in branches)
            {
                int index = checkedListBoxBranches.Items.Add(branch);
                if (selectedIds.Contains(branch.BranchId.ToString()))
                {
                    checkedListBoxBranches.SetItemChecked(index, true);
                }
            }
        }

        private void ok_btn_Click(object sender, EventArgs e)
        {
            // Build and return a comma-separated list of selected branch IDs.
            var selected = checkedListBoxBranches.CheckedItems.Cast<Branch>()
                            .Select(b => b.BranchId.ToString())
                            .ToArray();
            SelectedBranchIds = string.Join(",", selected);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancel_btn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
