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
using System.Windows.Shapes;
using Mod003263.db;
using Mod003263.wpf;
using Mod003263.wpf.controls;

namespace Mod003263.controllerview.view {
    /// <summary>
    /// Interaction logic for Leaderboard.xaml
    /// </summary>
    public partial class Leaderboard : UserControl{

        public Leaderboard() {
            InitializeComponent();
        }

        public void Init() {
            lst_appSummary.Items.Clear();
            List<Applicant> apps = DatabaseAccessor.GetInstance().PullApplicantData();
            foreach (Applicant applicant in apps) {
                ApplicantRow applicantRow = new ApplicantRow(applicant);
                applicantRow.MouseDoubleClick += applicantRow_DblClick;
                lst_appSummary.Items.Add(applicantRow);
            }
        }

        private void applicantRow_DblClick(object sender, RoutedEventArgs e) {
            ApplicantRow row = sender as ApplicantRow;
            if (row == null) return;
            app_details.PopulateDetails(row.Applicant);
            OpenApplicantDetails();
        }

        private void OpenApplicantDetails() {
            app_details_show_BeginStoryboard.Storyboard.Begin();
        }

        private void CloseApplicantDetails() {
            app_details_hide_BeginStoryboard.Storyboard.Begin();
        }

        private void btn_init_Click(object sender, RoutedEventArgs e) {
            Init();
        }

        private void app_details_CallbackButtonClicked(object sender, EventArgs e) {
            app_details.PopulateDetails(null);
            CloseApplicantDetails();
        }
    }
}
