# Script off a database to files (every object in a separate file).
# Based on https://www.simple-talk.com/sql/database-administration/automated-script-generation-with-powershell-and-smo/
# See also https://gist.github.com/vincpa/1755925

#param (
#    [string]$Database = $(throw "-Database is required.")
#)


#$ServerName = 'ACCALDEV1'
# Leave these blank to use integrated security.
#$User = 'philip.daniels2'
#$Pwd = 'Alphabet01'

$ServerName = 'ACSQL06\CALNEA'
# Leave these blank to use integrated security.
$User = 'phil.daniels'
$Pwd = 'Calneasql2013'


$Databases = @(
    'C3Admin', 'C3CMA', 'C3Logs', 'C3Tools', 'Email_Processing', 'LAAdmin',
    'LAMailer', 'M3Admin', 'M3AreaGuide', 'M3Errors', 'M3Listings'
    'M3Logs', 'M3Mailer', 'M3SEO', 'M3SPV'
    )


# Root of the path to write the output to.
$OutputDirectory = 'C:\temp\SQL'


# Specify what object types you want.
# See http://technet.microsoft.com/en-us/library/microsoft.sqlserver.management.smo.databaseobjecttypes.aspx
# for valid values.
$WhichObjects = [long] [Microsoft.SqlServer.Management.Smo.DatabaseObjectTypes]::all `
		  -bxor [Microsoft.SqlServer.Management.Smo.DatabaseObjectTypes]::ExtendedStoredProcedure

$WhichObjects = [Microsoft.SqlServer.Management.Smo.DatabaseObjectTypes] "Table, StoredProcedure, Synonym, User, UserDefinedFunction, UserDefinedTableTypes, UserDefinedType, View"


##########################################################################################

function ScriptObjects
    (
    [string]$ScriptDir,
    [string]$ServerName,
    [string]$Database,
    [long]$WhichObjects
    )
{
    $DirectoryToSaveTo = "$ScriptDir\$ServerName\$Database"

    "Scripting objects from $ServerName.$Database to $DirectoryToSaveTo"    
   
    # Load SMO assembly, and if we're running SQL 2008 DLLs load the SMOExtended and SQLWMIManagement libraries
    $v = [System.Reflection.Assembly]::LoadWithPartialName( 'Microsoft.SqlServer.SMO')
    if ((($v.FullName.Split(','))[1].Split('='))[1].Split('.')[0] -ne '9') {
        [System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.SMOExtended') | out-null
    }

    [System.Reflection.Assembly]::LoadWithPartialName('Microsoft.SqlServer.SmoEnum') | out-null
    set-psdebug -strict     # catch a few extra bugs

    $ErrorActionPreference = "stop"
    $SMO = 'Microsoft.SqlServer.Management.Smo'

    if ($User -eq '')
    {
    	# Attach to the server (integrated security mode).
    	$srv = new-object ("$SMO.Server") $ServerName    
    } 
    else
    {
    	# Attach to the server (mixed mode)
    	$Connection = new-object "Microsoft.SqlServer.Management.Common.ServerConnection" 
    	$Connection.ServerInstance = $ServerName
    	$Connection.LoginSecure = $false
    	$Connection.Login = $User 
    	$Connection.Password = $Pwd
    	$srv = new-object ("$SMO.Server") $Connection
    }

    if ($srv.ServerType -eq $null)
    {
       Write-Error "Sorry, but I couldn't find or login to Server '$ServerName'"
       return
    } 

    $scripter = new-object ("$SMO.Scripter") $srv
    $scripter.Options.ToFileOnly = $true
    $scripter.Options.ExtendedProperties = $true
    $scripter.Options.DRIAll = $true
    $scripter.Options.Indexes = $true
    $scripter.Options.Triggers = $true
    $scripter.Options.IncludeHeaders = $false      # Want this off, otherwise the time goes in the script.
    $scripter.Options.Permissions = $true
    $scripter.Options.AnsiFile = $true;		

    # Store them in a datatable
    $d = new-object System.Data.Datatable

    # Get everything except the servicebroker object, the information schema and system views
    $d = $srv.databases[$Database].EnumObjects([long]0x1FFFFFFF -band $WhichObjects) | `
        Where-Object {$_.Schema -ne 'sys'-and $_.Schema -ne "information_schema" -and $_.DatabaseObjectTypes -ne 'ServiceBroker'}
    	
    # Write out each scriptable object as a file in the directory you specify
    
    foreach ($obj in $d)
    {
        
        $SavePath ="$($DirectoryToSaveTo)\$($obj.DatabaseObjectTypes)"
        # create the directory if necessary (SMO doesn't).
        if (!(Test-Path -path $SavePath ))
            {Try { New-Item $SavePath -type directory | out-null }
            Catch [system.exception]{
                Write-Error "error while creating '$SavePath' $obj"
                return
            }
        }
        
        # tell the scripter object where to write it
        $scripter.Options.Filename = "$SavePath\$($obj.name -replace '[\\\/\:\.\?]','-').sql";
        write-host $scripter.Options.Filename
        
        # Create a single element URN array
        $UrnCollection = new-object ('Microsoft.SqlServer.Management.Smo.urnCollection')
        $URNCollection.add($obj.urn)
        
        # and write out the object to the specified file
        $scripter.script($URNCollection)
    }
}




foreach ($Db in $Databases)
{
    ScriptObjects $OutputDirectory $ServerName $Db $WhichObjects
}

write-host "Scripting complete."


