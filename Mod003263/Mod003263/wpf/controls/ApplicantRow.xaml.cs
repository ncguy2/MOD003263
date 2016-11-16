using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mod003263.wpf.controls
{
    /// <summary>
    /// Interaction logic for ApplicantRow.xaml
    /// </summary>
    public partial class ApplicantRow : UserControl {
        public ApplicantRow() {
            InitializeComponent();
        }

        private void image_Initialized(object sender, EventArgs e) {
            Base64Converter base64Converter = new Base64Converter();
            image.Source = base64Converter.Convert(Base64Converter.TEST_IMAGE);
        }
    }
}
