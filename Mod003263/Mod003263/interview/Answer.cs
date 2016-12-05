using System;

/**
 *  Author: Nick Guy
 *  Date: 24/10/2016
 *  Contains: Answer
 */
namespace Mod003263.interview {
    /// <summary>
    /// Stores the answer text to the containing question, along with the local weighting
    /// </summary>
    public class Answer {

        private string text;
        private int weight;

        public string Text => text;

        public int Weight => weight;
        public int Id { get; private set; }

        public Answer SetText(string text) {
            this.text = text;
            return this;
        }

        public Answer SetWeight(int weight) {
            this.weight = weight;
            return this;
        }
        public Answer(int id) {
            this.Id = id;
        }
    }
}