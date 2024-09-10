FROM mcr.microsoft.com/dotnet/sdk:8.0 as build
WORKDIR /app
EXPOSE 8080

# copy all .csproj files and restore as distinct layers.   Use of the same COPY command
# for every dockerfile in the project to take advantage of docker caching
COPY Paynet.Challenge.sln Paynet.Challenge.sln
COPY src/Paynet.Challenge.Core/Paynet.Challenge.Core.csproj src/Paynet.Challenge.Core/Paynet.Challenge.Core.csproj
COPY src/Paynet.Challenge.Entities/Paynet.Challenge.Entities.csproj src/Paynet.Challenge.Entities/Paynet.Challenge.Entities.csproj
COPY src/Paynet.Challenge.Repository/Paynet.Challenge.Repository.csproj src/Paynet.Challenge.Repository/Paynet.Challenge.Repository.csproj
COPY src/Paynet.Challenge.DataContract/Paynet.Challenge.DataContract.csproj src/Paynet.Challenge.DataContract/Paynet.Challenge.DataContract.csproj
COPY src/Paynet.Challenge.Api/Paynet.Challenge.Api.csproj src/Paynet.Challenge.Api/Paynet.Challenge.Api.csproj
COPY tests/Paynet.Challenge.UnitTests/Paynet.Challenge.UnitTests.csproj tests/Paynet.Challenge.UnitTests/Paynet.Challenge.UnitTests.csproj

# Restore package deps
RUN dotnet restore Paynet.Challenge.sln

# Copy the app folders over
COPY src/Paynet.Challenge.Api src/Paynet.Challenge.Api
COPY src/Paynet.Challenge.Core src/Paynet.Challenge.Core
COPY src/Paynet.Challenge.Entities src/Paynet.Challenge.Entities
COPY src/Paynet.Challenge.Repository src/Paynet.Challenge.Repository
COPY src/Paynet.Challenge.DataContract src/Paynet.Challenge.DataContract
WORKDIR /app/src/Paynet.Challenge.Api
RUN dotnet publish -c Release -o /app/src/out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/src/out .
ENTRYPOINT [ "dotnet", "Paynet.Challenge.Api.dll" ]