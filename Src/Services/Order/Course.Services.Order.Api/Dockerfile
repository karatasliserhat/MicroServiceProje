FROM mcr.microsoft.com/dotnet/sdk:6.0 as build
WORKDIR /app
EXPOSE 80
COPY Src/Shared/MicroService.Shareds/*.csproj Src/Shared/MicroService.Shareds/
COPY Src/Services/Order/Course.Service.Order.Application/*.csproj Src/Services/Order/Course.Service.Order.Application/
COPY Src/Services/Order/Course.Service.Order.Infrastructure/*.csproj Src/Services/Order/Course.Service.Order.Infrastructure/
COPY Src/Services/Order/Course.Services.Order.Domain/*.csproj Src/Services/Order/Course.Services.Order.Domain/
COPY Src/Services/Order/Course.Services.Order.Domain.Core/*.csproj Src/Services/Order/Course.Services.Order.Domain.Core/
COPY Src/Services/Order/Course.Services.Order.Api/*.csproj Src/Services/Order/Course.Services.Order.Api/
RUN dotnet restore Src/Services/Order/Course.Services.Order.Api/*.csproj
COPY . ./
RUN dotnet publish Src/Services/Order/Course.Services.Order.Api/*.csproj -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:6.0 as runtime
WORKDIR /app
COPY --from=build /app/out . 
ENTRYPOINT [ "dotnet","Course.Services.Order.Api.dll" ]