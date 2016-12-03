using Mod003263.interview;

namespace Mod003263.events.io {

    public class DeletePositionEvent : AbstractEvent {

        public AvailablePosition Position;

        public DeletePositionEvent() : this(null) {}

        public DeletePositionEvent(AvailablePosition position) {
            Position = position;
        }

        public interface DeletePositionListener {
            [Event]
            void OnDeletePosition(DeletePositionEvent e);
        }

    }
}