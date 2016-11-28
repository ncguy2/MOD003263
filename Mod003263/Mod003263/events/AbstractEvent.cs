
/**
 *  Author: Nick Guy
 *  Date: 24/11/2016
 *  Contains: AbstractEvent
 */
namespace Mod003263.events {

    /// <summary>
    /// Abstract class for the Pub-Sub event system
    /// </summary>
    public abstract class AbstractEvent {

        public void Fire() {
            EventBus.GetInstance().Post(this);
        }

    }
}