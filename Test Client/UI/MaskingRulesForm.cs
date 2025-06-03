using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static Shopify_Manager.UI.ManageLocations_frm;
using static ShopifyHelper.IO.ODAL.Locations_DAL;

namespace ShopifyHelper.IO
{
    public partial class MaskingRulesForm : Form
    {
        // The list of rules edited by the user.
        public List<StockMaskingRule> SelectedRules { get; set; }

        // Constructor accepts the current set of rules (which can be null).
        public MaskingRulesForm(List<StockMaskingRule> currentRules)
        {
            InitializeComponent();
            SelectedRules = currentRules != null
                                ? new List<StockMaskingRule>(currentRules)
                                : new List<StockMaskingRule>();

            this.Load += MaskingRulesForm_Load;
            btnAddRule.Click += btnAddRule_Click;
            btnRemoveRule.Click += btnRemoveRule_Click;
            btnOK.Click += btnOK_Click;
            btnCancel.Click += btnCancel_Click;
        }

        private void MaskingRulesForm_Load(object sender, EventArgs e)
        {
            // Initialize the grid.
            dgvRules.Columns.Clear();
            dgvRules.AutoGenerateColumns = false;

            // Column for the lower bound ("From").
            DataGridViewTextBoxColumn colFrom = new DataGridViewTextBoxColumn
            {
                Name = "RangeFrom",
                HeaderText = "From"
            };
            dgvRules.Columns.Add(colFrom);

            // Column for the optional upper bound ("To").
            DataGridViewTextBoxColumn colTo = new DataGridViewTextBoxColumn
            {
                Name = "RangeTo",
                HeaderText = "To (optional)"
            };
            dgvRules.Columns.Add(colTo);

            // Column for the masked stock value.
            DataGridViewTextBoxColumn colMasked = new DataGridViewTextBoxColumn
            {
                Name = "MaskedStock",
                HeaderText = "Masked Stock (if empty: use raw)"
            };
            dgvRules.Columns.Add(colMasked);

            // Populate existing rules if any.
            foreach (var rule in SelectedRules)
            {
                string toVal = rule.RangeTo.HasValue ? rule.RangeTo.Value.ToString() : "";
                string maskedVal = rule.MaskedStock.HasValue ? rule.MaskedStock.Value.ToString() : "";
                dgvRules.Rows.Add(rule.RangeFrom, toVal, maskedVal);
            }
        }

        // Button handler to add a new (empty) row.
        private void btnAddRule_Click(object sender, EventArgs e)
        {
            dgvRules.Rows.Add();
        }

