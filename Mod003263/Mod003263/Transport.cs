using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mod003263
{
    public partial class Transport : Form
    {
        public Transport()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            new Car().Show();
        }

        private void button2_Click(object sender, EventArgs e) {
            new Plane().Show();
        }

        private void button3_Click(object sender, EventArgs e) {
            new Train().Show();
        }

        private void button4_Click(object sender, EventArgs e) {
            new Ship().Show();
        }
    }
}
