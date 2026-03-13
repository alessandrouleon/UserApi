namespace UserApin.Exceptions;

/// <summary>
/// Thrown only for truly unexpected domain violations that cannot be expressed
/// as a Result (e.g., programmer errors, illegal state transitions).
/// Normal business-rule errors must use Result&lt;T&gt; instead.
/// </summary>
public sealed class DomainException : Exception
{
    public DomainException(string message) : base(message) { }

    public DomainException(string message, Exception inner) : base(message, inner) { }
}
