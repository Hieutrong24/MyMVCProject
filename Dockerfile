# Use base image with Mono & MSBuild that supports WebApplication.targets
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy solution and project
COPY *.sln .
COPY Web_LinhKienDienTu/*.csproj ./Web_LinhKienDienTu/

# Restore dependencies
RUN dotnet restore

# Copy all other files
COPY . .

# Publish project
WORKDIR /app/Web_LinhKienDienTu
RUN msbuild Web_LinhKienDienTu.csproj /p:Configuration=Release /p:DeployOnBuild=true /p:PublishProfile=FolderProfile

# Runtime image (aspnet)
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/Web_LinhKienDienTu/bin/Release/net6.0/publish/ .

ENTRYPOINT ["dotnet", "Web_LinhKienDienTu.dll"]
