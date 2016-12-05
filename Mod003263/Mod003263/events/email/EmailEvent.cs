using System.Net.Mail;

namespace Mod003263.events.email {

    public class EmailEvent : AbstractEvent {

        public MailMessage Message { get; set; }

        public EmailEvent() : this(null) {}

        public EmailEvent(MailMessage message) {
            Message = message;
        }

        public interface EmailListener {
            [Event]
            void OnEmail(EmailEvent e);
        }

    }
}