using Mod003263.interview;

/**
 *  Author: Nick Guy
 *  Date: 01/12/2016
 *  Contains: SaveInterviewEvent, SaveInterviewListener
 */
namespace Mod003263.events.io {
    public class SaveInterviewEvent : AbstractEvent {

        public Interview Interview { get; set; }

        public SaveInterviewEvent() : this(null){}
        public SaveInterviewEvent(Interview interview) {
            Interview = interview;
        }

        public interface SaveInterviewListener {
            [Event]
            void OnSaveInterview(SaveInterviewEvent e);
        }
    }
}