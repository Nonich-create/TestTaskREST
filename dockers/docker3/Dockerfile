#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 8001


FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TestTask.Core.csproj", "."]
RUN dotnet restore "./TestTask.Core.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TestTask.Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestTask.Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestTask.Core.dll"]


FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TestTask.DAL.csproj", "."]
RUN dotnet restore "./TestTask.DAL.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TestTask.DAL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestTask.DAL.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestTask.DAL.dll"]


FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TestTask.BLL.csproj", "."]
RUN dotnet restore "./TestTask.BLL.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TestTask.BLL.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestTask.BLL.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestTask.BLL.dll"]


FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["TestTaskREST.csproj", "."]
RUN dotnet restore "./TestTaskREST.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "TestTaskREST.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TestTaskREST.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TestTaskREST.dll"]
	

RUN dotnet ef database update 
 
  