var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Catalog_API>("catalog-api");

builder.AddProject<Projects.Discount_Grpc>("discount-grpc");

builder.AddProject<Projects.Basket_API>("basket-api");

builder.AddProject<Projects.Ordering_API>("ordering-api");

builder.Build().Run();
