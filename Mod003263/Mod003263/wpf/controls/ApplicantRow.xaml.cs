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
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Mod003263.db;

/**
 * Author: Nick Guy
 * Date: 28/11/2016
 * Contains: ApplicantRow
 */
namespace Mod003263.wpf.controls
{
    /// <summary>
    /// Interaction logic for ApplicantRow.xaml
    /// </summary>
    public partial class ApplicantRow : ListViewItem {
        public ApplicantRow() : this(null) {}

        public ApplicantRow(Applicant applicant) {
            this.Applicant = applicant;
            InitializeComponent();
            Base64Converter base64Converter = new Base64Converter();
            ((ImageBrush)userImgFrame.Background).ImageSource = base64Converter.ConvertToBitmapImage(Base64Converter.TEST_IMAGE);
            Populate(applicant);
        }

        public Applicant Applicant { get; private set; }

        public ApplicantRow SetWidth(double width) {
            Width = width;
            root.Width = width;
            rootGrid.Width = width;
            return this;
        }

        public void Populate(Applicant applicant) {
            // Image
            Base64Converter base64Converter = new Base64Converter();
            ((ImageBrush)this.userImgFrame.Background).ImageSource = base64Converter.ConvertToBitmapImage(applicant?.Picture);
            // Name
            this.usrName.Content = applicant?.Full_Name;
            // Position
            this.usrPosition.Content = applicant?.Applying_Position;
            // Flags
            // TODO discern icons for each flag
        }

        private void root_MouseEnter(object sender, MouseEventArgs e) {
            rootDropShadow.Opacity = 0;
            imgDropShadow.ShadowDepth = 5;
        }

        private void root_MouseLeave(object sender, MouseEventArgs e) {
            rootDropShadow.Opacity = 1;
            imgDropShadow.ShadowDepth = 2;
        }
    }
}
