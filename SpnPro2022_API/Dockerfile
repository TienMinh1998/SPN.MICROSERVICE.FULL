#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SpnPro2022_API/SpnPro2022_API.csproj", "SpnPro2022_API/"]
RUN dotnet restore "SpnPro2022_API/SpnPro2022_API.csproj"
COPY . .
WORKDIR "/src/SpnPro2022_API"
RUN dotnet build "SpnPro2022_API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SpnPro2022_API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SpnPro2022_API.dll"]