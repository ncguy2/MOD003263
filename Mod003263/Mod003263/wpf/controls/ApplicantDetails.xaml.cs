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
using Mod003263.db;

namespace Mod003263.wpf.controls
{
    /// <summary>
    /// Interaction logic for ApplicantDetails.xaml
    /// </summary>
    public partial class ApplicantDetails : UserControl {

        public event EventHandler CallbackButtonClicked;
        private Base64Converter converter;


        public ApplicantDetails() {
            converter = new Base64Converter();
            InitializeComponent();
        }

        private void btn_callback_Click(object sender, RoutedEventArgs e) {
            CallbackButtonClicked?.Invoke(this, e);
        }

        public void PopulateDetails(Applicant applicant) {
            ImageBrush brush = bdr_Picture.Background as ImageBrush;
            if (brush != null)
                brush.ImageSource = converter.ConvertToBitmapImage(applicant?.Picture);
            lbl_FullName.Content = applicant != null ? applicant.Full_Name : "";
            lbl_position.Content = applicant != null ? applicant.Applying_Position : "";
        }

    }
}
