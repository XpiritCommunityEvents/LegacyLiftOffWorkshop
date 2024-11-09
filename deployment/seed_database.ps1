
param (
    [string]$subscriptionId,
    [string]$databasePassword
)

function Install-ModuleIfNeeded {
    param (
        [string]$name,
        [string]$version
    )

    $module = Get-Module -ListAvailable -Name $name

    if (-not ($module.Version -ge $version)) {
        Write-Host "Installing Powershell module $name $version"
        Install-Module -Name $name -RequiredVersion $version -AllowClobber -Scope CurrentUser -Force
    }

    Import-Module $name -MinimumVersion $version
}

# Set the subscription context
az account set --subscription $subscriptionId

# Get my public IP address
$publicIpAddress = (Invoke-RestMethod -Uri "http://ipinfo.io/json").ip

# Add the public IP address to the SQL Server firewall rules
$serverName = "sql-hms-cloud"  
az sql server firewall-rule create `
    --resource-group rg-hms-cloud `
    --server $serverName `
    --name AllowMyIP `
    --start-ip-address $publicIpAddress `
    --end-ip-address $publicIpAddress

Install-ModuleIfNeeded "SqlServer" "22.3.0"

Invoke-SqlCmd -ServerInstance sql-hms-cloud.database.windows.net `
    -Database sqldb-hms-cloud `
    -UserName hms_admin `
    -Password $databasePassword `
    -InputFile "$PSScriptRoot/create_schema.sql"

Invoke-SqlCmd -ServerInstance sql-hms-cloud.database.windows.net `
    -Database sqldb-hms-cloud `
    -UserName hms_admin `
    -Password $databasePassword `
    -InputFile "$PSScriptRoot/seed_data.sql"
