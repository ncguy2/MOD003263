using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Template.macro {

    [AttributeUsage(AttributeTargets.Method)]
    public class Macro : Attribute {

        public readonly String key;
        public Macro(string key) {
            this.key = key;
        }
    }

    public class AttrMacro : IMacro {

        private String key;
        private Func<string> val;

        public AttrMacro(string key, Func<String> val)
        {
            this.key = key;
            this.val = val;
        }

        public string Key() { return this.key; }

        public string Value() {
            return this.val.Invoke();
        }
    }

    public class MacroFinder {
        public static void FindAndRegister() {
            foreach (Type domain in MacroManager.Instance().MacroDomains) {
                foreach (MethodInfo info in FindInClass(domain)) {
                    Macro macroAttr = info.GetCustomAttribute(typeof(Macro)) as Macro;
                    if (macroAttr != null)
                        MacroManager.Instance().RegisterMacro(new AttrMacro(macroAttr.key, () => info.Invoke(null, null).ToString()));
                }
            }
        }

        public static IEnumerable<MethodInfo> FindInClass(Type cls) {
            return cls.GetMethods().Where(IsValidMethod);
        }

        public static bool IsValidMethod(MethodInfo info) {
            if (info.GetCustomAttributes(typeof(Macro), true).Length <= 0) return false;
            if (info.ReturnType != typeof(String)) return false;
            if (!info.IsStatic) return false;
            if (info.GetParameters().Length > 0) return false;
            return true;
        }

    }

}