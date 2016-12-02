using System;
using System.Collections.Generic;
using System.Threading;

/**
 *  Author: Nick Guy
 *  Date: 22/10/2016
 *  Contains: ThreadFactory, ManagedThreadWrapper
 */
namespace Mod003263.threading {
    /// <summary>
    /// A _thread factory that keeps a reference to each one, allowing for thread pooling and identification
    /// </summary>
    public class ThreadFactory {

        private static ThreadFactory Instance;
        public static ThreadFactory GetInstance() {
            return Instance ?? (Instance = new ThreadFactory());
        }

        private List<ManagedThreadWrapper> managedThreads;

        private ThreadFactory() {
            managedThreads = new List<ManagedThreadWrapper>();
        }

        public Thread CreateManagedDaemonThread(int iterDelay, ThreadStart threadRef) {
            Thread thread = new Thread(threadRef) {IsBackground = true};
            managedThreads.Add(new ManagedThreadWrapper(() => {
                while (true) {
                    threadRef();
                    Thread.Sleep(iterDelay);
                }
            }, thread));
            return thread;
        }

        /// <summary>
        /// Creates a thread with the provided function and stores it in a local collection.
        /// </summary>
        /// <param name="threadRef">The function for the thread to invoke</param>
        /// <returns>The thread instance</returns>
        public Thread CreateManagedThread(ThreadStart threadRef) {
            Thread thread = new Thread(threadRef);
            managedThreads.Add(new ManagedThreadWrapper(threadRef, thread));
            return thread;
        }

        /// <summary>
        /// Creates a thread with the provided function, sets the _thread name, and stores it in a local collection.
        /// </summary>
        /// <param name="threadRef">he function for the thread to invoke</param>
        /// <param name="name">The name for thread identification</param>
        /// <returns>The thread instance</returns>
        public Thread CreateManagedThread(ThreadStart threadRef, String name) {
            Thread thread = new Thread(threadRef) {Name = name};
            managedThreads.Add(new ManagedThreadWrapper(threadRef, thread));
            return thread;
        }

    }

    /// <summary>
    /// An internal struct for use with the <see cref="ThreadFactory"/> class
    /// </summary>
    /// <remarks>
    /// Acting as a class to override native equality methods
    /// </remarks>
    internal class ManagedThreadWrapper {

        public readonly Thread _thread;
        public readonly ThreadStart _threadStart;

        public ManagedThreadWrapper(ThreadStart threadStart, Thread thread) {
            this._threadStart = threadStart;
            this._thread = thread;
        }

        protected bool Equals(ManagedThreadWrapper other) {
            return Equals(_thread, other._thread) && Equals(_threadStart, other._threadStart);
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
                return ((_threadStart != null ? _threadStart.GetHashCode() : 0) * 397) ^ (_thread != null ? _thread.GetHashCode() : 0);
            }
        }
    }
}