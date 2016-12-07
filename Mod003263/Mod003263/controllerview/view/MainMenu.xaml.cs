using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mod003263.events.ui;
using System.Windows.Media.Animation;
using Mod003263.events;
using Mod003263.events.email;
using Mod003263.wpf;

/**
 *  Author: Ryan Cowell
 *  Date: 26/10/2016
 *  Contains: MainMenu
 */



namespace Mod003263.controllerview.view{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl, BackEvent.BackListener, EmailSuccessEvent.EmailSuccessEventListener,
        OpenLeaderboardEvent.OpenLeaderboardListener {

        public MainMenu() {
            EventBus.GetInstance().Register(this);
            InitializeComponent();
        }

        [Event]
        public void OnBack(BackEvent e) {
//            interview_Press_BeginStoryboard.Storyboard.Seek(TimeSpan.Zero, TimeSeekOrigin.BeginTime);
//            interview_Press_BeginStoryboard.Storyboard.Stop();
//            question_Close_BeginStoryboard.Storyboard.Begin();
            ApplicantEntry_Close_BeginStoryboard.Storyboard.Begin();
            Options_Press_Reverse_BeginStoryboard.Storyboard.Begin();
            template_Close_BeginStoryboard.Storyboard.Begin();
            question_Close_BeginStoryboard.Storyboard.Begin();
            interview_Press_Reverse_BeginStoryboard.Storyboard.Begin();
            tmp_canvasOverlay_Reverse_BeginStoryboard.Storyboard.Begin();
        }

        private void interview_Back_Click(object sender, RoutedEventArgs e) {
            new BackEvent().Fire();
        }

        private void button_Click(object sender, RoutedEventArgs e) {
            
            InitializeObject(positions);
        }

        [Event]
        public void OnEmailSuccess(EmailSuccessEvent e) {
        }

        private void Btn_Interview_OnClick(object sender, RoutedEventArgs e) {
            InitializeObject(templateSelection);
        }

        private void btn_Template_Click(object sender, RoutedEventArgs e) {
            InitializeObject(templateEditor);
        }

        private void btn_Questions_Click(object sender, RoutedEventArgs e) {
            InitializeObject(questionEditor);
        }

        private void btn_Options_Click(object sender, RoutedEventArgs e) {
//            InitializeObject(options);
        }

        private void btn_ApplicantEntry_Click(object sender, RoutedEventArgs e) {
            InitializeObject(applicantEntry);
        }

        private void btn_leaderboard_Click(object sender, RoutedEventArgs e) {
            InitializeObject(leaderboard);
        }

        private void InitializeObject(object o) {
            InitializeObject(o as IInitializable);
        }
        private void InitializeObject(IInitializable i) {
            i?.OnInitialization();
        }

        [Event]
        public void OnOpenLeaderboard(OpenLeaderboardEvent e) {
            btn_leaderboard.RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }
    }
}
