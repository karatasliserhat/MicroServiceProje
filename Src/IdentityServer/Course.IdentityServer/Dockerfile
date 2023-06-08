FROM mcr.microsoft.com/dotnet/sdk:3.1 as build
WORKDIR /app
EXPOSE 80
COPY Src/Shared/MicroService.Shareds/*.csproj Src/Shared/MicroService.Shareds/
COPY Src/IdentityServer/Course.IdentityServer/*.csproj Src/IdentityServer/Course.IdentityServer/
RUN dotnet restore Src/IdentityServer/Course.IdentityServer/*.csproj
COPY . .
RUN dotnet publish Src/IdentityServer/Course.IdentityServer/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:3.1 as runtime
WORKDIR /app
COPY --from=build /app/out .
ENTRYPOINT ["dotnet","Course.IdentityServer.dll"]