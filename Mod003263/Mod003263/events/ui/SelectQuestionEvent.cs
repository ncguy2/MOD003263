using System;
using Mod003263.interview;

/**
 * Author: Nick Guy
 * Date: 30/11/2016
 * Contains: SelectQuestionEvent, SelectQuestionListener, SelectQuestionScopes
 */
namespace Mod003263.events.ui {
    public class SelectQuestionEvent : AbstractEvent {
        public SelectQuestionEvent() : this(null, "") {}

        public SelectQuestionEvent(String scope) : this(null, scope) {}
        public SelectQuestionEvent(Question question) : this(question, "") {}

        public SelectQuestionEvent(Question question, String scope) {
            this.Question = question;
            this.Scope = scope;
        }

        public Question Question { get; set; }
        public String Scope { get; set; }

        public interface SelectQuestionListener {
            [Event]
            void OnSelectQuestion(SelectQuestionEvent e);
        }
    }

    public class SelectQuestionScopes {

        public const String QUESTION_EDITOR = "select.question.editor";
        public const String QUESTION_USAGE = "select.question.interview";

    }
}