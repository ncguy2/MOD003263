using System;
using Mod003263.interview;

/**
 * Author: Nick Guy
 * Date: 30/11/2016
 * Contains: SelectTemplateEvent, SelectTemplateListener, SelectTemplateScopes
 */
namespace Mod003263.events.ui {

    public class SelectTemplateEvent : AbstractEvent {
        public SelectTemplateEvent() : this(null, "") {}

        public SelectTemplateEvent(String scope) : this(null, scope) {}
        public SelectTemplateEvent(InterviewFoundation template) : this(template, "") {}

        public SelectTemplateEvent(InterviewFoundation template, String scope) {
            this.Template = template;
            this.Scope = scope;
        }

        public InterviewFoundation Template { get; set; }
        public String Scope { get; set; }

        public interface SelectTemplateListener {
            [Event]
            void OnSelectTemplate(SelectTemplateEvent e);
        }

    }

    public class SelectTemplateScopes {

        public const String TEMPLATE_EDITOR = "select.template.editor";
        public const String TEMPLATE_USAGE = "select.template.interview";

    }

}