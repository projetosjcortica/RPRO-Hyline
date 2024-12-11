using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Timers;
using System.Windows.Forms;
using TesteImpresao.Entities;


namespace JCortica_RPRO
{
    public partial class DataHora : Form
    {

        public DateTime Dt1_1 { get; set; }
        public DateTime Dt2_1 { get; set; }
        public String File_1 { get; set; }
        public String File_2 { get; set; }
        public String File_3 { get; set; }
        public String Atual_file { get; set; }
        public String caminho_file { get; set; }

        public DataHora()
        {
            InitializeComponent();
        }

        private void DataHora_Load(object sender, EventArgs e)
        {

            

            CarregaCombobox();
            ObsBox.Enabled = false;
            checkBox1.Checked = false;
        }
        public void CarregaCombobox()
        {



            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

     
           


            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);
            String Sql = "Select Distinct Dia from relatorio";
            MySqlCommand Comando = new MySqlCommand(Sql, Conexao);
            Conexao.Open();
            if (comboBox1.Items.Count > 0) { comboBox1.Items.Clear(); }
            if (comboBox2.Items.Count > 0) { comboBox2.Items.Clear(); }

            try
            {
  

                


                String SqlBanco = "Select * from materiaprima";
                MySqlCommand ComNomeProd = new MySqlCommand(SqlBanco, Conexao);
                MySqlDataAdapter NomeAdapter = new MySqlDataAdapter(ComNomeProd);
                DataTable TempNome = new DataTable();
                NomeAdapter.Fill(TempNome);
                TabelasT.NomeProduto = TempNome;

                String Orsqlx = "select * from cadastro.relatorio order by str_to_date(dia,'%d/%m/%Y') asc";
                MySqlCommand OrComando1x = new MySqlCommand(Orsqlx, Conexao);
                OrComando1x.ExecuteNonQuery();


                MySqlDataAdapter objAdapter = new MySqlDataAdapter(Comando);
                DataTable dtlista = new DataTable();
                objAdapter.Fill(dtlista);
                MySqlDataReader leitor = Comando.ExecuteReader();
                while (leitor.Read())
                {
                    comboBox1.Items.Add(leitor["Dia"].ToString());
                    comboBox2.Items.Add(leitor["Dia"].ToString());

                }

               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na conexão MySql:" + ex.Message);

            }


            Conexao.Close();
        }

        public void CarregaComboApenas()
        {

            comboBox1.Text = "";
            comboBox2.Text = "";

            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);
            String Sql = "Select Distinct Dia from relatorio";
            MySqlCommand Comando = new MySqlCommand(Sql, Conexao);
            Conexao.Open();


            if (comboBox1.Items.Count > 0) { comboBox1.Items.Clear(); }
            if (comboBox2.Items.Count > 0) { comboBox2.Items.Clear(); }



            try
            {
                String Orsqlx = "select * from cadastro.relatorio order by str_to_date(dia,'%d/%m/%Y') asc";
                MySqlCommand OrComando1x = new MySqlCommand(Orsqlx, Conexao);
                OrComando1x.ExecuteNonQuery();

                MySqlDataAdapter objAdapter = new MySqlDataAdapter(Comando);
                DataTable dtlista = new DataTable();
                objAdapter.Fill(dtlista);
                MySqlDataReader leitor = Comando.ExecuteReader();
                while (leitor.Read())
                {
                    comboBox1.Items.Add(leitor["Dia"].ToString());
                    comboBox2.Items.Add(leitor["Dia"].ToString());

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na conexão MySql:" + ex.Message);

            }


            Conexao.Close();
        }




