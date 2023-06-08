FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app
EXPOSE 80
COPY Src/FrontedWeb/Course.Web/*.csproj Src/FrontedWeb/Course.Web/
RUN dotnet restore Src/FrontedWeb/Course.Web/*.csproj
COPY . .
RUN dotnet publish Src/FrontedWeb/Course.Web/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet","Course.Web.dll"]