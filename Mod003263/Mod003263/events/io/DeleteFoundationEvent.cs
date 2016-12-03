using Mod003263.interview;

namespace Mod003263.events.io {
    public class DeleteFoundationEvent : AbstractEvent {

        public InterviewFoundation Foundation { get; set; }

        public DeleteFoundationEvent() : this(null) {}

        public DeleteFoundationEvent(InterviewFoundation foundation) {
            Foundation = foundation;
        }

        public interface DeleteFoundationListener {
            [Event]
            void OnDeleteFoundation(DeleteFoundationEvent e);
        }

    }
}