#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 8001
COPY ["S:\.Net\ASP.net\TestTask\TestTaskREST", "/app"]
RUN dotnet restore "/app/TestTaskREST.csproj"
RUN dotnet build "/app/TestTaskREST.csproj" -c Release -o /app/build
LABEL alpha=0.1
