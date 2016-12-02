using System;
using System.Management.Instrumentation;
using System.Runtime.Remoting.Messaging;
using Mod003263.db;

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

        private int Id;
        private Applicant subject;
        private InterviewFoundationInstance foundation;
        private int flag;
        private float resultMetric;

        public Interview() {
            foundation = new InterviewFoundationInstance(new InterviewFoundation());
        }

        public Interview SetResultMetric(float metric) {
            this.resultMetric = metric;
            return this;
        }

        public float GetResultMetric() {
            return this.resultMetric;
        }

        public InterviewFoundationInstance GetFoundationInstance() {
            return this.foundation;
        }

        public Applicant Subject => subject;

        public int Flag => flag;

        public int ID => Id;

        public static class Flags {
            public const int COMPLETE = 1;
            public const int SUCCESS = 2;
            public const int REVIEWED = 4;
        }
    }
}