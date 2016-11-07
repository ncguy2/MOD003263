using System;
using System.Collections.Generic;

/**
 *  Author: Nick Guy
 *  Date: 24/10/2016
 *  Contains: Question
 */
namespace Mod003263.interview {
    /// <summary>
    /// Stores the data for the question
    /// </summary>
    public class Question {

        private String category;
        private String questionText;
        private int weight;
        private List<Answer> answers;

        public int Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public string Cat() {
            return category;
        }

        public string Text() {
            return questionText;
        }

        public string Path() {
            return Cat() + '|' + Text();
        }

    }
}