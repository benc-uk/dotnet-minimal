# =================================
# Stage 1 - Build and publish
# =================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY minimal.csproj .
COPY *.cs .
COPY Pages/ ./Pages/

RUN dotnet restore minimal.csproj
RUN dotnet publish -c Release -o /dist/ minimal.csproj -p:InvariantGlobalization=true

# =================================
# Stage 2 - Runtime image 
# =================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0-azurelinux3.0-distroless

WORKDIR /app
COPY --from=build /dist/ ./
COPY wwwroot/ ./wwwroot/

# Best practices; run as non-root, and set port explicitly
USER app
ENV ASPNETCORE_HTTP_PORTS=5000

EXPOSE 5000

ENTRYPOINT ["/app/minimal"]