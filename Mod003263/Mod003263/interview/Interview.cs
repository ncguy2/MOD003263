using System;

/**
 *  Author: Nick Guy
 *  Date: 24/10/2016
 *  Contains: Interview, Interview.Flags
 */
namespace Mod003263.interview {
    /// <summary>
    /// Contains all information related to an interview
    /// </summary>
    public class Interview {

        private object subject; // TODO replace with Subject object when implemented
        private InterviewFoundationInstance foundation;
        private int flag;
        private int resultMetric;

        public Interview SetResultMetric(int metric) {
            this.resultMetric = metric;
            return this;
        }

        public int GetResultMetric() {
            return this.resultMetric;
        }

        public InterviewFoundationInstance GetFoundationInstance() {
            return this.foundation;
        }

        public static class Flags {
            public const int COMPLETE = 1;
            public const int SUCCESS = 2;
            public const int REVIEWED = 4;
        }
    }
}