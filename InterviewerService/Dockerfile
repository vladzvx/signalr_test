#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["InterviewerService/InterviewerSevice.csproj", "InterviewerService/"]
COPY ["IASK.Cases.EMCReader/IASK.Cases.EMCReader.csproj", "IASK.Cases.EMCReader/"]
COPY ["IASK.Common/IASK.Common.csproj", "IASK.Common/"]
COPY ["IASK.InterviewerEngine/IASK.InterviewerEngine.csproj", "IASK.InterviewerEngine/"]
COPY ["IASK.EMC.Core/IASK.EMC.Core.csproj", "IASK.EMC.Core/"]
COPY ["IASK.DataStorage/IASK.DataStorage.csproj", "IASK.DataStorage/"]
COPY ["IASK.EMC.Instruments/IASK.EMC.Instruments.csproj", "IASK.EMC.Instruments/"]
COPY ["IASK.ETIntegration/IASK.ETIntegration.csproj", "IASK.ETIntegration/"]
COPY ["IASK.Cases.Semantic/IASK.Cases.Semantic.csproj", "IASK.Cases.Semantic/"]
COPY ["IASK.Cases.JsonStorage/IASK.Cases.JsonStorage.csproj", "IASK.Cases.JsonStorage/"]
COPY ["IASK.Cases.AnatomicAtlas/IASK.Cases.AnatomicAtlas.csproj", "IASK.Cases.AnatomicAtlas/"]
COPY ["IASK.Cases.ETIntegration/IASK.Cases.ETIntegration.csproj", "IASK.Cases.ETIntegration/"]
COPY ["IASK.Cases.EMCWriter/IASK.Cases.EMCWriter.csproj", "IASK.Cases.EMCWriter/"]
COPY ["IASK.Cases.Checker/IASK.Cases.Checker.csproj", "IASK.Cases.Checker/"]
COPY ["IASK.Cases.UI/IASK.Cases.UI.csproj", "IASK.Cases.UI/"]
COPY ["IASK.Cases.DataHub/IASK.Cases.DataHub.csproj", "IASK.Cases.DataHub/"]
COPY ["IASK.DataHub/IASK.DataHub.csproj", "IASK.DataHub/"]
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