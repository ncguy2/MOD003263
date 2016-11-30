using Mod003263.interview;

/**
 * Author: Nick Guy
 * Date: 30/11/2016
 * Contains: SaveTemplateEvent, SaveTemplateListener
 */
namespace Mod003263.events.db {

    public class SaveTemplateEvent : AbstractEvent {

        public InterviewFoundation Template { get; set; }

        public SaveTemplateEvent() : this(null) {}

        public SaveTemplateEvent(InterviewFoundation template) {
            Template = template;
        }

        public interface SaveTemplateListener {
            [Event]
            void OnSaveTemplate(SaveTemplateEvent e);
        }

    }
}