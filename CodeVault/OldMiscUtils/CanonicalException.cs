using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OldMiscUtils
{
    /// <summary>
    /// This is not a functional class. It simply demonstrates the canonical form
    /// of an application-specific exception. Copy and paste this class for your
    /// new exception types and rename. Code Analysis will then shut up.
    /// </summary>
    [Serializable]
    public class CanonicalException : Exception
    {
        /// <summary>
        /// Construct a new exception.
        /// </summary>
        public CanonicalException()
        {
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="message">Message to use.</param>
        public CanonicalException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Construct a new exception.
        /// </summary>
        /// <param name="message">Message to use.</param>
        /// <param name="innerException">Inner exception.</param>
        public CanonicalException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Construct a new exception using a serialization context.
        /// </summary>
        /// <param name="info">Serialization info.</param>
        /// <param name="context">Streaing context.</param>
        protected CanonicalException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
