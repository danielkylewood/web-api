FROM microsoft/dotnet:2.1.401-sdk AS build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0

# Copy csproj and restore as distinct layers
COPY src/WebApiTemplate.WebApi/WebApiTemplate.WebApi.csproj ./WebApiTemplate.WebApi/
COPY src/WebApiTemplate.Domain/WebApiTemplate.Domain.csproj ./WebApiTemplate.Domain/
RUN dotnet restore WebApiTemplate.WebApi/WebApiTemplate.WebApi.csproj

# Copy everything else and build
COPY ./src .
WORKDIR /WebApiTemplate.WebApi/
RUN dotnet publish -c $BUILDCONFIG -o out /p:Version=$VERSION

# Build runtime image
FROM microsoft/dotnet:2.1.3-aspnetcore-runtime
WORKDIR /app
COPY --from=build WebApiTemplate.WebApi/out .

EXPOSE 5050
CMD ["dotnet", "WebApiTemplate.WebApi.dll"]