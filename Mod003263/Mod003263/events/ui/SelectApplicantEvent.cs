using Mod003263.db;

/**
 * Author: Nick Guy
 * Date: 30/11/2016
 * Contains: SelectApplicantEvent, SelectApplicantListener, SelectApplicantScopes
 */
namespace Mod003263.events.ui {

    public class SelectApplicantEvent : AbstractEvent {

        public SelectApplicantEvent() : this(null, "") {}

        public SelectApplicantEvent(Applicant selected) : this(selected, "") {}

        public SelectApplicantEvent(string scope) : this(null, scope) {}

        public SelectApplicantEvent(Applicant selected, string scope) {
            Selected = selected;
            Scope = scope;
        }

        public Applicant Selected { get; set; }
        public string Scope { get; set; }

        public interface SelectApplicantListener {
            [Event]
            void OnSelectApplicant(SelectApplicantEvent e);
        }

    }

    public class SelectApplicantScopes {
        public const string APPLICANT_EDITOR = "applicant.editor";
        public const string APPLICANT_USAGE = "applicant.usage";
        public const string APPLICANT_RANKING = "applicant.ranking";
    }

}