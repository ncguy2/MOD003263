using System;
using System.Collections.Generic;

/**
 *  Author: Nick Guy
 *  Date: 24/10/2016
 *  Contains: InterviewFoundationInstance
 */
namespace Mod003263.interview {
    /// <summary>
    /// An instance of the foundation, containing the provided answers for each question
    /// </summary>
    public class InterviewFoundationInstance {

        private InterviewFoundation foundation;
        private Dictionary<Question, Answer> answerMap;

        public InterviewFoundation GetFoundation() {
            return foundation;
        }

        public Dictionary<Question, Answer> GetAnswerMap() {
            return answerMap;
        }

    }
}