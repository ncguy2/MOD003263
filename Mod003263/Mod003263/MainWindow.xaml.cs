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
using Mod003263.db;
using Mod003263.wpf.controls;

namespace Mod003263
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow() {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e) {
            test.AddApplicant(RandomApplicant());
        }

        private Applicant RandomApplicant() {
            Applicant app = new Applicant {
                Picture = RandomString(-1, true),
                First_Name = RandomString(12),
                Last_Name = RandomString(7),
                Applying_Position = RandomString(15)
            };
            return app;
        }

        private String RandomString(int maxLength, bool keepBase64 = false) {
            Guid g = Guid.NewGuid();
            string GuidString = Convert.ToBase64String(g.ToByteArray());
            if (keepBase64) return GuidString;
            GuidString = GuidString.Replace("=", "");
            GuidString = GuidString.Replace("+", "");
            if (maxLength > 0)
                GuidString = GuidString.Substring(0, maxLength);
            return GuidString;
        }

        private void levenshteinThreshold_SpinnerValueChanged(object sender, EventArgs e) {
            test.SetLevenshteinThreshold(levenshteinThreshold.GetNumericValue());
        }
    }
}
