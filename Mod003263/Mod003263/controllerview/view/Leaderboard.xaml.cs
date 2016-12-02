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
using Mod003263.events;
using Mod003263.events.ui;
using Mod003263.wpf;
using Mod003263.wpf.controls;

namespace Mod003263.controllerview.view {
    /// <summary>
    /// Interaction logic for Leaderboard.xaml
    /// </summary>
    public partial class Leaderboard : UserControl, SelectApplicantEvent.SelectApplicantListener {

        public Leaderboard() {
            EventBus.GetInstance().Register(this);
            InitializeComponent();
        }

        public void Init() {
            lst_appSummary.Items.Clear();
            List<Applicant> apps = DatabaseAccessor.GetInstance().PullApplicantData();
            foreach (Applicant applicant in apps) {
                ApplicantRowData row = new ApplicantRowData(applicant);
                lst_appSummary.Items.Add(row);
            }
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

        private void Lst_appSummary_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            ApplicantRowData data = lst_appSummary.SelectedItem as ApplicantRowData;
            if (data == null) return;
            new SelectApplicantEvent(data.Applicant, SelectApplicantScopes.APPLICANT_RANKING).Fire();
        }

        [Event]
        public void OnSelectApplicant(SelectApplicantEvent e) {
            if (!e.Scope.Equals(SelectApplicantScopes.APPLICANT_RANKING)) return;
            app_details.PopulateDetails(e.Selected);
            OpenApplicantDetails();
        }
    }
}
