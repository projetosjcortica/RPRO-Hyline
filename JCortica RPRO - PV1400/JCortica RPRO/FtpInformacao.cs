using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using MySql.Data.MySqlClient;

namespace JCortica_RPRO
{
    public partial class FtpInformacao : Form
    {
        public string str1 { get; set; }
        public string str2 { get; set; }
        public string str3 { get; set; }
        public string str4 { get; set; }
        public int AnoNum { get; set; }
        public int MesNum { get; set; }
        public bool AnoNumOK { get; set; }
        public bool MesNumOK { get; set; }

        public static List<string> ArquivosnoFTP = new List<string>();
        
        public FtpInformacao()
        {
            InitializeComponent();
            
           

        }

        private void FtpInformacao_Load(object sender, EventArgs e)
        {
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            
            
            // Call this procedure when the application starts.  
            // Set to 1 second.  
            timer1.Interval = 2000;
            timer1.Tick += new EventHandler(Timer1_Tick);

            Statusbox.Text = "Preparando atualização...";
            // Enable timer.  
            timer1.Enabled = true;

            
        }

        private void Timer1_Tick(object Sender, EventArgs e)
        {
            timer1.Enabled = false;
            ChamaAtualiza();
            
        }

        private void InitializeTimer2()
        {
            // Call this procedure when the application starts.  
            // Set to 1 second.  
            timer2.Interval = 1000;
            timer2.Tick += new EventHandler(Timer2_Tick);

            
            // Enable timer.  
            timer2.Enabled = true;


        }

        private void Timer2_Tick(object Sender, EventArgs e)
        {
            timer2.Enabled = false;
            this.Close();
        }

        public void ChamaAtualiza()
        {

            bool PorMes = Properties.Settings.Default.MetodoMesCSV;
            bool bFormula = Properties.Settings.Default.FormulaCSV;

            Statusbox.Text = "Conectando ftp...";
            Statusbox.Refresh();
            bool result = FtpDirectoryExists(@"ftp://" + Properties.Settings.Default.IP + "/", Properties.Settings.Default.UserFTP, Properties.Settings.Default.SenhaFTP);
            if (result == true)
            {
                ObterInformacao();
                if (PorMes == true)
                {
                    AtualizaBanco2_Central();
                }
                else
                {
                    AtualizaBanco_Unico_Central();
                }

                
                if (bFormula == true)
                {
                    AtualizaFormulas();
                }

            }
            else
            {
                MessageBox.Show("Falha na conexão ftp");
                this.Close();
            }
        }
        public void ObterInformacao()
        {

            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            List<string> liArquivos = new List<string>();
            //Cria comunicação com o servidor
            //Definir o diretório a ser listado
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(@"ftp://"+Properties.Settings.Default.IP+"/InternalStorage/data/");
            //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(@"ftp://" + Properties.Settings.Default.IP + "/usb1/");
            //Define que a ação vai ser de listar diretório
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            //Credenciais para o login (usuario, senha)
            request.Credentials = new NetworkCredential(Properties.Settings.Default.UserFTP, Properties.Settings.Default.SenhaFTP);
            //modo passivo
            request.UsePassive = true;
            //dados binarios
            request.UseBinary = true;
            //setar o KeepAlive para true
            request.KeepAlive = true;

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                //Criando a Stream para pegar o retorno
                Stream responseStream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(responseStream))
                {
                    //Adicionar os arquivos na lista
                    liArquivos = reader.ReadToEnd().Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList<string>();
                }
            }

           
            bool PorMes = Properties.Settings.Default.MetodoMesCSV;
            bool bFormula = Properties.Settings.Default.FormulaCSV;

