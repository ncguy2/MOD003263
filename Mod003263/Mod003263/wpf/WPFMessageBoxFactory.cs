﻿using System;

/**
 *  Author: Guy
 *  Date: 04/11/2016
 *  Contains: WPFMessageBoxFactory, WPFMessageBoxFactoryData
 */
namespace Mod003263.wpf {
    /// <summary>
    /// A factory class to create wpf-styled message boxes
    /// </summary>
    public class WPFMessageBoxFactory {

        /// <summary>
        /// Creates a WPF-styled message box, displaying the provided data
        /// </summary>
        /// <param name="data">The data required to build the form</param>
        /// <returns>The message box instance created</returns>
        public static WPFMessageBoxForm Create(WPFMessageBoxFactoryData data) {
            WPFMessageBoxForm form = new WPFMessageBoxForm();
            form.SetHeader(data.Header).SetContent(data.Content).SetBtnMask(data.Mask);
            form.SetBtnActions(data.OnLeft, data.OnMid, data.OnRight);
            return form;
        }

        /// <summary>
        /// Helper method for <see cref="Create(Mod003263.wpf.WPFMessageBoxFactoryData)"/>
        /// </summary>
        /// <param name="header">The message box title</param>
        /// <param name="content">The contents of the message box</param>
        /// <param name="mask">The buttons to display</param>
        /// <returns>The message box instance, for action binding</returns>
        public static WPFMessageBoxForm Create(String header, String content, int mask) {
            return Create(new WPFMessageBoxFactoryData{Header = header, Content = content, Mask = mask});
        }

    }

    /// <summary>
    /// A struct containing the necessary data to create a message box
    /// </summary>
    public struct WPFMessageBoxFactoryData {
        public string Header { get; set; }
        public string Content { get; set; }
        public int Mask { get; set; }
        public Action<WPFMessageBoxForm> OnLeft { get; set; }
        public Action<WPFMessageBoxForm> OnMid { get; set; }
        public Action<WPFMessageBoxForm> OnRight { get; set; }
    }

}