        // Button handler to remove selected row(s).
        private void btnRemoveRule_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvRules.SelectedRows)
            {
                if (!row.IsNewRow)
                    dgvRules.Rows.Remove(row);
            }
        }

        // OK button: validate input, update SelectedRules, and close with OK result.

        private void btnOK_Click(object sender, EventArgs e)
        {
            List<StockMaskingRule> rules = new List<StockMaskingRule>();

            // Process each row from the DataGridView.
            foreach (DataGridViewRow row in dgvRules.Rows)
            {
                if (row.IsNewRow)
                    continue;

                // Validate 'From' is a valid integer.
                if (!int.TryParse(Convert.ToString(row.Cells["RangeFrom"].Value), out int from))
                {
                    MessageBox.Show("Invalid 'From' value in one of the rules.", "Validation Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validate 'To' if provided.
                int? to = null;
                if (row.Cells["RangeTo"].Value != null && !string.IsNullOrWhiteSpace(row.Cells["RangeTo"].Value.ToString()))
                {
                    if (int.TryParse(row.Cells["RangeTo"].Value.ToString(), out int toVal))
                    {
                        if (toVal < from)
                        {
                            MessageBox.Show("'To' value cannot be less than 'From' value.", "Validation Error",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        to = toVal;
                    }
                    else
                    {
                        MessageBox.Show("Invalid 'To' value in one of the rules.", "Validation Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                // Validate 'Masked Stock': if empty and 'To' is provided then error; if 'To' is not set, allow empty (store as null).
                int? masked = null;
                string maskedStockText = row.Cells["MaskedStock"].Value?.ToString().Trim();
                if (string.IsNullOrEmpty(maskedStockText))
                {
                    if (to.HasValue)
                    {
                        MessageBox.Show("Masked Stock cannot be empty when range 'To' is specified. Please provide a valid numeric value.", "Validation Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    else
                    {
                        masked = null;
                    }
                }
                else
                {
                    if (!int.TryParse(maskedStockText, out int maskedVal))
                    {
                        MessageBox.Show("Invalid 'Masked Stock' value in one of the rules.", "Validation Error",
                                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    masked = maskedVal;
                }

                rules.Add(new StockMaskingRule { RangeFrom = from, RangeTo = to, MaskedStock = masked });
            }

            // Enforce that ranges do not intersect or form a junction.
            // If 'To' is not provided, the effective upper bound is 'From'.
            var sortedRules = rules.OrderBy(r => r.RangeFrom).ToList();
            for (int i = 0; i < sortedRules.Count - 1; i++)
            {
                var current = sortedRules[i];
                var next = sortedRules[i + 1];
                int effectiveCurrentTo = current.RangeTo.HasValue ? current.RangeTo.Value : current.RangeFrom;
                if (effectiveCurrentTo >= next.RangeFrom)
                {
                    MessageBox.Show(
                        $"The rule starting at {current.RangeFrom} with an effective upper bound of {effectiveCurrentTo} " +
                        $"overlaps or touches the rule starting at {next.RangeFrom}. " +
                        "Please ensure that no ranges intersect or share a common boundary.",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            SelectedRules = rules;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        //private void btnOK_Click(object sender, EventArgs e)
        //{
        //    List<StockMaskingRule> rules = new List<StockMaskingRule>();
        //    // HashSet to track unique rule keys.
        //    HashSet<string> ruleKeys = new HashSet<string>();

        //    foreach (DataGridViewRow row in dgvRules.Rows)
        //    {
        //        if (row.IsNewRow)
        //            continue;

        //        if (!int.TryParse(Convert.ToString(row.Cells["RangeFrom"].Value), out int from))
        //        {
        //            MessageBox.Show("Invalid 'From' value in one of the rules.", "Validation Error",
        //                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }
        //        int? to = null;
        //        if (row.Cells["RangeTo"].Value != null && !string.IsNullOrWhiteSpace(row.Cells["RangeTo"].Value.ToString()))
        //        {
        //            if (int.TryParse(row.Cells["RangeTo"].Value.ToString(), out int toVal))
        //            {
        //                to = toVal;
        //            }
        //            else
        //            {
        //                MessageBox.Show("Invalid 'To' value in one of the rules.", "Validation Error",
        //                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                return;
        //            }
        //        }
        //        int? masked = null;
        //        if (row.Cells["MaskedStock"].Value != null && !string.IsNullOrWhiteSpace(row.Cells["MaskedStock"].Value.ToString()))
        //        {
        //            if (int.TryParse(row.Cells["MaskedStock"].Value.ToString(), out int maskedVal))
        //            {
        //                masked = maskedVal;
        //            }
        //            else
        //            {
        //                MessageBox.Show("Invalid 'Masked Stock' value in one of the rules.", "Validation Error",
        //                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //                return;
        //            }
        //        }

        //        // Create a unique key for the rule.
        //        string key = $"{from}_{(to.HasValue ? to.Value.ToString() : "null")}_{(masked.HasValue ? masked.Value.ToString() : "null")}";
        //        if (!ruleKeys.Add(key))
        //        {
        //            MessageBox.Show($"Duplicate rule detected:\nFrom: {from}, To: {(to.HasValue ? to.Value.ToString() : "Not set")}, Masked Stock: {(masked.HasValue ? masked.Value.ToString() : "Not set")}",
        //                            "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //            return;
        //        }

        //        rules.Add(new StockMaskingRule { RangeFrom = from, RangeTo = to, MaskedStock = masked });
        //    }
        //    SelectedRules = rules;
        //    this.DialogResult = DialogResult.OK;
        //    this.Close();
        //}
        // Cancel button: close without saving changes.
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}