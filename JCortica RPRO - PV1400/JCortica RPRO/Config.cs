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
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Timers;

namespace JCortica_RPRO
{
    public partial class Config : Form
    {
        public static bool loginTrava;
        public Config()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }
        private void Config_Load(object sender, EventArgs e)
        {
            //CarregaXML();
            Travar_Action();
            loginTrava = false;
            Travar_Adm();
            Config_carrega();
            Adm_Carrega();
            cmd_edit.Enabled = false;
            Properties.Settings.Default.Reload();
        }

        private void Adm_Carrega()
        {
            cfg_PathDump.Text = Properties.Settings.Default.PathDump;
            cfg_pathSql.Text = Properties.Settings.Default.PathMySql;
            BDumpBox.Text = Properties.Settings.Default.BatDump;

        }
        private void Travar_Adm()
        {
            ImportDump.Enabled = false;
            ZerarBanco.Enabled = false;
            EditarPath.Enabled = false;
            SalvarPath.Enabled = false;
            CancelarPath.Enabled = false;
            cfg_pathSql.Enabled = false;
            cfg_PathDump.Enabled = false;
            BDumpBox.Enabled = false;
            button1.Enabled = false;
         
        }

        private void Travar_saveAdm()
        {
            cfg_pathSql.Enabled = false;
            cfg_PathDump.Enabled = false;
            BDumpBox.Enabled = false;
            EditarPath.Enabled = true;
            SalvarPath.Enabled = false;
            CancelarPath.Enabled = false;
            button1.Enabled = false;
           
        }

        private void editar_Adm()
        {
            cfg_pathSql.Enabled = true;
            cfg_PathDump.Enabled = true;
            BDumpBox.Enabled = true;
            EditarPath.Enabled = false;
            CancelarPath.Enabled = true;
            SalvarPath.Enabled = true;
           
        }

        private void Destravar_Adm()
        {
            ImportDump.Enabled = true;
            ZerarBanco.Enabled = true;
            EditarPath.Enabled = true;
            
        }
        
        private void Editar_Action()
        {
            cmd_Cancelar.Enabled = true;
            cmd_salva.Enabled = true;
            IP_text.Enabled = true;
            Usuario_ftp.Enabled = true;
            Senha_ftp.Enabled = true;
            Porta_text.Enabled = true;
            Usuario_text.Enabled = true;
            Senha_text.Enabled = true;
            ClienteBox.Enabled = true;
            Server_box.Enabled = true;
            PathCSVBox.Enabled = true;
            
            button1.Enabled = true;
            cmd_edit.Enabled = false;

            groupBox1.Enabled = true;
            groupBox2.Enabled = true;
        }
        private void Travar_Action()
        {
            cmd_salva.Enabled = false;
            IP_text.Enabled = false;
            Usuario_ftp.Enabled = false;
            Senha_ftp.Enabled = false;
            Porta_text.Enabled = false;
            Usuario_text.Enabled = false;
            Senha_text.Enabled = false;
            cmd_Cancelar.Enabled = false;
            ClienteBox.Enabled = false;
            Server_box.Enabled = false;
            PathCSVBox.Enabled = false;
           
            button1.Enabled = false;
            groupBox1.Enabled = false;
            groupBox2.Enabled = false;

            cmd_edit.Enabled = true;
        }
        private void CarregaXML() // Fora de Uso 14.02.2018
        {
        }
        private void AlteraXML() // Fora de Uso 14.02.2018
        {
        }
        private void Salva_ConfigUser()
        {
        }
        private void Config_carrega()
        {
            ClienteBox.Text = Properties.Settings.Default.Cliente;
            IP_text.Text = Properties.Settings.Default.IP;
            Usuario_ftp.Text = Properties.Settings.Default.UserFTP;
            Senha_ftp.Text = Properties.Settings.Default.SenhaFTP;
            Porta_text.Text = Properties.Settings.Default.Port;
            Usuario_text.Text = Properties.Settings.Default.User;
            Senha_text.Text = Properties.Settings.Default.Senha;
            Server_box.Text = Properties.Settings.Default.Server;
            PathCSVBox.Text = Properties.Settings.Default.PathCSV;

                if (Properties.Settings.Default.MetodoMesCSV == false)
                {
                optionCSV1.Checked = true;
                }
                else
                {
                optionCSV2.Checked = true;
                }

           if (Properties.Settings.Default.FormulaCSV == true)
           {
           CheckFormula.Checked = true;
           }
           else
           {
           CheckFormula.Checked = false;
           }

            //Impressora

            if (Properties.Settings.Default.NomeComputador.Equals(""))
            {
                Properties.Settings.Default.NomeComputador = Environment.MachineName;
                Properties.Settings.Default.Save();
            }

            if(Properties.Settings.Default.TipoImpressora.Equals("Local"))
            {
                radioLocal.Checked = true;
            } else
            {
                radioRede.Checked = true;
            }

            nomeImpressoraBox.Text = Properties.Settings.Default.NomeImpressora;
            nomeComputadorBox.Text = Properties.Settings.Default.NomeComputador;

        }

