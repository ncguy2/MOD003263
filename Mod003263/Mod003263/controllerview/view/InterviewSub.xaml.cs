using Mod003263.events;
using Mod003263.events.ui;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mod003263.db;
using Mod003263.utils;
using Utils.Tree.Builder;
using Utils.Tree;
using Mod003263.wpf;
using Mod003263.wpf.controls;

/**
 * Author: Ryan Cowell
 * Date: 01/12/2016
 * Contains: InterviewSub
 */

namespace Mod003263.controllerview.view
{
    /// <summary>
    /// Interaction logic for InterviewSub.xaml
    /// </summary>
    public partial class InterviewSub : UserControl, BackEvent.BackListener, SelectApplicantEvent.SelectApplicantListener {

        private Applicant selectedApplicant;
        private List<Applicant> aData;

        public InterviewSub() {
            EventBus.GetInstance().Register(this);
            InitializeComponent();
            PropertiesManager propertiesManager = PropertiesManager.GetInstance();
            try {
                aData = DatabaseAccessor.GetInstance().PullApplicantData();
                Rebuild();
            }catch (Exception e) {
                WPFMessageBoxFactory.Create("Exception", e.Message, 0).Show();
            }
        }

        public void Rebuild() {
            Rebuild(aData);
        }

        public void Rebuild(List<Applicant> applicants) {
            lst_Applicants.Items.Clear();
            foreach (Applicant applicant in applicants)
                AddApplicantRow(applicant);
        }

        private void AddApplicantRow(Applicant app) {
            lst_Applicants.Items.Add(new ApplicantRowData(app));
        }

        [Event]
        public void OnBack(BackEvent e) {
            SubMenu_Proceed_Reverse_BeginStoryboard.Storyboard.Begin();
        }

        private void lst_Applicants_OnSelectionChanged(Object sender, SelectionChangedEventArgs e) {
            Applicant a = lst_Applicants.SelectedItem as Applicant;
            new SelectApplicantEvent(a).Fire();
        }

        [Event]
        public void OnSelectApplicant(SelectApplicantEvent e) {
            if (!e.Scope.Equals("applicant.usage")) return;
            SelectApplicant(e.Selected);
        }

        private void SelectApplicant(Applicant a) {
            this.selectedApplicant = a;
            if (this.selectedApplicant == null) return;
            txt_Address.Text = a.Address;
            txt_ApplicantID.Text = a.Id + "";
            txt_ApplyingPosition.Text = a.Applying_Position;
            txt_BirthDate.Text = a.Dob + "";
            txt_EntryDate.Text = a.Doe + "";
            txt_Email.Text = a.Email;
            txt_PhoneNumber.Text = a.Phone_Number + "";
            txt_FullName.Text = a.Full_Name;
            ImageBrush brush = b_ApplicantSummary.Background as ImageBrush;
            if (brush != null)
                brush.ImageSource = Base64Converter.GetInstance().ConvertToBitmapImage(a?.Picture);
        }

        private void txt_Search_TextChanged(object sender, TextChangedEventArgs e) {
            string q = txt_Search.Text.ToLower();

            SmartSearchEntity<Applicant>[] entities = {
                new SmartSearchEntity<Applicant>("forename:", CheckFirstName),
                new SmartSearchEntity<Applicant>("surname:",  CheckLastName),
                new SmartSearchEntity<Applicant>("name:",     CheckName, true),
                new SmartSearchEntity<Applicant>("position:", CheckPosition)
            };

            List<Applicant> applicants = SmartSearch.Search(q, aData, entities);
            Rebuild(applicants);
        }

        private bool CheckPosition(string q, Applicant a) {
            string pos = a.Applying_Position;
            if(pos.Length > q.Length)
                pos = pos.Substring(0, q.Length);
            return q.Equals(pos, StringComparison.OrdinalIgnoreCase);
        }

        private bool CheckName(string q, Applicant a) {
            return CheckFirstName(q, a) || CheckLastName(q, a);
        }

        private bool CheckFirstName(string q, Applicant a) {
            string fName = a.First_Name;
            if(fName.Length > q.Length)
                fName = fName.Substring(0, q.Length);
            return q.Equals(fName, StringComparison.OrdinalIgnoreCase);
        }

        private bool CheckLastName(string q, Applicant a) {
            string lName = a.Last_Name;
            if(lName.Length > q.Length)
                lName = lName.Substring(0, q.Length);

            return q.Equals(lName, StringComparison.OrdinalIgnoreCase);
        }

    }

    internal class ApplicantRowData {
        public Border Border { get; set; }
        public ImageSource Image { get; set; }
        public String Full_Name { get; set; }
        public String Applying_Position { get; set; }

        public ApplicantRowData(ImageSource image, string fullName, string applyingPosition) {
            Border = new Border();
            Image = image;
            Full_Name = fullName;
            Applying_Position = applyingPosition;
        }

        public ApplicantRowData(string base64, string fullName, string applyingPosition)
        : this(Base64Converter.GetInstance().ConvertToBitmapImage(base64), fullName, applyingPosition) {}

        public ApplicantRowData(Applicant app)
        : this(app.Picture, app.Full_Name, app.Applying_Position) {}

    }

}
