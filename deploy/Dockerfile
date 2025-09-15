FROM mcr.microsoft.com/dotnet/sdk:9.0

WORKDIR /app

COPY ["src/DocFlow.Domain/DocFlow.Domain.csproj", "src/DocFlow.Domain/"]
COPY ["src/DocFlow.Application/DocFlow.Application.csproj", "src/DocFlow.Application/"]
COPY ["src/DocFlow.Infrastructure/DocFlow.Infrastructure.csproj", "src/DocFlow.Infrastructure/"]
COPY ["src/DocFlow.Api/DocFlow.Api.csproj", "src/DocFlow.Api/"]

RUN dotnet restore "src/DocFlow.Api/DocFlow.Api.csproj"

COPY . .

WORKDIR /app/src/DocFlow.Api
RUN dotnet build -c Release -o /app/build

RUN dotnet tool install dotnet-ef --tool-path /app/tools
ENV PATH="${PATH}:/app/tools"
ENV HOME=/tmp
ENV DOTNET_CLI_HOME=/tmp

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0
RUN dotnet publish /app/src/DocFlow.Api/DocFlow.Api.csproj -c Release -o /app/publish

WORKDIR /app/publish
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=0
ENTRYPOINT ["dotnet", "DocFlow.Api.dll"]