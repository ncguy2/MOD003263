namespace Mod003263.events.ui {
    public class InterviewToApplicantEvent : AbstractEvent {

        public interface InterviewToApplicantListener {
            [Event]
            void OnInterviewToApplicant(InterviewToApplicantEvent e);
        }

    }
}