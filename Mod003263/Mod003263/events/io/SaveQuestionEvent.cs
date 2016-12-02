
/**
 *  Author: Nick Guy
 *  Date: 01/12/2016
 *  Contains: SaveQuestionEvent, SaveQuestionListener
 */

using Mod003263.interview;

namespace Mod003263.events.io {
    public class SaveQuestionEvent : AbstractEvent {

        public Question Question { get; set; }

        public SaveQuestionEvent() : this(null) {}

        public SaveQuestionEvent(Question question){
            Question = question;
        }

        public interface SaveQuestionListener {
            [Event]
            void OnSaveQuestion(SaveQuestionEvent e);
        }
    }
}