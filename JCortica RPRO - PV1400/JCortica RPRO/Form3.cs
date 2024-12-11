using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Microsoft.Reporting.WinForms;
using System.Timers;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace JCortica_RPRO
{


    public partial class Form3 : Form
    {
        public DateTime Dt1_1 { get; set; }
        public DateTime Dt2_1 { get; set; }
        public String File_1 { get; set; }
        public String File_2 { get; set; }
        public String File_3 { get; set; }
        public DateTime Dt1 { get; set; }
        public DateTime Dt2 { get; set; }
        public bool TravaAtualiza = false;
        public String Atual_file { get; set; }
        public String caminho_file { get; set; }


        public Form3()
        {
            InitializeComponent();

        }
        private void Form3_Load(object sender, EventArgs e)
        {
            CarregaCombobox();
            ObsBox.Enabled = false;
            checkBox1.Checked = false;
            radioButton2.Checked = true;
            radioButton3.Checked = true;
            TabelasT.VisivelRelat = "0";
            SomeFormBox();

            bool XFormula = Properties.Settings.Default.FormulaCSV;

            if (XFormula == false)
            {
                tabControl1.TabPages.Remove(tabPage3);
                HabFormIdeal.Visible = false;
            }
        }
        public void SomeFormBox()
        {
            if (HabFormIdeal.Checked == false)
            {
                Totalbox2.Visible = false;
                label4.Visible = false;
            }
            else
            {
                Totalbox2.Visible = true;
                label4.Visible = true;
            }
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


            String SqlBanco = "Select * from materiaprima";
            MySqlCommand ComNomeProd = new MySqlCommand(SqlBanco, Conexao);
            Conexao.Open();
            MySqlDataAdapter NomeAdapter = new MySqlDataAdapter(ComNomeProd);
            DataTable Novo = new DataTable();
            NomeAdapter.Fill(Novo);
            Conexao.Close();
            TabelasT.NomeProduto = Novo;
        }
        private void CarregaCombobox_At()
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



            if (comboBox1.Items.Count > 0) { comboBox1.Items.Clear(); }
            if (comboBox2.Items.Count > 0) { comboBox2.Items.Clear(); }

            Conexao.Open();

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
        public void Pesquisar()
        {
            int IDForm;

            String Dia1 = comboBox1.SelectedItem.ToString();
            String Dia2 = comboBox2.SelectedItem.ToString();




            DataTable tabela = new DataTable();
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("Total", typeof(double));


            DataSet1 M = new DataSet1();

            DataTable db1 = new DataTable();
            db1.Columns.Add("Numero", typeof(ulong));
            db1.Columns.Add("Total", typeof(double));
            db1.Columns.Add("Nome", typeof(string));


            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);

            Conexao.Open();

            try
            {
                String SqlForm;

                if (radioButton1.Checked == true)
                {
                    SqlForm = "Select Distinct Form1 from relatorio";
                }
                else
                {
                    SqlForm = "Select Distinct Form2 from relatorio";
                }

                MySqlCommand FormNumero = new MySqlCommand(SqlForm, Conexao);
                MySqlDataAdapter NomeAdapter = new MySqlDataAdapter(FormNumero);
                DataTable IDTab = new DataTable();
                NomeAdapter.Fill(IDTab);




                //String TTexto = "Select Max(Prod_1) from cadastro.relatorio";
                //MySqlCommand FormMax = new MySqlCommand(TTexto, Conexao);

                //IDForm = Convert.ToInt16(FormMax.ExecuteScalar());

                String[] ProdCamp = new String[41];
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



                IDForm = IDTab.Rows.Count - 1;


                for (int i = 0; i <= IDForm; ++i)
                {
                    ulong NumForm = Convert.ToUInt64(IDTab.Rows[i][0]);

                    String Sql;
                    if (radioButton1.Checked == true)
                    {
                        Sql = "Select * from relatorio where (Form1 = @valor) and (str_to_date(dia,'%d/%m/%Y') >= str_to_date(@data1,'%d/%m/%Y')) and (str_to_date(dia,'%d/%m/%Y') <= str_to_date(@data2,'%d/%m/%Y'))";
                    }
                    else
                    {
                        Sql = "Select * from relatorio where (Form2 = @valor) and (str_to_date(dia,'%d/%m/%Y') >= str_to_date(@data1,'%d/%m/%Y')) and (str_to_date(dia,'%d/%m/%Y') <= str_to_date(@data2,'%d/%m/%Y'))";
                    }
                    MySqlCommand Comando = new MySqlCommand(Sql, Conexao);
                    Comando.Parameters.AddWithValue("@valor", NumForm);
                    Comando.Parameters.AddWithValue("@data1", Dia1);
                    Comando.Parameters.AddWithValue("@data2", Dia2);



                    MySqlDataAdapter objAdapter = new MySqlDataAdapter(Comando);
                    DataTable dtlista = new DataTable();
                    objAdapter.Fill(dtlista);
                    if (dtlista.Rows.Count > 0)
                    {
                        Double Totals = 0;
                        for (int t = 0; t <= 31; t++)
                        {
                            Object sumObject1;
                            String Texto1 = "Sum(" + ProdCamp[t] + ")";
                            if (!DBNull.Value.Equals(dtlista.Compute(Texto1, "")))
                            {
                                Double Medida = Convert.ToDouble(TabelasT.NomeProduto.Rows[t]["Medida"].ToString());
                                sumObject1 = dtlista.Compute(Texto1, "");
                                Double ValorLinha;
                                ValorLinha = Convert.ToDouble(sumObject1) / Medida;

                                Totals = Totals + ValorLinha;

                            }

                        }
                        Object Name;
                        Name = dtlista.Rows[0]["Nome"].ToString();

                        DataRow Row = db1.NewRow();
                        Row["Numero"] = NumForm;
                        Row["Total"] = Totals;
                        Row["Nome"] = Name;
                        db1.Rows.Add(Row);
                    }

                }
            }
            catch (MySqlException erro)
            {

                MessageBox.Show("Erro na Conexão:" + erro);
            }


            Conexao.Close();


            dataGridView1.DataSource = db1;
            dataGridView1.Columns["Numero"].DisplayIndex = 0;
            dataGridView1.Columns["Nome"].DisplayIndex = 1;
            dataGridView1.Columns["Total"].DisplayIndex = 2;
            //dataGridView1.Columns["Numero"].Width = 50;
            //dataGridView1.Columns["Nome"].Width = 150;
            //dataGridView1.Columns["Total"].Width = 100;


            dataGridView1.Columns["Total"].DefaultCellStyle.Format = "#,##0.000";
            dataGridView1.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            TabelasT.DataTable1 = db1;


        }
        public void Pesquisar_Individual_1()
        {
            int IDForm;

            String Dia1 = comboBox1.SelectedItem.ToString();
            String Dia2 = comboBox2.SelectedItem.ToString();

            DataTable tabela = new DataTable();
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("Total", typeof(double));

            DataSet1 M = new DataSet1();

            DataTable db1 = new DataTable();
            db1.Columns.Add("Numero", typeof(ulong));
            db1.Columns.Add("Total", typeof(double));
            db1.Columns.Add("Nome", typeof(string));


            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);

            Conexao.Open();

            try
            {
                String SqlForm;

                if (radioButton1.Checked == true)
                {
                    SqlForm = "Select Distinct Form1 from relatorio where (str_to_date(dia,'%d/%m/%Y') >= str_to_date(@data1,'%d/%m/%Y')) and (str_to_date(dia,'%d/%m/%Y') <= str_to_date(@data2,'%d/%m/%Y'))";
                }
                else
                {
                    SqlForm = "Select Distinct Form2 from relatorio where (str_to_date(dia,'%d/%m/%Y') >= str_to_date(@data1,'%d/%m/%Y')) and (str_to_date(dia,'%d/%m/%Y') <= str_to_date(@data2,'%d/%m/%Y'))";
                }
                MySqlCommand FormNumero = new MySqlCommand(SqlForm, Conexao);
                FormNumero.Parameters.AddWithValue("@data1", Dia1);
                FormNumero.Parameters.AddWithValue("@data2", Dia2);



                MySqlDataAdapter NomeAdapter = new MySqlDataAdapter(FormNumero);
                DataTable IDTab = new DataTable();
                NomeAdapter.Fill(IDTab);




                //String TTexto = "Select Max(Prod_1) from cadastro.relatorio";
                //MySqlCommand FormMax = new MySqlCommand(TTexto, Conexao);

                //IDForm = Convert.ToInt16(FormMax.ExecuteScalar());

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
                


                labelform.Visible = true;
                FormBox.Visible = true;

                if (FormBox.Items.Count > 0) { FormBox.Items.Clear(); }



                MySqlDataReader leitor = FormNumero.ExecuteReader();
                while (leitor.Read())
                {

                    if (radioButton1.Checked == true)
                    {
                        FormBox.Items.Add(leitor["Form1"].ToString());
                    }
                    else
                    {
                        FormBox.Items.Add(leitor["Form2"].ToString());
                    }
                }

            }
            catch (MySqlException erro)
            {

                MessageBox.Show("Erro na Conexão:" + erro);
            }


            Conexao.Close();

        }
        private void Pesquisar_Individual_3()
        {

            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            tabelagrid.DataSource = null;

            String[] ProdCamp3 = new String[66];
            String[] ProdCamp4 = new String[66];

            String BatidasStr;

            ProdCamp3[0] = "Prod_1I";
            ProdCamp3[1] = "Prod_2I";
            ProdCamp3[2] = "Prod_3I";
            ProdCamp3[3] = "Prod_4I";
            ProdCamp3[4] = "Prod_5I";
            ProdCamp3[5] = "Prod_6I";
            ProdCamp3[6] = "Prod_7I";
            ProdCamp3[7] = "Prod_8I";
            ProdCamp3[8] = "Prod_9I";
            ProdCamp3[9] = "Prod_10I";
            ProdCamp3[10] = "Prod_11I";
            ProdCamp3[11] = "Prod_12I";
            ProdCamp3[12] = "Prod_13I";
            ProdCamp3[13] = "Prod_14I";
            ProdCamp3[14] = "Prod_15I";
            ProdCamp3[15] = "Prod_16I";
            ProdCamp3[16] = "Prod_17I";
            ProdCamp3[17] = "Prod_18I";
            ProdCamp3[18] = "Prod_19I";
            ProdCamp3[19] = "Prod_20I";
            ProdCamp3[20] = "Prod_21I";
            ProdCamp3[21] = "Prod_22I";
            ProdCamp3[22] = "Prod_23I";
            ProdCamp3[23] = "Prod_24I";
            ProdCamp3[24] = "Prod_25I";
            ProdCamp3[25] = "Prod_26I";
            ProdCamp3[26] = "Prod_27I";
            ProdCamp3[27] = "Prod_28I";
            ProdCamp3[28] = "Prod_29I";
            ProdCamp3[29] = "Prod_30I";
            ProdCamp3[30] = "Prod_31I";
            ProdCamp3[31] = "Prod_32I";
            ProdCamp3[32] = "Prod_33I";
            ProdCamp3[33] = "Prod_34I";
            ProdCamp3[34] = "Prod_35I";
            ProdCamp3[35] = "Prod_36I";
            ProdCamp3[36] = "Prod_37I";
            ProdCamp3[37] = "Prod_38I";
            ProdCamp3[38] = "Prod_39I";
            ProdCamp3[39] = "Prod_40I";
            ProdCamp3[40] = "Prod_41I";
            ProdCamp3[41] = "Prod_42I";
            ProdCamp3[42] = "Prod_43I";
            ProdCamp3[43] = "Prod_44I";
            ProdCamp3[44] = "Prod_45I";
            ProdCamp3[45] = "Prod_46I";
            ProdCamp3[46] = "Prod_47I";
            ProdCamp3[47] = "Prod_48I";
            ProdCamp3[48] = "Prod_49I";
            ProdCamp3[49] = "Prod_50I";
            ProdCamp3[50] = "Prod_51I";
            ProdCamp3[51] = "Prod_52I";
            ProdCamp3[52] = "Prod_53I";
            ProdCamp3[53] = "Prod_54I";
            ProdCamp3[54] = "Prod_55I";
            ProdCamp3[55] = "Prod_56I";
            ProdCamp3[56] = "Prod_57I";
            ProdCamp3[57] = "Prod_58I";
            ProdCamp3[58] = "Prod_59I";
            ProdCamp3[59] = "Prod_60I";
            ProdCamp3[60] = "Prod_61I";
            ProdCamp3[61] = "Prod_62I";
            ProdCamp3[62] = "Prod_63I";
            ProdCamp3[63] = "Prod_64I";
            ProdCamp3[64] = "Prod_65I";
           

            ProdCamp4[0] = "Prod_1R";
            ProdCamp4[1] = "Prod_2R";
            ProdCamp4[2] = "Prod_3R";
            ProdCamp4[3] = "Prod_4R";
            ProdCamp4[4] = "Prod_5R";
            ProdCamp4[5] = "Prod_6R";
            ProdCamp4[6] = "Prod_7R";
            ProdCamp4[7] = "Prod_8R";
            ProdCamp4[8] = "Prod_9R";
            ProdCamp4[9] = "Prod_10R";
            ProdCamp4[10] = "Prod_11R";
            ProdCamp4[11] = "Prod_12R";
            ProdCamp4[12] = "Prod_13R";
            ProdCamp4[13] = "Prod_14R";
            ProdCamp4[14] = "Prod_15R";
            ProdCamp4[15] = "Prod_16R";
            ProdCamp4[16] = "Prod_17R";
            ProdCamp4[17] = "Prod_18R";
            ProdCamp4[18] = "Prod_19R";
            ProdCamp4[19] = "Prod_20R";
            ProdCamp4[20] = "Prod_21R";
            ProdCamp4[21] = "Prod_22R";
            ProdCamp4[22] = "Prod_23R";
            ProdCamp4[23] = "Prod_24R";
            ProdCamp4[24] = "Prod_25R";
            ProdCamp4[25] = "Prod_26R";
            ProdCamp4[26] = "Prod_27R";
            ProdCamp4[27] = "Prod_28R";
            ProdCamp4[28] = "Prod_29R";
            ProdCamp4[29] = "Prod_30R";
            ProdCamp4[30] = "Prod_31R";
            ProdCamp4[31] = "Prod_32R";
            ProdCamp4[32] = "Prod_33R";
            ProdCamp4[33] = "Prod_34R";
            ProdCamp4[34] = "Prod_35R";
            ProdCamp4[35] = "Prod_36R";
            ProdCamp4[36] = "Prod_37R";
            ProdCamp4[37] = "Prod_38R";
            ProdCamp4[38] = "Prod_39R";
            ProdCamp4[39] = "Prod_40R";
            ProdCamp4[40] = "Prod_41R";
            ProdCamp4[41] = "Prod_42R";
            ProdCamp4[42] = "Prod_43R";
            ProdCamp4[43] = "Prod_44R";
            ProdCamp4[44] = "Prod_45R";
            ProdCamp4[45] = "Prod_46R";
            ProdCamp4[46] = "Prod_47R";
            ProdCamp4[47] = "Prod_48R";
            ProdCamp4[48] = "Prod_49R";
            ProdCamp4[49] = "Prod_50R";
            ProdCamp4[50] = "Prod_51R";
            ProdCamp4[51] = "Prod_52R";
            ProdCamp4[52] = "Prod_53R";
            ProdCamp4[53] = "Prod_54R";
            ProdCamp4[54] = "Prod_55R";
            ProdCamp4[55] = "Prod_56R";
            ProdCamp4[56] = "Prod_57R";
            ProdCamp4[57] = "Prod_58R";
            ProdCamp4[58] = "Prod_59R";
            ProdCamp4[59] = "Prod_60R";
            ProdCamp4[60] = "Prod_61R";
            ProdCamp4[61] = "Prod_62R";
            ProdCamp4[62] = "Prod_63R";
            ProdCamp4[63] = "Prod_64R";
            ProdCamp4[64] = "Prod_65R";
           


            String Dia1 = comboBox1.SelectedItem.ToString();
            String Dia2 = comboBox2.SelectedItem.ToString();

            DataTable resultadoForm = new DataTable();
            resultadoForm.Columns.Add("Dia", typeof(string));
            resultadoForm.Columns.Add("Hora", typeof(string));
            resultadoForm.Columns.Add("Nome", typeof(string));
            resultadoForm.Columns.Add("Form1", typeof(int));
            resultadoForm.Columns.Add("Form2", typeof(int));
            resultadoForm.Columns.Add("Prod_1", typeof(int));
            resultadoForm.Columns.Add("Prod_2", typeof(int));
            resultadoForm.Columns.Add("Prod_3", typeof(int));
            resultadoForm.Columns.Add("Prod_4", typeof(int));
            resultadoForm.Columns.Add("Prod_5", typeof(int));
            resultadoForm.Columns.Add("Prod_6", typeof(int));
            resultadoForm.Columns.Add("Prod_7", typeof(int));
            resultadoForm.Columns.Add("Prod_8", typeof(int));
            resultadoForm.Columns.Add("Prod_9", typeof(int));
            resultadoForm.Columns.Add("Prod_10", typeof(int));
            resultadoForm.Columns.Add("Prod_11", typeof(int));
            resultadoForm.Columns.Add("Prod_12", typeof(int));
            resultadoForm.Columns.Add("Prod_13", typeof(int));
            resultadoForm.Columns.Add("Prod_14", typeof(int));
            resultadoForm.Columns.Add("Prod_15", typeof(int));
            resultadoForm.Columns.Add("Prod_16", typeof(int));
            resultadoForm.Columns.Add("Prod_17", typeof(int));
            resultadoForm.Columns.Add("Prod_18", typeof(int));
            resultadoForm.Columns.Add("Prod_19", typeof(int));
            resultadoForm.Columns.Add("Prod_20", typeof(int));
            resultadoForm.Columns.Add("Prod_21", typeof(int));
            resultadoForm.Columns.Add("Prod_22", typeof(int));
            resultadoForm.Columns.Add("Prod_23", typeof(int));
            resultadoForm.Columns.Add("Prod_24", typeof(int));
            resultadoForm.Columns.Add("Prod_25", typeof(int));
            resultadoForm.Columns.Add("Prod_26", typeof(int));
            resultadoForm.Columns.Add("Prod_27", typeof(int));
            resultadoForm.Columns.Add("Prod_28", typeof(int));
            resultadoForm.Columns.Add("Prod_29", typeof(int));
            resultadoForm.Columns.Add("Prod_30", typeof(int));
            resultadoForm.Columns.Add("Prod_31", typeof(int));
            resultadoForm.Columns.Add("Prod_32", typeof(int));
            resultadoForm.Columns.Add("Prod_33", typeof(int));
            resultadoForm.Columns.Add("Prod_34", typeof(int));
            resultadoForm.Columns.Add("Prod_35", typeof(int));
            resultadoForm.Columns.Add("Prod_36", typeof(int));
            resultadoForm.Columns.Add("Prod_37", typeof(int));
            resultadoForm.Columns.Add("Prod_38", typeof(int));
            resultadoForm.Columns.Add("Prod_39", typeof(int));
            resultadoForm.Columns.Add("Prod_40", typeof(int));
            resultadoForm.Columns.Add("Prod_41", typeof(int));
            resultadoForm.Columns.Add("Prod_42", typeof(int));
            resultadoForm.Columns.Add("Prod_43", typeof(int));
            resultadoForm.Columns.Add("Prod_44", typeof(int));
            resultadoForm.Columns.Add("Prod_45", typeof(int));
            resultadoForm.Columns.Add("Prod_46", typeof(int));
            resultadoForm.Columns.Add("Prod_47", typeof(int));
            resultadoForm.Columns.Add("Prod_48", typeof(int));
            resultadoForm.Columns.Add("Prod_49", typeof(int));
            resultadoForm.Columns.Add("Prod_50", typeof(int));
            resultadoForm.Columns.Add("Prod_51", typeof(int));
            resultadoForm.Columns.Add("Prod_52", typeof(int));
            resultadoForm.Columns.Add("Prod_53", typeof(int));
            resultadoForm.Columns.Add("Prod_54", typeof(int));
            resultadoForm.Columns.Add("Prod_55", typeof(int));
            resultadoForm.Columns.Add("Prod_56", typeof(int));
            resultadoForm.Columns.Add("Prod_57", typeof(int));
            resultadoForm.Columns.Add("Prod_58", typeof(int));
            resultadoForm.Columns.Add("Prod_59", typeof(int));
            resultadoForm.Columns.Add("Prod_60", typeof(int));
            resultadoForm.Columns.Add("Prod_61", typeof(int));
            resultadoForm.Columns.Add("Prod_62", typeof(int));
            resultadoForm.Columns.Add("Prod_63", typeof(int));
            resultadoForm.Columns.Add("Prod_64", typeof(int));
            resultadoForm.Columns.Add("Prod_65", typeof(int));
          

            DataTable FinalForm = new DataTable();


            FinalForm.Columns.Add("DiaR", typeof(string));
            FinalForm.Columns.Add("HoraR", typeof(string));
            FinalForm.Columns.Add("NomeR", typeof(string));

            FinalForm.Columns.Add("DiaI", typeof(string));
            FinalForm.Columns.Add("HoraI", typeof(string));
            FinalForm.Columns.Add("NomeI", typeof(string));

            FinalForm.Columns.Add("Form1R", typeof(int));
            FinalForm.Columns.Add("Form1I", typeof(int));
            FinalForm.Columns.Add("Form2R", typeof(int));
            FinalForm.Columns.Add("Form2I", typeof(int));
            FinalForm.Columns.Add("Prod_1R", typeof(int));
            FinalForm.Columns.Add("Prod_1I", typeof(int));
            FinalForm.Columns.Add("Prod_2R", typeof(int));
            FinalForm.Columns.Add("Prod_2I", typeof(int));
            FinalForm.Columns.Add("Prod_3R", typeof(int));
            FinalForm.Columns.Add("Prod_3I", typeof(int));
            FinalForm.Columns.Add("Prod_4R", typeof(int));
            FinalForm.Columns.Add("Prod_4I", typeof(int));
            FinalForm.Columns.Add("Prod_5R", typeof(int));
            FinalForm.Columns.Add("Prod_5I", typeof(int));
            FinalForm.Columns.Add("Prod_6R", typeof(int));
            FinalForm.Columns.Add("Prod_6I", typeof(int));
            FinalForm.Columns.Add("Prod_7R", typeof(int));
            FinalForm.Columns.Add("Prod_7I", typeof(int));
            FinalForm.Columns.Add("Prod_8R", typeof(int));
            FinalForm.Columns.Add("Prod_8I", typeof(int));
            FinalForm.Columns.Add("Prod_9R", typeof(int));
            FinalForm.Columns.Add("Prod_9I", typeof(int));
            FinalForm.Columns.Add("Prod_10R", typeof(int));
            FinalForm.Columns.Add("Prod_10I", typeof(int));
            FinalForm.Columns.Add("Prod_11R", typeof(int));
            FinalForm.Columns.Add("Prod_11I", typeof(int));
            FinalForm.Columns.Add("Prod_12R", typeof(int));
            FinalForm.Columns.Add("Prod_12I", typeof(int));
            FinalForm.Columns.Add("Prod_13R", typeof(int));
            FinalForm.Columns.Add("Prod_13I", typeof(int));
            FinalForm.Columns.Add("Prod_14R", typeof(int));
            FinalForm.Columns.Add("Prod_14I", typeof(int));
            FinalForm.Columns.Add("Prod_15R", typeof(int));
            FinalForm.Columns.Add("Prod_15I", typeof(int));
            FinalForm.Columns.Add("Prod_16R", typeof(int));
            FinalForm.Columns.Add("Prod_16I", typeof(int));
            FinalForm.Columns.Add("Prod_17R", typeof(int));
            FinalForm.Columns.Add("Prod_17I", typeof(int));
            FinalForm.Columns.Add("Prod_18R", typeof(int));
            FinalForm.Columns.Add("Prod_18I", typeof(int));
            FinalForm.Columns.Add("Prod_19R", typeof(int));
            FinalForm.Columns.Add("Prod_19I", typeof(int));
            FinalForm.Columns.Add("Prod_20R", typeof(int));
            FinalForm.Columns.Add("Prod_20I", typeof(int));
            FinalForm.Columns.Add("Prod_21R", typeof(int));
            FinalForm.Columns.Add("Prod_21I", typeof(int));
            FinalForm.Columns.Add("Prod_22R", typeof(int));
            FinalForm.Columns.Add("Prod_22I", typeof(int));
            FinalForm.Columns.Add("Prod_23R", typeof(int));
            FinalForm.Columns.Add("Prod_23I", typeof(int));
            FinalForm.Columns.Add("Prod_24R", typeof(int));
            FinalForm.Columns.Add("Prod_24I", typeof(int));
            FinalForm.Columns.Add("Prod_25R", typeof(int));
            FinalForm.Columns.Add("Prod_25I", typeof(int));
            FinalForm.Columns.Add("Prod_26R", typeof(int));
            FinalForm.Columns.Add("Prod_26I", typeof(int));
            FinalForm.Columns.Add("Prod_27R", typeof(int));
            FinalForm.Columns.Add("Prod_27I", typeof(int));
            FinalForm.Columns.Add("Prod_28R", typeof(int));
            FinalForm.Columns.Add("Prod_28I", typeof(int));
            FinalForm.Columns.Add("Prod_29R", typeof(int));
            FinalForm.Columns.Add("Prod_29I", typeof(int));
            FinalForm.Columns.Add("Prod_30R", typeof(int));
            FinalForm.Columns.Add("Prod_30I", typeof(int));
            FinalForm.Columns.Add("Prod_31R", typeof(int));
            FinalForm.Columns.Add("Prod_31I", typeof(int));
            FinalForm.Columns.Add("Prod_32R", typeof(int));
            FinalForm.Columns.Add("Prod_32I", typeof(int));
            FinalForm.Columns.Add("Prod_33R", typeof(int));
            FinalForm.Columns.Add("Prod_33I", typeof(int));
            FinalForm.Columns.Add("Prod_34R", typeof(int));
            FinalForm.Columns.Add("Prod_34I", typeof(int));
            FinalForm.Columns.Add("Prod_35R", typeof(int));
            FinalForm.Columns.Add("Prod_35I", typeof(int));
            FinalForm.Columns.Add("Prod_36R", typeof(int));
            FinalForm.Columns.Add("Prod_36I", typeof(int));
            FinalForm.Columns.Add("Prod_37R", typeof(int));
            FinalForm.Columns.Add("Prod_37I", typeof(int));
            FinalForm.Columns.Add("Prod_38R", typeof(int));
            FinalForm.Columns.Add("Prod_38I", typeof(int));
            FinalForm.Columns.Add("Prod_39R", typeof(int));
            FinalForm.Columns.Add("Prod_39I", typeof(int));
            FinalForm.Columns.Add("Prod_40R", typeof(int));
            FinalForm.Columns.Add("Prod_40I", typeof(int));
            FinalForm.Columns.Add("Prod_41R", typeof(int));
            FinalForm.Columns.Add("Prod_41I", typeof(int));
            FinalForm.Columns.Add("Prod_42R", typeof(int));
            FinalForm.Columns.Add("Prod_42I", typeof(int));
            FinalForm.Columns.Add("Prod_43R", typeof(int));
            FinalForm.Columns.Add("Prod_43I", typeof(int));
            FinalForm.Columns.Add("Prod_44R", typeof(int));
            FinalForm.Columns.Add("Prod_44I", typeof(int));
            FinalForm.Columns.Add("Prod_45R", typeof(int));
            FinalForm.Columns.Add("Prod_45I", typeof(int));
            FinalForm.Columns.Add("Prod_46R", typeof(int));
            FinalForm.Columns.Add("Prod_46I", typeof(int));
            FinalForm.Columns.Add("Prod_47R", typeof(int));
            FinalForm.Columns.Add("Prod_47I", typeof(int));
            FinalForm.Columns.Add("Prod_48R", typeof(int));
            FinalForm.Columns.Add("Prod_48I", typeof(int));
            FinalForm.Columns.Add("Prod_49R", typeof(int));
            FinalForm.Columns.Add("Prod_49I", typeof(int));
            FinalForm.Columns.Add("Prod_50R", typeof(int));
            FinalForm.Columns.Add("Prod_50I", typeof(int));
            FinalForm.Columns.Add("Prod_51R", typeof(int));
            FinalForm.Columns.Add("Prod_51I", typeof(int));
            FinalForm.Columns.Add("Prod_52R", typeof(int));
            FinalForm.Columns.Add("Prod_52I", typeof(int));
            FinalForm.Columns.Add("Prod_53R", typeof(int));
            FinalForm.Columns.Add("Prod_53I", typeof(int));
            FinalForm.Columns.Add("Prod_54R", typeof(int));
            FinalForm.Columns.Add("Prod_54I", typeof(int));
            FinalForm.Columns.Add("Prod_55R", typeof(int));
            FinalForm.Columns.Add("Prod_55I", typeof(int));
            FinalForm.Columns.Add("Prod_56R", typeof(int));
            FinalForm.Columns.Add("Prod_56I", typeof(int));
            FinalForm.Columns.Add("Prod_57R", typeof(int));
            FinalForm.Columns.Add("Prod_57I", typeof(int));
            FinalForm.Columns.Add("Prod_58R", typeof(int));
            FinalForm.Columns.Add("Prod_58I", typeof(int));
            FinalForm.Columns.Add("Prod_59R", typeof(int));
            FinalForm.Columns.Add("Prod_59I", typeof(int));
            FinalForm.Columns.Add("Prod_60R", typeof(int));
            FinalForm.Columns.Add("Prod_60I", typeof(int));
            FinalForm.Columns.Add("Prod_61R", typeof(int));
            FinalForm.Columns.Add("Prod_61I", typeof(int));
            FinalForm.Columns.Add("Prod_62R", typeof(int));
            FinalForm.Columns.Add("Prod_62I", typeof(int));
            FinalForm.Columns.Add("Prod_63R", typeof(int));
            FinalForm.Columns.Add("Prod_63I", typeof(int));
            FinalForm.Columns.Add("Prod_64R", typeof(int));
            FinalForm.Columns.Add("Prod_64I", typeof(int));
            FinalForm.Columns.Add("Prod_65R", typeof(int));
            FinalForm.Columns.Add("Prod_65I", typeof(int));

            String NomedaFormulaX1;
            String NumerodaFormulaX1;
            String NumerodaFormulaX2;

            DataTable db2 = new DataTable();

            db2.Columns.Add("TotalF", typeof(double));
            db2.Columns.Add("TotalR", typeof(double));
            db2.Columns.Add("Produto", typeof(String));

            DataTable db3 = new DataTable();
            db3.Columns.Add("CProg", typeof(string));
            db3.Columns.Add("CCliente", typeof(string));
            db3.Columns.Add("Batida", typeof(string));
            db3.Columns.Add("Nome", typeof(string));
            db3.Columns.Add("TotalR", typeof(double));
            db3.Columns.Add("TotalF", typeof(double));




            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);

            String NumForm = FormBox.SelectedItem.ToString();

            String Sql = "";

            if (radioButton1.Checked == true)
            {
                Sql = "Select * from relatorio where (Form1 = @valor) and (str_to_date(dia,'%d/%m/%Y') >= str_to_date(@data1,'%d/%m/%Y')) and (str_to_date(dia,'%d/%m/%Y') <= str_to_date(@data2,'%d/%m/%Y'))";
            }
            else
            {
                Sql = "Select * from relatorio where (Form2 = @valor) and (str_to_date(dia,'%d/%m/%Y') >= str_to_date(@data1,'%d/%m/%Y')) and (str_to_date(dia,'%d/%m/%Y') <= str_to_date(@data2,'%d/%m/%Y'))";
            }

            MySqlCommand Comando = new MySqlCommand(Sql, Conexao);
            Comando.Parameters.AddWithValue("@valor", NumForm);
            Comando.Parameters.AddWithValue("@data1", Dia1);
            Comando.Parameters.AddWithValue("@data2", Dia2);
            Conexao.Open();

            try
            {

                MySqlDataAdapter objAdapter = new MySqlDataAdapter(Comando);
                DataTable produtos = new DataTable();
                objAdapter.Fill(produtos);


                int Linhas = produtos.Rows.Count;
                int LinhasHora = Linhas - 1;
                InicalBox.Text = produtos.Rows[0]["Hora"].ToString();
                FinalBox.Text = produtos.Rows[LinhasHora]["Hora"].ToString();

                NomedaFormulaX1 = produtos.Rows[0]["Nome"].ToString();
                NumerodaFormulaX1 = produtos.Rows[0]["Form1"].ToString();
                NumerodaFormulaX2 = produtos.Rows[0]["Form2"].ToString();


                for (int i = 0; i < Linhas; i++)
                {
                    String Formx1 = produtos.Rows[i]["Form1"].ToString();
                    String Formx2 = produtos.Rows[i]["Form2"].ToString();

                    String Data = produtos.Rows[i]["Dia"].ToString();
                    String Hora = produtos.Rows[i]["Hora"].ToString();

                    String DataeHora = Data + " " + Hora;

                    String Formula;
                    String Sql1X;

                    if (radioButton1.Checked == true)
                    {
                        Sql1X = "Select * from cadastro.formulaideal where (Form1=@form) and (str_to_date(@DataeHora, '%d/%m/%Y %H:%i:%s') >=  str_to_date(CONCAT(dia,\" \", Hora),'%d/%m/%Y %H:%i:%s'))";
                        Formula = Formx1;
                    }
                    else
                    {
                        Sql1X = "Select * from cadastro.formulaideal where (Form2=@form) and (str_to_date(@DataeHora, '%d/%m/%Y %H:%i:%s') >=  str_to_date(CONCAT(dia,\" \", Hora),'%d/%m/%Y %H:%i:%s'))";
                        Formula = Formx2;
                    }

                    MySqlCommand Comando1x = new MySqlCommand(Sql1X, Conexao);
                    Comando1x.Parameters.AddWithValue("@form", Formula);
                    Comando1x.Parameters.AddWithValue("@DataeHora", DataeHora);

                    MySqlDataAdapter objAdapter1 = new MySqlDataAdapter(Comando1x);
                    DataTable temporario = new DataTable();
                    objAdapter1.Fill(temporario);

                    int linhaOK = temporario.Rows.Count;
                    linhaOK = linhaOK - 1;

                    if (temporario.Rows.Count > 0)
                    {

                        DataRow Row = resultadoForm.NewRow();
                        Row["Dia"] = temporario.Rows[linhaOK]["Dia"];
                        Row["Hora"] = temporario.Rows[linhaOK]["Hora"];
                        Row["Nome"] = temporario.Rows[linhaOK]["Nome"];

                        Row["Form1"] = temporario.Rows[linhaOK]["Form1"];
                        Row["Form2"] = temporario.Rows[linhaOK]["Form2"];
                        Row["Prod_1"] = temporario.Rows[linhaOK]["Prod_1"];
                        Row["Prod_2"] = temporario.Rows[linhaOK]["Prod_2"];
                        Row["Prod_3"] = temporario.Rows[linhaOK]["Prod_3"];
                        Row["Prod_4"] = temporario.Rows[linhaOK]["Prod_4"];
                        Row["Prod_5"] = temporario.Rows[linhaOK]["Prod_5"];
                        Row["Prod_6"] = temporario.Rows[linhaOK]["Prod_6"];
                        Row["Prod_7"] = temporario.Rows[linhaOK]["Prod_7"];
                        Row["Prod_8"] = temporario.Rows[linhaOK]["Prod_8"];
                        Row["Prod_9"] = temporario.Rows[linhaOK]["Prod_9"];
                        Row["Prod_10"] = temporario.Rows[linhaOK]["Prod_10"];
                        Row["Prod_11"] = temporario.Rows[linhaOK]["Prod_11"];
                        Row["Prod_12"] = temporario.Rows[linhaOK]["Prod_12"];
                        Row["Prod_13"] = temporario.Rows[linhaOK]["Prod_13"];
                        Row["Prod_14"] = temporario.Rows[linhaOK]["Prod_14"];
                        Row["Prod_15"] = temporario.Rows[linhaOK]["Prod_15"];
                        Row["Prod_16"] = temporario.Rows[linhaOK]["Prod_16"];
                        Row["Prod_17"] = temporario.Rows[linhaOK]["Prod_17"];
                        Row["Prod_18"] = temporario.Rows[linhaOK]["Prod_18"];
                        Row["Prod_19"] = temporario.Rows[linhaOK]["Prod_19"];
                        Row["Prod_20"] = temporario.Rows[linhaOK]["Prod_20"];
                        Row["Prod_21"] = temporario.Rows[linhaOK]["Prod_21"];
                        Row["Prod_22"] = temporario.Rows[linhaOK]["Prod_22"];
                        Row["Prod_23"] = temporario.Rows[linhaOK]["Prod_23"];
                        Row["Prod_24"] = temporario.Rows[linhaOK]["Prod_24"];
                        Row["Prod_25"] = temporario.Rows[linhaOK]["Prod_25"];
                        Row["Prod_26"] = temporario.Rows[linhaOK]["Prod_26"];
                        Row["Prod_27"] = temporario.Rows[linhaOK]["Prod_27"];
                        Row["Prod_28"] = temporario.Rows[linhaOK]["Prod_28"];
                        Row["Prod_29"] = temporario.Rows[linhaOK]["Prod_29"];
                        Row["Prod_30"] = temporario.Rows[linhaOK]["Prod_30"];
                        Row["Prod_31"] = temporario.Rows[linhaOK]["Prod_31"];
                        Row["Prod_32"] = temporario.Rows[linhaOK]["Prod_32"];
                        Row["Prod_33"] = temporario.Rows[linhaOK]["Prod_33"];
                        Row["Prod_34"] = temporario.Rows[linhaOK]["Prod_34"];
                        Row["Prod_35"] = temporario.Rows[linhaOK]["Prod_35"];
                        Row["Prod_36"] = temporario.Rows[linhaOK]["Prod_36"];
                        Row["Prod_37"] = temporario.Rows[linhaOK]["Prod_37"];
                        Row["Prod_38"] = temporario.Rows[linhaOK]["Prod_38"];
                        Row["Prod_39"] = temporario.Rows[linhaOK]["Prod_39"];
                        Row["Prod_40"] = temporario.Rows[linhaOK]["Prod_40"];
                        Row["Prod_41"] = temporario.Rows[linhaOK]["Prod_41"];
                        Row["Prod_42"] = temporario.Rows[linhaOK]["Prod_42"];
                        Row["Prod_43"] = temporario.Rows[linhaOK]["Prod_43"];
                        Row["Prod_44"] = temporario.Rows[linhaOK]["Prod_44"];
                        Row["Prod_45"] = temporario.Rows[linhaOK]["Prod_45"];
                        Row["Prod_46"] = temporario.Rows[linhaOK]["Prod_46"];
                        Row["Prod_47"] = temporario.Rows[linhaOK]["Prod_47"];
                        Row["Prod_48"] = temporario.Rows[linhaOK]["Prod_48"];
                        Row["Prod_49"] = temporario.Rows[linhaOK]["Prod_49"];
                        Row["Prod_50"] = temporario.Rows[linhaOK]["Prod_50"];
                        Row["Prod_51"] = temporario.Rows[linhaOK]["Prod_51"];
                        Row["Prod_52"] = temporario.Rows[linhaOK]["Prod_52"];
                        Row["Prod_53"] = temporario.Rows[linhaOK]["Prod_53"];
                        Row["Prod_54"] = temporario.Rows[linhaOK]["Prod_54"];
                        Row["Prod_55"] = temporario.Rows[linhaOK]["Prod_55"];
                        Row["Prod_56"] = temporario.Rows[linhaOK]["Prod_56"];
                        Row["Prod_57"] = temporario.Rows[linhaOK]["Prod_57"];
                        Row["Prod_58"] = temporario.Rows[linhaOK]["Prod_58"];
                        Row["Prod_59"] = temporario.Rows[linhaOK]["Prod_59"];
                        Row["Prod_60"] = temporario.Rows[linhaOK]["Prod_60"];
                        Row["Prod_61"] = temporario.Rows[linhaOK]["Prod_61"];
                        Row["Prod_62"] = temporario.Rows[linhaOK]["Prod_62"];
                        Row["Prod_63"] = temporario.Rows[linhaOK]["Prod_63"];
                        Row["Prod_64"] = temporario.Rows[linhaOK]["Prod_64"];
                        Row["Prod_65"] = temporario.Rows[linhaOK]["Prod_65"];
                      
                        resultadoForm.Rows.Add(Row);

                        DataRow RowX = FinalForm.NewRow();
                        RowX["DiaI"] = temporario.Rows[linhaOK]["Dia"];
                        RowX["HoraI"] = temporario.Rows[linhaOK]["Hora"];
                        RowX["NomeI"] = temporario.Rows[linhaOK]["Nome"];

                        RowX["DiaR"] = produtos.Rows[i]["Dia"];
                        RowX["HoraR"] = produtos.Rows[i]["Hora"];
                        RowX["NomeR"] = produtos.Rows[i]["Nome"];


                        RowX["Form1I"] = temporario.Rows[linhaOK]["Form1"];
                        RowX["Form2I"] = temporario.Rows[linhaOK]["Form2"];
                        RowX["Prod_1I"] = temporario.Rows[linhaOK]["Prod_1"];
                        RowX["Prod_2I"] = temporario.Rows[linhaOK]["Prod_2"];
                        RowX["Prod_3I"] = temporario.Rows[linhaOK]["Prod_3"];
                        RowX["Prod_4I"] = temporario.Rows[linhaOK]["Prod_4"];
                        RowX["Prod_5I"] = temporario.Rows[linhaOK]["Prod_5"];
                        RowX["Prod_6I"] = temporario.Rows[linhaOK]["Prod_6"];
                        RowX["Prod_7I"] = temporario.Rows[linhaOK]["Prod_7"];
                        RowX["Prod_8I"] = temporario.Rows[linhaOK]["Prod_8"];
                        RowX["Prod_9I"] = temporario.Rows[linhaOK]["Prod_9"];
                        RowX["Prod_10I"] = temporario.Rows[linhaOK]["Prod_10"];
                        RowX["Prod_11I"] = temporario.Rows[linhaOK]["Prod_11"];
                        RowX["Prod_12I"] = temporario.Rows[linhaOK]["Prod_12"];
                        RowX["Prod_13I"] = temporario.Rows[linhaOK]["Prod_13"];
                        RowX["Prod_14I"] = temporario.Rows[linhaOK]["Prod_14"];
                        RowX["Prod_15I"] = temporario.Rows[linhaOK]["Prod_15"];
                        RowX["Prod_16I"] = temporario.Rows[linhaOK]["Prod_16"];
                        RowX["Prod_17I"] = temporario.Rows[linhaOK]["Prod_17"];
                        RowX["Prod_18I"] = temporario.Rows[linhaOK]["Prod_18"];
                        RowX["Prod_19I"] = temporario.Rows[linhaOK]["Prod_19"];
                        RowX["Prod_20I"] = temporario.Rows[linhaOK]["Prod_20"];
                        RowX["Prod_21I"] = temporario.Rows[linhaOK]["Prod_21"];
                        RowX["Prod_22I"] = temporario.Rows[linhaOK]["Prod_22"];
                        RowX["Prod_23I"] = temporario.Rows[linhaOK]["Prod_23"];
                        RowX["Prod_24I"] = temporario.Rows[linhaOK]["Prod_24"];
                        RowX["Prod_25I"] = temporario.Rows[linhaOK]["Prod_25"];
                        RowX["Prod_26I"] = temporario.Rows[linhaOK]["Prod_26"];
                        RowX["Prod_27I"] = temporario.Rows[linhaOK]["Prod_27"];
                        RowX["Prod_28I"] = temporario.Rows[linhaOK]["Prod_28"];
                        RowX["Prod_29I"] = temporario.Rows[linhaOK]["Prod_29"];
                        RowX["Prod_30I"] = temporario.Rows[linhaOK]["Prod_30"];
                        RowX["Prod_31I"] = temporario.Rows[linhaOK]["Prod_31"];
                        RowX["Prod_32I"] = temporario.Rows[linhaOK]["Prod_32"];
                        RowX["Prod_33I"] = temporario.Rows[linhaOK]["Prod_33"];
                        RowX["Prod_34I"] = temporario.Rows[linhaOK]["Prod_34"];
                        RowX["Prod_35I"] = temporario.Rows[linhaOK]["Prod_35"];
                        RowX["Prod_36I"] = temporario.Rows[linhaOK]["Prod_36"];
                        RowX["Prod_37I"] = temporario.Rows[linhaOK]["Prod_37"];
                        RowX["Prod_38I"] = temporario.Rows[linhaOK]["Prod_38"];
                        RowX["Prod_39I"] = temporario.Rows[linhaOK]["Prod_39"];
                        RowX["Prod_40I"] = temporario.Rows[linhaOK]["Prod_40"];
                        RowX["Prod_41I"] = temporario.Rows[linhaOK]["Prod_41"];
                        RowX["Prod_42I"] = temporario.Rows[linhaOK]["Prod_42"];
                        RowX["Prod_43I"] = temporario.Rows[linhaOK]["Prod_43"];
                        RowX["Prod_44I"] = temporario.Rows[linhaOK]["Prod_44"];
                        RowX["Prod_45I"] = temporario.Rows[linhaOK]["Prod_45"];
                        RowX["Prod_46I"] = temporario.Rows[linhaOK]["Prod_46"];
                        RowX["Prod_47I"] = temporario.Rows[linhaOK]["Prod_47"];
                        RowX["Prod_48I"] = temporario.Rows[linhaOK]["Prod_48"];
                        RowX["Prod_49I"] = temporario.Rows[linhaOK]["Prod_49"];
                        RowX["Prod_50I"] = temporario.Rows[linhaOK]["Prod_50"];
                        RowX["Prod_51I"] = temporario.Rows[linhaOK]["Prod_51"];
                        RowX["Prod_52I"] = temporario.Rows[linhaOK]["Prod_52"];
                        RowX["Prod_53I"] = temporario.Rows[linhaOK]["Prod_53"];
                        RowX["Prod_54I"] = temporario.Rows[linhaOK]["Prod_54"];
                        RowX["Prod_55I"] = temporario.Rows[linhaOK]["Prod_55"];
                        RowX["Prod_56I"] = temporario.Rows[linhaOK]["Prod_56"];
                        RowX["Prod_57I"] = temporario.Rows[linhaOK]["Prod_57"];
                        RowX["Prod_58I"] = temporario.Rows[linhaOK]["Prod_58"];
                        RowX["Prod_59I"] = temporario.Rows[linhaOK]["Prod_59"];
                        RowX["Prod_60I"] = temporario.Rows[linhaOK]["Prod_60"];
                        RowX["Prod_61I"] = temporario.Rows[linhaOK]["Prod_61"];
                        RowX["Prod_62I"] = temporario.Rows[linhaOK]["Prod_62"];
                        RowX["Prod_63I"] = temporario.Rows[linhaOK]["Prod_63"];
                        RowX["Prod_64I"] = temporario.Rows[linhaOK]["Prod_64"];
                        RowX["Prod_65I"] = temporario.Rows[linhaOK]["Prod_65"];
                       

                        RowX["Form1R"] = produtos.Rows[i]["Form1"];
                        RowX["Form2R"] = produtos.Rows[i]["Form2"];
                        RowX["Prod_1R"] = produtos.Rows[i]["Prod_1"];
                        RowX["Prod_2R"] = produtos.Rows[i]["Prod_2"];
                        RowX["Prod_3R"] = produtos.Rows[i]["Prod_3"];
                        RowX["Prod_4R"] = produtos.Rows[i]["Prod_4"];
                        RowX["Prod_5R"] = produtos.Rows[i]["Prod_5"];
                        RowX["Prod_6R"] = produtos.Rows[i]["Prod_6"];
                        RowX["Prod_7R"] = produtos.Rows[i]["Prod_7"];
                        RowX["Prod_8R"] = produtos.Rows[i]["Prod_8"];
                        RowX["Prod_9R"] = produtos.Rows[i]["Prod_9"];
                        RowX["Prod_10R"] = produtos.Rows[i]["Prod_10"];
                        RowX["Prod_11R"] = produtos.Rows[i]["Prod_11"];
                        RowX["Prod_12R"] = produtos.Rows[i]["Prod_12"];
                        RowX["Prod_13R"] = produtos.Rows[i]["Prod_13"];
                        RowX["Prod_14R"] = produtos.Rows[i]["Prod_14"];
                        RowX["Prod_15R"] = produtos.Rows[i]["Prod_15"];
                        RowX["Prod_16R"] = produtos.Rows[i]["Prod_16"];
                        RowX["Prod_17R"] = produtos.Rows[i]["Prod_17"];
                        RowX["Prod_18R"] = produtos.Rows[i]["Prod_18"];
                        RowX["Prod_19R"] = produtos.Rows[i]["Prod_19"];
                        RowX["Prod_20R"] = produtos.Rows[i]["Prod_20"];
                        RowX["Prod_21R"] = produtos.Rows[i]["Prod_21"];
                        RowX["Prod_22R"] = produtos.Rows[i]["Prod_22"];
                        RowX["Prod_23R"] = produtos.Rows[i]["Prod_23"];
                        RowX["Prod_24R"] = produtos.Rows[i]["Prod_24"];
                        RowX["Prod_25R"] = produtos.Rows[i]["Prod_25"];
                        RowX["Prod_26R"] = produtos.Rows[i]["Prod_26"];
                        RowX["Prod_27R"] = produtos.Rows[i]["Prod_27"];
                        RowX["Prod_28R"] = produtos.Rows[i]["Prod_28"];
                        RowX["Prod_29R"] = produtos.Rows[i]["Prod_29"];
                        RowX["Prod_30R"] = produtos.Rows[i]["Prod_30"];
                        RowX["Prod_31R"] = produtos.Rows[i]["Prod_31"];
                        RowX["Prod_32R"] = produtos.Rows[i]["Prod_32"];
                        RowX["Prod_33R"] = produtos.Rows[i]["Prod_33"];
                        RowX["Prod_34R"] = produtos.Rows[i]["Prod_34"];
                        RowX["Prod_35R"] = produtos.Rows[i]["Prod_35"];
                        RowX["Prod_36R"] = produtos.Rows[i]["Prod_36"];
                        RowX["Prod_37R"] = produtos.Rows[i]["Prod_37"];
                        RowX["Prod_38R"] = produtos.Rows[i]["Prod_38"];
                        RowX["Prod_39R"] = produtos.Rows[i]["Prod_39"];
                        RowX["Prod_40R"] = produtos.Rows[i]["Prod_40"];
                        RowX["Prod_41R"] = produtos.Rows[i]["Prod_41"];
                        RowX["Prod_42R"] = produtos.Rows[i]["Prod_42"];
                        RowX["Prod_43R"] = produtos.Rows[i]["Prod_43"];
                        RowX["Prod_44R"] = produtos.Rows[i]["Prod_44"];
                        RowX["Prod_45R"] = produtos.Rows[i]["Prod_45"];
                        RowX["Prod_46R"] = produtos.Rows[i]["Prod_46"];
                        RowX["Prod_47R"] = produtos.Rows[i]["Prod_47"];
                        RowX["Prod_48R"] = produtos.Rows[i]["Prod_48"];
                        RowX["Prod_49R"] = produtos.Rows[i]["Prod_49"];
                        RowX["Prod_50R"] = produtos.Rows[i]["Prod_50"];
                        RowX["Prod_51R"] = produtos.Rows[i]["Prod_51"];
                        RowX["Prod_52R"] = produtos.Rows[i]["Prod_52"];
                        RowX["Prod_53R"] = produtos.Rows[i]["Prod_53"];
                        RowX["Prod_54R"] = produtos.Rows[i]["Prod_54"];
                        RowX["Prod_55R"] = produtos.Rows[i]["Prod_55"];
                        RowX["Prod_56R"] = produtos.Rows[i]["Prod_56"];
                        RowX["Prod_57R"] = produtos.Rows[i]["Prod_57"];
                        RowX["Prod_58R"] = produtos.Rows[i]["Prod_58"];
                        RowX["Prod_59R"] = produtos.Rows[i]["Prod_59"];
                        RowX["Prod_60R"] = produtos.Rows[i]["Prod_60"];
                        RowX["Prod_61R"] = produtos.Rows[i]["Prod_61"];
                        RowX["Prod_62R"] = produtos.Rows[i]["Prod_62"];
                        RowX["Prod_63R"] = produtos.Rows[i]["Prod_63"];
                        RowX["Prod_64R"] = produtos.Rows[i]["Prod_64"];
                        RowX["Prod_65R"] = produtos.Rows[i]["Prod_65"];
                        
                        FinalForm.Rows.Add(RowX);

                    }
                    else
                    {
                        DataRow Row = resultadoForm.NewRow();
                        Row["Dia"] = 0;
                        Row["Hora"] = 0;
                        Row["Nome"] = 0;
                        Row["Form1"] = 0;
                        Row["Form2"] = 0;
                        Row["Prod_1"] = 0;
                        Row["Prod_2"] = 0;
                        Row["Prod_3"] = 0;
                        Row["Prod_4"] = 0;
                        Row["Prod_5"] = 0;
                        Row["Prod_6"] = 0;
                        Row["Prod_7"] = 0;
                        Row["Prod_8"] = 0;
                        Row["Prod_9"] = 0;
                        Row["Prod_10"] = 0;
                        Row["Prod_11"] = 0;
                        Row["Prod_12"] = 0;
                        Row["Prod_13"] = 0;
                        Row["Prod_14"] = 0;
                        Row["Prod_15"] = 0;
                        Row["Prod_16"] = 0;
                        Row["Prod_17"] = 0;
                        Row["Prod_18"] = 0;
                        Row["Prod_19"] = 0;
                        Row["Prod_20"] = 0;
                        Row["Prod_21"] = 0;
                        Row["Prod_22"] = 0;
                        Row["Prod_23"] = 0;
                        Row["Prod_24"] = 0;
                        Row["Prod_25"] = 0;
                        Row["Prod_26"] = 0;
                        Row["Prod_27"] = 0;
                        Row["Prod_28"] = 0;
                        Row["Prod_29"] = 0;
                        Row["Prod_30"] = 0;
                        Row["Prod_31"] = 0;
                        Row["Prod_32"] = 0;
                        Row["Prod_33"] = 0;
                        Row["Prod_34"] = 0;
                        Row["Prod_35"] = 0;
                        Row["Prod_36"] = 0;
                        Row["Prod_37"] = 0;
                        Row["Prod_38"] = 0;
                        Row["Prod_39"] = 0;
                        Row["Prod_40"] = 0;
                        Row["Prod_41"] = 0;
                        Row["Prod_42"] = 0;
                        Row["Prod_43"] = 0;
                        Row["Prod_44"] = 0;
                        Row["Prod_45"] = 0;
                        Row["Prod_46"] = 0;
                        Row["Prod_47"] = 0;
                        Row["Prod_48"] = 0;
                        Row["Prod_49"] = 0;
                        Row["Prod_50"] = 0;
                        Row["Prod_51"] = 0;
                        Row["Prod_52"] = 0;
                        Row["Prod_53"] = 0;
                        Row["Prod_54"] = 0;
                        Row["Prod_55"] = 0;
                        Row["Prod_56"] = 0;
                        Row["Prod_57"] = 0;
                        Row["Prod_58"] = 0;
                        Row["Prod_59"] = 0;
                        Row["Prod_60"] = 0;
                        Row["Prod_61"] = 0;
                        Row["Prod_62"] = 0;
                        Row["Prod_63"] = 0;
                        Row["Prod_64"] = 0;
                        Row["Prod_65"] = 0;
                  

                        resultadoForm.Rows.Add(Row);

                        DataRow RowX = FinalForm.NewRow();
                        RowX["DiaI"] = "---";
                        RowX["HoraI"] = "---";
                        RowX["NomeI"] = "---";

                        RowX["DiaR"] = produtos.Rows[i]["Dia"];
                        RowX["HoraR"] = produtos.Rows[i]["Hora"];
                        RowX["NomeR"] = produtos.Rows[i]["Nome"];


                        RowX["Form1I"] = 0;
                        RowX["Form2I"] = 0;
                        RowX["Prod_1I"] = 0;
                        RowX["Prod_2I"] = 0;
                        RowX["Prod_3I"] = 0;
                        RowX["Prod_4I"] = 0;
                        RowX["Prod_5I"] = 0;
                        RowX["Prod_6I"] = 0;
                        RowX["Prod_7I"] = 0;
                        RowX["Prod_8I"] = 0;
                        RowX["Prod_9I"] = 0;
                        RowX["Prod_10I"] = 0;
                        RowX["Prod_11I"] = 0;
                        RowX["Prod_12I"] = 0;
                        RowX["Prod_13I"] = 0;
                        RowX["Prod_14I"] = 0;
                        RowX["Prod_15I"] = 0;
                        RowX["Prod_16I"] = 0;
                        RowX["Prod_17I"] = 0;
                        RowX["Prod_18I"] = 0;
                        RowX["Prod_19I"] = 0;
                        RowX["Prod_20I"] = 0;
                        RowX["Prod_21I"] = 0;
                        RowX["Prod_22I"] = 0;
                        RowX["Prod_23I"] = 0;
                        RowX["Prod_24I"] = 0;
                        RowX["Prod_25I"] = 0;
                        RowX["Prod_26I"] = 0;
                        RowX["Prod_27I"] = 0;
                        RowX["Prod_28I"] = 0;
                        RowX["Prod_29I"] = 0;
                        RowX["Prod_30I"] = 0;
                        RowX["Prod_31I"] = 0;
                        RowX["Prod_32I"] = 0;
                        RowX["Prod_33I"] = 0;
                        RowX["Prod_34I"] = 0;
                        RowX["Prod_35I"] = 0;
                        RowX["Prod_36I"] = 0;
                        RowX["Prod_37I"] = 0;
                        RowX["Prod_38I"] = 0;
                        RowX["Prod_39I"] = 0;
                        RowX["Prod_40I"] = 0;
                        RowX["Prod_41I"] = 0;
                        RowX["Prod_42I"] = 0;
                        RowX["Prod_43I"] = 0;
                        RowX["Prod_44I"] = 0;
                        RowX["Prod_45I"] = 0;
                        RowX["Prod_46I"] = 0;
                        RowX["Prod_47I"] = 0;
                        RowX["Prod_48I"] = 0;
                        RowX["Prod_49I"] = 0;
                        RowX["Prod_50I"] = 0;
                        RowX["Prod_51I"] = 0;
                        RowX["Prod_52I"] = 0;
                        RowX["Prod_53I"] = 0;
                        RowX["Prod_54I"] = 0;
                        RowX["Prod_55I"] = 0;
                        RowX["Prod_56I"] = 0;
                        RowX["Prod_57I"] = 0;
                        RowX["Prod_58I"] = 0;
                        RowX["Prod_59I"] = 0;
                        RowX["Prod_60I"] = 0;
                        RowX["Prod_61I"] = 0;
                        RowX["Prod_62I"] = 0;
                        RowX["Prod_63I"] = 0;
                        RowX["Prod_64I"] = 0;
                        RowX["Prod_65I"] = 0;


                        RowX["Form1R"] = produtos.Rows[i]["Form1"];
                        RowX["Form2R"] = produtos.Rows[i]["Form2"];
                        RowX["Prod_1R"] = produtos.Rows[i]["Prod_1"];
                        RowX["Prod_2R"] = produtos.Rows[i]["Prod_2"];
                        RowX["Prod_3R"] = produtos.Rows[i]["Prod_3"];
                        RowX["Prod_4R"] = produtos.Rows[i]["Prod_4"];
                        RowX["Prod_5R"] = produtos.Rows[i]["Prod_5"];
                        RowX["Prod_6R"] = produtos.Rows[i]["Prod_6"];
                        RowX["Prod_7R"] = produtos.Rows[i]["Prod_7"];
                        RowX["Prod_8R"] = produtos.Rows[i]["Prod_8"];
                        RowX["Prod_9R"] = produtos.Rows[i]["Prod_9"];
                        RowX["Prod_10R"] = produtos.Rows[i]["Prod_10"];
                        RowX["Prod_11R"] = produtos.Rows[i]["Prod_11"];
                        RowX["Prod_12R"] = produtos.Rows[i]["Prod_12"];
                        RowX["Prod_13R"] = produtos.Rows[i]["Prod_13"];
                        RowX["Prod_14R"] = produtos.Rows[i]["Prod_14"];
                        RowX["Prod_15R"] = produtos.Rows[i]["Prod_15"];
                        RowX["Prod_16R"] = produtos.Rows[i]["Prod_16"];
                        RowX["Prod_17R"] = produtos.Rows[i]["Prod_17"];
                        RowX["Prod_18R"] = produtos.Rows[i]["Prod_18"];
                        RowX["Prod_19R"] = produtos.Rows[i]["Prod_19"];
                        RowX["Prod_20R"] = produtos.Rows[i]["Prod_20"];
                        RowX["Prod_21R"] = produtos.Rows[i]["Prod_21"];
                        RowX["Prod_22R"] = produtos.Rows[i]["Prod_22"];
                        RowX["Prod_23R"] = produtos.Rows[i]["Prod_23"];
                        RowX["Prod_24R"] = produtos.Rows[i]["Prod_24"];
                        RowX["Prod_25R"] = produtos.Rows[i]["Prod_25"];
                        RowX["Prod_26R"] = produtos.Rows[i]["Prod_26"];
                        RowX["Prod_27R"] = produtos.Rows[i]["Prod_27"];
                        RowX["Prod_28R"] = produtos.Rows[i]["Prod_28"];
                        RowX["Prod_29R"] = produtos.Rows[i]["Prod_29"];
                        RowX["Prod_30R"] = produtos.Rows[i]["Prod_30"];
                        RowX["Prod_31R"] = produtos.Rows[i]["Prod_31"];
                        RowX["Prod_32R"] = produtos.Rows[i]["Prod_32"];
                        RowX["Prod_33R"] = produtos.Rows[i]["Prod_33"];
                        RowX["Prod_34R"] = produtos.Rows[i]["Prod_34"];
                        RowX["Prod_35R"] = produtos.Rows[i]["Prod_35"];
                        RowX["Prod_36R"] = produtos.Rows[i]["Prod_36"];
                        RowX["Prod_37R"] = produtos.Rows[i]["Prod_37"];
                        RowX["Prod_38R"] = produtos.Rows[i]["Prod_38"];
                        RowX["Prod_39R"] = produtos.Rows[i]["Prod_39"];
                        RowX["Prod_40R"] = produtos.Rows[i]["Prod_40"];
                        RowX["Prod_41R"] = produtos.Rows[i]["Prod_41"];
                        RowX["Prod_42R"] = produtos.Rows[i]["Prod_42"];
                        RowX["Prod_43R"] = produtos.Rows[i]["Prod_43"];
                        RowX["Prod_44R"] = produtos.Rows[i]["Prod_44"];
                        RowX["Prod_45R"] = produtos.Rows[i]["Prod_45"];
                        RowX["Prod_46R"] = produtos.Rows[i]["Prod_46"];
                        RowX["Prod_47R"] = produtos.Rows[i]["Prod_47"];
                        RowX["Prod_48R"] = produtos.Rows[i]["Prod_48"];
                        RowX["Prod_49R"] = produtos.Rows[i]["Prod_49"];
                        RowX["Prod_50R"] = produtos.Rows[i]["Prod_50"];
                        RowX["Prod_51R"] = produtos.Rows[i]["Prod_51"];
                        RowX["Prod_52R"] = produtos.Rows[i]["Prod_52"];
                        RowX["Prod_53R"] = produtos.Rows[i]["Prod_53"];
                        RowX["Prod_54R"] = produtos.Rows[i]["Prod_54"];
                        RowX["Prod_55R"] = produtos.Rows[i]["Prod_55"];
                        RowX["Prod_56R"] = produtos.Rows[i]["Prod_56"];
                        RowX["Prod_57R"] = produtos.Rows[i]["Prod_57"];
                        RowX["Prod_58R"] = produtos.Rows[i]["Prod_58"];
                        RowX["Prod_59R"] = produtos.Rows[i]["Prod_59"];
                        RowX["Prod_60R"] = produtos.Rows[i]["Prod_60"];
                        RowX["Prod_61R"] = produtos.Rows[i]["Prod_61"];
                        RowX["Prod_62R"] = produtos.Rows[i]["Prod_62"];
                        RowX["Prod_63R"] = produtos.Rows[i]["Prod_63"];
                        RowX["Prod_64R"] = produtos.Rows[i]["Prod_64"];
                        RowX["Prod_65R"] = produtos.Rows[i]["Prod_65"];
                       
                        FinalForm.Rows.Add(RowX);
                    }
                    //ComparaGrid1.DataSource = FinalForm;


                }

                String[] ProdCamp = new String[66];
                Object[] sumObject = new Object[66];
                Object[] sumObject1 = new Object[66];
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
               
                if (produtos.Rows.Count > 0)
                {
                    for (int i = 0; i <= 64; i++)
                    {


                        sumObject[i] = produtos.Compute("Sum(" + ProdCamp[i] + ")", "");
                        sumObject1[i] = resultadoForm.Compute("Sum(" + ProdCamp[i] + ")", "");




                        if (!DBNull.Value.Equals(sumObject[i]))
                        {
                            DataRow Row1 = db2.NewRow();
                            Double Valor = Convert.ToDouble(sumObject[i]);
                            if (Valor != 0)
                            {
                                Double Medida = Convert.ToDouble(TabelasT.NomeProduto.Rows[i]["Medida"].ToString());

                                if (!DBNull.Value.Equals(sumObject1[i]))
                                {
                                    Double Valor1 = Convert.ToDouble(sumObject1[i]);
                                    if (Valor1 != 0)
                                    {
                                        Row1["TotalF"] = Valor1 / Medida;
                                    }
                                    else
                                    {
                                        Row1["TotalF"] = 0;
                                    }

                                    if (Valor == 0 && Valor1 == 0)
                                    {
                                        TabelasT.Visivel1[i] = false;
                                        TabelasT.Visivel2[i] = false;
                                    }

                                    else
                                    {
                                        if (HabFormIdeal.Checked == false) { TabelasT.Visivel2[i] = false; } else { TabelasT.Visivel2[i] = true; }
                                        TabelasT.Visivel1[i] = true;
                                    }



                                    Row1["TotalR"] = Valor / Medida;
                                    Row1["Produto"] = TabelasT.NomeProduto.Rows[i]["Produto"].ToString();
                                    db2.Rows.Add(Row1);
                                }
                            }


                        }

                    }

                }


                object sumTotal;
                sumTotal = db2.Compute("Sum(TotalR)", "");

                object sumTotal2;
                sumTotal2 = db2.Compute("Sum(TotalF)", "");


                double ToTalTot = Convert.ToDouble(sumTotal);
                double ToTalTot2 = Convert.ToDouble(sumTotal2);

                BatidasStr = produtos.Rows.Count.ToString("#,#");

                //if (ToTalTot != 0 && ToTalTot2!=0)
                //{

                DataRow Row1X = db3.NewRow();
                Row1X["CProg"] = NumerodaFormulaX1;
                Row1X["CCliente"] = NumerodaFormulaX2;
                Row1X["Batida"] = BatidasStr;
                Row1X["Nome"] = NomedaFormulaX1;
                Row1X["TotalR"] = ToTalTot;
                Row1X["TotalF"] = ToTalTot2;
                db3.Rows.Add(Row1X);

                // }

                TotalBox.Text = ToTalTot.ToString("#,0.000");
                if (HabFormIdeal.Checked == true)
                {
                    Totalbox2.Text = ToTalTot2.ToString("#,0.000");
                }

                TabelasT.Total1 = ToTalTot.ToString("#,0.000");


                TBatidaBox.Text = produtos.Rows.Count.ToString("#,#");
                TabelasT.Batida1 = produtos.Rows.Count.ToString("#,#");

                for (int i = 0; i < 65; i++)
                {
                    String Temp1 = TabelasT.NomeProduto.Rows[i]["Produto"].ToString() + " Form.";
                    String Temp2 = TabelasT.NomeProduto.Rows[i]["Produto"].ToString() + " Real";
                    FinalForm.Columns[ProdCamp3[i]].ColumnName = Temp1;
                    FinalForm.Columns[ProdCamp4[i]].ColumnName = Temp2;
                }

                tabelagrid.DataSource = FinalForm;
                dataGridView1.DataSource = db3;
            }

            catch (MySqlException erro)
            {

                MessageBox.Show("Erro na Conexão: " + erro);
            }

            Conexao.Close();
            dataGridView2.DataSource = db2;
            TabelasT.DataTable2 = db2;
            TabelasT.DataTable1 = db3;
            ArrumaGrid();

        }
        private void ArrumaGrid()
        {
            int Col = 65;
            int j = 10;

            

            for (int i = 0; i < 65; i++)
            {
                if (TabelasT.Visivel1[i] == false)
                {
                    tabelagrid.Columns[j].Visible = false;
                }
                else
                {
                    tabelagrid.Columns[j].Visible = true;
                    tabelagrid.Columns[j].DefaultCellStyle.Format = "#,##0.000";
                }

                j = j + 2;

            }
            int x = 11;
            for (int i = 0; i < 65; i++)
            {
                if (TabelasT.Visivel2[i] == false)
                {
                    tabelagrid.Columns[x].Visible = false;
                }
                else
                {
                    tabelagrid.Columns[x].Visible = true;
                    tabelagrid.Columns[x].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    tabelagrid.Columns[x].HeaderCell.Style.BackColor = Color.LightGoldenrodYellow;
                    tabelagrid.Columns[x].DefaultCellStyle.Format = "#,##0.000";
                }
                x = x + 2;
            }

            DetalhesFormula();

            dataGridView1.Columns["CProg"].Width = 50;
            dataGridView1.Columns["CProg"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns["CProg"].HeaderText = "C. Prog";
            dataGridView1.Columns["CProg"].DefaultCellStyle.Format = "#,#";

            dataGridView1.Columns["CCliente"].Width = 50;
            dataGridView1.Columns["CCliente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns["CCliente"].HeaderText = "C. Clie.";
            dataGridView1.Columns["CCliente"].DefaultCellStyle.Format = "#,#";

            dataGridView1.Columns["Batida"].Width = 50;
            dataGridView1.Columns["Batida"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns["Batida"].HeaderText = "Batida";
            dataGridView1.Columns["Batida"].DefaultCellStyle.Format = "#,#";

            dataGridView1.Columns["Nome"].Width = 200;
            dataGridView1.Columns["Nome"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridView1.Columns["Nome"].HeaderText = "Nome Fórmula";

            dataGridView1.Columns["TotalR"].Width = 100;
            dataGridView1.Columns["TotalR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView1.Columns["TotalR"].HeaderText = "Total Real";
            dataGridView1.Columns["TotalR"].DefaultCellStyle.Format = "#,##0.000";

            if (HabFormIdeal.Checked == true)
            {
                dataGridView1.Columns["TotalF"].Visible = true;
                dataGridView1.Columns["TotalF"].Width = 100;
                dataGridView1.Columns["TotalF"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView1.Columns["TotalF"].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                dataGridView1.Columns["TotalF"].HeaderCell.Style.BackColor = Color.LightGoldenrodYellow;
                dataGridView1.Columns["TotalF"].HeaderText = "Total Form.";
                dataGridView1.Columns["TotalF"].DefaultCellStyle.Format = "#,##0.000";
            }
            else
            {
                dataGridView1.Columns["TotalF"].Visible = false;
            }


            dataGridView2.Columns["Produto"].DisplayIndex = 0;
            dataGridView2.Columns["Produto"].Width = 150;
            dataGridView2.Columns["Produto"].HeaderText = "Nome do Produto";


            dataGridView2.Columns["TotalR"].DisplayIndex = 1;
            dataGridView2.Columns["TotalR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView2.Columns["TotalR"].DefaultCellStyle.Format = "#,##0.000";
            dataGridView2.Columns["TotalR"].Width = 100;
            dataGridView2.Columns["TotalR"].HeaderText = "Total Real";

            if (HabFormIdeal.Checked == true)
            {
                dataGridView2.Columns["TotalF"].Visible = true;
                dataGridView2.Columns["TotalF"].DisplayIndex = 2;
                dataGridView2.Columns["TotalF"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dataGridView2.Columns["TotalF"].DefaultCellStyle.Format = "#,##0.000";
                dataGridView2.Columns["TotalF"].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                dataGridView2.Columns["TotalF"].HeaderCell.Style.BackColor = Color.LightGoldenrodYellow;
                dataGridView2.Columns["TotalF"].Width = 100;
                dataGridView2.Columns["TotalF"].HeaderText = "Total Fórm.";
            }
            else
            {
                dataGridView2.Columns["TotalF"].Visible = false;
            }



        }
        private void Pesquisar_Todas()
        {

            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            tabelagrid.DataSource = null;

            String[] ProdCamp3 = new String[66];
            String[] ProdCamp4 = new String[66];

            String BatidasStr;

            String HorIn;
            String HorFi;

            ProdCamp3[0] = "Prod_1I";
            ProdCamp3[1] = "Prod_2I";
            ProdCamp3[2] = "Prod_3I";
            ProdCamp3[3] = "Prod_4I";
            ProdCamp3[4] = "Prod_5I";
            ProdCamp3[5] = "Prod_6I";
            ProdCamp3[6] = "Prod_7I";
            ProdCamp3[7] = "Prod_8I";
            ProdCamp3[8] = "Prod_9I";
            ProdCamp3[9] = "Prod_10I";
            ProdCamp3[10] = "Prod_11I";
            ProdCamp3[11] = "Prod_12I";
            ProdCamp3[12] = "Prod_13I";
            ProdCamp3[13] = "Prod_14I";
            ProdCamp3[14] = "Prod_15I";
            ProdCamp3[15] = "Prod_16I";
            ProdCamp3[16] = "Prod_17I";
            ProdCamp3[17] = "Prod_18I";
            ProdCamp3[18] = "Prod_19I";
            ProdCamp3[19] = "Prod_20I";
            ProdCamp3[20] = "Prod_21I";
            ProdCamp3[21] = "Prod_22I";
            ProdCamp3[22] = "Prod_23I";
            ProdCamp3[23] = "Prod_24I";
            ProdCamp3[24] = "Prod_25I";
            ProdCamp3[25] = "Prod_26I";
            ProdCamp3[26] = "Prod_27I";
            ProdCamp3[27] = "Prod_28I";
            ProdCamp3[28] = "Prod_29I";
            ProdCamp3[29] = "Prod_30I";
            ProdCamp3[30] = "Prod_31I";
            ProdCamp3[31] = "Prod_32I";
            ProdCamp3[32] = "Prod_33I";
            ProdCamp3[33] = "Prod_34I";
            ProdCamp3[34] = "Prod_35I";
            ProdCamp3[35] = "Prod_36I";
            ProdCamp3[36] = "Prod_37I";
            ProdCamp3[37] = "Prod_38I";
            ProdCamp3[38] = "Prod_39I";
            ProdCamp3[39] = "Prod_40I";
            ProdCamp3[40] = "Prod_41I";
            ProdCamp3[41] = "Prod_42I";
            ProdCamp3[42] = "Prod_43I";
            ProdCamp3[43] = "Prod_44I";
            ProdCamp3[44] = "Prod_45I";
            ProdCamp3[45] = "Prod_46I";
            ProdCamp3[46] = "Prod_47I";
            ProdCamp3[47] = "Prod_48I";
            ProdCamp3[48] = "Prod_49I";
            ProdCamp3[49] = "Prod_50I";
            ProdCamp3[50] = "Prod_51I";
            ProdCamp3[51] = "Prod_52I";
            ProdCamp3[52] = "Prod_53I";
            ProdCamp3[53] = "Prod_54I";
            ProdCamp3[54] = "Prod_55I";
            ProdCamp3[55] = "Prod_56I";
            ProdCamp3[56] = "Prod_57I";
            ProdCamp3[57] = "Prod_58I";
            ProdCamp3[58] = "Prod_59I";
            ProdCamp3[59] = "Prod_60I";
            ProdCamp3[60] = "Prod_61I";
            ProdCamp3[61] = "Prod_62I";
            ProdCamp3[62] = "Prod_63I";
            ProdCamp3[63] = "Prod_64I";
            ProdCamp3[64] = "Prod_65I";
            

            ProdCamp4[0] = "Prod_1R";
            ProdCamp4[1] = "Prod_2R";
            ProdCamp4[2] = "Prod_3R";
            ProdCamp4[3] = "Prod_4R";
            ProdCamp4[4] = "Prod_5R";
            ProdCamp4[5] = "Prod_6R";
            ProdCamp4[6] = "Prod_7R";
            ProdCamp4[7] = "Prod_8R";
            ProdCamp4[8] = "Prod_9R";
            ProdCamp4[9] = "Prod_10R";
            ProdCamp4[10] = "Prod_11R";
            ProdCamp4[11] = "Prod_12R";
            ProdCamp4[12] = "Prod_13R";
            ProdCamp4[13] = "Prod_14R";
            ProdCamp4[14] = "Prod_15R";
            ProdCamp4[15] = "Prod_16R";
            ProdCamp4[16] = "Prod_17R";
            ProdCamp4[17] = "Prod_18R";
            ProdCamp4[18] = "Prod_19R";
            ProdCamp4[19] = "Prod_20R";
            ProdCamp4[20] = "Prod_21R";
            ProdCamp4[21] = "Prod_22R";
            ProdCamp4[22] = "Prod_23R";
            ProdCamp4[23] = "Prod_24R";
            ProdCamp4[24] = "Prod_25R";
            ProdCamp4[25] = "Prod_26R";
            ProdCamp4[26] = "Prod_27R";
            ProdCamp4[27] = "Prod_28R";
            ProdCamp4[28] = "Prod_29R";
            ProdCamp4[29] = "Prod_30R";
            ProdCamp4[30] = "Prod_31R";
            ProdCamp4[31] = "Prod_32R";
            ProdCamp4[32] = "Prod_33R";
            ProdCamp4[33] = "Prod_34R";
            ProdCamp4[34] = "Prod_35R";
            ProdCamp4[35] = "Prod_36R";
            ProdCamp4[36] = "Prod_37R";
            ProdCamp4[37] = "Prod_38R";
            ProdCamp4[38] = "Prod_39R";
            ProdCamp4[39] = "Prod_40R";
            ProdCamp4[40] = "Prod_41R";
            ProdCamp4[41] = "Prod_42R";
            ProdCamp4[42] = "Prod_43R";
            ProdCamp4[43] = "Prod_44R";
            ProdCamp4[44] = "Prod_45R";
            ProdCamp4[45] = "Prod_46R";
            ProdCamp4[46] = "Prod_47R";
            ProdCamp4[47] = "Prod_48R";
            ProdCamp4[48] = "Prod_49R";
            ProdCamp4[49] = "Prod_50R";
            ProdCamp4[50] = "Prod_51R";
            ProdCamp4[51] = "Prod_52R";
            ProdCamp4[52] = "Prod_53R";
            ProdCamp4[53] = "Prod_54R";
            ProdCamp4[54] = "Prod_55R";
            ProdCamp4[55] = "Prod_56R";
            ProdCamp4[56] = "Prod_57R";
            ProdCamp4[57] = "Prod_58R";
            ProdCamp4[58] = "Prod_59R";
            ProdCamp4[59] = "Prod_60R";
            ProdCamp4[60] = "Prod_61R";
            ProdCamp4[61] = "Prod_62R";
            ProdCamp4[62] = "Prod_63R";
            ProdCamp4[63] = "Prod_64R";
            ProdCamp4[64] = "Prod_65R";

            String Dia1 = comboBox1.SelectedItem.ToString();
            String Dia2 = comboBox2.SelectedItem.ToString();

            DataTable resultadoForm = new DataTable();
            resultadoForm.Columns.Add("Dia", typeof(string));
            resultadoForm.Columns.Add("Hora", typeof(string));
            resultadoForm.Columns.Add("Nome", typeof(string));
            resultadoForm.Columns.Add("Form1", typeof(int));
            resultadoForm.Columns.Add("Form2", typeof(int));
            resultadoForm.Columns.Add("Prod_1", typeof(int));
            resultadoForm.Columns.Add("Prod_2", typeof(int));
            resultadoForm.Columns.Add("Prod_3", typeof(int));
            resultadoForm.Columns.Add("Prod_4", typeof(int));
            resultadoForm.Columns.Add("Prod_5", typeof(int));
            resultadoForm.Columns.Add("Prod_6", typeof(int));
            resultadoForm.Columns.Add("Prod_7", typeof(int));
            resultadoForm.Columns.Add("Prod_8", typeof(int));
            resultadoForm.Columns.Add("Prod_9", typeof(int));
            resultadoForm.Columns.Add("Prod_10", typeof(int));
            resultadoForm.Columns.Add("Prod_11", typeof(int));
            resultadoForm.Columns.Add("Prod_12", typeof(int));
            resultadoForm.Columns.Add("Prod_13", typeof(int));
            resultadoForm.Columns.Add("Prod_14", typeof(int));
            resultadoForm.Columns.Add("Prod_15", typeof(int));
            resultadoForm.Columns.Add("Prod_16", typeof(int));
            resultadoForm.Columns.Add("Prod_17", typeof(int));
            resultadoForm.Columns.Add("Prod_18", typeof(int));
            resultadoForm.Columns.Add("Prod_19", typeof(int));
            resultadoForm.Columns.Add("Prod_20", typeof(int));
            resultadoForm.Columns.Add("Prod_21", typeof(int));
            resultadoForm.Columns.Add("Prod_22", typeof(int));
            resultadoForm.Columns.Add("Prod_23", typeof(int));
            resultadoForm.Columns.Add("Prod_24", typeof(int));
            resultadoForm.Columns.Add("Prod_25", typeof(int));
            resultadoForm.Columns.Add("Prod_26", typeof(int));
            resultadoForm.Columns.Add("Prod_27", typeof(int));
            resultadoForm.Columns.Add("Prod_28", typeof(int));
            resultadoForm.Columns.Add("Prod_29", typeof(int));
            resultadoForm.Columns.Add("Prod_30", typeof(int));
            resultadoForm.Columns.Add("Prod_31", typeof(int));
            resultadoForm.Columns.Add("Prod_32", typeof(int));
            resultadoForm.Columns.Add("Prod_33", typeof(int));
            resultadoForm.Columns.Add("Prod_34", typeof(int));
            resultadoForm.Columns.Add("Prod_35", typeof(int));
            resultadoForm.Columns.Add("Prod_36", typeof(int));
            resultadoForm.Columns.Add("Prod_37", typeof(int));
            resultadoForm.Columns.Add("Prod_38", typeof(int));
            resultadoForm.Columns.Add("Prod_39", typeof(int));
            resultadoForm.Columns.Add("Prod_40", typeof(int));
            resultadoForm.Columns.Add("Prod_41", typeof(int));
            resultadoForm.Columns.Add("Prod_42", typeof(int));
            resultadoForm.Columns.Add("Prod_43", typeof(int));
            resultadoForm.Columns.Add("Prod_44", typeof(int));
            resultadoForm.Columns.Add("Prod_45", typeof(int));
            resultadoForm.Columns.Add("Prod_46", typeof(int));
            resultadoForm.Columns.Add("Prod_47", typeof(int));
            resultadoForm.Columns.Add("Prod_48", typeof(int));
            resultadoForm.Columns.Add("Prod_49", typeof(int));
            resultadoForm.Columns.Add("Prod_50", typeof(int));
            resultadoForm.Columns.Add("Prod_51", typeof(int));
            resultadoForm.Columns.Add("Prod_52", typeof(int));
            resultadoForm.Columns.Add("Prod_53", typeof(int));
            resultadoForm.Columns.Add("Prod_54", typeof(int));
            resultadoForm.Columns.Add("Prod_55", typeof(int));
            resultadoForm.Columns.Add("Prod_56", typeof(int));
            resultadoForm.Columns.Add("Prod_57", typeof(int));
            resultadoForm.Columns.Add("Prod_58", typeof(int));
            resultadoForm.Columns.Add("Prod_59", typeof(int));
            resultadoForm.Columns.Add("Prod_60", typeof(int));
            resultadoForm.Columns.Add("Prod_61", typeof(int));
            resultadoForm.Columns.Add("Prod_62", typeof(int));
            resultadoForm.Columns.Add("Prod_63", typeof(int));
            resultadoForm.Columns.Add("Prod_64", typeof(int));
            resultadoForm.Columns.Add("Prod_65", typeof(int));
            
            DataTable FinalForm = new DataTable();
            
            FinalForm.Columns.Add("DiaR", typeof(string));
            FinalForm.Columns.Add("HoraR", typeof(string));
            FinalForm.Columns.Add("NomeR", typeof(string));

            FinalForm.Columns.Add("DiaI", typeof(string));
            FinalForm.Columns.Add("HoraI", typeof(string));
            FinalForm.Columns.Add("NomeI", typeof(string));

            FinalForm.Columns.Add("Form1R", typeof(int));
            FinalForm.Columns.Add("Form1I", typeof(int));
            FinalForm.Columns.Add("Form2R", typeof(int));
            FinalForm.Columns.Add("Form2I", typeof(int));
            FinalForm.Columns.Add("Prod_1R", typeof(int));
            FinalForm.Columns.Add("Prod_1I", typeof(int));
            FinalForm.Columns.Add("Prod_2R", typeof(int));
            FinalForm.Columns.Add("Prod_2I", typeof(int));
            FinalForm.Columns.Add("Prod_3R", typeof(int));
            FinalForm.Columns.Add("Prod_3I", typeof(int));
            FinalForm.Columns.Add("Prod_4R", typeof(int));
            FinalForm.Columns.Add("Prod_4I", typeof(int));
            FinalForm.Columns.Add("Prod_5R", typeof(int));
            FinalForm.Columns.Add("Prod_5I", typeof(int));
            FinalForm.Columns.Add("Prod_6R", typeof(int));
            FinalForm.Columns.Add("Prod_6I", typeof(int));
            FinalForm.Columns.Add("Prod_7R", typeof(int));
            FinalForm.Columns.Add("Prod_7I", typeof(int));
            FinalForm.Columns.Add("Prod_8R", typeof(int));
            FinalForm.Columns.Add("Prod_8I", typeof(int));
            FinalForm.Columns.Add("Prod_9R", typeof(int));
            FinalForm.Columns.Add("Prod_9I", typeof(int));
            FinalForm.Columns.Add("Prod_10R", typeof(int));
            FinalForm.Columns.Add("Prod_10I", typeof(int));
            FinalForm.Columns.Add("Prod_11R", typeof(int));
            FinalForm.Columns.Add("Prod_11I", typeof(int));
            FinalForm.Columns.Add("Prod_12R", typeof(int));
            FinalForm.Columns.Add("Prod_12I", typeof(int));
            FinalForm.Columns.Add("Prod_13R", typeof(int));
            FinalForm.Columns.Add("Prod_13I", typeof(int));
            FinalForm.Columns.Add("Prod_14R", typeof(int));
            FinalForm.Columns.Add("Prod_14I", typeof(int));
            FinalForm.Columns.Add("Prod_15R", typeof(int));
            FinalForm.Columns.Add("Prod_15I", typeof(int));
            FinalForm.Columns.Add("Prod_16R", typeof(int));
            FinalForm.Columns.Add("Prod_16I", typeof(int));
            FinalForm.Columns.Add("Prod_17R", typeof(int));
            FinalForm.Columns.Add("Prod_17I", typeof(int));
            FinalForm.Columns.Add("Prod_18R", typeof(int));
            FinalForm.Columns.Add("Prod_18I", typeof(int));
            FinalForm.Columns.Add("Prod_19R", typeof(int));
            FinalForm.Columns.Add("Prod_19I", typeof(int));
            FinalForm.Columns.Add("Prod_20R", typeof(int));
            FinalForm.Columns.Add("Prod_20I", typeof(int));
            FinalForm.Columns.Add("Prod_21R", typeof(int));
            FinalForm.Columns.Add("Prod_21I", typeof(int));
            FinalForm.Columns.Add("Prod_22R", typeof(int));
            FinalForm.Columns.Add("Prod_22I", typeof(int));
            FinalForm.Columns.Add("Prod_23R", typeof(int));
            FinalForm.Columns.Add("Prod_23I", typeof(int));
            FinalForm.Columns.Add("Prod_24R", typeof(int));
            FinalForm.Columns.Add("Prod_24I", typeof(int));
            FinalForm.Columns.Add("Prod_25R", typeof(int));
            FinalForm.Columns.Add("Prod_25I", typeof(int));
            FinalForm.Columns.Add("Prod_26R", typeof(int));
            FinalForm.Columns.Add("Prod_26I", typeof(int));
            FinalForm.Columns.Add("Prod_27R", typeof(int));
            FinalForm.Columns.Add("Prod_27I", typeof(int));
            FinalForm.Columns.Add("Prod_28R", typeof(int));
            FinalForm.Columns.Add("Prod_28I", typeof(int));
            FinalForm.Columns.Add("Prod_29R", typeof(int));
            FinalForm.Columns.Add("Prod_29I", typeof(int));
            FinalForm.Columns.Add("Prod_30R", typeof(int));
            FinalForm.Columns.Add("Prod_30I", typeof(int));
            FinalForm.Columns.Add("Prod_31R", typeof(int));
            FinalForm.Columns.Add("Prod_31I", typeof(int));
            FinalForm.Columns.Add("Prod_32R", typeof(int));
            FinalForm.Columns.Add("Prod_32I", typeof(int));
            FinalForm.Columns.Add("Prod_33R", typeof(int));
            FinalForm.Columns.Add("Prod_33I", typeof(int));
            FinalForm.Columns.Add("Prod_34R", typeof(int));
            FinalForm.Columns.Add("Prod_34I", typeof(int));
            FinalForm.Columns.Add("Prod_35R", typeof(int));
            FinalForm.Columns.Add("Prod_35I", typeof(int));
            FinalForm.Columns.Add("Prod_36R", typeof(int));
            FinalForm.Columns.Add("Prod_36I", typeof(int));
            FinalForm.Columns.Add("Prod_37R", typeof(int));
            FinalForm.Columns.Add("Prod_37I", typeof(int));
            FinalForm.Columns.Add("Prod_38R", typeof(int));
            FinalForm.Columns.Add("Prod_38I", typeof(int));
            FinalForm.Columns.Add("Prod_39R", typeof(int));
            FinalForm.Columns.Add("Prod_39I", typeof(int));
            FinalForm.Columns.Add("Prod_40R", typeof(int));
            FinalForm.Columns.Add("Prod_40I", typeof(int));
            FinalForm.Columns.Add("Prod_41R", typeof(int));
            FinalForm.Columns.Add("Prod_41I", typeof(int));
            FinalForm.Columns.Add("Prod_42R", typeof(int));
            FinalForm.Columns.Add("Prod_42I", typeof(int));
            FinalForm.Columns.Add("Prod_43R", typeof(int));
            FinalForm.Columns.Add("Prod_43I", typeof(int));
            FinalForm.Columns.Add("Prod_44R", typeof(int));
            FinalForm.Columns.Add("Prod_44I", typeof(int));
            FinalForm.Columns.Add("Prod_45R", typeof(int));
            FinalForm.Columns.Add("Prod_45I", typeof(int));
            FinalForm.Columns.Add("Prod_46R", typeof(int));
            FinalForm.Columns.Add("Prod_46I", typeof(int));
            FinalForm.Columns.Add("Prod_47R", typeof(int));
            FinalForm.Columns.Add("Prod_47I", typeof(int));
            FinalForm.Columns.Add("Prod_48R", typeof(int));
            FinalForm.Columns.Add("Prod_48I", typeof(int));
            FinalForm.Columns.Add("Prod_49R", typeof(int));
            FinalForm.Columns.Add("Prod_49I", typeof(int));
            FinalForm.Columns.Add("Prod_50R", typeof(int));
            FinalForm.Columns.Add("Prod_50I", typeof(int));
            FinalForm.Columns.Add("Prod_51R", typeof(int));
            FinalForm.Columns.Add("Prod_51I", typeof(int));
            FinalForm.Columns.Add("Prod_52R", typeof(int));
            FinalForm.Columns.Add("Prod_52I", typeof(int));
            FinalForm.Columns.Add("Prod_53R", typeof(int));
            FinalForm.Columns.Add("Prod_53I", typeof(int));
            FinalForm.Columns.Add("Prod_54R", typeof(int));
            FinalForm.Columns.Add("Prod_54I", typeof(int));
            FinalForm.Columns.Add("Prod_55R", typeof(int));
            FinalForm.Columns.Add("Prod_55I", typeof(int));
            FinalForm.Columns.Add("Prod_56R", typeof(int));
            FinalForm.Columns.Add("Prod_56I", typeof(int));
            FinalForm.Columns.Add("Prod_57R", typeof(int));
            FinalForm.Columns.Add("Prod_57I", typeof(int));
            FinalForm.Columns.Add("Prod_58R", typeof(int));
            FinalForm.Columns.Add("Prod_58I", typeof(int));
            FinalForm.Columns.Add("Prod_59R", typeof(int));
            FinalForm.Columns.Add("Prod_59I", typeof(int));
            FinalForm.Columns.Add("Prod_60R", typeof(int));
            FinalForm.Columns.Add("Prod_60I", typeof(int));
            FinalForm.Columns.Add("Prod_61R", typeof(int));
            FinalForm.Columns.Add("Prod_61I", typeof(int));
            FinalForm.Columns.Add("Prod_62R", typeof(int));
            FinalForm.Columns.Add("Prod_62I", typeof(int));
            FinalForm.Columns.Add("Prod_63R", typeof(int));
            FinalForm.Columns.Add("Prod_63I", typeof(int));
            FinalForm.Columns.Add("Prod_64R", typeof(int));
            FinalForm.Columns.Add("Prod_64I", typeof(int));
            FinalForm.Columns.Add("Prod_65R", typeof(int));
            FinalForm.Columns.Add("Prod_65I", typeof(int));

           
            String NomedaFormulaX1;
            String NumerodaFormulaX1;
            String NumerodaFormulaX2;

            DataTable db2 = new DataTable();

            db2.Columns.Add("TotalF", typeof(double));
            db2.Columns.Add("TotalR", typeof(double));
            db2.Columns.Add("Produto", typeof(String));

            DataTable db3 = new DataTable();
            db3.Columns.Add("CProg", typeof(string));
            db3.Columns.Add("CCliente", typeof(string));
            db3.Columns.Add("Batida", typeof(string));
            db3.Columns.Add("Nome", typeof(string));
            db3.Columns.Add("TotalR", typeof(double));
            db3.Columns.Add("TotalF", typeof(double));

            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);
            Conexao.Open();

            try
            {
                DataTable DataRelatorio = new DataTable();
                String SqlData = "Select * from relatorio where (str_to_date(dia,'%d/%m/%Y') >= str_to_date(@data1,'%d/%m/%Y')) and (str_to_date(dia,'%d/%m/%Y') <= str_to_date(@data2,'%d/%m/%Y'))";

                MySqlCommand ComandoData = new MySqlCommand(SqlData, Conexao);
                ComandoData.Parameters.AddWithValue("@data1", Dia1);
                ComandoData.Parameters.AddWithValue("@data2", Dia2);

                MySqlDataAdapter objAdapterData = new MySqlDataAdapter(ComandoData);

                objAdapterData.Fill(DataRelatorio);

                int LinhaDa = DataRelatorio.Rows.Count;
                LinhaDa = LinhaDa - 1;

                HorIn = DataRelatorio.Rows[0]["Hora"].ToString(); ;
                HorFi = DataRelatorio.Rows[LinhaDa]["Hora"].ToString(); ;



                //Loop de Fórmulas
                for (int t = 1; t <= 200; t++)
                {
                    DataTable produtos = new DataTable();
                    String NumForm = t.ToString();

                   

                    String Sql = "";

                    //if (radioButton1.Checked == true)
                    //{
                        Sql = "Select * from relatorio where (Form1 = @valor) and (str_to_date(dia,'%d/%m/%Y') >= str_to_date(@data1,'%d/%m/%Y')) and (str_to_date(dia,'%d/%m/%Y') <= str_to_date(@data2,'%d/%m/%Y'))";
                    //}
                    //else
                   //{
                        //Sql = "Select * from relatorio where (Form2 = @valor) and (str_to_date(dia,'%d/%m/%Y') >= str_to_date(@data1,'%d/%m/%Y')) and (str_to_date(dia,'%d/%m/%Y') <= str_to_date(@data2,'%d/%m/%Y'))";
                    //}

                    MySqlCommand Comando = new MySqlCommand(Sql, Conexao);
                    Comando.Parameters.AddWithValue("@valor", NumForm);
                    Comando.Parameters.AddWithValue("@data1", Dia1);
                    Comando.Parameters.AddWithValue("@data2", Dia2);
                    MySqlDataAdapter objAdapter = new MySqlDataAdapter(Comando);

                    objAdapter.Fill(produtos);

                    if (produtos.Rows.Count > 0)
                    {
                        int Linhas = produtos.Rows.Count;
                        int LinhasHora = Linhas - 1;
                        InicalBox.Text = HorIn;
                        FinalBox.Text = HorFi;

                        NomedaFormulaX1 = produtos.Rows[0]["Nome"].ToString();
                        NumerodaFormulaX1 = produtos.Rows[0]["Form1"].ToString();
                        NumerodaFormulaX2 = produtos.Rows[0]["Form2"].ToString();

                        resultadoForm.Clear();
                        for (int i = 0; i < Linhas; i++)//Esse
                        {
                            String Formx1 = produtos.Rows[i]["Form1"].ToString();
                            String Formx2 = produtos.Rows[i]["Form2"].ToString();

                            String Data = produtos.Rows[i]["Dia"].ToString();
                            String Hora = produtos.Rows[i]["Hora"].ToString();

                            String DataeHora = Data + " " + Hora;

                            String Formula;
                            String Sql1X;

                            if (radioButton1.Checked == true)
                            {
                                Sql1X = "Select * from cadastro.formulaideal where (Form1=@form) and (str_to_date(@DataeHora, '%d/%m/%Y %H:%i:%s') >=  str_to_date(CONCAT(dia,\" \", Hora),'%d/%m/%Y %H:%i:%s'))";
                                Formula = Formx1;
                            }
                            else
                            {
                                Sql1X = "Select * from cadastro.formulaideal where (Form2=@form) and (str_to_date(@DataeHora, '%d/%m/%Y %H:%i:%s') >=  str_to_date(CONCAT(dia,\" \", Hora),'%d/%m/%Y %H:%i:%s'))";
                                Formula = Formx2;
                            }

                            MySqlCommand Comando1x = new MySqlCommand(Sql1X, Conexao);
                            Comando1x.Parameters.AddWithValue("@form", Formula);
                            Comando1x.Parameters.AddWithValue("@DataeHora", DataeHora);

                            MySqlDataAdapter objAdapter1 = new MySqlDataAdapter(Comando1x);
                            DataTable temporario = new DataTable();
                            objAdapter1.Fill(temporario);

                            int linhaOK = temporario.Rows.Count;
                            linhaOK = linhaOK - 1;

                            if (temporario.Rows.Count > 0)
                            {

                                DataRow Row = resultadoForm.NewRow();
                                Row["Dia"] = temporario.Rows[linhaOK]["Dia"];
                                Row["Hora"] = temporario.Rows[linhaOK]["Hora"];
                                Row["Nome"] = temporario.Rows[linhaOK]["Nome"];

                                Row["Form1"] = temporario.Rows[linhaOK]["Form1"];
                                Row["Form2"] = temporario.Rows[linhaOK]["Form2"];
                                Row["Prod_1"] = temporario.Rows[linhaOK]["Prod_1"];
                                Row["Prod_2"] = temporario.Rows[linhaOK]["Prod_2"];
                                Row["Prod_3"] = temporario.Rows[linhaOK]["Prod_3"];
                                Row["Prod_4"] = temporario.Rows[linhaOK]["Prod_4"];
                                Row["Prod_5"] = temporario.Rows[linhaOK]["Prod_5"];
                                Row["Prod_6"] = temporario.Rows[linhaOK]["Prod_6"];
                                Row["Prod_7"] = temporario.Rows[linhaOK]["Prod_7"];
                                Row["Prod_8"] = temporario.Rows[linhaOK]["Prod_8"];
                                Row["Prod_9"] = temporario.Rows[linhaOK]["Prod_9"];
                                Row["Prod_10"] = temporario.Rows[linhaOK]["Prod_10"];
                                Row["Prod_11"] = temporario.Rows[linhaOK]["Prod_11"];
                                Row["Prod_12"] = temporario.Rows[linhaOK]["Prod_12"];
                                Row["Prod_13"] = temporario.Rows[linhaOK]["Prod_13"];
                                Row["Prod_14"] = temporario.Rows[linhaOK]["Prod_14"];
                                Row["Prod_15"] = temporario.Rows[linhaOK]["Prod_15"];
                                Row["Prod_16"] = temporario.Rows[linhaOK]["Prod_16"];
                                Row["Prod_17"] = temporario.Rows[linhaOK]["Prod_17"];
                                Row["Prod_18"] = temporario.Rows[linhaOK]["Prod_18"];
                                Row["Prod_19"] = temporario.Rows[linhaOK]["Prod_19"];
                                Row["Prod_20"] = temporario.Rows[linhaOK]["Prod_20"];
                                Row["Prod_21"] = temporario.Rows[linhaOK]["Prod_21"];
                                Row["Prod_22"] = temporario.Rows[linhaOK]["Prod_22"];
                                Row["Prod_23"] = temporario.Rows[linhaOK]["Prod_23"];
                                Row["Prod_24"] = temporario.Rows[linhaOK]["Prod_24"];
                                Row["Prod_25"] = temporario.Rows[linhaOK]["Prod_25"];
                                Row["Prod_26"] = temporario.Rows[linhaOK]["Prod_26"];
                                Row["Prod_27"] = temporario.Rows[linhaOK]["Prod_27"];
                                Row["Prod_28"] = temporario.Rows[linhaOK]["Prod_28"];
                                Row["Prod_29"] = temporario.Rows[linhaOK]["Prod_29"];
                                Row["Prod_30"] = temporario.Rows[linhaOK]["Prod_30"];
                                Row["Prod_31"] = temporario.Rows[linhaOK]["Prod_31"];
                                Row["Prod_32"] = temporario.Rows[linhaOK]["Prod_32"];
                                Row["Prod_33"] = temporario.Rows[linhaOK]["Prod_33"];
                                Row["Prod_34"] = temporario.Rows[linhaOK]["Prod_34"];
                                Row["Prod_35"] = temporario.Rows[linhaOK]["Prod_35"];
                                Row["Prod_36"] = temporario.Rows[linhaOK]["Prod_36"];
                                Row["Prod_37"] = temporario.Rows[linhaOK]["Prod_37"];
                                Row["Prod_38"] = temporario.Rows[linhaOK]["Prod_38"];
                                Row["Prod_39"] = temporario.Rows[linhaOK]["Prod_39"];
                                Row["Prod_40"] = temporario.Rows[linhaOK]["Prod_40"];
                                Row["Prod_41"] = temporario.Rows[linhaOK]["Prod_41"];
                                Row["Prod_42"] = temporario.Rows[linhaOK]["Prod_42"];
                                Row["Prod_43"] = temporario.Rows[linhaOK]["Prod_43"];
                                Row["Prod_44"] = temporario.Rows[linhaOK]["Prod_44"];
                                Row["Prod_45"] = temporario.Rows[linhaOK]["Prod_45"];
                                Row["Prod_46"] = temporario.Rows[linhaOK]["Prod_46"];
                                Row["Prod_47"] = temporario.Rows[linhaOK]["Prod_47"];
                                Row["Prod_48"] = temporario.Rows[linhaOK]["Prod_48"];
                                Row["Prod_49"] = temporario.Rows[linhaOK]["Prod_49"];
                                Row["Prod_50"] = temporario.Rows[linhaOK]["Prod_50"];
                                Row["Prod_51"] = temporario.Rows[linhaOK]["Prod_51"];
                                Row["Prod_52"] = temporario.Rows[linhaOK]["Prod_52"];
                                Row["Prod_53"] = temporario.Rows[linhaOK]["Prod_53"];
                                Row["Prod_54"] = temporario.Rows[linhaOK]["Prod_54"];
                                Row["Prod_55"] = temporario.Rows[linhaOK]["Prod_55"];
                                Row["Prod_56"] = temporario.Rows[linhaOK]["Prod_56"];
                                Row["Prod_57"] = temporario.Rows[linhaOK]["Prod_57"];
                                Row["Prod_58"] = temporario.Rows[linhaOK]["Prod_58"];
                                Row["Prod_59"] = temporario.Rows[linhaOK]["Prod_59"];
                                Row["Prod_60"] = temporario.Rows[linhaOK]["Prod_60"];
                                Row["Prod_61"] = temporario.Rows[linhaOK]["Prod_61"];
                                Row["Prod_62"] = temporario.Rows[linhaOK]["Prod_62"];
                                Row["Prod_63"] = temporario.Rows[linhaOK]["Prod_63"];
                                Row["Prod_64"] = temporario.Rows[linhaOK]["Prod_64"];
                                Row["Prod_65"] = temporario.Rows[linhaOK]["Prod_65"];
                               
                                resultadoForm.Rows.Add(Row);

                                DataRow RowX = FinalForm.NewRow();
                                RowX["DiaI"] = temporario.Rows[linhaOK]["Dia"];
                                RowX["HoraI"] = temporario.Rows[linhaOK]["Hora"];
                                RowX["NomeI"] = temporario.Rows[linhaOK]["Nome"];

                                RowX["DiaR"] = produtos.Rows[i]["Dia"];
                                RowX["HoraR"] = produtos.Rows[i]["Hora"];
                                RowX["NomeR"] = produtos.Rows[i]["Nome"];


                                RowX["Form1I"] = temporario.Rows[linhaOK]["Form1"];
                                RowX["Form2I"] = temporario.Rows[linhaOK]["Form2"];
                                RowX["Prod_1I"] = temporario.Rows[linhaOK]["Prod_1"];
                                RowX["Prod_2I"] = temporario.Rows[linhaOK]["Prod_2"];
                                RowX["Prod_3I"] = temporario.Rows[linhaOK]["Prod_3"];
                                RowX["Prod_4I"] = temporario.Rows[linhaOK]["Prod_4"];
                                RowX["Prod_5I"] = temporario.Rows[linhaOK]["Prod_5"];
                                RowX["Prod_6I"] = temporario.Rows[linhaOK]["Prod_6"];
                                RowX["Prod_7I"] = temporario.Rows[linhaOK]["Prod_7"];
                                RowX["Prod_8I"] = temporario.Rows[linhaOK]["Prod_8"];
                                RowX["Prod_9I"] = temporario.Rows[linhaOK]["Prod_9"];
                                RowX["Prod_10I"] = temporario.Rows[linhaOK]["Prod_10"];
                                RowX["Prod_11I"] = temporario.Rows[linhaOK]["Prod_11"];
                                RowX["Prod_12I"] = temporario.Rows[linhaOK]["Prod_12"];
                                RowX["Prod_13I"] = temporario.Rows[linhaOK]["Prod_13"];
                                RowX["Prod_14I"] = temporario.Rows[linhaOK]["Prod_14"];
                                RowX["Prod_15I"] = temporario.Rows[linhaOK]["Prod_15"];
                                RowX["Prod_16I"] = temporario.Rows[linhaOK]["Prod_16"];
                                RowX["Prod_17I"] = temporario.Rows[linhaOK]["Prod_17"];
                                RowX["Prod_18I"] = temporario.Rows[linhaOK]["Prod_18"];
                                RowX["Prod_19I"] = temporario.Rows[linhaOK]["Prod_19"];
                                RowX["Prod_20I"] = temporario.Rows[linhaOK]["Prod_20"];
                                RowX["Prod_21I"] = temporario.Rows[linhaOK]["Prod_21"];
                                RowX["Prod_22I"] = temporario.Rows[linhaOK]["Prod_22"];
                                RowX["Prod_23I"] = temporario.Rows[linhaOK]["Prod_23"];
                                RowX["Prod_24I"] = temporario.Rows[linhaOK]["Prod_24"];
                                RowX["Prod_25I"] = temporario.Rows[linhaOK]["Prod_25"];
                                RowX["Prod_26I"] = temporario.Rows[linhaOK]["Prod_26"];
                                RowX["Prod_27I"] = temporario.Rows[linhaOK]["Prod_27"];
                                RowX["Prod_28I"] = temporario.Rows[linhaOK]["Prod_28"];
                                RowX["Prod_29I"] = temporario.Rows[linhaOK]["Prod_29"];
                                RowX["Prod_30I"] = temporario.Rows[linhaOK]["Prod_30"];
                                RowX["Prod_31I"] = temporario.Rows[linhaOK]["Prod_31"];
                                RowX["Prod_32I"] = temporario.Rows[linhaOK]["Prod_32"];
                                RowX["Prod_33I"] = temporario.Rows[linhaOK]["Prod_33"];
                                RowX["Prod_34I"] = temporario.Rows[linhaOK]["Prod_34"];
                                RowX["Prod_35I"] = temporario.Rows[linhaOK]["Prod_35"];
                                RowX["Prod_36I"] = temporario.Rows[linhaOK]["Prod_36"];
                                RowX["Prod_37I"] = temporario.Rows[linhaOK]["Prod_37"];
                                RowX["Prod_38I"] = temporario.Rows[linhaOK]["Prod_38"];
                                RowX["Prod_39I"] = temporario.Rows[linhaOK]["Prod_39"];
                                RowX["Prod_40I"] = temporario.Rows[linhaOK]["Prod_40"];
                                RowX["Prod_41I"] = temporario.Rows[linhaOK]["Prod_41"];
                                RowX["Prod_42I"] = temporario.Rows[linhaOK]["Prod_42"];
                                RowX["Prod_43I"] = temporario.Rows[linhaOK]["Prod_43"];
                                RowX["Prod_44I"] = temporario.Rows[linhaOK]["Prod_44"];
                                RowX["Prod_45I"] = temporario.Rows[linhaOK]["Prod_45"];
                                RowX["Prod_46I"] = temporario.Rows[linhaOK]["Prod_46"];
                                RowX["Prod_47I"] = temporario.Rows[linhaOK]["Prod_47"];
                                RowX["Prod_48I"] = temporario.Rows[linhaOK]["Prod_48"];
                                RowX["Prod_49I"] = temporario.Rows[linhaOK]["Prod_49"];
                                RowX["Prod_50I"] = temporario.Rows[linhaOK]["Prod_50"];
                                RowX["Prod_51I"] = temporario.Rows[linhaOK]["Prod_51"];
                                RowX["Prod_52I"] = temporario.Rows[linhaOK]["Prod_52"];
                                RowX["Prod_53I"] = temporario.Rows[linhaOK]["Prod_53"];
                                RowX["Prod_54I"] = temporario.Rows[linhaOK]["Prod_54"];
                                RowX["Prod_55I"] = temporario.Rows[linhaOK]["Prod_55"];
                                RowX["Prod_56I"] = temporario.Rows[linhaOK]["Prod_56"];
                                RowX["Prod_57I"] = temporario.Rows[linhaOK]["Prod_57"];
                                RowX["Prod_58I"] = temporario.Rows[linhaOK]["Prod_58"];
                                RowX["Prod_59I"] = temporario.Rows[linhaOK]["Prod_59"];
                                RowX["Prod_60I"] = temporario.Rows[linhaOK]["Prod_60"];
                                RowX["Prod_61I"] = temporario.Rows[linhaOK]["Prod_61"];
                                RowX["Prod_62I"] = temporario.Rows[linhaOK]["Prod_62"];
                                RowX["Prod_63I"] = temporario.Rows[linhaOK]["Prod_63"];
                                RowX["Prod_64I"] = temporario.Rows[linhaOK]["Prod_64"];
                                RowX["Prod_65I"] = temporario.Rows[linhaOK]["Prod_65"];
                               

                                RowX["Form1R"] = produtos.Rows[i]["Form1"];
                                RowX["Form2R"] = produtos.Rows[i]["Form2"];
                                RowX["Prod_1R"] = produtos.Rows[i]["Prod_1"];
                                RowX["Prod_2R"] = produtos.Rows[i]["Prod_2"];
                                RowX["Prod_3R"] = produtos.Rows[i]["Prod_3"];
                                RowX["Prod_4R"] = produtos.Rows[i]["Prod_4"];
                                RowX["Prod_5R"] = produtos.Rows[i]["Prod_5"];
                                RowX["Prod_6R"] = produtos.Rows[i]["Prod_6"];
                                RowX["Prod_7R"] = produtos.Rows[i]["Prod_7"];
                                RowX["Prod_8R"] = produtos.Rows[i]["Prod_8"];
                                RowX["Prod_9R"] = produtos.Rows[i]["Prod_9"];
                                RowX["Prod_10R"] = produtos.Rows[i]["Prod_10"];
                                RowX["Prod_11R"] = produtos.Rows[i]["Prod_11"];
                                RowX["Prod_12R"] = produtos.Rows[i]["Prod_12"];
                                RowX["Prod_13R"] = produtos.Rows[i]["Prod_13"];
                                RowX["Prod_14R"] = produtos.Rows[i]["Prod_14"];
                                RowX["Prod_15R"] = produtos.Rows[i]["Prod_15"];
                                RowX["Prod_16R"] = produtos.Rows[i]["Prod_16"];
                                RowX["Prod_17R"] = produtos.Rows[i]["Prod_17"];
                                RowX["Prod_18R"] = produtos.Rows[i]["Prod_18"];
                                RowX["Prod_19R"] = produtos.Rows[i]["Prod_19"];
                                RowX["Prod_20R"] = produtos.Rows[i]["Prod_20"];
                                RowX["Prod_21R"] = produtos.Rows[i]["Prod_21"];
                                RowX["Prod_22R"] = produtos.Rows[i]["Prod_22"];
                                RowX["Prod_23R"] = produtos.Rows[i]["Prod_23"];
                                RowX["Prod_24R"] = produtos.Rows[i]["Prod_24"];
                                RowX["Prod_25R"] = produtos.Rows[i]["Prod_25"];
                                RowX["Prod_26R"] = produtos.Rows[i]["Prod_26"];
                                RowX["Prod_27R"] = produtos.Rows[i]["Prod_27"];
                                RowX["Prod_28R"] = produtos.Rows[i]["Prod_28"];
                                RowX["Prod_29R"] = produtos.Rows[i]["Prod_29"];
                                RowX["Prod_30R"] = produtos.Rows[i]["Prod_30"];
                                RowX["Prod_31R"] = produtos.Rows[i]["Prod_31"];
                                RowX["Prod_32R"] = produtos.Rows[i]["Prod_32"];
                                RowX["Prod_33R"] = produtos.Rows[i]["Prod_33"];
                                RowX["Prod_34R"] = produtos.Rows[i]["Prod_34"];
                                RowX["Prod_35R"] = produtos.Rows[i]["Prod_35"];
                                RowX["Prod_36R"] = produtos.Rows[i]["Prod_36"];
                                RowX["Prod_37R"] = produtos.Rows[i]["Prod_37"];
                                RowX["Prod_38R"] = produtos.Rows[i]["Prod_38"];
                                RowX["Prod_39R"] = produtos.Rows[i]["Prod_39"];
                                RowX["Prod_40R"] = produtos.Rows[i]["Prod_40"];
                                RowX["Prod_41R"] = produtos.Rows[i]["Prod_41"];
                                RowX["Prod_42R"] = produtos.Rows[i]["Prod_42"];
                                RowX["Prod_43R"] = produtos.Rows[i]["Prod_43"];
                                RowX["Prod_44R"] = produtos.Rows[i]["Prod_44"];
                                RowX["Prod_45R"] = produtos.Rows[i]["Prod_45"];
                                RowX["Prod_46R"] = produtos.Rows[i]["Prod_46"];
                                RowX["Prod_47R"] = produtos.Rows[i]["Prod_47"];
                                RowX["Prod_48R"] = produtos.Rows[i]["Prod_48"];
                                RowX["Prod_49R"] = produtos.Rows[i]["Prod_49"];
                                RowX["Prod_50R"] = produtos.Rows[i]["Prod_50"];
                                RowX["Prod_51R"] = produtos.Rows[i]["Prod_51"];
                                RowX["Prod_52R"] = produtos.Rows[i]["Prod_52"];
                                RowX["Prod_53R"] = produtos.Rows[i]["Prod_53"];
                                RowX["Prod_54R"] = produtos.Rows[i]["Prod_54"];
                                RowX["Prod_55R"] = produtos.Rows[i]["Prod_55"];
                                RowX["Prod_56R"] = produtos.Rows[i]["Prod_56"];
                                RowX["Prod_57R"] = produtos.Rows[i]["Prod_57"];
                                RowX["Prod_58R"] = produtos.Rows[i]["Prod_58"];
                                RowX["Prod_59R"] = produtos.Rows[i]["Prod_59"];
                                RowX["Prod_60R"] = produtos.Rows[i]["Prod_60"];
                                RowX["Prod_61R"] = produtos.Rows[i]["Prod_61"];
                                RowX["Prod_62R"] = produtos.Rows[i]["Prod_62"];
                                RowX["Prod_63R"] = produtos.Rows[i]["Prod_63"];
                                RowX["Prod_64R"] = produtos.Rows[i]["Prod_64"];
                                RowX["Prod_65R"] = produtos.Rows[i]["Prod_65"];

                                

                                FinalForm.Rows.Add(RowX);



                            }
                            else
                            {
                                DataRow Row = resultadoForm.NewRow();
                                Row["Dia"] = 0;
                                Row["Hora"] = 0;
                                Row["Nome"] = 0;

                                Row["Form1"] = 0;
                                Row["Form2"] = 0;
                                Row["Prod_1"] = 0;
                                Row["Prod_2"] = 0;
                                Row["Prod_3"] = 0;
                                Row["Prod_4"] = 0;
                                Row["Prod_5"] = 0;
                                Row["Prod_6"] = 0;
                                Row["Prod_7"] = 0;
                                Row["Prod_8"] = 0;
                                Row["Prod_9"] = 0;
                                Row["Prod_10"] = 0;
                                Row["Prod_11"] = 0;
                                Row["Prod_12"] = 0;
                                Row["Prod_13"] = 0;
                                Row["Prod_14"] = 0;
                                Row["Prod_15"] = 0;
                                Row["Prod_16"] = 0;
                                Row["Prod_17"] = 0;
                                Row["Prod_18"] = 0;
                                Row["Prod_19"] = 0;
                                Row["Prod_20"] = 0;
                                Row["Prod_21"] = 0;
                                Row["Prod_22"] = 0;
                                Row["Prod_23"] = 0;
                                Row["Prod_24"] = 0;
                                Row["Prod_25"] = 0;
                                Row["Prod_26"] = 0;
                                Row["Prod_27"] = 0;
                                Row["Prod_28"] = 0;
                                Row["Prod_29"] = 0;
                                Row["Prod_30"] = 0;
                                Row["Prod_31"] = 0;
                                Row["Prod_32"] = 0;
                                Row["Prod_33"] = 0;
                                Row["Prod_34"] = 0;
                                Row["Prod_35"] = 0;
                                Row["Prod_36"] = 0;
                                Row["Prod_37"] = 0;
                                Row["Prod_38"] = 0;
                                Row["Prod_39"] = 0;
                                Row["Prod_40"] = 0;
                                Row["Prod_41"] = 0;
                                Row["Prod_42"] = 0;
                                Row["Prod_43"] = 0;
                                Row["Prod_44"] = 0;
                                Row["Prod_45"] = 0;
                                Row["Prod_46"] = 0;
                                Row["Prod_47"] = 0;
                                Row["Prod_48"] = 0;
                                Row["Prod_49"] = 0;
                                Row["Prod_50"] = 0;
                                Row["Prod_51"] = 0;
                                Row["Prod_52"] = 0;
                                Row["Prod_53"] = 0;
                                Row["Prod_54"] = 0;
                                Row["Prod_55"] = 0;
                                Row["Prod_56"] = 0;
                                Row["Prod_57"] = 0;
                                Row["Prod_58"] = 0;
                                Row["Prod_59"] = 0;
                                Row["Prod_60"] = 0;
                                Row["Prod_61"] = 0;
                                Row["Prod_62"] = 0;
                                Row["Prod_63"] = 0;
                                Row["Prod_64"] = 0;
                                Row["Prod_65"] = 0;
                               

                                resultadoForm.Rows.Add(Row);

                                DataRow RowX = FinalForm.NewRow();
                                RowX["DiaI"] = "---";
                                RowX["HoraI"] = "---";
                                RowX["NomeI"] = "---";

                                RowX["DiaR"] = produtos.Rows[i]["Dia"];
                                RowX["HoraR"] = produtos.Rows[i]["Hora"];
                                RowX["NomeR"] = produtos.Rows[i]["Nome"];


                                RowX["Form1I"] = 0;
                                RowX["Form2I"] = 0;
                                RowX["Prod_1I"] = 0;
                                RowX["Prod_2I"] = 0;
                                RowX["Prod_3I"] = 0;
                                RowX["Prod_4I"] = 0;
                                RowX["Prod_5I"] = 0;
                                RowX["Prod_6I"] = 0;
                                RowX["Prod_7I"] = 0;
                                RowX["Prod_8I"] = 0;
                                RowX["Prod_9I"] = 0;
                                RowX["Prod_10I"] = 0;
                                RowX["Prod_11I"] = 0;
                                RowX["Prod_12I"] = 0;
                                RowX["Prod_13I"] = 0;
                                RowX["Prod_14I"] = 0;
                                RowX["Prod_15I"] = 0;
                                RowX["Prod_16I"] = 0;
                                RowX["Prod_17I"] = 0;
                                RowX["Prod_18I"] = 0;
                                RowX["Prod_19I"] = 0;
                                RowX["Prod_20I"] = 0;
                                RowX["Prod_21I"] = 0;
                                RowX["Prod_22I"] = 0;
                                RowX["Prod_23I"] = 0;
                                RowX["Prod_24I"] = 0;
                                RowX["Prod_25I"] = 0;
                                RowX["Prod_26I"] = 0;
                                RowX["Prod_27I"] = 0;
                                RowX["Prod_28I"] = 0;
                                RowX["Prod_29I"] = 0;
                                RowX["Prod_30I"] = 0;
                                RowX["Prod_31I"] = 0;
                                RowX["Prod_32I"] = 0;
                                RowX["Prod_33I"] = 0;
                                RowX["Prod_34I"] = 0;
                                RowX["Prod_35I"] = 0;
                                RowX["Prod_36I"] = 0;
                                RowX["Prod_37I"] = 0;
                                RowX["Prod_38I"] = 0;
                                RowX["Prod_39I"] = 0;
                                RowX["Prod_40I"] = 0;
                                RowX["Prod_41I"] = 0;
                                RowX["Prod_42I"] = 0;
                                RowX["Prod_43I"] = 0;
                                RowX["Prod_44I"] = 0;
                                RowX["Prod_45I"] = 0;
                                RowX["Prod_46I"] = 0;
                                RowX["Prod_47I"] = 0;
                                RowX["Prod_48I"] = 0;
                                RowX["Prod_49I"] = 0;
                                RowX["Prod_50I"] = 0;
                                RowX["Prod_51I"] = 0;
                                RowX["Prod_52I"] = 0;
                                RowX["Prod_53I"] = 0;
                                RowX["Prod_54I"] = 0;
                                RowX["Prod_55I"] = 0;
                                RowX["Prod_56I"] = 0;
                                RowX["Prod_57I"] = 0;
                                RowX["Prod_58I"] = 0;
                                RowX["Prod_59I"] = 0;
                                RowX["Prod_60I"] = 0;
                                RowX["Prod_61I"] = 0;
                                RowX["Prod_62I"] = 0;
                                RowX["Prod_63I"] = 0;
                                RowX["Prod_64I"] = 0;
                                RowX["Prod_65I"] = 0;
                                


                                RowX["Form1R"] = produtos.Rows[i]["Form1"];
                                RowX["Form2R"] = produtos.Rows[i]["Form2"];
                                RowX["Prod_1R"] = produtos.Rows[i]["Prod_1"];
                                RowX["Prod_2R"] = produtos.Rows[i]["Prod_2"];
                                RowX["Prod_3R"] = produtos.Rows[i]["Prod_3"];
                                RowX["Prod_4R"] = produtos.Rows[i]["Prod_4"];
                                RowX["Prod_5R"] = produtos.Rows[i]["Prod_5"];
                                RowX["Prod_6R"] = produtos.Rows[i]["Prod_6"];
                                RowX["Prod_7R"] = produtos.Rows[i]["Prod_7"];
                                RowX["Prod_8R"] = produtos.Rows[i]["Prod_8"];
                                RowX["Prod_9R"] = produtos.Rows[i]["Prod_9"];
                                RowX["Prod_10R"] = produtos.Rows[i]["Prod_10"];
                                RowX["Prod_11R"] = produtos.Rows[i]["Prod_11"];
                                RowX["Prod_12R"] = produtos.Rows[i]["Prod_12"];
                                RowX["Prod_13R"] = produtos.Rows[i]["Prod_13"];
                                RowX["Prod_14R"] = produtos.Rows[i]["Prod_14"];
                                RowX["Prod_15R"] = produtos.Rows[i]["Prod_15"];
                                RowX["Prod_16R"] = produtos.Rows[i]["Prod_16"];
                                RowX["Prod_17R"] = produtos.Rows[i]["Prod_17"];
                                RowX["Prod_18R"] = produtos.Rows[i]["Prod_18"];
                                RowX["Prod_19R"] = produtos.Rows[i]["Prod_19"];
                                RowX["Prod_20R"] = produtos.Rows[i]["Prod_20"];
                                RowX["Prod_21R"] = produtos.Rows[i]["Prod_21"];
                                RowX["Prod_22R"] = produtos.Rows[i]["Prod_22"];
                                RowX["Prod_23R"] = produtos.Rows[i]["Prod_23"];
                                RowX["Prod_24R"] = produtos.Rows[i]["Prod_24"];
                                RowX["Prod_25R"] = produtos.Rows[i]["Prod_25"];
                                RowX["Prod_26R"] = produtos.Rows[i]["Prod_26"];
                                RowX["Prod_27R"] = produtos.Rows[i]["Prod_27"];
                                RowX["Prod_28R"] = produtos.Rows[i]["Prod_28"];
                                RowX["Prod_29R"] = produtos.Rows[i]["Prod_29"];
                                RowX["Prod_30R"] = produtos.Rows[i]["Prod_30"];
                                RowX["Prod_31R"] = produtos.Rows[i]["Prod_31"];
                                RowX["Prod_32R"] = produtos.Rows[i]["Prod_32"];
                                RowX["Prod_33R"] = produtos.Rows[i]["Prod_33"];
                                RowX["Prod_34R"] = produtos.Rows[i]["Prod_34"];
                                RowX["Prod_35R"] = produtos.Rows[i]["Prod_35"];
                                RowX["Prod_36R"] = produtos.Rows[i]["Prod_36"];
                                RowX["Prod_37R"] = produtos.Rows[i]["Prod_37"];
                                RowX["Prod_38R"] = produtos.Rows[i]["Prod_38"];
                                RowX["Prod_39R"] = produtos.Rows[i]["Prod_39"];
                                RowX["Prod_40R"] = produtos.Rows[i]["Prod_40"];
                                RowX["Prod_41R"] = produtos.Rows[i]["Prod_41"];
                                RowX["Prod_42R"] = produtos.Rows[i]["Prod_42"];
                                RowX["Prod_43R"] = produtos.Rows[i]["Prod_43"];
                                RowX["Prod_44R"] = produtos.Rows[i]["Prod_44"];
                                RowX["Prod_45R"] = produtos.Rows[i]["Prod_45"];
                                RowX["Prod_46R"] = produtos.Rows[i]["Prod_46"];
                                RowX["Prod_47R"] = produtos.Rows[i]["Prod_47"];
                                RowX["Prod_48R"] = produtos.Rows[i]["Prod_48"];
                                RowX["Prod_49R"] = produtos.Rows[i]["Prod_49"];
                                RowX["Prod_50R"] = produtos.Rows[i]["Prod_50"];
                                RowX["Prod_51R"] = produtos.Rows[i]["Prod_51"];
                                RowX["Prod_52R"] = produtos.Rows[i]["Prod_52"];
                                RowX["Prod_53R"] = produtos.Rows[i]["Prod_53"];
                                RowX["Prod_54R"] = produtos.Rows[i]["Prod_54"];
                                RowX["Prod_55R"] = produtos.Rows[i]["Prod_55"];
                                RowX["Prod_56R"] = produtos.Rows[i]["Prod_56"];
                                RowX["Prod_57R"] = produtos.Rows[i]["Prod_57"];
                                RowX["Prod_58R"] = produtos.Rows[i]["Prod_58"];
                                RowX["Prod_59R"] = produtos.Rows[i]["Prod_59"];
                                RowX["Prod_60R"] = produtos.Rows[i]["Prod_60"];
                                RowX["Prod_61R"] = produtos.Rows[i]["Prod_61"];
                                RowX["Prod_62R"] = produtos.Rows[i]["Prod_62"];
                                RowX["Prod_63R"] = produtos.Rows[i]["Prod_63"];
                                RowX["Prod_64R"] = produtos.Rows[i]["Prod_64"];
                                RowX["Prod_65R"] = produtos.Rows[i]["Prod_65"];
                               




                                FinalForm.Rows.Add(RowX);
                            }
                            //ComparaGrid1.DataSource = FinalForm;


                        }

                        String[] ProdCamp1 = new String[66];
                        Object[] sumObject2 = new Object[66];
                        Object[] sumObject3 = new Object[66];
                        ProdCamp1[0] = "Prod_1";
                        ProdCamp1[1] = "Prod_2";
                        ProdCamp1[2] = "Prod_3";
                        ProdCamp1[3] = "Prod_4";
                        ProdCamp1[4] = "Prod_5";
                        ProdCamp1[5] = "Prod_6";
                        ProdCamp1[6] = "Prod_7";
                        ProdCamp1[7] = "Prod_8";
                        ProdCamp1[8] = "Prod_9";
                        ProdCamp1[9] = "Prod_10";
                        ProdCamp1[10] = "Prod_11";
                        ProdCamp1[11] = "Prod_12";
                        ProdCamp1[12] = "Prod_13";
                        ProdCamp1[13] = "Prod_14";
                        ProdCamp1[14] = "Prod_15";
                        ProdCamp1[15] = "Prod_16";
                        ProdCamp1[16] = "Prod_17";
                        ProdCamp1[17] = "Prod_18";
                        ProdCamp1[18] = "Prod_19";
                        ProdCamp1[19] = "Prod_20";
                        ProdCamp1[20] = "Prod_21";
                        ProdCamp1[21] = "Prod_22";
                        ProdCamp1[22] = "Prod_23";
                        ProdCamp1[23] = "Prod_24";
                        ProdCamp1[24] = "Prod_25";
                        ProdCamp1[25] = "Prod_26";
                        ProdCamp1[26] = "Prod_27";
                        ProdCamp1[27] = "Prod_28";
                        ProdCamp1[28] = "Prod_29";
                        ProdCamp1[29] = "Prod_30";
                        ProdCamp1[30] = "Prod_31";
                        ProdCamp1[31] = "Prod_32";
                        ProdCamp1[32] = "Prod_33";
                        ProdCamp1[33] = "Prod_34";
                        ProdCamp1[34] = "Prod_35";
                        ProdCamp1[35] = "Prod_36";
                        ProdCamp1[36] = "Prod_37";
                        ProdCamp1[37] = "Prod_38";
                        ProdCamp1[38] = "Prod_39";
                        ProdCamp1[39] = "Prod_40";
                        ProdCamp1[40] = "Prod_41";
                        ProdCamp1[41] = "Prod_42";
                        ProdCamp1[42] = "Prod_43";
                        ProdCamp1[43] = "Prod_44";
                        ProdCamp1[44] = "Prod_45";
                        ProdCamp1[45] = "Prod_46";
                        ProdCamp1[46] = "Prod_47";
                        ProdCamp1[47] = "Prod_48";
                        ProdCamp1[48] = "Prod_49";
                        ProdCamp1[49] = "Prod_50";
                        ProdCamp1[50] = "Prod_51";
                        ProdCamp1[51] = "Prod_52";
                        ProdCamp1[52] = "Prod_53";
                        ProdCamp1[53] = "Prod_54";
                        ProdCamp1[54] = "Prod_55";
                        ProdCamp1[55] = "Prod_56";
                        ProdCamp1[56] = "Prod_57";
                        ProdCamp1[57] = "Prod_58";
                        ProdCamp1[58] = "Prod_59";
                        ProdCamp1[59] = "Prod_60";
                        ProdCamp1[60] = "Prod_61";
                        ProdCamp1[61] = "Prod_62";
                        ProdCamp1[62] = "Prod_63";
                        ProdCamp1[63] = "Prod_64";
                        ProdCamp1[64] = "Prod_65";
                        
                        Double TotalIdeal = 0;
                        Double TotalReal = 0;

                      
                        if (produtos.Rows.Count > 0)
                        {
                            for (int i = 0; i <= 64; i++)
                            {

                                Object SummerHits1;
                                Object SummerHits2;

                                SummerHits1 = produtos.Compute("Sum(" + ProdCamp1[i] + ")", "");
                                SummerHits2 = resultadoForm.Compute("Sum(" + ProdCamp1[i] + ")", "");

                                Double Medida = Convert.ToDouble(TabelasT.NomeProduto.Rows[i]["Medida"].ToString());


                                if (!DBNull.Value.Equals(SummerHits1))
                                {
                                    DataRow Row1 = db2.NewRow();
                                    Double Valor = Convert.ToDouble(SummerHits1);
                                    if (Valor != 0)
                                    {
                                        

                                        if (!DBNull.Value.Equals(SummerHits2))
                                        {
                                            Double Valor1 = Convert.ToDouble(SummerHits2);
                                            if (Valor1 != 0)
                                            {
                                                Double Tempsoma1 = Valor1 / Medida;
                                                TotalIdeal = TotalIdeal + Tempsoma1;
                                            }
                                            /*
                                            else
                                            {
                                                //TotalIdeal = 0;
                                            }
                                            */


                                            Double Tempsoma2 = Valor / Medida;
                                            TotalReal = TotalReal + Tempsoma2;
                                        }
                                    }


                                }

                            }

                        }

                        BatidasStr = produtos.Rows.Count.ToString("#,#");


                        DataRow Row1X = db3.NewRow();
                        Row1X["CProg"] = NumerodaFormulaX1;
                        Row1X["CCliente"] = NumerodaFormulaX2;
                        Row1X["Batida"] = BatidasStr;
                        Row1X["Nome"] = NomedaFormulaX1;
                        Row1X["TotalR"] = TotalReal;
                        Row1X["TotalF"] = TotalIdeal;
                        db3.Rows.Add(Row1X);


                    }
                }






                String[] ProdCamp5 = new String[66];
                String[] ProdCamp6 = new String[66];
                Object[] sumObject = new Object[66];
                Object[] sumObject1 = new Object[66];
                ProdCamp5[0] = "Prod_1R";
                ProdCamp5[1] = "Prod_2R";
                ProdCamp5[2] = "Prod_3R";
                ProdCamp5[3] = "Prod_4R";
                ProdCamp5[4] = "Prod_5R";
                ProdCamp5[5] = "Prod_6R";
                ProdCamp5[6] = "Prod_7R";
                ProdCamp5[7] = "Prod_8R";
                ProdCamp5[8] = "Prod_9R";
                ProdCamp5[9] = "Prod_10R";
                ProdCamp5[10] = "Prod_11R";
                ProdCamp5[11] = "Prod_12R";
                ProdCamp5[12] = "Prod_13R";
                ProdCamp5[13] = "Prod_14R";
                ProdCamp5[14] = "Prod_15R";
                ProdCamp5[15] = "Prod_16R";
                ProdCamp5[16] = "Prod_17R";
                ProdCamp5[17] = "Prod_18R";
                ProdCamp5[18] = "Prod_19R";
                ProdCamp5[19] = "Prod_20R";
                ProdCamp5[20] = "Prod_21R";
                ProdCamp5[21] = "Prod_22R";
                ProdCamp5[22] = "Prod_23R";
                ProdCamp5[23] = "Prod_24R";
                ProdCamp5[24] = "Prod_25R";
                ProdCamp5[25] = "Prod_26R";
                ProdCamp5[26] = "Prod_27R";
                ProdCamp5[27] = "Prod_28R";
                ProdCamp5[28] = "Prod_29R";
                ProdCamp5[29] = "Prod_30R";
                ProdCamp5[30] = "Prod_31R";
                ProdCamp5[31] = "Prod_32R";
                ProdCamp5[32] = "Prod_33R";
                ProdCamp5[33] = "Prod_34R";
                ProdCamp5[34] = "Prod_35R";
                ProdCamp5[35] = "Prod_36R";
                ProdCamp5[36] = "Prod_37R";
                ProdCamp5[37] = "Prod_38R";
                ProdCamp5[38] = "Prod_39R";
                ProdCamp5[39] = "Prod_40R";
                ProdCamp5[40] = "Prod_41R";
                ProdCamp5[41] = "Prod_42R";
                ProdCamp5[42] = "Prod_43R";
                ProdCamp5[43] = "Prod_44R";
                ProdCamp5[44] = "Prod_45R";
                ProdCamp5[45] = "Prod_46R";
                ProdCamp5[46] = "Prod_47R";
                ProdCamp5[47] = "Prod_48R";
                ProdCamp5[48] = "Prod_49R";
                ProdCamp5[49] = "Prod_50R";
                ProdCamp5[50] = "Prod_51R";
                ProdCamp5[51] = "Prod_52R";
                ProdCamp5[52] = "Prod_53R";
                ProdCamp5[53] = "Prod_54R";
                ProdCamp5[54] = "Prod_55R";
                ProdCamp5[55] = "Prod_56R";
                ProdCamp5[56] = "Prod_57R";
                ProdCamp5[57] = "Prod_58R";
                ProdCamp5[58] = "Prod_59R";
                ProdCamp5[59] = "Prod_60R";
                ProdCamp5[60] = "Prod_61R";
                ProdCamp5[61] = "Prod_62R";
                ProdCamp5[62] = "Prod_63R";
                ProdCamp5[63] = "Prod_64R";
                ProdCamp5[64] = "Prod_65R";


                ProdCamp6[0] = "Prod_1I";
                ProdCamp6[1] = "Prod_2I";
                ProdCamp6[2] = "Prod_3I";
                ProdCamp6[3] = "Prod_4I";
                ProdCamp6[4] = "Prod_5I";
                ProdCamp6[5] = "Prod_6I";
                ProdCamp6[6] = "Prod_7I";
                ProdCamp6[7] = "Prod_8I";
                ProdCamp6[8] = "Prod_9I";
                ProdCamp6[9] = "Prod_10I";
                ProdCamp6[10] = "Prod_11I";
                ProdCamp6[11] = "Prod_12I";
                ProdCamp6[12] = "Prod_13I";
                ProdCamp6[13] = "Prod_14I";
                ProdCamp6[14] = "Prod_15I";
                ProdCamp6[15] = "Prod_16I";
                ProdCamp6[16] = "Prod_17I";
                ProdCamp6[17] = "Prod_18I";
                ProdCamp6[18] = "Prod_19I";
                ProdCamp6[19] = "Prod_20I";
                ProdCamp6[20] = "Prod_21I";
                ProdCamp6[21] = "Prod_22I";
                ProdCamp6[22] = "Prod_23I";
                ProdCamp6[23] = "Prod_24I";
                ProdCamp6[24] = "Prod_25I";
                ProdCamp6[25] = "Prod_26I";
                ProdCamp6[26] = "Prod_27I";
                ProdCamp6[27] = "Prod_28I";
                ProdCamp6[28] = "Prod_29I";
                ProdCamp6[29] = "Prod_30I";
                ProdCamp6[30] = "Prod_31I";
                ProdCamp6[31] = "Prod_32I";
                ProdCamp6[32] = "Prod_33I";
                ProdCamp6[33] = "Prod_34I";
                ProdCamp6[34] = "Prod_35I";
                ProdCamp6[35] = "Prod_36I";
                ProdCamp6[36] = "Prod_37I";
                ProdCamp6[37] = "Prod_38I";
                ProdCamp6[38] = "Prod_39I";
                ProdCamp6[39] = "Prod_40I";
                ProdCamp6[40] = "Prod_41I";
                ProdCamp6[41] = "Prod_42I";
                ProdCamp6[42] = "Prod_43I";
                ProdCamp6[43] = "Prod_44I";
                ProdCamp6[44] = "Prod_45I";
                ProdCamp6[45] = "Prod_46I";
                ProdCamp6[46] = "Prod_47I";
                ProdCamp6[47] = "Prod_48I";
                ProdCamp6[48] = "Prod_49I";
                ProdCamp6[49] = "Prod_50I";
                ProdCamp6[50] = "Prod_51I";
                ProdCamp6[51] = "Prod_52I";
                ProdCamp6[52] = "Prod_53I";
                ProdCamp6[53] = "Prod_54I";
                ProdCamp6[54] = "Prod_55I";
                ProdCamp6[55] = "Prod_56I";
                ProdCamp6[56] = "Prod_57I";
                ProdCamp6[57] = "Prod_58I";
                ProdCamp6[58] = "Prod_59I";
                ProdCamp6[59] = "Prod_60I";
                ProdCamp6[60] = "Prod_61I";
                ProdCamp6[61] = "Prod_62I";
                ProdCamp6[62] = "Prod_63I";
                ProdCamp6[63] = "Prod_64I";
                ProdCamp6[64] = "Prod_65I";
                


                if (resultadoForm.Rows.Count > 0)
                {
                    for (int i = 0; i <= 64; i++)
                    {


                        sumObject[i] = FinalForm.Compute("Sum(" + ProdCamp5[i] + ")", "");
                        sumObject1[i] = FinalForm.Compute("Sum(" + ProdCamp6[i] + ")", "");




                        if (!DBNull.Value.Equals(sumObject[i]))
                        {
                            DataRow Row1 = db2.NewRow();
                            Double Valor = Convert.ToDouble(sumObject[i]);
                            if (Valor != 0)
                            {
                                Double Medida = Convert.ToDouble(TabelasT.NomeProduto.Rows[i]["Medida"].ToString());

                                if (!DBNull.Value.Equals(sumObject1[i]))
                                {
                                    Double Valor1 = Convert.ToDouble(sumObject1[i]);
                                    if (Valor1 != 0)
                                    {
                                        Row1["TotalF"] = Valor1 / Medida;
                                    }
                                    else
                                    {
                                        Row1["TotalF"] = 0;
                                    }

                                    if (Valor == 0 && Valor1 == 0)
                                    {
                                        TabelasT.Visivel1[i] = false;
                                        TabelasT.Visivel2[i] = false;
                                    }

                                    else
                                    {
                                        if (HabFormIdeal.Checked == false) { TabelasT.Visivel2[i] = false; } else { TabelasT.Visivel2[i] = true; }
                                        TabelasT.Visivel1[i] = true;
                                    }



                                    Row1["TotalR"] = Valor / Medida;
                                    Row1["Produto"] = TabelasT.NomeProduto.Rows[i]["Produto"].ToString();
                                    db2.Rows.Add(Row1);
                                }
                            }


                        }

                    }

                }




                object sumTotal;
                sumTotal = db2.Compute("Sum(TotalR)", "");

                object sumTotal2;
                sumTotal2 = db2.Compute("Sum(TotalF)", "");


                double ToTalTot=Convert.ToDouble(sumTotal);
                double ToTalTot2=Convert.ToDouble(sumTotal2);



                //if (ToTalTot != 0 && ToTalTot2!=0)
                //{



                // }

                TotalBox.Text = ToTalTot.ToString("#,0.000");
                if (HabFormIdeal.Checked == true)
                {
                    Totalbox2.Text = ToTalTot2.ToString("#,0.000");
                    TabelasT.Total2 = ToTalTot2.ToString("#,0.000");
                }

                TabelasT.Total1 = ToTalTot.ToString("#,0.000");


                TBatidaBox.Text = FinalForm.Rows.Count.ToString("#,#");
                TabelasT.Batida1 = FinalForm.Rows.Count.ToString("#,#");

                for (int i = 0; i < 65; i++)
                {
                    String Temp1 = TabelasT.NomeProduto.Rows[i]["Produto"].ToString() + " Form.";
                    String Temp2 = TabelasT.NomeProduto.Rows[i]["Produto"].ToString() + " Real";
                    FinalForm.Columns[ProdCamp3[i]].ColumnName = Temp1;
                    FinalForm.Columns[ProdCamp4[i]].ColumnName = Temp2;
                }
            }




            catch (MySqlException erro)
            {
                MessageBox.Show("Erro na Conexão: " + erro);
            }
            Conexao.Close();
            tabelagrid.DataSource = FinalForm;
            dataGridView1.DataSource = db3;

            dataGridView2.DataSource = db2;
            TabelasT.DataTable2 = db2;
            TabelasT.DataTable1 = db3;
            ArrumaGrid();

        }
        private void DetalhesFormula()
        {
            tabelagrid.Columns["DiaR"].DisplayIndex = 0;
            tabelagrid.Columns["HoraR"].DisplayIndex = 1;
            tabelagrid.Columns["Form1R"].DisplayIndex = 2;
            tabelagrid.Columns["Form2R"].DisplayIndex = 3;
            tabelagrid.Columns["NomeR"].DisplayIndex = 4;

            tabelagrid.Columns["DiaR"].Width = 80;
            tabelagrid.Columns["HoraR"].Width = 80;
            tabelagrid.Columns["Form1R"].Width = 50;
            tabelagrid.Columns["Form2R"].Width = 50;
            tabelagrid.Columns["NomeR"].Width = 100;

            tabelagrid.Columns["DiaR"].HeaderText = "Dia Real";
            tabelagrid.Columns["HoraR"].HeaderText = "Hora Real";
            tabelagrid.Columns["NomeR"].HeaderText = "Nome Form. Real";
            tabelagrid.Columns["Form1R"].HeaderText = "Cód. Prog.";
            tabelagrid.Columns["Form2R"].HeaderText = "Cód. Cliente";

            if (HabFormIdeal.Checked == true)
            {
                if (Option1.Checked == true)
                {
                    tabelagrid.Columns["DiaI"].Visible = true;
                    tabelagrid.Columns["HoraI"].Visible = true;
                    tabelagrid.Columns["HoraI"].Visible = true;
                    tabelagrid.Columns["Form1I"].Visible = true;
                    tabelagrid.Columns["Form2I"].Visible = true;

                    tabelagrid.Columns["DiaI"].Width = 80;
                    tabelagrid.Columns["HoraI"].Width = 80;
                    tabelagrid.Columns["Form1I"].Width = 50;
                    tabelagrid.Columns["Form2I"].Width = 50;
                    tabelagrid.Columns["NomeI"].Width = 100;

                    tabelagrid.Columns["DiaI"].DisplayIndex = 5;
                    tabelagrid.Columns["HoraI"].DisplayIndex = 6;
                    tabelagrid.Columns["Form1I"].DisplayIndex = 7;
                    tabelagrid.Columns["Form2I"].DisplayIndex = 8;
                    tabelagrid.Columns["NomeI"].DisplayIndex = 9;

                    tabelagrid.Columns["DiaI"].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    tabelagrid.Columns["HoraI"].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    tabelagrid.Columns["NomeI"].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    tabelagrid.Columns["Form1I"].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    tabelagrid.Columns["Form2I"].DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;

                    tabelagrid.Columns["DiaI"].HeaderCell.Style.BackColor = Color.LightGoldenrodYellow;
                    tabelagrid.Columns["HoraI"].HeaderCell.Style.BackColor = Color.LightGoldenrodYellow;
                    tabelagrid.Columns["NomeI"].HeaderCell.Style.BackColor = Color.LightGoldenrodYellow;
                    tabelagrid.Columns["Form1I"].HeaderCell.Style.BackColor = Color.LightGoldenrodYellow;
                    tabelagrid.Columns["Form2I"].HeaderCell.Style.BackColor = Color.LightGoldenrodYellow;

                    tabelagrid.Columns["DiaI"].HeaderText = "Dia Fórm.";
                    tabelagrid.Columns["HoraI"].HeaderText = "Hora Form.";
                    tabelagrid.Columns["NomeI"].HeaderText = "Nome Form. Banco";
                    tabelagrid.Columns["Form1I"].HeaderText = "Cód. Prog. Banco";
                    tabelagrid.Columns["Form2I"].HeaderText = "Cód. Cliente Banco";

                }
                else
                {
                    tabelagrid.Columns["DiaI"].Visible = false;
                    tabelagrid.Columns["HoraI"].Visible = false;
                    tabelagrid.Columns["NomeI"].Visible = false;
                    tabelagrid.Columns["Form1I"].Visible = false;
                    tabelagrid.Columns["Form2I"].Visible = false;
                }
            }
            else
            {
                tabelagrid.Columns["DiaI"].Visible = false;
                tabelagrid.Columns["HoraI"].Visible = false;
                tabelagrid.Columns["HoraI"].Visible = false;
                tabelagrid.Columns["Form1I"].Visible = false;
                tabelagrid.Columns["Form2I"].Visible = false;
            }
        }
        private void Pesquisar_data2()
        {


            String Dia1 = comboBox1.SelectedItem.ToString();
            String Dia2 = comboBox2.SelectedItem.ToString();



            DataTable db2 = new DataTable();
            db2.Columns.Add("Total", typeof(double));
            db2.Columns.Add("Produto", typeof(String));

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
                DataTable produtos = new DataTable();
                objAdapter.Fill(produtos);
                int Linhas = produtos.Rows.Count;
                Linhas = Linhas - 1;
                InicalBox.Text = produtos.Rows[0]["Hora"].ToString();
                FinalBox.Text = produtos.Rows[Linhas]["Hora"].ToString();

                String[] ProdCamp = new String[41];
                Object[] sumObject = new Object[41];
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

                if (produtos.Rows.Count > 0)
                {
                    for (int i = 0; i <= 39; i++)
                    {


                        sumObject[i] = produtos.Compute("Sum(" + ProdCamp[i] + ")", "");

                        if (!DBNull.Value.Equals(sumObject[i]))
                        {
                            DataRow Row1 = db2.NewRow();
                            Double Valor = Convert.ToDouble(sumObject[i]);
                            if (Valor != 0)
                            {
                                Double Medida = Convert.ToDouble(TabelasT.NomeProduto.Rows[i]["Medida"].ToString());


                                Row1["Total"] = Valor / Medida;
                                Row1["Produto"] = TabelasT.NomeProduto.Rows[i]["Produto"].ToString();
                                db2.Rows.Add(Row1);
                            }
                        }


                    }

                }
                object sumTotal;
                sumTotal = db2.Compute("Sum(Total)", " ");
                double ToTalTot = Convert.ToDouble(sumTotal);

                TotalBox.Text = ToTalTot.ToString("#,0.000");
                TabelasT.Total1 = ToTalTot.ToString("#,0.000");

                TBatidaBox.Text = produtos.Rows.Count.ToString("#,#");
                TabelasT.Batida1 = produtos.Rows.Count.ToString("#,#");

            }
            catch (MySqlException erro)
            {

                MessageBox.Show("Erro na Conexão: " + erro);
            }

            Conexao.Close();
            dataGridView2.DataSource = db2;
            dataGridView2.Columns["Produto"].DisplayIndex = 0;
            dataGridView2.Columns["Total"].DisplayIndex = 1;
            dataGridView2.Columns["Total"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView2.Columns["Total"].DefaultCellStyle.Format = "#,##0.000";
            dataGridView2.Columns["Produto"].Width = 150;
            dataGridView2.Columns["Total"].Width = 100;

            TabelasT.DataTable2 = db2;
        }
        private void Pesquisar3_Click(object sender, EventArgs e)
        {
            ModoPesquisa();

        }
        private void ModoPesquisa()
        {
            DateTime Data1;
            DateTime Data2;


            Data1 = Convert.ToDateTime(comboBox1.SelectedItem);
            Data2 = Convert.ToDateTime(comboBox2.SelectedItem);

            bool XCadaMes = Properties.Settings.Default.MetodoMesCSV;
            bool XFormula = Properties.Settings.Default.FormulaCSV;

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
                    LimparGrids();
                    if (radioButton4.Checked == true)
                    {
                        Pesquisar_Individual_1();
                    }
                    else
                    {
                        if (XFormula == true)
                        {
                            Pesquisar3.Enabled = false;
                            if (radioButton3.Checked == true)
                            {
                                Pesquisar_Todas();
                            }

                        }
                        else
                        {
                            PesquisarX1();
                            Pesquisar_dataX2();
                        }
                        Pesquisar3.Enabled = true;
                    }
                    
                }
                else
                {
                    MessageBox.Show("Intervalo de datas incorreto!");
                }

            }
        }

        public void PesquisarX1()
        {
            int IDForm;

            String Dia1 = comboBox1.SelectedItem.ToString();
            String Dia2 = comboBox2.SelectedItem.ToString();




            DataTable tabela = new DataTable();
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("Total", typeof(double));


            DataSet1 M = new DataSet1();

            /*
            DataTable db1 = new DataTable();
            db1.Columns.Add("Numero", typeof(ulong));
            db1.Columns.Add("TotalR", typeof(double));
            db1.Columns.Add("Nome", typeof(string));
            */
            DataTable db1 = new DataTable();
            db1.Columns.Add("CProg", typeof(string));
            db1.Columns.Add("CCliente", typeof(string));
            db1.Columns.Add("Batida", typeof(string));
            db1.Columns.Add("Nome", typeof(string));
            db1.Columns.Add("TotalR", typeof(double));
            //db1.Columns.Add("TotalF", typeof(double));

            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);

            Conexao.Open();

            try
            {
                String SqlForm;

                if (radioButton1.Checked == true)
                {
                    SqlForm = "Select Distinct Form1 from relatorio";
                }
                else
                {
                    SqlForm = "Select Distinct Form2 from relatorio";
                }

                MySqlCommand FormNumero = new MySqlCommand(SqlForm, Conexao);
                MySqlDataAdapter NomeAdapter = new MySqlDataAdapter(FormNumero);
                DataTable IDTab = new DataTable();
                NomeAdapter.Fill(IDTab);




                //String TTexto = "Select Max(Prod_1) from cadastro.relatorio";
                //MySqlCommand FormMax = new MySqlCommand(TTexto, Conexao);

                //IDForm = Convert.ToInt16(FormMax.ExecuteScalar());

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
                
                IDForm = IDTab.Rows.Count - 1;

                for (int i = 0; i <= IDForm; ++i)
                {
                    ulong NumForm = Convert.ToUInt64(IDTab.Rows[i][0]);

                    String Sql;
                    if (radioButton1.Checked == true)
                    {
                        Sql = "Select * from relatorio where (Form1 = @valor) and (str_to_date(dia,'%d/%m/%Y') >= str_to_date(@data1,'%d/%m/%Y')) and (str_to_date(dia,'%d/%m/%Y') <= str_to_date(@data2,'%d/%m/%Y'))";
                    }
                    else
                    {
                        Sql = "Select * from relatorio where (Form2 = @valor) and (str_to_date(dia,'%d/%m/%Y') >= str_to_date(@data1,'%d/%m/%Y')) and (str_to_date(dia,'%d/%m/%Y') <= str_to_date(@data2,'%d/%m/%Y'))";
                    }
                    MySqlCommand Comando = new MySqlCommand(Sql, Conexao);
                    Comando.Parameters.AddWithValue("@valor", NumForm);
                    Comando.Parameters.AddWithValue("@data1", Dia1);
                    Comando.Parameters.AddWithValue("@data2", Dia2);
                    
                    MySqlDataAdapter objAdapter = new MySqlDataAdapter(Comando);
                    DataTable dtlista = new DataTable();
                    objAdapter.Fill(dtlista);

                    int NumeroBatida = dtlista.Rows.Count;

                    if (dtlista.Rows.Count > 0)
                    {
                        Double Totals = 0;
                        for (int t = 0; t <= 64; t++)
                        {
                            Object sumObject1;
                            String Texto1 = "Sum(" + ProdCamp[t] + ")";
                            if (!DBNull.Value.Equals(dtlista.Compute(Texto1, "")))
                            {
                                Double Medida = Convert.ToDouble(TabelasT.NomeProduto.Rows[t]["Medida"].ToString());
                                sumObject1 = dtlista.Compute(Texto1, "");
                                Double ValorLinha;
                                ValorLinha = Convert.ToDouble(sumObject1) / Medida;

                                Totals = Totals + ValorLinha;

                            }

                        }
                        Object Name;
                        Name = dtlista.Rows[0]["Nome"].ToString();

                        String CPForm = dtlista.Rows[0]["Form1"].ToString();
                        String CCForm = dtlista.Rows[0]["Form2"].ToString();

                        DataRow Row = db1.NewRow();
                        Row["CProg"] = CPForm;
                        Row["CCliente"] = CCForm;
                        Row["Batida"] = NumeroBatida;
                        Row["Nome"] = Name;
                        Row["TotalR"] = Totals;
                        /*
                        Row["Numero"] = NumForm;
                        Row["TotalR"] = Totals;
                        Row["Nome"] = Name;
                        */
                    db1.Rows.Add(Row);
                    }

                }
            }
            catch (MySqlException erro)
            {

                MessageBox.Show("Erro na Conexão:" + erro);
            }


            Conexao.Close();


            dataGridView1.DataSource = db1;
            dataGridView1.Columns["CProg"].DisplayIndex = 0;
            dataGridView1.Columns["CCliente"].DisplayIndex = 1;
            dataGridView1.Columns["Batida"].DisplayIndex = 2;
            dataGridView1.Columns["Nome"].DisplayIndex = 3;
            dataGridView1.Columns["TotalR"].DisplayIndex = 4;

            dataGridView1.Columns["CProg"].Width = 80;
            dataGridView1.Columns["CCliente"].Width = 80;
            dataGridView1.Columns["Batida"].Width = 80;
            dataGridView1.Columns["Nome"].Width = 150;
            dataGridView1.Columns["TotalR"].Width = 100;


            dataGridView1.Columns["TotalR"].DefaultCellStyle.Format = "#,##0.000";
            dataGridView1.Columns["TotalR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            TabelasT.DataTable1 = db1;



            dataGridView1.Columns["CProg"].HeaderText = "Form. Prog";
            dataGridView1.Columns["CCliente"].HeaderText = "Form. Cliente";
            dataGridView1.Columns["TotalR"].HeaderText = "Total";

        }

        private void Pesquisar_dataX2()
        {


            String Dia1 = comboBox1.SelectedItem.ToString();
            String Dia2 = comboBox2.SelectedItem.ToString();



            DataTable db2 = new DataTable();
            db2.Columns.Add("TotalR", typeof(double));
            db2.Columns.Add("Produto", typeof(String));

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
                DataTable produtos = new DataTable();
                objAdapter.Fill(produtos);
                int Linhas = produtos.Rows.Count;
                Linhas = Linhas - 1;
                InicalBox.Text = produtos.Rows[0]["Hora"].ToString();
                FinalBox.Text = produtos.Rows[Linhas]["Hora"].ToString();

                String[] ProdCamp = new String[66];
                Object[] sumObject = new Object[66];
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
               


                if (produtos.Rows.Count > 0)
                {
                    for (int i = 0; i <= 64; i++)
                    {


                        sumObject[i] = produtos.Compute("Sum(" + ProdCamp[i] + ")", "");

                        if (!DBNull.Value.Equals(sumObject[i]))
                        {
                            DataRow Row1 = db2.NewRow();
                            Double Valor = Convert.ToDouble(sumObject[i]);
                            if (Valor != 0)
                            {
                                Double Medida = Convert.ToDouble(TabelasT.NomeProduto.Rows[i]["Medida"].ToString());


                                Row1["TotalR"] = Valor / Medida;
                                Row1["Produto"] = TabelasT.NomeProduto.Rows[i]["Produto"].ToString();
                                db2.Rows.Add(Row1);
                            }
                        }


                    }

                }
                object sumTotal;
                sumTotal = db2.Compute("Sum(TotalR)", " ");
                double ToTalTot = Convert.ToDouble(sumTotal);

                TotalBox.Text = ToTalTot.ToString("#,0.000");
                TabelasT.Total1 = ToTalTot.ToString("#,0.000");

                TBatidaBox.Text = produtos.Rows.Count.ToString("#,#");
                TabelasT.Batida1 = produtos.Rows.Count.ToString("#,#");




            }
            catch (MySqlException erro)
            {

                MessageBox.Show("Erro na Conexão: " + erro);
            }

            Conexao.Close();
            dataGridView2.DataSource = db2;
            dataGridView2.Columns["Produto"].DisplayIndex = 0;
            dataGridView2.Columns["TotalR"].DisplayIndex = 1;
            dataGridView2.Columns["TotalR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView2.Columns["TotalR"].DefaultCellStyle.Format = "#,##0.000";
            dataGridView2.Columns["Produto"].Width = 150;
            dataGridView2.Columns["TotalR"].Width = 100;



            TabelasT.DataTable2 = db2;

            dataGridView2.Columns["TotalR"].HeaderText = "Total";

        }


        public void PesquisarX1_Ind()
        {
            int IDForm;

            String Dia1 = comboBox1.SelectedItem.ToString();
            String Dia2 = comboBox2.SelectedItem.ToString();

            String NumForm = FormBox.SelectedItem.ToString();


            DataTable tabela = new DataTable();
            tabela.Columns.Add("Nome", typeof(string));
            tabela.Columns.Add("Total", typeof(double));


            DataSet1 M = new DataSet1();

            DataTable db1 = new DataTable();
            /*
            db1.Columns.Add("Numero", typeof(ulong));
            db1.Columns.Add("Total", typeof(double));
            db1.Columns.Add("Nome", typeof(string));
            */
            db1.Columns.Add("CProg", typeof(string));
            db1.Columns.Add("CCliente", typeof(string));
            db1.Columns.Add("Batida", typeof(string));
            db1.Columns.Add("Nome", typeof(string));
            db1.Columns.Add("TotalR", typeof(double));

            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);

            Conexao.Open();

            try
            {
                /*
                String SqlForm;

                if (radioButton1.Checked == true)
                {
                    SqlForm = "Select Distinct Form1 from relatorio";
                }
                else
                {
                    SqlForm = "Select Distinct Form2 from relatorio";
                }

                MySqlCommand FormNumero = new MySqlCommand(SqlForm, Conexao);
                MySqlDataAdapter NomeAdapter = new MySqlDataAdapter(FormNumero);
                DataTable IDTab = new DataTable();
                NomeAdapter.Fill(IDTab);


                */

                //String TTexto = "Select Max(Prod_1) from cadastro.relatorio";
                //MySqlCommand FormMax = new MySqlCommand(TTexto, Conexao);

                //IDForm = Convert.ToInt16(FormMax.ExecuteScalar());

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


                //IDForm = IDTab.Rows.Count - 1;


                //for (int i = 0; i <= IDForm; ++i)
                //{
                    //ulong NumForm = Convert.ToUInt64(IDTab.Rows[i][0]);

                    String Sql;
                    if (radioButton1.Checked == true)
                    {
                        Sql = "Select * from relatorio where (Form1 = @valor) and (str_to_date(dia,'%d/%m/%Y') >= str_to_date(@data1,'%d/%m/%Y')) and (str_to_date(dia,'%d/%m/%Y') <= str_to_date(@data2,'%d/%m/%Y'))";
                    }
                    else
                    {
                        Sql = "Select * from relatorio where (Form2 = @valor) and (str_to_date(dia,'%d/%m/%Y') >= str_to_date(@data1,'%d/%m/%Y')) and (str_to_date(dia,'%d/%m/%Y') <= str_to_date(@data2,'%d/%m/%Y'))";
                    }
                    MySqlCommand Comando = new MySqlCommand(Sql, Conexao);
                    Comando.Parameters.AddWithValue("@valor", NumForm);
                    Comando.Parameters.AddWithValue("@data1", Dia1);
                    Comando.Parameters.AddWithValue("@data2", Dia2);



                    MySqlDataAdapter objAdapter = new MySqlDataAdapter(Comando);
                    DataTable dtlista = new DataTable();
                    objAdapter.Fill(dtlista);

                    int NumeroBatida = dtlista.Rows.Count;

                    if (dtlista.Rows.Count > 0)
                    {
                        Double Totals = 0;
                        for (int t = 0; t <= 64; t++)
                        {
                            Object sumObject1;
                            String Texto1 = "Sum(" + ProdCamp[t] + ")";
                            if (!DBNull.Value.Equals(dtlista.Compute(Texto1, "")))
                            {
                                Double Medida = Convert.ToDouble(TabelasT.NomeProduto.Rows[t]["Medida"].ToString());
                                sumObject1 = dtlista.Compute(Texto1, "");
                                Double ValorLinha;
                                ValorLinha = Convert.ToDouble(sumObject1) / Medida;

                                Totals = Totals + ValorLinha;

                            }

                        }
                    /*
                    Object Name;
                        Name = dtlista.Rows[0]["Nome"].ToString();

                        DataRow Row = db1.NewRow();
                        Row["Numero"] = NumForm;
                        Row["Total"] = Totals;
                        Row["Nome"] = Name;
                        db1.Rows.Add(Row);
                  */

                    Object Name;
                    Name = dtlista.Rows[0]["Nome"].ToString();

                    String CPForm = dtlista.Rows[0]["Form1"].ToString();
                    String CCForm = dtlista.Rows[0]["Form2"].ToString();

                    DataRow Row = db1.NewRow();
                    Row["CProg"] = CPForm;
                    Row["CCliente"] = CCForm;
                    Row["Batida"] = NumeroBatida;
                    Row["Nome"] = Name;
                    Row["TotalR"] = Totals;

                    db1.Rows.Add(Row);


                }
            }
            catch (MySqlException erro)
            {

                MessageBox.Show("Erro na Conexão:" + erro);
            }


            Conexao.Close();



            dataGridView1.DataSource = db1;
            dataGridView1.Columns["CProg"].DisplayIndex = 0;
            dataGridView1.Columns["CCliente"].DisplayIndex = 1;
            dataGridView1.Columns["Batida"].DisplayIndex = 2;
            dataGridView1.Columns["Nome"].DisplayIndex = 3;
            dataGridView1.Columns["TotalR"].DisplayIndex = 4;

            dataGridView1.Columns["CProg"].Width = 80;
            dataGridView1.Columns["CCliente"].Width = 80;
            dataGridView1.Columns["Batida"].Width = 80;
            dataGridView1.Columns["Nome"].Width = 150;
            dataGridView1.Columns["TotalR"].Width = 100;


            dataGridView1.Columns["TotalR"].DefaultCellStyle.Format = "#,##0.000";
            dataGridView1.Columns["TotalR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;


            TabelasT.DataTable1 = db1;



            dataGridView1.Columns["CProg"].HeaderText = "Form. Prog";
            dataGridView1.Columns["CCliente"].HeaderText = "Form. Cliente";
            dataGridView1.Columns["TotalR"].HeaderText = "Total";

        }

        private void Pesquisar_dataX2_Ind()
        {


            String Dia1 = comboBox1.SelectedItem.ToString();
            String Dia2 = comboBox2.SelectedItem.ToString();

            String NumForm = FormBox.SelectedItem.ToString();

            DataTable db2 = new DataTable();
            db2.Columns.Add("TotalR", typeof(double));
            db2.Columns.Add("Produto", typeof(String));

            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);

            String Sql;
            if (radioButton1.Checked == true)
            {
                Sql = "Select * from relatorio where (Form1 = @valor) and (str_to_date(dia,'%d/%m/%Y') >= str_to_date(@data1,'%d/%m/%Y')) and (str_to_date(dia,'%d/%m/%Y') <= str_to_date(@data2,'%d/%m/%Y'))";
            }
            else
            {
                Sql = "Select * from relatorio where (Form2 = @valor) and (str_to_date(dia,'%d/%m/%Y') >= str_to_date(@data1,'%d/%m/%Y')) and (str_to_date(dia,'%d/%m/%Y') <= str_to_date(@data2,'%d/%m/%Y'))";
            }


            //String Sql = "Select * from relatorio where (str_to_date(dia,'%d/%m/%Y') >= str_to_date(@data1,'%d/%m/%Y')) and (str_to_date(dia,'%d/%m/%Y') <= str_to_date(@data2,'%d/%m/%Y'))";
            MySqlCommand Comando = new MySqlCommand(Sql, Conexao);
            Comando.Parameters.AddWithValue("@data1", Dia1);
            Comando.Parameters.AddWithValue("@data2", Dia2);
            Comando.Parameters.AddWithValue("@valor", NumForm);
            Conexao.Open();

            try
            {

                MySqlDataAdapter objAdapter = new MySqlDataAdapter(Comando);
                DataTable produtos = new DataTable();
                objAdapter.Fill(produtos);
                int Linhas = produtos.Rows.Count;
                Linhas = Linhas - 1;
                InicalBox.Text = produtos.Rows[0]["Hora"].ToString();
                FinalBox.Text = produtos.Rows[Linhas]["Hora"].ToString();

                String[] ProdCamp = new String[66];
                Object[] sumObject = new Object[66];
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



                if (produtos.Rows.Count > 0)
                {
                    for (int i = 0; i <= 64; i++)
                    {


                        sumObject[i] = produtos.Compute("Sum(" + ProdCamp[i] + ")", "");

                        if (!DBNull.Value.Equals(sumObject[i]))
                        {
                            DataRow Row1 = db2.NewRow();
                            Double Valor = Convert.ToDouble(sumObject[i]);
                            if (Valor != 0)
                            {
                                Double Medida = Convert.ToDouble(TabelasT.NomeProduto.Rows[i]["Medida"].ToString());


                                Row1["TotalR"] = Valor / Medida;
                                Row1["Produto"] = TabelasT.NomeProduto.Rows[i]["Produto"].ToString();
                                db2.Rows.Add(Row1);
                            }
                        }


                    }

                }
                object sumTotal;
                sumTotal = db2.Compute("Sum(TotalR)", " ");
                double ToTalTot = Convert.ToDouble(sumTotal);

                TotalBox.Text = ToTalTot.ToString("#,0.000");
                TabelasT.Total1 = ToTalTot.ToString("#,0.000");

                TBatidaBox.Text = produtos.Rows.Count.ToString("#,#");
                TabelasT.Batida1 = produtos.Rows.Count.ToString("#,#");




            }
            catch (MySqlException erro)
            {

                MessageBox.Show("Erro na Conexão: " + erro);
            }

            Conexao.Close();
            dataGridView2.DataSource = db2;
            dataGridView2.Columns["Produto"].DisplayIndex = 0;
            dataGridView2.Columns["TotalR"].DisplayIndex = 1;
            dataGridView2.Columns["TotalR"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dataGridView2.Columns["TotalR"].DefaultCellStyle.Format = "#,##0.000";
            dataGridView2.Columns["Produto"].Width = 150;
            dataGridView2.Columns["TotalR"].Width = 100;

            TabelasT.DataTable2 = db2;

            dataGridView2.Columns["TotalR"].HeaderText = "Total";
        }
        private void label5_Click(object sender, EventArgs e)
        {

        }
        private void TesteSql()
        {

            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);

            Conexao.Open();

            try
            {
                String Sql1X = "Select * from cadastro.formulaideal where (Form1=@form) and (DATE_FORMAT(CONCAT(str_to_date(dia, '%d/%m/%Y'), \" \", Hora), '%d/%m/%Y %H:%i:%s') >= DATE_FORMAT(str_to_date(@data @hora1, '%d/%m/%Y %H:%i:%s'), '%d/%m/%Y %H:%i:%s'))";

                MySqlCommand Comando1x = new MySqlCommand(Sql1X, Conexao);
                Comando1x.Parameters.AddWithValue("@form", "1");
                Comando1x.Parameters.AddWithValue("@data", "02/04/2021");
                Comando1x.Parameters.AddWithValue("@hora1", "09:00:00");

                MySqlDataAdapter objAdapter = new MySqlDataAdapter(Comando1x);
                DataTable tabela1 = new DataTable();
                objAdapter.Fill(tabela1);

                //GridTeste1.DataSource = tabela1;


            }
            catch (MySqlException erro)
            {

                MessageBox.Show("Erro na Conexão: " + erro);
            }

            Conexao.Close();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DataSet1 Banco = new DataSet1();
            dataGridView1.DataSource = Banco.Teste;
            dataGridView1.Columns["Nome"].DisplayIndex = 0;
            dataGridView1.Columns["Total"].DisplayIndex = 1;

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void Atualizar3_Click(object sender, EventArgs e)
        {


        }
        private void Some_Componentes_Atua()
        {


        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            Atualiza novo = new Atualiza();
            novo.Update();
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
        private void button1_Click_2(object sender, EventArgs e)
        {
            Atualiza novo = new Atualiza();
            novo.Update();
        }
        private void button1_Click_3(object sender, EventArgs e)
        {
            Atualiza novo = new Atualiza();
            novo.Update();
        }
        private void label8_Click(object sender, EventArgs e)
        {

        }
        private void button1_Click_4(object sender, EventArgs e)
        {
            Atualiza novo = new Atualiza();
            novo.Update();
        }
        private void Form3_Activated(object sender, EventArgs e)
        {

        }
        private void button1_Click_5(object sender, EventArgs e)
        {

        }
        private void button1_Click_6(object sender, EventArgs e)
        {
            CarregaCombobox_At();
        }
        private void Printt1_Click(object sender, EventArgs e)
        {

            if (TotalBox.Text == "")
            {
                MessageBox.Show("Sem dados para Impressão!");
            }
            else
            {
                TabelasT.ClienteR = Properties.Settings.Default.Cliente;
                TabelasT.Total1 = TotalBox.Text;
                if (HabFormIdeal.Checked == true)
                {
                    TabelasT.Total2 = Totalbox2.Text;
                }
                else
                {
                    TabelasT.Total2 = "";
                }

                TabelasT.Batida1 = TBatidaBox.Text;
                TabelasT.HoraInicio = InicalBox.Text;
                TabelasT.HoraFinal = FinalBox.Text;
                TabelasT.DataInicial = comboBox1.SelectedItem.ToString();
                TabelasT.DataFinal = comboBox2.SelectedItem.ToString();
                if (checkBox1.Checked == true)
                {
                    TabelasT.Obs = "OBS: " + ObsBox.Text.ToString();
                }
                else
                {
                    TabelasT.Obs = null;
                }


                Relat1Form Imprimir = new Relat1Form();
                Imprimir.ShowDialog();
            }
        }

        private void ComboAtualiza_Click(object sender, EventArgs e)
        {
            CarregaCombobox_At();
        }

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

        private void button3_Click(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void radiolocal_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radioftp_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radiodata_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void radiopadrao_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void criar_textoInfo()
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {

        }

        private void button3_Click_2(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            FtpInformacao atual = new FtpInformacao();
            atual.Show();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton3.Checked == true) //Modo Todas as formulas
            {
                FormBox.Visible = false;
                labelform.Visible = false;
            }
            LimparGrids();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void FormBox_SelectedIndexChanged(object sender, EventArgs e)
        {

            //Pesquisar_Individual_2();
            bool XFormula = Properties.Settings.Default.FormulaCSV;

            if (XFormula == true)
            {
                Pesquisar_Individual_3();
            }
            else
            {
                PesquisarX1_Ind();
                Pesquisar_dataX2_Ind();
            }
        }
        private void LimparGrids()
        {
            dataGridView1.DataSource = null;
            dataGridView2.DataSource = null;
            tabelagrid.DataSource = null;
            TotalBox.Clear();
            Totalbox2.Clear();
            TBatidaBox.Clear();
            FinalBox.Clear();
            InicalBox.Clear();

        }

        private void button1_Click_7(object sender, EventArgs e)
        {
            //Teste_Pesquisar();
        }

        private void Option1_CheckedChanged(object sender, EventArgs e)
        {
            if (tabelagrid.DataSource != null)
            {
                DetalhesFormula();
            }
        }

        private void HabFormIdeal_CheckedChanged(object sender, EventArgs e)
        {
            if (HabFormIdeal.Checked == true) { TabelasT.VisivelRelat = "1"; } else { TabelasT.VisivelRelat = "0"; }
            if (radioButton3.Checked == true) { if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null) {} else { ModoPesquisa(); } }
            else { if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null || FormBox.SelectedItem == null) { } else { Pesquisar_Individual_3(); } }
            SomeFormBox();
        }

        private void button1_Click_8(object sender, EventArgs e)
        {
           
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }

    public static class TabelasT
    {
        public static DataTable DataTable1 { get; set; }
        public static DataTable DataTable2 { get; set; }
        public static DataTable DataTable3 { get; set; }
        public static DataTable NomeProduto {get; set;}
        public static string Total1;
        public static string Total2;
        public static string Batida1;
        public static string HoraInicio;
        public static string HoraFinal;
        public static string DataInicial;
        public static string DataFinal;
        public static string Obs;
        public static string ClienteR;

        public static string DTotal1;
        public static string DBatida1;
        public static string DHoraInicio;
        public static string DHoraFinal;
        public static string DDataInicial;
        public static string DDataFinal;
        public static string DObs;

        public static bool[] Visivel1 = new bool[66];
        public static bool[] Visivel2 = new bool[66];
        public static string VisivelRelat;


    }
}
