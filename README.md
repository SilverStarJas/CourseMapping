# CourseMapping
Course mapping between universities.
This is an application used for students wanting to transfer or do an exchange, allowing them to find the units they have already done or still have to complete, and their equivalents in the university they are moving to.


**Docker Command**
1. To pull the latest Docker image for postgres: `docker pull postgres`
2. To run the container, run the following command and configure according to your settings:
`docker run --name coursemapping-postgres -p 6789:5432 -e POSTGRES_USER=<username> -e POSTGRES_PASSWORD=<password> -d postgres:latest`

**Setting up and viewing the database**
1. Download the installer and run it: https://www.pgadmin.org/download/ 
2. Once PgAdmin is launched, register a new server - this can be in the already existing Server Group or a newly created one.
3. In the General tab, the name of the server should be the name of the connection string found in `appsettings.json`.
4. In the Connection tab, fill in the fields according to the defined connection string, then Save.
5. In the `CourseMapping.Infrastructure` directory, run `dotnet ef database update -s ..\CourseMapping.Web\`. This will create the tables needed according to the model(s).
6. In the server group, there should now be a database called 'coursemappingdb'. 

**Work in Progress**
- Matching logic: User inputs all their subjects and it will return matches
  - It won't be necessary to specify a course because it should check every subject of every course in the new university
  - Currently, only direct matches on the name will be returned
- Move the matching logic to the Course controller and create new request/response models