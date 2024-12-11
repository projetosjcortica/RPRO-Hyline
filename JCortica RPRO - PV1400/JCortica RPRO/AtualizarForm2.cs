using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JCortica_RPRO
{
    public partial class AtualizarForm2 : Form
    {
        public AtualizarForm2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Atualiza Central = new Atualiza();
            Central.Tempo();
        }

        private void AtualizarCSV_Click(object sender, EventArgs e)
        {
            Atualiza Central = new Atualiza();
            Central.Update();

        }
    }
}
