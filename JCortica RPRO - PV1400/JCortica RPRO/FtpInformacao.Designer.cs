namespace JCortica_RPRO
{
    partial class FtpInformacao
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FtpInformacao));
            this.Statusbox = new System.Windows.Forms.TextBox();
            this.Statusbox2 = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Statusbox
            // 
            this.Statusbox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Statusbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Statusbox.Enabled = false;
            this.Statusbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Statusbox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Statusbox.Location = new System.Drawing.Point(6, 115);
            this.Statusbox.Name = "Statusbox";
            this.Statusbox.ReadOnly = true;
            this.Statusbox.Size = new System.Drawing.Size(302, 15);
            this.Statusbox.TabIndex = 7;
            this.Statusbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Statusbox.TextChanged += new System.EventHandler(this.Statusbox_TextChanged);
            // 
            // Statusbox2
            // 
            this.Statusbox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.Statusbox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Statusbox2.Enabled = false;
            this.Statusbox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Statusbox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.Statusbox2.Location = new System.Drawing.Point(6, 141);
            this.Statusbox2.Name = "Statusbox2";
            this.Statusbox2.ReadOnly = true;
            this.Statusbox2.Size = new System.Drawing.Size(302, 15);
            this.Statusbox2.TabIndex = 8;
            this.Statusbox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.Statusbox2.TextChanged += new System.EventHandler(this.Statusbox2_TextChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(102, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.TabIndex = 19;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // FtpInformacao
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(315, 169);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Statusbox2);
            this.Controls.Add(this.Statusbox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FtpInformacao";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FtpInformacao";
            this.Load += new System.EventHandler(this.FtpInformacao_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox Statusbox;
        private System.Windows.Forms.TextBox Statusbox2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
    }
}