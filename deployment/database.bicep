targetScope = 'resourceGroup'

param adminGroupPrincipalId string
param tenantId string
@secure()
param databasePassword string

resource sqlServer 'Microsoft.Sql/servers@2022-02-01-preview' = {
  name: 'sql-hms-cloud'
  location: resourceGroup().location
  tags: resourceGroup().tags
  properties: {
    administratorLogin: 'hms_admin'
    administratorLoginPassword: databasePassword
    administrators: {
      azureADOnlyAuthentication: false
      principalType: 'Group'
      login: 'Database Admins'
      sid: adminGroupPrincipalId
      tenantId: tenantId
    }
  }
  identity: {
    type: 'SystemAssigned'
  }
}

resource sqlDatabase 'Microsoft.Sql/servers/databases@2023-05-01-preview' = {
  name: 'sqldb-hms-cloud'
  parent: sqlServer
  location: resourceGroup().location
  sku: {
    name: 'Free'
    tier: 'Free'
    capacity: 5
  }
  properties: {
    collation: 'SQL_Latin1_General_CP1_CI_AS'
  }
}

resource sql_allow_azure_services 'Microsoft.Sql/servers/firewallRules@2020-11-01-preview' = {
  name: 'AllowAllAzureIps'
  parent: sqlServer
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}
