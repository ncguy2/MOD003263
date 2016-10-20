namespace Mod003263.model {
    public class FlagManager {

        private static FlagManager instance;
        public static FlagManager Instance() {
            return instance ?? (instance = new FlagManager());
        }

        private FlagManager() {}

        /// <summary>
        /// Executes a bitwise AND function with the provided values
        /// </summary>
        /// <param name="composite">The full flag store</param>
        /// <param name="flag">The integer value of the flag, typically only has a single true bit</param>
        /// <returns>If the composite has the flag bit as true</returns>
        public bool IsFlagged(int composite, int flag) {
            return (composite & flag) != 0;
        }

    }
}