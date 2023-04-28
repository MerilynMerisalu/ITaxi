# icd0021-21-22-s

dotnet tool update --global dotnet-ef;dotnet tool update -g dotnet-aspnet-codegenerator

dotnet ef migrations add --project App.DAL.EF --startup-project WebApp Initial
echo y | dotnet ef database drop --project App.DAL.EF --startup-project WebApp
dotnet ef database update --project App.DAL.EF --startup-project WebApp        
dotnet aspnet-codegenerator identity -dc App.DAL.EF.AppDbContext -udui -f

controllers
MVC
dotnet aspnet-codegenerator controller -name FooBarsController -actions -m App.Domain.FooBar -dc AppDbContext -outDir
Areas/Admin/Controllers --useDefaultLayout --useAsyncActions --referenceScriptLibraries -f
Api
dotnet aspnet-codegenerator controller -name FooBarsController -m App.Domain.FooBar -actions -dc AppDbContext -outDir
ApiControllers -api --useAsyncActions -f