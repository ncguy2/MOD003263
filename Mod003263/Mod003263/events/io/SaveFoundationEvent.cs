using Mod003263.interview;

/**
 *  Author: Nick Guy
 *  Date: 01/12/2016
 *  Contains: SaveFoundationEvent, SaveFoundationListener
 */
namespace Mod003263.events.io {
    public class SaveFoundationEvent : AbstractEvent {
        public InterviewFoundation Foundation { get; set; }

        public SaveFoundationEvent() : this(null) {}
        public SaveFoundationEvent(InterviewFoundation foundation) {
            Foundation = foundation;
        }

        public interface SaveFoundationListener{
            [Event]
            void OnSaveFoundation(SaveFoundationEvent e);
        }

    }
}