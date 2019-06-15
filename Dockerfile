# Stage 1
FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS builder
WORKDIR /source

# caches restore result by copying csproj file separately
#COPY /NuGet.config /source/
COPY /DevePixelSecurer/*.csproj /source/DevePixelSecurer/
COPY /DevePixelSecurer.ConsoleApp/*.csproj /source/DevePixelSecurer.ConsoleApp/
COPY /DevePixelSecurer.Tests/*.csproj /source/DevePixelSecurer.Tests/
COPY /DevePixelSecurer.sln /source/
RUN ls
RUN dotnet restore

# copies the rest of your code
COPY . .
RUN dotnet build --configuration Release
RUN dotnet test --configuration Release ./DevePixelSecurer.Tests/DevePixelSecurer.Tests.csproj
RUN dotnet publish ./DevePixelSecurer.ConsoleApp/DevePixelSecurer.ConsoleApp.csproj --output /app/ --configuration Release

# Stage 2
FROM mcr.microsoft.com/dotnet/core/runtime:2.2-alpine3.9
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "DevePixelSecurer.ConsoleApp.dll"]