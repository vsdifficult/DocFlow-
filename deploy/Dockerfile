# Use the SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj files and restore as distinct layers
COPY ["src/DocFlow.Domain/DocFlow.Domain.csproj", "src/DocFlow.Domain/"]
COPY ["src/DocFlow.Application/DocFlow.Application.csproj", "src/DocFlow.Application/"]
COPY ["src/DocFlow.Infrastructure/DocFlow.Infrastructure.csproj", "src/DocFlow.Infrastructure/"]
COPY ["src/DocFlow.Api/DocFlow.Api.csproj", "src/DocFlow.Api/"]
RUN dotnet restore "src/DocFlow.Api/DocFlow.Api.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/app/src/DocFlow.Api"
RUN dotnet publish -c Release -o /app/publish

# Use the ASP.NET runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Create a non-root user
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

ENTRYPOINT ["dotnet", "DocFlow.Api.dll"]
