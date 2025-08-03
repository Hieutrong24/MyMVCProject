# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY *.sln .
COPY Web_LinhKienDienTu/*.csproj ./Web_LinhKienDienTu/
RUN dotnet restore

COPY . .
WORKDIR /app/Web_LinhKienDienTu
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Web_LinhKienDienTu.dll"]
