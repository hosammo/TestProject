namespace Shopify_Manager.UI
{
    partial class BulkProductCreate_frm
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
            this.components = new System.ComponentModel.Container();
            this.start_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.delimiter1_txt = new System.Windows.Forms.TextBox();
            this.delimiter2_txt = new System.Windows.Forms.TextBox();
            this.productByColor_chk = new System.Windows.Forms.CheckBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label3 = new System.Windows.Forms.Label();
            this.PhotosFolder_txt = new System.Windows.Forms.TextBox();
            this.browse_btn = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.addToTags_lst = new System.Windows.Forms.CheckedListBox();
            this.label16 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // start_btn
            // 
            this.start_btn.Location = new System.Drawing.Point(1055, 36);
            this.start_btn.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.start_btn.Name = "start_btn";
            this.start_btn.Size = new System.Drawing.Size(163, 51);
            this.start_btn.TabIndex = 0;
            this.start_btn.Text = "Start";
            this.start_btn.UseVisualStyleBackColor = true;
            this.start_btn.Click += new System.EventHandler(this.start_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 129);
            this.label1.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 29);
            this.label1.TabIndex = 2;
            this.label1.Text = "delimiter1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(251, 129);
            this.label2.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 29);
            this.label2.TabIndex = 2;
            this.label2.Text = "delimiter2";
            // 
            // delimiter1_txt
            // 
            this.delimiter1_txt.Location = new System.Drawing.Point(163, 120);
            this.delimiter1_txt.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.delimiter1_txt.MaxLength = 1;
            this.delimiter1_txt.Name = "delimiter1_txt";
            this.delimiter1_txt.Size = new System.Drawing.Size(39, 36);
            this.delimiter1_txt.TabIndex = 3;
            this.delimiter1_txt.Text = ".";
            // 
            // delimiter2_txt
            // 
            this.delimiter2_txt.Location = new System.Drawing.Point(379, 120);
            this.delimiter2_txt.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.delimiter2_txt.MaxLength = 1;
            this.delimiter2_txt.Name = "delimiter2_txt";
            this.delimiter2_txt.Size = new System.Drawing.Size(39, 36);
            this.delimiter2_txt.TabIndex = 3;
            this.delimiter2_txt.Text = "-";
            // 
            // productByColor_chk
            // 
            this.productByColor_chk.AutoSize = true;
            this.productByColor_chk.Location = new System.Drawing.Point(436, 125);
            this.productByColor_chk.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.productByColor_chk.Name = "productByColor_chk";
            this.productByColor_chk.Size = new System.Drawing.Size(342, 33);
            this.productByColor_chk.TabIndex = 28;
            this.productByColor_chk.Text = "New product for each color.";
            this.productByColor_chk.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(26, 257);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 92;
            this.dataGridView1.Size = new System.Drawing.Size(1014, 1015);
            this.dataGridView1.TabIndex = 29;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 47);
            this.label3.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 29);
            this.label3.TabIndex = 2;
            this.label3.Text = "Photos Folder";
            // 
            // PhotosFolder_txt
            // 
            this.PhotosFolder_txt.Location = new System.Drawing.Point(199, 38);
            this.PhotosFolder_txt.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.PhotosFolder_txt.Name = "PhotosFolder_txt";
            this.PhotosFolder_txt.Size = new System.Drawing.Size(661, 36);
            this.PhotosFolder_txt.TabIndex = 30;
            // 
            // browse_btn
            // 
            this.browse_btn.Location = new System.Drawing.Point(878, 36);
            this.browse_btn.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.browse_btn.Name = "browse_btn";
            this.browse_btn.Size = new System.Drawing.Size(163, 51);
            this.browse_btn.TabIndex = 31;
            this.browse_btn.Text = "Browse";
            this.browse_btn.UseVisualStyleBackColor = true;
            this.browse_btn.Click += new System.EventHandler(this.browse_btn_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 29;
            this.listBox1.Location = new System.Drawing.Point(1055, 257);
            this.listBox1.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(444, 1019);
            this.listBox1.TabIndex = 32;
            // 
            // addToTags_lst
            // 
            this.addToTags_lst.FormattingEnabled = true;
            this.addToTags_lst.Items.AddRange(new object[] {
            "Dept",
            "Color",
            "Size",
            "Brand",
            "Year",
            "Season"});
            this.addToTags_lst.Location = new System.Drawing.Point(1346, 40);
            this.addToTags_lst.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.addToTags_lst.Name = "addToTags_lst";
            this.addToTags_lst.Size = new System.Drawing.Size(154, 202);
            this.addToTags_lst.TabIndex = 34;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(1339, 4);
            this.label16.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(147, 29);
            this.label16.TabIndex = 33;
            this.label16.Text = "Add To Tags";
            // 
            // BulkProductCreate_frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1530, 1298);
            this.Controls.Add(this.addToTags_lst);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.browse_btn);
            this.Controls.Add(this.PhotosFolder_txt);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.productByColor_chk);
            this.Controls.Add(this.delimiter2_txt);
            this.Controls.Add(this.delimiter1_txt);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.start_btn);
            this.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Name = "BulkProductCreate_frm";
            this.ShowIcon = false;
            this.Text = "BulkProductCreate_frm";
            this.Load += new System.EventHandler(this.BulkProductCreate_frm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button start_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox delimiter1_txt;
        private System.Windows.Forms.TextBox delimiter2_txt;
        private System.Windows.Forms.CheckBox productByColor_chk;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox PhotosFolder_txt;
        private System.Windows.Forms.Button browse_btn;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.CheckedListBox addToTags_lst;
        private System.Windows.Forms.Label label16;
    }
}