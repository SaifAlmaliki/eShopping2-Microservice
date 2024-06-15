using MediatR;
namespace Shared.CQRS;

// Declaring a (marker interface) ICommand that extends ICommand<Unit>.
// Unit is a type from the MediatR library that represents a void response, indicating the command does not return any data.
public interface ICommand : ICommand<Unit>
{
}

// Declaring a (generic interface) ICommand with an out parameter TResponse.
// This interface represents a command operation that returns a response of type TResponse.
public interface ICommand<out TResponse> : IRequest<TResponse>
{
}


// File Purpose:
// This file defines two shared ICommand interfaces to be used across the entire microservices architecture.
// - The first ICommand interface (without a generic parameter) is for commands that do not return any data (void commands).
// - The second ICommand interface (with a generic parameter) is for commands that return a response of type TResponse.
// These interfaces standardize the way commands are defined and handled, ensuring consistency and reusability of command definitions across the system.