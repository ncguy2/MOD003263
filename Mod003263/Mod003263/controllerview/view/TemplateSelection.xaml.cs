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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

/**
 *  Author: Ryan Cowell
 *  Date: 26/10/2016
 *  Contains: TemplateSelection
 */

namespace Mod003263.controllerview.view
{
    /// <summary>
    /// Interaction logic for TemplateSelection.xaml
    /// </summary>
    public partial class TemplateSelection : UserControl, BackEvent.BackListener
    
    {
        public TemplateSelection()
        {
            EventBus.GetInstance().Register(this);
            InitializeComponent();
        }

        [Event]
        public void OnBack(BackEvent e) {
            Interview_Reverse_BeginStoryboard.Storyboard.Begin();
        }
    }
}
