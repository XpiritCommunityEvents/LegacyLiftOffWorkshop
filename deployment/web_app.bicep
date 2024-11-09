targetScope = 'resourceGroup'

resource asp_hms_cloud 'Microsoft.Web/serverfarms@2021-02-01' = {
  name: 'asp-hms-cloud'
  location: resourceGroup().location
  sku: {
    name: 'F1'
    tier: 'Free'
  }
  kind: 'linux'
  properties: {
    reserved: true
  }
}

resource webApp 'Microsoft.Web/sites@2021-02-01' = {
  name: 'app-hms-cloud'
  location: resourceGroup().location
  properties: {
    serverFarmId: asp_hms_cloud.id
    siteConfig: {
      linuxFxVersion: 'DOTNETCORE|8.0'
    }
  }
}

// When your SKU allows it, you can add a deployment slot
// resource stagingSlot 'Microsoft.Web/sites/slots@2021-02-01' = {
//   parent: webApp
//   name: 'staging'
//   location: resourceGroup().location
//   properties: {
//     serverFarmId: asp_hms_cloud.id
//     siteConfig: {
//       linuxFxVersion: 'DOTNETCORE|8.0'
//     }
//   }
// }
