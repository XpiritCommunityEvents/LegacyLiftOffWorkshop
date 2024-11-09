
param (
    [string]$subscriptionId,
    [string]$location,
    [string]$tenantId,
    [string]$databasePassword
)

# Set the subscription context
az account set --subscription $subscriptionId

$adminGroupId = az ad group show --group "Database Admins" --query "id" --output tsv

# Deploy the Bicep template, which deploys the resource group, storage account, database etc. to Azure
az deployment sub create `
    --location $location `
    --template-file $PSScriptRoot/project_hms.bicep `
    --parameters tenantId=$tenantId location="$location" adminGroupPrincipalId=$adminGroupId databasePassword=$databasePassword `
    --verbose

& "$PSScriptRoot/seed_database.ps1" -subscriptionId $subscriptionId -databasePassword $databasePassword
