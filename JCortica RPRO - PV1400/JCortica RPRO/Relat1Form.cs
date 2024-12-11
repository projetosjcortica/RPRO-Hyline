using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace JCortica_RPRO
{
    public partial class Relat1Form : Form
    {
        public Relat1Form()
        {
            InitializeComponent();
        }

        private void Relat1Form_Load(object sender, EventArgs e)
        {
            
           
            DataTable dbb = TabelasT.DataTable1;
            Carregar_Relatorio();
            this.reportViewer1.RefreshReport();
        }
        private void Carregar_Relatorio()
        {

            
            Form3 Pagina = new Form3();
            DataSet1 Banco = new DataSet1();
            string ValorTot = TabelasT.Total1;
            string ValorTot2 = TabelasT.Total2;
            string ValorBat = TabelasT.Batida1;
            string HoraIni = TabelasT.HoraInicio;
            string HoraFim = TabelasT.HoraFinal;
            string Data1 = TabelasT.DataInicial;
            string Data2 = TabelasT.DataFinal;
            string Observa = TabelasT.Obs;
            string ClienteRR = TabelasT.ClienteR;
            string VisivelFormula = TabelasT.VisivelRelat;
            try
            {

            ReportDataSource Novovo = new ReportDataSource("Teste",TabelasT.DataTable1);
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(Novovo);

                ReportParameter teste = new ReportParameter("Novo", ValorTot);
                this.reportViewer1.LocalReport.SetParameters(teste);
                ReportParameter teste2 = new ReportParameter("Novo2", ValorBat);
                this.reportViewer1.LocalReport.SetParameters(teste2);
                ReportParameter teste3 = new ReportParameter("DataIn", Data1);
                this.reportViewer1.LocalReport.SetParameters(teste3);
                ReportParameter teste4 = new ReportParameter("DataFi", Data2);
                this.reportViewer1.LocalReport.SetParameters(teste4);
                ReportParameter teste5 = new ReportParameter("HoraIN", HoraIni);
                this.reportViewer1.LocalReport.SetParameters(teste5);
                ReportParameter teste6 = new ReportParameter("HoraFi", HoraFim);
                this.reportViewer1.LocalReport.SetParameters(teste6);
                ReportParameter teste7 = new ReportParameter("ObsCamp", Observa);
                this.reportViewer1.LocalReport.SetParameters(teste7);
                ReportParameter teste8 = new ReportParameter("Cliente", ClienteRR);
                this.reportViewer1.LocalReport.SetParameters(teste8);
                ReportParameter teste9 = new ReportParameter("VisivelRelat", VisivelFormula);
                this.reportViewer1.LocalReport.SetParameters(teste9);
                ReportParameter teste10 = new ReportParameter("TotalForm", ValorTot2);
                this.reportViewer1.LocalReport.SetParameters(teste10);


                this.reportViewer1.LocalReport.Refresh();
            this.reportViewer1.RefreshReport();
             }

             catch
            {
            MessageBox.Show("Erro no Report 1!");
             }

            try
            {
                ReportDataSource Novo2 = new ReportDataSource("Produto", TabelasT.DataTable2);
                this.reportViewer1.LocalReport.DataSources.Add(Novo2);
                this.reportViewer1.LocalReport.Refresh();
                this.reportViewer1.RefreshReport();
                
            }

            catch
            {
                MessageBox.Show("Erro no Report 2!");
            }


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
