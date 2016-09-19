using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mod003263 {
    public partial class Train : Form {
        public Train() {
            InitializeComponent();
            cmbEnginetype.Items.Add("Petrol");
            cmbEnginetype.Items.Add("Deisel");
            cmbEnginetype.Items.Add("Electric");
        }

        private void btnClose_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
