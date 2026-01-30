FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy all project files
COPY ["PRN232.FUNewsManagement.API/PRN232.FUNewsManagement.API.csproj", "PRN232.FUNewsManagement.API/"]
COPY ["PRN232.FUNewsManagement.Repo/PRN232.FUNewsManagement.Repo.csproj", "PRN232.FUNewsManagement.Repo/"]
COPY ["PRN232.FUNewsManagement.Services/PRN232.FUNewsManagement.Services.csproj", "PRN232.FUNewsManagement.Services/"]

# Restore dependencies
RUN dotnet restore "PRN232.FUNewsManagement.API/PRN232.FUNewsManagement.API.csproj"

# Copy everything else
COPY . .

# Build
WORKDIR "/src/PRN232.FUNewsManagement.API"
RUN dotnet build "PRN232.FUNewsManagement.API.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "PRN232.FUNewsManagement.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "PRN232.FUNewsManagement.API.dll"]