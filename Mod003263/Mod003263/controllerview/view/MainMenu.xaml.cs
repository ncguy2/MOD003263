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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mod003263.events.ui;
using System.Windows.Media.Animation;
using Mod003263.events;

namespace Mod003263.controllerview.view
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : UserControl, BackEvent.BackListener
    {
        public MainMenu()
        {
            EventBus.GetInstance().Register(this);
            InitializeComponent();
        }

        [Event]
        public void OnBack(BackEvent e)
        {
            interview_Press_BeginStoryboard.Storyboard.Seek(TimeSpan.Zero, TimeSeekOrigin.BeginTime);
            interview_Press_BeginStoryboard.Storyboard.Stop();

        }

        private void interview_Back_Click(object sender, RoutedEventArgs e)
        {
            new BackEvent().Fire();
        }
    }
}
