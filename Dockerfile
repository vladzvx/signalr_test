#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

# установка окружения и пакетов Nuget
ARG ASPNETCORE_ENVIRONMENT
RUN set ASPNETCORE_ENVIRONMENT $ASPNETCORE_ENVIRONMENT
COPY ./nuget.config /root/.nuget/NuGet/NuGet.Config

COPY . .
RUN ls
RUN dotnet restore "InterviewerService/InterviewerSevice.csproj"
COPY . .
WORKDIR "/src/InterviewerService"
RUN dotnet build "InterviewerSevice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InterviewerSevice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN ls
ENTRYPOINT ["dotnet", "InterviewerSevice.dll"]