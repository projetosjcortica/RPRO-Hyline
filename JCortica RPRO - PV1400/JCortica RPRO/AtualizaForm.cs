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
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Timers;
using System.Net;


namespace JCortica_RPRO
{
    public partial class AtualizaForm : Form
    {
        public string str1 { get; set; }
        public string str2 { get; set; }
        public string str3 { get; set; }
        public string str4 { get; set; }
        public int AnoNum { get; set; }
        public int MesNum { get; set; }
        public bool AnoNumOK { get; set; }
        public bool MesNumOK { get; set; }

        public AtualizaForm()
        {
            InitializeComponent();
           

        }

        private void button1_Click(object sender, EventArgs e)
        {
          
            TesteAtualiza();

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public void ProcuraeCompara()
        {
         
        }

        public void Pesquisa()
        {
            
        }

        public void testeteste()
        {
            




        }

        public void TesteAtualiza()
        {
            

        }



        public void AtualizaBanco2_Central() {


            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            int UltTabela = 0;

            DataTable tabelanomes = new DataTable();

            uint TotalRegistros;



            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);

            Conexao.Open();

            listView1.View = View.Details;
            listView1.Columns.Add("Arquivo", 160);
            listView1.Columns.Add("Status", 80);
            listView1.Columns.Add("Ultima Atualização", 160);
            listView1.Columns.Add("Detalhes", 160);


            //Lendo diretorio de arquivos
            string[] arquivos = Directory.GetFiles(@"C:\JCortica\BancoCSV20", "Relatorio_20*", SearchOption.AllDirectories)
                            .Where(s => s.EndsWith(".csv", StringComparison.OrdinalIgnoreCase)).ToArray();

            int numarq = arquivos.Length;
            StatusBox.Text = numarq.ToString();
            progressBar1.Maximum = numarq-1;
            progressBar1.Minimum = 0;
            for (int i = 0; i < numarq; i++)//Peso1
            {
                progressBar1.Value = i;
                progressBar1.Refresh();                
                int Limite = arquivos[i].LastIndexOf(".csv");
                int Letra1 = arquivos[i].IndexOf("Relatorio_20");

                //FASE 1 - Teste para ver se o arquivo é legitimo em quantidade caracteres 
                if (Limite - Letra1 == 17)
                {
                    str1 = arquivos[i].Substring(Letra1, 21); //Nome Arquivo Completo
                    str2 = arquivos[i].Substring(Letra1 + 10, 4); //Ano do Arquivo
                    str3 = arquivos[i].Substring(Letra1 + 15, 2); //Mes do Arquivo
                    str4 = str2 + str3; //Codigo Arquivo (AnoMes)
                    int Numero1;
                    int Numero2;
                    AnoNumOK = int.TryParse(str2, out Numero1);
                    MesNumOK = int.TryParse(str3, out Numero2);

                    //FASE 2 - Ve se os dados de data são letigitimos
                    if (AnoNumOK == true && MesNumOK == true)
                    {
                        AnoNum = Numero1;
                        MesNum = Numero2;

                        FileInfo fi2 = new FileInfo(arquivos[i]);
                        DateTime UltAtualiza = fi2.LastAccessTime;
                        long PesoArq = fi2.Length;
                        bool AttOK = false;

                        String Sql = "Select * from cadastro.nomearq where Codigo = @code ";
                        MySqlCommand Comando = new MySqlCommand(Sql, Conexao);
                        Comando.Parameters.AddWithValue("@code", str4);

                        tabelanomes.Clear();

                        MySqlDataAdapter objAdapter = new MySqlDataAdapter(Comando);
                        objAdapter.Fill(tabelanomes);

                        //-------------------------------------------------------------------------------------------------
                        //----*******------*******-------*******----
                        // EXISTE CADASTRO NO BANCO DE FONTE (nomearq)
                        if (tabelanomes.Rows.Count != 0)//1
                        {
                            //Nao precisa fazer atualização
                            if (PesoArq.ToString() == tabelanomes.Rows[0]["Tamanho"].ToString())
                            {
                                ListViewItem item = new ListViewItem(new[] { str1, "Atualizado", tabelanomes.Rows[0]["Ultimaban"].ToString(),"" });
                                listView1.Items.Add(item);
                                listView1.Refresh();
                                StatusBox.Text = str1 + " " + "Atualizado" + " " + tabelanomes.Rows[0]["Ultimaban"].ToString();
                                StatusBox.Refresh();
                            }
                            //Existe cadastro mas não está atualizado - Irá procurar o que precisa ser atualizado cruzando informações
                            else//Atualiza1
                            {
                                DateTime timeStamp = DateTime.Now;

                                TotalRegistros =0;

                                ListViewItem item2 = new ListViewItem(new[] { str1, "Atualizando...",tabelanomes.Rows[0]["Ultimaban"].ToString(),"..." });
                                listView1.Items.Add(item2);
                                listView1.Refresh();

                                StatusBox.Text = str1 + " " + "Atualizando..." + " " + tabelanomes.Rows[0]["Ultimaban"].ToString();
                                StatusBox.Refresh();

                                String Atsql1 = "Create Temporary Table cadastro.Tempat1(Dia varchar(10), Hora time, Nome varchar(30), Form1 int, Form2 int, Prod_1 int, Prod_2 int, Prod_3 int, Prod_4 int, Prod_5 int, Prod_6 int, Prod_7 int, Prod_8 int, Prod_9 int, Prod_10 int, Prod_11 int, Prod_12 int, Prod_13 int, Prod_14 int, Prod_15 int, Prod_16 int, Prod_17 int, Prod_18 int, Prod_19 int, Prod_20 int, Prod_21 int, Prod_22 int, Prod_23 int, Prod_24 int, Prod_25 int, Prod_26 int, Prod_27 int, Prod_28 int, Prod_29 int, Prod_30 int, Prod_31 int, Prod_32 int)";
                                MySqlCommand AtComando1 = new MySqlCommand(Atsql1, Conexao);
                                AtComando1.ExecuteNonQuery();

                                String ArquivoCSV = arquivos[i].Replace("\\", "/");
                                String Spath = "'" + ArquivoCSV + "'";
                                String Atsql2 = "LOAD DATA LOCAL INFILE " + Spath + " INTO TABLE cadastro.Tempat1 FIELDS TERMINATED BY ',' LINES TERMINATED BY '\\r\\n'";
                                MySqlCommand AtComando2 = new MySqlCommand(Atsql2, Conexao);
                                AtComando2.ExecuteNonQuery();

                                String Atsql3 = "Delete from cadastro.Tempat1 where month(str_to_date(dia, '%d/%m/%Y')) <> @data1 or year(str_to_date(dia, '%d/%m/%Y')) <> @data2";
                                MySqlCommand AtComando3 = new MySqlCommand(Atsql3, Conexao);
                                AtComando3.Parameters.AddWithValue("@data1", str3);
                                AtComando3.Parameters.AddWithValue("@data2", str2);
                                AtComando3.ExecuteNonQuery();

                                String Atsql3X = "Select Count(*) from cadastro.Tempat1";
                                MySqlCommand Comando3X = new MySqlCommand(Atsql3X, Conexao);
                                Object Linhas3X;
                                Linhas3X = Comando3X.ExecuteScalar();
                                uint LinhasInt3X;
                                LinhasInt3X = Convert.ToUInt32(Linhas3X);

                                UltTabela = listView1.Items.Count;
                                UltTabela = UltTabela - 1;

                                //Sessão A1 - Há dados para serem atualizados no arquivo csv
                                if (LinhasInt3X != 0)//Sessão A1
                                { 
                                    String Atsql4 = "Create Temporary Table cadastro.ultimat1 select * from cadastro.relatorio where month(str_to_date(dia, '%d/%m/%Y')) = @data1 and year(str_to_date(dia, '%d/%m/%Y')) = @data2";
                                    MySqlCommand AtComando4 = new MySqlCommand(Atsql4, Conexao);
                                    AtComando4.Parameters.AddWithValue("@data1", str3);
                                    AtComando4.Parameters.AddWithValue("@data2", str2);
                                    AtComando4.ExecuteNonQuery();

                                    String Atsql4X = "Select Count(*) from cadastro.ultimat1";
                                    MySqlCommand Comando4X = new MySqlCommand(Atsql4X, Conexao);
                                    Object Linhas4X;
                                    Linhas4X = Comando4X.ExecuteScalar();
                                    uint LinhasInt4X;
                                    LinhasInt4X = Convert.ToUInt32(Linhas4X);

                                    String Atsql4X2 = "Drop table cadastro.ultimat1";
                                    MySqlCommand Comando4X2 = new MySqlCommand(Atsql4X2, Conexao);
                                    Comando4X2.ExecuteNonQuery();

                                    //Há dados para serem cruzados
                                    //EMULADOR DE DADOS POR MES E DIA -------
                                    if (LinhasInt4X != 0)//Sessão A2
                                    {


                                        for (int x = 1; x < 32; x++)//Sessão A3
                                        {
                                            int porcentagem;
                                            porcentagem = x * 100 / 31;
                                            listView1.Items[UltTabela].SubItems[3].Text = "Lendo arquivo:" + porcentagem.ToString() + "%";
                                            listView1.Refresh();

                                            StatusBox.Text = str1 + "Lendo arquivo:" + porcentagem.ToString() + "%";
                                            StatusBox.Refresh();

                                            String Emsql1 = "Create Temporary Table cadastro.nova1 select * from cadastro.tempat1 where day(str_to_date(dia, '%d/%m/%Y')) = @dado1 and month(str_to_date(dia, '%d/%m/%Y')) = @dado2 and year(str_to_date(dia, '%d/%m/%Y')) = @dado3";
                                            MySqlCommand Ecomando1 = new MySqlCommand(Emsql1, Conexao);
                                            Ecomando1.Parameters.AddWithValue("@dado1", x.ToString()); //Dia
                                            Ecomando1.Parameters.AddWithValue("@dado2", str3); //Mes
                                            Ecomando1.Parameters.AddWithValue("@dado3", str2); //Ano
                                            Ecomando1.ExecuteNonQuery();

                                            String Emsql2 = "Select Count(*) from cadastro.nova1";
                                            MySqlCommand Ecomando2 = new MySqlCommand(Emsql2, Conexao);
                                            Object linhasE2;
                                            linhasE2 = Ecomando2.ExecuteScalar();
                                            uint LinhasIntE2;
                                            LinhasIntE2 = Convert.ToUInt32(linhasE2);

                                            //Há dados no dia Especificio do arquivo csv
                                            if (LinhasIntE2 != 0) //Sessão A4
                                            {
                                                String Emsql3 = "Create Temporary Table cadastro.recorte1 select * from cadastro.relatorio where day(str_to_date(dia, '%d/%m/%Y')) = @dado4 and month(str_to_date(dia, '%d/%m/%Y')) = @dado5 and year(str_to_date(dia, '%d/%m/%Y')) = @dado6";
                                                MySqlCommand Ecomando3 = new MySqlCommand(Emsql3, Conexao);
                                                Ecomando3.Parameters.AddWithValue("@dado4", x.ToString()); //Dia
                                                Ecomando3.Parameters.AddWithValue("@dado5", str3); //Mes
                                                Ecomando3.Parameters.AddWithValue("@dado6", str2); //Ano
                                                Ecomando3.ExecuteNonQuery();
                                                /*
                                                String Emsql3x = "Select Count(*) from cadastro.recorte1";
                                                MySqlCommand EComando3X = new MySqlCommand(Emsql3x, Conexao);
                                                Object LinhaEm3x;
                                                LinhaEm3x = EComando3X.ExecuteScalar();
                                                uint LinhasIntEm3x;
                                                LinhasIntEm3x = Convert.ToUInt32(Linhas4X);

                                                if (LinhasIntEm3x != 0)
                                                { */

                                                    String Emsql4 = "Create Temporary Table cadastro.Tempos SELECT b.Dia,b.Hora,b.Nome,b.Form1,b.Form2,b.Prod_1,b.Prod_2,b.Prod_3,b.Prod_4,b.Prod_5,b.Prod_6,b.Prod_7,b.Prod_8,b.Prod_9,b.Prod_10,b.Prod_11,b.Prod_12,b.Prod_13,b.Prod_14,b.Prod_15,b.Prod_16,b.Prod_17,b.Prod_18,b.Prod_19,b.Prod_20,b.Prod_21,b.Prod_22,b.Prod_23,b.Prod_24,b.Prod_25,b.Prod_26,b.Prod_27,b.Prod_28,b.Prod_29,b.Prod_30,b.Prod_31,b.Prod_32 FROM cadastro.nova1 as b LEFT JOIN cadastro.recorte1 as a on b.Dia = a.Dia and b.Hora = a.Hora WHERE a.Dia is null";
                                                    MySqlCommand Ecomando4 = new MySqlCommand(Emsql4, Conexao);
                                                    Ecomando4.ExecuteNonQuery();

                                                    String Emsql5 = "Select Count(*) from cadastro.Tempos";
                                                    MySqlCommand Ecomando5 = new MySqlCommand(Emsql5, Conexao);
                                                    Object Elinhas5;
                                                    Elinhas5 = Ecomando5.ExecuteScalar();
                                                    uint ELinhasInt5;
                                                    ELinhasInt5 = Convert.ToUInt16(Elinhas5);

                                                    //Coloca os dados no relatorio
                                                    if (ELinhasInt5 != 0)//Sessão A5 
                                                    {
                                                        TotalRegistros = TotalRegistros + ELinhasInt5;
                                                        String Emsql6 = "Insert into cadastro.relatorio select * from cadastro.tempos";
                                                        MySqlCommand Ecomando6 = new MySqlCommand(Emsql6, Conexao);
                                                        Ecomando6.ExecuteNonQuery();
                                                    }

                                                    String Emsql8 = "drop table cadastro.Tempos";
                                                    MySqlCommand Ecomando8 = new MySqlCommand(Emsql8, Conexao);
                                                    Ecomando8.ExecuteNonQuery();

                                                    String Emsql9 = "drop table cadastro.recorte1";
                                                    MySqlCommand Ecomando9 = new MySqlCommand(Emsql9, Conexao);
                                                    Ecomando9.ExecuteNonQuery();

                                                //}
                                            }//Fim SESSÂO A4
                                            String Emsql10 = "drop table cadastro.nova1";
                                            MySqlCommand Ecomando10 = new MySqlCommand(Emsql10, Conexao);
                                            Ecomando10.ExecuteNonQuery();

                                        } // SESSÂO 3 - Fim do Loop de Dia 


                                        ListViewItem item4 = new ListViewItem(new[] { str1, "Atualizado",timeStamp.ToString(),TotalRegistros + " Novos Registros" });
                                        listView1.Items.Add(item4);
                                        listView1.Refresh();

                                        StatusBox.Text = str1 + " " + "Atualizado" + " " + timeStamp.ToString() + " " + TotalRegistros + " Novos Registros";
                                        StatusBox.Refresh();
                                    } //FIM DA SEÇÃO A2 
                                    else
                                    {   
                                        //Atualização direta para banco 
                                        String A1Sql1 = "Insert into cadastro.relatorio select * from cadastro.ultimat1";
                                        MySqlCommand A1comando1 = new MySqlCommand(A1Sql1, Conexao);
                                        A1comando1.ExecuteNonQuery();
                                        /*
                                        String A1Sql2 = "drop table cadastro.ultimat1";
                                        MySqlCommand A1comando2 = new MySqlCommand(A1Sql2, Conexao);
                                        A1comando2.ExecuteNonQuery();
                                        */

                                        ListViewItem item4 = new ListViewItem(new[] { str1, "Atualizado", timeStamp.ToString(), LinhasInt3X + " Novos Registros" });
                                        listView1.Items.Add(item4);
                                        listView1.Refresh();

                                        StatusBox.Text = str1 + " " + "Atualizado" + " " + timeStamp.ToString() + " " + LinhasInt3X + " Novos Registros";
                                        StatusBox.Refresh();
                                    }
                                }
                                else
                                {
                                    ListViewItem item5 = new ListViewItem(new[] { str1, "Atualizado", LinhasInt3X + "Nenhum registro novo" });
                                    listView1.Items.Add(item5);
                                    listView1.Refresh();

                                    StatusBox.Text = str1 + " " + "Atualizado" + " " + LinhasInt3X + "Nenhum registro novo" ;
                                    StatusBox.Refresh();
                                }

                                String Asql1 = "drop table cadastro.tempat1";
                                MySqlCommand A0Comando1 = new MySqlCommand(Asql1, Conexao);
                                A0Comando1.ExecuteNonQuery();

                                //DateTime timeStamp = DateTime.Now;

                                int AnoUltimo = Convert.ToInt16(UltAtualiza.Year);
                                int MesUltimo = Convert.ToInt16(UltAtualiza.Month);

                                AttOK = false;

                                if (AnoUltimo > AnoNum) { AttOK = true; }
                                else
                                {
                                    if (AnoUltimo == AnoNum)
                                    {
                                        if (MesUltimo > MesNum)
                                        {
                                            AttOK = true;
                                        }
                                    }
                                }

                               

                                //Atualiza Registro de fonte do Banco 
                                String SSqlX1 = "Update cadastro.nomearq set ultimaarq = @ultima_arq, ultimaban=@ultima_ban, tamanho = @peso, Dtok = @atok  where codigo=@code";
                                MySqlCommand XSComando1 = new MySqlCommand(SSqlX1, Conexao);
                                XSComando1.Parameters.AddWithValue("@code", str4);
                                XSComando1.Parameters.AddWithValue("@ultima_arq", UltAtualiza);
                                XSComando1.Parameters.AddWithValue("@ultima_ban", timeStamp);
                                XSComando1.Parameters.AddWithValue("@peso", PesoArq);
                                XSComando1.Parameters.AddWithValue("@atok", AttOK);
                                XSComando1.ExecuteNonQuery();
                              }
                            } //Fim Atualização com cadastro no banco

                        //-------------------------------------------------------------------------------------------------
                        //----*******------*******-------*******----
                        // NÃO EXISTE CADASTRO NO BANCO DE FONTE (nomearq)
                        //São o mesmos comandos com a diferença de criar uma linha no nome do arquivo registrando o banco
                        else//Atualiza2
                        {
                            TotalRegistros = 0;

                            DateTime timeStamp = DateTime.Now;

                            ListViewItem item2 = new ListViewItem(new[] { str1, "Atualizando...","...","..."  });
                            listView1.Items.Add(item2);
                            listView1.Refresh();

                            StatusBox.Text = str1 + " " + "Atualizado" + " ";
                            StatusBox.Refresh();

                            String Atsql1 = "Create Temporary Table cadastro.Tempat1(Dia varchar(10), Hora time, Nome varchar(30), Form1 int, Form2 int, Prod_1 int, Prod_2 int, Prod_3 int, Prod_4 int, Prod_5 int, Prod_6 int, Prod_7 int, Prod_8 int, Prod_9 int, Prod_10 int, Prod_11 int, Prod_12 int, Prod_13 int, Prod_14 int, Prod_15 int, Prod_16 int, Prod_17 int, Prod_18 int, Prod_19 int, Prod_20 int, Prod_21 int, Prod_22 int, Prod_23 int, Prod_24 int, Prod_25 int, Prod_26 int, Prod_27 int, Prod_28 int, Prod_29 int, Prod_30 int, Prod_31 int, Prod_32 int)";
                            MySqlCommand AtComando1 = new MySqlCommand(Atsql1, Conexao);
                            AtComando1.ExecuteNonQuery();

                            String ArquivoCSV = arquivos[i].Replace("\\", "/");
                            String Spath = "'" + ArquivoCSV + "'";
                            String Atsql2 = "LOAD DATA LOCAL INFILE " + Spath + " INTO TABLE cadastro.Tempat1 FIELDS TERMINATED BY ',' LINES TERMINATED BY '\\r\\n'";
                            MySqlCommand AtComando2 = new MySqlCommand(Atsql2, Conexao);
                            AtComando2.ExecuteNonQuery();

                            String Atsql3 = "Delete from cadastro.Tempat1 where month(str_to_date(dia, '%d/%m/%Y')) <> @data1 or year(str_to_date(dia, '%d/%m/%Y')) <> @data2";
                            MySqlCommand AtComando3 = new MySqlCommand(Atsql3, Conexao);
                            AtComando3.Parameters.AddWithValue("@data1", str3);
                            AtComando3.Parameters.AddWithValue("@data2", str2);
                            AtComando3.ExecuteNonQuery();

                            String Atsql3X = "Select Count(*) from cadastro.Tempat1";
                            MySqlCommand Comando3X = new MySqlCommand(Atsql3X, Conexao);
                            Object Linhas3X;
                            Linhas3X = Comando3X.ExecuteScalar();
                            uint LinhasInt3X;
                            LinhasInt3X = Convert.ToUInt32(Linhas3X);

                            UltTabela = listView1.Items.Count;
                            UltTabela = UltTabela - 1;

                            //Sessão A1 - Há dados para serem atualizados no arquivo csv
                            if (LinhasInt3X != 0)//Sessão A1
                            {
                                String Atsql4 = "Create Temporary Table cadastro.ultimat1 select * from cadastro.relatorio where month(str_to_date(dia, '%d/%m/%Y')) = @data1 and year(str_to_date(dia, '%d/%m/%Y')) = @data2";
                                MySqlCommand AtComando4 = new MySqlCommand(Atsql4, Conexao);
                                AtComando4.Parameters.AddWithValue("@data1", str3);
                                AtComando4.Parameters.AddWithValue("@data2", str2);
                                AtComando4.ExecuteNonQuery();

                                String Atsql4X = "Select Count(*) from cadastro.ultimat1";
                                MySqlCommand Comando4X = new MySqlCommand(Atsql4X, Conexao);
                                Object Linhas4X;
                                Linhas4X = Comando4X.ExecuteScalar();
                                uint LinhasInt4X;
                                LinhasInt4X = Convert.ToUInt32(Linhas4X);

                                String Atsql4X2 = "Drop table cadastro.ultimat1";
                                MySqlCommand Comando4X2 = new MySqlCommand(Atsql4X2, Conexao);
                                Comando4X2.ExecuteNonQuery();

                                //Há dados para serem cruzados
                                //EMULADOR DE DADOS POR MES E DIA -------
                                if (LinhasInt4X != 0)//Sessão A2
                                {


                                    for (int x = 1; x < 32; x++)//Sessão A3
                                    {
                                        int porcentagem;
                                        porcentagem = x * 100 / 31;
                                        listView1.Items[UltTabela].SubItems[3].Text = "Lendo arquivo:" + porcentagem.ToString() + "%";
                                        listView1.Refresh();

                                        StatusBox.Text = str1 + " " + "Atualizando" + " " + "Lendo arquivo:" + porcentagem.ToString() + "%";
                                        StatusBox.Refresh();

                                        String Emsql1 = "Create Temporary Table cadastro.nova1 select * from cadastro.tempat1 where day(str_to_date(dia, '%d/%m/%Y')) = @dado1 and month(str_to_date(dia, '%d/%m/%Y')) = @dado2 and year(str_to_date(dia, '%d/%m/%Y')) = @dado3";
                                        MySqlCommand Ecomando1 = new MySqlCommand(Emsql1, Conexao);
                                        Ecomando1.Parameters.AddWithValue("@dado1", x.ToString()); //Dia
                                        Ecomando1.Parameters.AddWithValue("@dado2", str3); //Mes
                                        Ecomando1.Parameters.AddWithValue("@dado3", str2); //Ano
                                        Ecomando1.ExecuteNonQuery();

                                        String Emsql2 = "Select Count(*) from cadastro.nova1";
                                        MySqlCommand Ecomando2 = new MySqlCommand(Emsql2, Conexao);
                                        Object linhasE2;
                                        linhasE2 = Ecomando2.ExecuteScalar();
                                        uint LinhasIntE2;
                                        LinhasIntE2 = Convert.ToUInt32(linhasE2);

                                        //Há dados no dia Especificio do arquivo csv
                                        if (LinhasIntE2 != 0) //Sessão A4
                                        {
                                            String Emsql3 = "Create Temporary Table cadastro.recorte1 select * from cadastro.relatorio where day(str_to_date(dia, '%d/%m/%Y')) = @dado4 and month(str_to_date(dia, '%d/%m/%Y')) = @dado5 and year(str_to_date(dia, '%d/%m/%Y')) = @dado6";
                                            MySqlCommand Ecomando3 = new MySqlCommand(Emsql3, Conexao);
                                            Ecomando3.Parameters.AddWithValue("@dado4", x.ToString()); //Dia
                                            Ecomando3.Parameters.AddWithValue("@dado5", str3); //Mes
                                            Ecomando3.Parameters.AddWithValue("@dado6", str2); //Ano
                                            Ecomando3.ExecuteNonQuery();

                                            /*
                                            String Emsql3x = "Select Count(*) from cadastro.recorte1";
                                            MySqlCommand EComando3X = new MySqlCommand(Emsql3x, Conexao);
                                            Object LinhaEm3x;
                                            LinhaEm3x = EComando3X.ExecuteScalar();
                                            uint LinhasIntEm3x;
                                            LinhasIntEm3x = Convert.ToUInt32(Linhas4X);

                                            if (LinhasIntEm3x != 0)
                                            {
                                            */
                                                String Emsql4 = "Create Temporary Table cadastro.Tempos SELECT b.Dia,b.Hora,b.Nome,b.Form1,b.Form2,b.Prod_1,b.Prod_2,b.Prod_3,b.Prod_4,b.Prod_5,b.Prod_6,b.Prod_7,b.Prod_8,b.Prod_9,b.Prod_10,b.Prod_11,b.Prod_12,b.Prod_13,b.Prod_14,b.Prod_15,b.Prod_16,b.Prod_17,b.Prod_18,b.Prod_19,b.Prod_20,b.Prod_21,b.Prod_22,b.Prod_23,b.Prod_24,b.Prod_25,b.Prod_26,b.Prod_27,b.Prod_28,b.Prod_29,b.Prod_30,b.Prod_31,b.Prod_32 FROM cadastro.nova1 as b LEFT JOIN cadastro.recorte1 as a on b.Dia = a.Dia and b.Hora = a.Hora WHERE a.Dia is null";
                                                MySqlCommand Ecomando4 = new MySqlCommand(Emsql4, Conexao);
                                                Ecomando4.ExecuteNonQuery();

                                                String Emsql5 = "Select Count(*) from cadastro.Tempos";
                                                MySqlCommand Ecomando5 = new MySqlCommand(Emsql5, Conexao);
                                                Object Elinhas5;
                                                Elinhas5 = Ecomando5.ExecuteScalar();
                                                uint ELinhasInt5;
                                                ELinhasInt5 = Convert.ToUInt16(Elinhas5);

                                                //Coloca os dados no relatorio
                                                if (ELinhasInt5 != 0)//Sessão A5 
                                                {
                                                    TotalRegistros = TotalRegistros + ELinhasInt5;
                                                    String Emsql6 = "Insert into cadastro.relatorio select * from cadastro.Tempos";
                                                    MySqlCommand Ecomando6 = new MySqlCommand(Emsql6, Conexao);
                                                    Ecomando6.ExecuteNonQuery();
                                                }

                                                String Emsql8 = "drop table cadastro.Tempos";
                                                MySqlCommand Ecomando8 = new MySqlCommand(Emsql8, Conexao);
                                                Ecomando8.ExecuteNonQuery();

                                                String Emsql9 = "drop table cadastro.recorte1";
                                                MySqlCommand Ecomando9 = new MySqlCommand(Emsql9, Conexao);
                                                Ecomando9.ExecuteNonQuery();

                                            //}
                                        } //Fim SESSÂO A4
                                        String Emsql10 = "drop table cadastro.nova1";
                                        MySqlCommand Ecomando10 = new MySqlCommand(Emsql10, Conexao);
                                        Ecomando10.ExecuteNonQuery();

                                    } // SESSÂO 3 - Fim do Loop de Dia 


                                    ListViewItem item4 = new ListViewItem(new[] { str1, "Atualizado", timeStamp.ToString(), TotalRegistros + " Novos Registros" });
                                    listView1.Items.Add(item4);
                                    listView1.Refresh();

                                    StatusBox.Text = str1 + " " + "Atualizado" + " " + timeStamp.ToString() + " " + TotalRegistros + " Novos Registros";
                                    StatusBox.Refresh();
                                } //FIM DA SEÇÃO A2 
                                else
                                {   //Atualização direta para banco 
                                    String A1Sql1 = "Insert into cadastro.relatorio select * from cadastro.Tempat1";
                                    MySqlCommand A1comando1 = new MySqlCommand(A1Sql1, Conexao);
                                    A1comando1.ExecuteNonQuery();
                                    /*
                                    String A1Sql2 = "drop table cadastro.Tempat1";
                                    MySqlCommand A1comando2 = new MySqlCommand(A1Sql2, Conexao);
                                    A1comando2.ExecuteNonQuery();
                                    */

                                    ListViewItem item4 = new ListViewItem(new[] { str1, "Atualizado",timeStamp.ToString(),LinhasInt3X + " Novos Registros" });
                                    listView1.Items.Add(item4);
                                    listView1.Refresh();

                                    StatusBox.Text = str1 + " " + "Atualizado" + " " + timeStamp.ToString() + " " + LinhasInt3X + " Novos Registros";
                                    StatusBox.Refresh();
                                }
                            }
                            else
                            {
                                ListViewItem item5 = new ListViewItem(new[] { str1, "Atualizado", timeStamp.ToString(), LinhasInt3X + "Nenhum registro novo" });
                                listView1.Items.Add(item5);
                                listView1.Refresh();

                                StatusBox.Text = str1 + " " + "Atualizado" + " " + timeStamp.ToString() + " " + LinhasInt3X + "Nenhum registro novo";
                                StatusBox.Refresh();
                            }

                            String Asql1 = "drop table cadastro.tempat1";
                            MySqlCommand A0Comando1 = new MySqlCommand(Asql1, Conexao);
                            A0Comando1.ExecuteNonQuery();

                            int AnoUltimo = Convert.ToInt16(UltAtualiza.Year);
                            int MesUltimo = Convert.ToInt16(UltAtualiza.Month);

                            AttOK = false;

                            if (AnoUltimo > AnoNum) { AttOK = true; }
                            else
                            {
                                if (AnoUltimo == AnoNum)
                                {
                                    if (MesUltimo > MesNum)
                                    {
                                        AttOK = true;
                                    }
                                }
                            }

                            //Atualiza Registro de fonte do Banco 
                            String SSqlX1 = "Insert into cadastro.nomearq values(@codex,@anox,@mesx,@arquivox,@ultima_arqx,@ultima_banx,@pesox,@atokx)";
                            MySqlCommand XSComando1 = new MySqlCommand(SSqlX1, Conexao);
                            XSComando1.Parameters.AddWithValue("@codex", str4);
                            XSComando1.Parameters.AddWithValue("@anox", AnoNum);
                            XSComando1.Parameters.AddWithValue("@mesx", MesNum);
                            XSComando1.Parameters.AddWithValue("@arquivox", str1);
                            XSComando1.Parameters.AddWithValue("@ultima_arqx", UltAtualiza);
                            XSComando1.Parameters.AddWithValue("@ultima_banx", timeStamp);
                            XSComando1.Parameters.AddWithValue("@pesox", PesoArq);
                            XSComando1.Parameters.AddWithValue("@atokx", AttOK);
                            XSComando1.ExecuteNonQuery();
                        }
                    } //Fim Atualização com cadastro no banco
                }
        }

            Conexao.Close();
        }

        public void ProcuraSqlArquivo()
        {

            
            
        }

        public void testeftp()
        {
           
        }

        public void PesquisaArquivos()
        {
           
        }

        private void AtualizaForm_Load(object sender, EventArgs e)
        {
            
        }

        public void Testa_Mysql()
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();


        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            AtualizaBanco2_Central();
        }

        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }

}
