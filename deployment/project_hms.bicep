targetScope = 'subscription'

param adminGroupPrincipalId string
param tenantId string
param location string
@secure()
param databasePassword string

resource rgHmsCloud 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: 'rg-hms-cloud'
  location: location
  tags: {
    owner: 'Roy Cornelissen, Marcel de Vries'
    purpose: 'Live360 demos'
  }
}

module deploy_db './database.bicep' = {
  name: 'database'
  scope: rgHmsCloud
  params: {
    adminGroupPrincipalId: adminGroupPrincipalId
    tenantId: tenantId
    databasePassword: databasePassword
  }
}

module web_app './web_app.bicep' = {
  name: 'web_app'
  scope: rgHmsCloud
}
