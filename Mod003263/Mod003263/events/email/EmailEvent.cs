namespace Mod003263.events.email {

    public class EmailEvent : AbstractEvent {

        public interface EmailListener {
            [Event]
            void OnEmail(EmailEvent e);
        }

    }
}