        private void Pesquisar_data2()
        {


            String Dia1 = comboBox1.SelectedItem.ToString();
            String Dia2 = comboBox2.SelectedItem.ToString();

            DataTable produtos = new DataTable();
            DataTable produtos2 = new DataTable();


            DataTable db2 = new DataTable();
            db2.Columns.Add("Total", typeof(double));
            db2.Columns.Add("Produto", typeof(String));

            DataTable ProdutosSoma = new DataTable();
            ProdutosSoma.Columns.Add("Produto", typeof(String));
            ProdutosSoma.Columns.Add("Total", typeof(double));

            String[] ProdCamp = new String[66];

            ProdCamp[0] = "Prod_1";
            ProdCamp[1] = "Prod_2";
            ProdCamp[2] = "Prod_3";
            ProdCamp[3] = "Prod_4";
            ProdCamp[4] = "Prod_5";
            ProdCamp[5] = "Prod_6";
            ProdCamp[6] = "Prod_7";
            ProdCamp[7] = "Prod_8";
            ProdCamp[8] = "Prod_9";
            ProdCamp[9] = "Prod_10";
            ProdCamp[10] = "Prod_11";
            ProdCamp[11] = "Prod_12";
            ProdCamp[12] = "Prod_13";
            ProdCamp[13] = "Prod_14";
            ProdCamp[14] = "Prod_15";
            ProdCamp[15] = "Prod_16";
            ProdCamp[16] = "Prod_17";
            ProdCamp[17] = "Prod_18";
            ProdCamp[18] = "Prod_19";
            ProdCamp[19] = "Prod_20";
            ProdCamp[20] = "Prod_21";
            ProdCamp[21] = "Prod_22";
            ProdCamp[22] = "Prod_23";
            ProdCamp[23] = "Prod_24";
            ProdCamp[24] = "Prod_25";
            ProdCamp[25] = "Prod_26";
            ProdCamp[26] = "Prod_27";
            ProdCamp[27] = "Prod_28";
            ProdCamp[28] = "Prod_29";
            ProdCamp[29] = "Prod_30";
            ProdCamp[30] = "Prod_31";
            ProdCamp[31] = "Prod_32";
            ProdCamp[32] = "Prod_33";
            ProdCamp[33] = "Prod_34";
            ProdCamp[34] = "Prod_35";
            ProdCamp[35] = "Prod_36";
            ProdCamp[36] = "Prod_37";
            ProdCamp[37] = "Prod_38";
            ProdCamp[38] = "Prod_39";
            ProdCamp[39] = "Prod_40";
            ProdCamp[40] = "Prod_41";
            ProdCamp[41] = "Prod_42";
            ProdCamp[42] = "Prod_43";
            ProdCamp[43] = "Prod_44";
            ProdCamp[44] = "Prod_45";
            ProdCamp[45] = "Prod_46";
            ProdCamp[46] = "Prod_47";
            ProdCamp[47] = "Prod_48";
            ProdCamp[48] = "Prod_49";
            ProdCamp[49] = "Prod_50";
            ProdCamp[50] = "Prod_51";
            ProdCamp[51] = "Prod_52";
            ProdCamp[52] = "Prod_53";
            ProdCamp[53] = "Prod_54";
            ProdCamp[54] = "Prod_55";
            ProdCamp[55] = "Prod_56";
            ProdCamp[56] = "Prod_57";
            ProdCamp[57] = "Prod_58";
            ProdCamp[58] = "Prod_59";
            ProdCamp[59] = "Prod_60";
            ProdCamp[60] = "Prod_61";
            ProdCamp[61] = "Prod_62";
            ProdCamp[62] = "Prod_63";
            ProdCamp[63] = "Prod_64";
            ProdCamp[64] = "Prod_65";


            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);

            String Sql = "Select * from relatorio where (str_to_date(dia,'%d/%m/%Y') >= str_to_date(@data1,'%d/%m/%Y')) and (str_to_date(dia,'%d/%m/%Y') <= str_to_date(@data2,'%d/%m/%Y'))";
            MySqlCommand Comando = new MySqlCommand(Sql, Conexao);
            Comando.Parameters.AddWithValue("@data1", Dia1);
            Comando.Parameters.AddWithValue("@data2", Dia2);
            Conexao.Open();

            try
            {

                MySqlDataAdapter objAdapter = new MySqlDataAdapter(Comando);
                objAdapter.Fill(produtos);
                objAdapter.Fill(produtos2);

                int Linhas = produtos.Rows.Count;
                Linhas = Linhas - 1;

                produtos.Columns[2].ColumnName = "Nome Formula";
                produtos.Columns[3].ColumnName = "Cód. Prog";
                produtos.Columns[4].ColumnName = "Cód. Form";
               
                String[] nomeColuna = new String[66];
                String[] nMedida = new String[66];
                Object[] SomaP = new Object[66];

                Object[] sumObject = new Object[66];
                if (produtos.Rows.Count > 0)
                {
                    for (int i = 0; i < 65; i++)
                    {
                        sumObject[i] = produtos.Compute("Sum(" + ProdCamp[i] + ")", "");

                        if ((DBNull.Value.Equals(sumObject[i])) || (Convert.ToInt64(sumObject[i]) == 0))
                        {
                            produtos.Columns.Remove(ProdCamp[i]);
                        }
                        else
                        {
                            nomeColuna[i] = TabelasT.NomeProduto.Rows[i]["Produto"].ToString();
                            nMedida[i] = TabelasT.NomeProduto.Rows[i]["Medida"].ToString();
                            produtos.Columns.Add(nomeColuna[i], typeof(double));
                            produtos.Columns[nomeColuna[i]].Expression = ProdCamp[i] + "/" + nMedida[i];
                        }
                    }

                    for (int i = 0; i < 65; i++)
                    {
                        SomaP[i] = produtos2.Compute("Sum(" + ProdCamp[i] + ")", "");
                        if (!DBNull.Value.Equals(SomaP[i]))
                        {
                            if (Convert.ToInt64(SomaP[i]) != 0)
                            {
                                

                                    
                                        Double MedidaSoma = Convert.ToInt16(TabelasT.NomeProduto.Rows[i]["Medida"]);
                                        DataRow Row1 = ProdutosSoma.NewRow();
                                        Row1["Produto"] = TabelasT.NomeProduto.Rows[i]["Produto"].ToString();
                                        Double ValorProd = Convert.ToDouble(SomaP[i]) / MedidaSoma;
                                        Row1["Total"] = ValorProd;
                                        ProdutosSoma.Rows.Add(Row1);
                            }
                            }
                        }
                    
                   
                }
            }
            catch (MySqlException erro)
            {

                MessageBox.Show("Erro na Conexão: " + erro);
            }

