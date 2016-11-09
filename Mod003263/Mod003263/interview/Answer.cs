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

        private String text;
        private int weight;

        public String Text => text;
        public int Weight => weight;
    }
}