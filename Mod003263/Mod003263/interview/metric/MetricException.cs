using System;
using System.Runtime.Serialization;

/**
 *  Author: Nick Guy
 *  Date: 24/10/2016
 *  Contains: MetricException
 */
namespace Mod003263.interview.metric {
    /// <summary>
    /// A specialist exception to be thrown when an error occures with the metric calculation
    /// </summary>
    public class MetricException : Exception {

        protected MetricException(SerializationInfo info, StreamingContext context) : base(info, context) {}
        
        public MetricException() {}
        public MetricException(string message) : base(message) {}
        public MetricException(string message, Exception innerException) : base(message, innerException) {}

    }
}