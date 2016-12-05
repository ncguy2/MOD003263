using Mod003263.db;
using Mod003263.interview;

namespace Mod003263.events {

    public class HireEvent : AbstractEvent {

        public Applicant Applicant { get; set; }
        public AvailablePosition Position { get; set; }

        public HireEvent() : this(null, null) {}
        public HireEvent(Applicant applicant, AvailablePosition position) {
            Applicant = applicant;
            Position = position;
        }

        public interface HireListener {
            [Event]
            void OnHire(HireEvent e);
        }

    }
}