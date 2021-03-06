using System.Collections.Generic;
using Microsoft.AspNetCore.SignalR.Internal;
using Microsoft.AspNetCore.SignalR.Internal.Protocol;

namespace Microsoft.AspNetCore.SignalR.Redis.Internal
{
    public readonly struct RedisInvocation
    {
        /// <summary>
        /// Gets a list of connections that should be excluded from this invocation.
        /// May be null to indicate that no connections are to be excluded.
        /// </summary>
        public IReadOnlyList<string> ExcludedIds { get; }

        /// <summary>
        /// Gets the message serialization cache containing serialized payloads for the message.
        /// </summary>
        public SerializedHubMessage Message { get; }

        public RedisInvocation(SerializedHubMessage message, IReadOnlyList<string> excludedIds)
        {
            Message = message;
            ExcludedIds = excludedIds;
        }

        public static RedisInvocation Create(string target, object[] arguments, IReadOnlyList<string> excludedIds = null)
        {
            return new RedisInvocation(
                new SerializedHubMessage(new InvocationMessage(target, null, arguments)),
                excludedIds);
        }
    }
}
