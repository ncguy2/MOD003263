using Mod003263.interview;

namespace Mod003263.events.io {

    public class SavePositionEvent : AbstractEvent {

        public AvailablePosition Position { get; set; }

        public SavePositionEvent() : this(null) {}

        public SavePositionEvent(AvailablePosition position) {
            Position = position;
        }

        public interface SavePositionListener {
            [Event]
            void OnSavePosition(SavePositionEvent e);
        }

    }
}