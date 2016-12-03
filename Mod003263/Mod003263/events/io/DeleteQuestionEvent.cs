using Mod003263.interview;

namespace Mod003263.events.io {
    public class DeleteQuestionEvent : AbstractEvent {

        public Question Question { get; set; }

        public DeleteQuestionEvent() : this(null) {}

        public DeleteQuestionEvent(Question question) {
            Question = question;
        }

        public interface DeleteQuestionListener {
            [Event]
            void OnDeleteQuestion(DeleteQuestionEvent e);
        }

    }
}