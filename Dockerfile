FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["CourseMapping.Web/CourseMapping.Web.csproj", "CourseMapping.Web/"]
COPY ["CourseMapping.Infrastructure/CourseMapping.Infrastructure.csproj", "CourseMapping.Infrastructure/"]
COPY ["CourseMapping.Domain.Tests.UnitTests/CourseMapping.Domain.Tests.UnitTests.csproj", "CourseMapping.Domain.Tests.UnitTests/"]
COPY ["CourseMapping.Tests.ComponentTests/CourseMapping.Tests.ComponentTests.csproj", "CourseMapping.Tests.ComponentTests/"]
COPY ["CourseMapping.Tests.IntegrationTests/CourseMapping.Tests.IntegrationTests.csproj", "CourseMapping.Tests.IntegrationTests/"]
RUN dotnet restore "CourseMapping.Web/CourseMapping.Web.csproj"
COPY . .
WORKDIR "/src/CourseMapping.Web"
RUN dotnet build "CourseMapping.Web.csproj" -c Release -o /app/build

# Unit tests
FROM build AS test-unit
WORKDIR /src
RUN dotnet test CourseMapping.Domain.Tests.UnitTests/CourseMapping.Domain.Tests.UnitTests.csproj --no-restore --verbosity normal --logger "trx;LogFileName=test-results.trx" --results-directory /src/test-results/unit

# Component tests
FROM build AS test-component
WORKDIR /src
RUN dotnet test CourseMapping.Tests.ComponentTests/CourseMapping.Tests.ComponentTests.csproj --no-restore --verbosity normal --logger "trx;LogFileName=test-results.trx" --results-directory /src/test-results/component

# Integration tests
FROM build AS test-integration
WORKDIR /src
RUN dotnet test CourseMapping.Tests.IntegrationTests/CourseMapping.Tests.IntegrationTests.csproj --no-restore --verbosity normal --logger "trx;LogFileName=test-results.trx" --results-directory /src/test-results/integration

FROM build AS publish
RUN dotnet publish "CourseMapping.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CourseMapping.Web.dll"]