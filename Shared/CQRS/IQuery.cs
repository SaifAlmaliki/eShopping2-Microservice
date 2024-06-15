using MediatR;
namespace Shared.CQRS;

// Declaring a (generic interface) IQuery with an out parameter TResponse.
// This interface represents a query operation that returns a response of type TResponse.
// It extends the IRequest interface from MediatR, ensuring it follows the request-response pattern.
public interface IQuery<out TResponse> : IRequest<TResponse>
    // Adding a constraint to the generic type TResponse:
    // TResponse must be a non-nullable type.
    where TResponse : notnull
{
    // No additional members are defined here. This interface inherits members from IRequest.
}



// File Purpose:
// This file defines a shared IQuery interface to be used across the entire microservices architecture.
// It standardizes the way queries are defined, ensuring consistency and reusability of query definitions.
// The IQuery interface enforces that any query must specify a non-nullable response type (TResponse).