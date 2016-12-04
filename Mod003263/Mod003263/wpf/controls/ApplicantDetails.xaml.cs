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
using Mod003263.interview;
using Mod003263.interview.metric;
using Mod003263.model;

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
            lbl_FullName.Content = applicant?.Full_Name + "";
            lbl_position.Content = applicant?.Applying_Position + "";
            btn_callback.IsEnabled = applicant != null;
            btn_hire.IsEnabled = !isHired && !isDenied;
            btn_deny.IsEnabled = !isHired && !isDenied;

            int f = Applicant?.Flag ?? 0;
            if(FlagManager.GetInstance().IsFlagged(f, ApplicantFlags.HIRED))
                lbl_decision.Content = $"{applicant?.Full_Name} has been hired";
            else if (FlagManager.GetInstance().IsFlagged(f, ApplicantFlags.FINISHED))
                lbl_decision.Content = $"{applicant?.Full_Name} has not been hired";
            else lbl_decision.Content = $"No decision has been made for {applicant?.Full_Name}";

            Interview i = applicant?.GetInterview();
            lbl_templateName.Content = i?.GetFoundationInstance().GetFoundation().Name();
            lst_Answers.Items.Clear();
            if (i == null) return;
            MetricCalculator calculator = new MetricCalculator();
            foreach (KeyValuePair<Question, Answer> pair in i.GetFoundationInstance().GetAnswerMap()) {
                try {
                    AnswerRowData d = new AnswerRowData {
                        Question = pair.Key.Text(),
                        Grade = pair.Value.Text,
                        Score = calculator.CalculateLocalMetric(i, pair.Key, pair.Value)
                    };
                    lst_Answers.Items.Add(d);
                }catch (Exception e) {
                    WPFMessageBoxFactory.CreateErrorAndShow(e);
                }
            }
        }

        private void Btn_deny_OnClick(object sender, RoutedEventArgs e) {
            DenyButtonClicked?.Invoke(this, e);
        }

        [Event]
        public void OnHire(HireEvent e) {
            if (e.Applicant != Applicant) return;
            btn_hire.IsEnabled = false;
            btn_deny.IsEnabled = false;
            lbl_decision.Content = $"{e.Applicant.Full_Name} has been hired";
        }

        [Event]
        public void OnDeny(DenyEvent e) {
            if (e.Applicant != Applicant) return;
            btn_hire.IsEnabled = false;
            btn_deny.IsEnabled = false;
            lbl_decision.Content = $"{e.Applicant.Full_Name} has not been hired";
        }
    }

    internal class AnswerRowData {
        public string Question { get; set; }
        public string Grade { get; set; }
        public float Score { get; set; }
    }

}
