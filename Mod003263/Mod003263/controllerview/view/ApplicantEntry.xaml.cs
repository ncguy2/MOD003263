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

/**
 * Author: Ryan Cowell
 * Date: 30/11/2016
 * Contains: ApplicantEntry
 */ 
namespace Mod003263.controllerview.view
{
    /// <summary>
    /// Interaction logic for ApplicantEntry.xaml
    /// </summary>
    public partial class ApplicantEntry : UserControl
    {
        public ApplicantEntry()
        {
            InitializeComponent();
        }

        private void btn_Browse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            Nullable<bool> result = ofd.ShowDialog();
            ofd.Filter = "Image Files(*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp";

            if (result == true)
            {
                txt_FileName.Text = ofd.FileName;
            }
        }

        private void btn_Upload_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                if (txt_FileName.Text.Trim().Length != 0)
                {
                    BitmapImage src = new BitmapImage();
                    src.BeginInit();
                    src.UriSource = new Uri(txt_FileName.Text.Trim(), UriKind.Relative);
                    src.CacheOption = BitmapCacheOption.OnLoad;
                    src.EndInit();
                    ((ImageBrush)img_Applicant.Background).ImageSource = src;


                }
            }
            catch (Exception exc)
            {
            }
        }
    }
}
