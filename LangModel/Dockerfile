#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LangModel/LangModel.csproj", "LangModel/"]
RUN dotnet restore "LangModel/LangModel.csproj"
COPY . .
WORKDIR "/src/LangModel"
RUN dotnet build "LangModel.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LangModel.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LangModel.dll"]