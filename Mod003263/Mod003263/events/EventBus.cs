
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

        private bool threaded = false;
        private List<ThreadStart> eventTasks;
        private Dictionary<Type, List<KeyValuePair<Object, Action<AbstractEvent>>>>  eventSubscribers;
        private Thread eventThread;
        private bool eventThreadActive;

        private EventBus() {
            this.eventSubscribers = new Dictionary<Type, List<KeyValuePair<object, Action<AbstractEvent>>>>();
            this.eventTasks = new List<ThreadStart>();
            if (!threaded) return;
            this.eventThread = ThreadFactory.GetInstance().CreateManagedThread(ThreadLoop, "Event Bus");
            this.eventThreadActive = true;
            this.eventThread.Start();
        }

        private void PostTask(ThreadStart task) {
            if(threaded) eventTasks.Add(task);
            else task();
        }

        public void Register(Object subscriber) {
            IEnumerable<MethodInfo> events = EventFinder.FindEvents(subscriber);
            foreach (MethodInfo info in events) {
                if (info.GetParameters().Length != 1) continue;
                Type paramType = info.GetParameters()[0].ParameterType;
                if (!paramType.IsSubclassOf(typeof(AbstractEvent))) continue;
                Action<AbstractEvent> e = evt => info.Invoke(subscriber, new object[] {evt});
                AddSubscriber(paramType, subscriber, e);
            }
        }

        public void Post(AbstractEvent e) {
            PostTask(() => {
                Type type = e.GetType();
                if (!eventSubscribers.ContainsKey(type)) return;
                foreach (KeyValuePair<object, Action<AbstractEvent>> keyValuePair in eventSubscribers[type])
                    keyValuePair.Value(e);
            });
        }

        private void AddSubscriber(Type evt, Object subscriber, Action<AbstractEvent> method) {
            if(!eventSubscribers.ContainsKey(evt))
                eventSubscribers.Add(evt, new List<KeyValuePair<Object, Action<AbstractEvent>>>());
            eventSubscribers[evt].Add(new KeyValuePair<Object, Action<AbstractEvent>>(subscriber, method));
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