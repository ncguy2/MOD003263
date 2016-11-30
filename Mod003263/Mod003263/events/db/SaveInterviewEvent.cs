
/**
 * Author: Nick Guy
 * Date: 30/11/2016
 * Contains: SaveInterviewEvent, SaveInterviewListener
 */

using Mod003263.interview;

namespace Mod003263.events.db {

    public class SaveInterviewEvent : AbstractEvent {

        public Interview Interview { get; set; }

        public SaveInterviewEvent() : this(null) {}

        public SaveInterviewEvent(Interview interview) {
            Interview = interview;
        }

        public interface SaveInterviewListener {
            [Event]
            void OnSaveInterview(SaveInterviewEvent e);
        }

    }
}