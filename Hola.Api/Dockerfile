FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["./Hola.Core/Hola.Core.csproj", "Hola.Core/"]
RUN dotnet restore "Hola.Core/Hola.Core.csproj"
COPY ["./Hola.Api/Hola.Api.csproj", "Hola.Api/"]
RUN dotnet restore "Hola.Api/Hola.Api.csproj"
COPY . .
WORKDIR "/src/Hola.Api"
RUN dotnet build "Hola.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Hola.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Hola.Api.dll"]
