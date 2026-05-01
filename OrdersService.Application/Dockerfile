FROM mcr.microsoft.com/dotnet/sdk:10.0-alpine AS build

WORKDIR /app

COPY . .

RUN dotnet publish OrdersService.Application/OrdersService.Application.csproj -c Release -o /app/publish --self-contained false

FROM mcr.microsoft.com/dotnet/aspnet:10.0-alpine AS final

EXPOSE 8080
EXPOSE 8081

WORKDIR /app
COPY --from=build /app/publish/* ./

ENTRYPOINT ["dotnet", "OrdersService.Application.dll"]

