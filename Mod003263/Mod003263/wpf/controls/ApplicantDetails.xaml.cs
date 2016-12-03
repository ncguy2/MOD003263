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
using Mod003263.events;

namespace Mod003263.wpf.controls
{
    /// <summary>
    /// Interaction logic for ApplicantDetails.xaml
    /// </summary>
    public partial class ApplicantDetails : UserControl, HireEvent.HireListener, DenyEvent.DenyListener {

        public event EventHandler CallbackButtonClicked;
        public event EventHandler HireButtonClicked;
        public event EventHandler DenyButtonClicked;

        public Applicant Applicant { get; set; }

        public ApplicantDetails() {
            EventBus.GetInstance().Register(this);
            InitializeComponent();
        }

        private void btn_callback_Click(object sender, RoutedEventArgs e) {
            CallbackButtonClicked?.Invoke(this, e);
        }

        private void Btn_hire_OnClick(object sender, RoutedEventArgs e) {
            HireButtonClicked?.Invoke(this, e);
        }

        public void PopulateDetails(Applicant applicant, bool isHired = false, bool isDenied = false) {
            this.Applicant = applicant;
            if (applicant == null)
                isHired = true;
            ImageBrush brush = bdr_Picture.Background as ImageBrush;
            if (brush != null)
                brush.ImageSource = Base64Converter.GetInstance().ConvertToBitmapImage(applicant?.Picture);
            lbl_FullName.Content = applicant != null ? applicant.Full_Name : "";
            lbl_position.Content = applicant != null ? applicant.Applying_Position : "";
            btn_callback.IsEnabled = applicant != null;
            btn_hire.IsEnabled = !isHired && !isDenied;
            btn_deny.IsEnabled = !isHired && !isDenied;
        }

        private void Btn_deny_OnClick(object sender, RoutedEventArgs e) {
            DenyButtonClicked?.Invoke(this, e);
        }

        [Event]
        public void OnHire(HireEvent e) {
            if (e.Applicant != Applicant) return;
            btn_hire.IsEnabled = false;
            btn_deny.IsEnabled = false;
        }

        [Event]
        public void OnDeny(DenyEvent e) {
            if (e.Applicant != Applicant) return;
            btn_hire.IsEnabled = false;
            btn_deny.IsEnabled = false;
        }
    }
}
