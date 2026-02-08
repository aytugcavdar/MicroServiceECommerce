FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files
COPY ["src/Services/Payment/Payment.API/Payment.API.csproj", "src/Services/Payment/Payment.API/"]
COPY ["src/Services/Payment/Payment.Application/Payment.Application.csproj", "src/Services/Payment/Payment.Application/"]
COPY ["src/Services/Payment/Payment.Domain/Payment.Domain.csproj", "src/Services/Payment/Payment.Domain/"]
COPY ["src/Services/Payment/Payment.Infrastructure/Payment.Infrastructure.csproj", "src/Services/Payment/Payment.Infrastructure/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Core/BuildingBlocks.Core.csproj", "src/BuildingBlocks/BuildingBlocks.Core/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Infrastructure/BuildingBlocks.Infrastructure.csproj", "src/BuildingBlocks/BuildingBlocks.Infrastructure/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Messaging/BuildingBlocks.Messaging.csproj", "src/BuildingBlocks/BuildingBlocks.Messaging/"]
COPY ["src/BuildingBlocks/BuildingBlocks.CrossCutting/BuildingBlocks.CrossCutting.csproj", "src/BuildingBlocks/BuildingBlocks.CrossCutting/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Logging/BuildingBlocks.Logging.csproj", "src/BuildingBlocks/BuildingBlocks.Logging/"]
COPY ["src/BuildingBlocks/BuildingBlocks.Security/BuildingBlocks.Security.csproj", "src/BuildingBlocks/BuildingBlocks.Security/"]

RUN dotnet restore "src/Services/Payment/Payment.API/Payment.API.csproj"

# Copy source code
COPY . .

WORKDIR "/src/src/Services/Payment/Payment.API"
RUN dotnet build "Payment.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Payment.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Payment.API.dll"]
