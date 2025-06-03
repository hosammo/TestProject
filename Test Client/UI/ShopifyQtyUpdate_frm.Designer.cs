namespace Shopify_Manager.UI
{
    partial class ShopifyQtyUpdate_frm
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
            this.QtyAvailable_grd = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.UpdateAllQty_btn = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.updatePrices_btn = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.barcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.location = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oldQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.newQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oldprice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.newPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.oldcomparedat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.newComparedat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.state = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.QtyAvailable_grd)).BeginInit();
            this.SuspendLayout();
            // 
            // QtyAvailable_grd
            // 
            this.QtyAvailable_grd.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.QtyAvailable_grd.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.QtyAvailable_grd.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.barcode,
            this.location,
            this.oldQty,
            this.newQty,
            this.oldprice,
            this.newPrice,
            this.oldcomparedat,
            this.newComparedat,
            this.state});
            this.QtyAvailable_grd.Location = new System.Drawing.Point(8, 97);
            this.QtyAvailable_grd.Name = "QtyAvailable_grd";
            this.QtyAvailable_grd.RowHeadersWidth = 92;
            this.QtyAvailable_grd.Size = new System.Drawing.Size(836, 344);
            this.QtyAvailable_grd.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(199, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Variants with available qty in Ocean.Net";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(210, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(176, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Variants HAS NO QTY in Ocean.Net";
            // 
            // UpdateAllQty_btn
            // 
            this.UpdateAllQty_btn.Location = new System.Drawing.Point(9, 39);
            this.UpdateAllQty_btn.Name = "UpdateAllQty_btn";
            this.UpdateAllQty_btn.Size = new System.Drawing.Size(117, 23);
            this.UpdateAllQty_btn.TabIndex = 8;
            this.UpdateAllQty_btn.Text = "Update All Qty";
            this.UpdateAllQty_btn.UseVisualStyleBackColor = true;
            this.UpdateAllQty_btn.Click += new System.EventHandler(this.UpdateAllQty_btn_Click);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(685, 39);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(159, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Get 0 Weight Variants";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // updatePrices_btn
            // 
            this.updatePrices_btn.Location = new System.Drawing.Point(132, 39);
            this.updatePrices_btn.Name = "updatePrices_btn";
            this.updatePrices_btn.Size = new System.Drawing.Size(117, 23);
            this.updatePrices_btn.TabIndex = 11;
            this.updatePrices_btn.Text = "Update All Prices";
            this.updatePrices_btn.UseVisualStyleBackColor = true;
            this.updatePrices_btn.Click += new System.EventHandler(this.updatePrices_btn_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(604, 39);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 17;
            this.button2.Text = "Get 0 Prices";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(9, 68);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(835, 23);
            this.progressBar1.TabIndex = 18;
            // 
            // barcode
            // 
            this.barcode.HeaderText = "barcode";
            this.barcode.MinimumWidth = 11;
            this.barcode.Name = "barcode";
            this.barcode.ReadOnly = true;
            this.barcode.Width = 225;
            // 
            // location
            // 
            this.location.HeaderText = "Location";
            this.location.Name = "location";
            this.location.ReadOnly = true;
            // 
            // oldQty
            // 
            this.oldQty.HeaderText = "O Qty";
            this.oldQty.MinimumWidth = 11;
            this.oldQty.Name = "oldQty";
            this.oldQty.ReadOnly = true;
            this.oldQty.Width = 65;
            // 
            // newQty
            // 
            this.newQty.HeaderText = "N Qty";
            this.newQty.MinimumWidth = 11;
            this.newQty.Name = "newQty";
            this.newQty.ReadOnly = true;
            this.newQty.Width = 65;
            // 
            // oldprice
            // 
            this.oldprice.HeaderText = "O Price";
            this.oldprice.MinimumWidth = 11;
            this.oldprice.Name = "oldprice";
            this.oldprice.ReadOnly = true;
            this.oldprice.Width = 70;
            // 
            // newPrice
            // 
            this.newPrice.HeaderText = "N Price";
            this.newPrice.MinimumWidth = 11;
            this.newPrice.Name = "newPrice";
            this.newPrice.ReadOnly = true;
            this.newPrice.Width = 65;
            // 
            // oldcomparedat
            // 
            this.oldcomparedat.HeaderText = "O Cmprd At";
            this.oldcomparedat.MinimumWidth = 11;
            this.oldcomparedat.Name = "oldcomparedat";
            this.oldcomparedat.ReadOnly = true;
            this.oldcomparedat.Width = 225;
            // 
            // newComparedat
            // 
            this.newComparedat.HeaderText = "N Cmprd At";
            this.newComparedat.MinimumWidth = 11;
            this.newComparedat.Name = "newComparedat";
            this.newComparedat.ReadOnly = true;
            this.newComparedat.Width = 225;
            // 
            // state
            // 
            this.state.HeaderText = "State";
            this.state.MinimumWidth = 11;
            this.state.Name = "state";
            this.state.ReadOnly = true;
            this.state.Width = 225;
            // 
            // ShopifyQtyUpdate_frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(853, 450);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.updatePrices_btn);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.UpdateAllQty_btn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.QtyAvailable_grd);
            this.Name = "ShopifyQtyUpdate_frm";
            this.ShowIcon = false;
            this.Text = "Shopify Qty Update";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShopifyQtyUpdate_frm_FormClosing);
            this.Load += new System.EventHandler(this.ShopifyQtyUpdate_frm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.QtyAvailable_grd)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView QtyAvailable_grd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button UpdateAllQty_btn;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button updatePrices_btn;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.DataGridViewTextBoxColumn barcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn location;
        private System.Windows.Forms.DataGridViewTextBoxColumn oldQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn newQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn oldprice;
        private System.Windows.Forms.DataGridViewTextBoxColumn newPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn oldcomparedat;
        private System.Windows.Forms.DataGridViewTextBoxColumn newComparedat;
        private System.Windows.Forms.DataGridViewTextBoxColumn state;
    }
}

