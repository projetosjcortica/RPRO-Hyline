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
using System.Timers;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace JCortica_RPRO
{

    public partial class Form1 : Form
    {
        public object teste { get; set; }



        public Form1()
        {
            InitializeComponent();
            Splash Inicio = new Splash();
            Inicio.Show();
            Thread.Sleep(3000);
            Inicio.Close();

            this.WindowState = FormWindowState.Maximized;


        }

        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DataHora Data = new DataHora();
            Data.Show(this);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            Form3 PesqDataForm = new Form3();
            PesqDataForm.Show(this);
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            MatPrima Materia = new MatPrima();
            Materia.Show(this);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Atualiza.Carregar_ComboData();
        }

        private void iPDaIHMToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config Abre = new Config();
            Abre.Show(this);
        }

        private void mySqlAvançadoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void atualizarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FtpInformacao atualiza1 = new FtpInformacao();
            atualiza1.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Atualiza Novo = new Atualiza();
            Novo.Update();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
                       
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_3(object sender, EventArgs e)
        {
            
        }

        private void arquivoToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void eitquetasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Etiquetas etiquetas = new Etiquetas();
            etiquetas.Show();
        }
    }
    public partial class Atualiza
    {

        public DateTime Dt1 { get; set; }
        public DateTime Dt2 { get; set; }
        public bool RefreshCombos = false;

        // ----------------------- N O V O   A T U A L I Z A Ç A O -------------------------------------

        public void Tempo() //Chamar Atualização de Relatorio.
        {


            String Parte1 = Properties.Settings.Default.PathCSV;
            String Parte2 = Properties.Settings.Default.NomeCSV;
            String Arq = Parte1 + "\\" + Parte2;
            FileInfo fi = new FileInfo(Arq);
            Dt1 = fi.LastAccessTime;

            ProcessStartInfo Baixar = new ProcessStartInfo(@"" + Parte1 + "\\ENGINE.bat");
            Baixar.WindowStyle = ProcessWindowStyle.Minimized;

            Process.Start(Baixar);

            // System.Diagnostics.Process.Start(@"C:\JCortica\BancoCSV\Engine.bat"); - Antigo medoto de chamada

            int TempoIn = Convert.ToInt32(Properties.Settings.Default.TempLimit);


            System.Timers.Timer cro = new System.Timers.Timer();
            cro.Interval = TempoIn; //1000 milésimos = 1 segundo
            cro.Enabled = true;
            cro.AutoReset = false;
            cro.Elapsed += new ElapsedEventHandler(Time_Elapsed);



        }

        public void Time_Elapsed(object sender, ElapsedEventArgs e)
        {

            String Parte1 = Properties.Settings.Default.PathCSV;
            String Parte2 = Properties.Settings.Default.NomeCSV;

            String Arq = Parte1 + "\\" + Parte2;

            FileInfo fi2 = new FileInfo(Arq);
            Dt2 = fi2.LastAccessTime;

            if (Dt1 == Dt2)
            {
                MessageBox.Show("Erro na comunicação com a IHM");

            }
            else
            {
                Update();
            }
        }


        public void Update()
        {

            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);

            Conexao.Open();
            try
            {


                //Limpa a tabela Relatorio de valores nulos
                String Sql8 = "Delete from cadastro.relatorio where hora is null";
                MySqlCommand Comando8 = new MySqlCommand(Sql8, Conexao);
                Comando8.ExecuteNonQuery();


                // Pega a ultima data do banco Relatorio
                String SqlUlt = "Select max(str_to_date(Dia,'%d/%m/%Y')) from cadastro.relatorio";
                MySqlCommand ComandoUlt = new MySqlCommand(SqlUlt, Conexao);
                Object UltData;
                UltData = ComandoUlt.ExecuteScalar();
                String Referencia = Convert.ToString(UltData);

                if (Referencia == "")
                {
                    //Cria uma tabela temporaria chamada "Temp" que vai receber o CSV
                    String SSql1 = "Create Temporary Table Temp (Dia varchar(10),Hora time,Nome varchar(30),Form1 int,Form2 int,Prod_1 int,Prod_2 int,Prod_3 int,Prod_4 int,Prod_5 int,Prod_6 int,Prod_7 int,Prod_8 int,Prod_9 int,Prod_10 int,Prod_11 int,Prod_12 int,Prod_13 int,Prod_14 int,Prod_15 int,Prod_16 int,Prod_17 int,Prod_18 int,Prod_19 int,Prod_20 int,Prod_21 int,Prod_22 int,Prod_23 int,Prod_24 int,Prod_25 int,Prod_26 int,Prod_27 int,Prod_28 int,Prod_29 int,Prod_30 int,Prod_31 int,Prod_32 int)";
                    MySqlCommand SComando1 = new MySqlCommand(SSql1, Conexao);
                    SComando1.ExecuteNonQuery();

                    //Carrega o CSV
                    String SParte1 = Properties.Settings.Default.PathCSV;
                    String SParte2 = Properties.Settings.Default.NomeCSV;
                    SParte1 = SParte1.Replace("\\", "/");
                    SParte2 = SParte2.Replace("\\", "/");
                    String SParte3 = SParte1 + "/" + SParte2;

                    String Spath = "'" + SParte3 + "'";
                    String SSql2 = "LOAD DATA LOCAL INFILE " + Spath + " INTO TABLE cadastro.Temp FIELDS TERMINATED BY ',' LINES TERMINATED BY '\\r\\n' ";
                    MySqlCommand SComando2 = new MySqlCommand(SSql2, Conexao);
                    SComando2.ExecuteNonQuery();

                    //Conta quantos valores novos existem
                    String SSql6 = "Select Count(*) from cadastro.Temp";
                    MySqlCommand SComando6 = new MySqlCommand(SSql6, Conexao);
                    Object Slinhas;
                    Slinhas = SComando6.ExecuteScalar();
                    int SLinhasInt;
                    SLinhasInt = Convert.ToInt16(Slinhas);



                    if (SLinhasInt != 0)
                    {


                        String SMensagem1 = "Foram adicionados " + Slinhas.ToString() + " novos registros.";
                        String SSql4 = "Insert into relatorio select * from cadastro.Temp";
                        MySqlCommand SComando4 = new MySqlCommand(SSql4, Conexao);
                        SComando4.ExecuteNonQuery();

                        MessageBox.Show(SMensagem1);

                        String SSqlDrop2_1 = "Drop Table cadastro.Temp";
                        MySqlCommand SComandoD2_1 = new MySqlCommand(SSqlDrop2_1, Conexao);
                        SComandoD2_1.ExecuteNonQuery();
                        Conexao.Close();

                    }
                    else
                    {
                        MessageBox.Show("Nenhum registro novo!");
                        String SSqlDrop2 = "Drop Table cadastro.Temp";
                        MySqlCommand SComandoD2 = new MySqlCommand(SSqlDrop2, Conexao);
                        SComandoD2.ExecuteNonQuery();
                        Conexao.Close();
                    }

                }
                else
                {


                    DateTime DataConvert = Convert.ToDateTime(UltData);

                    // Cria uma tabela de um Select na tabela Relatorio dos valores iguais ou maiores da ultima data
                    String SqlNovaT = "Create Temporary Table Cadastro.Ultima Select * from cadastro.relatorio where str_to_date(Dia, '%d/%m/%Y') >= str_to_date(@DataFinal,'%d/%m/%Y')";
                    MySqlCommand ComandoNovaT = new MySqlCommand(SqlNovaT, Conexao);
                    ComandoNovaT.Parameters.AddWithValue("@DataFinal", DataConvert.ToString("d/M/y"));
                    ComandoNovaT.ExecuteNonQuery();

                    //Cria uma tabela temporaria chamada "Temp" que vai receber o CSV
                    String Sql1 = "Create Temporary Table Temp (Dia varchar(10),Hora time,Nome varchar(30),Form1 int,Form2 int,Prod_1 int,Prod_2 int,Prod_3 int,Prod_4 int,Prod_5 int,Prod_6 int,Prod_7 int,Prod_8 int,Prod_9 int,Prod_10 int,Prod_11 int,Prod_12 int,Prod_13 int,Prod_14 int,Prod_15 int,Prod_16 int,Prod_17 int,Prod_18 int,Prod_19 int,Prod_20 int,Prod_21 int,Prod_22 int,Prod_23 int,Prod_24 int,Prod_25 int,Prod_26 int,Prod_27 int,Prod_28 int,Prod_29 int,Prod_30 int,Prod_31 int,Prod_32 int)";
                    MySqlCommand Comando1 = new MySqlCommand(Sql1, Conexao);
                    Comando1.ExecuteNonQuery();

                    //Carrega o CSV
                    String Parte1 = Properties.Settings.Default.PathCSV;
                    String Parte2 = Properties.Settings.Default.NomeCSV;
                    Parte1 = Parte1.Replace("\\", "/");
                    Parte2 = Parte2.Replace("\\", "/");
                    String Parte3 = Parte1 + "/" + Parte2;

                    String path = "'" + Parte3 + "'";
                    String Sql2 = "LOAD DATA LOCAL INFILE " + path + " INTO TABLE cadastro.Temp FIELDS TERMINATED BY ',' LINES TERMINATED BY '\\r\\n' ";
                    MySqlCommand Comando2 = new MySqlCommand(Sql2, Conexao);
                    Comando2.ExecuteNonQuery();

                    //Deleta da Tabela Temporaria Temp valores menores do que da ultima data do relatorio principal
                    String SqlDel = "Delete from cadastro.Temp where str_to_date(Dia,'%d/%m/%Y') < str_to_date(@datadel,'%d/%m/%Y')";
                    MySqlCommand ComandoDel = new MySqlCommand(SqlDel, Conexao);
                    ComandoDel.Parameters.AddWithValue("@datadel", DataConvert.ToString("d/M/y"));
                    ComandoDel.ExecuteNonQuery();

                    //Cria uma tabela ("Tempos") a partir de um SELECT que cruza os dados (LEFT JOIN) entre tabela temporaria "Temp" e "Ultima"
                    String Sql3 = "Create Temporary Table Tempos SELECT b.Dia,b.Hora,b.Nome,b.Form1,b.Form2,b.Prod_1,b.Prod_2,b.Prod_3,b.Prod_4,b.Prod_5,b.Prod_6,b.Prod_7,b.Prod_8,b.Prod_9,b.Prod_10,b.Prod_11,b.Prod_12,b.Prod_13,b.Prod_14,b.Prod_15,b.Prod_16,b.Prod_17,b.Prod_18,b.Prod_19,b.Prod_20,b.Prod_21,b.Prod_22,b.Prod_23,b.Prod_24,b.Prod_25,b.Prod_26,b.Prod_27,b.Prod_28,b.Prod_29,b.Prod_30,b.Prod_31,b.Prod_32 FROM cadastro.Temp as b LEFT JOIN cadastro.ultima as a on b.Dia = a.Dia and b.Hora = a.Hora WHERE a.Dia is null";
                    MySqlCommand Comando3 = new MySqlCommand(Sql3, Conexao);
                    Comando3.ExecuteNonQuery();

                    //Deleta valores nulos da tabela Tempos 
                    String Sql7 = "Delete from Tempos where Hora is null";
                    MySqlCommand Comando7 = new MySqlCommand(Sql7, Conexao);
                    Comando7.ExecuteNonQuery();

                    //Conta quantos valores novos existem
                    String Sql6 = "Select Count(*) from cadastro.tempos";
                    MySqlCommand Comando6 = new MySqlCommand(Sql6, Conexao);
                    Object linhas;
                    linhas = Comando6.ExecuteScalar();
                    int LinhasInt;
                    LinhasInt = Convert.ToInt16(linhas);

                    if (LinhasInt != 0)
                    {


                        String Mensagem1 = "Foram adicionados " + linhas.ToString() + " novos registros.";
                        String Sql4 = "Insert into relatorio select * from cadastro.Tempos";
                        MySqlCommand Comando4 = new MySqlCommand(Sql4, Conexao);
                        Comando4.ExecuteNonQuery();

                        MessageBox.Show(Mensagem1);

                        String SqlDrop1V = "Drop Table cadastro.Tempos";
                        MySqlCommand ComandoD1V = new MySqlCommand(SqlDrop1V, Conexao);
                        ComandoD1V.ExecuteNonQuery();

                        String SqlDrop2V = "Drop Table cadastro.Temp";
                        MySqlCommand ComandoD2V = new MySqlCommand(SqlDrop2V, Conexao);
                        ComandoD2V.ExecuteNonQuery();

                        String SqlDrop3V = "Drop Table cadastro.Ultima";
                        MySqlCommand ComandoD3V = new MySqlCommand(SqlDrop3V, Conexao);
                        ComandoD3V.ExecuteNonQuery();

                        Conexao.Close();

                    }
                    else
                    {
                        MessageBox.Show("Nenhum registro novo!");
                        String SqlDrop1 = "Drop Table cadastro.Tempos";
                        MySqlCommand ComandoD1 = new MySqlCommand(SqlDrop1, Conexao);
                        ComandoD1.ExecuteNonQuery();

                        String SqlDrop2 = "Drop Table cadastro.Temp";
                        MySqlCommand ComandoD2 = new MySqlCommand(SqlDrop2, Conexao);
                        ComandoD2.ExecuteNonQuery();

                        String SqlDrop3 = "Drop Table cadastro.Ultima";
                        MySqlCommand ComandoD3 = new MySqlCommand(SqlDrop3, Conexao);
                        ComandoD3.ExecuteNonQuery();

                        Conexao.Close();

                    }

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        public void Update_Formula()
        {

            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);

            Conexao.Open();
            try
            {


                //Limpa a tabela Relatorio de valores nulos
                String Sql8 = "Delete from cadastro.formulaideal where hora is null";
                MySqlCommand Comando8 = new MySqlCommand(Sql8, Conexao);
                Comando8.ExecuteNonQuery();


                // Pega a ultima data do banco Relatorio
                String SqlUlt = "Select max(str_to_date(Dia,'%d/%m/%Y')) from cadastro.relatorio";
                MySqlCommand ComandoUlt = new MySqlCommand(SqlUlt, Conexao);
                Object UltData;

                UltData = ComandoUlt.ExecuteScalar();
                String Referencia = Convert.ToString(UltData);
                if (Referencia == "")
                {
                    //Cria uma tabela temporaria chamada "Temp" que vai receber o CSV
                    String SSql1 = "Create Temporary Table Temp (Dia varchar(10),Hora time,Nome varchar(30),Form1 int,Form2 int,Prod_1 int,Prod_2 int,Prod_3 int,Prod_4 int,Prod_5 int,Prod_6 int,Prod_7 int,Prod_8 int,Prod_9 int,Prod_10 int,Prod_11 int,Prod_12 int,Prod_13 int,Prod_14 int,Prod_15 int,Prod_16 int,Prod_17 int,Prod_18 int,Prod_19 int,Prod_20 int,Prod_21 int,Prod_22 int,Prod_23 int,Prod_24 int,Prod_25 int,Prod_26 int,Prod_27 int,Prod_28 int,Prod_29 int,Prod_30 int,Prod_31 int,Prod_32 int)";
                    MySqlCommand SComando1 = new MySqlCommand(SSql1, Conexao);
                    SComando1.ExecuteNonQuery();

                    //Carrega o CSV
                    String SParte1 = Properties.Settings.Default.PathCSV;
                    String SParte2 = Properties.Settings.Default.NomeCSV;
                    SParte1 = SParte1.Replace("\\", "/");
                    SParte2 = SParte2.Replace("\\", "/");
                    String SParte3 = SParte1 + "/" + SParte2;

                    String Spath = "'" + SParte3 + "'";
                    String SSql2 = "LOAD DATA LOCAL INFILE " + Spath + " INTO TABLE cadastro.Temp FIELDS TERMINATED BY ',' LINES TERMINATED BY '\\r\\n' ";
                    MySqlCommand SComando2 = new MySqlCommand(SSql2, Conexao);
                    SComando2.ExecuteNonQuery();

                    //Conta quantos valores novos existem
                    String SSql6 = "Select Count(*) from cadastro.Temp";
                    MySqlCommand SComando6 = new MySqlCommand(SSql6, Conexao);
                    Object Slinhas;
                    Slinhas = SComando6.ExecuteScalar();
                    int SLinhasInt;
                    SLinhasInt = Convert.ToInt16(Slinhas);



                    if (SLinhasInt != 0)
                    {


                        String SMensagem1 = "Foram adicionados " + Slinhas.ToString() + " novos registros.";
                        String SSql4 = "Insert into relatorio select * from cadastro.Temp";
                        MySqlCommand SComando4 = new MySqlCommand(SSql4, Conexao);
                        SComando4.ExecuteNonQuery();

                        MessageBox.Show(SMensagem1);

                        String SSqlDrop2_1 = "Drop Table cadastro.Temp";
                        MySqlCommand SComandoD2_1 = new MySqlCommand(SSqlDrop2_1, Conexao);
                        SComandoD2_1.ExecuteNonQuery();
                        Conexao.Close();

                    }
                    else
                    {
                        MessageBox.Show("Nenhum registro novo!");
                        String SSqlDrop2 = "Drop Table cadastro.Temp";
                        MySqlCommand SComandoD2 = new MySqlCommand(SSqlDrop2, Conexao);
                        SComandoD2.ExecuteNonQuery();
                        Conexao.Close();
                    }

                }
                else
                {


                    DateTime DataConvert = Convert.ToDateTime(UltData);

                    // Cria uma tabela de um Select na tabela Relatorio dos valores iguais ou maiores da ultima data
                    String SqlNovaT = "Create Temporary Table Cadastro.Ultima Select * from cadastro.relatorio where str_to_date(Dia, '%d/%m/%Y') >= str_to_date(@DataFinal,'%d/%m/%Y')";
                    MySqlCommand ComandoNovaT = new MySqlCommand(SqlNovaT, Conexao);
                    ComandoNovaT.Parameters.AddWithValue("@DataFinal", DataConvert.ToString("d/M/y"));
                    ComandoNovaT.ExecuteNonQuery();

                    //Cria uma tabela temporaria chamada "Temp" que vai receber o CSV
                    String Sql1 = "Create Temporary Table Temp (Dia varchar(10),Hora time,Nome varchar(30),Form1 int,Form2 int,Prod_1 int,Prod_2 int,Prod_3 int,Prod_4 int,Prod_5 int,Prod_6 int,Prod_7 int,Prod_8 int,Prod_9 int,Prod_10 int,Prod_11 int,Prod_12 int,Prod_13 int,Prod_14 int,Prod_15 int,Prod_16 int,Prod_17 int,Prod_18 int,Prod_19 int,Prod_20 int,Prod_21 int,Prod_22 int,Prod_23 int,Prod_24 int,Prod_25 int,Prod_26 int,Prod_27 int,Prod_28 int,Prod_29 int,Prod_30 int,Prod_31 int,Prod_32 int)";
                    MySqlCommand Comando1 = new MySqlCommand(Sql1, Conexao);
                    Comando1.ExecuteNonQuery();

                    //Carrega o CSV
                    String Parte1 = Properties.Settings.Default.PathCSV;
                    String Parte2 = Properties.Settings.Default.NomeCSV;
                    Parte1 = Parte1.Replace("\\", "/");
                    Parte2 = Parte2.Replace("\\", "/");
                    String Parte3 = Parte1 + "/" + Parte2;

                    String path = "'" + Parte3 + "'";
                    String Sql2 = "LOAD DATA LOCAL INFILE " + path + " INTO TABLE cadastro.Temp FIELDS TERMINATED BY ',' LINES TERMINATED BY '\\r\\n' ";
                    MySqlCommand Comando2 = new MySqlCommand(Sql2, Conexao);
                    Comando2.ExecuteNonQuery();

                    //Deleta da Tabela Temporaria Temp valores menores do que da ultima data do relatorio principal
                    String SqlDel = "Delete from cadastro.Temp where str_to_date(Dia,'%d/%m/%Y') < str_to_date(@datadel,'%d/%m/%Y')";
                    MySqlCommand ComandoDel = new MySqlCommand(SqlDel, Conexao);
                    ComandoDel.Parameters.AddWithValue("@datadel", DataConvert.ToString("d/M/y"));
                    ComandoDel.ExecuteNonQuery();

                    //Cria uma tabela ("Tempos") a partir de um SELECT que cruza os dados (LEFT JOIN) entre tabela temporaria "Temp" e "Ultima"
                    String Sql3 = "Create Temporary Table Tempos SELECT b.Dia,b.Hora,b.Nome,b.Form1,b.Form2,b.Prod_1,b.Prod_2,b.Prod_3,b.Prod_4,b.Prod_5,b.Prod_6,b.Prod_7,b.Prod_8,b.Prod_9,b.Prod_10,b.Prod_11,b.Prod_12,b.Prod_13,b.Prod_14,b.Prod_15,b.Prod_16,b.Prod_17,b.Prod_18,b.Prod_19,b.Prod_20,b.Prod_21,b.Prod_22,b.Prod_23,b.Prod_24,b.Prod_25,b.Prod_26,b.Prod_27,b.Prod_28,b.Prod_29,b.Prod_30,b.Prod_31,b.Prod_32 FROM cadastro.Temp as b LEFT JOIN cadastro.ultima as a on b.Dia = a.Dia and b.Hora = a.Hora WHERE a.Dia is null";
                    MySqlCommand Comando3 = new MySqlCommand(Sql3, Conexao);
                    Comando3.ExecuteNonQuery();

                    //Deleta valores nulos da tabela Tempos 
                    String Sql7 = "Delete from Tempos where Hora is null";
                    MySqlCommand Comando7 = new MySqlCommand(Sql7, Conexao);
                    Comando7.ExecuteNonQuery();

                    //Conta quantos valores novos existem
                    String Sql6 = "Select Count(*) from cadastro.tempos";
                    MySqlCommand Comando6 = new MySqlCommand(Sql6, Conexao);
                    Object linhas;
                    linhas = Comando6.ExecuteScalar();
                    int LinhasInt;
                    LinhasInt = Convert.ToInt16(linhas);

                    if (LinhasInt != 0)
                    {


                        String Mensagem1 = "Foram adicionados " + linhas.ToString() + " novos registros.";
                        String Sql4 = "Insert into relatorio select * from cadastro.Tempos";
                        MySqlCommand Comando4 = new MySqlCommand(Sql4, Conexao);
                        Comando4.ExecuteNonQuery();

                        MessageBox.Show(Mensagem1);

                        String SqlDrop1V = "Drop Table cadastro.Tempos";
                        MySqlCommand ComandoD1V = new MySqlCommand(SqlDrop1V, Conexao);
                        ComandoD1V.ExecuteNonQuery();

                        String SqlDrop2V = "Drop Table cadastro.Temp";
                        MySqlCommand ComandoD2V = new MySqlCommand(SqlDrop2V, Conexao);
                        ComandoD2V.ExecuteNonQuery();

                        String SqlDrop3V = "Drop Table cadastro.Ultima";
                        MySqlCommand ComandoD3V = new MySqlCommand(SqlDrop3V, Conexao);
                        ComandoD3V.ExecuteNonQuery();

                        Conexao.Close();

                    }
                    else
                    {
                        MessageBox.Show("Nenhum registro novo!");
                        String SqlDrop1 = "Drop Table cadastro.Tempos";
                        MySqlCommand ComandoD1 = new MySqlCommand(SqlDrop1, Conexao);
                        ComandoD1.ExecuteNonQuery();

                        String SqlDrop2 = "Drop Table cadastro.Temp";
                        MySqlCommand ComandoD2 = new MySqlCommand(SqlDrop2, Conexao);
                        ComandoD2.ExecuteNonQuery();

                        String SqlDrop3 = "Drop Table cadastro.Ultima";
                        MySqlCommand ComandoD3 = new MySqlCommand(SqlDrop3, Conexao);
                        ComandoD3.ExecuteNonQuery();

                        Conexao.Close();

                    }

                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }


        // ----------------------- F I M   A T U A L I Z A Ç A O  --------------------------------



        public void Update_SemReg()
        {
            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);


            try
            {
                Conexao.Open();

                //Carrega o CSV
                String SParte1 = Properties.Settings.Default.PathCSV;
                String SParte2 = Properties.Settings.Default.NomeCSV;
                SParte1 = SParte1.Replace("\\", "/");
                SParte2 = SParte2.Replace("\\", "/");
                String SParte3 = SParte1 + "/" + SParte2;

                String Spath = "c:/RPRO/BancoCSV/Relatorio_1.csv";
                String SSql2 = "LOAD DATA LOCAL INFILE " + Spath + " INTO TABLE cadastro.Temp FIELDS TERMINATED BY ',' LINES TERMINATED BY '\\r\\n' ";
                MySqlCommand SComando2 = new MySqlCommand(SSql2, Conexao);
                SComando2.ExecuteNonQuery();

                //Conta quantos valores novos existem
                String SSql6 = "Select Count(*) from cadastro.Temp";
                MySqlCommand SComando6 = new MySqlCommand(SSql6, Conexao);
                Object Slinhas;
                Slinhas = SComando6.ExecuteScalar();
                int SLinhasInt;
                SLinhasInt = Convert.ToInt16(Slinhas);



                if (SLinhasInt != 0)
                {


                    String SMensagem1 = "Foram adicionados " + Slinhas.ToString() + " novos registros.";
                    String SSql4 = "Insert into relatorio select * from cadastro.Temp";
                    MySqlCommand SComando4 = new MySqlCommand(SSql4, Conexao);
                    SComando4.ExecuteNonQuery();

                    MessageBox.Show(SMensagem1);

                }
                else
                {
                    MessageBox.Show("Nenhum registro novo!");
                }


                String SSqlDrop2 = "Drop Table cadastro.Temp";
                MySqlCommand SComandoD2 = new MySqlCommand(SSqlDrop2, Conexao);
                SComandoD2.ExecuteNonQuery();

                Conexao.Close();
            }

            catch
            {
                MessageBox.Show("Erro na segunda conexão!");
            }
            Conexao.Close();
        }

        public void Update2()
        {

            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);

            Conexao.Open();
            String Sql8 = "Delete from cadastro.relatorio where hora is null";
            MySqlCommand Comando8 = new MySqlCommand(Sql8, Conexao);
            Comando8.ExecuteNonQuery();

            String Sql1 = "Create temporary Table Temp (Dia varchar(10),Hora time,Nome varchar(30),Form1 int,Form2 int,Prod_1 int,Prod_2 int,Prod_3 int,Prod_4 int,Prod_5 int,Prod_6 int,Prod_7 int,Prod_8 int,Prod_9 int,Prod_10 int,Prod_11 int,Prod_12 int,Prod_13 int,Prod_14 int,Prod_15 int,Prod_16 int,Prod_17 int,Prod_18 int,Prod_19 int,Prod_20 int,Prod_21 int,Prod_22 int,Prod_23 int,Prod_24 int,Prod_25 int,Prod_26 int,Prod_27 int,Prod_28 int,Prod_29 int,Prod_30 int,Prod_31 int,Prod_32 int)";
            MySqlCommand Comando1 = new MySqlCommand(Sql1, Conexao);
            Comando1.ExecuteNonQuery();

            String path = "'c:/JCortica/BancoCSV/Relatorio_1.csv'";

            String Sql2 = "LOAD DATA LOCAL INFILE " + path + " INTO TABLE cadastro.Temp FIELDS TERMINATED BY ',' LINES TERMINATED BY '\\r\\n' ";
            MySqlCommand Comando2 = new MySqlCommand(Sql2, Conexao);
            Comando2.ExecuteNonQuery();

            String Sql3 = "Create Temporary Table Tempos SELECT b.Dia,b.Hora,b.Nome,b.Form1,b.Form2,b.Prod_1,b.Prod_2,b.Prod_3,b.Prod_4,b.Prod_5,b.Prod_6,b.Prod_7,b.Prod_8,b.Prod_9,b.Prod_10,b.Prod_11,b.Prod_12,b.Prod_13,b.Prod_14,b.Prod_15,b.Prod_16,b.Prod_17,b.Prod_18,b.Prod_19,b.Prod_20,b.Prod_21,b.Prod_22,b.Prod_23,b.Prod_24,b.Prod_25,b.Prod_26,b.Prod_27,b.Prod_28,b.Prod_29,b.Prod_30,b.Prod_31,b.Prod_32 FROM cadastro.Temp as b LEFT JOIN cadastro.relatorio as a on b.Dia = a.Dia and b.Hora = a.Hora WHERE a.Dia is null";
            MySqlCommand Comando3 = new MySqlCommand(Sql3, Conexao);
            Comando3.ExecuteNonQuery();

            String Sql7 = "Delete from Tempos where Hora is null";
            MySqlCommand Comando7 = new MySqlCommand(Sql7, Conexao);
            Comando7.ExecuteNonQuery();

            String Sql6 = "Select Count(*) from cadastro.tempos";
            MySqlCommand Comando6 = new MySqlCommand(Sql6, Conexao);
            Object linhas;
            linhas = Comando6.ExecuteScalar();
            int LinhasInt;
            LinhasInt = Convert.ToInt16(linhas);

            if (LinhasInt != 0)
            {

                String Sql4 = "Insert into relatorio select * from cadastro.Tempos";
                MySqlCommand Comando4 = new MySqlCommand(Sql4, Conexao);
                Comando4.ExecuteNonQuery();
                String Sql5 = "Drop Table cadastro.Tempos";
                MySqlCommand Comando5 = new MySqlCommand(Sql5, Conexao);
                Comando5.ExecuteNonQuery();

                String Sql9 = "Drop Table cadastro.Temp";
                MySqlCommand Comando9 = new MySqlCommand(Sql9, Conexao);
                Comando9.ExecuteNonQuery();

                Conexao.Close();



                MessageBox.Show("Realizado com sucesso!");

            }
            else
            {
                String Sql5 = "Drop Table cadastro.Tempos";
                MySqlCommand Comando5 = new MySqlCommand(Sql5, Conexao);
                Comando5.ExecuteNonQuery();

                String Sql9 = "Drop Table cadastro.Temp";
                MySqlCommand Comando9 = new MySqlCommand(Sql9, Conexao);
                Comando9.ExecuteNonQuery();


                Conexao.Close();
                MessageBox.Show("Nenhum registro novo!");

            }

        }
        public static void Carregar_ComboData() // Não usado - Teste para carregar apenas o formulario aberto.
        {

        }

        public static void testeftp() //teste fora de uso para usar o a propria aplicação para processar download ...
        {

        }
    }
    
}

        
    
    

