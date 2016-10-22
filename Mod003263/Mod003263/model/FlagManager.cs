/**
 *  Author: Nick Guy
 *  Date: 20/10/2016
 *  Classes: FlagManager
 */
namespace Mod003263.model {
    /// <summary>
    /// A singleton class to execute bitwise comparisons on the flag fields
    /// </summary>
    public class FlagManager {

        private static FlagManager Instance;
        public static FlagManager GetInstance() {
            return Instance ?? (Instance = new FlagManager());
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