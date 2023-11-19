namespace BoundingBoxing
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.pnl_window = new System.Windows.Forms.Panel();
            this.lbl_title = new System.Windows.Forms.Label();
            this.pnl_window.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnl_window
            // 
            this.pnl_window.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.pnl_window.Controls.Add(this.lbl_title);
            this.pnl_window.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnl_window.Location = new System.Drawing.Point(0, 0);
            this.pnl_window.Margin = new System.Windows.Forms.Padding(2);
            this.pnl_window.Name = "pnl_window";
            this.pnl_window.Size = new System.Drawing.Size(708, 378);
            this.pnl_window.TabIndex = 0;
            this.pnl_window.Paint += new System.Windows.Forms.PaintEventHandler(this.pnl_window_Paint);
            this.pnl_window.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.pnl_window_MouseDoubleClick);
            this.pnl_window.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pnl_window_MouseDown);
            this.pnl_window.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pnl_window_MouseMove);
            this.pnl_window.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pnl_window_MouseUp);
            // 
            // lbl_title
            // 
            this.lbl_title.AutoSize = true;
            this.lbl_title.Location = new System.Drawing.Point(0, 0);
            this.lbl_title.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(87, 12);
            this.lbl_title.TabIndex = 1;
            this.lbl_title.Text = "BoundingBoxing";
            this.lbl_title.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbl_title_MouseDown);
            this.lbl_title.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lbl_title_MouseMove);
            this.lbl_title.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lbl_title_MouseUp);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(708, 378);
            this.Controls.Add(this.pnl_window);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "BoundingBoxing";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.pnl_window.ResumeLayout(false);
            this.pnl_window.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnl_window;
        private System.Windows.Forms.Label lbl_title;
    }
}

