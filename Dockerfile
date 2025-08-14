FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["CourseMapping.Web/CourseMapping.Web.csproj", "CourseMapping.Web/"]
COPY ["CourseMapping.Infrastructure/CourseMapping.Infrastructure.csproj", "CourseMapping.Infrastructure/"]
RUN dotnet restore "CourseMapping.Web/CourseMapping.Web.csproj"
COPY . .
WORKDIR "/src/CourseMapping.Web"
RUN dotnet build "CourseMapping.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CourseMapping.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CourseMapping.Web.dll"]