            if(PorMes == true) {

                //Responder a lista dos arquivos
                foreach (string item in liArquivos)
                {

                    int Limite = item.LastIndexOf(".csv");
                    int Letra1 = item.IndexOf("Relatorio_20");

                    //FASE 1 - Teste para ver se o arquivo é legitimo em quantidade caracteres 
                    if (Limite - Letra1 == 17)
                    {
                        String str1 = item.Substring(Letra1, 21); //Nome Arquivo Completo
                        String str2 = item.Substring(Letra1 + 10, 4); //Ano do Arquivo
                        String str3 = item.Substring(Letra1 + 15, 2); //Mes do Arquivo
                        String str4 = str2 + str3; //Codigo Arquivo (AnoMes)
                        int Numero1;
                        int Numero2;
                        bool AnoNumOK = int.TryParse(str2, out Numero1);
                        bool MesNumOK = int.TryParse(str3, out Numero2);

                        //FASE 2 - Ve se os dados de data são letigitimos
                        if (AnoNumOK == true && MesNumOK == true)
                        {
                            ArquivosnoFTP.Add(item);
                        }
                    }
                    if (bFormula == true)
                    {
                        if (item == "Formula.csv")
                        {
                            ArquivosnoFTP.Add(item);
                        }
                    }

                }

            
            }
            else //Metodo por arquivo unico
            {
                foreach (string item in liArquivos)
                {
                    if (item == "Relatorio_1.csv")
                    {
                        ArquivosnoFTP.Add(item);
                    }

                    if (bFormula == true)
                    {
                        if (item == "Formula.csv")
                        {
                            ArquivosnoFTP.Add(item);
                        }
                    }



                }

             }
            BaixaArquivo();

        }

        public bool FtpDirectoryExists(string directoryPath, string ftpUser, string ftpPassword)
        {
            bool IsExists = true;
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(directoryPath);
                request.Credentials = new NetworkCredential(ftpUser, ftpPassword);
                request.Method = WebRequestMethods.Ftp.PrintWorkingDirectory;

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            }
            catch //WebException ex)
            {
                IsExists = false;
            }
            return IsExists;
        }

        public void BaixaArquivo()
        {
            int TotalArquivos = ArquivosnoFTP.Count();


            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;


            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);

            Conexao.Open();

            DataTable tabelanomes = new DataTable();

            if (TotalArquivos != 0)
            {

                foreach (string item in ArquivosnoFTP)
                {
                    bool Proximo = false;
                    String Sql = "Select * from cadastro.nomearq where Arquivo = @arquivo ";
                    MySqlCommand Comando = new MySqlCommand(Sql, Conexao);
                    Comando.Parameters.AddWithValue("@arquivo", item);

                    tabelanomes.Clear();

                    MySqlDataAdapter objAdapter = new MySqlDataAdapter(Comando);
                    objAdapter.Fill(tabelanomes);

                    int linhasC = tabelanomes.Rows.Count;

                    //textBox1.Text = linhasC.ToString();

                    if (linhasC != 0)
                    {
                        
                        if (tabelanomes.Rows[0]["Dtok"].ToString() == "True")
                        {
                            Proximo = true;
                        }
                    }

                    if (Proximo == false)
                    {
                        //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        //Cria comunicação com o servidor
                        //definindo o arquivo para download
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(@"ftp://"+Properties.Settings.Default.IP+"/InternalStorage/data/" + item);
                        //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(@"ftp://" + Properties.Settings.Default.IP + "/usb1/"+item);
                        //Define que a ação vai ser de download
                        request.Method = WebRequestMethods.Ftp.DownloadFile;
                        //Credenciais para o login (usuario, senha)
                        request.Credentials = new NetworkCredential(Properties.Settings.Default.UserFTP,Properties.Settings.Default.SenhaFTP);
                        //modo passivo
                        request.UsePassive = true;
                        //dados binarios
                        request.UseBinary = true;
                        //setar o KeepAlive para true
                        request.KeepAlive = true;
                        request.ServicePoint.ConnectionLimit = 2000;

                        //criando o objeto FtpWebResponse
                        FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                        //Criando a Stream para ler o arquivo
                        Stream responseStream = response.GetResponseStream();

                        byte[] buffer = new byte[10048];

                        //Definir o local onde o arquivo será criado.
                        FileStream newFile = new FileStream(Properties.Settings.Default.PathCSV + item, FileMode.Create);
                        //Ler o arquivo de origem
                        int readCount = responseStream.Read(buffer, 0, buffer.Length);
                        int bytesReceived = 0;
                        int bytesconvertido = 0;

                        Statusbox.Text = "Baixando arquivo " + item;
                        Statusbox.Refresh();
                        pictureBox1.Refresh();
                        

                        while (readCount > 0)
                        {
                            //Escrever o arquivo
                            newFile.Write(buffer, 0, readCount);
                            readCount = responseStream.Read(buffer, 0, buffer.Length);
                            bytesReceived += readCount;
                            bytesconvertido = bytesReceived / 100;
                            Statusbox2.Text = bytesconvertido.ToString()+"Kb";
                            Statusbox2.Refresh();
                            pictureBox1.Refresh();
                        }
                        newFile.Close();
                        responseStream.Close();
                        response.Close();

                    }

                }
                Statusbox2.Text = "";
            }
            ArquivosnoFTP.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ObterInformacao();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool result = FtpDirectoryExists(@"ftp://"+"bratac2018.ddns.net/", "anonymous", "197575");
            //textBox3.Text = result.ToString();

           
        }

        private void Statusbox_TextChanged(object sender, EventArgs e)
        {

        }

        public void AtualizaBanco2_Central()
        {


            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            int UltTabela = 0;

            DataTable tabelanomes = new DataTable();

            uint TotalRegistros;

            pictureBox1.Refresh();

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);

            Conexao.Open();
            /*
            listView1.View = View.Details;
            listView1.Columns.Add("Arquivo", 160);
            listView1.Columns.Add("Status", 80);
            listView1.Columns.Add("Ultima Atualização", 160);
            listView1.Columns.Add("Detalhes", 160);
            */

            //Lendo diretorio de arquivos
            string[] arquivos = Directory.GetFiles(Properties.Settings.Default.PathCSV, "Relatorio_20*", SearchOption.AllDirectories)
                            .Where(s => s.EndsWith(".csv", StringComparison.OrdinalIgnoreCase)).ToArray();

            int numarq = arquivos.Length;
            Statusbox.Text = numarq.ToString();
            //progressBar1.Maximum = numarq - 1;
            //progressBar1.Minimum = 0;

            if (numarq == 0)
            {
                MessageBox.Show("Nenhum arquivo compatível no diretório FTP");
            }
            else //Inicio
            {
                for (int i = 0; i < numarq; i++)//Peso1
                {
                    //progressBar1.Value = i;
                    //progressBar1.Refresh();
                    int Limite = arquivos[i].LastIndexOf(".csv");
                    int Letra1 = arquivos[i].IndexOf("Relatorio_20");
                    pictureBox1.Refresh();
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
                            pictureBox1.Refresh();
                            //-------------------------------------------------------------------------------------------------
                            //----*******------*******-------*******----
                            // EXISTE CADASTRO NO BANCO DE FONTE (nomearq)
                            if (tabelanomes.Rows.Count != 0)//1
                            {
                                //Nao precisa fazer atualização
                                if (PesoArq.ToString() == tabelanomes.Rows[0]["Tamanho"].ToString())
                                {
                                    ListViewItem item = new ListViewItem(new[] { str1, "Atualizado", tabelanomes.Rows[0]["Ultimaban"].ToString(), "" });
                                    // listView1.Items.Add(item);
                                    // listView1.Refresh();

                                    int AnoUltimo = Convert.ToInt16(UltAtualiza.Year);
                                    int MesUltimo = Convert.ToInt16(UltAtualiza.Month);

                                    AttOK = false;
                                    pictureBox1.Refresh();

                                    DateTime timeStamp2 = DateTime.Now;

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


                                    pictureBox1.Refresh();
                                    //Atualiza Registro de fonte do Banco 
                                    String SSqlX1 = "Update cadastro.nomearq set ultimaarq = @ultima_arq, ultimaban=@ultima_ban, tamanho = @peso, Dtok = @atok  where codigo=@code";
                                    MySqlCommand XSComando1 = new MySqlCommand(SSqlX1, Conexao);
                                    XSComando1.Parameters.AddWithValue("@code", str4);
                                    XSComando1.Parameters.AddWithValue("@ultima_arq", UltAtualiza);
                                    XSComando1.Parameters.AddWithValue("@ultima_ban", timeStamp2);
                                    XSComando1.Parameters.AddWithValue("@peso", PesoArq);
                                    XSComando1.Parameters.AddWithValue("@atok", AttOK);
                                    XSComando1.ExecuteNonQuery();


                                    Statusbox.Text = str1 + " " + "Atualizado" + " " + tabelanomes.Rows[0]["Ultimaban"].ToString();
                                    Statusbox.Refresh();
                                    pictureBox1.Refresh();
                                }
                                //Existe cadastro mas não está atualizado - Irá procurar o que precisa ser atualizado cruzando informações
                                else//Atualiza1
                                {
                                    DateTime timeStamp = DateTime.Now;

                                    TotalRegistros = 0;
                                    /*
                                    ListViewItem item2 = new ListViewItem(new[] { str1, "Atualizando...", tabelanomes.Rows[0]["Ultimaban"].ToString(), "..." });
                                    listView1.Items.Add(item2);
                                    listView1.Refresh();
                                    */
                                    Statusbox.Text = str1 + " " + "Atualizando..." + " " + tabelanomes.Rows[0]["Ultimaban"].ToString();
                                    Statusbox.Refresh();
                                    pictureBox1.Refresh();

                                    String Atsql1 = @"Create Temporary Table cadastro.Tempat1(Dia varchar(10), Hora time, Nome varchar(30), Form1 int, Form2 int, Prod_1 int, Prod_2 int, Prod_3 int, Prod_4 int, Prod_5 int, Prod_6 int, Prod_7 int, Prod_8 int, Prod_9 int, Prod_10 int, 
                                                      Prod_11 int, Prod_12 int, Prod_13 int, Prod_14 int, Prod_15 int, Prod_16 int, Prod_17 int, Prod_18 int, Prod_19 int, Prod_20 int, 
                                                      Prod_21 int, Prod_22 int, Prod_23 int, Prod_24 int, Prod_25 int, Prod_26 int, Prod_27 int, Prod_28 int, Prod_29 int, Prod_30 int, 
                                                      Prod_31 int, Prod_32 int, Prod_33 int, Prod_34 int, Prod_35 int, Prod_36 int, Prod_37 int, Prod_38 int, Prod_39 int, Prod_40 int,
                                                      Prod_41 int,Prod_42 int,Prod_43 int,Prod_44 int,Prod_45 int,Prod_46 int,Prod_47 int,Prod_48 int,Prod_49 int,Prod_50 int,
                                                      Prod_51 int,Prod_52 int,Prod_53 int,Prod_54 int,Prod_55 int,Prod_56 int,Prod_57 int,Prod_58 int,Prod_59 int,Prod_60 int,
                                                      Prod_61 int,Prod_62 int,Prod_63 int,Prod_64 int,Prod_65 int)";
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

                                    //UltTabela = listView1.Items.Count;
                                    //UltTabela = UltTabela - 1;

                                    //Sessão A1 - Há dados para serem atualizados no arquivo csv
                                    if (LinhasInt3X != 0)//Sessão A1
                                    {
                                        String Atsql4 = "Create Temporary Table cadastro.ultimat1 select * from cadastro.relatorio where month(str_to_date(dia, '%d/%m/%Y')) = @data1 and year(str_to_date(dia, '%d/%m/%Y')) = @data2";
                                        MySqlCommand AtComando4 = new MySqlCommand(Atsql4, Conexao);
                                        AtComando4.Parameters.AddWithValue("@data1", str3);
                                        AtComando4.Parameters.AddWithValue("@data2", str2);
                                        AtComando4.ExecuteNonQuery();
                                        pictureBox1.Refresh();
                                        String Atsql4X = "Select Count(*) from cadastro.ultimat1";
                                        MySqlCommand Comando4X = new MySqlCommand(Atsql4X, Conexao);
                                        Object Linhas4X;
                                        Linhas4X = Comando4X.ExecuteScalar();
                                        uint LinhasInt4X;
                                        LinhasInt4X = Convert.ToUInt32(Linhas4X);

                                        String Atsql4X2 = "Drop table cadastro.ultimat1";
                                        MySqlCommand Comando4X2 = new MySqlCommand(Atsql4X2, Conexao);
                                        Comando4X2.ExecuteNonQuery();
                                        pictureBox1.Refresh();
                                        //Há dados para serem cruzados
                                        //EMULADOR DE DADOS POR MES E DIA -------
                                        if (LinhasInt4X != 0)//Sessão A2
                                        {

                                            pictureBox1.Refresh();
                                            for (int x = 1; x < 32; x++)//Sessão A3
                                            {
                                                int porcentagem;
                                                porcentagem = x * 100 / 31;
                                                //listView1.Items[UltTabela].SubItems[3].Text = "Lendo arquivo:" + porcentagem.ToString() + "%";
                                                //listView1.Refresh();

                                                Statusbox.Text = str1 + "Lendo arquivo:" + porcentagem.ToString() + "%";
                                                Statusbox.Refresh();
                                                pictureBox1.Refresh();
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
                                                    pictureBox1.Refresh();
                                                    String Emsql4 = @"Create Temporary Table cadastro.Tempos SELECT b.Dia,b.Hora,b.Nome,b.Form1,b.Form2,b.Prod_1,b.Prod_2,b.Prod_3,b.Prod_4,b.Prod_5,b.Prod_6,b.Prod_7,b.Prod_8,b.Prod_9,b.Prod_10,
                                                                    b.Prod_11,b.Prod_12,b.Prod_13,b.Prod_14,b.Prod_15,b.Prod_16,b.Prod_17,b.Prod_18,b.Prod_19,b.Prod_20,
                                                                    b.Prod_21,b.Prod_22,b.Prod_23,b.Prod_24,b.Prod_25,b.Prod_26,b.Prod_27,b.Prod_28,b.Prod_29,b.Prod_30,
                                                                    b.Prod_31,b.Prod_32,b.Prod_33,b.Prod_34,b.Prod_35,b.Prod_36,b.Prod_37,b.Prod_38,b.Prod_39,b.Prod_40, 
                                                                    b.Prod_41,b.Prod_42,b.Prod_43,b.Prod_44,b.Prod_45,b.Prod_46,b.Prod_47,b.Prod_48,b.Prod_49,b.Prod_50,
                                                                    b.Prod_51,b.Prod_52,b.Prod_53,b.Prod_54,b.Prod_55,b.Prod_56,b.Prod_57,b.Prod_58,b.Prod_59,b.Prod_60,
                                                                    b.Prod_61,b.Prod_62,b.Prod_63,b.Prod_64,b.Prod_65
                                                                    FROM cadastro.nova1 as b LEFT JOIN cadastro.recorte1 as a on b.Dia = a.Dia and b.Hora = a.Hora WHERE a.Dia is null";
                                                    MySqlCommand Ecomando4 = new MySqlCommand(Emsql4, Conexao);
                                                    Ecomando4.ExecuteNonQuery();

                                                    String Emsql5 = "Select Count(*) from cadastro.Tempos";
                                                    MySqlCommand Ecomando5 = new MySqlCommand(Emsql5, Conexao);
                                                    Object Elinhas5;
                                                    Elinhas5 = Ecomando5.ExecuteScalar();
                                                    uint ELinhasInt5;
                                                    ELinhasInt5 = Convert.ToUInt16(Elinhas5);
                                                    pictureBox1.Refresh();
                                                    //Coloca os dados no relatorio
                                                    if (ELinhasInt5 != 0)//Sessão A5 
                                                    {
                                                        TotalRegistros = TotalRegistros + ELinhasInt5;
                                                        String Emsql6 = "Insert into cadastro.relatorio select * from cadastro.tempos";
                                                        MySqlCommand Ecomando6 = new MySqlCommand(Emsql6, Conexao);
                                                        Ecomando6.ExecuteNonQuery();
                                                    }
                                                    pictureBox1.Refresh();
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
                                                pictureBox1.Refresh();
                                            } // SESSÂO 3 - Fim do Loop de Dia 

                                            /*
                                            ListViewItem item4 = new ListViewItem(new[] { str1, "Atualizado", timeStamp.ToString(), TotalRegistros + " Novos Registros" });
                                            listView1.Items.Add(item4);
                                            listView1.Refresh();
                                            */

                                            Statusbox.Text = str1 + " " + "Atualizado" + " " + timeStamp.ToString() + " " + TotalRegistros + " Novos Registros";
                                            Statusbox.Refresh();
                                            Statusbox2.Text = TotalRegistros + " Novos Registros";
                                            Statusbox2.Refresh();
                                            pictureBox1.Refresh();
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
                                            

                                            ListViewItem item4 = new ListViewItem(new[] { str1, "Atualizado", timeStamp.ToString(), LinhasInt3X + " Novos Registros" });
                                            listView1.Items.Add(item4);
                                            listView1.Refresh();
                                            */

                                            Statusbox.Text = str1 + " " + "Atualizado" + " " + timeStamp.ToString();
                                            Statusbox.Refresh();
                                            Statusbox2.Text = LinhasInt3X + " Novos Registros";
                                            Statusbox2.Refresh();
                                            pictureBox1.Refresh();
                                        }
                                    }
                                    else
                                    {
                                        /*
                                        ListViewItem item5 = new ListViewItem(new[] { str1, "Atualizado", LinhasInt3X + "Nenhum registro novo" });
                                        listView1.Items.Add(item5);
                                        listView1.Refresh();
                                        */
                                        Statusbox.Text = str1 + " " + "Atualizado";
                                        Statusbox.Refresh();
                                        Statusbox2.Text = LinhasInt3X + "registros";
                                        Statusbox2.Refresh();
                                        pictureBox1.Refresh();
                                    }

                                    String Asql1 = "drop table cadastro.tempat1";
                                    MySqlCommand A0Comando1 = new MySqlCommand(Asql1, Conexao);
                                    A0Comando1.ExecuteNonQuery();

                                    //DateTime timeStamp = DateTime.Now;

                                    int AnoUltimo = Convert.ToInt16(UltAtualiza.Year);
                                    int MesUltimo = Convert.ToInt16(UltAtualiza.Month);

                                    AttOK = false;
                                    pictureBox1.Refresh();

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


                                    pictureBox1.Refresh();
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
                                /*
                                ListViewItem item2 = new ListViewItem(new[] { str1, "Atualizando...", "...", "..." });
                                listView1.Items.Add(item2);
                                listView1.Refresh();
                                */
                                pictureBox1.Refresh();
                                Statusbox.Text = str1 + " " + "Atualizado";
                                Statusbox.Refresh();

                                String Atsql1 = @"Create Temporary Table cadastro.Tempat1(Dia varchar(10), Hora time, Nome varchar(30), Form1 int, Form2 int, Prod_1 int, Prod_2 int, Prod_3 int, Prod_4 int, Prod_5 int, Prod_6 int, Prod_7 int, Prod_8 int, Prod_9 int, Prod_10 int, 
                                                Prod_11 int, Prod_12 int, Prod_13 int, Prod_14 int, Prod_15 int, Prod_16 int, Prod_17 int, Prod_18 int, Prod_19 int, Prod_20 int, 
                                                Prod_21 int, Prod_22 int, Prod_23 int, Prod_24 int, Prod_25 int, Prod_26 int, Prod_27 int, Prod_28 int, Prod_29 int, Prod_30 int, 
                                                Prod_31 int, Prod_32 int, Prod_33 int, Prod_34 int, Prod_35 int, Prod_36 int, Prod_37 int, Prod_38 int, Prod_39 int, Prod_40 int,
                                                Prod_41 int,Prod_42 int,Prod_43 int,Prod_44 int,Prod_45 int,Prod_46 int,Prod_47 int,Prod_48 int,Prod_49 int,Prod_50 int,
                                                Prod_51 int,Prod_52 int,Prod_53 int,Prod_54 int,Prod_55 int,Prod_56 int,Prod_57 int,Prod_58 int,Prod_59 int,Prod_60 int,
                                                Prod_61 int,Prod_62 int,Prod_63 int,Prod_64 int,Prod_65 int)";

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
                                /*
                                UltTabela = listView1.Items.Count;
                                UltTabela = UltTabela - 1;
                                */
                                //Sessão A1 - Há dados para serem atualizados no arquivo csv
                                if (LinhasInt3X != 0)//Sessão A1
                                {
                                    String Atsql4 = "Create Temporary Table cadastro.ultimat1 select * from cadastro.relatorio where month(str_to_date(dia, '%d/%m/%Y')) = @data1 and year(str_to_date(dia, '%d/%m/%Y')) = @data2";
                                    MySqlCommand AtComando4 = new MySqlCommand(Atsql4, Conexao);
                                    AtComando4.Parameters.AddWithValue("@data1", str3);
                                    AtComando4.Parameters.AddWithValue("@data2", str2);
                                    AtComando4.ExecuteNonQuery();
                                    pictureBox1.Refresh();
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

                                        pictureBox1.Refresh();
                                        for (int x = 1; x < 32; x++)//Sessão A3
                                        {
                                            int porcentagem;
                                            porcentagem = x * 100 / 31;
                                            /*
                                            listView1.Items[UltTabela].SubItems[3].Text = "Lendo arquivo:" + porcentagem.ToString() + "%";
                                            listView1.Refresh();
                                            */
                                            Statusbox.Text = str1 + " " + "Atualizando" + " " + "Lendo arquivo:" + porcentagem.ToString() + "%";
                                            Statusbox.Refresh();
                                            Statusbox2.Text = "Lendo arquivo:" + porcentagem.ToString() + "%";
                                            Statusbox2.Refresh();
                                            pictureBox1.Refresh();
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
                                                pictureBox1.Refresh();
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
                                                String Emsql4 = @"Create Temporary Table cadastro.Tempos SELECT b.Dia,b.Hora,b.Nome,b.Form1,b.Form2,b.Prod_1,b.Prod_2,b.Prod_3,b.Prod_4,b.Prod_5,b.Prod_6,b.Prod_7,b.Prod_8,b.Prod_9,b.Prod_10,
                                                                 b.Prod_11,b.Prod_12,b.Prod_13,b.Prod_14,b.Prod_15,b.Prod_16,b.Prod_17,b.Prod_18,b.Prod_19,b.Prod_20,
                                                                 b.Prod_21,b.Prod_22,b.Prod_23,b.Prod_24,b.Prod_25,b.Prod_26,b.Prod_27,b.Prod_28,b.Prod_29,b.Prod_30,
                                                                 b.Prod_31,b.Prod_32,b.Prod_33,b.Prod_34,b.Prod_35,b.Prod_36,b.Prod_37,b.Prod_38,b.Prod_39,b.Prod_40,
                                                                 b.Prod_41,b.Prod_42,b.Prod_43,b.Prod_44,b.Prod_45,b.Prod_46,b.Prod_47,b.Prod_48,b.Prod_49,b.Prod_50,
                                                                 b.Prod_51,b.Prod_52,b.Prod_53,b.Prod_54,b.Prod_55,b.Prod_56,b.Prod_57,b.Prod_58,b.Prod_59,b.Prod_60,
                                                                 b.Prod_61,b.Prod_62,b.Prod_63,b.Prod_64,b.Prod_65 
                                                                 FROM cadastro.nova1 as b LEFT JOIN cadastro.recorte1 as a on b.Dia = a.Dia and b.Hora = a.Hora WHERE a.Dia is null";
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
                                                pictureBox1.Refresh();
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
                                        /*
                                        listView1.Items.Add(item4);
                                        listView1.Refresh();
                                        */
                                        Statusbox.Text = str1 + " " + "Atualizado" + " " + timeStamp.ToString() + " " + TotalRegistros + " Novos Registros";
                                        Statusbox.Refresh();
                                        Statusbox2.Text = TotalRegistros + " Novos Registros";
                                        Statusbox2.Refresh();
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

                                        ListViewItem item4 = new ListViewItem(new[] { str1, "Atualizado", timeStamp.ToString(), LinhasInt3X + " Novos Registros" });
                                        /*
                                        listView1.Items.Add(item4);
                                        listView1.Refresh();
                                        */
                                        Statusbox.Text = str1 + " " + "Atualizado" + " " + timeStamp.ToString();
                                        Statusbox.Refresh();
                                        Statusbox2.Text = LinhasInt3X + " Novos Registros";
                                        Statusbox2.Refresh();

                                        pictureBox1.Refresh();
                                    }
                                }
                                else
                                {
                                    /*
                                    ListViewItem item5 = new ListViewItem(new[] { str1, "Atualizado", timeStamp.ToString(), LinhasInt3X + "Nenhum registro novo" });
                                    listView1.Items.Add(item5);
                                    listView1.Refresh();
                                    */
                                    Statusbox.Text = str1 + " " + "Atualizado" + " " + timeStamp.ToString();
                                    Statusbox.Refresh();
                                    Statusbox2.Text = LinhasInt3X + "registros";
                                    Statusbox2.Refresh();

                                    pictureBox1.Refresh();
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

                                pictureBox1.Refresh();

                                
                            }
                        } //Fim Atualização com cadastro no banco
                    }
                }
               

            }

            Conexao.Close();
            Statusbox.Text = "Atualizado com sucesso!";
            Statusbox2.Text = "";
            pictureBox1.Refresh();
            InitializeTimer2();

        }

        public void AtualizaBanco_Unico_Central()
        {


            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            int UltTabela = 0;

            DataTable tabelanomes = new DataTable();

            uint TotalRegistros;

            pictureBox1.Refresh();

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);

            Conexao.Open();
            /*
            listView1.View = View.Details;
            listView1.Columns.Add("Arquivo", 160);
            listView1.Columns.Add("Status", 80);
            listView1.Columns.Add("Ultima Atualização", 160);
            listView1.Columns.Add("Detalhes", 160);
            */

            //Lendo diretorio de arquivos
            string[] arquivos = Directory.GetFiles(Properties.Settings.Default.PathCSV, "Relatorio_1*", SearchOption.AllDirectories)
                            .Where(s => s.EndsWith(".csv", StringComparison.OrdinalIgnoreCase)).ToArray();

            int numarq = arquivos.Length;
            Statusbox.Text = numarq.ToString();
            //progressBar1.Maximum = numarq - 1;
            //progressBar1.Minimum = 0;

            if (numarq == 0)
            {
                MessageBox.Show("Nenhum arquivo compatível no diretório FTP");
            }
            else //Inicio
            {
                

                            FileInfo fi2 = new FileInfo(arquivos[0]);
                            DateTime UltAtualiza = fi2.LastAccessTime;
                            long PesoArq = fi2.Length;
                            bool AttOK = false;
                            

                            String Sql = "Select * from cadastro.nomearq where Codigo = @code ";
                            MySqlCommand Comando = new MySqlCommand(Sql, Conexao);
                            Comando.Parameters.AddWithValue("@code", 10);

                            tabelanomes.Clear();

                            MySqlDataAdapter objAdapter = new MySqlDataAdapter(Comando);
                            objAdapter.Fill(tabelanomes);
                            pictureBox1.Refresh();
                //-------------------------------------------------------------------------------------------------
                //----*******------*******-------*******----
                // EXISTE CADASTRO NO BANCO DE FONTE (nomearq)
                if (tabelanomes.Rows.Count != 0)//1
                {
                    //Nao precisa fazer atualização
                    if (PesoArq.ToString() == tabelanomes.Rows[0]["Tamanho"].ToString())
                    {
                        ListViewItem item = new ListViewItem(new[] { str1, "Atualizado", tabelanomes.Rows[0]["Ultimaban"].ToString(), "" });
                        // listView1.Items.Add(item);
                        // listView1.Refresh();

                        int AnoUltimo = Convert.ToInt16(UltAtualiza.Year);
                        int MesUltimo = Convert.ToInt16(UltAtualiza.Month);

                        AttOK = false;
                        pictureBox1.Refresh();

                        DateTime timeStamp2 = DateTime.Now;


                        pictureBox1.Refresh();
                        //Atualiza Registro de fonte do Banco 
                        String SSqlX1 = "Update cadastro.nomearq set ultimaarq = @ultima_arq, ultimaban=@ultima_ban, tamanho = @peso, Dtok = @atok  where codigo=@code";
                        MySqlCommand XSComando1 = new MySqlCommand(SSqlX1, Conexao);
                        XSComando1.Parameters.AddWithValue("@code", 10);
                        XSComando1.Parameters.AddWithValue("@ultima_arq", UltAtualiza);
                        XSComando1.Parameters.AddWithValue("@ultima_ban", timeStamp2);
                        XSComando1.Parameters.AddWithValue("@peso", PesoArq);
                        XSComando1.Parameters.AddWithValue("@atok", AttOK);
                        XSComando1.ExecuteNonQuery();


                        Statusbox.Text = str1 + " " + "Atualizado" + " " + tabelanomes.Rows[0]["Ultimaban"].ToString();
                        Statusbox.Refresh();
                        pictureBox1.Refresh();
                    }
                    //Existe cadastro mas não está atualizado - Irá procurar o que precisa ser atualizado cruzando informações
                    else//Atualiza1
                    {
                        DateTime timeStamp = DateTime.Now;

                        TotalRegistros = 0;
                        /*
                        ListViewItem item2 = new ListViewItem(new[] { str1, "Atualizando...", tabelanomes.Rows[0]["Ultimaban"].ToString(), "..." });
                        listView1.Items.Add(item2);
                        listView1.Refresh();
                        */
                        Statusbox.Text = str1 + " " + "Atualizando...";
                        Statusbox.Refresh();
                        pictureBox1.Refresh();





                        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

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
                            String SSql1 = @"Create Temporary Table Temp (Dia varchar(10),Hora time,Nome varchar(30),Form1 int,Form2 int,Prod_1 int,Prod_2 int,Prod_3 int,Prod_4 int,Prod_5 int,Prod_6 int,Prod_7 int,Prod_8 int,Prod_9 int,Prod_10 int,
                                                          Prod_11 int,Prod_12 int,Prod_13 int,Prod_14 int,Prod_15 int,Prod_16 int,Prod_17 int,Prod_18 int,Prod_19 int,Prod_20 int,
                                                          Prod_21 int,Prod_22 int,Prod_23 int,Prod_24 int,Prod_25 int,Prod_26 int,Prod_27 int,Prod_28 int,Prod_29 int,Prod_30 int,
                                                          Prod_31 int,Prod_32 int,Prod_33 int,Prod_34 int,Prod_35 int,Prod_36 int,Prod_37 int, Prod_38 int,Prod_39 int,Prod_40 int,
                                                          Prod_41 int,Prod_42 int,Prod_43 int,Prod_44 int,Prod_45 int,Prod_46 int,Prod_47 int,Prod_48 int,Prod_49 int,Prod_50 int,
                                                          Prod_51 int,Prod_52 int,Prod_53 int,Prod_54 int,Prod_55 int,Prod_56 int,Prod_57 int,Prod_58 int,Prod_59 int,Prod_60 int,
                                                          Prod_61 int,Prod_62 int,Prod_63 int,Prod_64 int,Prod_65 int)";


                            MySqlCommand SComando1 = new MySqlCommand(SSql1, Conexao);
                            SComando1.ExecuteNonQuery();

                            String ArquivoCSV = arquivos[0].Replace("\\", "/");
                            String Spath = "'" + ArquivoCSV + "'";

                            String SSql2 = "LOAD DATA LOCAL INFILE " + Spath + " INTO TABLE cadastro.Temp FIELDS TERMINATED BY ',' LINES TERMINATED BY '\\r\\n' ";
                            MySqlCommand SComando2 = new MySqlCommand(SSql2, Conexao);
                            SComando2.ExecuteNonQuery();

                            //Conta quantos valores novos existem
                            String SSql6 = "Select Count(*) from cadastro.Temp";
                            MySqlCommand SComando6 = new MySqlCommand(SSql6, Conexao);
                            Object Slinhas;
                            Slinhas = SComando6.ExecuteScalar();
                            uint SLinhasInt;
                            SLinhasInt = Convert.ToUInt32(Slinhas);



                            if (SLinhasInt != 0)
                            {
                                String SMensagem1 = "Foram adicionados " + Slinhas.ToString() + " novos registros.";
                                String SSql4 = "Insert into relatorio select * from cadastro.Temp";
                                MySqlCommand SComando4 = new MySqlCommand(SSql4, Conexao);
                                SComando4.ExecuteNonQuery();

                                //MessageBox.Show(SMensagem1);

                                Statusbox.Text = SMensagem1;
                                Statusbox.Refresh();
                                pictureBox1.Refresh();

                                String SSqlDrop2_1 = "Drop Table cadastro.Temp";
                                MySqlCommand SComandoD2_1 = new MySqlCommand(SSqlDrop2_1, Conexao);
                                SComandoD2_1.ExecuteNonQuery();
                                
                            }
                            else
                            {
                                String SSqlDrop2 = "Drop Table cadastro.Temp";
                                MySqlCommand SComandoD2 = new MySqlCommand(SSqlDrop2, Conexao);
                                SComandoD2.ExecuteNonQuery();

                                

                                Statusbox.Text = "Nenhum registro novo!";
                                Statusbox.Refresh();
                                pictureBox1.Refresh();
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
                            String Sql1 = @"Create Temporary Table Temp (Dia varchar(10),Hora time,Nome varchar(30),Form1 int,Form2 int,Prod_1 int,Prod_2 int,Prod_3 int,Prod_4 int,Prod_5 int,Prod_6 int,Prod_7 int,Prod_8 int,Prod_9 int,Prod_10 int,
                                                          Prod_11 int,Prod_12 int,Prod_13 int,Prod_14 int,Prod_15 int,Prod_16 int,Prod_17 int,Prod_18 int,Prod_19 int,Prod_20 int,
                                                          Prod_21 int,Prod_22 int,Prod_23 int,Prod_24 int,Prod_25 int,Prod_26 int,Prod_27 int,Prod_28 int,Prod_29 int,Prod_30 int,
                                                          Prod_31 int,Prod_32 int,Prod_33 int,Prod_34 int,Prod_35 int,Prod_36 int,Prod_37 int,Prod_38 int,Prod_39 int,Prod_40 int,
                                                          Prod_41 int,Prod_42 int,Prod_43 int,Prod_44 int,Prod_45 int,Prod_46 int,Prod_47 int,Prod_48 int,Prod_49 int,Prod_50 int,
                                                          Prod_51 int,Prod_52 int,Prod_53 int,Prod_54 int,Prod_55 int,Prod_56 int,Prod_57 int,Prod_58 int,Prod_59 int,Prod_60 int,
                                                          Prod_61 int,Prod_62 int,Prod_63 int,Prod_64 int,Prod_65 int)";

                            MySqlCommand Comando1 = new MySqlCommand(Sql1, Conexao);
                            Comando1.ExecuteNonQuery();

                            String ArquivoCSV = arquivos[0].Replace("\\", "/");
                            String Spath = "'" + ArquivoCSV + "'";

                            String Sql2 = "LOAD DATA LOCAL INFILE " + Spath + " INTO TABLE cadastro.Temp FIELDS TERMINATED BY ',' LINES TERMINATED BY '\\r\\n' ";
                            MySqlCommand Comando2 = new MySqlCommand(Sql2, Conexao);
                            Comando2.ExecuteNonQuery();

                            //Deleta da Tabela Temporaria Temp valores menores do que da ultima data do relatorio principal
                            String SqlDel = "Delete from cadastro.Temp where str_to_date(Dia,'%d/%m/%Y') < str_to_date(@datadel,'%d/%m/%Y')";
                            MySqlCommand ComandoDel = new MySqlCommand(SqlDel, Conexao);
                            ComandoDel.Parameters.AddWithValue("@datadel", DataConvert.ToString("d/M/y"));
                            ComandoDel.ExecuteNonQuery();

                            //Cria uma tabela ("Tempos") a partir de um SELECT que cruza os dados (LEFT JOIN) entre tabela temporaria "Temp" e "Ultima"
                            String Sql3 = @"Create Temporary Table Tempos SELECT b.Dia,b.Hora,b.Nome,b.Form1,b.Form2,b.Prod_1,b.Prod_2,b.Prod_3,b.Prod_4,b.Prod_5,b.Prod_6,b.Prod_7,b.Prod_8,b.Prod_9,b.Prod_10,
                                                          b.Prod_11,b.Prod_12,b.Prod_13,b.Prod_14,b.Prod_15,b.Prod_16,b.Prod_17,b.Prod_18,b.Prod_19,b.Prod_20,
                                                          b.Prod_21,b.Prod_22,b.Prod_23,b.Prod_24,b.Prod_25,b.Prod_26,b.Prod_27,b.Prod_28,b.Prod_29,b.Prod_30,
                                                          b.Prod_31,b.Prod_32,b.Prod_33,b.Prod_34,b.Prod_35,b.Prod_36,b.Prod_37,b.Prod_38,b.Prod_39,b.Prod_40,
                                                          b.Prod_41,b.Prod_42,b.Prod_43,b.Prod_44,b.Prod_45,b.Prod_46,b.Prod_47,b.Prod_48,b.Prod_49,b.Prod_50,
                                                          b.Prod_51,b.Prod_52,b.Prod_53,b.Prod_54,b.Prod_55,b.Prod_56,b.Prod_57,b.Prod_58,b.Prod_59,b.Prod_60,
                                                          b.Prod_61,b.Prod_62,b.Prod_63,b.Prod_64,b.Prod_65 
                                                          FROM cadastro.Temp as b LEFT JOIN cadastro.ultima as a on b.Dia = a.Dia and b.Hora = a.Hora WHERE a.Dia is null";
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
                            uint LinhasInt;
                            LinhasInt = Convert.ToUInt32(linhas);

                            if (LinhasInt != 0)
                            {


                                String Mensagem1 = "Foram adicionados " + linhas.ToString() + " novos registros.";
                                String Sql4 = "Insert into relatorio select * from cadastro.Tempos";
                                MySqlCommand Comando4 = new MySqlCommand(Sql4, Conexao);
                                Comando4.ExecuteNonQuery();

                                Statusbox.Text = Mensagem1;
                                Statusbox.Refresh();
                                pictureBox1.Refresh();

                                String SqlDrop1V = "Drop Table cadastro.Tempos";
                                MySqlCommand ComandoD1V = new MySqlCommand(SqlDrop1V, Conexao);
                                ComandoD1V.ExecuteNonQuery();

                                String SqlDrop2V = "Drop Table cadastro.Temp";
                                MySqlCommand ComandoD2V = new MySqlCommand(SqlDrop2V, Conexao);
                                ComandoD2V.ExecuteNonQuery();

                                String SqlDrop3V = "Drop Table cadastro.Ultima";
                                MySqlCommand ComandoD3V = new MySqlCommand(SqlDrop3V, Conexao);
                                ComandoD3V.ExecuteNonQuery();

                               


                            }
                            else
                            {
                                Statusbox.Text = "Nenhum registro novo.";
                                Statusbox.Refresh();
                                pictureBox1.Refresh();


                                String SqlDrop1 = "Drop Table cadastro.Tempos";
                                MySqlCommand ComandoD1 = new MySqlCommand(SqlDrop1, Conexao);
                                ComandoD1.ExecuteNonQuery();

                                String SqlDrop2 = "Drop Table cadastro.Temp";
                                MySqlCommand ComandoD2 = new MySqlCommand(SqlDrop2, Conexao);
                                ComandoD2.ExecuteNonQuery();

                                String SqlDrop3 = "Drop Table cadastro.Ultima";
                                MySqlCommand ComandoD3 = new MySqlCommand(SqlDrop3, Conexao);
                                ComandoD3.ExecuteNonQuery();

                                
                            }

                        }


                        //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------                            


                        Statusbox2.Refresh();
                        pictureBox1.Refresh();


                        int AnoUltimo = Convert.ToInt16(UltAtualiza.Year);
                        int MesUltimo = Convert.ToInt16(UltAtualiza.Month);

                        AttOK = false;
                        pictureBox1.Refresh();




                        pictureBox1.Refresh();
                        //Atualiza Registro de fonte do Banco 
                        String SSqlX1 = "Update cadastro.nomearq set ultimaarq = @ultima_arq, ultimaban=@ultima_ban, tamanho = @peso, Dtok = @atok  where codigo=@code";
                        MySqlCommand XSComando1 = new MySqlCommand(SSqlX1, Conexao);
                        XSComando1.Parameters.AddWithValue("@code", 10);
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

                    Statusbox.Text = str1 + " " + "Atualizando...";
                    Statusbox.Refresh();
                    pictureBox1.Refresh();

                    //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

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
                        String SSql1 = @"Create Temporary Table Temp (Dia varchar(10),Hora time,Nome varchar(30),Form1 int,Form2 int,Prod_1 int,Prod_2 int,Prod_3 int,Prod_4 int,Prod_5 int,Prod_6 int,Prod_7 int,Prod_8 int,Prod_9 int,Prod_10 int,
                                                          Prod_11 int,Prod_12 int,Prod_13 int,Prod_14 int,Prod_15 int,Prod_16 int,Prod_17 int,Prod_18 int,Prod_19 int,Prod_20 int,
                                                          Prod_21 int,Prod_22 int,Prod_23 int,Prod_24 int,Prod_25 int,Prod_26 int,Prod_27 int,Prod_28 int,Prod_29 int,Prod_30 int,
                                                          Prod_31 int,Prod_32 int,Prod_33 int,Prod_34 int,Prod_35 int,Prod_36 int,Prod_37 int, Prod_38 int,Prod_39 int,Prod_40 int,
                                                          Prod_41 int,Prod_42 int,Prod_43 int,Prod_44 int,Prod_45 int,Prod_46 int,Prod_47 int,Prod_48 int,Prod_49 int,Prod_50 int,
                                                          Prod_51 int,Prod_52 int,Prod_53 int,Prod_54 int,Prod_55 int,Prod_56 int,Prod_57 int,Prod_58 int,Prod_59 int,Prod_60 int,
                                                          Prod_61 int,Prod_62 int,Prod_63 int,Prod_64 int,Prod_65 int)";


                        MySqlCommand SComando1 = new MySqlCommand(SSql1, Conexao);
                        SComando1.ExecuteNonQuery();

                        String ArquivoCSV = arquivos[0].Replace("\\", "/");
                        String Spath = "'" + ArquivoCSV + "'";

                        String SSql2 = "LOAD DATA LOCAL INFILE " + Spath + " INTO TABLE cadastro.Temp FIELDS TERMINATED BY ',' LINES TERMINATED BY '\\r\\n' ";
                        MySqlCommand SComando2 = new MySqlCommand(SSql2, Conexao);
                        SComando2.ExecuteNonQuery();

                        //Conta quantos valores novos existem
                        String SSql6 = "Select Count(*) from cadastro.Temp";
                        MySqlCommand SComando6 = new MySqlCommand(SSql6, Conexao);
                        Object Slinhas;
                        Slinhas = SComando6.ExecuteScalar();
                        uint SLinhasInt;
                        SLinhasInt = Convert.ToUInt32(Slinhas);



                        if (SLinhasInt != 0)
                        {
                            String SMensagem1 = "Foram adicionados " + Slinhas.ToString() + " novos registros.";
                            String SSql4 = "Insert into relatorio select * from cadastro.Temp";
                            MySqlCommand SComando4 = new MySqlCommand(SSql4, Conexao);
                            SComando4.ExecuteNonQuery();

                            //MessageBox.Show(SMensagem1);

                            Statusbox.Text = SMensagem1;
                            Statusbox.Refresh();
                            pictureBox1.Refresh();

                            String SSqlDrop2_1 = "Drop Table cadastro.Temp";
                            MySqlCommand SComandoD2_1 = new MySqlCommand(SSqlDrop2_1, Conexao);
                            SComandoD2_1.ExecuteNonQuery();

                        }
                        else
                        {
                            String SSqlDrop2 = "Drop Table cadastro.Temp";
                            MySqlCommand SComandoD2 = new MySqlCommand(SSqlDrop2, Conexao);
                            SComandoD2.ExecuteNonQuery();



                            Statusbox.Text = "Nenhum registro novo!";
                            Statusbox.Refresh();
                            pictureBox1.Refresh();
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
                        String Sql1 = @"Create Temporary Table Temp (Dia varchar(10),Hora time,Nome varchar(30),Form1 int,Form2 int,Prod_1 int,Prod_2 int,Prod_3 int,Prod_4 int,Prod_5 int,Prod_6 int,Prod_7 int,Prod_8 int,Prod_9 int,Prod_10 int,
                                                          Prod_11 int,Prod_12 int,Prod_13 int,Prod_14 int,Prod_15 int,Prod_16 int,Prod_17 int,Prod_18 int,Prod_19 int,Prod_20 int,
                                                          Prod_21 int,Prod_22 int,Prod_23 int,Prod_24 int,Prod_25 int,Prod_26 int,Prod_27 int,Prod_28 int,Prod_29 int,Prod_30 int,
                                                          Prod_31 int,Prod_32 int,Prod_33 int,Prod_34 int,Prod_35 int,Prod_36 int,Prod_37 int,Prod_38 int,Prod_39 int,Prod_40 int,
                                                          Prod_41 int,Prod_42 int,Prod_43 int,Prod_44 int,Prod_45 int,Prod_46 int,Prod_47 int,Prod_48 int,Prod_49 int,Prod_50 int,
                                                          Prod_51 int,Prod_52 int,Prod_53 int,Prod_54 int,Prod_55 int,Prod_56 int,Prod_57 int,Prod_58 int,Prod_59 int,Prod_60 int,
                                                          Prod_61 int,Prod_62 int,Prod_63 int,Prod_64 int,Prod_65 int)";

                        MySqlCommand Comando1 = new MySqlCommand(Sql1, Conexao);
                        Comando1.ExecuteNonQuery();

                        String ArquivoCSV = arquivos[0].Replace("\\", "/");
                        String Spath = "'" + ArquivoCSV + "'";

                        String Sql2 = "LOAD DATA LOCAL INFILE " + Spath + " INTO TABLE cadastro.Temp FIELDS TERMINATED BY ',' LINES TERMINATED BY '\\r\\n' ";
                        MySqlCommand Comando2 = new MySqlCommand(Sql2, Conexao);
                        Comando2.ExecuteNonQuery();

                        //Deleta da Tabela Temporaria Temp valores menores do que da ultima data do relatorio principal
                        String SqlDel = "Delete from cadastro.Temp where str_to_date(Dia,'%d/%m/%Y') < str_to_date(@datadel,'%d/%m/%Y')";
                        MySqlCommand ComandoDel = new MySqlCommand(SqlDel, Conexao);
                        ComandoDel.Parameters.AddWithValue("@datadel", DataConvert.ToString("d/M/y"));
                        ComandoDel.ExecuteNonQuery();

                        //Cria uma tabela ("Tempos") a partir de um SELECT que cruza os dados (LEFT JOIN) entre tabela temporaria "Temp" e "Ultima"
                        String Sql3 = @"Create Temporary Table Tempos SELECT b.Dia,b.Hora,b.Nome,b.Form1,b.Form2,b.Prod_1,b.Prod_2,b.Prod_3,b.Prod_4,b.Prod_5,b.Prod_6,b.Prod_7,b.Prod_8,b.Prod_9,b.Prod_10,
                                                          b.Prod_11,b.Prod_12,b.Prod_13,b.Prod_14,b.Prod_15,b.Prod_16,b.Prod_17,b.Prod_18,b.Prod_19,b.Prod_20,
                                                          b.Prod_21,b.Prod_22,b.Prod_23,b.Prod_24,b.Prod_25,b.Prod_26,b.Prod_27,b.Prod_28,b.Prod_29,b.Prod_30,
                                                          b.Prod_31,b.Prod_32,b.Prod_33,b.Prod_34,b.Prod_35,b.Prod_36,b.Prod_37,b.Prod_38,b.Prod_39,b.Prod_40,
                                                          b.Prod_41,b.Prod_42,b.Prod_43,b.Prod_44,b.Prod_45,b.Prod_46,b.Prod_47,b.Prod_48,b.Prod_49,b.Prod_50,
                                                          b.Prod_51,b.Prod_52,b.Prod_53,b.Prod_54,b.Prod_55,b.Prod_56,b.Prod_57,b.Prod_58,b.Prod_59,b.Prod_60,
                                                          b.Prod_61,b.Prod_62,b.Prod_63,b.Prod_64,b.Prod_65 
                                                          FROM cadastro.Temp as b LEFT JOIN cadastro.ultima as a on b.Dia = a.Dia and b.Hora = a.Hora WHERE a.Dia is null";
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
                        uint LinhasInt;
                        LinhasInt = Convert.ToUInt32(linhas);

                        if (LinhasInt != 0)
                        {


                            String Mensagem1 = "Foram adicionados " + linhas.ToString() + " novos registros.";
                            String Sql4 = "Insert into relatorio select * from cadastro.Tempos";
                            MySqlCommand Comando4 = new MySqlCommand(Sql4, Conexao);
                            Comando4.ExecuteNonQuery();

                            Statusbox.Text = Mensagem1;
                            Statusbox.Refresh();
                            pictureBox1.Refresh();

                            String SqlDrop1V = "Drop Table cadastro.Tempos";
                            MySqlCommand ComandoD1V = new MySqlCommand(SqlDrop1V, Conexao);
                            ComandoD1V.ExecuteNonQuery();

                            String SqlDrop2V = "Drop Table cadastro.Temp";
                            MySqlCommand ComandoD2V = new MySqlCommand(SqlDrop2V, Conexao);
                            ComandoD2V.ExecuteNonQuery();

                            String SqlDrop3V = "Drop Table cadastro.Ultima";
                            MySqlCommand ComandoD3V = new MySqlCommand(SqlDrop3V, Conexao);
                            ComandoD3V.ExecuteNonQuery();




                        }
                        else
                        {
                            Statusbox.Text = "Nenhum registro novo.";
                            Statusbox.Refresh();
                            pictureBox1.Refresh();


                            String SqlDrop1 = "Drop Table cadastro.Tempos";
                            MySqlCommand ComandoD1 = new MySqlCommand(SqlDrop1, Conexao);
                            ComandoD1.ExecuteNonQuery();

                            String SqlDrop2 = "Drop Table cadastro.Temp";
                            MySqlCommand ComandoD2 = new MySqlCommand(SqlDrop2, Conexao);
                            ComandoD2.ExecuteNonQuery();

                            String SqlDrop3 = "Drop Table cadastro.Ultima";
                            MySqlCommand ComandoD3 = new MySqlCommand(SqlDrop3, Conexao);
                            ComandoD3.ExecuteNonQuery();


                        }

                    }


                    //--------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------                            

                    pictureBox1.Refresh();
                    Statusbox.Refresh();


                    int AnoUltimo = Convert.ToInt16(UltAtualiza.Year);
                    int MesUltimo = Convert.ToInt16(UltAtualiza.Month);

                    AttOK = false;



                    //Atualiza Registro de fonte do Banco 
                    String SSqlX1 = "Insert into cadastro.nomearq values(@codex,@anox,@mesx,@arquivox,@ultima_arqx,@ultima_banx,@pesox,@atokx)";
                    MySqlCommand XSComando1 = new MySqlCommand(SSqlX1, Conexao);
                    XSComando1.Parameters.AddWithValue("@codex", 10);
                    XSComando1.Parameters.AddWithValue("@anox", AnoNum);
                    XSComando1.Parameters.AddWithValue("@mesx", MesNum);
                    XSComando1.Parameters.AddWithValue("@arquivox", str1);
                    XSComando1.Parameters.AddWithValue("@ultima_arqx", UltAtualiza);
                    XSComando1.Parameters.AddWithValue("@ultima_banx", timeStamp);
                    XSComando1.Parameters.AddWithValue("@pesox", PesoArq);
                    XSComando1.Parameters.AddWithValue("@atokx", AttOK);
                    XSComando1.ExecuteNonQuery();

                    pictureBox1.Refresh();



                } //Fim Atualização com cadastro no banco
            }
             

            Conexao.Close();
            Statusbox.Text = "Atualizado com sucesso!";
            Statusbox2.Text = "";
            pictureBox1.Refresh();
            InitializeTimer2();

        }

        private void AtualizaFormulas()
        {
            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            int UltTabela = 0;

            DataTable tabelanomes = new DataTable();

            uint TotalRegistros;

            pictureBox1.Refresh();

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);


            /*
            listView1.View = View.Details;
            listView1.Columns.Add("Arquivo", 160);
            listView1.Columns.Add("Status", 80);
            listView1.Columns.Add("Ultima Atualização", 160);
            listView1.Columns.Add("Detalhes", 160);
            */

            //Lendo diretorio de arquivos
            string[] arquivos = Directory.GetFiles(Properties.Settings.Default.PathCSV, "Formula*", SearchOption.AllDirectories)
                            .Where(s => s.EndsWith(".csv", StringComparison.OrdinalIgnoreCase)).ToArray();

            FileInfo fi2 = new FileInfo(arquivos[0]);
            DateTime UltAtualiza = fi2.LastAccessTime;
            long PesoArq = fi2.Length;
            bool AttOK = false;



            int numarq = arquivos.Length;
            if (numarq == 1)
            {
                Conexao.Open();
                String Sql = "Select * from cadastro.nomearq where Codigo = 1 ";
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
                    DateTime timeStamp = DateTime.Now;
                    //Nao precisa fazer atualização
                    if (PesoArq.ToString() == tabelanomes.Rows[0]["Tamanho"].ToString())
                    {
                        Statusbox.Text = "Registro de Fórmulas Atualizado";
                        Statusbox.Refresh();
                    }
                    //Existe cadastro mas não está atualizado - Irá procurar o que precisa ser atualizado cruzando informações
                    else//Atualiza1
                    {


                        TotalRegistros = 0;
                        /*
                        ListViewItem item2 = new ListViewItem(new[] { str1, "Atualizando...", tabelanomes.Rows[0]["Ultimaban"].ToString(), "..." });
                        listView1.Items.Add(item2);
                        listView1.Refresh();
                        */
                        Statusbox.Text = "Atualizando Fórmulas";
                        Statusbox.Refresh();
                        pictureBox1.Refresh();

                        String Atsql1 = @"Create Temporary Table cadastro.Tempform1(Dia varchar(10), Hora varchar(10), Nome varchar(30), Form1 int, Form2 int, Prod_1 int, Prod_2 int, Prod_3 int, Prod_4 int, Prod_5 int, Prod_6 int, Prod_7 int, Prod_8 int, Prod_9 int, Prod_10 int, 
                                        Prod_11 int, Prod_12 int, Prod_13 int, Prod_14 int, Prod_15 int, Prod_16 int, Prod_17 int, Prod_18 int, Prod_19 int, Prod_20 int, 
                                        Prod_21 int, Prod_22 int, Prod_23 int, Prod_24 int, Prod_25 int, Prod_26 int, Prod_27 int, Prod_28 int, Prod_29 int, Prod_30 int, 
                                        Prod_31 int, Prod_32 int, Prod_33 int, Prod_34 int, Prod_35 int, Prod_36 int, Prod_37 int, Prod_38 int, Prod_39 int, Prod_40 int,
                                        Prod_41 int,Prod_42 int,Prod_43 int,Prod_44 int,Prod_45 int,Prod_46 int,Prod_47 int,Prod_48 int,Prod_49 int,Prod_50 int,
                                        Prod_51 int,Prod_52 int,Prod_53 int,Prod_54 int,Prod_55 int,Prod_56 int,Prod_57 int,Prod_58 int,Prod_59 int,Prod_60 int,
                                        Prod_61 int,Prod_62 int,Prod_63 int,Prod_64 int,Prod_65 int)";
                                        
                        MySqlCommand AtComando1 = new MySqlCommand(Atsql1, Conexao);
                        AtComando1.ExecuteNonQuery();

                        String ArquivoCSV = arquivos[0].Replace("\\", "/");
                        String Spath = "'" + ArquivoCSV + "'";
                        String Atsql2 = "LOAD DATA LOCAL INFILE " + Spath + " INTO TABLE cadastro.Tempform1 FIELDS TERMINATED BY ',' LINES TERMINATED BY '\\r\\n'";
                        MySqlCommand AtComando2 = new MySqlCommand(Atsql2, Conexao);
                        AtComando2.ExecuteNonQuery();

                        String Emsql4 = @"Create Temporary Table cadastro.TemposForm SELECT b.Dia,b.Hora,b.Nome,b.Form1,b.Form2,b.Prod_1,b.Prod_2,b.Prod_3,b.Prod_4,b.Prod_5,b.Prod_6,b.Prod_7,b.Prod_8,b.Prod_9,b.Prod_10,
                                          b.Prod_11,b.Prod_12,b.Prod_13,b.Prod_14,b.Prod_15,b.Prod_16,b.Prod_17,b.Prod_18,b.Prod_19,b.Prod_20,
                                          b.Prod_21,b.Prod_22,b.Prod_23,b.Prod_24,b.Prod_25,b.Prod_26,b.Prod_27,b.Prod_28,b.Prod_29,b.Prod_30,
                                          b.Prod_31,b.Prod_32,b.Prod_33,b.Prod_34,b.Prod_35,b.Prod_36,b.Prod_37,b.Prod_38,b.Prod_39,b.Prod_40
                                          b.Prod_41,b.Prod_42,b.Prod_43,b.Prod_44,b.Prod_45,b.Prod_46,b.Prod_47,b.Prod_48,b.Prod_49,b.Prod_50,
                                          b.Prod_51,b.Prod_52,b.Prod_53,b.Prod_54,b.Prod_55,b.Prod_56,b.Prod_57,b.Prod_58,b.Prod_59,b.Prod_60,
                                          b.Prod_61,b.Prod_62,b.Prod_63,b.Prod_64,b.Prod_65 
                                          FROM cadastro.Tempform1 as b LEFT JOIN cadastro.formulaideal as a on b.Dia = a.Dia and b.Hora = a.Hora WHERE a.Dia is null";
                        MySqlCommand Ecomando4 = new MySqlCommand(Emsql4, Conexao);
                        Ecomando4.ExecuteNonQuery();

                        String Emsql5 = "Select Count(*) from cadastro.TemposForm";
                        MySqlCommand Ecomando5 = new MySqlCommand(Emsql5, Conexao);
                        Object Elinhas5;
                        Elinhas5 = Ecomando5.ExecuteScalar();
                        uint ELinhasInt5;
                        ELinhasInt5 = Convert.ToUInt16(Elinhas5);
                        pictureBox1.Refresh();
                        //Coloca os dados no relatorio
                        if (ELinhasInt5 != 0)//Sessão A5 
                        {

                            String Emsql6 = "Insert into cadastro.formulaideal select * from cadastro.temposForm";
                            MySqlCommand Ecomando6 = new MySqlCommand(Emsql6, Conexao);
                            Ecomando6.ExecuteNonQuery();

                            String Dropsql1 = "Drop table cadastro.TemposForm";
                            MySqlCommand Dropcomando1 = new MySqlCommand(Dropsql1, Conexao);
                            Dropcomando1.ExecuteNonQuery();

                            String Dropsql2 = "Drop table cadastro.Tempform1";
                            MySqlCommand Dropcomando2 = new MySqlCommand(Dropsql2, Conexao);
                            Dropcomando2.ExecuteNonQuery();

                            String SSqlX1 = "Update cadastro.nomearq set ultimaarq = @ultima_arq, ultimaban=@ultima_ban, tamanho = @peso, Dtok = @atok  where codigo=@code";
                            MySqlCommand XSComando1 = new MySqlCommand(SSqlX1, Conexao);
                            XSComando1.Parameters.AddWithValue("@code", "1");
                            XSComando1.Parameters.AddWithValue("@ultima_arq", UltAtualiza);
                            XSComando1.Parameters.AddWithValue("@ultima_ban", timeStamp);
                            XSComando1.Parameters.AddWithValue("@peso", PesoArq);
                            XSComando1.Parameters.AddWithValue("@atok", "0");
                            XSComando1.ExecuteNonQuery();
                        }

                    }
                }
                else
                {  //Atualização Nova
                    DateTime timeStamp = DateTime.Now;

                    Statusbox.Text = "Atualizando Fórmulas";
                    Statusbox.Refresh();
                    pictureBox1.Refresh();

                    String Atsql1 = @"Create Temporary Table cadastro.Tempform1(Dia varchar(10), Hora time, Nome varchar(30), Form1 int, Form2 int, Prod_1 int, Prod_2 int, Prod_3 int, Prod_4 int, Prod_5 int, Prod_6 int, Prod_7 int, Prod_8 int, Prod_9 int, Prod_10 int, 
                                    Prod_11 int, Prod_12 int, Prod_13 int, Prod_14 int, Prod_15 int, Prod_16 int, Prod_17 int, Prod_18 int, Prod_19 int, Prod_20 int, 
                                    Prod_21 int, Prod_22 int, Prod_23 int, Prod_24 int, Prod_25 int, Prod_26 int, Prod_27 int, Prod_28 int, Prod_29 int, Prod_30 int, 
                                    Prod_31 int, Prod_32 int, Prod_33 int, Prod_34 int, Prod_35 int, Prod_36 int, Prod_37 int, Prod_38 int, Prod_39 int, Prod_40 int,
                                    Prod_41 int, Prod_42 int, Prod_43 int, Prod_44 int, Prod_45 int, Prod_46 int, Prod_47 int, Prod_48 int, Prod_49 int, Prod_50 int,
                                    Prod_51 int, Prod_52 int, Prod_53 int, Prod_54 int, Prod_55 int, Prod_56 int, Prod_57 int, Prod_58 int, Prod_59 int, Prod_60 int,
                                    Prod_61 int, Prod_62 int, Prod_63 int, Prod_64 int, Prod_65 int)";
                    MySqlCommand AtComando1 = new MySqlCommand(Atsql1, Conexao);
                    AtComando1.ExecuteNonQuery();

                    String ArquivoCSV = arquivos[0].Replace("\\", "/");
                    String Spath = "'" + ArquivoCSV + "'";
                    String Atsql2 = "LOAD DATA LOCAL INFILE " + Spath + " INTO TABLE cadastro.Tempform1 FIELDS TERMINATED BY ',' LINES TERMINATED BY '\\r\\n'";
                    MySqlCommand AtComando2 = new MySqlCommand(Atsql2, Conexao);
                    AtComando2.ExecuteNonQuery();

                    String Emsql5 = "Select Count(*) from cadastro.Tempform1";
                    MySqlCommand Ecomando5 = new MySqlCommand(Emsql5, Conexao);
                    Object Elinhas5;
                    Elinhas5 = Ecomando5.ExecuteScalar();
                    uint ELinhasInt5;
                    ELinhasInt5 = Convert.ToUInt16(Elinhas5);

                    pictureBox1.Refresh();
                    //Coloca os dados no relatorio
                    if (ELinhasInt5 != 0)//Sessão A5 
                    {

                        String Emsql6 = "Insert into cadastro.formulaideal select * from cadastro.Tempform1";
                        MySqlCommand Ecomando6 = new MySqlCommand(Emsql6, Conexao);
                        Ecomando6.ExecuteNonQuery();

                        String Dropsql3 = "drop table cadastro.Tempform1";
                        MySqlCommand Dropcomando3 = new MySqlCommand(Dropsql3, Conexao);
                        Dropcomando3.ExecuteNonQuery();

                        String SSqlX1 = "Insert into cadastro.nomearq values(@codex,@anox,@mesx,@arquivox,@ultima_arqx,@ultima_banx,@pesox,@atokx)";
                        MySqlCommand XSComando1 = new MySqlCommand(SSqlX1, Conexao);
                        XSComando1.Parameters.AddWithValue("@codex","1");
                        XSComando1.Parameters.AddWithValue("@anox", "0");
                        XSComando1.Parameters.AddWithValue("@mesx", "0");
                        XSComando1.Parameters.AddWithValue("@arquivox", "Formula.csv");
                        XSComando1.Parameters.AddWithValue("@ultima_arqx", UltAtualiza);
                        XSComando1.Parameters.AddWithValue("@ultima_banx", timeStamp);
                        XSComando1.Parameters.AddWithValue("@pesox", PesoArq);
                        XSComando1.Parameters.AddWithValue("@atokx", "0");
                        XSComando1.ExecuteNonQuery();


                    }
                }
                Conexao.Close();
            }

        }





        private void button1_Click_1(object sender, EventArgs e)
        {
            Statusbox.Text = "Conectando ftp...";
            Statusbox.Refresh();
            bool result = FtpDirectoryExists(@"ftp://"+Properties.Settings.Default.IP+"/",Properties.Settings.Default.UserFTP,Properties.Settings.Default.SenhaFTP);
            if(result == true)
            {
                ObterInformacao();
                AtualizaBanco2_Central();

            }
            else
            {
                MessageBox.Show("Falha na conexão ftp");
                this.Close();
            }



        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
           
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            AtualizaFormulas();
        }

        private void Statusbox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
