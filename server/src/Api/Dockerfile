#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["src/Api/Recipes.WebApi/Recipes.WebApi.csproj", "src/Api/Recipes.WebApi/"]
RUN dotnet restore "src/Api/Recipes.WebApi/Recipes.WebApi.csproj"
COPY . .
WORKDIR "/src/src/Api/Recipes.WebApi"
RUN dotnet build "Recipes.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Recipes.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Recipes.WebApi.dll"]