using ShopifyHelper.IO;
using ShopifyHelper.IO.ODAL;
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
    // Class representing the Shopify location configuration.

    public partial class ManageLocations_frm : Form
    {
        private List<Branch> localBranches = new List<Branch>();
        Locations_DAL ldal = new Locations_DAL();
        public ManageLocations_frm()
        {
            InitializeComponent();
        }

        private void ManageLocations_Load(object sender, EventArgs e)
        {
            SyncLocations();
        }

        private void LoadShopifyLocations()
        {
            // Clear the grid first.
            dgvLocations.Rows.Clear();

            string query = @"SELECT id, name, country, city,active,
                                CASE 
                                  WHEN ocean_stock_location_ids IS NULL OR ocean_stock_location_ids = '' THEN 'Not Assigned'
                                  ELSE 'Assigned'
                                END AS linkstatus,
                                ocean_stock_location_ids  
                                FROM dbo.Locations";
            _ = new DataTable();

            DataTable dt = Fields.CachingDB.ExecuteDatatable(query);
            foreach (DataRow row in dt.Rows)
            {
                // Load row values; if LocalDBBranch is null, show empty.
                long id = Convert.ToInt64(row["id"]);
                string name = row["name"].ToString();
                string country = row["country"].ToString();
                string city = row["city"].ToString();
                string linkstatus = row["linkstatus"].ToString();
                string localDBBranch = row["ocean_stock_location_ids"] == DBNull.Value ? string.Empty : row["ocean_stock_location_ids"].ToString();
                string isActive = row["active"].ToString();

                // Retrieve masking rules for this location.
                List<StockMaskingRule> maskingRules = ldal.GetMaskingRulesForLocation(id);
                string maskingSummary = GenerateRulesSummary(maskingRules);

                int rowIndex = dgvLocations.Rows.Add(id, isActive,  name, country, city, linkstatus, localDBBranch, "Edit Branches", maskingSummary, "Edit Masking");
                dgvLocations.Rows[rowIndex].Cells["maskingRules"].Tag = maskingRules;

                if (dgvLocations.Rows[rowIndex].Cells["is_active"].Value.ToString() == "True")
                {
                    dgvLocations.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightGreen;
                }
                else
                {
                    dgvLocations.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightPink;
                }

                if (dgvLocations.Rows[rowIndex].Cells["oceanBranchesIds"].Value.ToString() == "")
                {
                    dgvLocations.Rows[rowIndex].Cells["oceanBranchesIds"].Style.BackColor = Color.Orange;
                }

            }
        }



        // Generates a summary string for a list of masking rules.
        private string GenerateRulesSummary(List<StockMaskingRule> rules)
        {
            if (rules == null || rules.Count == 0)
                return "No rules configured";

            var summaries = rules.Select(rule =>
            {
                string toPart = rule.RangeTo.HasValue ? rule.RangeTo.Value.ToString() : "∞";
                string valuePart = rule.MaskedStock.HasValue ? rule.MaskedStock.Value.ToString() : "raw";
                return $"{rule.RangeFrom}-{toPart}: {valuePart}";
            });
            return string.Join("; ", summaries);
        }

        // Handle cell clicks in the grid.

        private void LoadOceanBranches()
        {
            // Query the branches table from your local database.

            DataTable dt = new DataTable();

            dt = ldal.ListOceanBranches();

            foreach (DataRow row in dt.Rows)
            {
                localBranches.Add(new Branch
                {
                    BranchId = Convert.ToInt32(row["branchid"]),
                    BranchName = row["branchName"].ToString()
                });
            }
        }

        private void syncLocations_btn_Click(object sender, EventArgs e)
        {
            SyncLocations();
        }

        private void dgvLocations_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string columnName = dgvLocations.Columns[e.ColumnIndex].Name;

            if (dgvLocations.Columns[e.ColumnIndex].Name == "LocalDBBranch" && e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvLocations.Rows[e.RowIndex];
                // Retrieve any current selection; assumed as a comma‐separated string (e.g., "1,3,5").
                string currentSelection = row.Cells["oceanBranchesIds"].Value?.ToString();

                using (BranchSelectionForm branchForm = new BranchSelectionForm(localBranches, currentSelection))
                {
                    if (branchForm.ShowDialog() == DialogResult.OK)
                    {
                        // The branch form returns a comma-separated list of selected branch IDs.
                        string selectedBranches = branchForm.SelectedBranchIds;
                        dgvLocations.Rows[e.RowIndex].Cells["oceanBranchesIds"].Value = selectedBranches;
                        dgvLocations.Rows[e.RowIndex].Cells["linkstatus"].Value = string.IsNullOrWhiteSpace(selectedBranches) ? "Not Assigned" : "Assigned";
                    }
                }
            }
            //If the Edit Masking button is clicked, open the masking rule editor.
            else if (dgvLocations.Columns[e.ColumnIndex].Name == "editMasking" && e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvLocations.Rows[e.RowIndex];

                long locationId = Convert.ToInt64(row.Cells["id"].Value);
                var currentRules = row.Cells["maskingRules"].Tag as List<StockMaskingRule>;
                using (MaskingRulesForm maskingForm = new MaskingRulesForm(currentRules))
                {
                    maskingForm.Text = $"Edit Masking Rules for { row.Cells["name"].Value} ";
                    if (maskingForm.ShowDialog() == DialogResult.OK)
                    {
                        List<StockMaskingRule> newRules = maskingForm.SelectedRules;
                        // Update the row's Tag and the summary display.
                        row.Cells["maskingRules"].Tag = newRules;
                        row.Cells["maskingRules"].Value = GenerateRulesSummary(newRules);
                        // Immediately persist the changes to the masking rules for this location.
                        bool updated = ldal.UpdateMaskingRulesForLocation(locationId, newRules);
                        if (!updated)
                        {
                            MessageBox.Show("Error saving masking rules for location: " + row.Cells["name"].Value);
                        }
                    }
                }
            }
        }
        private void updateLocations_btn_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvLocations.Rows)
            {
                if (row.IsNewRow)
                    continue;
                long id = Convert.ToInt64(row.Cells["id"].Value);
                string localDBBranch = row.Cells["oceanBranchesIds"].Value?.ToString();
                ldal.UpdateLinkInfo(localDBBranch, id);
            }
            MessageBox.Show("Locations updated successfully.");
        }

        private void SyncLocations()
        {
            Fields.SyncLocations();

            LoadOceanBranches();
            LoadShopifyLocations();
        }
    }
}
