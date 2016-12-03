using Mod003263.interview;

/**
 * Author: Nick Guy
 * Date: 30/11/2016
 * Contains: SaveQuestionEvent, SaveQuestionListener
 */
namespace Mod003263.events.io {

    public class SaveQuestionEvent : AbstractEvent {

        public Question Payload { get; set; }

        public SaveQuestionEvent() : this(null) {}

        public SaveQuestionEvent(Question payload) {
            Payload = payload;
        }

        public interface SaveQuestionListener {
            [Event]
            void OnSaveQuestion(SaveQuestionEvent e);
        }
    }
}