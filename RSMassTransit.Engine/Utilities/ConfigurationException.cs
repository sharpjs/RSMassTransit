// Copyright Jeffrey Sharp
// SPDX-License-Identifier: ISC

namespace RSMassTransit;

internal class ConfigurationException : Exception
{
    public ConfigurationException()
        : base("One or more configuration settings are invalid.") { }

    public ConfigurationException(string message)
        : base(message) { }

    public ConfigurationException(string message, Exception innerException)
        : base(message, innerException) { }

#if !NET8_0_OR_GREATER
    protected ConfigurationException(SerializationInfo info, StreamingContext context)
        : base(info, context) { }
#endif
}
