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
using Utils.Tree.Builder;
using Utils.Tree;
using Mod003263.wpf;

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


        public InterviewSub() {
            EventBus.GetInstance().Register(this);
            InitializeComponent();
            PropertiesManager propertiesManager = PropertiesManager.GetInstance();
            try {
                List<Applicant> aData = DatabaseAccessor.GetInstance().PullApplicantData();
                aData.ForEach(a => lst_Applicants.Items.Add(a));
            }catch (Exception e) {
                WPFMessageBoxFactory.Create("Exception", e.Message, 0).Show();
            }
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

        private void SelectApplicant(Applicant a)
        {
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


    }
}
