namespace JCortica_RPRO
{
    partial class Relat1Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Relat1Form));
            this.TesteBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.DataSet1 = new JCortica_RPRO.DataSet1();
            this.SProdutosBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.TesteBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SProdutosBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // TesteBindingSource
            // 
            this.TesteBindingSource.DataMember = "Teste";
            this.TesteBindingSource.DataSource = this.DataSet1;
            // 
            // DataSet1
            // 
            this.DataSet1.DataSetName = "DataSet1";
            this.DataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // SProdutosBindingSource
            // 
            this.SProdutosBindingSource.DataMember = "SProdutos";
            this.SProdutosBindingSource.DataSource = this.DataSet1;
            // 
            // Relat1Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1280, 721);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "Relat1Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Impressão: Relatório por Data e Fórmula";
            this.Load += new System.EventHandler(this.Relat1Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TesteBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SProdutosBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.BindingSource TesteBindingSource;
        private DataSet1 DataSet1;
        private System.Windows.Forms.BindingSource SProdutosBindingSource;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
    }
}