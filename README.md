# Recipes

Recipes app using Dotnet Core (core functionality), NestJS (identity), and Vue with Typescript (client). Authentication will eventually be handled with Magic Link. The full project will be deployed as a *private* service (invitation only). Please contact me if interested in joining. Hosting this app in the cloud isn't free. I will eventually also attempt to host this as a demo project somehow. If you attempt to run this locally, please make sure to handle the dependencies and fill out the `.env` files listed below as well as the `appsettings.json` file in the server/Core/Api project.

## Dependencies

- Google bucket (image storage)
- Magic Link (authentication)
- Mongodb (identity db)
- Ravendb (core db)
- docker
- kubernetes? (not set up yet)

## Environment

#### Vue .env Sample

```.env
VUE_APP_WEB_API_URL="https://localhost:****/api/"
VUE_APP_IDENTITY_URL="https://localhost:****/api/"
```

#### Identity .env Sample

```.env
MAGIC_KEY=""
JWT_SECRET=""
JWT_ISSUER=""
DATABASE_CONNECTION_STRING=""
```

#### Appsettings.json

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "RavenSettings": {
    "Urls": [ "http://localhost:8080" ],
    "DatabaseName": "***"
  },
  "GoogleBucketName": "****"
}
```

## ToDo

- rewrite frontend auth
- update Core API auth (possibly rewrite recipes API to use mongo?)
- pick a theme and update styling in the front end
- write unit tests for Vue and Core API!!!
- Create CI/CD files
