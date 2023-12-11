SOLUTION="FunWithFlights"              # Solution name
LOCATION="germanywestcentral"          # Azure location
RESOURCE_GROUP="${SOLUTION,,}rg"       # Resource Group name, e.g. funwithflightsrg
CONTAINER_REGISTRY="${SOLUTION,,}cr"   # Azure Container Registry name, e.g. funwithflightscr
IMAGE_PREFIX="${SOLUTION,,}"           # Container image name prefix, e.g. funwithflights
IDENTITY="${SOLUTION,,}id"             # Azure Managed Identity, e.g. funwithflightsid
ENVIRONMENT="${SOLUTION,,}cae"         # Azure Container Apps Environment name, e.g. funwithflightscae

# Create resource group
az group create --location $LOCATION --name $RESOURCE_GROUP

# Create ACR instance
az acr create --location $LOCATION --name $CONTAINER_REGISTRY --resource-group $RESOURCE_GROUP --sku Basic

# Login to ACR instance & store server URL
az acr login --name $CONTAINER_REGISTRY
loginServer=$(az acr show --name $CONTAINER_REGISTRY --query loginServer --output tsv)

# Create the container apps environment
az containerapp env create --name $ENVIRONMENT --resource-group $RESOURCE_GROUP --location $LOCATION

# Publish the projects as container images to ACR
dotnet publish -r linux-x64 -p:PublishProfile=DefaultContainer -p:ContainerRegistry=$loginServer

# Create managed identity
az identity create --name $IDENTITY --resource-group $RESOURCE_GROUP --location $LOCATION
identityId=$(az identity show --name $IDENTITY --resource-group $RESOURCE_GROUP --query id --output tsv)

# These logger configuration values will adjust the apps log format to be better suited for the Azure Container Apps environment
loggerFormat="Logging__Console__FormatterName=json"
loggerSingleLine="Logging__Console__FormatterOptions__SingleLine=true"
loggerIncludeScopes="Logging__Console__FormatterOptions__IncludeScopes=true"

# Create the datasourcesservice
az containerapp create --name datasourcesservice --resource-group $RESOURCE_GROUP --environment $ENVIRONMENT `
    --image $loginServer/$IMAGE_PREFIX-datasourcesservice --ingress internal --target-port 8080 `
    --env-vars $loggerFormat $loggerSingleLine $loggerIncludeScopes --registry-server $loginServer --registry-identity $identityId
	
az containerapp create --name datasourcesdbmanager --resource-group $RESOURCE_GROUP --environment $ENVIRONMENT `
	--image $loginServer/$IMAGE_PREFIX-datasourcesdbmanager --target-port 8080 --ingress internal `
	--env-vars $loggerFormat $loggerSingleLine $loggerIncludeScopes --registry-server $loginServer --registry-identity $identityId

# Create the frontend
az containerapp create --name frontend --resource-group $RESOURCE_GROUP --environment $ENVIRONMENT `
    --image $loginServer/$IMAGE_PREFIX-frontend --target-port 8080 --ingress external `
    --env-vars $loggerFormat $loggerSingleLine $loggerIncludeScopes --registry-server $loginServer --registry-identity $identityId

# Get FQDN of frontend app
az containerapp show --name frontend --resource-group $RESOURCE_GROUP --query properties.configuration.ingress.fqdn

# Create a PostgreSQL service and bind the datasourcesservice to it
az containerapp service postgres create --name postgres --environment $ENVIRONMENT --resource-group $RESOURCE_GROUP
az containerapp update --name datasourcesservice --resource-group $RESOURCE_GROUP --bind postgres
az containerapp update --name datasourcesdbmanager --resource-group $RESOURCE_GROUP --bind postgres

# Create a debug app to use to retrieve injected connection information and bind it to the postgres service
az containerapp create --name bindingdebug --image mcr.microsoft.com/k8se/services/postgres:14 `
    --bind postgres --environment $ENVIRONMENT --resource-group $RESOURCE_GROUP `
    --min-replicas 1 --max-replicas 1 --command "/bin/sleep" "infinity"

# Retrieve PostgreSQL connection string from injected environment variables
az containerapp exec --name bindingdebug --resource-group $RESOURCE_GROUP
#env | grep "^POSTGRES_CONNECTION_STRING"
#exit

# Configure the datasourcesservice app with the postgres connection string
az containerapp update --name datasourcesservice --resource-group $RESOURCE_GROUP --set-env-vars `
    'ConnectionStrings__Aspire.PostgreSQL="Host=postgres;Database=postgres;Username=postgres;Password=AI3U7..."'
az containerapp update --name datasourcesdbmanager --resource-group $RESOURCE_GROUP --set-env-vars `
    'ConnectionStrings__Aspire.PostgreSQL="Host=postgres;Database=postgres;Username=postgres;Password=AI3U7..."'