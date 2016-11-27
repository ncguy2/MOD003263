using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

/**
 *  Author: Nick Guy
 *  Date: 24/11/2016
 *  Contains: EventAttribute, EventFinder
 */
namespace Mod003263.events {

    [AttributeUsage(AttributeTargets.Method)]
    public class EventAttribute : Attribute {}

    public class EventFinder {

        public static IEnumerable<MethodInfo> FindEvents(Object obj) {
            return FindEvents(obj.GetType());
        }
        public static IEnumerable<MethodInfo> FindEvents(Type type) {
            return type.GetMethods().Where(IsValidMethod);
        }

        public static bool IsValidMethod(MethodInfo info) {
            return info.GetCustomAttributes(typeof(EventAttribute), true).Length > 0;
        }

    }

}