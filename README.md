# Recipes

Recipes app using Dotnet Core (core functionality), NestJS (identity), and Vue with Typescript (client). Authentication will eventually be handled with Magic Link. The full project will be deployed as a *private* service (invitation only). Please contact me if interested in joining. Hosting this app in the cloud isn't free. I will eventually also attempt to host this as a demo project somehow. If you attempt to run this locally, please make sure to handle the dependencies and fill out the `.env` files listed below as well as the `appsettings.json` file in the server/Core/Api project.

Magic link allows this app to be run completely passwordless. In order to run this, sign up for [Magic link](https://dashboard.magic.link/signup) to create an app. Copy the public key to the client `.env` file and the private key to the identity `.env` file.

## Dependencies

- [Google bucket](https://console.cloud.google.com/storage/browser) (image storage)
- [Magic Link](https://dashboard.magic.link/signup) (authentication)
- [Mongodb](https://www.mongodb.com/cloud/atlas) (identity db)
- [Ravendb](https://ravendb.net/) (core db)
- docker
- kubernetes? (not set up yet)

## Environment

#### Vue .env Sample

```.env
VUE_APP_WEB_API_URL="https://localhost:****/api/"
VUE_APP_IDENTITY_URL="https://localhost:****/api/"
VUE_APP_MAGIC_KEY=pk_*****
```

#### Identity .env Sample

```.env
MAGIC_KEY=sk_****
JWT_SECRET=****
JWT_ISSUER=****
DATABASE_CONNECTION_STRING=****
JWT_ISSUER=****
JWT_AUDIENCE=****
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
    "Urls": [ "http://localhost:****" ],
    "DatabaseName": "Recipes"
  },
  "Auth": {
    "JwtBearerSettings": {
      "SecretKey": "****",
      "Issuer": "****",
      "Audience": "****",
      "ExpiryTimeInSeconds": 60
    }
  },
  "Google": {
    "BucketName": "****" 
  }
}
```

## ToDo

- finish rewriting front-end auth
- update Core API auth (possibly rewrite recipes API to use mongo?)
- pick a theme and update styling in the front end
- write unit tests for Vue and Core API!!!
- Create CI/CD files
