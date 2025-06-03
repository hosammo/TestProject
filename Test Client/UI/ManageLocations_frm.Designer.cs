
namespace Shopify_Manager.UI
{
    partial class ManageLocations_frm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.syncLocations_btn = new System.Windows.Forms.Button();
            this.dgvLocations = new System.Windows.Forms.DataGridView();
            this.updateLocations_btn = new System.Windows.Forms.Button();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.is_active = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.country = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.city = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.linkstatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oceanBranchesIds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocalDBBranch = new System.Windows.Forms.DataGridViewButtonColumn();
            this.maskingRules = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.editMasking = new System.Windows.Forms.DataGridViewButtonColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLocations)).BeginInit();
            this.SuspendLayout();
            // 
            // syncLocations_btn
            // 
            this.syncLocations_btn.Location = new System.Drawing.Point(6, 5);
            this.syncLocations_btn.Margin = new System.Windows.Forms.Padding(1);
            this.syncLocations_btn.Name = "syncLocations_btn";
            this.syncLocations_btn.Size = new System.Drawing.Size(89, 30);
            this.syncLocations_btn.TabIndex = 0;
            this.syncLocations_btn.Text = "Sync Locations";
            this.syncLocations_btn.UseVisualStyleBackColor = true;
            this.syncLocations_btn.Click += new System.EventHandler(this.syncLocations_btn_Click);
            // 
            // dgvLocations
            // 
            this.dgvLocations.AllowUserToAddRows = false;
            this.dgvLocations.AllowUserToDeleteRows = false;
            this.dgvLocations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLocations.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvLocations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLocations.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.is_active,
            this.name,
            this.country,
            this.city,
            this.linkstatus,
            this.oceanBranchesIds,
            this.LocalDBBranch,
            this.maskingRules,
            this.editMasking});
            this.dgvLocations.Location = new System.Drawing.Point(6, 48);
            this.dgvLocations.Margin = new System.Windows.Forms.Padding(1);
            this.dgvLocations.Name = "dgvLocations";
            this.dgvLocations.ReadOnly = true;
            this.dgvLocations.RowHeadersWidth = 92;
            this.dgvLocations.RowTemplate.Height = 38;
            this.dgvLocations.Size = new System.Drawing.Size(679, 317);
            this.dgvLocations.TabIndex = 1;
            this.dgvLocations.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvLocations_CellContentClick);
            // 
            // updateLocations_btn
            // 
            this.updateLocations_btn.Location = new System.Drawing.Point(577, 5);
            this.updateLocations_btn.Margin = new System.Windows.Forms.Padding(1);
            this.updateLocations_btn.Name = "updateLocations_btn";
            this.updateLocations_btn.Size = new System.Drawing.Size(108, 30);
            this.updateLocations_btn.TabIndex = 2;
            this.updateLocations_btn.Text = "Update Locations";
            this.updateLocations_btn.UseVisualStyleBackColor = true;
            this.updateLocations_btn.Click += new System.EventHandler(this.updateLocations_btn_Click);
            // 
            // ID
            // 
            this.ID.Frozen = true;
            this.ID.HeaderText = "id";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Width = 40;
            // 
            // is_active
            // 
            this.is_active.HeaderText = "Active ?";
            this.is_active.Name = "is_active";
            this.is_active.ReadOnly = true;
            this.is_active.Width = 70;
            // 
            // name
            // 
            this.name.HeaderText = "Name";
            this.name.Name = "name";
            this.name.ReadOnly = true;
            this.name.Width = 59;
            // 
            // country
            // 
            this.country.HeaderText = "Country";
            this.country.Name = "country";
            this.country.ReadOnly = true;
            this.country.Width = 71;
            // 
            // city
            // 
            this.city.HeaderText = "City";
            this.city.Name = "city";
            this.city.ReadOnly = true;
            this.city.Width = 51;
            // 
            // linkstatus
            // 
            this.linkstatus.HeaderText = "Link Status";
            this.linkstatus.Name = "linkstatus";
            this.linkstatus.ReadOnly = true;
            this.linkstatus.Width = 84;
            // 
            // oceanBranchesIds
            // 
            this.oceanBranchesIds.HeaderText = "Ocean Branches";
            this.oceanBranchesIds.Name = "oceanBranchesIds";
            this.oceanBranchesIds.ReadOnly = true;
            this.oceanBranchesIds.Width = 101;
            // 
            // LocalDBBranch
            // 
            this.LocalDBBranch.HeaderText = "";
            this.LocalDBBranch.Name = "LocalDBBranch";
            this.LocalDBBranch.ReadOnly = true;
            this.LocalDBBranch.Width = 5;
            // 
            // maskingRules
            // 
            this.maskingRules.HeaderText = "Masking Rules";
            this.maskingRules.Name = "maskingRules";
            this.maskingRules.ReadOnly = true;
            this.maskingRules.Width = 91;
            // 
            // editMasking
            // 
            this.editMasking.HeaderText = "";
            this.editMasking.Name = "editMasking";
            this.editMasking.ReadOnly = true;
            this.editMasking.Width = 5;
            // 
            // ManageLocations_frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 368);
            this.Controls.Add(this.updateLocations_btn);
            this.Controls.Add(this.dgvLocations);
            this.Controls.Add(this.syncLocations_btn);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "ManageLocations_frm";
            this.ShowIcon = false;
            this.Text = "Manage Locations";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.ManageLocations_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLocations)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button syncLocations_btn;
        private System.Windows.Forms.DataGridView dgvLocations;
        private System.Windows.Forms.Button updateLocations_btn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn is_active;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn country;
        private System.Windows.Forms.DataGridViewTextBoxColumn city;
        private System.Windows.Forms.DataGridViewTextBoxColumn linkstatus;
        private System.Windows.Forms.DataGridViewTextBoxColumn oceanBranchesIds;
        private System.Windows.Forms.DataGridViewButtonColumn LocalDBBranch;
        private System.Windows.Forms.DataGridViewTextBoxColumn maskingRules;
        private System.Windows.Forms.DataGridViewButtonColumn editMasking;
    }
}