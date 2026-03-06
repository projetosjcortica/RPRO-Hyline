using JCortica_RPRO.Leonardo.Domain.Entities.André;
using Leonardo.Domain;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TesteImpresao.Entities;

namespace JCortica_RPRO.Repositories
{
    public class LabelRepository
    {
        private string _connectionString;
        public LabelRepository()
        {
            string Port = Properties.Settings.Default.Port;
            string User = Properties.Settings.Default.User;
            string Password = Properties.Settings.Default.Senha;
            string Server = Properties.Settings.Default.Server;

            _connectionString = $"Server={Server};port={Port};User Id={User};database=cadastro;password={Password}";
        }
        public async Task<LabelZebra> GetLabelById(int Id)
        {
            string sql = "SELECT * from etiqueta WHERE etiqueta_id = @Id";
            List<LabelItem> labelsItems = new List<LabelItem>();
            var label = new LabelZebra();
            var produtosNome = GetMateriaPrima();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Id", Id);

                using (MySqlDataReader queryResult = (MySqlDataReader)await command.ExecuteReaderAsync())
                {
                    if (await queryResult.ReadAsync())
                    {

                        label.SetId(queryResult.GetInt32("etiqueta_id"))
                             .SetResponsavel(queryResult.GetString("responsavel"))
                             .SetObservacao(queryResult.GetString("observacao"))
                             .SetDia(queryResult.GetString("dia"))
                             .SetHora(queryResult.GetTimeSpan("hora"))
                             .SetCodigoFormula(queryResult.GetInt32("cod_form"))
                             .SetNomeFormula(queryResult.GetString("nome_form"))
                             .SetNumeroFormula(queryResult.GetInt32("numero_form"))
                             .SetCiclo(queryResult.GetString("ciclo"));

                        for (int i = 1; i < 25; i++)
                        {
                            try
                            {
                                var labelItem = new LabelItem(queryResult.GetString($"prod_{i}_nome"), queryResult.GetInt32($"prod_{i}_lote"), queryResult.GetInt32($"prod_{i}_peso"));
                                labelsItems.Add(labelItem);
                            }
                            catch (Exception e)
                            {
                                var labelItem = new LabelItem(produtosNome[i - 1]);
                                labelsItems.Add(labelItem);
                            }
                            
                        }
                    }
                    else return null;
                }
            }
            label.SetLabelItem(labelsItems);
            return label;
        }

        public async Task<List<LabelZebra>> GetLabelNotPrinter()
        {
            string sql = "SELECT * from etiqueta WHERE impressa = 0";
            var produtosNome = GetMateriaPrima();

            var labelsList = new List<LabelZebra>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                MySqlCommand command = new MySqlCommand(sql, connection);

                using (MySqlDataReader queryResult = (MySqlDataReader)await command.ExecuteReaderAsync())
                {
                    while (await queryResult.ReadAsync())
                    {
                        var label = new LabelZebra();
                        List<LabelItem> labelsItems = new List<LabelItem>();

                        label.SetId(queryResult.GetInt32("etiqueta_id"))
                             .SetResponsavel(queryResult.GetString("responsavel"))
                             .SetObservacao(queryResult.GetString("observacao"))
                             .SetDia(queryResult.GetString("dia"))
                             .SetHora(queryResult.GetTimeSpan("hora"))
                             .SetCodigoFormula(queryResult.GetInt32("cod_form"))
                             .SetNomeFormula(queryResult.GetString("nome_form"))
                             .SetNumeroFormula(queryResult.GetInt32("numero_form"))
                             .SetCiclo(queryResult.GetString("ciclo"));

                        for (int i = 1; i < 25; i++)
                        {
                            var peso = queryResult.GetInt32($"prod_{i}_peso");
                            if(peso != 0)
                            {
                                var labelItem = new LabelItem(produtosNome[i - 1], queryResult.GetInt32($"prod_{i}_lote"), queryResult.GetInt32($"prod_{i}_peso"));
                                labelsItems.Add(labelItem);
                            }
                        }
                        label.SetLabelItem(labelsItems);
                        labelsList.Add(label);
                    }
                }
            }
           
