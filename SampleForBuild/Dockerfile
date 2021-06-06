#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

#事先安装镜像：docker pull mcr.microsoft.com/dotnet/aspnet:6.0
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

#事先安装镜像：docker pull mcr.microsoft.com/dotnet/sdk:6.0
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SampleForBuild.csproj", "."]
RUN dotnet restore "./SampleForBuild.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SampleForBuild.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SampleForBuild.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SampleForBuild.dll"]