using MediatR;
namespace Shared.CQRS
{
    // Declaring a generic interface IQueryHandler which implements IRequestHandler from MediatR.
    // This interface represents a handler for a specific query type TQuery that returns a response of type TResponse.
    public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
        // Adding constraints to the generic types:
        // TQuery must implement the IQuery interface with a response type of TResponse.
        // TResponse must be a non-nullable type.
        where TQuery : IQuery<TResponse>
        where TResponse : notnull
    {
    }
}

// File Purpose:
// This file defines a shared IQueryHandler interface to be used across the entire microservices architecture.
// It standardizes the way queries are handled, ensuring consistency and reusability of query handling logic.
