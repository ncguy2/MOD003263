namespace Mod003263.controllerview {
    public class ControllerViewManager {

        private static ControllerViewManager instance;
        public static ControllerViewManager Instance() {
            return instance ?? (instance = new ControllerViewManager());
        }

        private ControllerViewManager() {}

    }
}