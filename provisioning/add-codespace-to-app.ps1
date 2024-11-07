param (
    [Parameter(Mandatory=$true)]
    [string]$Codespace,
    [Parameter(Mandatory=$true)]
    [string]$AzureCredentialsJson
)

$azureCredentials = ConvertFrom-Json $AzureCredentialsJson
az login --allow-no-subscriptions --service-principal -u $($azureCredentials.clientId) -p $($azureCredentials.clientSecret) --tenant $($azureCredentials.tenantId)

$appId="f1d141cf-c7fe-442d-93ec-53a72a416d8a"
$urls=az ad app show --id $appId --query "[web.redirectUris]" | ConvertFrom-Json

$Codespace = $Codespace.Trim('"')

foreach ($url in $urls) {
    if ($url -like "*$Codespace*") {
        Write-Output "Codespace already added to app"
        return
    }
}

$urls += "https://$Codespace-7219.app.github.dev/signin-oidc"
az ad app update --id $appId --web-redirect-uris $urls
