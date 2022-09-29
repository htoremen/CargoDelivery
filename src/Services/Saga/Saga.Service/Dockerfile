#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/Services/Saga/Saga.Service/Saga.Service.csproj", "src/Services/Saga/Saga.Service/"]
COPY ["src/Core/Core.Application/Core.Application.csproj", "src/Core/Core.Application/"]
COPY ["src/Core/Core.Domain/Core.Domain.csproj", "src/Core/Core.Domain/"]
COPY ["src/Core/Core.Infrastructure/Core.Infrastructure.csproj", "src/Core/Core.Infrastructure/"]
COPY ["src/NoSQLs/NoSQLMongo/NoSQLMongo.Infrastructure/NoSQLMongo.Infrastructure.csproj", "src/NoSQLs/NoSQLMongo/NoSQLMongo.Infrastructure/"]
COPY ["src/NoSQLs/NoSQLMongo/NoSQLMongo.Application/NoSQLMongo.Application.csproj", "src/NoSQLs/NoSQLMongo/NoSQLMongo.Application/"]
COPY ["src/NoSQLs/NoSQLMongo/NoSQLMongo.Domain/NoSQLMongo.Domain.csproj", "src/NoSQLs/NoSQLMongo/NoSQLMongo.Domain/"]
COPY ["src/Services/Saga/Saga.Infrastructure/Saga.Infrastructure.csproj", "src/Services/Saga/Saga.Infrastructure/"]
COPY ["src/Services/Saga/Saga.Application/Saga.Application.csproj", "src/Services/Saga/Saga.Application/"]
COPY ["src/Services/Saga/Saga.Domain/Saga.Domain.csproj", "src/Services/Saga/Saga.Domain/"]
RUN dotnet restore "src/Services/Saga/Saga.Service/Saga.Service.csproj"
COPY . .
WORKDIR "/src/src/Services/Saga/Saga.Service"
RUN dotnet build "Saga.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Saga.Service.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Saga.Service.dll"]