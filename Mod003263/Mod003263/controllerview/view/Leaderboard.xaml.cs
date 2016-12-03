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
using Mod003263.events.io;
using Mod003263.events.ui;
using Mod003263.interview;
using Mod003263.model;
using Mod003263.utils;
using Mod003263.wpf;
using Mod003263.wpf.controls;

namespace Mod003263.controllerview.view {
    /// <summary>
    /// Interaction logic for Leaderboard.xaml
    /// </summary>
    public partial class Leaderboard : UserControl, SelectApplicantEvent.SelectApplicantListener,
        HireEvent.HireListener {

        private List<Applicant> apps;
        private List<AvailablePosition> poss;
        private List<Applicant> hired;

        public Leaderboard() {
            EventBus.GetInstance().Register(this);
            InitializeComponent();
            Init();
        }

        public void Init() {
            hired = new List<Applicant>();
            FlagManager fm = FlagManager.GetInstance();
            DatabaseAccessor.GetInstance().UsingApplicantData(apps => {
                this.apps = new List<Applicant>();
                foreach (Applicant app in apps) {
                    // Has completed their interview
                    if (fm.IsFlagged(app.Flag, ApplicantFlags.COMPLETE)) {
                        // But no decision has been made
                        if (!fm.IsFlagged(app.Flag, ApplicantFlags.FINISHED))
                            this.apps.Add(app);
                    }
                }
                RebuildList();
            });
            DatabaseAccessor.GetInstance().UsingAvailablePositions(poss => {
                this.poss = poss;
                RebuildPositionsList();
            });
        }

        private void RebuildPositionsList() {
            RebuildPositionsList(this.poss);
        }

        private void RebuildPositionsList(IEnumerable<AvailablePosition> poss) {
            lst_positions.Items.Clear();
            foreach (AvailablePosition pos in poss)
                lst_positions.Items.Add(pos);
        }

        private void RebuildList() {
            RebuildList(this.apps);
        }

        private void RebuildList(IEnumerable<Applicant> apps) {
            lst_appSummary.Items.Clear();
            foreach (Applicant applicant in apps)
                lst_appSummary.Items.Add(new ApplicantRowData(applicant));
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
            lst_appSummary.UnselectAll();
        }

        private void Lst_appSummary_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            ApplicantRowData data = lst_appSummary.SelectedItem as ApplicantRowData;
            if (data == null) return;
            new SelectApplicantEvent(data.Applicant, SelectApplicantScopes.APPLICANT_RANKING).Fire();
        }

        [Event]
        public void OnSelectApplicant(SelectApplicantEvent e) {
            if (!e.Scope.Equals(SelectApplicantScopes.APPLICANT_RANKING)) return;
            app_details.PopulateDetails(e.Selected, hired.Contains(e.Selected));
            OpenApplicantDetails();
        }

        private void Txt_search_OnTextChanged(object sender, TextChangedEventArgs e) {
            string q = txt_search.Text;
            if (string.IsNullOrWhiteSpace(q)) {
                RebuildList();
                return;
            }
            RebuildList(SmartSearch.Search(q, this.apps, Applicant.GetEntities()));
        }

        private void App_details_OnHireButtonClicked(object sender, EventArgs e) {
            ApplicantDetails details = sender as ApplicantDetails;
            Applicant app = details?.Applicant;
            if (app == null) {
                WPFMessageBoxFactory.CreateAndShow("Error", "Unable to find applicant", 0);
                return;
            }
            AvailablePosition pos = poss.FirstOrDefault(p => p.Position.Equals(app.Applying_Position));
            if (pos == null) {
                WPFMessageBoxFactory.CreateAndShow("Error",
                    $"Unable to find position entry for {app.Applying_Position}", 0);
                return;
            }
            pos.Seats--;
            app.Flag = app.Flag | ApplicantFlags.HIRED;
            new SavePositionEvent(pos).Fire();
            new HireEvent(app, pos).Fire();
        }

        [Event]
        public void OnHire(HireEvent e) {
            hired.Add(e.Applicant);
            if (e.Position.Seats <= 0)
                this.poss.Remove(e.Position);
            RebuildPositionsList();
        }
    }
}
