
using MediatR;

namespace Shared.CQRS;

// Declaring a generic interface ICommandHandler (with a single generic parameter) TCommand.
// This interface represents a handler for a specific command type TCommand that does not return any data (void response).
// It extends ICommandHandler<TCommand, Unit>, indicating the command handler returns a Unit (void) response.
public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit>
    // Adding a constraint to the generic type TCommand:
    // TCommand must implement the ICommand<Unit> interface.
    where TCommand : ICommand<Unit>
{
}

// Declaring a generic interface ICommandHandler (with two generic parameters): TCommand and TResponse.
// This interface represents a handler for a specific command type TCommand that returns a response of type TResponse.
// It extends the IRequestHandler interface from MediatR, ensuring it follows the request-response pattern.
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
    // Adding constraints to the generic types:
    // TCommand must implement the ICommand<TResponse> interface.
    // TResponse must be a non-nullable type.
    where TCommand : ICommand<TResponse>
    where TResponse : notnull
{
}


// File Purpose:
// This file defines two shared ICommandHandler interfaces to be used across the entire microservices architecture.
// - The first ICommandHandler interface (with a single generic parameter) is for handling commands that do not return any data (void commands).
// - The second ICommandHandler interface (with two generic parameters) is for handling commands that return a response of type TResponse.
// These interfaces standardize the way commands are handled, ensuring consistency and reusability of command handling logic across the system.