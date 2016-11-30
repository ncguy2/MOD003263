using System;

/**
 * Author: Nick Guy
 * Date: 28/11/2016
 * Contains: StringPayloadEvent
 */
namespace Mod003263.events.test {

    /// <summary>
    /// A test event to demonstrate implementation usage for the pub-sub event system
    /// </summary>
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