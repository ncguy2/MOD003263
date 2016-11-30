using System;
using System.Collections.Generic;

/**
 *  Author: Nick Guy
 *  Date: 24/10/2016
 *  Contains: InterviewFoundationInstance
 */
namespace Mod003263.interview {
    /// <summary>
    /// An instance of the foundation, decorating the foundation base
    /// </summary>
    public class InterviewFoundationInstance {

        private InterviewFoundation foundation;
        private Dictionary<Question, Answer> answerMap;

        public InterviewFoundationInstance(InterviewFoundation foundation) {
            this.foundation = foundation;
            this.answerMap = new Dictionary<Question, Answer>();
        }

        public InterviewFoundation GetFoundation() {
            return foundation;
        }

        public Dictionary<Question, Answer> GetAnswerMap() {
            return answerMap;
        }

        public int GetQuestionWeight(Question question) {
            return foundation.GetQuestionWeight(question);
        }

    }
}