namespace Shopify_Manager.UI
{
    partial class ProfileSelector_frm
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
            this.profiles_lst = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ok_btn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // profiles_lst
            // 
            this.profiles_lst.FormattingEnabled = true;
            this.profiles_lst.ItemHeight = 29;
            this.profiles_lst.Location = new System.Drawing.Point(33, 76);
            this.profiles_lst.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.profiles_lst.Name = "profiles_lst";
            this.profiles_lst.Size = new System.Drawing.Size(604, 352);
            this.profiles_lst.TabIndex = 0;
            this.profiles_lst.KeyDown += new System.Windows.Forms.KeyEventHandler(this.profiles_lst_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 29);
            this.label1.TabIndex = 1;
            this.label1.Text = "Available profiles";
            // 
            // ok_btn
            // 
            this.ok_btn.Location = new System.Drawing.Point(251, 442);
            this.ok_btn.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.ok_btn.Name = "ok_btn";
            this.ok_btn.Size = new System.Drawing.Size(163, 60);
            this.ok_btn.TabIndex = 2;
            this.ok_btn.Text = "OK";
            this.ok_btn.UseVisualStyleBackColor = true;
            this.ok_btn.Click += new System.EventHandler(this.ok_btn_Click);
            // 
            // ProfileSelector_frm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 29F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 515);
            this.Controls.Add(this.ok_btn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.profiles_lst);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(7, 7, 7, 7);
            this.MaximizeBox = false;
            this.Name = "ProfileSelector_frm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select a profile";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ProfileSelector_frm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox profiles_lst;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ok_btn;
    }
}