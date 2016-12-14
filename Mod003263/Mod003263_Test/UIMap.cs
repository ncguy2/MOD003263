namespace Mod003263_Test
{
    using System;
    using System.CodeDom.Compiler;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using System.Windows.Input;
    using Microsoft.VisualStudio.TestTools.UITest.Extension;
    using Microsoft.VisualStudio.TestTools.UITesting;
    using Microsoft.VisualStudio.TestTools.UITesting.WinControls;
    using Microsoft.VisualStudio.TestTools.UITesting.WpfControls;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Keyboard = Microsoft.VisualStudio.TestTools.UITesting.Keyboard;
    using Mouse = Microsoft.VisualStudio.TestTools.UITesting.Mouse;
    using MouseButtons = System.Windows.Forms.MouseButtons;
    
    
    public partial class UIMap
    {
        
        #region Fields
        private FullSoftwareDemoParams mFullSoftwareDemoParams;
        
        #region Properties
        public virtual FullSoftwareDemoParams FullSoftwareDemoParams
        {
            get
            {
                if ((this.mFullSoftwareDemoParams == null))
                {
                    this.mFullSoftwareDemoParams = new FullSoftwareDemoParams();
                }
                return this.mFullSoftwareDemoParams;
            }
        }
        
        /// <summary>
        /// This is a test of a full run through of the program
        /// </summary>
        public void FullSoftwareDemo()
        {
            #region Variable Declarations
            WpfWindow uIWpfWindow = this.UIWpfWindow;
            WpfEdit uITxt_FirstNameEdit = this.UIMainWindowWindow.UIItemCustom.UIApplicantEntryCustom.UITxt_FirstNameEdit;
            WpfEdit uITxt_LastNameEdit = this.UIMainWindowWindow.UIItemCustom.UIApplicantEntryCustom.UITxt_LastNameEdit;
            WpfEdit uITxt_EmailEdit = this.UIMainWindowWindow.UIItemCustom.UIApplicantEntryCustom.UITxt_EmailEdit;
            WpfComboBox uISel_ApplyingPositionComboBox = this.UIMainWindowWindow.UIItemCustom.UIApplicantEntryCustom.UISel_ApplyingPositionComboBox;
            WpfEdit uITxt_PhoneNumberEdit = this.UIMainWindowWindow.UIItemCustom.UIApplicantEntryCustom.UITxt_PhoneNumberEdit;
            WpfEdit uITxt_AddressEdit = this.UIMainWindowWindow.UIItemCustom.UIApplicantEntryCustom.UITxt_AddressEdit;
            WpfDatePicker uIDat_DoBDatePicker = this.UIMainWindowWindow.UIItemCustom.UIApplicantEntryCustom.UIDat_DoBDatePicker;
            WpfEdit uITxt_CategoryEdit = this.UIMainWindowWindow.UIItemCustom.UIQuestionEditorCustom.UITxt_CategoryEdit;
            WpfEdit uITxt_QuestionEdit = this.UIMainWindowWindow.UIItemCustom.UIQuestionEditorCustom.UITxt_QuestionEdit;
            WpfCell uIItemCell = this.UIMainWindowWindow.UIItemCustom.UIQuestionEditorCustom.UITbl_AnswerTableTable.UIItemDataItem.UIItemCell;
            WpfCell uIItemCell1 = this.UIMainWindowWindow.UIItemCustom.UIQuestionEditorCustom.UITbl_AnswerTableTable.UIItemDataItem1.UIItemCell;
            WpfEdit uITxt_FeedbackEdit = this.UIMainWindowWindow.UIInterviewSubCustom.UIInterviewCustom.UITxt_FeedbackEdit;
            WinButton uICloseButton = this.UIQueryWindow.UICloseButton;
            #endregion

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(314, 234));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(197, 325));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(688, 41));

            // Type 'Rob' in 'txt_FirstName' text box
            uITxt_FirstNameEdit.Text = this.FullSoftwareDemoParams.UITxt_FirstNameEditText;

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(705, 69));

            // Type 'Bor' in 'txt_LastName' text box
            uITxt_LastNameEdit.Text = this.FullSoftwareDemoParams.UITxt_LastNameEditText;

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(666, 114));

            // Type 'Robbor' in 'txt_Email' text box
            uITxt_EmailEdit.Text = this.FullSoftwareDemoParams.UITxt_EmailEditText;

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(676, 141));

            // Select 'Teacher [1]' in 'sel_ApplyingPosition' combo box
            uISel_ApplyingPositionComboBox.SelectedItem = this.FullSoftwareDemoParams.UISel_ApplyingPositionComboBoxSelectedItem;

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(693, 175));

            // Type '12345678905' in 'txt_PhoneNumber' text box
            uITxt_PhoneNumberEdit.Text = this.FullSoftwareDemoParams.UITxt_PhoneNumberEditText;

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(698, 224));

            // Type 'rbbbb 18' in 'txt_Address' text box
            uITxt_AddressEdit.Text = this.FullSoftwareDemoParams.UITxt_AddressEditText;

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(721, 116));

            // Type 'Robbor@me.com' in 'txt_Email' text box
            uITxt_EmailEdit.Text = this.FullSoftwareDemoParams.UITxt_EmailEditText1;

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(903, 244));

            // Select '09-May-1971' in 'dat_DoB' date picker
            uIDat_DoBDatePicker.DateAsString = this.FullSoftwareDemoParams.UIDat_DoBDatePickerDateAsString;

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(777, 324));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(72, 21));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(821, 192));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(564, 120));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(489, 34));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(232, 333));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(415, 33));

            // Type 'Experience' in 'txt_Category' text box
            uITxt_CategoryEdit.Text = this.FullSoftwareDemoParams.UITxt_CategoryEditText;

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(417, 81));

            // Type 'How much experience do you have teaching?' in 'txt_Question' text box
            uITxt_QuestionEdit.Text = this.FullSoftwareDemoParams.UITxt_QuestionEditText;

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(369, 326));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(420, 148));

            // Type '0 - 12 months' in cell
            uIItemCell.Value = this.FullSoftwareDemoParams.UIItemCellValue;

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(390, 246));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(950, 182));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(357, 324));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(425, 190));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(394, 150));

            // Type '0 - 11 months' in cell
            uIItemCell.Value = this.FullSoftwareDemoParams.UIItemCellValue1;

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(499, 190));

            // Type '1 - 3 Years' in cell
            uIItemCell1.Value = this.FullSoftwareDemoParams.UIItemCellValue2;

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(542, 234));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(1023, 322));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(63, 68));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(321, 147));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(135, 27));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(179, 41));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(391, 124));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(448, 145));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(650, 207));

            // Double-Click 'Wpf' window
            Mouse.DoubleClick(uIWpfWindow, new Point(1061, 160));

            // Double-Click 'Wpf' window
            Mouse.DoubleClick(uIWpfWindow, new Point(1061, 160));

            // Double-Click 'Wpf' window
            Mouse.DoubleClick(uIWpfWindow, new Point(1061, 160));

            // Double-Click 'Wpf' window
            Mouse.DoubleClick(uIWpfWindow, new Point(1061, 160));

            // Double-Click 'Wpf' window
            Mouse.DoubleClick(uIWpfWindow, new Point(1061, 160));

            // Double-Click 'Wpf' window
            Mouse.DoubleClick(uIWpfWindow, new Point(1061, 160));

            // Double-Click 'Wpf' window
            Mouse.DoubleClick(uIWpfWindow, new Point(1061, 160));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(1015, 327));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(48, 49));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(107, 57));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(125, 55));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(151, 65));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(184, 86));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(1005, 295));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(307, 138));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(1010, 299));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(240, 136));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(593, 259));

            // Type 'Gave a good response' in 'txt_Feedback' text box
            uITxt_FeedbackEdit.Text = this.FullSoftwareDemoParams.UITxt_FeedbackEditText;

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(726, 221));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(740, 121));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(220, 159));

            // Type 'Gave a good response & has sufficient experience' in 'txt_Feedback' text box
            uITxt_FeedbackEdit.Text = this.FullSoftwareDemoParams.UITxt_FeedbackEditText1;

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(785, 114));

            // Click 'Close' button
            Mouse.Click(uICloseButton, new Point(24, 7));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(170, 115));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(390, 148));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(355, 102));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(354, 38));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(433, 104));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(362, 65));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(363, 36));

            // Click 'Wpf' window
            Mouse.Click(uIWpfWindow, new Point(90, 49));
        }
    }
    
    /// <summary>
    /// Parameters to be passed into 'FullSoftwareDemo'
    /// </summary>
    [GeneratedCode("Coded UITest Builder", "14.0.23107.0")]
    public class FullSoftwareDemoParams
    {
        
        #region Fields
        /// <summary>
        /// Type 'Rob' in 'txt_FirstName' text box
        /// </summary>
        public string UITxt_FirstNameEditText = "Rob";
        
        /// <summary>
        /// Type 'Bor' in 'txt_LastName' text box
        /// </summary>
        public string UITxt_LastNameEditText = "Bor";
        
        /// <summary>
        /// Type 'Robbor' in 'txt_Email' text box
        /// </summary>
        public string UITxt_EmailEditText = "Robbor";
        
        /// <summary>
        /// Select 'Teacher [1]' in 'sel_ApplyingPosition' combo box
        /// </summary>
        public string UISel_ApplyingPositionComboBoxSelectedItem = "Teacher [1]";
        
        /// <summary>
        /// Type '12345678905' in 'txt_PhoneNumber' text box
        /// </summary>
        public string UITxt_PhoneNumberEditText = "12345678905";
        
        /// <summary>
        /// Type 'rbbbb 18' in 'txt_Address' text box
        /// </summary>
        public string UITxt_AddressEditText = "rbbbb 18";
        
        /// <summary>
        /// Type 'Robbor@me.com' in 'txt_Email' text box
        /// </summary>
        public string UITxt_EmailEditText1 = "Robbor@me.com";
        
        /// <summary>
        /// Select '09-May-1971' in 'dat_DoB' date picker
        /// </summary>
        public string UIDat_DoBDatePickerDateAsString = "09-May-1971";
        
        /// <summary>
        /// Type 'Experience' in 'txt_Category' text box
        /// </summary>
        public string UITxt_CategoryEditText = "Experience";
        
        /// <summary>
        /// Type 'How much experience do you have teaching?' in 'txt_Question' text box
        /// </summary>
        public string UITxt_QuestionEditText = "How much experience do you have teaching?";
        
        /// <summary>
        /// Type '0 - 12 months' in cell
        /// </summary>
        public string UIItemCellValue = "0 - 12 months";
        
        /// <summary>
        /// Type '0 - 11 months' in cell
        /// </summary>
        public string UIItemCellValue1 = "0 - 11 months";
        
        /// <summary>
        /// Type '1 - 3 Years' in cell
        /// </summary>
        public string UIItemCellValue2 = "1 - 3 Years";
        
        /// <summary>
        /// Type 'Gave a good response' in 'txt_Feedback' text box
        /// </summary>
        public string UITxt_FeedbackEditText = "Gave a good response";
        
        /// <summary>
        /// Type 'Gave a good response & has sufficient experience' in 'txt_Feedback' text box
        /// </summary>
        public string UITxt_FeedbackEditText1 = "Gave a good response & has sufficient experience";
        #endregion
    }
}
