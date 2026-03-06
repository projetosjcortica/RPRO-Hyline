using JCortica_RPRO.Leonardo.Domain;
using JCortica_RPRO.Repositories;
using Leonardo.Domain;
using Leonardo.Services;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TesteImpresao.Entities;

namespace JCortica_RPRO
{
    public partial class Etiquetas : Form
    {

        private int _currentLabelId;
        private LabelZebra _lastLabel;

        private LabelZebra _currentLabel;
        private DataTable _labelDataTable;
        private LabelRepository _labelRepository;
        private LabelServices _labelServices;
        private int _countTimer;
        private bool _isUpdating;

        public List<string> _Dias { get; private set; }

        private bool IsTimerRunning;
        private int TimeInSeg = 5;

        public string str1 { get; set; }
        public string str2 { get; set; }
        public string str3 { get; set; }
        public string str4 { get; set; }
        public int AnoNum { get; set; }
        public int MesNum { get; set; }
        public bool AnoNumOK { get; set; }
        public bool MesNumOK { get; set; }

        public static List<string> ArquivosnoFTP = new List<string>();


        public Etiquetas()
        {
            InitializeComponent();
            _labelDataTable = CreateEtiquetasDataTable();
            _labelRepository = new LabelRepository();
            _labelServices = new LabelServices();
        }

        private void Etiquetas_Load(object sender, EventArgs e)
        {
            tempoAtualização.Text = TimeInSeg.ToString();
            var dias = _labelRepository.GetAllDates();
            RefreshDateDropdowns(dias);

            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;

            comboBoxNomeFormula.Enabled = false;
            comboBoxNumeroFormula.Enabled = false;
            comboBoxCodigoFormula.Enabled = false;
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            AddSelectDataGrid();
        }

        private async void AddSelectDataGrid()
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                var selectedRow = dataGridView1.SelectedRows[0];

                var labelId = selectedRow.Cells["Id"].Value;

                var label = await _labelRepository.GetLabelById((int)labelId);

