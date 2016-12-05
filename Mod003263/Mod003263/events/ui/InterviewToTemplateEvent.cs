namespace Mod003263.events.ui {
    public class InterviewToTemplateEvent : AbstractEvent {

        public interface InterviewToTemplateListener {
            [Event]
            void OnInterviewToTemplate(InterviewToTemplateEvent e);
        }

    }
}