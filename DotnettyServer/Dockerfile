##See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.
#
#FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
#WORKDIR /app
#EXPOSE 6666
#EXPOSE 80
#
#
#FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#WORKDIR /src
#COPY ["DotnettyServer.csproj", "."]
#RUN dotnet restore "./DotnettyServer.csproj"
#COPY . .
#WORKDIR "/src/."
#RUN dotnet build "DotnettyServer.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "DotnettyServer.csproj" -c Release -o /app/publish /p:UseAppHost=false
#
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "DotnettyServer.dll"]

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app
EXPOSE 6666
COPY . .
ENV ASPNETCORE_ENVIRONMENT=Development
ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
#RUN apt-get update && apt-get install -y vim && apt-get install -y libgdiplus
ENTRYPOINT ["dotnet", "DotnettyServer.dll"]