                _currentLabel = label;
            }
        }

        private async void Pesquisar3_Click(object sender, EventArgs e)
        {
            _labelDataTable = CreateEtiquetasDataTable();
            var listLabel = new List<LabelZebra>();

            var existId = etiquetaIdBox.Text.Length != 0;


            if ((comboBoxNomeFormula.SelectedItem == null || string.IsNullOrEmpty(comboBoxNomeFormula.SelectedItem.ToString())) &&
                (comboBoxNumeroFormula.SelectedItem == null || string.IsNullOrEmpty(comboBoxNumeroFormula.SelectedItem.ToString())) &&
                (comboBoxCodigoFormula.SelectedItem == null || string.IsNullOrEmpty(comboBoxCodigoFormula.SelectedItem.ToString())) ||
                existId)
            {

                if (existId)
                {
                    var label = await SearchById();
                    if (label != null)
                        listLabel.Add(label);
                }
                else if (!string.IsNullOrEmpty(comboBoxDatas2.SelectedItem?.ToString()))
                {
                    var resultQuery = await SearchByPeriod();

                    if (resultQuery == null)
                    {
                        dataGridView1.DataSource = _labelDataTable;
                        return;
                    }

                    for (int i = 0; i < resultQuery.Count; i++)
                    {
                        listLabel.Add(resultQuery[i]);
                    }
                }
                else
                {
                    var resultQuery = await SearchByData();

                    if (resultQuery == null)
                    {
                        dataGridView1.DataSource = _labelDataTable;
                        return;
                    }

                    for (int i = 0; i < resultQuery.Count; i++)
                    {
                        listLabel.Add(resultQuery[i]);
                    }
                }
            }
            else
            {
                var DiaInicial = comboBoxDatas.SelectedItem?.ToString() ?? string.Empty;
                var DiaFinal = comboBoxDatas2.SelectedItem?.ToString() ?? string.Empty;
                var NumeroFormula = comboBoxNumeroFormula.SelectedItem?.ToString() ?? string.Empty;
                var CodigoFormula = comboBoxCodigoFormula.SelectedItem?.ToString() ?? string.Empty;
                var NomeFormula = comboBoxNomeFormula.SelectedItem?.ToString() ?? string.Empty;


                var resultQuery = await _labelRepository.GetLabelAdvancedSearch(NomeFormula, NumeroFormula, CodigoFormula, DiaInicial, DiaFinal);

                for (int i = 0; i < resultQuery.Count; i++)
                {
                    listLabel.Add(resultQuery[i]);
                }
            }


            if (listLabel.Count > 0)
            {
                for (int i = 0; i < listLabel.Count; i++)
                {
                    var label = listLabel[i];
                    var row = CreateRowFromLabel(label);
                    _labelDataTable.Rows.Add(row);
                }
            }

            dataGridView1.DataSource = _labelDataTable;

            DefineComboBoxSearch();
        }

        private void DefineComboBoxSearch()
        {
            var DataInicial = comboBoxDatas.SelectedItem?.ToString() ?? string.Empty;
            var DataFinal = comboBoxDatas2.SelectedItem?.ToString() ?? string.Empty;

            if (string.IsNullOrEmpty(DataInicial))
            {
                comboBoxNumeroFormula.Enabled = false;
                comboBoxNomeFormula.Enabled = false;
                comboBoxCodigoFormula.Enabled = false;
                comboBoxNumeroFormula.DataSource = new List<string>() { "" };
                comboBoxNomeFormula.DataSource = new List<string>() { "" };
                comboBoxCodigoFormula.DataSource = new List<string>() { "" };
                return;
            }

            if (radioNumeroFormula.Checked)
            {
                comboBoxNumeroFormula.Enabled = true;
                var list = _labelRepository.GetNumeroFormula(DataInicial, DataFinal);
                list.Insert(0, "");
                comboBoxNumeroFormula.DataSource = list;
                comboBoxNomeFormula.Enabled = false;
                comboBoxCodigoFormula.Enabled = false;
                comboBoxNomeFormula.DataSource = new List<string>() { "" };
                comboBoxCodigoFormula.DataSource = new List<string>() { "" };
            }

            if (radioNomeFormula.Checked)
            {
                comboBoxNomeFormula.Enabled = true;
                var list = _labelRepository.GetNomeFormula(DataInicial, DataFinal);
                list.Insert(0, "");
                comboBoxNomeFormula.DataSource = list;
                comboBoxNumeroFormula.Enabled = false;
                comboBoxCodigoFormula.Enabled = false;
                comboBoxNumeroFormula.DataSource = new List<string>() { "" };
                comboBoxCodigoFormula.DataSource = new List<string>() { "" };
            }

            if (radioCodigoFormula.Checked)
            {
                comboBoxCodigoFormula.Enabled = true;
                var list = _labelRepository.GetCodigoFormula(DataInicial, DataFinal);
                list.Insert(0, "");
                comboBoxCodigoFormula.DataSource = list;
                comboBoxNomeFormula.Enabled = false;
                comboBoxNumeroFormula.Enabled = false;
                comboBoxNomeFormula.DataSource = new List<string>() { "" };
                comboBoxNumeroFormula.DataSource = new List<string>() { "" };
            }
        }

        public void MappLabelsForDataGrid(List<LabelZebra> labels)
        {
            _labelDataTable = CreateEtiquetasDataTable();


            for (int i = 0; i < labels.Count; i++)
            {
                var label = labels[i];
                var row = CreateRowFromLabel(label);
                _labelDataTable.Rows.Add(row);
            }

            dataGridView1.DataSource = _labelDataTable;

        }


        private async Task<LabelZebra> SearchById()
        {
            if (!int.TryParse(etiquetaIdBox.Text, out int labelId))
            {
                MessageBox.Show($"Ocorreu um erro: O valor de Etiqueta ID precisa ser um número.",
                  "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            if (labelId == 0)
            {
                MessageBox.Show($"Ocorreu um erro: O valor de Etiqueta ID não pode ser zero.",
                     "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            var label = await _labelRepository.GetLabelById(labelId);

            if (label == null)
            {
                MessageBox.Show($"Ocorreu um erro: Etiqueta não encontrada.",
                      "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            _currentLabel = label;
            return label;
        }

        private async Task<List<LabelZebra>> SearchByData()
        {
            var dia = comboBoxDatas.SelectedItem as string;

            if (string.IsNullOrEmpty(dia))
            {
                MessageBox.Show("Por favor, selecione um dia válido.");
                return null;
            }

            return await _labelRepository.GetLabelFromDate(dia);
        }

        private async Task<List<LabelZebra>> SearchByPeriod()
        {
            var diaInicial = comboBoxDatas.SelectedItem as string;
            var diaFinal = comboBoxDatas2.SelectedItem as string;

            if (string.IsNullOrEmpty(diaInicial))
            {
                MessageBox.Show("Por favor, selecione um dia válido em data inicial.");
                return null;
            }

            if (string.IsNullOrEmpty(diaFinal))
            {
                MessageBox.Show("Por favor, selecione um dia válido em data final.");
                return null;
            }

            return await _labelRepository.GetLabelFromPeriod(diaInicial, diaFinal);
        }

        private void Print2_Click(object sender, EventArgs e)
        {
            if (_currentLabel == null) return;

            _labelServices.PrintLabel(_currentLabel);
        }

        private DataTable CreateEtiquetasDataTable()
        {
            DataTable table = new DataTable("Etiquetas");

            table.Columns.Add("Id", typeof(int));
            table.Columns.Add("Dia", typeof(string));
            table.Columns.Add("Hora", typeof(TimeSpan));
            table.Columns.Add("Ciclo", typeof(string));
            table.Columns.Add("Responsável", typeof(string));
            table.Columns.Add("Observação", typeof(string));
            table.Columns.Add("Nome Fórmula", typeof(string));
            table.Columns.Add("Código Fórmula", typeof(string));
            table.Columns.Add("Número Fórmula", typeof(string));

            for (int i = 1; i <= 24; i++)
            {
                table.Columns.Add($"Produto {i} Nome", typeof(string));
                table.Columns.Add($"Produto {i} Lote", typeof(string));
                table.Columns.Add($"Produto {i} Peso", typeof(double));
            }

            return table;
        }

        private DataRow CreateRowFromLabel(LabelZebra label)
        {
            DataRow row = _labelDataTable.NewRow();
            row["Responsável"] = label.Responsavel;
            row["Id"] = label.Id;
            row["Observação"] = label.Observacao;
            row["Dia"] = label.Dia;
            row["Hora"] = label.Hora;
            row["Nome Fórmula"] = label.NomeFormula;
            row["Código Fórmula"] = label.CodigoFormula;
            row["Número Fórmula"] = label.NumeroFormula;
            row["Ciclo"] = label.Ciclo;

            for (int i = 0; i < label.LabelItem.Count; i++)
            {
                LabelItem labelItem = label.LabelItem[i];
                row[$"Produto {i + 1} Nome"] = labelItem.ProductName;
                row[$"Produto {i + 1} Lote"] = labelItem.Lote;
                row[$"Produto {i + 1} Peso"] = labelItem.Weight;
            }

            return row;
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            _countTimer++;
            TimeInSeg = TimeInSeg = int.Parse(tempoAtualização.Text);
            var TimeInterval = _countTimer == TimeInSeg;

            if (TimeInterval)
            {
                await AtualizaCicloAsync();
                await automaticPrintLabel();
                _countTimer = 0;
            }
        }

        private async Task automaticPrintLabel()
        {
            var listLabel = await _labelRepository.GetLabelNotPrinter();
            foreach (var label in listLabel)
            {
                _labelServices.PrintLabel(label);
                await _labelRepository.UpdateLabelForPrinter(label.Id);
            }
        }

        private void MockEtiqueta_Click(object sender, EventArgs e)
        {

        }

        private async void pictureBox1_Click(object sender, EventArgs e)
        {
            if (IsTimerRunning)
            {
                timer1.Stop();
                IsTimerRunning = false;
                btnImpressaoAutomatica.BackgroundImage = Properties.Resources.botao_play;
                Print2.Enabled = true;

            }
            else
            {
                await AtualizaCicloAsync();
                var listLabel = await _labelRepository.GetLabelNotPrinter();

                if (listLabel.Count > 0)
                {
                    DialogResult resultado = MessageBox.Show($"Existe um total de {listLabel.Count} etiquetas não impressas, deseja imprimir?",
                                                  "Confirmação",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Question);

                    if (resultado == DialogResult.Yes)
                    {
                        foreach (var label in listLabel)
                        {
                            _labelServices.PrintLabel(label);
                            await _labelRepository.UpdateLabelForPrinter(label.Id);
                        }
                    }
                    else if (resultado == DialogResult.No)
                    {
                        foreach (var label in listLabel)
                        {
                            await _labelRepository.UpdateLabelForPrinter(label.Id);
                        }
                    }
                }

                timer1.Start();
                IsTimerRunning = true;
                btnImpressaoAutomatica.BackgroundImage = Properties.Resources.pausa;
                Print2.Enabled = false;
            }
        }

        private void tempoAtualização_Leave(object sender, EventArgs e)
        {
            // Tenta converter o texto do campo para um número inteiro
            if (int.TryParse(tempoAtualização.Text, out int tempo))
            {
                // Verifica se o número é menor que 5
                if (tempo < 5)
                {
                    // Se for menor que 5, define o valor como 5
                    tempoAtualização.Text = "5";
                }
            }
            else
            {
                // Se não for um número válido, exibe uma mensagem de erro
                MessageBox.Show("Por favor, insira um número válido.", "Entrada Inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                tempoAtualização.Focus(); // Foca de volta no campo de texto
            }
        }
        public void AtualizaBanco3_Central()
        {


            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            int UltTabela = 0;

            DataTable tabelanomes = new DataTable();

            uint TotalRegistros;

            //pictureBox1.Refresh();

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);

            Conexao.Open();

            //Lendo diretorio de arquivos
            string[] arquivos = Directory.GetFiles(Properties.Settings.Default.PathCSV, "Pesagem_20*", SearchOption.AllDirectories)
                            .Where(s => s.EndsWith(".csv", StringComparison.OrdinalIgnoreCase)).ToArray();

            int numarq = arquivos.Length;
            Statusbox.Text = numarq.ToString();

            if (numarq == 0)
            {
                MessageBox.Show("Nenhum arquivo compatível no diretório FTP");
            }
            else //Inicio
            {
                for (int i = 0; i < numarq; i++)//Peso1
                {
                    int Limite = arquivos[i].LastIndexOf(".csv");
                    int Letra1 = arquivos[i].IndexOf("Pesagem_20");
                    //pictureBox1.Refresh();
                    //FASE 1 - Teste para ver se o arquivo é legitimo em quantidade caracteres 
                    if (Limite - Letra1 == 15)
                    {
                        str1 = arquivos[i].Substring(Letra1, 15); //Nome Arquivo Completo
                        str2 = arquivos[i].Substring(Letra1 + 8, 4); //Ano do Arquivo
                        str3 = arquivos[i].Substring(Letra1 + 13, 2); //Mes do Arquivo
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
                            //pictureBox1.Refresh();
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
                                    //pictureBox1.Refresh();

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


                                    //pictureBox1.Refresh();
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
                                    //pictureBox1.Refresh();
                                }
                                //Existe cadastro mas não está atualizado - Irá procurar o que precisa ser atualizado cruzando informações
                                else//Atualiza1
                                {
                                    DateTime timeStamp = DateTime.Now;

                                    TotalRegistros = 0;

                                    Statusbox.Text = str1 + " " + "Atualizando..." + " " + tabelanomes.Rows[0]["Ultimaban"].ToString();
                                    Statusbox.Refresh();
                                    //pictureBox1.Refresh();

                                    //AntigoX
                                    /*String Atsql1 = @"Create Temporary Table cadastro.Tempat1(Dia varchar(10), Hora time, Nome varchar(30), Form1 int, Form2 int, Prod_1 int, Prod_2 int, Prod_3 int, Prod_4 int, Prod_5 int, Prod_6 int, Prod_7 int, Prod_8 int, Prod_9 int, Prod_10 int, 
                                                      Prod_11 int, Prod_12 int, Prod_13 int, Prod_14 int, Prod_15 int, Prod_16 int, Prod_17 int, Prod_18 int, Prod_19 int, Prod_20 int, 
                                                      Prod_21 int, Prod_22 int, Prod_23 int, Prod_24 int, Prod_25 int, Prod_26 int, Prod_27 int, Prod_28 int, Prod_29 int, Prod_30 int, 
                                                      Prod_31 int, Prod_32 int, Prod_33 int, Prod_34 int, Prod_35 int, Prod_36 int, Prod_37 int, Prod_38 int, Prod_39 int, Prod_40 int,
                                                      Prod_41 int,Prod_42 int,Prod_43 int,Prod_44 int,Prod_45 int,Prod_46 int,Prod_47 int,Prod_48 int,Prod_49 int,Prod_50 int,
                                                      Prod_51 int,Prod_52 int,Prod_53 int,Prod_54 int,Prod_55 int,Prod_56 int,Prod_57 int,Prod_58 int,Prod_59 int,Prod_60 int,
                                                      Prod_61 int,Prod_62 int,Prod_63 int,Prod_64 int,Prod_65 int)";
                                    */

                                    String Atsql1 = @"Create Temporary Table cadastro.Tempat1(dia varchar(10),hora TIME,responsavel varchar(50),observacao varchar(500),prod_1_peso INT,prod_2_peso INT,prod_3_peso INT,prod_4_peso INT,prod_5_peso INT,prod_6_peso INT,prod_7_peso INT,
                                                       prod_8_peso INT,prod_9_peso INT,prod_10_peso INT,prod_11_peso INT,prod_12_peso INT,prod_13_peso INT,prod_14_peso INT,prod_15_peso INT,prod_16_peso INT,prod_17_peso INT,prod_18_peso INT,prod_19_peso INT,prod_20_peso INT,prod_21_peso INT,
                                                       prod_22_peso INT,prod_23_peso INT,prod_24_peso INT)";


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

                                    String Atsql3X = "Select Count(*) from cadastro.Temat1";
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
                                        String Atsql4 = "Create Temporary Table cadastro.ultimat1 select * from cadastro.pesagemcsv where month(str_to_date(dia, '%d/%m/%Y')) = @data1 and year(str_to_date(dia, '%d/%m/%Y')) = @data2";
                                        MySqlCommand AtComando4 = new MySqlCommand(Atsql4, Conexao);
                                        AtComando4.Parameters.AddWithValue("@data1", str3);
                                        AtComando4.Parameters.AddWithValue("@data2", str2);
                                        AtComando4.ExecuteNonQuery();
                                        //pictureBox1.Refresh();
                                        String Atsql4X = "Select Count(*) from cadastro.ultimat1";
                                        MySqlCommand Comando4X = new MySqlCommand(Atsql4X, Conexao);
                                        Object Linhas4X;
                                        Linhas4X = Comando4X.ExecuteScalar();
                                        uint LinhasInt4X;
                                        LinhasInt4X = Convert.ToUInt32(Linhas4X);

                                        String Atsql4X2 = "Drop table cadastro.ultimat1";
                                        MySqlCommand Comando4X2 = new MySqlCommand(Atsql4X2, Conexao);
                                        Comando4X2.ExecuteNonQuery();
                                        //pictureBox1.Refresh();
                                        //Há dados para serem cruzados
                                        //EMULADOR DE DADOS POR MES E DIA -------
                                        if (LinhasInt4X != 0)//Sessão A2
                                        {

                                            //pictureBox1.Refresh();
                                            for (int x = 1; x < 32; x++)//Sessão A3
                                            {
                                                int porcentagem;
                                                porcentagem = x * 100 / 31;
                                                //listView1.Items[UltTabela].SubItems[3].Text = "Lendo arquivo:" + porcentagem.ToString() + "%";
                                                //listView1.Refresh();

                                                Statusbox.Text = str1 + "Lendo arquivo:" + porcentagem.ToString() + "%";
                                                Statusbox.Refresh();
                                                //pictureBox1.Refresh();
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
                                                    String Emsql3 = "Create Temporary Table cadastro.recorte1 select * from cadastro.pesagemcsv where day(str_to_date(dia, '%d/%m/%Y')) = @dado4 and month(str_to_date(dia, '%d/%m/%Y')) = @dado5 and year(str_to_date(dia, '%d/%m/%Y')) = @dado6";
                                                    MySqlCommand Ecomando3 = new MySqlCommand(Emsql3, Conexao);
                                                    Ecomando3.Parameters.AddWithValue("@dado4", x.ToString()); //Dia
                                                    Ecomando3.Parameters.AddWithValue("@dado5", str3); //Mes
                                                    Ecomando3.Parameters.AddWithValue("@dado6", str2); //Ano
                                                    Ecomando3.ExecuteNonQuery();

                                                    //pictureBox1.Refresh();
                                                    //AntigoX
                                                    /*
                                                    String Emsql4 = @"Create Temporary Table cadastro.Tempos SELECT b.Dia,b.Hora,b.Nome,b.Form1,b.Form2,b.Prod_1,b.Prod_2,b.Prod_3,b.Prod_4,b.Prod_5,b.Prod_6,b.Prod_7,b.Prod_8,b.Prod_9,b.Prod_10,
                                                                    b.Prod_11,b.Prod_12,b.Prod_13,b.Prod_14,b.Prod_15,b.Prod_16,b.Prod_17,b.Prod_18,b.Prod_19,b.Prod_20,
                                                                    b.Prod_21,b.Prod_22,b.Prod_23,b.Prod_24,b.Prod_25,b.Prod_26,b.Prod_27,b.Prod_28,b.Prod_29,b.Prod_30,
                                                                    b.Prod_31,b.Prod_32,b.Prod_33,b.Prod_34,b.Prod_35,b.Prod_36,b.Prod_37,b.Prod_38,b.Prod_39,b.Prod_40, 
                                                                    b.Prod_41,b.Prod_42,b.Prod_43,b.Prod_44,b.Prod_45,b.Prod_46,b.Prod_47,b.Prod_48,b.Prod_49,b.Prod_50,
                                                                    b.Prod_51,b.Prod_52,b.Prod_53,b.Prod_54,b.Prod_55,b.Prod_56,b.Prod_57,b.Prod_58,b.Prod_59,b.Prod_60,
                                                                    b.Prod_61,b.Prod_62,b.Prod_63,b.Prod_64,b.Prod_65
                                                                    FROM cadastro.nova1 as b LEFT JOIN cadastro.recorte1 as a on b.Dia = a.Dia and b.Hora = a.Hora WHERE a.Dia is null";
                                                    */
                                                    String Emsql4 = @"Create Temporary Table cadastro.Tempos SELECT b.dia,b.hora,b.responsavel,b.observacao,b.prod_1_peso,b.prod_2_peso,b.prod_3_peso,b.prod_4_peso,b.prod_5_peso,b.prod_6_peso,
                                                                    b.prod_7_peso,b.prod_8_peso,b.prod_9_peso,b.prod_10_peso,b.prod_11_peso,b.prod_12_peso,b.prod_13_peso,b.prod_14_peso,b.prod_15_peso,b.prod_16_peso,b.prod_17_peso,b.prod_18_peso,
                                                                    b.prod_19_peso,b.prod_20_peso,b.prod_21_peso,b.prod_22_peso,b.prod_23_peso,b.prod_24_peso
                                                                    FROM cadastro.nova1 as b LEFT JOIN cadastro.recorte1 as a on b.Dia = a.Dia and b.Hora = a.Hora WHERE a.Dia is null";


                                                    MySqlCommand Ecomando4 = new MySqlCommand(Emsql4, Conexao);
                                                    Ecomando4.ExecuteNonQuery();

                                                    String Emsql5 = "Select Count(*) from cadastro.Tempos";
                                                    MySqlCommand Ecomando5 = new MySqlCommand(Emsql5, Conexao);
                                                    Object Elinhas5;
                                                    Elinhas5 = Ecomando5.ExecuteScalar();
                                                    uint ELinhasInt5;
                                                    ELinhasInt5 = Convert.ToUInt16(Elinhas5);
                                                    //pictureBox1.Refresh();
                                                    //Coloca os dados no relatorio
                                                    if (ELinhasInt5 != 0)//Sessão A5 
                                                    {
                                                        TotalRegistros = TotalRegistros + ELinhasInt5;
                                                        //String Emsql6 = "Insert into cadastro.pesagemcsv select * from cadastro.tempos";

                                                        String Emsql6 = @"INSERT INTO pesagemcsv(
                                                            dia,hora,responsavel,observacao,prod_1_peso,prod_2_peso,prod_3_peso,prod_4_peso,prod_5_peso,prod_6_peso,prod_7_peso,prod_8_peso,prod_9_peso,prod_10_peso,prod_11_peso,prod_12_peso,prod_13_peso,
                                                            prod_14_peso,prod_15_peso,prod_16_peso,prod_17_peso,prod_18_peso,prod_19_peso,prod_20_peso,prod_21_peso,prod_22_peso,prod_23_peso,prod_24_peso,valida)
                                                            SELECT
                                                            dia,hora,responsavel,observacao,prod_1_peso,prod_2_peso,prod_3_peso,prod_4_peso,prod_5_peso,prod_6_peso,prod_7_peso,prod_8_peso,prod_9_peso,prod_10_peso,prod_11_peso,prod_12_peso,prod_13_peso,
                                                            prod_14_peso,prod_15_peso,prod_16_peso,prod_17_peso,prod_18_peso,prod_19_peso,prod_20_peso,prod_21_peso,prod_22_peso,prod_23_peso,prod_24_peso,FALSE 
                                                            from cadastro.tempos";

                                                        MySqlCommand Ecomando6 = new MySqlCommand(Emsql6, Conexao);
                                                        Ecomando6.ExecuteNonQuery();
                                                    }
                                                    //pictureBox1.Refresh();
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
                                                //pictureBox1.Refresh();
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
                                            //pictureBox1.Refresh();
                                        } //FIM DA SEÇÃO A2 
                                        else
                                        {
                                            //Atualização direta para banco 

                                            //AntigoX
                                            //String A1Sql1 = "Insert into cadastro.relatorio select * from cadastro.ultimat1";

                                            String A1Sql1 = @"INSERT INTO pesagemcsv(
                                                            dia,hora,responsavel,observacao,prod_1_peso,prod_2_peso,prod_3_peso,prod_4_peso,prod_5_peso,prod_6_peso,prod_7_peso,prod_8_peso,prod_9_peso,prod_10_peso,prod_11_peso,prod_12_peso,prod_13_peso,
                                                            prod_14_peso,prod_15_peso,prod_16_peso,prod_17_peso,prod_18_peso,prod_19_peso,prod_20_peso,prod_21_peso,prod_22_peso,prod_23_peso,prod_24_peso,valida)
                                                            SELECT
                                                            dia,hora,responsavel,observacao,prod_1_peso,prod_2_peso,prod_3_peso,prod_4_peso,prod_5_peso,prod_6_peso,prod_7_peso,prod_8_peso,prod_9_peso,prod_10_peso,prod_11_peso,prod_12_peso,prod_13_peso,
                                                            prod_14_peso,prod_15_peso,prod_16_peso,prod_17_peso,prod_18_peso,prod_19_peso,prod_20_peso,prod_21_peso,prod_22_peso,prod_23_peso,prod_24_peso,FALSE 
                                                            from cadastro.ultimat1";


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
                                            //pictureBox1.Refresh();
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
                                        //pictureBox1.Refresh();
                                    }

                                    String Asql1 = "drop table cadastro.tempat1";
                                    MySqlCommand A0Comando1 = new MySqlCommand(Asql1, Conexao);
                                    A0Comando1.ExecuteNonQuery();

                                    //DateTime timeStamp = DateTime.Now;

                                    int AnoUltimo = Convert.ToInt16(UltAtualiza.Year);
                                    int MesUltimo = Convert.ToInt16(UltAtualiza.Month);

                                    AttOK = false;
                                    //pictureBox1.Refresh();

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


                                    //pictureBox1.Refresh();
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
                                // pictureBox1.Refresh();
                                Statusbox.Text = str1 + " " + "Atualizado";
                                Statusbox.Refresh();

                                //AntigoX
                                /*String Atsql1 = @"Create Temporary Table cadastro.Tempat1(Dia varchar(10), Hora time, Nome varchar(30), Form1 int, Form2 int, Prod_1 int, Prod_2 int, Prod_3 int, Prod_4 int, Prod_5 int, Prod_6 int, Prod_7 int, Prod_8 int, Prod_9 int, Prod_10 int, 
                                                Prod_11 int, Prod_12 int, Prod_13 int, Prod_14 int, Prod_15 int, Prod_16 int, Prod_17 int, Prod_18 int, Prod_19 int, Prod_20 int, 
                                                Prod_21 int, Prod_22 int, Prod_23 int, Prod_24 int, Prod_25 int, Prod_26 int, Prod_27 int, Prod_28 int, Prod_29 int, Prod_30 int, 
                                                Prod_31 int, Prod_32 int, Prod_33 int, Prod_34 int, Prod_35 int, Prod_36 int, Prod_37 int, Prod_38 int, Prod_39 int, Prod_40 int,
                                                Prod_41 int,Prod_42 int,Prod_43 int,Prod_44 int,Prod_45 int,Prod_46 int,Prod_47 int,Prod_48 int,Prod_49 int,Prod_50 int,
                                                Prod_51 int,Prod_52 int,Prod_53 int,Prod_54 int,Prod_55 int,Prod_56 int,Prod_57 int,Prod_58 int,Prod_59 int,Prod_60 int,
                                                Prod_61 int,Prod_62 int,Prod_63 int,Prod_64 int,Prod_65 int)";
                                */

                                String Atsql1 = @"Create Temporary Table cadastro.Tempat1(dia varchar(10),hora TIME,responsavel varchar(50),observacao varchar(500),prod_1_peso INT,prod_2_peso INT,prod_3_peso INT,prod_4_peso INT,prod_5_peso INT,prod_6_peso INT,prod_7_peso INT,
                                                       prod_8_peso INT,prod_9_peso INT,prod_10_peso INT,prod_11_peso INT,prod_12_peso INT,prod_13_peso INT,prod_14_peso INT,prod_15_peso INT,prod_16_peso INT,prod_17_peso INT,prod_18_peso INT,prod_19_peso INT,prod_20_peso INT,prod_21_peso INT,
                                                       prod_22_peso INT,prod_23_peso INT,prod_24_peso INT)";

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
                                    String Atsql4 = "Create Temporary Table cadastro.ultimat1 select * from cadastro.pesagemcsv where month(str_to_date(dia, '%d/%m/%Y')) = @data1 and year(str_to_date(dia, '%d/%m/%Y')) = @data2";
                                    MySqlCommand AtComando4 = new MySqlCommand(Atsql4, Conexao);
                                    AtComando4.Parameters.AddWithValue("@data1", str3);
                                    AtComando4.Parameters.AddWithValue("@data2", str2);
                                    AtComando4.ExecuteNonQuery();
                                    //pictureBox1.Refresh();
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

                                        //pictureBox1.Refresh();
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
                                            //pictureBox1.Refresh();
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
                                                String Emsql3 = "Create Temporary Table cadastro.recorte1 select * from cadastro.pesagemcsv where day(str_to_date(dia, '%d/%m/%Y')) = @dado4 and month(str_to_date(dia, '%d/%m/%Y')) = @dado5 and year(str_to_date(dia, '%d/%m/%Y')) = @dado6";
                                                MySqlCommand Ecomando3 = new MySqlCommand(Emsql3, Conexao);
                                                Ecomando3.Parameters.AddWithValue("@dado4", x.ToString()); //Dia
                                                Ecomando3.Parameters.AddWithValue("@dado5", str3); //Mes
                                                Ecomando3.Parameters.AddWithValue("@dado6", str2); //Ano
                                                Ecomando3.ExecuteNonQuery();
                                                //pictureBox1.Refresh();
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
                                                //AntigoX
                                                /*String Emsql4 = @"Create Temporary Table cadastro.Tempos SELECT b.Dia,b.Hora,b.Nome,b.Form1,b.Form2,b.Prod_1,b.Prod_2,b.Prod_3,b.Prod_4,b.Prod_5,b.Prod_6,b.Prod_7,b.Prod_8,b.Prod_9,b.Prod_10,
                                                                 b.Prod_11,b.Prod_12,b.Prod_13,b.Prod_14,b.Prod_15,b.Prod_16,b.Prod_17,b.Prod_18,b.Prod_19,b.Prod_20,
                                                                 b.Prod_21,b.Prod_22,b.Prod_23,b.Prod_24,b.Prod_25,b.Prod_26,b.Prod_27,b.Prod_28,b.Prod_29,b.Prod_30,
                                                                 b.Prod_31,b.Prod_32,b.Prod_33,b.Prod_34,b.Prod_35,b.Prod_36,b.Prod_37,b.Prod_38,b.Prod_39,b.Prod_40,
                                                                 b.Prod_41,b.Prod_42,b.Prod_43,b.Prod_44,b.Prod_45,b.Prod_46,b.Prod_47,b.Prod_48,b.Prod_49,b.Prod_50,
                                                                 b.Prod_51,b.Prod_52,b.Prod_53,b.Prod_54,b.Prod_55,b.Prod_56,b.Prod_57,b.Prod_58,b.Prod_59,b.Prod_60,
                                                                 b.Prod_61,b.Prod_62,b.Prod_63,b.Prod_64,b.Prod_65 
                                                                 FROM cadastro.nova1 as b LEFT JOIN cadastro.recorte1 as a on b.Dia = a.Dia and b.Hora = a.Hora WHERE a.Dia is null";
                                                */
                                                String Emsql4 = @"Create Temporary Table cadastro.Tempos SELECT b.dia,b.hora,b.responsavel,b.observacao,b.prod_1_peso,b.prod_2_peso,b.prod_3_peso,b.prod_4_peso,b.prod_5_peso,b.prod_6_peso,
                                                                    b.prod_7_peso,b.prod_8_peso,b.prod_9_peso,b.prod_10_peso,b.prod_11_peso,b.prod_12_peso,b.prod_13_peso,b.prod_14_peso,b.prod_15_peso,b.prod_16_peso,b.prod_17_peso,b.prod_18_peso,
                                                                    b.prod_19_peso,b.prod_20_peso,b.prod_21_peso,b.prod_22_peso,b.prod_23_peso,b.prod_24_peso
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
                                                    //AntigoX
                                                    //String Emsql6 = "Insert into cadastro.relatorio select * from cadastro.Tempos";
                                                    String Emsql6 = @"INSERT INTO pesagemcsv(
                                                            dia,hora,responsavel,observacao,prod_1_peso,prod_2_peso,prod_3_peso,prod_4_peso,prod_5_peso,prod_6_peso,prod_7_peso,prod_8_peso,prod_9_peso,prod_10_peso,prod_11_peso,prod_12_peso,prod_13_peso,
                                                            prod_14_peso,prod_15_peso,prod_16_peso,prod_17_peso,prod_18_peso,prod_19_peso,prod_20_peso,prod_21_peso,prod_22_peso,prod_23_peso,prod_24_peso,valida)
                                                            SELECT
                                                            dia,hora,responsavel,observacao,prod_1_peso,prod_2_peso,prod_3_peso,prod_4_peso,prod_5_peso,prod_6_peso,prod_7_peso,prod_8_peso,prod_9_peso,prod_10_peso,prod_11_peso,prod_12_peso,prod_13_peso,
                                                            prod_14_peso,prod_15_peso,prod_16_peso,prod_17_peso,prod_18_peso,prod_19_peso,prod_20_peso,prod_21_peso,prod_22_peso,prod_23_peso,prod_24_peso,FALSE 
                                                            from cadastro.Tempos";

                                                    MySqlCommand Ecomando6 = new MySqlCommand(Emsql6, Conexao);
                                                    Ecomando6.ExecuteNonQuery();
                                                }
                                                //pictureBox1.Refresh();
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
                                        //String A1Sql1 = "Insert into cadastro.relatorio select * from cadastro.Tempat1";
                                        String A1Sql1 = @"INSERT INTO cadastro.pesagemcsv(
                                                         dia,hora,responsavel,observacao,prod_1_peso,prod_2_peso,prod_3_peso,prod_4_peso,prod_5_peso,prod_6_peso,prod_7_peso,prod_8_peso,prod_9_peso,prod_10_peso,prod_11_peso,prod_12_peso,prod_13_peso,
                                                         prod_14_peso,prod_15_peso,prod_16_peso,prod_17_peso,prod_18_peso,prod_19_peso,prod_20_peso,prod_21_peso,prod_22_peso,prod_23_peso,prod_24_peso,valida)
                                                         SELECT
                                                         dia,hora,responsavel,observacao,prod_1_peso,prod_2_peso,prod_3_peso,prod_4_peso,prod_5_peso,prod_6_peso,prod_7_peso,prod_8_peso,prod_9_peso,prod_10_peso,prod_11_peso,prod_12_peso,prod_13_peso,
                                                         prod_14_peso,prod_15_peso,prod_16_peso,prod_17_peso,prod_18_peso,prod_19_peso,prod_20_peso,prod_21_peso,prod_22_peso,prod_23_peso,prod_24_peso,FALSE 
                                                         from cadastro.Tempat1";
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

                                        //pictureBox1.Refresh();
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

                                    //pictureBox1.Refresh();
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

                                //pictureBox1.Refresh();


                            }
                        } //Fim Atualização com cadastro no banco
                    }
                }


            }

            Conexao.Close();
            Statusbox.Text = "Atualizado com sucesso!";
            Statusbox2.Text = "";
            //pictureBox1.Refresh();
            //InitializeTimer2();

        }

        private void AtualizaLoteAntigo()
        {
            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            int UltTabela = 0;

            DataTable tabelanomes = new DataTable();

            uint TotalRegistros;

            //pictureBox1.Refresh();

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);


            /*
            listView1.View = View.Details;
            listView1.Columns.Add("Arquivo", 160);
            listView1.Columns.Add("Status", 80);
            listView1.Columns.Add("Ultima Atualização", 160);
            listView1.Columns.Add("Detalhes", 160);
            */

            //Lendo diretorio de arquivos
            string[] arquivos = Directory.GetFiles(Properties.Settings.Default.PathCSV, "Lote*", SearchOption.AllDirectories)
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
                        Statusbox.Text = "Registro de Lote Atualizado";
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
                        Statusbox.Text = "Atualizando Lote";
                        Statusbox.Refresh();
                        //pictureBox1.Refresh();

                        /*
                        String Atsql1 = @"Create Temporary Table cadastro.Tempform1(Dia varchar(10), Hora varchar(10), Nome varchar(30), Form1 int, Form2 int, Prod_1 int, Prod_2 int, Prod_3 int, Prod_4 int, Prod_5 int, Prod_6 int, Prod_7 int, Prod_8 int, Prod_9 int, Prod_10 int, 
                                        Prod_11 int, Prod_12 int, Prod_13 int, Prod_14 int, Prod_15 int, Prod_16 int, Prod_17 int, Prod_18 int, Prod_19 int, Prod_20 int, 
                                        Prod_21 int, Prod_22 int, Prod_23 int, Prod_24 int, Prod_25 int, Prod_26 int, Prod_27 int, Prod_28 int, Prod_29 int, Prod_30 int, 
                                        Prod_31 int, Prod_32 int, Prod_33 int, Prod_34 int, Prod_35 int, Prod_36 int, Prod_37 int, Prod_38 int, Prod_39 int, Prod_40 int,
                                        Prod_41 int,Prod_42 int,Prod_43 int,Prod_44 int,Prod_45 int,Prod_46 int,Prod_47 int,Prod_48 int,Prod_49 int,Prod_50 int,
                                        Prod_51 int,Prod_52 int,Prod_53 int,Prod_54 int,Prod_55 int,Prod_56 int,Prod_57 int,Prod_58 int,Prod_59 int,Prod_60 int,
                                        Prod_61 int,Prod_62 int,Prod_63 int,Prod_64 int,Prod_65 int)";
                        */
                        String Atsql1 = @"Create Temporary Table cadastro.Tempform1(dia varchar(10),hora TIME,responsavel varchar(50),observacao varchar(500),prod_1_peso INT,prod_2_peso INT,prod_3_peso INT,prod_4_peso INT,prod_5_peso INT,prod_6_peso INT,prod_7_peso INT,
                                        prod_8_peso INT,prod_9_peso INT,prod_10_peso INT,prod_11_peso INT,prod_12_peso INT,prod_13_peso INT,prod_14_peso INT,prod_15_peso INT,prod_16_peso INT,prod_17_peso INT,prod_18_peso INT,prod_19_peso INT,prod_20_peso INT,prod_21_peso INT,
                                        prod_22_peso INT,prod_23_peso INT,prod_24_peso INT)";



                        MySqlCommand AtComando1 = new MySqlCommand(Atsql1, Conexao);
                        AtComando1.ExecuteNonQuery();

                        String ArquivoCSV = arquivos[0].Replace("\\", "/");
                        String Spath = "'" + ArquivoCSV + "'";
                        String Atsql2 = "LOAD DATA LOCAL INFILE " + Spath + " INTO TABLE cadastro.Tempform1 FIELDS TERMINATED BY ',' LINES TERMINATED BY '\\r\\n'";
                        MySqlCommand AtComando2 = new MySqlCommand(Atsql2, Conexao);
                        AtComando2.ExecuteNonQuery();

                        /*
                        String Emsql4 = @"Create Temporary Table cadastro.TemposForm SELECT b.Dia,b.Hora,b.Nome,b.Form1,b.Form2,b.Prod_1,b.Prod_2,b.Prod_3,b.Prod_4,b.Prod_5,b.Prod_6,b.Prod_7,b.Prod_8,b.Prod_9,b.Prod_10,
                                          b.Prod_11,b.Prod_12,b.Prod_13,b.Prod_14,b.Prod_15,b.Prod_16,b.Prod_17,b.Prod_18,b.Prod_19,b.Prod_20,
                                          b.Prod_21,b.Prod_22,b.Prod_23,b.Prod_24,b.Prod_25,b.Prod_26,b.Prod_27,b.Prod_28,b.Prod_29,b.Prod_30,
                                          b.Prod_31,b.Prod_32,b.Prod_33,b.Prod_34,b.Prod_35,b.Prod_36,b.Prod_37,b.Prod_38,b.Prod_39,b.Prod_40
                                          b.Prod_41,b.Prod_42,b.Prod_43,b.Prod_44,b.Prod_45,b.Prod_46,b.Prod_47,b.Prod_48,b.Prod_49,b.Prod_50,
                                          b.Prod_51,b.Prod_52,b.Prod_53,b.Prod_54,b.Prod_55,b.Prod_56,b.Prod_57,b.Prod_58,b.Prod_59,b.Prod_60,
                                          b.Prod_61,b.Prod_62,b.Prod_63,b.Prod_64,b.Prod_65 
                                          FROM cadastro.Tempform1 as b LEFT JOIN cadastro.formulaideal as a on b.Dia = a.Dia and b.Hora = a.Hora WHERE a.Dia is null";
                        */
                        String Emsql4 = @"Create Temporary Table cadastro.Tempos SELECT b.dia,b.hora,b.responsavel,b.observacao,b.prod_1_peso,b.prod_2_peso,b.prod_3_peso,b.prod_4_peso,b.prod_5_peso,b.prod_6_peso,
                                          b.prod_7_peso,b.prod_8_peso,b.prod_9_peso,b.prod_10_peso,b.prod_11_peso,b.prod_12_peso,b.prod_13_peso,b.prod_14_peso,b.prod_15_peso,b.prod_16_peso,b.prod_17_peso,b.prod_18_peso,
                                          b.prod_19_peso,b.prod_20_peso,b.prod_21_peso,b.prod_22_peso,b.prod_23_peso,b.prod_24_peso
                                          FROM cadastro.Tempform1 as b LEFT JOIN cadastro.lotecsv as a on b.Dia = a.Dia and b.Hora = a.Hora WHERE a.Dia is null";




                        MySqlCommand Ecomando4 = new MySqlCommand(Emsql4, Conexao);
                        Ecomando4.ExecuteNonQuery();

                        String Emsql5 = "Select Count(*) from cadastro.TemposForm";
                        MySqlCommand Ecomando5 = new MySqlCommand(Emsql5, Conexao);
                        Object Elinhas5;
                        Elinhas5 = Ecomando5.ExecuteScalar();
                        uint ELinhasInt5;
                        ELinhasInt5 = Convert.ToUInt16(Elinhas5);
                        //pictureBox1.Refresh();
                        //Coloca os dados no relatorio
                        if (ELinhasInt5 != 0)//Sessão A5 
                        {

                            String Emsql6 = "Insert into cadastro.lotecsv select * from cadastro.temposForm";
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

                    Statusbox.Text = "Atualizando Lote";
                    Statusbox.Refresh();
                    //pictureBox1.Refresh();
                    /*
                    String Atsql1 = @"Create Temporary Table cadastro.Tempform1(Dia varchar(10), Hora time, Nome varchar(30), Form1 int, Form2 int, Prod_1 int, Prod_2 int, Prod_3 int, Prod_4 int, Prod_5 int, Prod_6 int, Prod_7 int, Prod_8 int, Prod_9 int, Prod_10 int, 
                                    Prod_11 int, Prod_12 int, Prod_13 int, Prod_14 int, Prod_15 int, Prod_16 int, Prod_17 int, Prod_18 int, Prod_19 int, Prod_20 int, 
                                    Prod_21 int, Prod_22 int, Prod_23 int, Prod_24 int, Prod_25 int, Prod_26 int, Prod_27 int, Prod_28 int, Prod_29 int, Prod_30 int, 
                                    Prod_31 int, Prod_32 int, Prod_33 int, Prod_34 int, Prod_35 int, Prod_36 int, Prod_37 int, Prod_38 int, Prod_39 int, Prod_40 int,
                                    Prod_41 int, Prod_42 int, Prod_43 int, Prod_44 int, Prod_45 int, Prod_46 int, Prod_47 int, Prod_48 int, Prod_49 int, Prod_50 int,
                                    Prod_51 int, Prod_52 int, Prod_53 int, Prod_54 int, Prod_55 int, Prod_56 int, Prod_57 int, Prod_58 int, Prod_59 int, Prod_60 int,
                                    Prod_61 int, Prod_62 int, Prod_63 int, Prod_64 int, Prod_65 int)";
                    */
                    String Atsql1 = @"Create Temporary Table cadastro.Tempform1(dia varchar(10),hora TIME,responsavel varchar(50),observacao varchar(500),prod_1_peso INT,prod_2_peso INT,prod_3_peso INT,prod_4_peso INT,prod_5_peso INT,prod_6_peso INT,prod_7_peso INT,
                                    prod_8_peso INT,prod_9_peso INT,prod_10_peso INT,prod_11_peso INT,prod_12_peso INT,prod_13_peso INT,prod_14_peso INT,prod_15_peso INT,prod_16_peso INT,prod_17_peso INT,prod_18_peso INT,prod_19_peso INT,prod_20_peso INT,prod_21_peso INT,
                                    prod_22_peso INT,prod_23_peso INT,prod_24_peso INT)";




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

                    //pictureBox1.Refresh();
                    //Coloca os dados no relatorio
                    if (ELinhasInt5 != 0)//Sessão A5 
                    {

                        String Emsql6 = "Insert into cadastro.lotecsv select * from cadastro.Tempform1";
                        MySqlCommand Ecomando6 = new MySqlCommand(Emsql6, Conexao);
                        Ecomando6.ExecuteNonQuery();

                        String Dropsql3 = "drop table cadastro.Tempform1";
                        MySqlCommand Dropcomando3 = new MySqlCommand(Dropsql3, Conexao);
                        Dropcomando3.ExecuteNonQuery();

                        String SSqlX1 = "Insert into cadastro.nomearq values(@codex,@anox,@mesx,@arquivox,@ultima_arqx,@ultima_banx,@pesox,@atokx)";
                        MySqlCommand XSComando1 = new MySqlCommand(SSqlX1, Conexao);
                        XSComando1.Parameters.AddWithValue("@codex", "1");
                        XSComando1.Parameters.AddWithValue("@anox", "0");
                        XSComando1.Parameters.AddWithValue("@mesx", "0");
                        XSComando1.Parameters.AddWithValue("@arquivox", "Lote.csv");
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

        public void ObterInformacao2()
        {

            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            List<string> liArquivos = new List<string>();
            //Cria comunicação com o servidor
            //Definir o diretório a ser listado
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(@"ftp://" + Properties.Settings.Default.IP + "/InternalStorage/data/");
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



            //Responder a lista dos arquivos
            foreach (string item in liArquivos)
            {

                int Limite = item.LastIndexOf(".csv");
                int Letra1 = item.IndexOf("Pesagem_20");

                //FASE 1 - Teste para ver se o arquivo é legitimo em quantidade caracteres 
                if (Limite - Letra1 == 15)
                {
                    String str1 = item.Substring(Letra1, 19); //Nome Arquivo Completo
                    String str2 = item.Substring(Letra1 + 8, 4); //Ano do Arquivo
                    String str3 = item.Substring(Letra1 + 13, 2); //Mes do Arquivo
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

                if (item == "Lote.csv")
                {
                    ArquivosnoFTP.Add(item);
                }


            }




            BaixaArquivo();

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
                        Proximo = false;
                    }

                    if (Proximo == false)
                    {
                        //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                        //Cria comunicação com o servidor
                        //definindo o arquivo para download
                        FtpWebRequest request = (FtpWebRequest)WebRequest.Create(@"ftp://" + Properties.Settings.Default.IP + "/InternalStorage/data/" + item);
                        //FtpWebRequest request = (FtpWebRequest)WebRequest.Create(@"ftp://" + Properties.Settings.Default.IP + "/usb1/"+item);
                        //Define que a ação vai ser de download
                        request.Method = WebRequestMethods.Ftp.DownloadFile;
                        //Credenciais para o login (usuario, senha)
                        request.Credentials = new NetworkCredential(Properties.Settings.Default.UserFTP, Properties.Settings.Default.SenhaFTP);
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

                        SetStatusTextSafe("Baixando arquivo " + item);
                        //pictureBox1.Refresh();


                        while (readCount > 0)
                        {
                            //Escrever o arquivo
                            newFile.Write(buffer, 0, readCount);
                            readCount = responseStream.Read(buffer, 0, buffer.Length);
                            bytesReceived += readCount;
                            bytesconvertido = bytesReceived / 100;
                            SetStatus2TextSafe(bytesconvertido.ToString() + "Kb");
                            //pictureBox1.Refresh();
                        }
                        newFile.Close();
                        responseStream.Close();
                        response.Close();

                    }

                }
                SetStatus2TextSafe("");
            }
            ArquivosnoFTP.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Baixa Arquivo FTP
            _ = AtualizaCicloAsync();
        }

        private async Task AtualizaCicloAsync()
        {
            if (_isUpdating)
                return;

            _isUpdating = true;

            try
            {
                await Task.Run(() => ObterInformacao2());
                await Task.Run(() => AtualizarBancoPesagem());
                await Task.Run(() => AtualizaLote());
                await Task.Run(() => CreateLabelsFromDatas());
                await RefreshDateDropdownsAfterUpdateAsync();
            }
            catch (Exception ex)
            {
                ShowMessageSafe($"Erro durante atualização: {ex.Message}");
            }
            finally
            {
                _isUpdating = false;
            }
        }

        private void SetStatusTextSafe(string text)
        {
            if (Statusbox.InvokeRequired)
            {
                Statusbox.BeginInvoke((Action)(() =>
                {
                    Statusbox.Text = text;
                    Statusbox.Refresh();
                }));
                return;
            }

            Statusbox.Text = text;
            Statusbox.Refresh();
        }

        private void SetStatus2TextSafe(string text)
        {
            if (Statusbox2.InvokeRequired)
            {
                Statusbox2.BeginInvoke((Action)(() =>
                {
                    Statusbox2.Text = text;
                    Statusbox2.Refresh();
                }));
                return;
            }

            Statusbox2.Text = text;
            Statusbox2.Refresh();
        }

        private void ShowMessageSafe(string message)
        {
            if (InvokeRequired)
            {
                BeginInvoke((Action)(() => MessageBox.Show(message)));
                return;
            }

            MessageBox.Show(message);
        }

        private void RefreshDateDropdowns(List<string> dias)
        {
            var selectedDataInicial = comboBoxDatas.SelectedItem?.ToString();
            var selectedDataFinal = comboBoxDatas2.SelectedItem?.ToString();

            _Dias = dias ?? new List<string>();

            var Datas = new List<string>(_Dias);
            var Datas2 = new List<string>(_Dias);
            Datas2.Insert(0, "");

            comboBoxDatas.DataSource = null;
            comboBoxDatas2.DataSource = null;

            comboBoxDatas.DataSource = Datas;
            comboBoxDatas2.DataSource = Datas2;

            if (!string.IsNullOrEmpty(selectedDataInicial) && Datas.Contains(selectedDataInicial))
                comboBoxDatas.SelectedItem = selectedDataInicial;

            if (!string.IsNullOrEmpty(selectedDataFinal) && Datas2.Contains(selectedDataFinal))
                comboBoxDatas2.SelectedItem = selectedDataFinal;

            DefineComboBoxSearch();
        }

        private async Task RefreshDateDropdownsAfterUpdateAsync()
        {
            var dias = await Task.Run(() => _labelRepository.GetAllDates());

            if (IsDisposed)
                return;

            if (InvokeRequired)
            {
                BeginInvoke((Action)(() => RefreshDateDropdowns(dias)));
                return;
            }

            RefreshDateDropdowns(dias);
        }

        private void CreateLabelsFromDatas()
        {
            var pesos = _labelRepository.GetAllPesagemInvalid();
            var produtoNomes = _labelRepository.GetMateriaPrima();

            foreach (var peso in pesos)
            {
                var lote = _labelRepository.GetLoteFromDate(peso.Dia, peso.Hora);
                var label = FactoryLabel.Create(produtoNomes, peso, lote);
                _labelRepository.InsertNewLabel(label);
                _labelRepository.UpdatePesagemForValid(peso.Dia, peso.Hora);
            }
        }

        private void comboBoxDatas_SelectedIndexChanged(object sender, EventArgs e)
        {
            var dataSelect = comboBoxDatas.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(dataSelect) || _Dias == null || _Dias.Count == 0)
            {
                comboBoxDatas2.DataSource = new List<string>() { "" };
                return;
            }

            var index = _Dias.IndexOf(dataSelect);
            if (index >= 0)
            {
                var Datas2 = _Dias.GetRange(index + 1, _Dias.Count - index - 1);
                Datas2.Insert(0, "");

                comboBoxDatas2.DataSource = Datas2;
            }
        }

        private void radioNumeroFormula_Click(object sender, EventArgs e)
        {
            DefineComboBoxSearch();
        }

        private void radioNomeFormula_CheckedChanged(object sender, EventArgs e)
        {
            DefineComboBoxSearch();
        }

        private void radioCodigoFormula_CheckedChanged(object sender, EventArgs e)
        {
            DefineComboBoxSearch();
        }

        private const int TOTAL_PRODUTOS = 24;

        public void AtualizarBancoPesagem()
        {
            string connectionString =
                $"Server={Properties.Settings.Default.Server};" +
                $"Port={Properties.Settings.Default.Port};" +
                $"Database=cadastro;" +
                $"Uid={Properties.Settings.Default.User};" +
                $"Pwd={Properties.Settings.Default.Senha};";

            using (var conexao = new MySqlConnection(connectionString))
            {
                conexao.Open();

                string[] arquivos = Directory
                    .GetFiles(Properties.Settings.Default.PathCSV, "Pesagem_20*.csv");

                if (arquivos.Length == 0)
                {
                    ShowMessageSafe("Nenhum arquivo CSV encontrado.");
                    return;
                }

                foreach (string arquivo in arquivos)
                {
                    ProcessarArquivoCSV(conexao, arquivo);
                }
            }
        }

        private void ProcessarArquivoCSV(MySqlConnection conexao, string arquivo)
        {
            FileInfo info = new FileInfo(arquivo);

            string nomeArquivo = Path.GetFileNameWithoutExtension(arquivo);

            // Valida e extrai ano/mês do formato Pesagem_YYYY_MM
            string[] partes = nomeArquivo.Split('_');
            if (partes.Length != 3)
            {
                SetStatusTextSafe($"Formato de arquivo inválido: {nomeArquivo}");
                return;
            }

            string codigo = partes[1] + partes[2]; // ex: "2025" + "03" = "202503"

            long tamanhoArquivo = info.Length;
            DateTime ultimaAlteracao = info.LastWriteTime;

            if (ArquivoJaProcessado(conexao, codigo, tamanhoArquivo))
            {
                SetStatusTextSafe($"{nomeArquivo} já atualizado.");
                return;
            }

            CriarTabelaTemporaria(conexao);

            CarregarCSV(conexao, arquivo);

            int novosRegistros = InserirRegistrosNovos(conexao);

            AtualizarControleArquivo(conexao, codigo, tamanhoArquivo, ultimaAlteracao);

            SetStatusTextSafe($"{nomeArquivo} atualizado ({novosRegistros} novos registros)");
        }

        private void CriarTabelaTemporaria(MySqlConnection conexao)
        {
            var sb = new StringBuilder();

            sb.AppendLine("CREATE TEMPORARY TABLE IF NOT EXISTS tempat1(");
            sb.AppendLine("dia VARCHAR(10),");
            sb.AppendLine("hora TIME,");
            sb.AppendLine("nome_form VARCHAR(50),");
            sb.AppendLine("numero_form INT,");
            sb.AppendLine("cod_form INT,");
            sb.AppendLine("responsavel VARCHAR(50),");
            sb.AppendLine("observacao VARCHAR(500),");
            sb.AppendLine("ciclo NVARCHAR(15),");

            for (int i = 1; i <= TOTAL_PRODUTOS; i++)
            {
                sb.AppendLine($"prod_{i}_peso INT{(i == TOTAL_PRODUTOS ? "" : ",")}");
            }

            sb.AppendLine(")");

            new MySqlCommand(sb.ToString(), conexao).ExecuteNonQuery();
        }

        private void CarregarCSV(MySqlConnection conexao, string arquivo)
        {
            string caminho = arquivo.Replace("\\", "/");

            string sql = $@"
                            LOAD DATA LOCAL INFILE '{caminho}'
                            INTO TABLE tempat1
                            FIELDS TERMINATED BY ','
                            LINES TERMINATED BY '\r\n'";

            new MySqlCommand(sql, conexao).ExecuteNonQuery();
        }

        private int InserirRegistrosNovos(MySqlConnection conexao)
        {
            var colunas = new StringBuilder();
            var select = new StringBuilder();

            colunas.AppendLine("dia,");
            colunas.AppendLine("hora,");
            colunas.AppendLine("nome_form,");
            colunas.AppendLine("numero_form,");
            colunas.AppendLine("cod_form,");
            colunas.AppendLine("responsavel,");
            colunas.AppendLine("observacao,");
            colunas.AppendLine("ciclo,");

            select.AppendLine("dia,");
            select.AppendLine("hora,");
            select.AppendLine("nome_form,");
            select.AppendLine("numero_form,");
            select.AppendLine("cod_form,");
            select.AppendLine("responsavel,");
            select.AppendLine("observacao,");
            select.AppendLine("ciclo,");

            for (int i = 1; i <= TOTAL_PRODUTOS; i++)
            {
                colunas.AppendLine($"prod_{i}_peso,");
                select.AppendLine($"prod_{i}_peso,");
            }

            colunas.AppendLine("valida");
            select.AppendLine("FALSE");

            string sql = $@"
        INSERT IGNORE INTO pesagemcsv(
            {colunas}
        )
        SELECT
            {select}
        FROM tempat1";

            return new MySqlCommand(sql, conexao).ExecuteNonQuery();
        }

        private bool ArquivoJaProcessado(MySqlConnection conexao, string codigo, long tamanho)
        {
            string sql = "SELECT tamanho FROM nomearq WHERE codigo = @codigo";

            using (var cmd = new MySqlCommand(sql, conexao))
            {
                cmd.Parameters.AddWithValue("@codigo", codigo);

                var result = cmd.ExecuteScalar();

                if (result == null)
                    return false;

                return Convert.ToInt64(result) == tamanho;
            }
        }


        private void AtualizarControleArquivo(MySqlConnection conexao, string codigo, long tamanho, DateTime data)
        {
            int ano = int.Parse(codigo.Substring(0, 4));
            int mes = int.Parse(codigo.Substring(4, 2));

            string update = @"
                    UPDATE nomearq
                    SET 
                        Tamanho = @tamanho,
                        Ultimaarq = @data,
                        Ultimaban = NOW()
                    WHERE Codigo = @codigo";

            using (var cmd = new MySqlCommand(update, conexao))
            {
                cmd.Parameters.AddWithValue("@codigo", codigo);
                cmd.Parameters.AddWithValue("@tamanho", tamanho);
                cmd.Parameters.AddWithValue("@data", data);

                int linhas = cmd.ExecuteNonQuery();

                if (linhas == 0)
                {
                    string insert = @"
                    INSERT INTO nomearq
                    (Codigo, Ano, Mes, Arquivo, Ultimaarq, Ultimaban, Tamanho, Dtok)
                    VALUES
                    (@codigo, @ano, @mes, @arquivo, @data, NOW(), @tamanho, 1)";

                    using (var cmdInsert = new MySqlCommand(insert, conexao))
                    {
                        cmdInsert.Parameters.AddWithValue("@codigo", codigo);
                        cmdInsert.Parameters.AddWithValue("@ano", ano);
                        cmdInsert.Parameters.AddWithValue("@mes", mes);
                        cmdInsert.Parameters.AddWithValue("@arquivo", $"Pesagem_{ano}_{mes:00}.csv");
                        cmdInsert.Parameters.AddWithValue("@data", data);
                        cmdInsert.Parameters.AddWithValue("@tamanho", tamanho);

                        cmdInsert.ExecuteNonQuery();
                    }
                }
            }
        }

        private void AtualizaLote()
        {
            string port = Properties.Settings.Default.Port;
            string user = Properties.Settings.Default.User;
            string password = Properties.Settings.Default.Senha;
            string server = Properties.Settings.Default.Server;

            string connString =
                $"Server={server};port={port};User Id={user};database=cadastro;password={password}";

            string[] arquivos = Directory
                .GetFiles(Properties.Settings.Default.PathCSV, "Lote*", SearchOption.AllDirectories)
                .Where(x => x.EndsWith(".csv", StringComparison.OrdinalIgnoreCase))
                .ToArray();

            if (arquivos.Length == 0)
                return;

            string arquivo = arquivos[0];

            FileInfo fi = new FileInfo(arquivo);

            DateTime ultimaAtualizacao = fi.LastAccessTime;
            long pesoArquivo = fi.Length;

            using (var conexao = new MySqlConnection(connString))
            {
                conexao.Open();

                DataTable tabelaControle = LerControleArquivo(conexao);

                bool precisaAtualizar = true;

                if (tabelaControle.Rows.Count > 0)
                {
                    if (pesoArquivo.ToString() == tabelaControle.Rows[0]["tamanho"].ToString())
                        precisaAtualizar = false;
                }

                if (!precisaAtualizar)
                {
                    SetStatusTextSafe("Registro de Lote Atualizado");
                    return;
                }

                SetStatusTextSafe("Atualizando Lote");

                CriarTabelaTemporariaLote(conexao);

                CarregarCsv(conexao, arquivo);

                if (tabelaControle.Rows.Count > 0)
                    InserirSomenteNovos(conexao);
                else
                    InserirTudo(conexao);

                AtualizarControle(conexao, pesoArquivo, ultimaAtualizacao, tabelaControle.Rows.Count);

                RemoverTabelaTemporaria(conexao);
            }
        }

        private DataTable LerControleArquivo(MySqlConnection conexao)
        {
            DataTable tabela = new DataTable();

            using (var cmd = new MySqlCommand("SELECT * FROM cadastro.nomearq WHERE codigo = 1", conexao))
            using (var adapter = new MySqlDataAdapter(cmd))
            {
                adapter.Fill(tabela);
            }

            return tabela;
        }

        private void CriarTabelaTemporariaLote(MySqlConnection conexao)
        {
            string sql = @"
    CREATE TEMPORARY TABLE temp_lote_csv(
        dia VARCHAR(10),
        hora TIME,
        prod_1_lote INT,
        prod_2_lote INT,
        prod_3_lote INT,
        prod_4_lote INT,
        prod_5_lote INT,
        prod_6_lote INT,
        prod_7_lote INT,
        prod_8_lote INT,
        prod_9_lote INT,
        prod_10_lote INT,
        prod_11_lote INT,
        prod_12_lote INT,
        prod_13_lote INT,
        prod_14_lote INT,
        prod_15_lote INT,
        prod_16_lote INT,
        prod_17_lote INT,
        prod_18_lote INT,
        prod_19_lote INT,
        prod_20_lote INT,
        prod_21_lote INT,
        prod_22_lote INT,
        prod_23_lote INT,
        prod_24_lote INT
    )";

            using (var cmd = new MySqlCommand(sql, conexao))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void CarregarCsv(MySqlConnection conexao, string arquivo)
        {
            string path = arquivo.Replace("\\", "/");

            string sql = $@"
    LOAD DATA LOCAL INFILE '{path}'
    INTO TABLE temp_lote_csv
    FIELDS TERMINATED BY ','
    LINES TERMINATED BY '\r\n'";

            using (var cmd = new MySqlCommand(sql, conexao))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void InserirSomenteNovos(MySqlConnection conexao)
        {
            string sql = @"
    INSERT INTO cadastro.lotecsv
    SELECT t.*
    FROM temp_lote_csv t
    LEFT JOIN cadastro.lotecsv l
    ON t.dia = l.dia AND t.hora = l.hora
        WHERE l.dia IS NULL
            AND NULLIF(TRIM(t.dia), '') IS NOT NULL
            AND t.hora IS NOT NULL";

            using (var cmd = new MySqlCommand(sql, conexao))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void InserirTudo(MySqlConnection conexao)
        {
                        string sql = @"
        INSERT INTO cadastro.lotecsv
        SELECT *
        FROM temp_lote_csv
        WHERE NULLIF(TRIM(dia), '') IS NOT NULL
            AND hora IS NOT NULL";

            using (var cmd = new MySqlCommand(sql, conexao))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void AtualizarControle(MySqlConnection conexao, long peso, DateTime ultimaAtualizacao, int registrosControle)
        {
            if (registrosControle > 0)
            {
                string sql = @"
        UPDATE cadastro.nomearq
        SET tamanho = @peso,
            ultimaarq = @ult_arq,
            ultimaban = @ult_ban
        WHERE codigo = 1";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@peso", peso);
                    cmd.Parameters.AddWithValue("@ult_arq", ultimaAtualizacao);
                    cmd.Parameters.AddWithValue("@ult_ban", DateTime.Now);

                    cmd.ExecuteNonQuery();
                }
            }
            else
            {
                string sql = @"
        INSERT INTO cadastro.nomearq
        (codigo,ano,mes,arquivo,ultimaarq,ultimaban,tamanho,Dtok)
        VALUES
        (1,0,0,'Lote.csv',@ult_arq,@ult_ban,@peso,0)";

                using (var cmd = new MySqlCommand(sql, conexao))
                {
                    cmd.Parameters.AddWithValue("@ult_arq", ultimaAtualizacao);
                    cmd.Parameters.AddWithValue("@ult_ban", DateTime.Now);
                    cmd.Parameters.AddWithValue("@peso", peso);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void RemoverTabelaTemporaria(MySqlConnection conexao)
        {
            using (var cmd = new MySqlCommand("DROP TEMPORARY TABLE IF EXISTS temp_lote_csv", conexao))
            {
                cmd.ExecuteNonQuery();
            }
        }

        private void comboBoxDatas2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }


}
    
    
