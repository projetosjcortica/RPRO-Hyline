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
    public partial class RelatorioData : Form
    {
        public RelatorioData()
        {
            InitializeComponent();
        }

        private void RelatorioData_Load(object sender, EventArgs e)
        {

            Carregar_Relatorio();
            this.reportViewer1.RefreshReport();
        }

         private void Carrega()
        {
            
        }
        private void Carregar_Relatorio()
        {
            Form3 Pagina = new Form3();
          
            string ValorTot = TabelasT.DTotal1;
            string ValorBat = TabelasT.DBatida1;
            string HoraIni = TabelasT.DHoraInicio;
            string HoraFim = TabelasT.DHoraFinal;
            string Data1 = TabelasT.DDataInicial;
            string Data2 = TabelasT.DDataFinal;
            string Observa = TabelasT.DObs;
            string ClienteRR = Properties.Settings.Default.Cliente;
            try
            {

                ReportDataSource Novovo = new ReportDataSource("DataSet1", TabelasT.DataTable3);
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




                this.reportViewer1.LocalReport.Refresh();
                this.reportViewer1.RefreshReport();
            }

            catch
            {
                MessageBox.Show("Erro no Report 1!");
            }
        }
    }
    }

