using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mod003263;
using System.IO;
using System.Linq;

namespace Mod003263_Test {

    [TestClass]
    public class DataModelTests1 {

        /// <summary>
        /// Take database credentials from properties file
        /// </summary>
        [TestMethod]
        public void Test1()
        {
            string propfile = "properties.dat";
            string fileData = "";
            using (StreamReader sr = new StreamReader(propfile))
            {
                fileData = sr.ReadToEnd().Replace("\r", "");
            }
            string[] kvp;
            string[] records = fileData.Split("\n".ToCharArray());
            foreach (string record in records)
            {
                kvp = record.Split("=".ToCharArray());
            }
            records.ToList().ForEach(i => Console.WriteLine(i.ToString()));
        }

        /// <summary>
        /// Connect to database and load details
        /// </summary>
        [TestMethod]
        public void Test2()
        {

        }
    }

    [TestClass]
    public class DataModelTests2 {

        /// <summary>
        /// Delete entries with a timestamp difference greater than a set threshold
        /// </summary>
        [TestMethod]
        public void Test1() {

        }

    }

}
