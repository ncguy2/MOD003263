using System;
using System.Collections.Generic;
using System.Linq;

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

        private int id;
        private String name;
        private String category;
        private Dictionary<Question, int> questions;
        public String Position { get; set; }

        public InterviewFoundation() : this(-1, "", "") {}

        public InterviewFoundation(int id, String name, String category) {
            this.id = id;
            this.name = name;
            this.category = category;
            questions = new Dictionary<Question, Int32>();
        }


        public int GetQuestionWeight(Question question) {
            return questions.ContainsKey(question) ? questions[question] : 0;
        }

        public Dictionary<Question, int> GetQuestions() {
            return this.questions;
        }

        public int GetQuestionsWeight() {
            return GetQuestions().Sum(pair => pair.Value);
        }

        public override string ToString() {
            return Name();
        }

        public String Name() { return name; }
        public String Cat() { return category; }
        public String Path() { return Cat() + "/" + Name(); }

        public void SetName(string n) { name = n; }
        public void SetCat(string c) { category = c; }

        public int Id() { return id; }
        public void Id(int id) { this.id = id; }

        protected bool Equals(InterviewFoundation other) {
            return id == other.id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((InterviewFoundation) obj);
        }

        public override int GetHashCode()
        {
            return id;
        }
    }

}