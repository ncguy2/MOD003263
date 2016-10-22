using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Mod003263.threading {
    public class ThreadFactory {

        private static ThreadFactory instance;
        public static ThreadFactory Instance() {
            return instance ?? (instance = new ThreadFactory());
        }

        private List<ManagedThreadWrapper> managedThreads;

        private ThreadFactory() {
            managedThreads = new List<ManagedThreadWrapper>();
        }

        public Thread CreateManagedThread(ThreadStart threadRef) {
            Thread thread = new Thread(threadRef);
            managedThreads.Add(new ManagedThreadWrapper(threadRef, thread));
            return thread;
        }

        public Thread CreateManagedThread(ThreadStart threadRef, String name) {
            Thread thread = new Thread(threadRef) {Name = name};
            managedThreads.Add(new ManagedThreadWrapper(threadRef, thread));
            return thread;
        }

    }

    internal class ManagedThreadWrapper {

        public readonly Thread thread;
        public readonly ThreadStart threadStart;

        public ManagedThreadWrapper(ThreadStart threadStart, Thread thread) {
            this.threadStart = threadStart;
            this.thread = thread;
        }

        protected bool Equals(ManagedThreadWrapper other) {
            return Equals(thread, other.thread) && Equals(threadStart, other.threadStart);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ManagedThreadWrapper) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((threadStart != null ? threadStart.GetHashCode() : 0) * 397) ^ (thread != null ? thread.GetHashCode() : 0);
            }
        }
    }
}