#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
#COPY ["./webapp.csproj", "webapp/"]
COPY . .
RUN dotnet restore "./webapp.csproj"
#WORKDIR "/src/webapp"
RUN dotnet build "webapp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "webapp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "webapp.dll"]