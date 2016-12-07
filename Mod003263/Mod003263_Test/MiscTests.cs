using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mod003263.events;
using Mod003263.events.test;
using Mod003263.utils;
using Mod003263.wpf;

/**
 * Author: Nick Guy
 * Date: 07/12/2016
 * Contains: MiscTests
 */
namespace Mod003263_Test {

    [TestClass]
    public class MiscTests : StringPayloadEvent.StringPayloadListener {

        #region EventBus

        private String payload;

        [TestMethod]
        public void EventTest() {
            // Disable threaded factory
            EventBus.SetFactoryThreaded(false);
            // Register this object as a listener
            EventBus.GetInstance().Register(this);
            // Fire the StringPayloadEvent with the payload of "Lowley"
            new StringPayloadEvent("Lowley").Fire();
            // Check that the payload was received correctly
            Assert.AreEqual("Lowley", this.payload);
        }

        [Event]
        public void OnStringPayload(StringPayloadEvent e) {
            this.payload = e.Payload;
        }

        #endregion

        #region Utils

        [TestMethod]
        public void ConversionTest() {
            DateTime dateTime = new DateTime(2016, 12, 07, 12, 18, 00);
            long epoch = ConversionUtils.ToEpochTime(dateTime);
            Assert.AreEqual(1481113080, epoch);
            DateTime epochDateTime = ConversionUtils.ToDateTimeFromEpoch(epoch);
            Assert.AreEqual(dateTime.ToLongDateString(), epochDateTime.ToLongDateString());
        }

        [TestMethod]
        public void RegexTest() {
            String valid   = "happytech@gmail.com";
            String invalid = "happytechisgreatfun";

            bool validIsValid   = RegexUtilities.IsEmailValid(valid);
            bool invalidIsValid = RegexUtilities.IsEmailValid(invalid);

            Assert.AreEqual(true, validIsValid);
            Assert.AreEqual(false, invalidIsValid);
            Assert.AreNotEqual(validIsValid, invalidIsValid);
        }

        #endregion

    }
}