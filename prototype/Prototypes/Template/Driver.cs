using System;
using System.Collections.Generic;
using Template.macro;

namespace Template {
    public class Driver {

        public static void Main(string[] args) {
            MacroManager mgr = MacroManager.Instance();
            // Register domains
            mgr.RegisterMacroDomain(typeof(Macros));

            // Search domains for valid macros, and register them
            MacroFinder.FindAndRegister();

            foreach (KeyValuePair<string,Func<string>> pair in mgr.RegisteredMacros)
                Console.WriteLine("Found macro with key \"{{{0}}}\" and value of \"{1}\"", pair.Key, pair.Value());

            Console.WriteLine("\n=====\n");

            Template tmp = new Template("Hello, My name is {name} \n" +
                                        "and I am {age} years old! \n" +
                                        "I'll be doing {task} soon");

            Console.WriteLine(tmp.ToString());
            Console.WriteLine("\n=====\n");
            Console.WriteLine(tmp.Populate());

        }

    }
}