# Recipes

Recipes app using dotnet core and Vue with Typescript. The full project will be deployed as a Private service. Please contact me if interested in joining. Hosting in the cloud isn't free. I will eventually also attempt to host this as a demo project somehow. If you attempt to run this locally, please make sure to create a SendGrid account and set your own user secrets in the Identity project as well as create the .env file in the front end, which looks like the code snippet below. If requested, I'll see if I can't share a screenshot/explanation of my email templates.

## .env Sample

VUE_APP_WEB_API_URL="https://localhost:****/api/"

VUE_APP_IDENTITY_URL="https://localhost:****/api/"

## ToDo:

- pick a theme and update styling in the front end
- write unit tests!!!
- Create docker files
- Create CI/CD files
