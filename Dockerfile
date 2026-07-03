# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
 
# Copy only the project file first so restore can be cached
COPY Blog.API/Blog.API.csproj Blog.API/
RUN dotnet restore Blog.API/Blog.API.csproj
 
# Copy everything else and build
COPY . .
WORKDIR /src/Blog.API
RUN dotnet publish Blog.API.csproj -c Release -o /app/publish /p:UseAppHost=false
 
# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
EXPOSE 8080
 
COPY --from=build /app/publish .
 
ENTRYPOINT ["dotnet", "Blog.API.dll"]
 
 