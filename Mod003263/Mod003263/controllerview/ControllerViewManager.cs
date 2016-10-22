/**
 *  Author: Nick Guy
 *  Date: 20/10/2016
 *  Classes: ControllerViewManager
 */
namespace Mod003263.controllerview {
    /// <summary>
    /// Manager to ensure that each controller is paired up with the correct view when a switch is made
    /// </summary>
    public class ControllerViewManager {

        private static ControllerViewManager Instance;
        public static ControllerViewManager GetInstance() {
            return Instance ?? (Instance = new ControllerViewManager());
        }

        private ControllerViewManager() {}

    }
}