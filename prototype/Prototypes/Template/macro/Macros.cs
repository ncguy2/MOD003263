using System;

namespace Template.macro {
    public class Macros {

        [Macro("name")]
        public static String Name() {
            return "Dave";
        }

        [Macro("age")]
        public static String Age() {
            return "24";
        }

    }
}