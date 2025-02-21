#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GayQuyTuThien/GayQuyTuThien/GayQuyTuThien.csproj", "GayQuyTuThien/"]
RUN dotnet restore "GayQuyTuThien/GayQuyTuThien.csproj"
COPY ["GayQuyTuThien/GayQuyTuThien/", "GayQuyTuThien/"]
COPY ["conf/prod/appsettings.json", "GayQuyTuThien/"]
WORKDIR "/src/GayQuyTuThien"
RUN dotnet build "GayQuyTuThien.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GayQuyTuThien.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ASPNETCORE_HTTP_PORT=8300
ENV ASPNETCORE_URLS=http://+:8300
ENTRYPOINT ["dotnet", "GayQuyTuThien.dll"]