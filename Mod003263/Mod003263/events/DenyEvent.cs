using Mod003263.db;

namespace Mod003263.events {
    public class DenyEvent : AbstractEvent {

        public Applicant Applicant { get; set; }

        public DenyEvent() : this(null) {}

        public DenyEvent(Applicant applicant) {
            Applicant = applicant;
        }

        public interface DenyListener {
            [Event]
            void OnDeny(DenyEvent e);
        }

    }
}