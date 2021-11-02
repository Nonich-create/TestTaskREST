FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["S:\.Net\ASP.net\TestTask\TestTask.Core\TestTask.Core.csproj", "/src/"]
RUN dotnet restore "./src/TestTask.Core.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TestTask.Core.csproj" -c Release -o /app/build
