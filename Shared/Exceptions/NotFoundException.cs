namespace Shared.Exceptions;

// Custom exception class for handling "not found" errors
public class NotFoundException : Exception
{
    // Constructor accepting a custom message
    public NotFoundException(string message) : base(message)
    {
    }

    // Constructor accepting an entity name and key, and constructing a default message
    public NotFoundException(string name, object key)
        : base($"Entity \"{name}\" with key ({key}) was not found.")
    {
    }
}
