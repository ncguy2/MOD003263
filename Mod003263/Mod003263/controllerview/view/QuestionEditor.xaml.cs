﻿using System;
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
using Mod003263.events.test;
using Mod003263.interview;
using Mod003263.wpf.controls;

/**
 * Author: Nick Guy
 * Date: 28/11/2016
 * Contains: QuestionEditor
 */
namespace Mod003263.controllerview.view {
    /// <summary>
    /// Interaction logic for QuestionEditor.xaml
    /// </summary>
    public partial class QuestionEditor : UserControl {

        private Question selectedQuestion;

        public QuestionEditor() {
            InitializeComponent();
        }

        private void addAnsBtn_Click(object sender, RoutedEventArgs e) {
            new StringPayloadEvent("Test payload").Fire();
            if (selectedQuestion == null) return;
            AnswerRow row = new AnswerRow();
            row.SetButtonClicked(OnRowBtnClick);
            AddAnswerRow(row);
        }

        /// <summary>
        /// Add answer row to the answer table
        /// </summary>
        /// <param name="row"></param>
        private void AddAnswerRow(AnswerRow row) {
            if (selectedQuestion == null) return;
            selectedQuestion.GetAnswers().Add(row.GetAnswer());
            tbl_AnswerTable.Items.Add(row);
        }

        private void OnRowBtnClick(AnswerRow ansRow) {
            selectedQuestion?.GetAnswers().Remove(ansRow.GetAnswer());
            tbl_AnswerTable.Items.Remove(ansRow);
        }

    }
}
