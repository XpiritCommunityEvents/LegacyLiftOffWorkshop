# How to deploy

Run the PowerShell script `./deploy_to_azure.ps1` with the correct parameters:

```pwsh
./deploy_to_azure.ps1 -subscriptionId '<your azure subscription>' -location 'East US' -tenantId '<your tenant ID>' -databasePassword '<your database password>'
```

> ⚠️ The script assumes that there is a group in your Entra ID tenant named 'Database Admins', which will be assigned to the SQL Server so they can administer the server. If you don't have that, create one or change the script and Bicep templates accordingly.
