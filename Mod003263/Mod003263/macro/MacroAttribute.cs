using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/**
 * Author: Nick Guy
 * Date: 30/11/2016
 * Contains: Macro
 */
namespace Mod003263.macro {
    [AttributeUsage(AttributeTargets.Method)]
    public class MacroAttribute : Attribute {

        public readonly String key;
        public MacroAttribute(string key) {
            this.key = key;
        }
    }

    public class MacroFinder {
        public static void FindAndRegister() {
            foreach (Type domain in MacroManager.Instance().MacroDomains) {
                foreach (MethodInfo info in FindInClass(domain)) {
                    MacroAttribute macroAttr = info.GetCustomAttribute(typeof(MacroAttribute)) as MacroAttribute;
                    if (macroAttr != null)
                        MacroManager.Instance().RegisterMacro(macroAttr.key, () => info.Invoke(null, null)?.ToString());
                }
            }
        }

        public static IEnumerable<MethodInfo> FindInClass(Type cls) {
            return cls.GetMethods().Where(IsValidMethod);
        }

        public static bool IsValidMethod(MethodInfo info) {
            if (info.GetCustomAttributes(typeof(MacroAttribute), true).Length <= 0) return false;
            if (info.ReturnType != typeof(String)) return false;
            if (!info.IsStatic) return false;
            if (info.GetParameters().Length > 0) return false;
            return true;
        }

    }
}