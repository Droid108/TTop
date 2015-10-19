using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TTWinAPp
{
    public partial class frmTwitterManager : Form
    {
        public frmTwitterManager()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 frmTM = new Form1();
            frmTM.ShowDialog();
            this.Close();
        }
    }
}
