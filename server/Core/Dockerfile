FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app/core
EXPOSE 5000
EXPOSE 5001

RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["Recipes.Core.sln", "./"]
COPY ["./Api/Recipes.Core.Api.csproj", "./Api/"]
COPY ["./Application/Recipes.Core.Application.csproj", "./Application/"]
COPY ["./Domain/Recipes.Core.Domain.csproj", "./Domain/"]
COPY ["./Infrastructure/Recipes.Core.Infrastructure.csproj", "./Infrastructure/"]
RUN dotnet restore
COPY . .
WORKDIR "/src/Api"
RUN dotnet build -c Release -o /app/core/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/core/publish

FROM base AS final
WORKDIR /app/core
COPY --from=publish /app/core/publish .
ENTRYPOINT ["dotnet", "Recipes.Core.Api.dll"]
