# Build the application
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Install libicu to support globalization
RUN apk add --no-cache icu-libs

# Set environment variable to enable globalization
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY ["server/Finance_Tracker.sln", "server/"]
COPY ["server/API/API.csproj", "server/API/"]
COPY ["server/Core/Core.csproj", "server/Core/"]
COPY ["server/Infrastructure/Infrastructure.csproj", "server/Infrastructure/"]
RUN dotnet restore "server/Finance_Tracker.sln"
COPY . .
WORKDIR "/src/server"
RUN dotnet build "Finance_Tracker.sln" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Finance_Tracker.sln" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
