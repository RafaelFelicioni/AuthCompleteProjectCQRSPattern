to run postgres on docker
 docker run -d --name postgres-dev -e POSTGRES_USER=appuser -e POSTGRES_PASSWORD=apppass -e POSTGRES_DB=appserviceproviderdb -p 5432:5432 postgres:16

to run migrations

for a new migration

inside principal folder of solution

dotnet ef migrations add InitialMigration --project .\CleanArchMonolit.Infrastructure\CleanArchMonolit.Infrastructure.csproj --startup-project .\App.WebAPI\App.WebAPI.csproj --context AuthDbContext --output-dir Auth\Data\Migrations

for update

dotnet ef database update --project .\CleanArchMonolit.Infrastructure\CleanArchMonolit.Infrastructure.csproj --startup-project .\App.WebAPI\App.WebAPI.csproj --context AuthDbContext

to create database container in docker
docker run -d --name postgres-dev -e POSTGRES_USER=appuser -e POSTGRES_PASSWORD=apppass -e POSTGRES_DB=appserviceproviderdb -p 5432:5432 postgres:16


command to run postgres in cmd
docker exec -it postgres-dev psql -U appuser -d appserviceproviderdb