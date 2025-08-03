# Use the official ASP.NET runtime image
FROM mcr.microsoft.com/dotnet/aspnet:4.8 AS base
WORKDIR /inetpub/wwwroot

# Use SDK image to build the app
FROM mcr.microsoft.com/dotnet/framework/sdk:4.8 AS build
WORKDIR /app

# Copy everything and build
COPY . .

# You may want to build with msbuild if needed
# RUN msbuild Web_LinhKienDienTu.sln /p:Configuration=Release

# Publish content
RUN mkdir /app/publish
RUN xcopy /s /y Web_LinhKienDienTu\bin\Release\* /app/publish\

# Final image
FROM base AS final
WORKDIR /inetpub/wwwroot
COPY --from=build /app/publish .

# Expose port
EXPOSE 80
