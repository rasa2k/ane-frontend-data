FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ane-frontend-data.csproj", "."]
RUN dotnet restore "./ane-frontend-data.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "ane-frontend-data.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ane-frontend-data.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ane-frontend-data.dll"]