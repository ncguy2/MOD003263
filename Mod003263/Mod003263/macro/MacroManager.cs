/**
 * Author: Nick Guy
 * Date: 30/11/2016
 * Contains: MacroManager
 */

using System;
using System.Collections.Generic;

namespace Mod003263.macro {

    public class MacroManager {

        private static MacroManager instance;
        public static MacroManager Instance() {
            return instance ?? (instance = new MacroManager());
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

        public void RegisterMacro(String key, Func<String> macro) {
            registeredMacros.Add(key, macro);
        }

        public List<Type> MacroDomains => macroDomains;
        public Dictionary<String, Func<String>> RegisteredMacros => registeredMacros;
    }
}