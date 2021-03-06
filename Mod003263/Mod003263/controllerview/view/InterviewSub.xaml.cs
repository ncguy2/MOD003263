﻿using Mod003263.events;
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
using Mod003263.interview;
using Mod003263.model;
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
    public partial class InterviewSub : UserControl, BackEvent.BackListener, SelectApplicantEvent.SelectApplicantListener,
        IInitializable, SelectTemplateEvent.SelectTemplateListener, InterviewToTemplateEvent.InterviewToTemplateListener,
        InterviewToApplicantEvent.InterviewToApplicantListener {

        private Applicant selectedApplicant;
        private List<Applicant> aData;

        private InterviewFoundation selectedTemplate;

        public InterviewSub() {
            EventBus.GetInstance().Register(this);
            InitializeComponent();
        }

        public void OnInitialization() {
            try {
                DatabaseAccessor.GetInstance().UsingApplicantData(list => {
                    aData = list;
                    Rebuild();
                });
                new SelectApplicantEvent(SelectApplicantScopes.APPLICANT_USAGE).Fire();
            }catch (Exception e) {
                WPFMessageBoxFactory.Create("Exception", e.Message, 0).Show();
            }
        }

        public void Rebuild() {
            Rebuild(aData);
        }

        public void Rebuild(List<Applicant> applicants) {
            try {
                lst_Applicants.Items.Clear();
                foreach (Applicant applicant in applicants) {
                    if (FlagManager.GetInstance().IsFlagged(applicant.Flag, ApplicantFlags.COMPLETE)) continue;
                    if (selectedTemplate.Position.Equals(applicant.Applying_Position))
                        AddApplicantRow(applicant);
                }
            }catch (Exception e) {
                WPFMessageBoxFactory.CreateErrorAndShow(e);
            }
        }

        private void AddApplicantRow(Applicant app) {
            lst_Applicants.Items.Add(new ApplicantRowData(app));
        }

        [Event]
        public void OnBack(BackEvent e) {
            SubMenu_Proceed_Reverse_BeginStoryboard.Storyboard.Begin();
        }

        private void lst_Applicants_OnSelectionChanged(Object sender, SelectionChangedEventArgs e) {
            ApplicantRowData row = lst_Applicants.SelectedItem as ApplicantRowData;
            if (row == null) return;
            Applicant a = row.Applicant;
            new SelectApplicantEvent(a, SelectApplicantScopes.APPLICANT_USAGE).Fire();
        }

        [Event]
        public void OnSelectApplicant(SelectApplicantEvent e) {
            if (!e.Scope.Equals(SelectApplicantScopes.APPLICANT_USAGE)) return;
            SelectApplicant(e.Selected);
        }

        private void SelectApplicant(Applicant a) {
            this.selectedApplicant = a;
            txt_Address.Text = a?.Address+"";
            txt_ApplicantID.Text = a?.Id + "";
            txt_ApplyingPosition.Text = a?.Applying_Position+"";
            txt_BirthDate.Text = a?.Dob + "";
            txt_EntryDate.Text = a?.Doe + "";
            txt_Email.Text = a?.Email+"";
            txt_PhoneNumber.Text = a?.Phone_Number + "";
            txt_FullName.Text = a?.Full_Name+"";
            ImageBrush brush = b_ApplicantSummary.Background as ImageBrush;
            if (brush == null)
                b_ApplicantSummary.Background = brush = new ImageBrush();
            brush.ImageSource = Base64Converter.GetInstance().ConvertToBitmapImage(a?.Picture);
        }

        private void txt_Search_TextChanged(object sender, TextChangedEventArgs e) {
            string q = txt_Search.Text.ToLower();
            if (q.Length <= 0) {
                Rebuild();
                return;
            }
            List<Applicant> applicants = SmartSearch.Search(q, aData, Applicant.GetEntities());
            Rebuild(applicants);
        }

        [Event]
        public void OnSelectTemplate(SelectTemplateEvent e) {
            if (!e.Scope.Equals(SelectTemplateScopes.TEMPLATE_USAGE)) return;
            selectedTemplate = e.Template;
        }

        private void Btn_InterviewProceed_OnClick(object sender, RoutedEventArgs e) {
            interview.OnInitialization();
        }

        [Event]
        public void OnInterviewToTemplate(InterviewToTemplateEvent e) {
            SubMenu_Proceed_Reverse_BeginStoryboard.Storyboard.Begin();
            OnInitialization();
        }

        [Event]
        public void OnInterviewToApplicant(InterviewToApplicantEvent e) {
            SubMenu_Proceed_Reverse_BeginStoryboard.Storyboard.Begin();
            OnInitialization();
        }
    }

}
