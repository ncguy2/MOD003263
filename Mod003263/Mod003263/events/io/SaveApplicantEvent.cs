using Mod003263.db;

/**
 *  Author: Nick Guy
 *  Date: 01/12/2016
 *  Contains: SaveApplicantEvent, SaveApplicantListener
 */
namespace Mod003263.events.io {

    public class SaveApplicantEvent : AbstractEvent {
        public Applicant Applicant { get; set; }

        public SaveApplicantEvent() : this(null) {}

        public SaveApplicantEvent(Applicant applicant) {
            Applicant = applicant;
        }

        public interface SaveApplicantListener {
            [Event]
            void OnSaveApplicant(SaveApplicantEvent e);
        }

    }
}