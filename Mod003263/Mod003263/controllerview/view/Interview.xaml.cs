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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mod003263.events;

/**
 *  Author: Ryan Cowell
 *  Date: 28/11/2016
 *  Contains: Interview
 */
namespace Mod003263.controllerview.view
{
    /// <summary>
    /// Interaction logic for Interview.xaml
    /// </summary>
    public partial class Interview : UserControl, BackEvent.BackListener 
    {
        public Interview()
        {
            InitializeComponent();

        }



        [Event]
        public void OnBack(BackEvent e) {
            throw new NotImplementedException();
        }

        private void btn_Abort_Click(object sender, RoutedEventArgs e) {
            new BackEvent().Fire();
        }
    }
}
