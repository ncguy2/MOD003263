using Microsoft.Win32;
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
using Mod003263.utils;
using Mod003263.wpf;

/**
 * Author: Ryan Cowell, Nick Guy
 * Date: 30/11/2016
 * Contains: ApplicantEntry
 */ 
namespace Mod003263.controllerview.view {
    /// <summary>
    /// Interaction logic for ApplicantEntry.xaml
    /// </summary>
    public partial class ApplicantEntry : UserControl {

        private Applicant app;

        public ApplicantEntry() {
            InitializeComponent();
        }

        private void btn_SelectPicture_Click(object sender, RoutedEventArgs e) {
            if (app == null) return;

            OpenFileDialog ofd = new OpenFileDialog();
            bool? result = ofd.ShowDialog();
            ofd.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";

            if (result == true) txt_FileName.Text = ofd.FileName;

            try {
                if (txt_FileName.Text.Trim().Length == 0) return;
                BitmapImage src = new BitmapImage();
                src.BeginInit();
                src.UriSource = new Uri(txt_FileName.Text.Trim(), UriKind.Relative);
                src.CacheOption = BitmapCacheOption.OnLoad;
                src.EndInit();
                ((ImageBrush)img_Applicant.Background).ImageSource = src;
                app.Picture = Base64Converter.GetInstance().ConvertToBase64(src);
            } catch (Exception exc) {}
        }

        private void btn_newApp_Click(object sender, RoutedEventArgs e) {
            app = new Applicant{Id = -1};

        }

        public void PopulateFields(Applicant app) {
            SetDisabled(app == null);
            ImageBrush b = img_Applicant.Background as ImageBrush;
            if (b != null) b.ImageSource = Base64Converter.GetInstance().ConvertToBitmapImage(app?.Picture ?? Base64Converter.TEST_IMAGE);
            txt_FirstName.Text = app?.First_Name ?? "";
            txt_LastName.Text = app?.Last_Name ?? "";
            txt_Email.Text = app?.Email ?? "";
            txt_PhoneNumber.Text = app?.Phone_Number ?? "";
            txt_Address.Text = app?.First_Name ?? "";

            dat_DoB.Text = app != null ? ConversionUtils.ToDateTimeFromEpoch(app.Dob).ToLongDateString() : "";
            txt_FirstName.Text = app?.First_Name ?? "";
        }

        public void SetDisabled(bool disabled) {
            // Don't ask, I don't know either
            bool enabled = !disabled;
            btn_SelectPicture.IsEnabled = enabled;
            txt_FirstName.IsEnabled = enabled;
            txt_LastName.IsEnabled = enabled;
            txt_Email.IsEnabled = enabled;
            sel_ApplyingPosition.IsEnabled = enabled;
            txt_PhoneNumber.IsEnabled = enabled;
            txt_Address.IsEnabled = enabled;
            dat_DoB.IsEnabled = enabled;
            btn_Save.IsEnabled = enabled;
        }

    }
}
