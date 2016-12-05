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
using System.Windows.Shapes;

/**
 * Author: Nick Guy
 * Date: 28/11/2016
 * Contains: WPFMessageBoxForm
 */
namespace Mod003263.wpf {
    /// <summary>
    /// Interaction logic for WPFMessageBoxForm.xaml
    /// </summary>
    public partial class WPFMessageBoxForm : Window {

        public const int LEFT = 1;
        public const int MID = 2;
        public const int RIGHT = 4;

        private Action<WPFMessageBoxForm> onLeftBtnClick;
        private Action<WPFMessageBoxForm> onMidBtnClick;
        private Action<WPFMessageBoxForm> onRightBtnClick;

        public WPFMessageBoxForm() {
            InitializeComponent();
        }

        public WPFMessageBoxForm SetHeader(String text) {
            winRoot.Title = text;
            return this;
        }

        public WPFMessageBoxForm SetContent(String text) {
            txtContent.Text = text;
            return this;
        }

        public WPFMessageBoxForm SetBtnMask(int mask) {
            SetVisibility((mask & LEFT)  != 0, btnLeft);
            SetVisibility((mask & MID)   != 0, btnMid);
            SetVisibility((mask & RIGHT) != 0, btnRight);
            return this;
        }

        public void SetLeftBtnText(string text) {
            btnLeft.Content = text;
        }
        public void SetMidBtnText(string text) {
            btnMid.Content = text;
        }
        public void SetRightBtnText(string text) {
            btnRight.Content = text;
        }

        public void SetBtnActions(Action<WPFMessageBoxForm> left, Action<WPFMessageBoxForm> mid,
            Action<WPFMessageBoxForm> right) {
            SetOnLeftBtnClick(left);
            SetOnMidBtnClick(mid);
            SetOnRightBtnClick(right);
        }

        public void SetOnLeftBtnClick(Action<WPFMessageBoxForm> action) {
            onLeftBtnClick = action;
        }
        public void SetOnMidBtnClick(Action<WPFMessageBoxForm> action) {
            onMidBtnClick = action;
        }
        public void SetOnRightBtnClick(Action<WPFMessageBoxForm> action) {
            onRightBtnClick = action;
        }

        private void SetVisibility(bool cond, UIElement target) {
            target.Visibility = cond ? Visibility.Visible : Visibility.Hidden;
        }

        private void btnLeft_Click(object sender, RoutedEventArgs e) {
            onLeftBtnClick?.Invoke(this);
        }

        private void btnMid_Click(object sender, RoutedEventArgs e) {
            onMidBtnClick?.Invoke(this);
        }

        private void btnRight_Click(object sender, RoutedEventArgs e) {
            onRightBtnClick?.Invoke(this);
        }
    }
}
