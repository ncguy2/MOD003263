using System.Collections.Generic;
using System.Linq;

/**
 *  Author: Nick Guy
 *  Date: 24/10/2016
 *  Contains: MetricCalculator
 */

namespace Mod003263.interview.metric {
    /// <summary>
    /// Calculates a single metric value from an interview instance
    /// </summary>
    public class MetricCalculator {

        private float maxMetric = -1;
        private Interview interview;
        private Dictionary<Question, Answer> answerMap;

        /// <summary>
        /// Entry point into metric calculator, calculates the interview metric based upon questions and answers found within the interview instance
        /// </summary>
        /// <param name="i">Interview instance to calculate the metric for</param>
        /// <returns>The calculated metric, also stored in i.<see cref="Interview.resultMetric"/></returns>
        public float CalculateMetric(Interview i) {
            this.interview = i;
            this.answerMap = i.GetFoundationInstance().GetAnswerMap();
            CalculateMaxMetric();
            AssertValues();

//            int metric = answerMap.Sum(CalculateLocalMetric);
            float metric = 0;
            answerMap.Keys.ToList().ForEach(q => {
                float m = CalculateLocalMetric(q, answerMap[q]);
                metric += m;
            });

            if (metric > 0)
                metric = metric / maxMetric;

            CleanValues();
            return i.SetResultMetric(metric).GetResultMetric();
        }

        /// <summary>
        /// Calculates the max possible metric from the questions included with the interview
        /// </summary>
        private void CalculateMaxMetric() {
            if (this.interview == null) {
                this.maxMetric = -1;
                return;
            }
            this.maxMetric = CalculateMaxMetric(this.interview);
        }

        /// <summary>
        /// Public variant of internal <see cref="CalculateMaxMetric()"/>
        /// </summary>
        /// <param name="interview">The interview instance to use</param>
        /// <returns>the max possible metric attainable for the interview</returns>
        public float CalculateMaxMetric(Interview interview) {
            answerMap = interview.GetFoundationInstance().GetAnswerMap();
            return answerMap.Keys.Sum(q => interview.GetFoundationInstance().GetQuestionWeight(q));
        }

        /// <summary>
        /// Helper method to allow for LINQ parsing
        /// </summary>
        /// <param name="pair">Pair of question and provided answer</param>
        /// <returns>The local metric of the provided answer</returns>
        private float CalculateLocalMetric(KeyValuePair<Question, Answer> pair) {
            return CalculateLocalMetric(pair.Key, pair.Value);
        }

        /// <summary>
        /// Calculates the metric for a specific question based upon the answer
        /// </summary>
        /// <param name="question">The owning question</param>
        /// <param name="answer">The provided answer</param>
        /// <returns>The local metric of the provided answer</returns>
        private float CalculateLocalMetric(Question question, Answer answer) {
            float weight = interview.GetFoundationInstance().GetQuestionWeight(question);
            float w = (weight * (answer.Weight / 100f));
            return w;
        }

        /// <summary>
        /// Ensures all values are valid before continuing
        /// </summary>
        /// <exception cref="MetricException">Thrown to halt the processing of the metric calculations</exception>
        private void AssertValues() {
            if(this.interview == null) throw new MetricException("No valid interview instance provided");
            if(this.answerMap == null) throw new MetricException("No answer map provided");
            if(this.maxMetric <= -1) throw new MetricException($"Maximum metric calculated to invalid value \"{this.maxMetric}\"");
        }

        /// <summary>
        /// Cleans up values to prevent them from interfering with subsequent calculations
        /// </summary>
        private void CleanValues() {
            this.interview = null;
            this.answerMap = null;
            this.maxMetric = -1;
        }

    }
}