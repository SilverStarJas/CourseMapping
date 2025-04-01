# CourseMapping
Course mapping between universities.
This is an application used for students wanting to transfer or do an exchange, allowing them to find the units they have already done or still have to complete, and their equivalents in the university they are moving to.

**TODO:**
- Return response DTO instead of domain for endpoints
- Fix controller to call update methods from Domain object (with nullable fields)
- Retrieve Course and Subject through University - not necessary to have extra methods just for retrieving Course and Subject
- Format and rename tables/columns of database to follow convention
- Grouping configuration: https://learn.microsoft.com/en-us/ef/core/modeling/#grouping-configuration
- Extension methods: Making repo internal and adding services in the Infrastructure layer
