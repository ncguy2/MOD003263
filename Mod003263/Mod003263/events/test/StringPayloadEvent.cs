using System;

namespace Mod003263.events.test {

    public class StringPayloadEvent : AbstractEvent {
        public StringPayloadEvent() : this("") {}

        public StringPayloadEvent(String payload) {
            this.Payload = payload;
        }

        public String Payload { get; set; }

        public interface StringPayloadListener {
            [Event]
            void OnStringPayload(StringPayloadEvent e);
        }
    }
}