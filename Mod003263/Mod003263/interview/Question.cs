using System;
using System.Collections.Generic;
using System.IO.Pipes;

/**
 *  Author: Nick Guy
 *  Date: 24/10/2016
 *  Contains: Question
 */
namespace Mod003263.interview {
    /// <summary>
    /// Stores the data for the question
    /// </summary>
    public class Question
    {
        private String category;
        private String questionText;
        private List<Answer> answers;

        public int Id { get; }

        public Question(int id) {
            Id = id;
            answers = new List<Answer>();
        }

        public Question AddAnswers(params Answer[] answers) {
            this.answers.AddRange(answers);
            return this;
        }

        public string Cat() {
            return category;
        }

        public string Text() {
            return questionText;
        }

        public string Path() {
            return Cat() + '/' + Text();
        }

        public List<Answer> GetAnswers() {
            return this.answers;
        }

    }
}