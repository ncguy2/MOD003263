using System;
using System.Collections.Generic;

namespace Template.macro {
    public class MacroManager {

        private static MacroManager instance;
        public static MacroManager Instance() {
            if(instance == null) instance = new MacroManager();
            return instance;
        }

        private Dictionary<String, Func<String>> registeredMacros;
        private List<Type> macroDomains;

        private MacroManager() {
            registeredMacros = new Dictionary<String, Func<String>>();
            macroDomains = new List<Type>();
        }

        public void RegisterMacroDomain(Type domain) {
            macroDomains.Add(domain);
        }

        public void RegisterMacro(IMacro macro) {
            registeredMacros.Add(macro.Key(), macro.Value);
        }

        public List<Type> MacroDomains => macroDomains;
        public Dictionary<String, Func<String>> RegisteredMacros => registeredMacros;
    }
}