## Generate a migration
```
Add-Migration PUT_MIGRATION_NAME_HERE -Project Consumers\Emails\Core31.Consumers.CreateEmail.Data -StartupProject Consumers\Emails\Core31.Consumers.CreateEmail -OutputDir EntityFramework\Migrations
```

## Update the Database schema
```
Update-Database -Verbose -Project Consumers\Emails\Core31.Consumers.CreateEmail.Data -StartupProject Consumers\Emails\Core31.Consumers.CreateEmail
```
