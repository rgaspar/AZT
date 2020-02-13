param(
 [Parameter(Mandatory=$True)]
 [string]
 $Subscription,

 [Parameter(Mandatory=$True)]
 [string]
 $ResourceGroupName,

 [string]
 $ResourceGroupLocation,

 [string]
 $TemplateFilePath = "azuredeploy.json",

 [string]
 $ParametersFilePath = "parameters.json"
)

# sign in
Write-Host "Logging in...";
Connect-AzAccount; 

# 
# select subscription
Write-Host "Selecting subscription '$Subscription'";
Select-AzSubscription -Subscription $Subscription;
