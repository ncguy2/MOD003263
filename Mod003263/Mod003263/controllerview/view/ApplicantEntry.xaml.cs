using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using Mod003263.events.io;
using Mod003263.events.ui;
using Mod003263.interview;
using Mod003263.utils;
using Mod003263.wpf;
using Mod003263.wpf.controls;
using Utils.Difference;

/**
 * Author: Ryan Cowell, Nick Guy
 * Date: 30/11/2016
 * Contains: ApplicantEntry
 */
namespace Mod003263.controllerview.view {
    /// <summary>
    /// Interaction logic for ApplicantEntry.xaml
    /// </summary>
    public partial class ApplicantEntry : UserControl, SelectApplicantEvent.SelectApplicantListener {

        private Applicant app;
        private List<Applicant> applicants;

        public ApplicantEntry() {
            EventBus.GetInstance().Register(this);
            InitializeComponent();
            Init();
        }

        private void Init() {
            try {
                List<AvailablePosition> positions = DatabaseAccessor.GetInstance().GetAllPositions();
                sel_ApplyingPosition.Items.Clear();
                foreach (AvailablePosition pos in positions)
                    sel_ApplyingPosition.Items.Add(pos);

                applicants = DatabaseAccessor.GetInstance().PullApplicantData();
                RebuildListView();
            } catch (Exception e) {
                WPFMessageBoxFactory.Create("Error", e.Message, 0).Show();
            }
            PopulateFields(null);
        }

        private void btn_SelectPicture_Click(object sender, RoutedEventArgs e) {
            if (app == null) return;

            OpenFileDialog ofd = new OpenFileDialog();
            bool? result = ofd.ShowDialog();
            ofd.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";

            if (result == true) txt_FileName.Text = ofd.FileName;

            try {
                if (txt_FileName.Text.Trim().Length == 0) return;
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri(txt_FileName.Text.Trim(), UriKind.Relative);
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.EndInit();
                ((ImageBrush)img_Applicant.Background).ImageSource = src;
                app.Picture = Base64Converter.GetInstance().ConvertToBase64(src);
            } catch (Exception exc) {}
        }

        private void btn_newApp_Click(object sender, RoutedEventArgs e) {
            lv_Applicant.UnselectAll();
            app = new Applicant{Id = -1};
            PopulateFields(app);
        }

        public void PopulateFields(Applicant app) {
            SetDisabled(app == null);
            ImageBrush b = img_Applicant.Background as ImageBrush;
            if (b != null) b.ImageSource = Base64Converter.GetInstance().ConvertToBitmapImage(app?.Picture ?? Base64Converter.TEST_IMAGE);
            txt_FirstName.Text = app?.First_Name ?? "";
            txt_LastName.Text = app?.Last_Name ?? "";
            txt_Email.Text = app?.Email ?? "";
            txt_PhoneNumber.Text = app?.Phone_Number ?? "";
            txt_Address.Text = app?.Address ?? "";

            int targetIndex = -1;
            for (int i = 0; i < sel_ApplyingPosition.Items.Count; i++) {
                AvailablePosition pos = sel_ApplyingPosition.Items[i] as AvailablePosition;
                if(pos == null) continue;
                if (!pos.Position.Equals(app?.Applying_Position, StringComparison.OrdinalIgnoreCase)) continue;
                targetIndex = i;
                i += sel_ApplyingPosition.Items.Count;
            }
            if (targetIndex >= 0)
                sel_ApplyingPosition.SelectedIndex = targetIndex;

            if (app != null) {
                DateTime t = ConversionUtils.ToDateTimeFromEpoch(app.Dob);
                String s = t.ToLongDateString();
                dat_DoB.Text = s;
            }
//            dat_DoB.Text = app != null ? ConversionUtils.ToDateTimeFromEpoch(app.Dob).ToLongDateString() : "";
            txt_FirstName.Text = app?.First_Name ?? "";
        }

        public void SetDisabled(bool disabled) {
            // Don't ask, I don't know either
            bool enabled = !disabled;
            btn_SelectPicture.IsEnabled = enabled;
            txt_FirstName.IsEnabled = enabled;
            txt_LastName.IsEnabled = enabled;
            txt_Email.IsEnabled = enabled;
            sel_ApplyingPosition.IsEnabled = enabled;
            txt_PhoneNumber.IsEnabled = enabled;
            txt_Address.IsEnabled = enabled;
            dat_DoB.IsEnabled = enabled;
            btn_Save.IsEnabled = enabled;
        }

        public void CascadeData() {
            if (this.app == null) return;
            this.app.First_Name = txt_FirstName.Text;
            this.app.Last_Name = txt_LastName.Text;
            this.app.Email = txt_Email.Text;
            AvailablePosition pos = sel_ApplyingPosition.SelectedItem as AvailablePosition;
            this.app.Applying_Position = pos?.Position;
            this.app.Phone_Number = txt_PhoneNumber.Text;
            this.app.Address = txt_Address.Text;
            this.app.Dob = ConversionUtils.ToEpochTime(dat_DoB.DisplayDate);
        }

        private void lv_Applicant_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            Applicant applicant = lv_Applicant.SelectedItem as Applicant;
            new SelectApplicantEvent(applicant, SelectApplicantScopes.APPLICANT_EDITOR).Fire();
        }

        private void Btn_Save_OnClick(object sender, RoutedEventArgs e) {
            if (this.app == null) return;
            CascadeData();
            lv_Applicant.Items.Add(this.app);
            new SaveApplicantEvent(this.app).Fire();
        }

        private void btn_refresh_Click(object sender, RoutedEventArgs e) {
            Init();
        }

        private void RebuildListView() {
            RebuildListView(this.applicants);
        }

        private void RebuildListView(List<Applicant> apps) {
            lv_Applicant.Items.Clear();
            foreach (Applicant app in apps)
                lv_Applicant.Items.Add(app);
        }

        private void txt_Search_TextChanged(object sender, TextChangedEventArgs e) {
            string q = txt_Search.Text.ToLower();
            if (q.Length <= 0) {
                RebuildListView();
                return;
            }


            List<Applicant> valids = SmartSearch.Search(q, applicants, Applicant.GetEntities());
            valids.Sort((a1, a2) => String.Compare(a1.Full_Name, a2.Full_Name, StringComparison.Ordinal));
            RebuildListView(valids);
        }

        [Event]
        public void OnSelectApplicant(SelectApplicantEvent e) {
            if (!e.Scope.Equals(SelectApplicantScopes.APPLICANT_EDITOR)) return;
            this.app = e.Selected;
            if(this.app != null)
                PopulateFields(this.app);
        }
    }
}
