# CourseMapping
Course mapping between universities.
This is an application used for students wanting to transfer or do an exchange, allowing them to find the units they have already done or still have to complete, and their equivalents in the university they are moving to.


**Docker Command**
docker run --name coursemapping-postgres -p 6789:5432 -e POSTGRES_USER=jazza -e POSTGRES_PASSWORD=JasGres -d postgres:latest
