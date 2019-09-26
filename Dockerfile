FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["CompositeKey/CompositeKey.csproj", "CompositeKey/"]
COPY ["CompositeKey.AutoMapper/CompositeKey.AutoMapper.csproj", "CompositeKey.AutoMapper/"]
COPY ["CompositeKey.Domain/CompositeKey.Domain.csproj", "CompositeKey.Domain/"]
COPY ["CompositeKey.Model/CompositeKey.Model.csproj", "CompositeKey.Model/"]
COPY ["CompositeKey.Core/CompositeKey.Core.csproj", "CompositeKey.Core/"]
COPY ["CompositeKey.API/CompositeKey.API.csproj", "CompositeKey.API/"]
COPY ["CompositeKey.Infrastructure/CompositeKey.Infrastructure.csproj", "CompositeKey.Infrastructure/"]
COPY ["CompositeKey.Common/CompositeKey.Common.csproj", "CompositeKey.Common/"]
RUN dotnet restore "CompositeKey/CompositeKey.csproj"
COPY . .
WORKDIR "/src/CompositeKey"
RUN dotnet build "CompositeKey.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CompositeKey.csproj" -c Release -o /app

FROM base AS final
RUN mkdir /keys
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CompositeKey.dll"]