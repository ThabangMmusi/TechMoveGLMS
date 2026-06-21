FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["TechMoveGLMS.API/TechMoveGLMS.API.csproj", "TechMoveGLMS.API/"]
COPY ["TechMoveGLMS/TechMoveGLMS.csproj", "TechMoveGLMS/"]
RUN dotnet restore "TechMoveGLMS.API/TechMoveGLMS.API.csproj"
COPY . .
WORKDIR "/src/TechMoveGLMS.API"
RUN dotnet build "TechMoveGLMS.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TechMoveGLMS.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
COPY --from=publish /app/publish .
# Ensure the database directory exists
RUN mkdir -p /app/data
ENTRYPOINT ["dotnet", "TechMoveGLMS.API.dll"]
