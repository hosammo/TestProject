namespace Shopify_Manager.UI
{
    partial class GrandMall_frm
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
            this.GenerateImagesExcel_btn = new System.Windows.Forms.Button();
            this.txt_tag = new System.Windows.Forms.TextBox();
            this.qmallExport_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GenerateImagesExcel_btn
            // 
            this.GenerateImagesExcel_btn.Location = new System.Drawing.Point(349, 118);
            this.GenerateImagesExcel_btn.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.GenerateImagesExcel_btn.Name = "GenerateImagesExcel_btn";
            this.GenerateImagesExcel_btn.Size = new System.Drawing.Size(600, 51);
            this.GenerateImagesExcel_btn.TabIndex = 0;
            this.GenerateImagesExcel_btn.Text = "Grand Mall Export";
            this.GenerateImagesExcel_btn.UseVisualStyleBackColor = true;
            this.GenerateImagesExcel_btn.Click += new System.EventHandler(this.GenerateImagesExcel_btn_Click);
            // 
            // txt_tag
            // 
            this.txt_tag.Location = new System.Drawing.Point(349, 248);
            this.txt_tag.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.txt_tag.Name = "txt_tag";
            this.txt_tag.Size = new System.Drawing.Size(596, 36);
            this.txt_tag.TabIndex = 1;
            // 
            // qmallExport_btn
            // 
            this.qmallExport_btn.Location = new System.Drawing.Point(349, 183);
            this.qmallExport_btn.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.qmallExport_btn.Name = "qmallExport_btn";
            this.qmallExport_btn.Size = new System.Drawing.Size(600, 51);
            this.qmallExport_btn.TabIndex = 2;
            this.qmallExport_btn.Text = "QMALL Export";
            this.qmallExport_btn.UseVisualStyleBackColor = true;
            this.qmallExport_btn.Click += new System.EventHandler(this.qmallExport_btn_Click);
            // 
            // GrandMall_frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1413, 1142);
            this.Controls.Add(this.qmallExport_btn);
            this.Controls.Add(this.txt_tag);
            this.Controls.Add(this.GenerateImagesExcel_btn);
            this.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.Name = "GrandMall_frm";
            this.ShowIcon = false;
            this.Text = "GrandMall_frm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button GenerateImagesExcel_btn;
        private System.Windows.Forms.TextBox txt_tag;
        private System.Windows.Forms.Button qmallExport_btn;
    }
}