using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace JCortica_RPRO
{
    public partial class MatPrima : Form
    {
        public MatPrima()
        {
            InitializeComponent();
        }

        private void Editar_MP_Click(object sender, EventArgs e)
        {
            Salvar_MP.Enabled = true;
            Cancelar_MP.Enabled = true;
            destravar_box();

        }

        private void Salvar_MP_Click(object sender, EventArgs e)
        {
            Salvar_Produtos();
            Travar_box();
            Salvar_MP.Enabled = false;
            Cancelar_MP.Enabled = false;
        }


        public void destravar_box()
        {
            TextboxP1.Enabled = true;
            TextboxP2.Enabled = true;
            TextboxP3.Enabled = true;
            TextboxP4.Enabled = true;
            TextboxP5.Enabled = true;
            TextboxP6.Enabled = true;
            TextboxP7.Enabled = true;
            TextboxP8.Enabled = true;
            TextboxP9.Enabled = true;
            TextboxP10.Enabled = true;
            TextboxP11.Enabled = true;
            TextboxP12.Enabled = true;
            TextboxP13.Enabled = true;
            TextboxP14.Enabled = true;
            TextboxP15.Enabled = true;
            TextboxP16.Enabled = true;
            TextboxP17.Enabled = true;
            TextboxP18.Enabled = true;
            TextboxP19.Enabled = true;
            TextboxP20.Enabled = true;
            TextboxP21.Enabled = true;
            TextboxP22.Enabled = true;
            TextboxP23.Enabled = true;
            TextboxP24.Enabled = true;
            
            PainelPL1.Enabled = true;
            PainelPL2.Enabled = true;
            PainelPL3.Enabled = true;
            PainelPL4.Enabled = true;
            PainelPL5.Enabled = true;
            PainelPL6.Enabled = true;
            PainelPL7.Enabled = true;
            PainelPL8.Enabled = true;
            PainelPL9.Enabled = true;
            PainelPL10.Enabled = true;
            PainelPL11.Enabled = true;
            PainelPL12.Enabled = true;
            PainelPL13.Enabled = true;
            PainelPL14.Enabled = true;
            PainelPL15.Enabled = true;
            PainelPL16.Enabled = true;
            PainelPL17.Enabled = true;
            PainelPL18.Enabled = true;
            PainelPL19.Enabled = true;
            PainelPL20.Enabled = true;
            PainelPL21.Enabled = true;
            PainelPL22.Enabled = true;
            PainelPL23.Enabled = true;
            PainelPL24.Enabled = true;
        }

        public void Travar_box()
        {
            TextboxP1.Enabled = false;
            TextboxP2.Enabled = false;
            TextboxP3.Enabled = false;
            TextboxP4.Enabled = false;
            TextboxP5.Enabled = false;
            TextboxP6.Enabled = false;
            TextboxP7.Enabled = false;
            TextboxP8.Enabled = false;
            TextboxP9.Enabled = false;
            TextboxP10.Enabled = false;
            TextboxP11.Enabled = false;
            TextboxP12.Enabled = false;
            TextboxP13.Enabled = false;
            TextboxP14.Enabled = false;
            TextboxP15.Enabled = false;
            TextboxP16.Enabled = false;
            TextboxP17.Enabled = false;
            TextboxP18.Enabled = false;
            TextboxP19.Enabled = false;
            TextboxP20.Enabled = false;
            TextboxP21.Enabled = false;
            TextboxP22.Enabled = false;
            TextboxP23.Enabled = false;
            TextboxP24.Enabled = false;
           
            PainelPL1.Enabled = false;
            PainelPL2.Enabled = false;
            PainelPL3.Enabled = false;
            PainelPL4.Enabled = false;
            PainelPL5.Enabled = false;
            PainelPL6.Enabled = false;
            PainelPL7.Enabled = false;
            PainelPL8.Enabled = false;
            PainelPL9.Enabled = false;
            PainelPL10.Enabled = false;
            PainelPL11.Enabled = false;
            PainelPL12.Enabled = false;
            PainelPL13.Enabled = false;
            PainelPL14.Enabled = false;
            PainelPL15.Enabled = false;
            PainelPL16.Enabled = false;
            PainelPL17.Enabled = false;
            PainelPL18.Enabled = false;
            PainelPL19.Enabled = false;
            PainelPL20.Enabled = false;
            PainelPL21.Enabled = false;
            PainelPL22.Enabled = false;
            PainelPL23.Enabled = false;
            PainelPL24.Enabled = false;

        }


        public void Carregar_produtos()
        {
            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);

            String Sql1 = "Select * from materiaprima";
            MySqlCommand Comando = new MySqlCommand(Sql1, Conexao);
            Conexao.Open();

            MySqlDataAdapter objAdapter = new MySqlDataAdapter(Comando);
            DataTable ProdOld2 = new DataTable();
            objAdapter.Fill(ProdOld2);

            TextboxP1.Text = ProdOld2.Rows[0]["Produto"].ToString();
            TextboxP2.Text = ProdOld2.Rows[1]["Produto"].ToString();
            TextboxP3.Text = ProdOld2.Rows[2]["Produto"].ToString();
            TextboxP4.Text = ProdOld2.Rows[3]["Produto"].ToString();
            TextboxP5.Text = ProdOld2.Rows[4]["Produto"].ToString();
            TextboxP6.Text = ProdOld2.Rows[5]["Produto"].ToString();
            TextboxP7.Text = ProdOld2.Rows[6]["Produto"].ToString();
            TextboxP8.Text = ProdOld2.Rows[7]["Produto"].ToString();
            TextboxP9.Text = ProdOld2.Rows[8]["Produto"].ToString();
            TextboxP10.Text = ProdOld2.Rows[9]["Produto"].ToString();
            TextboxP11.Text = ProdOld2.Rows[10]["Produto"].ToString();
            TextboxP12.Text = ProdOld2.Rows[11]["Produto"].ToString();
            TextboxP13.Text = ProdOld2.Rows[12]["Produto"].ToString();
            TextboxP14.Text = ProdOld2.Rows[13]["Produto"].ToString();
            TextboxP15.Text = ProdOld2.Rows[14]["Produto"].ToString();
            TextboxP16.Text = ProdOld2.Rows[15]["Produto"].ToString();
            TextboxP17.Text = ProdOld2.Rows[16]["Produto"].ToString();
            TextboxP18.Text = ProdOld2.Rows[17]["Produto"].ToString();
            TextboxP19.Text = ProdOld2.Rows[18]["Produto"].ToString();
            TextboxP20.Text = ProdOld2.Rows[19]["Produto"].ToString();
            TextboxP21.Text = ProdOld2.Rows[20]["Produto"].ToString();
            TextboxP22.Text = ProdOld2.Rows[21]["Produto"].ToString();
            TextboxP23.Text = ProdOld2.Rows[22]["Produto"].ToString();
            TextboxP24.Text = ProdOld2.Rows[23]["Produto"].ToString();
            

            int[] Medida = new int[66];

            Medida[1] = Convert.ToInt16(ProdOld2.Rows[0]["Medida"]);
            Medida[2] = Convert.ToInt16(ProdOld2.Rows[1]["Medida"]);
            Medida[3] = Convert.ToInt16(ProdOld2.Rows[2]["Medida"]);
            Medida[4] = Convert.ToInt16(ProdOld2.Rows[3]["Medida"]);
            Medida[5] = Convert.ToInt16(ProdOld2.Rows[4]["Medida"]);
            Medida[6] = Convert.ToInt16(ProdOld2.Rows[5]["Medida"]);
            Medida[7] = Convert.ToInt16(ProdOld2.Rows[6]["Medida"]);
            Medida[8] = Convert.ToInt16(ProdOld2.Rows[7]["Medida"]);
            Medida[9] = Convert.ToInt16(ProdOld2.Rows[8]["Medida"]);
            Medida[10] = Convert.ToInt16(ProdOld2.Rows[9]["Medida"]);
            Medida[11] = Convert.ToInt16(ProdOld2.Rows[10]["Medida"]);
            Medida[12] = Convert.ToInt16(ProdOld2.Rows[11]["Medida"]);
            Medida[13] = Convert.ToInt16(ProdOld2.Rows[12]["Medida"]);
            Medida[14] = Convert.ToInt16(ProdOld2.Rows[13]["Medida"]);
            Medida[15] = Convert.ToInt16(ProdOld2.Rows[14]["Medida"]);
            Medida[16] = Convert.ToInt16(ProdOld2.Rows[15]["Medida"]);
            Medida[17] = Convert.ToInt16(ProdOld2.Rows[16]["Medida"]);
            Medida[18] = Convert.ToInt16(ProdOld2.Rows[17]["Medida"]);
            Medida[19] = Convert.ToInt16(ProdOld2.Rows[18]["Medida"]);
            Medida[20] = Convert.ToInt16(ProdOld2.Rows[19]["Medida"]);
            Medida[21] = Convert.ToInt16(ProdOld2.Rows[20]["Medida"]);
            Medida[22] = Convert.ToInt16(ProdOld2.Rows[21]["Medida"]);
            Medida[23] = Convert.ToInt16(ProdOld2.Rows[22]["Medida"]);
            Medida[24] = Convert.ToInt16(ProdOld2.Rows[23]["Medida"]);


            switch (Medida[1])
            {
                case 1000:
                    Radio_L1g.Checked = true;
                    break;
                case 1:
                    Radio_L1Kg.Checked = true;
                    break;
            
            }
            switch (Medida[2])
            {
                case 1000:
                    Radio_L2g.Checked = true;
                    break;
                case 1:
                    Radio_L2Kg.Checked = true;
                    break;
              
            }
            switch (Medida[3])
            {
                case 1000:
                    Radio_L3g.Checked = true;
                    break;
                case 1:
                    Radio_L3Kg.Checked = true;
                    break;
                
            }

            switch (Medida[4])
            {
                case 1000:
                    Radio_L4g.Checked = true;
                    break;
                case 1:
                    Radio_L4Kg.Checked = true;
                    break;
               
            }

            switch (Medida[5])
            {
                case 1000:
                    Radio_L5g.Checked = true;
                    break;
                case 1:
                    Radio_L5Kg.Checked = true;
                    break;
             
            }

            switch (Medida[6])
            {
                case 1000:
                    Radio_L6g.Checked = true;
                    break;
                case 1:
                    Radio_L6Kg.Checked = true;
                    break;
                
            }

            switch (Medida[7])
            {
                case 1000:
                    Radio_L7g.Checked = true;
                    break;
                case 1:
                    Radio_L7Kg.Checked = true;
                    break;
               
            }

            switch (Medida[8])
            {
                case 1000:
                    Radio_L8g.Checked = true;
                    break;
                case 1:
                    Radio_L8Kg.Checked = true;
                    break;
      
            }

            switch (Medida[9])
            {
                case 1000:
                    Radio_L9g.Checked = true;
                    break;
                case 1:
                    Radio_L9Kg.Checked = true;
                    break;
        
            }

            switch (Medida[10])
            {
                case 1000:
                    Radio_L10g.Checked = true;
                    break;
                case 1:
                    Radio_L10Kg.Checked = true;
                    break;
                
            }

            switch (Medida[11])
            {
                case 1000:
                    Radio_L11g.Checked = true;
                    break;
                case 1:
                    Radio_L11Kg.Checked = true;
                    break;
               
            }

            switch (Medida[12])
            {
                case 1000:
                    Radio_L12g.Checked = true;
                    break;
                case 1:
                    Radio_L12Kg.Checked = true;
                    break;
        
            }

            switch (Medida[13])
            {
                case 1000:
                    Radio_L13g.Checked = true;
                    break;
                case 1:
                    Radio_L13Kg.Checked = true;
                    break;
                
            }

            switch (Medida[14])
            {
                case 1000:
                    Radio_L14g.Checked = true;
                    break;
                case 1:
                    Radio_L14Kg.Checked = true;
                    break;
                }

            switch (Medida[15])
            {
                case 1000:
                    Radio_L15g.Checked = true;
                    break;
                case 1:
                    Radio_L15Kg.Checked = true;
                    break;
         }

            switch (Medida[16])
            {
                case 1000:
                    Radio_L16g.Checked = true;
                    break;
                case 1:
                    Radio_L16Kg.Checked = true;
                    break;
                }
            

            switch (Medida[17])
            {
                case 1000:
                    Radio_L17g.Checked = true;
                    break;
                case 1:
                    Radio_L17Kg.Checked = true;
                    break;
            }
            switch (Medida[18])
            {
                case 1000:
                    Radio_L18g.Checked = true;
                    break;
                case 1:
                    Radio_L18Kg.Checked = true;
                    break;
            }
            switch (Medida[19])
            {
                case 1000:
                    Radio_L19g.Checked = true;
                    break;
                case 1:
                    Radio_L19Kg.Checked = true;
                    break;
            }
            switch (Medida[20])
            {
                case 1000:
                    Radio_L20g.Checked = true;
                    break;
                case 1:
                    Radio_L20Kg.Checked = true;
                    break;
            }
            switch (Medida[21])
            {
                case 1000:
                    Radio_L21g.Checked = true;
                    break;
                case 1:
                    Radio_L21Kg.Checked = true;
                    break;
            }

            switch (Medida[22])
            {
                case 1000:
                    Radio_L22g.Checked = true;
                    break;
                case 1:
                    Radio_L22Kg.Checked = true;
                    break;
            }
            switch (Medida[23])
            {
                case 1000:
                    Radio_L23g.Checked = true;
                    break;
                case 1:
                    Radio_L23Kg.Checked = true;
                    break;
            }
            switch (Medida[24])
            {
                case 1000:
                    Radio_L24g.Checked = true;
                    break;
                case 1:
                    Radio_L24Kg.Checked = true;
                    break;
            }
            Conexao.Close();
        }



        public void Salvar_Produtos() {

            String Port = Properties.Settings.Default.Port;
            String User = Properties.Settings.Default.User;
            String Password = Properties.Settings.Default.Senha;
            String Server = Properties.Settings.Default.Server;

            MySqlConnection Conexao = new MySqlConnection("Server=" + Server + ";port=" + Port + ";User Id=" + User + ";database=cadastro;password=" + Password);

            String Sql1 = "Select * from materiaprima";
            MySqlCommand Comando = new MySqlCommand(Sql1, Conexao);
            Conexao.Open();

            MySqlDataAdapter objAdapter = new MySqlDataAdapter(Comando);
            DataTable ProdOld = new DataTable();
            objAdapter.Fill(ProdOld);
            
            // Produto 1 = Confere e Salva Informações
            String Nome1;
            String Combo1;
            String Medida1;
            String medtab1;
            Medida1 = "1";
            if (Radio_L1g.Checked) { Medida1 = "1000"; }
            if (Radio_L1Kg.Checked) { Medida1 = "1"; }
            

            Nome1 = ProdOld.Rows[0]["Produto"].ToString();
            medtab1 = ProdOld.Rows[0]["Medida"].ToString();
            Combo1 = TextboxP1.Text;
            if (Combo1 != Nome1)
            {
                String SqlProd1 = "Update materiaprima set Produto = @novo1 where Num=1";
                MySqlCommand ComProd1 = new MySqlCommand(SqlProd1, Conexao);
                ComProd1.Parameters.AddWithValue("@novo1", Combo1);
                ComProd1.ExecuteNonQuery();
            }

            if (medtab1 != Medida1)
            {
                String SqlMed1 = "Update materiaprima set Medida = @medida1 where Num=1";
                MySqlCommand ComMed1 = new MySqlCommand(SqlMed1, Conexao);
                ComMed1.Parameters.AddWithValue("@medida1", Convert.ToInt32(Medida1));
                ComMed1.ExecuteNonQuery();
            }



            // Produto 2 = Confere e Salva Informações
            String Nome2;
            String Combo2;
            String Medida2;
            String medtab2;
            Medida2 = "1";
            if (Radio_L2g.Checked) { Medida2 = "1000"; }
            if (Radio_L2Kg.Checked) { Medida2 = "1"; }
           

            Nome2 = ProdOld.Rows[1]["Produto"].ToString();
            medtab2 = ProdOld.Rows[1]["Medida"].ToString();
            Combo2 = TextboxP2.Text;
            if (Combo2 != Nome2)
            {
                String SqlProd2 = "Update materiaprima set Produto = @novo2 where Num=2";
                MySqlCommand ComProd2 = new MySqlCommand(SqlProd2, Conexao);
                ComProd2.Parameters.AddWithValue("@novo2", Combo2);
                ComProd2.ExecuteNonQuery();
            }

            if (medtab2 != Medida2)
            {
                String SqlMed2 = "Update materiaprima set Medida = @medida2 where Num=2";
                MySqlCommand ComMed2 = new MySqlCommand(SqlMed2, Conexao);
                ComMed2.Parameters.AddWithValue("@medida2", Convert.ToInt32(Medida2));
                ComMed2.ExecuteNonQuery();
            }

            // Produto 3 = Confere e Salva Informações
            String Nome3;
            String Combo3;
            String Medida3;
            String medtab3;
            Medida3 = "1";
            if (Radio_L3g.Checked) { Medida3 = "1000"; }
            if (Radio_L3Kg.Checked) { Medida3 = "1"; }
           

            Nome3 = ProdOld.Rows[2]["Produto"].ToString();
            medtab3 = ProdOld.Rows[2]["Medida"].ToString();
            Combo3 = TextboxP3.Text;
            if (Combo3 != Nome3)
            {
                String SqlProd3 = "Update materiaprima set Produto = @novo3 where Num=3";
                MySqlCommand ComProd3 = new MySqlCommand(SqlProd3, Conexao);
                ComProd3.Parameters.AddWithValue("@novo3", Combo3);
                ComProd3.ExecuteNonQuery();
            }

            if (medtab3 != Medida3)
            {
                String SqlMed3 = "Update materiaprima set Medida = @medida3 where Num=3";
                MySqlCommand ComMed3 = new MySqlCommand(SqlMed3, Conexao);
                ComMed3.Parameters.AddWithValue("@medida3", Convert.ToInt32(Medida3));
                ComMed3.ExecuteNonQuery();
            }

            // Produto 4 = Confere e Salva Informações
            String Nome4;
            String Combo4;
            String Medida4;
            String medtab4;
            Medida4 = "1";
            if (Radio_L4g.Checked) { Medida4 = "1000"; }
            if (Radio_L4Kg.Checked) { Medida4 = "1"; }
         
            Nome4 = ProdOld.Rows[3]["Produto"].ToString();
            medtab4 = ProdOld.Rows[3]["Medida"].ToString();
            Combo4 = TextboxP4.Text;
            if (Combo4 != Nome4)
            {
                String SqlProd4 = "Update materiaprima set Produto = @novo4 where Num=4";
                MySqlCommand ComProd4 = new MySqlCommand(SqlProd4, Conexao);
                ComProd4.Parameters.AddWithValue("@novo4", Combo4);
                ComProd4.ExecuteNonQuery();
            }

            if (medtab4 != Medida4)
            {
                String SqlMed4 = "Update materiaprima set Medida = @medida4 where Num=4";
                MySqlCommand ComMed4 = new MySqlCommand(SqlMed4, Conexao);
                ComMed4.Parameters.AddWithValue("@medida4", Convert.ToInt32(Medida4));
                ComMed4.ExecuteNonQuery();
            }

            // Produto 5 = Confere e Salva Informações
            String Nome5;
            String Combo5;
            String Medida5;
            String medtab5;
            Medida5 ="1";
            if (Radio_L5g.Checked) { Medida5 = "1000"; }
            if (Radio_L5Kg.Checked) { Medida5 = "1"; }
    
            Nome5 = ProdOld.Rows[4]["Produto"].ToString();
            medtab5 = ProdOld.Rows[4]["Medida"].ToString();
            Combo5 = TextboxP5.Text;
            if (Combo5 != Nome5)
            {
                String SqlProd5 = "Update materiaprima set Produto = @novo5 where Num=5";
                MySqlCommand ComProd5 = new MySqlCommand(SqlProd5, Conexao);
                ComProd5.Parameters.AddWithValue("@novo5", Combo5);
                ComProd5.ExecuteNonQuery();
            }

            if (medtab5 != Medida5)
            {
                String SqlMed5 = "Update materiaprima set Medida = @medida5 where Num=5";
                MySqlCommand ComMed5 = new MySqlCommand(SqlMed5, Conexao);
                ComMed5.Parameters.AddWithValue("@medida5", Convert.ToInt32(Medida5));
                ComMed5.ExecuteNonQuery();
            }

            // Produto 6 = Confere e Salva Informações
            String Nome6;
            String Combo6;
            String Medida6;
            String medtab6;
            Medida6 = "1";
            if (Radio_L6g.Checked) { Medida6 = "1000"; }
            if (Radio_L6Kg.Checked) { Medida6 = "1"; }
     
            Nome6 = ProdOld.Rows[5]["Produto"].ToString();
            medtab6 = ProdOld.Rows[5]["Medida"].ToString();
            Combo6 = TextboxP6.Text;
            if (Combo6 != Nome6)
            {
                String SqlProd6 = "Update materiaprima set Produto = @novo6 where Num=6";
                MySqlCommand ComProd6 = new MySqlCommand(SqlProd6, Conexao);
                ComProd6.Parameters.AddWithValue("@novo6", Combo6);
                ComProd6.ExecuteNonQuery();
            }

            if (medtab6 != Medida6)
            {
                String SqlMed6 = "Update materiaprima set Medida = @medida6 where Num=6";
                MySqlCommand ComMed6 = new MySqlCommand(SqlMed6, Conexao);
                ComMed6.Parameters.AddWithValue("@medida6", Convert.ToInt32(Medida6));
                ComMed6.ExecuteNonQuery();
            }

            // Produto 7 = Confere e Salva Informações
            String Nome7;
            String Combo7;
            String Medida7;
            String medtab7;
            Medida7 = "1";
            if (Radio_L7g.Checked) { Medida7 = "1000"; }
            if (Radio_L7Kg.Checked) { Medida7 = "1"; }
            

            Nome7 = ProdOld.Rows[6]["Produto"].ToString();
            medtab7= ProdOld.Rows[6]["Medida"].ToString();
            Combo7= TextboxP7.Text;
            if (Combo7 != Nome7)
            {
                String SqlProd7 = "Update materiaprima set Produto = @novo7 where Num=7";
                MySqlCommand ComProd7 = new MySqlCommand(SqlProd7, Conexao);
                ComProd7.Parameters.AddWithValue("@novo7", Combo7);
                ComProd7.ExecuteNonQuery();
            }

            if (medtab7 != Medida7)
            {
                String SqlMed7 = "Update materiaprima set Medida = @medida7 where Num=7";
                MySqlCommand ComMed7 = new MySqlCommand(SqlMed7, Conexao);
                ComMed7.Parameters.AddWithValue("@medida7", Convert.ToInt32(Medida7));
                ComMed7.ExecuteNonQuery();
            }

            // Produto 8 = Confere e Salva Informações
            String Nome8;
            String Combo8;
            String Medida8;
            String medtab8;
            Medida8 = "1";
            if (Radio_L8g.Checked) { Medida8 = "1000"; }
            if (Radio_L8Kg.Checked) { Medida8 = "1"; }
           

            Nome8 = ProdOld.Rows[7]["Produto"].ToString();
            medtab8 = ProdOld.Rows[7]["Medida"].ToString();
            Combo8 = TextboxP8.Text;
            if (Combo8 != Nome8)
            {
                String SqlProd8 = "Update materiaprima set Produto = @novo8 where Num=8";
                MySqlCommand ComProd8 = new MySqlCommand(SqlProd8, Conexao);
                ComProd8.Parameters.AddWithValue("@novo8", Combo8);
                ComProd8.ExecuteNonQuery();
            }

            if (medtab8 != Medida8)
            {
                String SqlMed8 = "Update materiaprima set Medida = @medida8 where Num=8";
                MySqlCommand ComMed8 = new MySqlCommand(SqlMed8, Conexao);
                ComMed8.Parameters.AddWithValue("@medida8", Convert.ToInt32(Medida8));
                ComMed8.ExecuteNonQuery();
            }

            // Produto 9 = Confere e Salva Informações
            String Nome9;
            String Combo9;
            String Medida9;
            String medtab9;
            Medida9 = "12";
            if (Radio_L9g.Checked) { Medida9 = "1000"; }
            if (Radio_L9Kg.Checked) { Medida9 = "1"; }
            

            Nome9 = ProdOld.Rows[8]["Produto"].ToString();
            medtab9 = ProdOld.Rows[8]["Medida"].ToString();
            Combo9 = TextboxP9.Text;
            if (Combo9 != Nome9)
            {
                String SqlProd9 = "Update materiaprima set Produto = @novo9 where Num=9";
                MySqlCommand ComProd9 = new MySqlCommand(SqlProd9, Conexao);
                ComProd9.Parameters.AddWithValue("@novo9", Combo9);
                ComProd9.ExecuteNonQuery();
            }

            if (medtab9 != Medida9)
            {
                String SqlMed9 = "Update materiaprima set Medida = @medida9 where Num=9";
                MySqlCommand ComMed9 = new MySqlCommand(SqlMed9, Conexao);
                ComMed9.Parameters.AddWithValue("@medida9", Convert.ToInt32(Medida9));
                ComMed9.ExecuteNonQuery();
            }

            // Produto 10 = Confere e Salva Informações
            String Nome10;
            String Combo10;
            String Medida10;
            String medtab10;
            Medida10 = "1";
            if (Radio_L10g.Checked) { Medida10 = "1000"; }
            if (Radio_L10Kg.Checked) { Medida10 = "1"; }
            

            Nome10 = ProdOld.Rows[9]["Produto"].ToString();
            medtab10 = ProdOld.Rows[9]["Medida"].ToString();
            Combo10 = TextboxP10.Text;
            if (Combo10 != Nome10)
            {
                String SqlProd10 = "Update materiaprima set Produto = @novo10 where Num=10";
                MySqlCommand ComProd10 = new MySqlCommand(SqlProd10, Conexao);
                ComProd10.Parameters.AddWithValue("@novo10", Combo10);
                ComProd10.ExecuteNonQuery();
            }

            if (medtab10 != Medida10)
            {
                String SqlMed10 = "Update materiaprima set Medida = @medida10 where Num=10";
                MySqlCommand ComMed10 = new MySqlCommand(SqlMed10, Conexao);
                ComMed10.Parameters.AddWithValue("@medida10", Convert.ToInt32(Medida10));
                ComMed10.ExecuteNonQuery();
            }

            // Produto 11 = Confere e Salva Informações
            String Nome11;
            String Combo11;
            String Medida11;
            String medtab11;
            Medida11 = "1";
            if (Radio_L11g.Checked) { Medida11 = "1000"; }
            if (Radio_L11Kg.Checked) { Medida11 = "1"; }
           

            Nome11 = ProdOld.Rows[10]["Produto"].ToString();
            medtab11 = ProdOld.Rows[10]["Medida"].ToString();
            Combo11 = TextboxP11.Text;
            if (Combo11 != Nome11)
            {
                String SqlProd11 = "Update materiaprima set Produto = @novo11 where Num=11";
                MySqlCommand ComProd11 = new MySqlCommand(SqlProd11, Conexao);
                ComProd11.Parameters.AddWithValue("@novo11", Combo11);
                ComProd11.ExecuteNonQuery();
            }

            if (medtab11 != Medida11)
            {
                String SqlMed11 = "Update materiaprima set Medida = @medida11 where Num=11";
                MySqlCommand ComMed11 = new MySqlCommand(SqlMed11, Conexao);
                ComMed11.Parameters.AddWithValue("@medida11", Convert.ToInt32(Medida11));
                ComMed11.ExecuteNonQuery();
            }

            // Produto 12 = Confere e Salva Informações
            String Nome12;
            String Combo12;
            String Medida12;
            String medtab12;
            Medida12 = "1";
            if (Radio_L12g.Checked) { Medida12 = "1000"; }
            if (Radio_L12Kg.Checked) { Medida12 = "1"; }
           

            Nome12 = ProdOld.Rows[11]["Produto"].ToString();
            medtab12 = ProdOld.Rows[11]["Medida"].ToString();
            Combo12 = TextboxP12.Text;
            if (Combo12 != Nome12)
            {
                String SqlProd12 = "Update materiaprima set Produto = @novo12 where Num=12";
                MySqlCommand ComProd12 = new MySqlCommand(SqlProd12, Conexao);
                ComProd12.Parameters.AddWithValue("@novo12", Combo12);
                ComProd12.ExecuteNonQuery();
            }

            if (medtab12 != Medida12)
            {
                String SqlMed12 = "Update materiaprima set Medida = @medida12 where Num=12";
                MySqlCommand ComMed12 = new MySqlCommand(SqlMed12, Conexao);
                ComMed12.Parameters.AddWithValue("@medida12", Convert.ToInt32(Medida12));
                ComMed12.ExecuteNonQuery();
            }

            // Produto 13 = Confere e Salva Informações
            String Nome13;
            String Combo13;
            String Medida13;
            String medtab13;
            Medida13 = "1";
            if (Radio_L13g.Checked) { Medida13 = "1000"; }
            if (Radio_L13Kg.Checked) { Medida13 = "1"; }
           

            Nome13 = ProdOld.Rows[12]["Produto"].ToString();
            medtab13 = ProdOld.Rows[12]["Medida"].ToString();
            Combo13 = TextboxP13.Text;
            if (Combo13 != Nome13)
            {
                String SqlProd13 = "Update materiaprima set Produto = @novo13 where Num=13";
                MySqlCommand ComProd13 = new MySqlCommand(SqlProd13, Conexao);
                ComProd13.Parameters.AddWithValue("@novo13", Combo13);
                ComProd13.ExecuteNonQuery();
            }

            if (medtab13 != Medida13)
            {
                String SqlMed13 = "Update materiaprima set Medida = @medida13 where Num=13";
                MySqlCommand ComMed13 = new MySqlCommand(SqlMed13, Conexao);
                ComMed13.Parameters.AddWithValue("@medida13", Convert.ToInt32(Medida13));
                ComMed13.ExecuteNonQuery();
            }

            // Produto 14 = Confere e Salva Informações
            String Nome14;
            String Combo14;
            String Medida14;
            String medtab14;
            Medida14 = "1";
            if (Radio_L14g.Checked) { Medida14 = "1000"; }
            if (Radio_L14Kg.Checked) { Medida14 = "1"; }
            

            Nome14 = ProdOld.Rows[13]["Produto"].ToString();
            medtab14 = ProdOld.Rows[13]["Medida"].ToString();
            Combo14 = TextboxP14.Text;
            if (Combo14 != Nome14)
            {
                String SqlProd14 = "Update materiaprima set Produto = @novo14 where Num=14";
                MySqlCommand ComProd14 = new MySqlCommand(SqlProd14, Conexao);
                ComProd14.Parameters.AddWithValue("@novo14", Combo14);
                ComProd14.ExecuteNonQuery();
            }

            if (medtab14 != Medida14)
            {
                String SqlMed14 = "Update materiaprima set Medida = @medida14 where Num=14";
                MySqlCommand ComMed14 = new MySqlCommand(SqlMed14, Conexao);
                ComMed14.Parameters.AddWithValue("@medida14", Convert.ToInt32(Medida14));
                ComMed14.ExecuteNonQuery();
            }

            // Produto 15 = Confere e Salva Informações
            String Nome15;
            String Combo15;
            String Medida15;
            String medtab15;
            Medida15 = "1";
            if (Radio_L15g.Checked) { Medida15 = "1000"; }
            if (Radio_L15Kg.Checked) { Medida15 = "1"; }
            

            Nome15 = ProdOld.Rows[14]["Produto"].ToString();
            medtab15 = ProdOld.Rows[14]["Medida"].ToString();
            Combo15 = TextboxP15.Text;
            if (Combo15 != Nome15)
            {
                String SqlProd15 = "Update materiaprima set Produto = @novo15 where Num=15";
                MySqlCommand ComProd15 = new MySqlCommand(SqlProd15, Conexao);
                ComProd15.Parameters.AddWithValue("@novo15", Combo15);
                ComProd15.ExecuteNonQuery();
            }

            if (medtab15 != Medida15)
            {
                String SqlMed15 = "Update materiaprima set Medida = @medida15 where Num=15";
                MySqlCommand ComMed15 = new MySqlCommand(SqlMed15, Conexao);
                ComMed15.Parameters.AddWithValue("@medida15", Convert.ToInt32(Medida15));
                ComMed15.ExecuteNonQuery();
            }
            // Produto 16 = Confere e Salva Informações
            String Nome16;
            String Combo16;
            String Medida16;
            String medtab16;
            Medida16 = "1";
            if (Radio_L16g.Checked) { Medida16 = "1000"; }
            if (Radio_L16Kg.Checked) { Medida16 = "1"; }
            
            Nome16 = ProdOld.Rows[15]["Produto"].ToString();
            medtab16 = ProdOld.Rows[15]["Medida"].ToString();
            Combo16 = TextboxP16.Text;
            if (Combo16 != Nome16)
            {
                String SqlProd16 = "Update materiaprima set Produto = @novo16 where Num=16";
                MySqlCommand ComProd16 = new MySqlCommand(SqlProd16, Conexao);
                ComProd16.Parameters.AddWithValue("@novo16", Combo16);
                ComProd16.ExecuteNonQuery();
            }

            if (medtab16 != Medida16)
            {
                String SqlMed16 = "Update materiaprima set Medida = @medida16 where Num=16";
                MySqlCommand ComMed16 = new MySqlCommand(SqlMed16, Conexao);
                ComMed16.Parameters.AddWithValue("@medida16", Convert.ToInt32(Medida16));
                ComMed16.ExecuteNonQuery();
            }
           
            // Produto 17 = Confere e Salva Informações
            String Nome17;
            String Combo17;
            String Medida17;
            String medtab17;
            Medida17 = "1";
            if (Radio_L17g.Checked) { Medida17 = "1000"; }
            if (Radio_L17Kg.Checked) { Medida17 = "1"; }


            Nome17 = ProdOld.Rows[16]["Produto"].ToString();
            medtab17 = ProdOld.Rows[16]["Medida"].ToString();
            Combo17 = TextboxP17.Text;
            if (Combo17 != Nome17)
            {
                String SqlProd17 = "Update materiaprima set Produto = @novo17 where Num=17";
                MySqlCommand ComProd17 = new MySqlCommand(SqlProd17, Conexao);
                ComProd17.Parameters.AddWithValue("@novo17", Combo17);
                ComProd17.ExecuteNonQuery();
            }

            if (medtab17 != Medida17)
            {
                String SqlMed17 = "Update materiaprima set Medida = @medida17 where Num=17";
                MySqlCommand ComMed17 = new MySqlCommand(SqlMed17, Conexao);
                ComMed17.Parameters.AddWithValue("@medida17", Convert.ToInt32(Medida17));
                ComMed17.ExecuteNonQuery();
            }



            // Produto 18 = Confere e Salva Informações
            String Nome18;
            String Combo18;
            String Medida18;
            String medtab18;
            Medida18 = "1";
            if (Radio_L18g.Checked) { Medida18 = "1000"; }
            if (Radio_L18Kg.Checked) { Medida18 = "1"; }


            Nome18 = ProdOld.Rows[17]["Produto"].ToString();
            medtab18 = ProdOld.Rows[17]["Medida"].ToString();
            Combo18 = TextboxP18.Text;
            if (Combo18 != Nome18)
            {
                String SqlProd18 = "Update materiaprima set Produto = @novo18 where Num=18";
                MySqlCommand ComProd18 = new MySqlCommand(SqlProd18, Conexao);
                ComProd18.Parameters.AddWithValue("@novo18", Combo18);
                ComProd18.ExecuteNonQuery();
            }

            if (medtab18 != Medida18)
            {
                String SqlMed18 = "Update materiaprima set Medida = @medida18 where Num=18";
                MySqlCommand ComMed18 = new MySqlCommand(SqlMed18, Conexao);
                ComMed18.Parameters.AddWithValue("@medida18", Convert.ToInt32(Medida18));
                ComMed18.ExecuteNonQuery();
            }

            // Produto 19 = Confere e Salva Informações
            String Nome19;
            String Combo19;
            String Medida19;
            String medtab19;
            Medida19 = "1";
            if (Radio_L19g.Checked) { Medida19 = "1000"; }
            if (Radio_L19Kg.Checked) { Medida19 = "1"; }


            Nome19 = ProdOld.Rows[18]["Produto"].ToString();
            medtab19 = ProdOld.Rows[18]["Medida"].ToString();
            Combo19 = TextboxP19.Text;
            if (Combo19 != Nome19)
            {
                String SqlProd19 = "Update materiaprima set Produto = @novo19 where Num=19";
                MySqlCommand ComProd19 = new MySqlCommand(SqlProd19, Conexao);
                ComProd19.Parameters.AddWithValue("@novo19", Combo19);
                ComProd19.ExecuteNonQuery();
            }

            if (medtab19 != Medida19)
            {
                String SqlMed19 = "Update materiaprima set Medida = @medida19 where Num=19";
                MySqlCommand ComMed19 = new MySqlCommand(SqlMed19, Conexao);
                ComMed19.Parameters.AddWithValue("@medida19", Convert.ToInt32(Medida19));
                ComMed19.ExecuteNonQuery();
            }

            // Produto 20 = Confere e Salva Informações
            String Nome20;
            String Combo20;
            String Medida20;
            String medtab20;
            Medida20 = "1";
            if (Radio_L20g.Checked) { Medida20 = "1000"; }
            if (Radio_L20Kg.Checked) { Medida20 = "1"; }

            Nome20 = ProdOld.Rows[19]["Produto"].ToString();
            medtab20 = ProdOld.Rows[19]["Medida"].ToString();
            Combo20 = TextboxP20.Text;
            if (Combo20 != Nome20)
            {
                String SqlProd20 = "Update materiaprima set Produto = @novo20 where Num=20";
                MySqlCommand ComProd20 = new MySqlCommand(SqlProd20, Conexao);
                ComProd20.Parameters.AddWithValue("@novo20", Combo20);
                ComProd20.ExecuteNonQuery();
            }

            if (medtab20 != Medida20)
            {
                String SqlMed20 = "Update materiaprima set Medida = @medida20 where Num=20";
                MySqlCommand ComMed20 = new MySqlCommand(SqlMed20, Conexao);
                ComMed20.Parameters.AddWithValue("@medida20", Convert.ToInt32(Medida20));
                ComMed20.ExecuteNonQuery();
            }

            // Produto 21 = Confere e Salva Informações
            String Nome21;
            String Combo21;
            String Medida21;
            String medtab21;
            Medida21 = "1";
            if (Radio_L21g.Checked) { Medida21 = "1000"; }
            if (Radio_L21Kg.Checked) { Medida21 = "1"; }

            Nome21 = ProdOld.Rows[20]["Produto"].ToString();
            medtab21 = ProdOld.Rows[20]["Medida"].ToString();
            Combo21 = TextboxP21.Text;
            if (Combo21 != Nome21)
            {
                String SqlProd21 = "Update materiaprima set Produto = @novo21 where Num=21";
                MySqlCommand ComProd21 = new MySqlCommand(SqlProd21, Conexao);
                ComProd21.Parameters.AddWithValue("@novo21", Combo21);
                ComProd21.ExecuteNonQuery();
            }

            if (medtab21 != Medida21)
            {
                String SqlMed21 = "Update materiaprima set Medida = @medida21 where Num=21";
                MySqlCommand ComMed21 = new MySqlCommand(SqlMed21, Conexao);
                ComMed21.Parameters.AddWithValue("@medida21", Convert.ToInt32(Medida21));
                ComMed21.ExecuteNonQuery();
            }

            // Produto 22 = Confere e Salva Informações
            String Nome22;
            String Combo22;
            String Medida22;
            String medtab22;
            Medida22 = "1";
            if (Radio_L22g.Checked) { Medida22 = "1000"; }
            if (Radio_L22Kg.Checked) { Medida22 = "1"; }

            Nome22 = ProdOld.Rows[21]["Produto"].ToString();
            medtab22 = ProdOld.Rows[21]["Medida"].ToString();
            Combo22 = TextboxP22.Text;
            if (Combo22 != Nome22)
            {
                String SqlProd22 = "Update materiaprima set Produto = @novo22 where Num=22";
                MySqlCommand ComProd22 = new MySqlCommand(SqlProd22, Conexao);
                ComProd22.Parameters.AddWithValue("@novo22", Combo22);
                ComProd22.ExecuteNonQuery();
            }

            if (medtab22 != Medida22)
            {
                String SqlMed22 = "Update materiaprima set Medida = @medida22 where Num=22";
                MySqlCommand ComMed22 = new MySqlCommand(SqlMed22, Conexao);
                ComMed22.Parameters.AddWithValue("@medida22", Convert.ToInt32(Medida22));
                ComMed22.ExecuteNonQuery();
            }

            // Produto 23 = Confere e Salva Informações
            String Nome23;
            String Combo23;
            String Medida23;
            String medtab23;
            Medida23 = "1";
            if (Radio_L23g.Checked) { Medida23 = "1000"; }
            if (Radio_L23Kg.Checked) { Medida23 = "1"; }


            Nome23 = ProdOld.Rows[22]["Produto"].ToString();
            medtab23 = ProdOld.Rows[22]["Medida"].ToString();
            Combo23 = TextboxP23.Text;
            if (Combo23 != Nome23)
            {
                String SqlProd23 = "Update materiaprima set Produto = @novo23 where Num=23";
                MySqlCommand ComProd23 = new MySqlCommand(SqlProd23, Conexao);
                ComProd23.Parameters.AddWithValue("@novo23", Combo23);
                ComProd23.ExecuteNonQuery();
            }

            if (medtab23 != Medida23)
            {
                String SqlMed23 = "Update materiaprima set Medida = @medida23 where Num=23";
                MySqlCommand ComMed23 = new MySqlCommand(SqlMed23, Conexao);
                ComMed23.Parameters.AddWithValue("@medida23", Convert.ToInt32(Medida23));
                ComMed23.ExecuteNonQuery();
            }

            // Produto 24 = Confere e Salva Informações
            String Nome24;
            String Combo24;
            String Medida24;
            String medtab24;
            Medida24 = "1";
            if (Radio_L24g.Checked) { Medida24 = "1000"; }
            if (Radio_L24Kg.Checked) { Medida24 = "1"; }


            Nome24 = ProdOld.Rows[23]["Produto"].ToString();
            medtab24 = ProdOld.Rows[23]["Medida"].ToString();
            Combo24 = TextboxP24.Text;
            if (Combo24 != Nome24)
            {
                String SqlProd24 = "Update materiaprima set Produto = @novo24 where Num=24";
                MySqlCommand ComProd24 = new MySqlCommand(SqlProd24, Conexao);
                ComProd24.Parameters.AddWithValue("@novo24", Combo24);
                ComProd24.ExecuteNonQuery();
            }

            if (medtab24 != Medida24)
            {
                String SqlMed24 = "Update materiaprima set Medida = @medida24 where Num=24";
                MySqlCommand ComMed24 = new MySqlCommand(SqlMed24, Conexao);
                ComMed24.Parameters.AddWithValue("@medida24", Convert.ToInt32(Medida24));
                ComMed24.ExecuteNonQuery();
            }

            //// Produto 25 = Confere e Salva Informações
            //String Nome25;
            //String Combo25;
            //String Medida25;
            //String medtab25;
            //Medida25 = "1";
            //if (Radio_L25g.Checked) { Medida25 = "1000"; }
            //if (Radio_L25Kg.Checked) { Medida25 = "1"; }


            //Nome25 = ProdOld.Rows[24]["Produto"].ToString();
            //medtab25 = ProdOld.Rows[24]["Medida"].ToString();
            //Combo25 = TextboxP25.Text;
            //if (Combo25 != Nome25)
            //{
            //    String SqlProd25 = "Update materiaprima set Produto = @novo25 where Num=25";
            //    MySqlCommand ComProd25 = new MySqlCommand(SqlProd25, Conexao);
            //    ComProd25.Parameters.AddWithValue("@novo25", Combo25);
            //    ComProd25.ExecuteNonQuery();
            //}

            //if (medtab25 != Medida25)
            //{
            //    String SqlMed25 = "Update materiaprima set Medida = @medida25 where Num=25";
            //    MySqlCommand ComMed25 = new MySqlCommand(SqlMed25, Conexao);
            //    ComMed25.Parameters.AddWithValue("@medida25", Convert.ToInt32(Medida25));
            //    ComMed25.ExecuteNonQuery();
            //}

            //// Produto 26 = Confere e Salva Informações
            //String Nome26;
            //String Combo26;
            //String Medida26;
            //String medtab26;
            //Medida26 = "1";
            //if (Radio_L26g.Checked) { Medida26 = "1000"; }
            //if (Radio_L26Kg.Checked) { Medida26 = "1"; }


            //Nome26 = ProdOld.Rows[25]["Produto"].ToString();
            //medtab26 = ProdOld.Rows[25]["Medida"].ToString();
            //Combo26 = TextboxP26.Text;
            //if (Combo26 != Nome26)
            //{
            //    String SqlProd26 = "Update materiaprima set Produto = @novo26 where Num=26";
            //    MySqlCommand ComProd26 = new MySqlCommand(SqlProd26, Conexao);
            //    ComProd26.Parameters.AddWithValue("@novo26", Combo26);
            //    ComProd26.ExecuteNonQuery();
            //}

            //if (medtab26 != Medida26)
            //{
            //    String SqlMed26 = "Update materiaprima set Medida = @medida26 where Num=26";
            //    MySqlCommand ComMed26 = new MySqlCommand(SqlMed26, Conexao);
            //    ComMed26.Parameters.AddWithValue("@medida26", Convert.ToInt32(Medida26));
            //    ComMed26.ExecuteNonQuery();
            //}

            //// Produto 27 = Confere e Salva Informações
            //String Nome27;
            //String Combo27;
            //String Medida27;
            //String medtab27;
            //Medida27 = "1";
            //if (Radio_L27g.Checked) { Medida27 = "1000"; }
            //if (Radio_L27Kg.Checked) { Medida27 = "1"; }


            //Nome27 = ProdOld.Rows[26]["Produto"].ToString();
            //medtab27 = ProdOld.Rows[26]["Medida"].ToString();
            //Combo27 = TextboxP27.Text;
            //if (Combo27 != Nome27)
            //{
            //    String SqlProd27 = "Update materiaprima set Produto = @novo27 where Num=27";
            //    MySqlCommand ComProd27 = new MySqlCommand(SqlProd27, Conexao);
            //    ComProd27.Parameters.AddWithValue("@novo27", Combo27);
            //    ComProd27.ExecuteNonQuery();
            //}

            //if (medtab27 != Medida27)
            //{
            //    String SqlMed27 = "Update materiaprima set Medida = @medida27 where Num=27";
            //    MySqlCommand ComMed27 = new MySqlCommand(SqlMed27, Conexao);
            //    ComMed27.Parameters.AddWithValue("@medida27", Convert.ToInt32(Medida27));
            //    ComMed27.ExecuteNonQuery();
            //}

            //// Produto 28 = Confere e Salva Informações
            //String Nome28;
            //String Combo28;
            //String Medida28;
            //String medtab28;
            //Medida28 = "1";
            //if (Radio_L28g.Checked) { Medida28 = "1000"; }
            //if (Radio_L28Kg.Checked) { Medida28 = "1"; }


            //Nome28 = ProdOld.Rows[27]["Produto"].ToString();
            //medtab28 = ProdOld.Rows[27]["Medida"].ToString();
            //Combo28 = TextboxP28.Text;
            //if (Combo28 != Nome28)
            //{
            //    String SqlProd28 = "Update materiaprima set Produto = @novo28 where Num=28";
            //    MySqlCommand ComProd28 = new MySqlCommand(SqlProd28, Conexao);
            //    ComProd28.Parameters.AddWithValue("@novo28", Combo28);
            //    ComProd28.ExecuteNonQuery();
            //}

            //if (medtab28 != Medida28)
            //{
            //    String SqlMed28 = "Update materiaprima set Medida = @medida28 where Num=28";
            //    MySqlCommand ComMed28 = new MySqlCommand(SqlMed28, Conexao);
            //    ComMed28.Parameters.AddWithValue("@medida28", Convert.ToInt32(Medida28));
            //    ComMed28.ExecuteNonQuery();
            //}

            //// Produto 29 = Confere e Salva Informações
            //String Nome29;
            //String Combo29;
            //String Medida29;
            //String medtab29;
            //Medida29 = "1";
            //if (Radio_L29g.Checked) { Medida29 = "1000"; }
            //if (Radio_L29Kg.Checked) { Medida29 = "1"; }


            //Nome29 = ProdOld.Rows[28]["Produto"].ToString();
            //medtab29 = ProdOld.Rows[28]["Medida"].ToString();
            //Combo29 = TextboxP29.Text;
            //if (Combo29 != Nome29)
            //{
            //    String SqlProd29 = "Update materiaprima set Produto = @novo29 where Num=29";
            //    MySqlCommand ComProd29 = new MySqlCommand(SqlProd29, Conexao);
            //    ComProd29.Parameters.AddWithValue("@novo29", Combo29);
            //    ComProd29.ExecuteNonQuery();
            //}

            //if (medtab29 != Medida29)
            //{
            //    String SqlMed29 = "Update materiaprima set Medida = @medida29 where Num=29";
            //    MySqlCommand ComMed29 = new MySqlCommand(SqlMed29, Conexao);
            //    ComMed29.Parameters.AddWithValue("@medida29", Convert.ToInt32(Medida29));
            //    ComMed29.ExecuteNonQuery();
            //}

            //// Produto 30 = Confere e Salva Informações
            //String Nome30;
            //String Combo30;
            //String Medida30;
            //String medtab30;
            //Medida30 = "1";
            //if (Radio_L30g.Checked) { Medida30 = "1000"; }
            //if (Radio_L30Kg.Checked) { Medida30 = "1"; }


            //Nome30 = ProdOld.Rows[29]["Produto"].ToString();
            //medtab30 = ProdOld.Rows[29]["Medida"].ToString();
            //Combo30 = TextboxP30.Text;
            //if (Combo30 != Nome30)
            //{
            //    String SqlProd30 = "Update materiaprima set Produto = @novo30 where Num=30";
            //    MySqlCommand ComProd30 = new MySqlCommand(SqlProd30, Conexao);
            //    ComProd30.Parameters.AddWithValue("@novo30", Combo30);
            //    ComProd30.ExecuteNonQuery();
            //}

            //if (medtab30 != Medida30)
            //{
            //    String SqlMed30 = "Update materiaprima set Medida = @medida30 where Num=30";
            //    MySqlCommand ComMed30 = new MySqlCommand(SqlMed30, Conexao);
            //    ComMed30.Parameters.AddWithValue("@medida30", Convert.ToInt32(Medida30));
            //    ComMed30.ExecuteNonQuery();
            //}

            //// Produto 31 = Confere e Salva Informações
            //String Nome31;
            //String Combo31;
            //String Medida31;
            //String medtab31;
            //Medida31 = "1";
            //if (Radio_L31g.Checked) { Medida31 = "1000"; }
            //if (Radio_L31Kg.Checked) { Medida31 = "1"; }


            //Nome31 = ProdOld.Rows[30]["Produto"].ToString();
            //medtab31 = ProdOld.Rows[30]["Medida"].ToString();
            //Combo31 = TextboxP31.Text;
            //if (Combo31 != Nome31)
            //{
            //    String SqlProd31= "Update materiaprima set Produto = @novo31 where Num=31";
            //    MySqlCommand ComProd31 = new MySqlCommand(SqlProd31, Conexao);
            //    ComProd31.Parameters.AddWithValue("@novo31", Combo31);
            //    ComProd31.ExecuteNonQuery();
            //}

            //if (medtab31 != Medida31)
            //{
            //    String SqlMed31 = "Update materiaprima set Medida = @medida31 where Num=31";
            //    MySqlCommand ComMed31 = new MySqlCommand(SqlMed31, Conexao);
            //    ComMed31.Parameters.AddWithValue("@medida31", Convert.ToInt32(Medida31));
            //    ComMed31.ExecuteNonQuery();
            //}
            //// Produto 32 = Confere e Salva Informações
            //String Nome32;
            //String Combo32;
            //String Medida32;
            //String medtab32;
            //Medida32 = "1";
            //if (Radio_L32g.Checked) { Medida32 = "1000"; }
            //if (Radio_L32Kg.Checked) { Medida32 = "1"; }

            //Nome32 = ProdOld.Rows[31]["Produto"].ToString();
            //medtab32 = ProdOld.Rows[31]["Medida"].ToString();
            //Combo32 = TextboxP32.Text;
            //if (Combo32 != Nome32)
            //{
            //    String SqlProd32 = "Update materiaprima set Produto = @novo32 where Num=32";
            //    MySqlCommand ComProd32 = new MySqlCommand(SqlProd32, Conexao);
            //    ComProd32.Parameters.AddWithValue("@novo32", Combo32);
            //    ComProd32.ExecuteNonQuery();
            //}

            //if (medtab32 != Medida32)
            //{
            //    String SqlMed32 = "Update materiaprima set Medida = @medida32 where Num=32";
            //    MySqlCommand ComMed32 = new MySqlCommand(SqlMed32, Conexao);
            //    ComMed32.Parameters.AddWithValue("@medida32", Convert.ToInt32(Medida32));
            //    ComMed32.ExecuteNonQuery();
            //}

            //// Produto 33 = Confere e Salva Informações
            //String Nome33;
            //String Combo33;
            //String Medida33;
            //String medtab33;
            //Medida33 = "1";
            //if (Radio_L33g.Checked) { Medida33 = "1000"; }
            //if (Radio_L33Kg.Checked) { Medida33 = "1"; }

            //Nome33 = ProdOld.Rows[32]["Produto"].ToString();
            //medtab33 = ProdOld.Rows[32]["Medida"].ToString();
            //Combo33 = TextboxP33.Text;
            //if (Combo33 != Nome33)
            //{
            //    String SqlProd33 = "Update materiaprima set Produto = @novo33 where Num=33";
            //    MySqlCommand ComProd33 = new MySqlCommand(SqlProd33, Conexao);
            //    ComProd33.Parameters.AddWithValue("@novo33", Combo33);
            //    ComProd33.ExecuteNonQuery();
            //}

            //if (medtab33 != Medida33)
            //{
            //    String SqlMed33 = "Update materiaprima set Medida = @medida33 where Num=33";
            //    MySqlCommand ComMed33 = new MySqlCommand(SqlMed33, Conexao);
            //    ComMed33.Parameters.AddWithValue("@medida33", Convert.ToInt32(Medida33));
            //    ComMed33.ExecuteNonQuery();
            //}

            //// Produto 34 = Confere e Salva Informações
            //String Nome34;
            //String Combo34;
            //String Medida34;
            //String medtab34;
            //Medida34 = "1";
            //if (Radio_L34g.Checked) { Medida34 = "1000"; }
            //if (Radio_L34Kg.Checked) { Medida34 = "1"; }

            //Nome34 = ProdOld.Rows[33]["Produto"].ToString();
            //medtab34 = ProdOld.Rows[33]["Medida"].ToString();
            //Combo34 = TextboxP34.Text;
            //if (Combo34 != Nome34)
            //{
            //    String SqlProd34 = "Update materiaprima set Produto = @novo34 where Num=34";
            //    MySqlCommand ComProd34 = new MySqlCommand(SqlProd34, Conexao);
            //    ComProd34.Parameters.AddWithValue("@novo34", Combo34);
            //    ComProd34.ExecuteNonQuery();
            //}

            //if (medtab34 != Medida34)
            //{
            //    String SqlMed34 = "Update materiaprima set Medida = @medida34 where Num=34";
            //    MySqlCommand ComMed34 = new MySqlCommand(SqlMed34, Conexao);
            //    ComMed34.Parameters.AddWithValue("@medida34", Convert.ToInt32(Medida34));
            //    ComMed34.ExecuteNonQuery();
            //}

            //// Produto 35 = Confere e Salva Informações
            //String Nome35;
            //String Combo35;
            //String Medida35;
            //String medtab35;
            //Medida35 = "1";
            //if (Radio_L35g.Checked) { Medida35 = "1000"; }
            //if (Radio_L35Kg.Checked) { Medida35 = "1"; }

            //Nome35 = ProdOld.Rows[34]["Produto"].ToString();
            //medtab35 = ProdOld.Rows[34]["Medida"].ToString();
            //Combo35 = TextboxP35.Text;
            //if (Combo35 != Nome35)
            //{
            //    String SqlProd35 = "Update materiaprima set Produto = @novo35 where Num=35";
            //    MySqlCommand ComProd35 = new MySqlCommand(SqlProd35, Conexao);
            //    ComProd35.Parameters.AddWithValue("@novo35", Combo35);
            //    ComProd35.ExecuteNonQuery();
            //}

            //if (medtab35 != Medida35)
            //{
            //    String SqlMed35 = "Update materiaprima set Medida = @medida35 where Num=35";
            //    MySqlCommand ComMed35 = new MySqlCommand(SqlMed35, Conexao);
            //    ComMed35.Parameters.AddWithValue("@medida35", Convert.ToInt32(Medida35));
            //    ComMed35.ExecuteNonQuery();
            //}

            //// Produto 36 = Confere e Salva Informações
            //String Nome36;
            //String Combo36;
            //String Medida36;
            //String medtab36;
            //Medida36 = "1";
            //if (Radio_L36g.Checked) { Medida36 = "1000"; }
            //if (Radio_L36Kg.Checked) { Medida36 = "1"; }

            //Nome36 = ProdOld.Rows[35]["Produto"].ToString();
            //medtab36 = ProdOld.Rows[35]["Medida"].ToString();
            //Combo36 = TextboxP36.Text;
            //if (Combo36 != Nome36)
            //{
            //    String SqlProd36 = "Update materiaprima set Produto = @novo36 where Num=36";
            //    MySqlCommand ComProd36 = new MySqlCommand(SqlProd36, Conexao);
            //    ComProd36.Parameters.AddWithValue("@novo36", Combo36);
            //    ComProd36.ExecuteNonQuery();
            //}

            //if (medtab36 != Medida36)
            //{
            //    String SqlMed36 = "Update materiaprima set Medida = @medida36 where Num=36";
            //    MySqlCommand ComMed36 = new MySqlCommand(SqlMed36, Conexao);
            //    ComMed36.Parameters.AddWithValue("@medida36", Convert.ToInt32(Medida36));
            //    ComMed36.ExecuteNonQuery();
            //}

            //// Produto 37 = Confere e Salva Informações
            //String Nome37;
            //String Combo37;
            //String Medida37;
            //String medtab37;
            //Medida37 = "1";
            //if (Radio_L37g.Checked) { Medida37 = "1000"; }
            //if (Radio_L37Kg.Checked) { Medida37 = "1"; }

            //Nome37 = ProdOld.Rows[36]["Produto"].ToString();
            //medtab37 = ProdOld.Rows[36]["Medida"].ToString();
            //Combo37 = TextboxP37.Text;
            //if (Combo37 != Nome37)
            //{
            //    String SqlProd37 = "Update materiaprima set Produto = @novo37 where Num=37";
            //    MySqlCommand ComProd37 = new MySqlCommand(SqlProd37, Conexao);
            //    ComProd37.Parameters.AddWithValue("@novo37", Combo37);
            //    ComProd37.ExecuteNonQuery();
            //}

            //if (medtab37 != Medida37)
            //{
            //    String SqlMed37 = "Update materiaprima set Medida = @medida37 where Num=37";
            //    MySqlCommand ComMed37 = new MySqlCommand(SqlMed37, Conexao);
            //    ComMed37.Parameters.AddWithValue("@medida37", Convert.ToInt32(Medida37));
            //    ComMed37.ExecuteNonQuery();
            //}


            //// Produto 38 = Confere e Salva Informações
            //String Nome38;
            //String Combo38;
            //String Medida38;
            //String medtab38;
            //Medida38 = "1";
            //if (Radio_L38g.Checked) { Medida38 = "1000"; }
            //if (Radio_L38Kg.Checked) { Medida38 = "1"; }

            //Nome38 = ProdOld.Rows[37]["Produto"].ToString();
            //medtab38 = ProdOld.Rows[37]["Medida"].ToString();
            //Combo38 = TextboxP38.Text;
            //if (Combo38 != Nome38)
            //{
            //    String SqlProd38 = "Update materiaprima set Produto = @novo38 where Num=38";
            //    MySqlCommand ComProd38 = new MySqlCommand(SqlProd38, Conexao);
            //    ComProd38.Parameters.AddWithValue("@novo38", Combo38);
            //    ComProd38.ExecuteNonQuery();
            //}

            //if (medtab38 != Medida38)
            //{
            //    String SqlMed38 = "Update materiaprima set Medida = @medida38 where Num=38";
            //    MySqlCommand ComMed38 = new MySqlCommand(SqlMed38, Conexao);
            //    ComMed38.Parameters.AddWithValue("@medida38", Convert.ToInt32(Medida38));
            //    ComMed38.ExecuteNonQuery();
            //}


            //// Produto 39 = Confere e Salva Informações
            //String Nome39;
            //String Combo39;
            //String Medida39;
            //String medtab39;
            //Medida39 = "1";
            //if (Radio_L39g.Checked) { Medida39 = "1000"; }
            //if (Radio_L39Kg.Checked) { Medida39 = "1"; }

            //Nome39 = ProdOld.Rows[38]["Produto"].ToString();
            //medtab39 = ProdOld.Rows[38]["Medida"].ToString();
            //Combo39 = TextboxP39.Text;
            //if (Combo39 != Nome39)
            //{
            //    String SqlProd39 = "Update materiaprima set Produto = @novo39 where Num=39";
            //    MySqlCommand ComProd39 = new MySqlCommand(SqlProd39, Conexao);
            //    ComProd39.Parameters.AddWithValue("@novo39", Combo39);
            //    ComProd39.ExecuteNonQuery();
            //}

            //if (medtab39 != Medida39)
            //{
            //    String SqlMed39 = "Update materiaprima set Medida = @medida39 where Num=39";
            //    MySqlCommand ComMed39 = new MySqlCommand(SqlMed39, Conexao);
            //    ComMed39.Parameters.AddWithValue("@medida39", Convert.ToInt32(Medida39));
            //    ComMed39.ExecuteNonQuery();
            //}


            //// Produto 40 = Confere e Salva Informações
            //String Nome40;
            //String Combo40;
            //String Medida40;
            //String medtab40;
            //Medida40 = "1";
            //if (Radio_L40g.Checked) { Medida40 = "1000"; }
            //if (Radio_L40Kg.Checked) { Medida40 = "1"; }

            //Nome40 = ProdOld.Rows[39]["Produto"].ToString();
            //medtab40 = ProdOld.Rows[39]["Medida"].ToString();
            //Combo40 = TextboxP40.Text;
            //if (Combo40 != Nome40)
            //{
            //    String SqlProd40 = "Update materiaprima set Produto = @novo40 where Num=40";
            //    MySqlCommand ComProd40 = new MySqlCommand(SqlProd40, Conexao);
            //    ComProd40.Parameters.AddWithValue("@novo40", Combo40);
            //    ComProd40.ExecuteNonQuery();
            //}

            //if (medtab40 != Medida40)
            //{
            //    String SqlMed40 = "Update materiaprima set Medida = @medida40 where Num=40";
            //    MySqlCommand ComMed40 = new MySqlCommand(SqlMed40, Conexao);
            //    ComMed40.Parameters.AddWithValue("@medida40", Convert.ToInt32(Medida40));
            //    ComMed40.ExecuteNonQuery();
            //}

            //// Produto 41 = Confere e Salva Informações
            //String Nome41;
            //String Combo41;
            //String Medida41;
            //String medtab41;
            //Medida41 = "1";
            //if (Radio_L41g.Checked) { Medida41 = "1000"; }
            //if (Radio_L41Kg.Checked) { Medida41 = "1"; }

            //Nome41 = ProdOld.Rows[40]["Produto"].ToString();
            //medtab41 = ProdOld.Rows[40]["Medida"].ToString();
            //Combo41 = TextboxP41.Text;
            //if (Combo41 != Nome41)
            //{
            //    String SqlProd41 = "Update materiaprima set Produto = @novo41 where Num=41";
            //    MySqlCommand ComProd41 = new MySqlCommand(SqlProd41, Conexao);
            //    ComProd41.Parameters.AddWithValue("@novo41", Combo41);
            //    ComProd41.ExecuteNonQuery();
            //}

            //if (medtab41 != Medida41)
            //{
            //    String SqlMed41 = "Update materiaprima set Medida = @medida41 where Num=41";
            //    MySqlCommand ComMed41 = new MySqlCommand(SqlMed41, Conexao);
            //    ComMed41.Parameters.AddWithValue("@medida41", Convert.ToInt32(Medida41));
            //    ComMed41.ExecuteNonQuery();
            //}


            //// Produto 42 = Confere e Salva Informações
            //String Nome42;
            //String Combo42;
            //String Medida42;
            //String medtab42;
            //Medida42 = "1";
            //if (Radio_L42g.Checked) { Medida42 = "1000"; }
            //if (Radio_L42Kg.Checked) { Medida42 = "1"; }

            //Nome42 = ProdOld.Rows[41]["Produto"].ToString();
            //medtab42 = ProdOld.Rows[41]["Medida"].ToString();
            //Combo42 = TextboxP42.Text;
            //if (Combo42 != Nome42)
            //{
            //    String SqlProd42 = "Update materiaprima set Produto = @novo42 where Num=42";
            //    MySqlCommand ComProd42 = new MySqlCommand(SqlProd42, Conexao);
            //    ComProd42.Parameters.AddWithValue("@novo42", Combo42);
            //    ComProd42.ExecuteNonQuery();
            //}

            //if (medtab42 != Medida42)
            //{
            //    String SqlMed42 = "Update materiaprima set Medida = @medida42 where Num=42";
            //    MySqlCommand ComMed42 = new MySqlCommand(SqlMed42, Conexao);
            //    ComMed42.Parameters.AddWithValue("@medida42", Convert.ToInt32(Medida42));
            //    ComMed42.ExecuteNonQuery();
            //}

            //// Produto 43 = Confere e Salva Informações
            //String Nome43;
            //String Combo43;
            //String Medida43;
            //String medtab43;
            //Medida43 = "1";
            //if (Radio_L43g.Checked) { Medida43 = "1000"; }
            //if (Radio_L43Kg.Checked) { Medida43 = "1"; }

            //Nome43 = ProdOld.Rows[42]["Produto"].ToString();
            //medtab43 = ProdOld.Rows[42]["Medida"].ToString();
            //Combo43 = TextboxP43.Text;
            //if (Combo43 != Nome43)
            //{
            //    String SqlProd43 = "Update materiaprima set Produto = @novo43 where Num=43";
            //    MySqlCommand ComProd43 = new MySqlCommand(SqlProd43, Conexao);
            //    ComProd43.Parameters.AddWithValue("@novo43", Combo43);
            //    ComProd43.ExecuteNonQuery();
            //}

            //if (medtab43 != Medida43)
            //{
            //    String SqlMed43 = "Update materiaprima set Medida = @medida43 where Num=43";
            //    MySqlCommand ComMed43 = new MySqlCommand(SqlMed43, Conexao);
            //    ComMed43.Parameters.AddWithValue("@medida43", Convert.ToInt32(Medida43));
            //    ComMed43.ExecuteNonQuery();
            //}

            //// Produto 44 = Confere e Salva Informações
            //String Nome44;
            //String Combo44;
            //String Medida44;
            //String medtab44;
            //Medida44 = "1";
            //if (Radio_L44g.Checked) { Medida44 = "1000"; }
            //if (Radio_L44Kg.Checked) { Medida44 = "1"; }

            //Nome44 = ProdOld.Rows[43]["Produto"].ToString();
            //medtab44 = ProdOld.Rows[43]["Medida"].ToString();
            //Combo44 = TextboxP44.Text;
            //if (Combo44 != Nome44)
            //{
            //    String SqlProd44 = "Update materiaprima set Produto = @novo44 where Num=44";
            //    MySqlCommand ComProd44 = new MySqlCommand(SqlProd44, Conexao);
            //    ComProd44.Parameters.AddWithValue("@novo44", Combo44);
            //    ComProd44.ExecuteNonQuery();
            //}

            //if (medtab44 != Medida44)
            //{
            //    String SqlMed44 = "Update materiaprima set Medida = @medida44 where Num=44";
            //    MySqlCommand ComMed44 = new MySqlCommand(SqlMed44, Conexao);
            //    ComMed44.Parameters.AddWithValue("@medida44", Convert.ToInt32(Medida44));
            //    ComMed44.ExecuteNonQuery();
            //}

            //// Produto 45 = Confere e Salva Informações
            //String Nome45;
            //String Combo45;
            //String Medida45;
            //String medtab45;
            //Medida45 = "1";
            //if (Radio_L45g.Checked) { Medida45 = "1000"; }
            //if (Radio_L45Kg.Checked) { Medida45 = "1"; }

            //Nome45 = ProdOld.Rows[44]["Produto"].ToString();
            //medtab45 = ProdOld.Rows[44]["Medida"].ToString();
            //Combo45 = TextboxP45.Text;
            //if (Combo45 != Nome45)
            //{
            //    String SqlProd45 = "Update materiaprima set Produto = @novo45 where Num=45";
            //    MySqlCommand ComProd45 = new MySqlCommand(SqlProd45, Conexao);
            //    ComProd45.Parameters.AddWithValue("@novo45", Combo45);
            //    ComProd45.ExecuteNonQuery();
            //}

            //if (medtab45 != Medida45)
            //{
            //    String SqlMed45 = "Update materiaprima set Medida = @medida45 where Num=45";
            //    MySqlCommand ComMed45 = new MySqlCommand(SqlMed45, Conexao);
            //    ComMed45.Parameters.AddWithValue("@medida45", Convert.ToInt32(Medida45));
            //    ComMed45.ExecuteNonQuery();
            //}

            //// Produto 46 = Confere e Salva Informações
            //String Nome46;
            //String Combo46;
            //String Medida46;
            //String medtab46;
            //Medida46 = "1";
            //if (Radio_L46g.Checked) { Medida46 = "1000"; }
            //if (Radio_L46Kg.Checked) { Medida46 = "1"; }

            //Nome46 = ProdOld.Rows[45]["Produto"].ToString();
            //medtab46 = ProdOld.Rows[45]["Medida"].ToString();
            //Combo46 = TextboxP46.Text;
            //if (Combo46 != Nome46)
            //{
            //    String SqlProd46 = "Update materiaprima set Produto = @novo46 where Num=46";
            //    MySqlCommand ComProd46 = new MySqlCommand(SqlProd46, Conexao);
            //    ComProd46.Parameters.AddWithValue("@novo46", Combo46);
            //    ComProd46.ExecuteNonQuery();
            //}

            //if (medtab46 != Medida46)
            //{
            //    String SqlMed46 = "Update materiaprima set Medida = @medida46 where Num=46";
            //    MySqlCommand ComMed46 = new MySqlCommand(SqlMed46, Conexao);
            //    ComMed46.Parameters.AddWithValue("@medida46", Convert.ToInt32(Medida46));
            //    ComMed46.ExecuteNonQuery();
            //}

            //// Produto 47 = Confere e Salva Informações
            //String Nome47;
            //String Combo47;
            //String Medida47;
            //String medtab47;
            //Medida47 = "1";
            //if (Radio_L47g.Checked) { Medida47 = "1000"; }
            //if (Radio_L47Kg.Checked) { Medida47 = "1"; }

            //Nome47 = ProdOld.Rows[46]["Produto"].ToString();
            //medtab47 = ProdOld.Rows[46]["Medida"].ToString();
            //Combo47 = TextboxP47.Text;
            //if (Combo47 != Nome47)
            //{
            //    String SqlProd47 = "Update materiaprima set Produto = @novo47 where Num=47";
            //    MySqlCommand ComProd47 = new MySqlCommand(SqlProd47, Conexao);
            //    ComProd47.Parameters.AddWithValue("@novo47", Combo47);
            //    ComProd47.ExecuteNonQuery();
            //}

            //if (medtab47 != Medida47)
            //{
            //    String SqlMed47 = "Update materiaprima set Medida = @medida47 where Num=47";
            //    MySqlCommand ComMed47 = new MySqlCommand(SqlMed47, Conexao);
            //    ComMed47.Parameters.AddWithValue("@medida47", Convert.ToInt32(Medida47));
            //    ComMed47.ExecuteNonQuery();
            //}

            //// Produto 48 = Confere e Salva Informações
            //String Nome48;
            //String Combo48;
            //String Medida48;
            //String medtab48;
            //Medida48 = "1";
            //if (Radio_L48g.Checked) { Medida48 = "1000"; }
            //if (Radio_L48Kg.Checked) { Medida48 = "1"; }

            //Nome48 = ProdOld.Rows[47]["Produto"].ToString();
            //medtab48 = ProdOld.Rows[47]["Medida"].ToString();
            //Combo48 = TextboxP48.Text;
            //if (Combo48 != Nome48)
            //{
            //    String SqlProd48 = "Update materiaprima set Produto = @novo48 where Num=48";
            //    MySqlCommand ComProd48 = new MySqlCommand(SqlProd48, Conexao);
            //    ComProd48.Parameters.AddWithValue("@novo48", Combo48);
            //    ComProd48.ExecuteNonQuery();
            //}

            //if (medtab48 != Medida48)
            //{
            //    String SqlMed48 = "Update materiaprima set Medida = @medida48 where Num=48";
            //    MySqlCommand ComMed48 = new MySqlCommand(SqlMed48, Conexao);
            //    ComMed48.Parameters.AddWithValue("@medida48", Convert.ToInt32(Medida48));
            //    ComMed48.ExecuteNonQuery();
            //}

            //// Produto 49 = Confere e Salva Informações
            //String Nome49;
            //String Combo49;
            //String Medida49;
            //String medtab49;
            //Medida49 = "1";
            //if (Radio_L49g.Checked) { Medida49 = "1000"; }
            //if (Radio_L49Kg.Checked) { Medida49 = "1"; }

            //Nome49 = ProdOld.Rows[48]["Produto"].ToString();
            //medtab49 = ProdOld.Rows[48]["Medida"].ToString();
            //Combo49 = TextboxP49.Text;
            //if (Combo49 != Nome49)
            //{
            //    String SqlProd49 = "Update materiaprima set Produto = @novo49 where Num=49";
            //    MySqlCommand ComProd49 = new MySqlCommand(SqlProd49, Conexao);
            //    ComProd49.Parameters.AddWithValue("@novo49", Combo49);
            //    ComProd49.ExecuteNonQuery();
            //}

            //if (medtab49 != Medida49)
            //{
            //    String SqlMed49 = "Update materiaprima set Medida = @medida49 where Num=49";
            //    MySqlCommand ComMed49 = new MySqlCommand(SqlMed49, Conexao);
            //    ComMed49.Parameters.AddWithValue("@medida49", Convert.ToInt32(Medida49));
            //    ComMed49.ExecuteNonQuery();
            //}

            //// Produto 50 = Confere e Salva Informações
            //String Nome50;
            //String Combo50;
            //String Medida50;
            //String medtab50;
            //Medida50 = "1";
            //if (Radio_L50g.Checked) { Medida50 = "1000"; }
            //if (Radio_L50Kg.Checked) { Medida50 = "1"; }

            //Nome50 = ProdOld.Rows[49]["Produto"].ToString();
            //medtab50 = ProdOld.Rows[49]["Medida"].ToString();
            //Combo50 = TextboxP50.Text;
            //if (Combo50 != Nome50)
            //{
            //    String SqlProd50 = "Update materiaprima set Produto = @novo50 where Num=50";
            //    MySqlCommand ComProd50 = new MySqlCommand(SqlProd50, Conexao);
            //    ComProd50.Parameters.AddWithValue("@novo50", Combo50);
            //    ComProd50.ExecuteNonQuery();
            //}

            //if (medtab50 != Medida50)
            //{
            //    String SqlMed50 = "Update materiaprima set Medida = @medida50 where Num=50";
            //    MySqlCommand ComMed50 = new MySqlCommand(SqlMed50, Conexao);
            //    ComMed50.Parameters.AddWithValue("@medida50", Convert.ToInt32(Medida50));
            //    ComMed50.ExecuteNonQuery();
            //}


            //// Produto 51 = Confere e Salva Informações
            //String Nome51;
            //String Combo51;
            //String Medida51;
            //String medtab51;
            //Medida51 = "1";
            //if (Radio_L51g.Checked) { Medida51 = "1000"; }
            //if (Radio_L51Kg.Checked) { Medida51 = "1"; }

            //Nome51 = ProdOld.Rows[50]["Produto"].ToString();
            //medtab51 = ProdOld.Rows[50]["Medida"].ToString();
            //Combo51 = TextboxP51.Text;
            //if (Combo51 != Nome51)
            //{
            //    String SqlProd51 = "Update materiaprima set Produto = @novo51 where Num=51";
            //    MySqlCommand ComProd51 = new MySqlCommand(SqlProd51, Conexao);
            //    ComProd51.Parameters.AddWithValue("@novo51", Combo51);
            //    ComProd51.ExecuteNonQuery();
            //}

            //if (medtab51 != Medida51)
            //{
            //    String SqlMed51 = "Update materiaprima set Medida = @medida51 where Num=51";
            //    MySqlCommand ComMed51 = new MySqlCommand(SqlMed51, Conexao);
            //    ComMed51.Parameters.AddWithValue("@medida51", Convert.ToInt32(Medida51));
            //    ComMed51.ExecuteNonQuery();
            //}

            //// Produto 52 = Confere e Salva Informações
            //String Nome52;
            //String Combo52;
            //String Medida52;
            //String medtab52;
            //Medida52 = "1";
            //if (Radio_L52g.Checked) { Medida52 = "1000"; }
            //if (Radio_L52Kg.Checked) { Medida52 = "1"; }

            //Nome52 = ProdOld.Rows[51]["Produto"].ToString();
            //medtab52 = ProdOld.Rows[51]["Medida"].ToString();
            //Combo52 = TextboxP52.Text;
            //if (Combo52 != Nome52)
            //{
            //    String SqlProd52 = "Update materiaprima set Produto = @novo52 where Num=52";
            //    MySqlCommand ComProd52 = new MySqlCommand(SqlProd52, Conexao);
            //    ComProd52.Parameters.AddWithValue("@novo52", Combo52);
            //    ComProd52.ExecuteNonQuery();
            //}

            //if (medtab52 != Medida52)
            //{
            //    String SqlMed52 = "Update materiaprima set Medida = @medida52 where Num=52";
            //    MySqlCommand ComMed52 = new MySqlCommand(SqlMed52, Conexao);
            //    ComMed52.Parameters.AddWithValue("@medida52", Convert.ToInt32(Medida52));
            //    ComMed52.ExecuteNonQuery();
            //}

            //// Produto 53 = Confere e Salva Informações
            //String Nome53;
            //String Combo53;
            //String Medida53;
            //String medtab53;
            //Medida53 = "1";
            //if (Radio_L53g.Checked) { Medida53 = "1000"; }
            //if (Radio_L53Kg.Checked) { Medida53 = "1"; }

            //Nome53 = ProdOld.Rows[52]["Produto"].ToString();
            //medtab53 = ProdOld.Rows[52]["Medida"].ToString();
            //Combo53 = TextboxP53.Text;
            //if (Combo53 != Nome53)
            //{
            //    String SqlProd53 = "Update materiaprima set Produto = @novo53 where Num=53";
            //    MySqlCommand ComProd53 = new MySqlCommand(SqlProd53, Conexao);
            //    ComProd53.Parameters.AddWithValue("@novo53", Combo53);
            //    ComProd53.ExecuteNonQuery();
            //}

            //if (medtab53 != Medida53)
            //{
            //    String SqlMed53 = "Update materiaprima set Medida = @medida53 where Num=53";
            //    MySqlCommand ComMed53 = new MySqlCommand(SqlMed53, Conexao);
            //    ComMed53.Parameters.AddWithValue("@medida53", Convert.ToInt32(Medida53));
            //    ComMed53.ExecuteNonQuery();
            //}

            //// Produto 54 = Confere e Salva Informações
            //String Nome54;
            //String Combo54;
            //String Medida54;
            //String medtab54;
            //Medida54 = "1";
            //if (Radio_L54g.Checked) { Medida54 = "1000"; }
            //if (Radio_L54Kg.Checked) { Medida54 = "1"; }

            //Nome54 = ProdOld.Rows[53]["Produto"].ToString();
            //medtab54 = ProdOld.Rows[53]["Medida"].ToString();
            //Combo54 = TextboxP54.Text;
            //if (Combo54 != Nome54)
            //{
            //    String SqlProd54 = "Update materiaprima set Produto = @novo54 where Num=54";
            //    MySqlCommand ComProd54 = new MySqlCommand(SqlProd54, Conexao);
            //    ComProd54.Parameters.AddWithValue("@novo54", Combo54);
            //    ComProd54.ExecuteNonQuery();
            //}

            //if (medtab54 != Medida54)
            //{
            //    String SqlMed54 = "Update materiaprima set Medida = @medida54 where Num=54";
            //    MySqlCommand ComMed54 = new MySqlCommand(SqlMed54, Conexao);
            //    ComMed54.Parameters.AddWithValue("@medida54", Convert.ToInt32(Medida54));
            //    ComMed54.ExecuteNonQuery();
            //}

            //// Produto 55 = Confere e Salva Informações
            //String Nome55;
            //String Combo55;
            //String Medida55;
            //String medtab55;
            //Medida55 = "1";
            //if (Radio_L55g.Checked) { Medida55 = "1000"; }
            //if (Radio_L55Kg.Checked) { Medida55 = "1"; }

            //Nome55 = ProdOld.Rows[54]["Produto"].ToString();
            //medtab55 = ProdOld.Rows[54]["Medida"].ToString();
            //Combo55 = TextboxP55.Text;
            //if (Combo55 != Nome55)
            //{
            //    String SqlProd55 = "Update materiaprima set Produto = @novo55 where Num=55";
            //    MySqlCommand ComProd55 = new MySqlCommand(SqlProd55, Conexao);
            //    ComProd55.Parameters.AddWithValue("@novo55", Combo55);
            //    ComProd55.ExecuteNonQuery();
            //}

            //if (medtab55 != Medida55)
            //{
            //    String SqlMed55 = "Update materiaprima set Medida = @medida55 where Num=55";
            //    MySqlCommand ComMed55 = new MySqlCommand(SqlMed55, Conexao);
            //    ComMed55.Parameters.AddWithValue("@medida55", Convert.ToInt32(Medida55));
            //    ComMed55.ExecuteNonQuery();
            //}

            //// Produto 56 = Confere e Salva Informações
            //String Nome56;
            //String Combo56;
            //String Medida56;
            //String medtab56;
            //Medida56 = "1";
            //if (Radio_L56g.Checked) { Medida56 = "1000"; }
            //if (Radio_L56Kg.Checked) { Medida56 = "1"; }

            //Nome56 = ProdOld.Rows[55]["Produto"].ToString();
            //medtab56 = ProdOld.Rows[55]["Medida"].ToString();
            //Combo56 = TextboxP56.Text;
            //if (Combo56 != Nome56)
            //{
            //    String SqlProd56 = "Update materiaprima set Produto = @novo56 where Num=56";
            //    MySqlCommand ComProd56 = new MySqlCommand(SqlProd56, Conexao);
            //    ComProd56.Parameters.AddWithValue("@novo56", Combo56);
            //    ComProd56.ExecuteNonQuery();
            //}

            //if (medtab56 != Medida56)
            //{
            //    String SqlMed56 = "Update materiaprima set Medida = @medida56 where Num=560";
            //    MySqlCommand ComMed56 = new MySqlCommand(SqlMed56, Conexao);
            //    ComMed56.Parameters.AddWithValue("@medida56", Convert.ToInt32(Medida56));
            //    ComMed56.ExecuteNonQuery();
            //}

            //// Produto 57 = Confere e Salva Informações
            //String Nome57;
            //String Combo57;
            //String Medida57;
            //String medtab57;
            //Medida57 = "1";
            //if (Radio_L57g.Checked) { Medida57 = "1000"; }
            //if (Radio_L57Kg.Checked) { Medida57 = "1"; }

            //Nome57 = ProdOld.Rows[56]["Produto"].ToString();
            //medtab57 = ProdOld.Rows[56]["Medida"].ToString();
            //Combo57 = TextboxP57.Text;
            //if (Combo57 != Nome57)
            //{
            //    String SqlProd57 = "Update materiaprima set Produto = @novo57 where Num=57";
            //    MySqlCommand ComProd57 = new MySqlCommand(SqlProd57, Conexao);
            //    ComProd57.Parameters.AddWithValue("@novo57", Combo57);
            //    ComProd57.ExecuteNonQuery();
            //}

            //if (medtab57 != Medida57)
            //{
            //    String SqlMed57 = "Update materiaprima set Medida = @medida57 where Num=57";
            //    MySqlCommand ComMed57 = new MySqlCommand(SqlMed57, Conexao);
            //    ComMed57.Parameters.AddWithValue("@medida57", Convert.ToInt32(Medida57));
            //    ComMed57.ExecuteNonQuery();
            //}

            //// Produto 58 = Confere e Salva Informações
            //String Nome58;
            //String Combo58;
            //String Medida58;
            //String medtab58;
            //Medida58 = "1";
            //if (Radio_L58g.Checked) { Medida58 = "1000"; }
            //if (Radio_L58Kg.Checked) { Medida58 = "1"; }

            //Nome58 = ProdOld.Rows[57]["Produto"].ToString();
            //medtab58 = ProdOld.Rows[57]["Medida"].ToString();
            //Combo58 = TextboxP58.Text;
            //if (Combo58 != Nome58)
            //{
            //    String SqlProd58 = "Update materiaprima set Produto = @novo58 where Num=58";
            //    MySqlCommand ComProd58 = new MySqlCommand(SqlProd58, Conexao);
            //    ComProd58.Parameters.AddWithValue("@novo58", Combo58);
            //    ComProd58.ExecuteNonQuery();
            //}

            //if (medtab58 != Medida58)
            //{
            //    String SqlMed58 = "Update materiaprima set Medida = @medida58 where Num=58";
            //    MySqlCommand ComMed58 = new MySqlCommand(SqlMed58, Conexao);
            //    ComMed58.Parameters.AddWithValue("@medida58", Convert.ToInt32(Medida58));
            //    ComMed58.ExecuteNonQuery();
            //}

            //// Produto 59 = Confere e Salva Informações
            //String Nome59;
            //String Combo59;
            //String Medida59;
            //String medtab59;
            //Medida59 = "1";
            //if (Radio_L59g.Checked) { Medida59 = "1000"; }
            //if (Radio_L59Kg.Checked) { Medida59 = "1"; }

            //Nome59 = ProdOld.Rows[58]["Produto"].ToString();
            //medtab59 = ProdOld.Rows[58]["Medida"].ToString();
            //Combo59 = TextboxP59.Text;
            //if (Combo59 != Nome59)
            //{
            //    String SqlProd59 = "Update materiaprima set Produto = @novo59 where Num=59";
            //    MySqlCommand ComProd59 = new MySqlCommand(SqlProd59, Conexao);
            //    ComProd59.Parameters.AddWithValue("@novo59", Combo59);
            //    ComProd59.ExecuteNonQuery();
            //}

            //if (medtab59 != Medida59)
            //{
            //    String SqlMed59 = "Update materiaprima set Medida = @medida59 where Num=59";
            //    MySqlCommand ComMed59 = new MySqlCommand(SqlMed59, Conexao);
            //    ComMed59.Parameters.AddWithValue("@medida59", Convert.ToInt32(Medida59));
            //    ComMed59.ExecuteNonQuery();
            //}

            //// Produto 60 = Confere e Salva Informações
            //String Nome60;
            //String Combo60;
            //String Medida60;
            //String medtab60;
            //Medida60 = "1";
            //if (Radio_L60g.Checked) { Medida60 = "1000"; }
            //if (Radio_L60Kg.Checked) { Medida60 = "1"; }

            //Nome60 = ProdOld.Rows[59]["Produto"].ToString();
            //medtab60 = ProdOld.Rows[59]["Medida"].ToString();
            //Combo60 = TextboxP60.Text;
            //if (Combo60 != Nome60)
            //{
            //    String SqlProd60 = "Update materiaprima set Produto = @novo60 where Num=60";
            //    MySqlCommand ComProd60 = new MySqlCommand(SqlProd60, Conexao);
            //    ComProd60.Parameters.AddWithValue("@novo60", Combo60);
            //    ComProd60.ExecuteNonQuery();
            //}

            //if (medtab60 != Medida60)
            //{
            //    String SqlMed60 = "Update materiaprima set Medida = @medida60 where Num=60";
            //    MySqlCommand ComMed60 = new MySqlCommand(SqlMed60, Conexao);
            //    ComMed60.Parameters.AddWithValue("@medida60", Convert.ToInt32(Medida60));
            //    ComMed60.ExecuteNonQuery();
            //}

            //// Produto 61 = Confere e Salva Informações
            //String Nome61;
            //String Combo61;
            //String Medida61;
            //String medtab61;
            //Medida61 = "1";
            //if (Radio_L61g.Checked) { Medida61 = "1000"; }
            //if (Radio_L61Kg.Checked) { Medida61 = "1"; }

            //Nome61 = ProdOld.Rows[60]["Produto"].ToString();
            //medtab61 = ProdOld.Rows[60]["Medida"].ToString();
            //Combo61 = TextboxP61.Text;
            //if (Combo61 != Nome61)
            //{
            //    String SqlProd61 = "Update materiaprima set Produto = @novo61 where Num=61";
            //    MySqlCommand ComProd61 = new MySqlCommand(SqlProd61, Conexao);
            //    ComProd61.Parameters.AddWithValue("@novo61", Combo61);
            //    ComProd61.ExecuteNonQuery();
            //}

            //if (medtab61 != Medida61)
            //{
            //    String SqlMed61 = "Update materiaprima set Medida = @medida61 where Num=61";
            //    MySqlCommand ComMed61 = new MySqlCommand(SqlMed61, Conexao);
            //    ComMed61.Parameters.AddWithValue("@medida61", Convert.ToInt32(Medida61));
            //    ComMed61.ExecuteNonQuery();
            //}

            //// Produto 62 = Confere e Salva Informações
            //String Nome62;
            //String Combo62;
            //String Medida62;
            //String medtab62;
            //Medida62 = "1";
            //if (Radio_L62g.Checked) { Medida62 = "1000"; }
            //if (Radio_L62Kg.Checked) { Medida62 = "1"; }

            //Nome62 = ProdOld.Rows[61]["Produto"].ToString();
            //medtab62 = ProdOld.Rows[61]["Medida"].ToString();
            //Combo62 = TextboxP62.Text;
            //if (Combo62 != Nome62)
            //{
            //    String SqlProd62 = "Update materiaprima set Produto = @novo62 where Num=62";
            //    MySqlCommand ComProd62 = new MySqlCommand(SqlProd62, Conexao);
            //    ComProd62.Parameters.AddWithValue("@novo62", Combo62);
            //    ComProd62.ExecuteNonQuery();
            //}

            //if (medtab62 != Medida62)
            //{
            //    String SqlMed62 = "Update materiaprima set Medida = @medida62 where Num=62";
            //    MySqlCommand ComMed62 = new MySqlCommand(SqlMed62, Conexao);
            //    ComMed62.Parameters.AddWithValue("@medida62", Convert.ToInt32(Medida62));
            //    ComMed62.ExecuteNonQuery();
            //}

            //// Produto 63 = Confere e Salva Informações
            //String Nome63;
            //String Combo63;
            //String Medida63;
            //String medtab63;
            //Medida63 = "1";
            //if (Radio_L63g.Checked) { Medida63 = "1000"; }
            //if (Radio_L63Kg.Checked) { Medida63 = "1"; }

            //Nome63 = ProdOld.Rows[62]["Produto"].ToString();
            //medtab63 = ProdOld.Rows[62]["Medida"].ToString();
            //Combo63 = TextboxP63.Text;
            //if (Combo63 != Nome63)
            //{
            //    String SqlProd63 = "Update materiaprima set Produto = @novo63 where Num=63";
            //    MySqlCommand ComProd63 = new MySqlCommand(SqlProd63, Conexao);
            //    ComProd63.Parameters.AddWithValue("@novo63", Combo63);
            //    ComProd63.ExecuteNonQuery();
            //}

            //if (medtab63 != Medida63)
            //{
            //    String SqlMed63 = "Update materiaprima set Medida = @medida63 where Num=63";
            //    MySqlCommand ComMed63 = new MySqlCommand(SqlMed63, Conexao);
            //    ComMed63.Parameters.AddWithValue("@medida63", Convert.ToInt32(Medida63));
            //    ComMed63.ExecuteNonQuery();
            //}

            //// Produto 64 = Confere e Salva Informações
            //String Nome64;
            //String Combo64;
            //String Medida64;
            //String medtab64;
            //Medida64 = "1";
            //if (Radio_L64g.Checked) { Medida64 = "1000"; }
            //if (Radio_L64Kg.Checked) { Medida64 = "1"; }

            //Nome64 = ProdOld.Rows[63]["Produto"].ToString();
            //medtab64 = ProdOld.Rows[63]["Medida"].ToString();
            //Combo64 = TextboxP64.Text;
            //if (Combo64 != Nome64)
            //{
            //    String SqlProd64 = "Update materiaprima set Produto = @novo64 where Num=64";
            //    MySqlCommand ComProd64 = new MySqlCommand(SqlProd64, Conexao);
            //    ComProd64.Parameters.AddWithValue("@novo64", Combo64);
            //    ComProd64.ExecuteNonQuery();
            //}

            //if (medtab64 != Medida64)
            //{
            //    String SqlMed64 = "Update materiaprima set Medida = @medida64 where Num=64";
            //    MySqlCommand ComMed64 = new MySqlCommand(SqlMed64, Conexao);
            //    ComMed64.Parameters.AddWithValue("@medida64", Convert.ToInt32(Medida64));
            //    ComMed64.ExecuteNonQuery();
            //}

            //// Produto 65 = Confere e Salva Informações
            //String Nome65;
            //String Combo65;
            //String Medida65;
            //String medtab65;
            //Medida65 = "1";
            //if (Radio_L65g.Checked) { Medida65 = "1000"; }
            //if (Radio_L65Kg.Checked) { Medida65 = "1"; }

            //Nome65 = ProdOld.Rows[64]["Produto"].ToString();
            //medtab65 = ProdOld.Rows[64]["Medida"].ToString();
            //Combo65 = TextboxP65.Text;
            //if (Combo65 != Nome65)
            //{
            //    String SqlProd65 = "Update materiaprima set Produto = @novo65 where Num=65";
            //    MySqlCommand ComProd65 = new MySqlCommand(SqlProd65, Conexao);
            //    ComProd65.Parameters.AddWithValue("@novo65", Combo65);
            //    ComProd65.ExecuteNonQuery();
            //}

            //if (medtab65 != Medida65)
            //{
            //    String SqlMed65 = "Update materiaprima set Medida = @medida65 where Num=65";
            //    MySqlCommand ComMed65 = new MySqlCommand(SqlMed65, Conexao);
            //    ComMed65.Parameters.AddWithValue("@medida65", Convert.ToInt32(Medida65));
            //    ComMed65.ExecuteNonQuery();
            //}

            Conexao.Close();

        }

        private void Radio_L1g_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void MatPrima_Load(object sender, EventArgs e)
        {
            Carregar_produtos();
            Salvar_MP.Enabled = false;
            Cancelar_MP.Enabled = false;
            Travar_box();
        }

        private void Cancelar_MP_Click(object sender, EventArgs e)
        {
            if(Salvar_MP.Enabled == true)
            {
                Carregar_produtos();
            }
            Salvar_MP.Enabled = false;
            Cancelar_MP.Enabled = false;
            Travar_box();
        }

        private void Sair_MP_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TextboxP1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Carregar_produtos();
        }

        private void label38_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void TextboxP21_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
