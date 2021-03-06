﻿using Mod003263.interview;
using System;
using System.Collections.Generic;
using System.IO;
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

/**
 * Author: Nick Guy
 * Date: 28/11/2016
 * Contains: AnswerRow
 */
namespace Mod003263.wpf.controls
{
    /// <summary>
    /// Interaction logic for AnswerRow.xaml
    /// </summary>
    public partial class AnswerRow : ListViewItem {

        private Answer answer;

        public AnswerRow() : this(null) {}

        public AnswerRow(Answer answer) {
            InitializeComponent();
            this.answer = answer;
        }

        private Action<AnswerRow> ButtonClicked;

        public Answer GetAnswer() {
            return this.answer;
        }

        private void answerText_TextChanged(object sender, TextChangedEventArgs e) {
            this.answer.SetText(answerText.Text);
        }

        private void answerWeight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            this.answer.SetWeight((int)answerWeight.Value);
            answerPercent.Content = answerWeight.Value + "%";
        }

        public void SetButtonClicked(Action<AnswerRow> buttonClicked) {
            this.ButtonClicked = buttonClicked;
        }

        private void button_Click(object sender, RoutedEventArgs e) {
            ButtonClicked?.Invoke(this);
        }
    }
}
