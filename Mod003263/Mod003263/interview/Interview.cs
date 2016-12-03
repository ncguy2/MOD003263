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

        private int id;
        private InterviewFoundationInstance foundation;
        private float resultMetric;

        public Interview(int id, InterviewFoundation foundation) {
            this.id = id;
            this.foundation = new InterviewFoundationInstance(foundation);
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

        public Applicant Subject { get; set; }

        public int Flag { get; set; }

        public int ID => id;

        public static class Flags {
            public const int COMPLETE = 1;
            public const int SUCCESS = 2;
            public const int REVIEWED = 4;
        }
    }
}