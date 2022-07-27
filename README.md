# icd0021-21-22-s

dotnet tool update --global dotnet-ef;dotnet tool update -g dotnet-aspnet-codegenerator

dotnet ef migrations add --project App.DAL.EF --startup-project WebApp Initial
echo y | dotnet ef database drop --project App.DAL.EF --startup-project WebApp
dotnet ef database update --project App.DAL.EF --startup-project WebApp        
