using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mod003263;
using Mod003263.interview;
using Mod003263.interview.metric;

namespace Mod003263_Test {

    [TestClass]
    public class ControllerTests1 {

        /// <summary>
        /// Given sample data, calculate the correct metric
        /// </summary>
        [TestMethod]
        public void Test1() {
            MetricCalculator calc = new MetricCalculator();
            Interview interview = ImperfectSet();
            float max = calc.CalculateMaxMetric(interview);
            float metric = calc.CalculateMetric(interview);
            max /= 2;
            Console.WriteLine("{0}/{1}", metric, max);
            Assert.AreEqual(max/100, metric);
        }

        /// <summary>
        /// Given perfect data, calculate the correct metric
        /// </summary>
        [TestMethod]
        public void Test2() {
            MetricCalculator calc = new MetricCalculator();
            Interview interview = PerfectSet();
            float max = calc.CalculateMaxMetric(interview);
            float metric = calc.CalculateMetric(interview);
            Console.WriteLine("{0}/{1}", metric, max);
            Assert.AreEqual(max/100, metric);
        }

        /// <summary>
        /// Given multiple data sets (including a perfect set), return a list sorted by metric result
        /// </summary>
        [TestMethod]
        public void Test3() {
            Interview perfectSet = PerfectSet();
            Interview imperfectSet = ImperfectSet();
            Interview wrongSet = WrongSet();

            List<Interview> sets = new List<Interview> {wrongSet, perfectSet, imperfectSet};

            MetricCalculator calc = new MetricCalculator();
            sets.ForEach(i => calc.CalculateMetric(i));
            sets.Sort((i1, i2) => {
                if (i1.GetResultMetric() < i2.GetResultMetric()) return 1;
                if (i1.GetResultMetric() > i2.GetResultMetric()) return -1;
                return 0;
            });

            Assert.AreEqual(sets[0], perfectSet);
            Assert.AreEqual(sets[1], imperfectSet);
            Assert.AreEqual(sets[2], wrongSet);

        }

        private static Interview PerfectSet() {
            Interview interview = new Interview();
            Dictionary<Question, int> questions = interview.GetFoundationInstance().GetFoundation().GetQuestions();
            questions.Add(new Question(0).AddAnswers(new Answer().SetWeight(100)), 5);
            questions.Add(new Question(1).AddAnswers(new Answer().SetWeight(100)), 7);
            questions.Add(new Question(2).AddAnswers(new Answer().SetWeight(100)), 15);
            questions.Add(new Question(3).AddAnswers(new Answer().SetWeight(100)), 71);
            questions.Add(new Question(4).AddAnswers(new Answer().SetWeight(100)), 2);
            foreach (Question q in questions.Keys)
                interview.GetFoundationInstance().GetAnswerMap().Add(q, q.GetAnswers()[0]);
            return interview;
        }

        private static Interview ImperfectSet() {
            Interview interview = new Interview();
            Dictionary<Question, int> questions = interview.GetFoundationInstance().GetFoundation().GetQuestions();
            questions.Add(new Question(0).AddAnswers(new Answer().SetWeight(50)), 5);
            questions.Add(new Question(1).AddAnswers(new Answer().SetWeight(50)), 7);
            questions.Add(new Question(2).AddAnswers(new Answer().SetWeight(50)), 15);
            questions.Add(new Question(3).AddAnswers(new Answer().SetWeight(50)), 71);
            questions.Add(new Question(4).AddAnswers(new Answer().SetWeight(50)), 2);
            foreach (Question q in questions.Keys)
                interview.GetFoundationInstance().GetAnswerMap().Add(q, q.GetAnswers()[0]);
            return interview;
        }

        private static Interview WrongSet() {
            Interview interview = new Interview();
            Dictionary<Question, int> questions = interview.GetFoundationInstance().GetFoundation().GetQuestions();
            questions.Add(new Question(0).AddAnswers(new Answer().SetWeight(0)), 5);
            questions.Add(new Question(1).AddAnswers(new Answer().SetWeight(0)), 7);
            questions.Add(new Question(2).AddAnswers(new Answer().SetWeight(0)), 15);
            questions.Add(new Question(3).AddAnswers(new Answer().SetWeight(0)), 71);
            questions.Add(new Question(4).AddAnswers(new Answer().SetWeight(0)), 2);
            foreach (Question q in questions.Keys)
                interview.GetFoundationInstance().GetAnswerMap().Add(q, q.GetAnswers()[0]);
            return interview;
        }

    }

    [TestClass]
    public class ControllerTests2 {


        /// <summary>
        /// Load properties upon initialization from "properties.dat"
        /// </summary>
        [TestMethod]
        public void Test1() {
            PropertiesManager mgr = PropertiesManager.GetInstance();
            foreach (string key in mgr.GetKeys()) {
                Console.WriteLine("{0}: {1}", key, mgr.GetProperty(key));
            }
        }

        /// <summary>
        /// If Properties.dat contains Provider key, Provider exists in cache
        /// </summary>
        [TestMethod]
        public void Test2() {
            PropertiesManager mgr = PropertiesManager.GetInstance();

            Assert.IsTrue(mgr.HasProperty("Provider"));
            Assert.AreEqual("MySQL", mgr.GetProperty("Provider"));
        }

    }

}