            return labelsList;
        }

        public async Task<bool> UpdateLabelForPrinter(int id)
        {
            // Criação da conexão dentro de um bloco usando 'using' para garantir o fechamento
            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync();

                    string query = "UPDATE etiqueta SET impressa = TRUE WHERE etiqueta_id = @id";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);

                        int rowsAffected = await cmd.ExecuteNonQueryAsync();

                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    // Caso ocorra algum erro, loga ou trata a exceção
                    Console.WriteLine("Erro ao atualizar o campo 'impressa': " + ex.Message);
                    return false;
                }
            }

        } 

        public List<string> GetMateriaPrima()
        {
            var result = new List<string>();
            string sql = "SELECT * from materiaprima";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand(sql, connection);

                using (MySqlDataReader queryResult = command.ExecuteReader())
                {
                    int MaxIndex = 1;
                    while (queryResult.Read() && MaxIndex <= 24 )
                    {
                        result.Add(queryResult.GetString($"Produto"));
                        MaxIndex++;
                    }
                }
            }

            return result;
        }


        public List<Pesagem> GetAllPesagemInvalid()
        {
            var result = new List<Pesagem>();
            string sql = "SELECT * FROM pesagemcsv WHERE valida = 0";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand(sql, connection);

                using (MySqlDataReader queryResult = command.ExecuteReader())
                {
                    while (queryResult.Read())
                    {
                        var pesos = new List<int>();
                        for (int i = 1; i <= 24; i++)
                        {
                            pesos.Add(queryResult.GetInt32($"prod_{i}_peso"));
                        }

                        var pesagem = new Pesagem
                        {
                            Dia = queryResult.GetString("dia"),
                            Hora = queryResult.GetTimeSpan("hora"),
                            Responsavel = queryResult.GetString("responsavel"),
                            Observacao = queryResult.GetString("observacao"),
                            Valida = queryResult.GetBoolean("valida"),
                            NumeroFormula = queryResult.IsDBNull(queryResult.GetOrdinal("numero_form")) ? 0 : queryResult.GetInt32("numero_form"),
                            CodigoFormula = queryResult.IsDBNull(queryResult.GetOrdinal("cod_form")) ? 0 : queryResult.GetInt32("cod_form"),
                            NomeFormula = queryResult.IsDBNull(queryResult.GetOrdinal("nome_form")) ? string.Empty : queryResult.GetString("nome_form"),
                            Ciclo = queryResult.IsDBNull(queryResult.GetOrdinal("ciclo")) ? string.Empty : queryResult.GetString("ciclo"),
                            Pesos = pesos
                        };

                        result.Add(pesagem);
                    }
                }
            }

            return result;
        }

        public Lotes GetLoteFromDate(string date, TimeSpan hour)
        {
            var sql = @"
                    SELECT *
                    FROM (
                        (
                            SELECT *
                            FROM lotecsv
                            WHERE STR_TO_DATE(CONCAT(dia,' ',hora), '%d/%m/%y %H:%i:%s')
                                  <= STR_TO_DATE(CONCAT(@Dia,' ',@Hora), '%d/%m/%y %H:%i:%s')
                            ORDER BY STR_TO_DATE(CONCAT(dia,' ',hora), '%d/%m/%y %H:%i:%s') DESC
                            LIMIT 1
                        )

                        UNION ALL

                        (
                            SELECT *
                            FROM lotecsv
                            ORDER BY STR_TO_DATE(CONCAT(dia,' ',hora), '%d/%m/%y %H:%i:%s') ASC
                            LIMIT 1
                        )
                    ) t
                    LIMIT 1;
                    ";

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (MySqlCommand command = new MySqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@Dia", date);
                    command.Parameters.AddWithValue("@Hora", hour.ToString(@"hh\:mm\:ss"));

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        var lotes = new List<int>();

                        for (int i = 1; i <= 24; i++)
                        {
                            int index = reader.GetOrdinal($"prod_{i}_lote");

                            if (!reader.IsDBNull(index))
                                lotes.Add(reader.GetInt32(index));
                            else
                                lotes.Add(0);
                        }

                        return new Lotes
                        {
                            Dia = reader.GetString(reader.GetOrdinal("dia")),
                            Hora = reader.GetTimeSpan(reader.GetOrdinal("hora")),
                            NumeroLotes = lotes
                        };
                    }
                }
            }
        }

        public void InsertNewLabel(LabelZebra label)
        {
            var sql = @"INSERT INTO etiqueta(
                        dia, hora, responsavel, observacao,
                        nome_form, numero_form, cod_form, ciclo,
                        prod_1_nome, prod_1_peso, prod_1_lote,
                        prod_2_nome, prod_2_peso, prod_2_lote,
                        prod_3_nome, prod_3_peso, prod_3_lote,
                        prod_4_nome, prod_4_peso, prod_4_lote,
                        prod_5_nome, prod_5_peso, prod_5_lote,
                        prod_6_nome, prod_6_peso, prod_6_lote,
                        prod_7_nome, prod_7_peso, prod_7_lote,
                        prod_8_nome, prod_8_peso, prod_8_lote,
                        prod_9_nome, prod_9_peso, prod_9_lote,
                        prod_10_nome, prod_10_peso, prod_10_lote,
                        prod_11_nome, prod_11_peso, prod_11_lote,
                        prod_12_nome, prod_12_peso, prod_12_lote,
                        prod_13_nome, prod_13_peso, prod_13_lote,
                        prod_14_nome, prod_14_peso, prod_14_lote,
                        prod_15_nome, prod_15_peso, prod_15_lote,
                        prod_16_nome, prod_16_peso, prod_16_lote,
                        prod_17_nome, prod_17_peso, prod_17_lote,
                        prod_18_nome, prod_18_peso, prod_18_lote,
                        prod_19_nome, prod_19_peso, prod_19_lote,
                        prod_20_nome, prod_20_peso, prod_20_lote,
                        prod_21_nome, prod_21_peso, prod_21_lote,
                        prod_22_nome, prod_22_peso, prod_22_lote,
                        prod_23_nome, prod_23_peso, prod_23_lote,
                        prod_24_nome, prod_24_peso, prod_24_lote,
                        impressa
                    ) VALUES(
                        @Dia, @Hora, @Responsavel, @Observacao,
                        @NomeFormula, @NumeroFormula, @CodigoFormula, @Ciclo,
                        @Prod1Nome, @Prod1Peso, @Prod1Lote,
                        @Prod2Nome, @Prod2Peso, @Prod2Lote,
                        @Prod3Nome, @Prod3Peso, @Prod3Lote,
                        @Prod4Nome, @Prod4Peso, @Prod4Lote,
                        @Prod5Nome, @Prod5Peso, @Prod5Lote,
                        @Prod6Nome, @Prod6Peso, @Prod6Lote,
                        @Prod7Nome, @Prod7Peso, @Prod7Lote,
                        @Prod8Nome, @Prod8Peso, @Prod8Lote,
                        @Prod9Nome, @Prod9Peso, @Prod9Lote,
                        @Prod10Nome, @Prod10Peso, @Prod10Lote,
                        @Prod11Nome, @Prod11Peso, @Prod11Lote,
                        @Prod12Nome, @Prod12Peso, @Prod12Lote,
                        @Prod13Nome, @Prod13Peso, @Prod13Lote,
                        @Prod14Nome, @Prod14Peso, @Prod14Lote,
                        @Prod15Nome, @Prod15Peso, @Prod15Lote,
                        @Prod16Nome, @Prod16Peso, @Prod16Lote,
                        @Prod17Nome, @Prod17Peso, @Prod17Lote,
                        @Prod18Nome, @Prod18Peso, @Prod18Lote,
                        @Prod19Nome, @Prod19Peso, @Prod19Lote,
                        @Prod20Nome, @Prod20Peso, @Prod20Lote,
                        @Prod21Nome, @Prod21Peso, @Prod21Lote,
                        @Prod22Nome, @Prod22Peso, @Prod22Lote,
                        @Prod23Nome, @Prod23Peso, @Prod23Lote,
                        @Prod24Nome, @Prod24Peso, @Prod24Lote,
                        @Impressa
                    )";

            using (var connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();

                    using (var command = new MySqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Dia", label.Dia);
                        command.Parameters.AddWithValue("@Hora", label.Hora);
                        command.Parameters.AddWithValue("@Responsavel", label.Responsavel);
                        command.Parameters.AddWithValue("@Observacao", label.Observacao);
                        command.Parameters.AddWithValue("@NomeFormula", label.NomeFormula);
                        command.Parameters.AddWithValue("@NumeroFormula", label.NumeroFormula);
                        command.Parameters.AddWithValue("@CodigoFormula", label.CodigoFormula);
                        command.Parameters.AddWithValue("@Ciclo", label.Ciclo);
                        command.Parameters.AddWithValue("@Impressa", false);

                        for (int i = 0; i < label.LabelItem.Count; i++)
                        {
                            var item = label.LabelItem[i];
                            var paramPrefix = $"@Prod{i + 1}";
                            command.Parameters.AddWithValue($"{paramPrefix}Nome", item.ProductName);
                            command.Parameters.AddWithValue($"{paramPrefix}Peso", item.Weight);
                            command.Parameters.AddWithValue($"{paramPrefix}Lote", item.Lote);
                        }

                        int rowsAffected = command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao atualizar o campo 'impressa': " + ex.Message);
                }

            }
        }

        public void UpdatePesagemForValid(string dia, TimeSpan hora)
        {
            var sql = @"
                UPDATE pesagemcsv SET valida = 1 WHERE dia = @dia AND hora = @hora
            "
            ;
            using (var connection = new MySqlConnection(_connectionString))
            {
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@dia", dia);
                    command.Parameters.AddWithValue("@hora", hora.ToString(@"hh\:mm\:ss"));

                    command.ExecuteNonQuery();
                }

            }
        }

        public List<string> GetAllDates()
        {
            var result = new List<string>();
            var sql = @"
                SELECT dia FROM etiqueta
             ";

            using (var connection = new MySqlConnection(_connectionString))
            {
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();

                    using (MySqlDataReader queryResult = command.ExecuteReader())
                    {
                        while (queryResult.Read())
                        {
                            var dia = queryResult.GetString("dia");

                            if (!result.Contains(dia))
                            {
                                result.Add(dia);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public async Task<List<LabelZebra>> GetLabelFromDate(string Dia)
        {
            string sql = "SELECT * from etiqueta WHERE dia = @Dia";
            var produtosNome = GetMateriaPrima();
            var result = new List<LabelZebra>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@Dia", Dia);

                using (MySqlDataReader queryResult = (MySqlDataReader)await command.ExecuteReaderAsync())
                {
                    while(await queryResult.ReadAsync())
                    {
                        List<LabelItem> labelsItems = new List<LabelItem>();
                        var label = new LabelZebra();

                        label.SetId(queryResult.GetInt32("etiqueta_id"))
                             .SetResponsavel(queryResult.GetString("responsavel"))
                             .SetObservacao(queryResult.GetString("observacao"))
                             .SetDia(queryResult.GetString("dia"))
                             .SetHora(queryResult.GetTimeSpan("hora"))
                             .SetCodigoFormula(queryResult.GetInt32("cod_form"))
                             .SetNomeFormula(queryResult.GetString("nome_form"))
                             .SetNumeroFormula(queryResult.GetInt32("numero_form"))
                             .SetCiclo(queryResult.GetString("ciclo"));

                        for (int i = 1; i < 25; i++)
                        {
                            try
                            {
                                var labelItem = new LabelItem(queryResult.GetString($"prod_{i}_nome"), queryResult.GetInt32($"prod_{i}_lote"), queryResult.GetInt32($"prod_{i}_peso"));
                                labelsItems.Add(labelItem);
                            }
                            catch (Exception e)
                            {
                                var labelItem = new LabelItem(produtosNome[i - 1]);
                                labelsItems.Add(labelItem);
                            }

                        }
                        label.SetLabelItem(labelsItems);
                        result.Add(label);
                    }
                }
            }
            
            return result;
        }

        public async Task<List<LabelZebra>> GetLabelFromPeriod(string DiaInicial, string DiaFinal)
        {
            string sql = @"SELECT * 
                            FROM etiqueta
                            WHERE STR_TO_DATE(dia, '%d/%m/%Y') >= STR_TO_DATE(@DiaInicial, '%d/%m/%Y')
                            AND STR_TO_DATE(dia, '%d/%m/%Y') <= STR_TO_DATE(@DiaFinal, '%d/%m/%Y')";

            var produtosNome = GetMateriaPrima();
            var result = new List<LabelZebra>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@DiaInicial", DiaInicial);
                command.Parameters.AddWithValue("@DiaFinal", DiaFinal);

                using (MySqlDataReader queryResult = (MySqlDataReader)await command.ExecuteReaderAsync())
                {
                    while (await queryResult.ReadAsync())
                    {
                        List<LabelItem> labelsItems = new List<LabelItem>();
                        var label = new LabelZebra();

                        label.SetId(queryResult.GetInt32("etiqueta_id"))
                             .SetResponsavel(queryResult.GetString("responsavel"))
                             .SetObservacao(queryResult.GetString("observacao"))
                             .SetDia(queryResult.GetString("dia"))
                             .SetHora(queryResult.GetTimeSpan("hora"))
                             .SetCodigoFormula(queryResult.GetInt32("cod_form"))
                             .SetNomeFormula(queryResult.GetString("nome_form"))
                             .SetNumeroFormula(queryResult.GetInt32("numero_form"))
                             .SetCiclo(queryResult.GetString("ciclo"));

                        for (int i = 1; i < 25; i++)
                        {
                            try
                            {
                                var labelItem = new LabelItem(queryResult.GetString($"prod_{i}_nome"), queryResult.GetInt32($"prod_{i}_lote"), queryResult.GetInt32($"prod_{i}_peso"));
                                labelsItems.Add(labelItem);
                            }
                            catch (Exception e)
                            {
                                var labelItem = new LabelItem(produtosNome[i - 1]);
                                labelsItems.Add(labelItem);
                            }

                        }
                        label.SetLabelItem(labelsItems);
                        result.Add(label);
                    }
                }
            }

            return result;
        }

        public async Task<List<LabelZebra>> GetLabelAdvancedSearch(string nomeFormula, string numeroFormula, string codigoFormula, string dataInicial, string dataFinal)
        {
            string sql = "SELECT * FROM etiqueta WHERE 1 = 1"; 

            if (!string.IsNullOrEmpty(nomeFormula))
            {
                sql += " AND nome_form LIKE @NomeFormula";
            }

            if (!string.IsNullOrEmpty(numeroFormula))
            {
                sql += " AND numero_form LIKE @NumeroFormula";
            }

            if (!string.IsNullOrEmpty(codigoFormula))
            {
                sql += " AND cod_form LIKE @CodigoFormula";
            }

            if(!string.IsNullOrEmpty(dataInicial) && string.IsNullOrEmpty(dataFinal))
            {
                sql += " AND dia = @DataInicial";
            }

            if(!string.IsNullOrEmpty(dataInicial) && !string.IsNullOrEmpty(dataFinal))
            {
                sql += " AND STR_TO_DATE(dia, '%d/%m/%Y') >= STR_TO_DATE(@DataInicial, '%d/%m/%Y')";
                sql += " AND STR_TO_DATE(dia, '%d/%m/%Y') <= STR_TO_DATE(@DataFinal, '%d/%m/%Y')";
            }

            var produtosNome = GetMateriaPrima();
            var result = new List<LabelZebra>();

            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@NomeFormula", $"%{nomeFormula}%");
                command.Parameters.AddWithValue("@NumeroFormula", $"%{numeroFormula}%");
                command.Parameters.AddWithValue("@CodigoFormula", $"%{codigoFormula}%");
                command.Parameters.AddWithValue("@DataInicial", dataInicial);
                command.Parameters.AddWithValue("@DataFinal", dataFinal);


                using (MySqlDataReader queryResult = (MySqlDataReader)await command.ExecuteReaderAsync())
                {
                    while (await queryResult.ReadAsync())
                    {
                        List<LabelItem> labelsItems = new List<LabelItem>();
                        var label = new LabelZebra();

                        label.SetId(queryResult.GetInt32("etiqueta_id"))
                             .SetResponsavel(queryResult.GetString("responsavel"))
                             .SetObservacao(queryResult.GetString("observacao"))
                             .SetDia(queryResult.GetString("dia"))
                             .SetHora(queryResult.GetTimeSpan("hora"))
                             .SetCodigoFormula(queryResult.GetInt32("cod_form"))
                             .SetNomeFormula(queryResult.GetString("nome_form"))
                             .SetNumeroFormula(queryResult.GetInt32("numero_form"))
                             .SetCiclo(queryResult.GetString("ciclo"));

                        for (int i = 1; i < 25; i++)
                        {
                            try
                            {
                                var labelItem = new LabelItem(queryResult.GetString($"prod_{i}_nome"), queryResult.GetInt32($"prod_{i}_lote"), queryResult.GetInt32($"prod_{i}_peso"));
                                labelsItems.Add(labelItem);
                            }
                            catch (Exception e)
                            {
                                var labelItem = new LabelItem(produtosNome[i - 1]);
                                labelsItems.Add(labelItem);
                            }

                        }
                        label.SetLabelItem(labelsItems);
                        result.Add(label);
                    }
                }
            }

            return result;
        }

        public List<string> GetNomeFormula(string DataInicial, string DataFinal)
        {
            var result = new List<string>();
            var sql = @"
                SELECT nome_form FROM etiqueta WHERE 1 = 1
             ";

            if(string.IsNullOrEmpty(DataFinal))
            {
                sql += " AND dia = @DataInicial";
            }
            else
            {
                sql += " AND STR_TO_DATE(dia, '%d/%m/%Y') >= STR_TO_DATE(@DataInicial, '%d/%m/%Y')";
                sql += " AND STR_TO_DATE(dia, '%d/%m/%Y') <= STR_TO_DATE(@DataFinal, '%d/%m/%Y')";
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();

                    command.Parameters.AddWithValue("@DataInicial", DataInicial);
                    command.Parameters.AddWithValue("@DataFinal", DataFinal);

                    using (MySqlDataReader queryResult = command.ExecuteReader())
                    {
                        while (queryResult.Read())
                        {
                            var nomeFormula = queryResult.GetString("nome_form");

                            if (!result.Contains(nomeFormula))
                            {
                                result.Add(nomeFormula);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public List<string> GetCodigoFormula(string DataInicial, string DataFinal)
        {
            var result = new List<string>();
            var sql = @"
                SELECT cod_form FROM etiqueta WHERE 1 = 1 
             ";

            if (string.IsNullOrEmpty(DataFinal))
            {
                sql += " AND dia = @DataInicial";
            }
            else
            {
                sql += " AND STR_TO_DATE(dia, '%d/%m/%Y') >= STR_TO_DATE(@DataInicial, '%d/%m/%Y')";
                sql += " AND STR_TO_DATE(dia, '%d/%m/%Y') <= STR_TO_DATE(@DataFinal, '%d/%m/%Y')";
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@DataInicial", DataInicial);
                    command.Parameters.AddWithValue("@DataFinal", DataFinal);


                    using (MySqlDataReader queryResult = command.ExecuteReader())
                    {
                        while (queryResult.Read())
                        {
                            var codigoFormula = queryResult.GetString("cod_form");

                            if (!result.Contains(codigoFormula))
                            {
                                result.Add(codigoFormula);
                            }
                        }
                    }
                }
            }

            return result;
        }

        public List<string> GetNumeroFormula(string DataInicial, string DataFinal)
        {
            var result = new List<string>();
            var sql = @"
                SELECT numero_form FROM etiqueta WHERE 1 = 1
             ";

            if (string.IsNullOrEmpty(DataFinal))
            {
                sql += " AND dia = @DataInicial";
            }
            else
            {
                sql += " AND STR_TO_DATE(dia, '%d/%m/%Y') >= STR_TO_DATE(@DataInicial, '%d/%m/%Y')";
                sql += " AND STR_TO_DATE(dia, '%d/%m/%Y') <= STR_TO_DATE(@DataFinal, '%d/%m/%Y')";
            }

            using (var connection = new MySqlConnection(_connectionString))
            {
                using (var command = new MySqlCommand(sql, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@DataInicial", DataInicial);
                    command.Parameters.AddWithValue("@DataFinal", DataFinal);

                    using (MySqlDataReader queryResult = command.ExecuteReader())
                    {
                        while (queryResult.Read())
                        {
                            var numeroFormula = queryResult.GetString("numero_form");

                            if (!result.Contains(numeroFormula))
                            {
                                result.Add(numeroFormula);
                            }
                        }
                    }
                }
            }

            return result;
        }

    }
}
