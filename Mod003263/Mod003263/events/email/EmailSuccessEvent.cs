namespace Mod003263.events.email {

    public class EmailSuccessEvent : AbstractEvent {

        public interface EmailSuccessEventListener {
            [Event]
            void OnEmailSuccess(EmailSuccessEvent e);
        }

    }
}