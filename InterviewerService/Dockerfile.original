#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src

COPY ["InterviewerService/InterviewerSevice.csproj", "InterviewerService/"]
COPY ["IASK.InterviewerEngine/IASK.InterviewerEngine.csproj", "IASK.InterviewerEngine/"]
COPY ["ETIntegration/ETIntegration.csproj", "ETIntegration/"]
COPY ["Common/Common.csproj", "Common/"]
RUN dotnet restore "InterviewerService/InterviewerSevice.csproj"
COPY . .
WORKDIR "/src/InterviewerService"
RUN dotnet build "InterviewerSevice.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InterviewerSevice.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InterviewerSevice.dll"]