            Conexao.Close();
            dataGridView1.DataSource = produtos;
            dataGridView2.DataSource = ProdutosSoma;
            dataGridView2.Columns["Total"].DefaultCellStyle.Format = "#,##0.000";
            dataGridView2.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            int Linhas2 = produtos.Rows.Count;
            Linhas2 = Linhas2 - 1;
            InicalBox.Text = produtos.Rows[0]["Hora"].ToString();
            TabelasT.DHoraInicio = produtos.Rows[0]["Hora"].ToString(); //Relatorio Impresso

            FinalBox.Text = produtos.Rows[Linhas2]["Hora"].ToString();
            TabelasT.DHoraFinal = produtos.Rows[Linhas2]["Hora"].ToString();

            object sumTotal1;
            sumTotal1 = ProdutosSoma.Compute("Sum(Total)", " ");
            double ToTalTot1 = Convert.ToDouble(sumTotal1);
            

            TotalBox.Text = ToTalTot1.ToString("#,##0.000");
            TabelasT.DTotal1 = ToTalTot1.ToString("#,##0.000");

            TBatidaBox.Text = produtos.Rows.Count.ToString("#,#");
            TabelasT.DBatida1 = produtos.Rows.Count.ToString("#,#");

            TabelasT.DDataInicial = Dia1;
            TabelasT.DDataFinal = Dia2;

            TabelasT.DataTable3 = ProdutosSoma;
            

            for (int t = 0; t < 65; t++)
            {
                if (dataGridView1.Columns[ProdCamp[t]] != null)
                {
                    dataGridView1.Columns[ProdCamp[t]].Visible = false;
                }
             
            }


            int colunas = dataGridView1.Columns.Count;

            for (int j = 5; j < colunas; j++)
            {
                if (dataGridView1.Columns[j] != null)
                {
                    dataGridView1.Columns[j].DefaultCellStyle.Format = "#,##0.000";
                }

            }
                
            dataGridView1.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns[0].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[1].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[2].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[3].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns[4].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


        }

        private void Pesquisar3_Click(object sender, EventArgs e)
        {
            DateTime Data1;
            DateTime Data2;


            Data1 = Convert.ToDateTime(comboBox1.SelectedItem);
            Data2 = Convert.ToDateTime(comboBox2.SelectedItem);

            if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null)
            {
                MessageBox.Show("Selecione uma data!");
            }
            else
            {
                Data1 = Convert.ToDateTime(comboBox1.SelectedItem);
                Data2 = Convert.ToDateTime(comboBox2.SelectedItem);
                if (Data1 <= Data2)
                {
                    Pesquisar3.Enabled = false;
                    Pesquisar_data2();
                    Pesquisar3.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Intervalo de datas incorreto!");
                }

            }

        }
    

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true) { ObsBox.Enabled = true; }
            else
            {
                ObsBox.Clear();
                ObsBox.Enabled = false;
            }
        }
        private void Atualizar3_Click(object sender, EventArgs e)
        {
            
        }
        
        private void SomeBotoesAtua()
        {
            
        }

        public void Conferindo_Arquivo()
        {
            
        }

        public void Atualizar_ftpmes()
        {

        }

        public void Atualizando_3Arquivos()
        {
           
        }

        private void Print2_Click(object sender, EventArgs e)
        {
            if (TotalBox.Text == "")
            {
                MessageBox.Show("Sem dados para Impressão!");
            }
            else
            {

                if (checkBox1.Checked == true)
                {
                    TabelasT.DObs = "OBS: " + ObsBox.Text.ToString();
                }
                else
                {
                    TabelasT.DObs = null;
                }


                RelatorioData Imprimir = new RelatorioData();
                Imprimir.ShowDialog();
            }
        }
        private void ComboDHAtual_Click(object sender, EventArgs e)
        {
            CarregaComboApenas();
        }

        // Atualização ---------------------------------------------------------------------------------------

        public void Tempo1() //Chamar Atualização de Relatorio.
        {
           
        }

        public void Time_Elapsed1(object sender, ElapsedEventArgs e)
        {
            
        }


        //Atualiza o arquivo pronto com caminho
        public void Update1()
        {

           

        }
        delegate void Aguarda1();
        private void Atualizar_Some1()
        {
           
        }
        private void Atualizar_aparece1()
        {
            
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            
        }
        public void ChooseFolder()
        {
            
        }
        public void Procurando_Arquivo()
        {
        }
        public void GravarCaminho()
        {
          
        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void criar_textoInfo()
        {
           
        }

        private void radioftp_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void radiolocal_CheckedChanged(object sender, EventArgs e)
        {
        
        }

        private void radiopadrao_CheckedChanged(object sender, EventArgs e)
        {
       
        }

        private void radiodata_CheckedChanged(object sender, EventArgs e)
        {
         
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FtpInformacao novo = new FtpInformacao();
            novo.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {

        }
    }
}