        private void cmd_edit_Click(object sender, EventArgs e)
        {
            Editar_Action();
        }
        private void label8_Click(object sender, EventArgs e)
        {
        }
        private void cmd_Cancelar_Click(object sender, EventArgs e)
        {
            Config_carrega();
            Travar_Action();
        }
        private void criar_texto()
        {
            string endereco_1 = IP_text.Text;
            string endereco_2 = PathCSVBox.Text;
            string endereco_3 = "";
            string[] lines = { "lcd " + endereco_2, "open " + endereco_1, "anonymous", "bin", "get InternalStorage/data/" + endereco_3, "quit" };
            try
            {
                File.WriteAllLines(@"" + endereco_2 + "\\texto_1.txt", lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }
        private void cmd_criar_Click(object sender, EventArgs e)
        {
        }
        private void cmd_salva_Click(object sender, EventArgs e)
        {
            Escreve_Config();
            criar_texto();
            
            Travar_Action();
        }

        private void cmd_sair_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Escreve_Config()
        {
            Properties.Settings.Default.Cliente = ClienteBox.Text;
            Properties.Settings.Default.IP = IP_text.Text;
            Properties.Settings.Default.UserFTP = Usuario_ftp.Text;
            Properties.Settings.Default.SenhaFTP = Senha_ftp.Text;
            Properties.Settings.Default.Port = Porta_text.Text;
            Properties.Settings.Default.User = Usuario_text.Text;
            Properties.Settings.Default.Senha = Senha_text.Text;
            Properties.Settings.Default.Server = Server_box.Text;
            Properties.Settings.Default.PathCSV = PathCSVBox.Text;
            //Properties.Settings.Default.NomeCSV = NomeCSVBox.Text;

            if (optionCSV1.Checked == true)
            {
                Properties.Settings.Default.MetodoMesCSV = false;
            }
            else
            {
                Properties.Settings.Default.MetodoMesCSV = true;
            }

            if (CheckFormula.Checked == true) { Properties.Settings.Default.FormulaCSV = true; }
            else { Properties.Settings.Default.FormulaCSV = false; }

            Properties.Settings.Default.Save();
        }
        private void Login_cmd_Click(object sender, EventArgs e)
        {
            if (config_senha.Text == "")
            {
                MessageBox.Show("Preencha o campo de senha para fazer o login.");
            }
            else
            {
                if (config_senha.Text == "Padrao")
                {
                    Destravar_Adm();
                    cmd_edit.Enabled = true;

                    //Impressora
                    editImpressoraButton.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Senha incorreta!");
                }
            }
        }
        private void criar_texto2()
        {
            string endereco1 = cfg_pathSql.Text;
            string endereco2 = cfg_PathDump.Text;
            string endereco3 = BDumpBox.Text;
            string[] lines = { "cd " + endereco1, "mysql -u root -p < " + endereco2 + "\\DumpPadrao.sql", "pause" };

            try
            {
                File.WriteAllLines(@"" + endereco3 + "\\ImportDump.bat", lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }

        private void criar_texto3()
        {
            string enderecoCSV = PathCSVBox.Text;
           
            string[] lines = { "ftp -s:" + enderecoCSV + "\\texto_1.txt"};
            try
            {
                File.WriteAllLines(@"" + enderecoCSV + "\\ENGINE.bat", lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
            criar_texto4();
        }
        private void criar_texto4()
        {
            string enderecoCSV = PathCSVBox.Text;

            string[] lines = { "ftp -s:" + enderecoCSV + "\\texto_2.txt" };
            try
            {
                File.WriteAllLines(@"" + enderecoCSV + "\\ENGINE2.bat", lines);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
        }


        private void EditarPath_Click(object sender, EventArgs e)
        {
            editar_Adm();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Server_box_TextChanged(object sender, EventArgs e)
        {
        }
        private void PathCSVBox_TextChanged(object sender, EventArgs e)
        {
        }
        private void CancelarPath_Click(object sender, EventArgs e)
        {
            Adm_Carrega();
            Travar_saveAdm();
        }
        private void SalvarPath_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.PathMySql = cfg_pathSql.Text;
            Properties.Settings.Default.PathDump = cfg_PathDump.Text;
            Properties.Settings.Default.BatDump = BDumpBox.Text;
            Properties.Settings.Default.Save();

            criar_texto2();
            criar_texto3();
            Travar_saveAdm();
        }

        private void ZerarBanco_Click(object sender, EventArgs e)
        {
            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);
            Conexao.Open();
            String Sql = "Truncate cadastro.relatorio";
            MySqlCommand ComandoDel = new MySqlCommand(Sql, Conexao);
            try
            {
                ComandoDel.ExecuteNonQuery();

                String Sql2 = "Truncate cadastro.nomearq";
                MySqlCommand ComandoDel2 = new MySqlCommand(Sql2, Conexao);
                ComandoDel2.ExecuteNonQuery();

                String Sql3 = "Truncate cadastro.formulaideal";
                MySqlCommand ComandoDel3 = new MySqlCommand(Sql3, Conexao);
                ComandoDel3.ExecuteNonQuery();

                MessageBox.Show("Registros do relatorio apagados do servidor MySql");
                Conexao.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Falha na conexão MySql: " + ex.Message);
                Conexao.Close();
            }
           
        }

        private void ImportDump_Click(object sender, EventArgs e)
        {
            String Parte1 = Properties.Settings.Default.BatDump;
            ProcessStartInfo Baixar = new ProcessStartInfo(@"" + Parte1 + "\\ImportDump.bat");
            Baixar.WindowStyle = ProcessWindowStyle.Normal;
            try
            {
                Process.Start(Baixar);
            }
           catch(Exception ex)
            {
                MessageBox.Show("Erro no comando: " + ex.Message);
            }
        }

        private void label4_Click_1(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void ChamaRelatAntigo()
        {
            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            int UltTabela = 0;

            DataTable tabelanomes = new DataTable();

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);

            Conexao.Open();
            try{
                String Atsql4X = "Select Count(*) from cadastro.relatorio";
                MySqlCommand Comando4X = new MySqlCommand(Atsql4X, Conexao);
                Object Linhas4X;
                Linhas4X = Comando4X.ExecuteScalar();
                uint LinhasInt4X;
                LinhasInt4X = Convert.ToUInt32(Linhas4X);

                if (LinhasInt4X == 0)
                {
                    String Atsql1 = "Create Temporary Table cadastro.Temp(Dia varchar(10), Hora time, Nome varchar(30), Form1 int, Form2 int, Prod_1 int, Prod_2 int, Prod_3 int, Prod_4 int, Prod_5 int, Prod_6 int, Prod_7 int, Prod_8 int, Prod_9 int, Prod_10 int, Prod_11 int, Prod_12 int, Prod_13 int, Prod_14 int, Prod_15 int, Prod_16 int, Prod_17 int, Prod_18 int, Prod_19 int, Prod_20 int, Prod_21 int, Prod_22 int, Prod_23 int, Prod_24 int, Prod_25 int, Prod_26 int, Prod_27 int, Prod_28 int, Prod_29 int, Prod_30 int, Prod_31 int, Prod_32 int, Prod_33 int, Prod_34 int, Prod_35 int, Prod_36 int, Prod_37 int, Prod_38 int, Prod_39 int, Prod_40 int)";
                    MySqlCommand AtComando1 = new MySqlCommand(Atsql1, Conexao);
                    AtComando1.ExecuteNonQuery();
                    
                    String ArquivoPath = Properties.Settings.Default.PathCSV;
                    String ArquivoInvertido = ArquivoPath.Replace("\\", "/");
                    String ArquivoCSV = "Relatorio_1.csv";

                    String Spath = "'" + ArquivoInvertido + ArquivoCSV + "'";

                    String Atsql2 = "LOAD DATA LOCAL INFILE " + Spath + " INTO TABLE cadastro.Temp FIELDS TERMINATED BY ',' LINES TERMINATED BY '\\r\\n'";
                    MySqlCommand AtComando2 = new MySqlCommand(Atsql2, Conexao);
                    AtComando2.ExecuteNonQuery();

                    String Atsql5X = "Select Count(*) from cadastro.Temp";
                    MySqlCommand Comando5X = new MySqlCommand(Atsql5X, Conexao);
                    Object Linhas5X;
                    Linhas5X = Comando5X.ExecuteScalar();
                    uint LinhasInt5X;
                    LinhasInt5X = Convert.ToUInt32(Linhas5X);

                    if (LinhasInt5X != 0)
                    {

                        String Emsql6 = "Insert into cadastro.relatorio select * from cadastro.Temp";
                        MySqlCommand Ecomando6 = new MySqlCommand(Emsql6, Conexao);
                        Ecomando6.ExecuteNonQuery();

                        String Dropsql1 = "Drop table cadastro.Temp";
                        MySqlCommand Dropcomando1 = new MySqlCommand(Dropsql1, Conexao);
                        Dropcomando1.ExecuteNonQuery();

                        MessageBox.Show("Foram adicionados " + LinhasInt5X + " registros no banco");
                    }
                    else
                    {
                        MessageBox.Show("Nenhum registro no arquivo.");
                    }
                }
                else
                {
                    MessageBox.Show("Apague os dados do banco para importar o relatorio da versão anterior.");
                }
            }
                
            catch (MySqlException erro)
            {
               MessageBox.Show("Erro na Conexão: " + erro);
            }

            Conexao.Close();

        }
            
            


        private void Usuario_ftp_TextChanged(object sender, EventArgs e)
        {

        }

        private void IP_text_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChamaRelatAntigo();
        }

        private void CheckFormula_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void optionCSV1_CheckedChanged(object sender, EventArgs e)
        {
           
        }

        private void ClienteBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void config_senha_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void label21_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioLocal_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void editImpressoraButton_Click(object sender, EventArgs e)
        {
            nomeImpressoraBox.Enabled = true;
            nomeComputadorBox.Enabled = true;

            radioLocal.Enabled = true;
            radioRede.Enabled = true;

            salvarImpressoraButton.Enabled = true;
        }

        private void salvarImpressoraButton_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.NomeImpressora = nomeImpressoraBox.Text;
            Properties.Settings.Default.NomeComputador = nomeComputadorBox.Text;

            if (radioLocal.Checked == true)
                Properties.Settings.Default.TipoImpressora = "Local";
            else
                Properties.Settings.Default.TipoImpressora = "Rede";

            Properties.Settings.Default.Save();
        }   
    }
}
