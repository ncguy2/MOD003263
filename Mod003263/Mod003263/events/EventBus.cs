
/**
 *  Author: Nick Guy
 *  Date: 24/11/2016
 *  Contains: EventBus
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using Mod003263.threading;
using Expression = System.Linq.Expressions.Expression;

namespace Mod003263.events {

    public class EventBus {

        private static EventBus instance;
        public static EventBus GetInstance() {
            return instance ?? (instance = new EventBus());
        }

        private List<ThreadStart> eventTasks;
        private Dictionary<Type, List<KeyValuePair<Object, Delegate>>>  eventSubscribers;
        private Thread eventThread;
        private bool eventThreadActive;

        private EventBus() {
            this.eventSubscribers = new Dictionary<Type, List<KeyValuePair<object, Delegate>>>();
            this.eventTasks = new List<ThreadStart>();
            this.eventThread = ThreadFactory.GetInstance().CreateManagedThread(ThreadLoop, "Event Bus");
            this.eventThreadActive = true;
            this.eventThread.Start();
        }

        private void PostTask(ThreadStart task) {
            eventTasks.Add(task);
        }

        public void Register(Object subscriber) {
            IEnumerable<MethodInfo> events = EventFinder.FindEvents(subscriber);
            foreach (MethodInfo info in events) {
                if (info.GetParameters().Length != 1) continue;
                Type paramType = info.GetParameters()[0].GetType();
                if (paramType.IsSubclassOf(typeof(AbstractEvent))) {
                    AddSubscriber(paramType, subscriber, info.CreateDelegate(Expression.GetDelegateType(
                    (from param in info.GetParameters() select param.ParameterType)
                    .Concat(new[] { info.ReturnType })
                    .ToArray())));
                }
            }
        }

        public void Post(AbstractEvent e) {
            PostTask(() => {
                Type type = e.GetType();
                if (!eventSubscribers.ContainsKey(type)) return;
                foreach (KeyValuePair<object, Delegate> keyValuePair in eventSubscribers[type])
                    keyValuePair.Value.Method.Invoke(keyValuePair.Key, new object[]{ e });
            });
        }

        private void AddSubscriber(Type evt, Object subscriber, Delegate method) {
            if(!eventSubscribers.ContainsKey(evt))
                eventSubscribers.Add(evt, new List<KeyValuePair<Object, Delegate>>());
            eventSubscribers[evt].Add(new KeyValuePair<Object, Delegate>(subscriber, method));
        }

        private void ThreadLoop() {
            while (eventThreadActive) {
                if (this.eventTasks.Count <= 0)
                    Thread.Sleep(100);
                else {
                    this.eventTasks[0]();
                    this.eventTasks.RemoveAt(0);
                }
            }
        }

    }

}