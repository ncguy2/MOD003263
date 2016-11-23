using System;
using System.Collections.Generic;

/**
 *  Author: Nick Guy
 *  Date: 24/10/2016
 *  Contains: InterviewFoundation
 */
namespace Mod003263.interview {
    /// <summary>
    /// Compilation of interview questions, to be used as a single entity
    /// </summary>
    public class InterviewFoundation {

        private String name;
        private String category;
        private Dictionary<Question, int> questions;

        public InterviewFoundation() {
            questions = new Dictionary<Question, Int32>();
        }

        public int GetQuestionWeight(Question question) {
            return questions.ContainsKey(question) ? questions[question] : 0;
        }

        public Dictionary<Question, int> GetQuestions() {
            return this.questions;
        }

    }

}