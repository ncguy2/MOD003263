using Mod003263.db;

/**
 * Author: Nick Guy
 * Date: 30/11/2016
 * Contains: SelectApplicantEvent, SelectApplicantListener
 */
namespace Mod003263.events.ui {

    public class SelectApplicantEvent : AbstractEvent {

        public SelectApplicantEvent() : this(null) {}

        public SelectApplicantEvent(Applicant selected) {
            this.Selected = selected;
        }

        public Applicant Selected { get; set; }

        public interface SelectApplicantListener {
            [Event]
            void OnSelectApplicant(SelectApplicantEvent e);
        }

    }
}