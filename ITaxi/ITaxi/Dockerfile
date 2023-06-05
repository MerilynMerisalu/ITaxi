FROM  mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app
#EXPOSE 80

#diable buildkit
# export DOCKER_BUILDKIT=0
# export COMPOSE_DOCKER_CLI_BUILD=0

# copy csproj and restore as distinct layers
COPY *.props .
COPY *.sln .

# copy over all the projects
COPY App.BLL/*.csproj ./App.BLL/
COPY App.BLL.DTO/*.csproj ./App.BLL.DTO/
COPY App.Contracts.BLL/*.csproj ./App.Contracts.BLL/
COPY App.Contracts.DAL/*.csproj ./App.Contracts.DAL/
COPY App.DAL.DTO/*.csproj ./App.DAL.DTO/
COPY App.DAL.EF/*.csproj ./App.DAL.EF/
COPY App.Domain/*.csproj ./App.Domain/
COPY App.Enum/*.csproj ./App.Enum/
COPY App.Public.DTO/*.csproj ./App.Public.DTO/
COPY App.Resources/*.csproj ./App.Resources/
COPY Base.BLL/*.csproj ./Base.BLL/
COPY Base.Contracts/*.csproj ./Base.Contracts/
COPY Base.Contracts.BLL/*.csproj ./Base.Contracts.BLL/
COPY Base.Contracts.DAL/*.csproj ./Base.Contracts.DAL/
COPY Base.Contracts.Domain/*.csproj ./Base.Contracts.Domain/
COPY Base.DAL/*.csproj ./Base.DAL/
COPY Base.DAL.EF/*.csproj ./Base.DAL.EF/
COPY Base.Domain/*.csproj ./Base.Domain/
COPY Base.Extensions/*.csproj ./Base.Extensions/
COPY Base.Resources/*.csproj ./Base.Resources/
COPY WebApp.Helpers/*.csproj ./WebApp.Helpers/
COPY WebApp/*.csproj ./WebApp/

# restore the layers
RUN dotnet restore

# Copy everything else into the build
COPY App.BLL/. ./App.BLL/
COPY App.BLL.DTO/. ./App.BLL.DTO/
COPY App.Contracts.BLL/. ./App.Contracts.BLL/
COPY App.Contracts.DAL/. ./App.Contracts.DAL/
COPY App.DAL.DTO/. ./App.DAL.DTO/
COPY App.DAL.EF/. ./App.DAL.EF/
COPY App.Domain/. ./App.Domain/
COPY App.Enum/. ./App.Enum/
COPY App.Public.DTO/. ./App.Public.DTO/
COPY App.Resources/. ./App.Resources/
COPY Base.BLL/. ./Base.BLL/
COPY Base.Contracts/. ./Base.Contracts/
COPY Base.Contracts.BLL/. ./Base.Contracts.BLL/
COPY Base.Contracts.DAL/. ./Base.Contracts.DAL/
COPY Base.Contracts.Domain/. ./Base.Contracts.Domain/
COPY Base.DAL/. ./Base.DAL/
COPY Base.DAL.EF/. ./Base.DAL.EF/
COPY Base.Domain/. ./Base.Domain/
COPY Base.Extensions/. ./Base.Extensions/
COPY Base.Resources/. ./Base.Resources/
COPY WebApp.Helpers/. ./WebApp.Helpers/
COPY WebApp/. ./WebApp/

WORKDIR /src/WebApp
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM  mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /app
#EXPOSE 80

COPY --from=build /app/WebApp/out ./
ENTRYPOINT ["dotnet", "WebApp.dll"]