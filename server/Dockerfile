# Stage 1: Base image for runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Install necessary tools and clean up (e.g., mssql tools)
RUN apt-get update && \
    apt-get install -y bash curl gnupg2 apt-transport-https && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/debian/11/prod.list > /etc/apt/sources.list.d/mssql-release.list && \
    apt-get update && \
    ACCEPT_EULA=Y apt-get install -y mssql-tools && \
    apt-get autoremove -y && \
    rm -rf /var/lib/apt/lists/*

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Stage 2: Build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files into the container
# Update the paths based on your directory structure
COPY ["server/Finance_Tracker.sln", "server/"]
COPY ["server/API/API.csproj", "server/API/"]
COPY ["server/Core/Core.csproj", "server/Core/"]
COPY ["server/Infrastructure/Infrastructure.csproj", "server/Infrastructure/"]

# Restore dependencies
RUN dotnet restore "server/Finance_Tracker.sln"

# Copy the rest of the source code from the local machine to the container
COPY ./server/ ./server/

# Build the application
WORKDIR "/src/server"
RUN dotnet build "Finance_Tracker.sln" -c Release -o /app/build

# Stage 3: Publish the application
FROM build AS publish
RUN dotnet publish "Finance_Tracker.sln" -c Release -o /app/publish

# Stage 4: Final runtime image (only SDK tools temporarily used)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Install the .NET SDK correctly by adding Microsoft's repository (needed for dotnet-ef)
RUN apt-get update && \
    apt-get install -y curl gnupg2 apt-transport-https && \
    curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add - && \
    curl https://packages.microsoft.com/config/debian/11/prod.list > /etc/apt/sources.list.d/dotnetdev.list && \
    apt-get update && \
    apt-get install -y dotnet-sdk-8.0

# Install dotnet-ef globally
RUN dotnet tool install --global dotnet-ef

# Add the path to the .NET tools to the PATH
ENV PATH="${PATH}:/root/.dotnet/tools"

# Copy the published files from the previous stage
COPY --from=publish /app/publish .

# Copy the wait-for-it script (to wait for SQL Server to be ready)
COPY ./wait-for-it.sh /wait-for-it.sh

# Make sure the script is executable
RUN chmod +x /wait-for-it.sh

# Copy the database initialization script (if required)
COPY ./server/init-db.sql /init-db.sql

# Set the working directory to the correct folder for migrations
WORKDIR /app/server/API

# Run migrations and start the application
CMD /wait-for-it.sh sql:1433 -- dotnet ef database update --project /app/server/API/API.csproj && dotnet API.dll
