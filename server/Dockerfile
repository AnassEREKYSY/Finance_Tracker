# Use a Debian-based .NET image for easier compatibility with mssql-tools
FROM mcr.microsoft.com/dotnet/aspnet:8.0-debian AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Install dependencies: bash, curl, gnupg, mssql-tools
RUN apt-get update \
    && apt-get install -y bash curl gnupg2 apt-transport-https \
    && curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - \
    && curl https://packages.microsoft.com/config/debian/11/prod.list > /etc/apt/sources.list.d/mssql-release.list \
    && apt-get update \
    && ACCEPT_EULA=Y apt-get install -y mssql-tools \
    && apt-get clean

# Set environment variable to enable globalization
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Build stage for the API
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

# Final image to run the application
FROM base AS final
WORKDIR /app

# Copy the published output from the previous stage
COPY --from=publish /app/publish .

# Copy the wait-for-it script
COPY ./wait-for-it.sh /wait-for-it.sh
RUN chmod +x /wait-for-it.sh

# Copy the init-db.sql for database initialization
COPY ./init-db.sql /init-db.sql

# Entry point for the container: wait for SQL Server to be ready and then start the API
ENTRYPOINT ["/wait-for-it.sh", "sql:1433", "--", "dotnet", "API.dll"]
