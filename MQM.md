<a name='assembly'></a>
# FarlandsCoreMod

## Contents

- [CommandShell](#T-CommandTerminal-CommandShell 'CommandTerminal.CommandShell')
  - [RegisterCommands()](#M-CommandTerminal-CommandShell-RegisterCommands 'CommandTerminal.CommandShell.RegisterCommands')
  - [RunCommand()](#M-CommandTerminal-CommandShell-RunCommand-System-String- 'CommandTerminal.CommandShell.RunCommand(System.String)')
- [Farlands](#T-FarlandsCoreMod-Resources-Farlands 'FarlandsCoreMod.Resources.Farlands')
  - [Culture](#P-FarlandsCoreMod-Resources-Farlands-Culture 'FarlandsCoreMod.Resources.Farlands.Culture')
  - [ResourceManager](#P-FarlandsCoreMod-Resources-Farlands-ResourceManager 'FarlandsCoreMod.Resources.Farlands.ResourceManager')
- [FarlandsEasyMod](#T-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod 'FarlandsCoreMod.FarlandsLua.FarlandsEasyMod')
  - [ConfigFile](#F-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-ConfigFile 'FarlandsCoreMod.FarlandsLua.FarlandsEasyMod.ConfigFile')
  - [Mod](#F-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-Mod 'FarlandsCoreMod.FarlandsLua.FarlandsEasyMod.Mod')
  - [PathValue](#F-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-PathValue 'FarlandsCoreMod.FarlandsLua.FarlandsEasyMod.PathValue')
  - [Tag](#F-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-Tag 'FarlandsCoreMod.FarlandsLua.FarlandsEasyMod.Tag')
  - [Item](#P-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-Item-System-String- 'FarlandsCoreMod.FarlandsLua.FarlandsEasyMod.Item(System.String)')
  - [ExecuteMain()](#M-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-ExecuteMain 'FarlandsCoreMod.FarlandsLua.FarlandsEasyMod.ExecuteMain')
  - [FromZip(zipPath)](#M-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-FromZip-System-String- 'FarlandsCoreMod.FarlandsLua.FarlandsEasyMod.FromZip(System.String)')
  - [GetFilesInFolder(t,folder)](#M-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-GetFilesInFolder-System-String,System-String- 'FarlandsCoreMod.FarlandsLua.FarlandsEasyMod.GetFilesInFolder(System.String,System.String)')
  - [LoadAndAddZip(zipPath)](#M-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-LoadAndAddZip-System-String- 'FarlandsCoreMod.FarlandsLua.FarlandsEasyMod.LoadAndAddZip(System.String)')
  - [LoadFolder(path,acumPath)](#M-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-LoadFolder-System-String,System-String- 'FarlandsCoreMod.FarlandsLua.FarlandsEasyMod.LoadFolder(System.String,System.String)')
  - [LoadZip(zipPath)](#M-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-LoadZip-System-String- 'FarlandsCoreMod.FarlandsLua.FarlandsEasyMod.LoadZip(System.String)')
- [LuaGameObjectComponent](#T-FarlandsCoreMod-FarlandsLua-Functions-LuaGameObjectComponent 'FarlandsCoreMod.FarlandsLua.Functions.LuaGameObjectComponent')
  - [Result](#P-FarlandsCoreMod-FarlandsLua-Functions-LuaGameObjectComponent-Result 'FarlandsCoreMod.FarlandsLua.Functions.LuaGameObjectComponent.Result')
  - [StartFunction](#P-FarlandsCoreMod-FarlandsLua-Functions-LuaGameObjectComponent-StartFunction 'FarlandsCoreMod.FarlandsLua.Functions.LuaGameObjectComponent.StartFunction')
  - [UpdateFunction](#P-FarlandsCoreMod-FarlandsLua-Functions-LuaGameObjectComponent-UpdateFunction 'FarlandsCoreMod.FarlandsLua.Functions.LuaGameObjectComponent.UpdateFunction')
  - [Start()](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaGameObjectComponent-Start 'FarlandsCoreMod.FarlandsLua.Functions.LuaGameObjectComponent.Start')
- [LuaManager](#T-FarlandsCoreMod-FarlandsLua-LuaManager 'FarlandsCoreMod.FarlandsLua.LuaManager')
  - [MOD](#P-FarlandsCoreMod-FarlandsLua-LuaManager-MOD 'FarlandsCoreMod.FarlandsLua.LuaManager.MOD')
  - [ExecuteEvent(ev)](#M-FarlandsCoreMod-FarlandsLua-LuaManager-ExecuteEvent-System-String[]- 'FarlandsCoreMod.FarlandsLua.LuaManager.ExecuteEvent(System.String[])')
  - [GetFilesInMod(path)](#M-FarlandsCoreMod-FarlandsLua-LuaManager-GetFilesInMod-System-String- 'FarlandsCoreMod.FarlandsLua.LuaManager.GetFilesInMod(System.String)')
  - [GetFromMod(path)](#M-FarlandsCoreMod-FarlandsLua-LuaManager-GetFromMod-System-String- 'FarlandsCoreMod.FarlandsLua.LuaManager.GetFromMod(System.String)')
  - [Init()](#M-FarlandsCoreMod-FarlandsLua-LuaManager-Init 'FarlandsCoreMod.FarlandsLua.LuaManager.Init')
- [Resources](#T-FarlandsCoreMod-Properties-Resources 'FarlandsCoreMod.Properties.Resources')
  - [Culture](#P-FarlandsCoreMod-Properties-Resources-Culture 'FarlandsCoreMod.Properties.Resources.Culture')
  - [ResourceManager](#P-FarlandsCoreMod-Properties-Resources-ResourceManager 'FarlandsCoreMod.Properties.Resources.ResourceManager')
  - [Version](#P-FarlandsCoreMod-Properties-Resources-Version 'FarlandsCoreMod.Properties.Resources.Version')
- [SO](#T-FarlandsCoreMod-Attributes-SO 'FarlandsCoreMod.Attributes.SO')
  - [#ctor(sceneName,type)](#M-FarlandsCoreMod-Attributes-SO-#ctor-System-String,System-Type- 'FarlandsCoreMod.Attributes.SO.#ctor(System.String,System.Type)')

<a name='T-CommandTerminal-CommandShell'></a>
## CommandShell `type`

##### Namespace

CommandTerminal

<a name='M-CommandTerminal-CommandShell-RegisterCommands'></a>
### RegisterCommands() `method`

##### Summary

Uses reflection to find all RegisterCommand attributes
and adds them to the commands dictionary.

##### Parameters

This method has no parameters.

<a name='M-CommandTerminal-CommandShell-RunCommand-System-String-'></a>
### RunCommand() `method`

##### Summary

Parses an input line into a command and runs that command.

##### Parameters

This method has no parameters.

<a name='T-FarlandsCoreMod-Resources-Farlands'></a>
## Farlands `type`

##### Namespace

FarlandsCoreMod.Resources

##### Summary

Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.

<a name='P-FarlandsCoreMod-Resources-Farlands-Culture'></a>
### Culture `property`

##### Summary

Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
  búsquedas de recursos mediante esta clase de recurso fuertemente tipado.

<a name='P-FarlandsCoreMod-Resources-Farlands-ResourceManager'></a>
### ResourceManager `property`

##### Summary

Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.

<a name='T-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod'></a>
## FarlandsEasyMod `type`

##### Namespace

FarlandsCoreMod.FarlandsLua

##### Summary

FarlandsEasyMod es el mod en si

<a name='F-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-ConfigFile'></a>
### ConfigFile `constants`

##### Summary

Configuracion del mod

<a name='F-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-Mod'></a>
### Mod `constants`

##### Summary

Informacion del mod en DynValue -> LUA

<a name='F-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-PathValue'></a>
### PathValue `constants`

##### Summary

Diccionario que contiene la ruta y el contenido de los archivos del mod

<a name='F-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-Tag'></a>
### Tag `constants`

##### Summary

Ni puta idea

<a name='P-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-Item-System-String-'></a>
### Item `property`

##### Summary

Getters y Setters de PathValue

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Es una direccion, ¿De que? Ni puta idea |

<a name='M-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-ExecuteMain'></a>
### ExecuteMain() `method`

##### Summary

Ejecuta el archivo main.lua
   y guarda la informacion del mod

##### Parameters

This method has no parameters.

<a name='M-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-FromZip-System-String-'></a>
### FromZip(zipPath) `method`

##### Summary

Carga el mod del zip en en un FarlandsEasyMod
    Abvertencias: CIUDADO

##### Returns

Desvuelve un el mod en FarlandsEasyMod

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| zipPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Direccion donde esta el zip |

<a name='M-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-GetFilesInFolder-System-String,System-String-'></a>
### GetFilesInFolder(t,folder) `method`

##### Summary

Obtiene una lista de archivos en una carpeta específica dentro del diccionario PathValue.

##### Returns

Un array de strings que contiene las rutas de los archivos en la carpeta especificada, con el prefijo t añadido a cada ruta.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| t | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Un prefijo que se añadirá a cada ruta de archivo en el resultado. |
| folder | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | La carpeta dentro de PathValue de la cual se quieren obtener los archivos. |

<a name='M-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-LoadAndAddZip-System-String-'></a>
### LoadAndAddZip(zipPath) `method`

##### Summary

Carga un archivo ZIP, crea una instancia de [FarlandsEasyMod](#T-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod 'FarlandsCoreMod.FarlandsLua.FarlandsEasyMod') y ejecuta el archivo main.lua.
    Abvertencias: CIUDADO

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| zipPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Direccion donde eta el zip |

<a name='M-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-LoadFolder-System-String,System-String-'></a>
### LoadFolder(path,acumPath) `method`

##### Summary

Cargar mod desde carpeta en vez dedde zip
    TODO: comprobar

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Direccin donde esta |
| acumPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Direccion de lo que tengo ni idea |

<a name='M-FarlandsCoreMod-FarlandsLua-FarlandsEasyMod-LoadZip-System-String-'></a>
### LoadZip(zipPath) `method`

##### Summary

carga un archivo zip y lo guarda en el diccionario
    Abertencias: CIUDADO

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| zipPath | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | La direccion del zip |

<a name='T-FarlandsCoreMod-FarlandsLua-Functions-LuaGameObjectComponent'></a>
## LuaGameObjectComponent `type`

##### Namespace

FarlandsCoreMod.FarlandsLua.Functions

##### Summary

Clase de C# que la gracia es permitir ejecutar LUA en un GameObject como si fuera nativo

<a name='P-FarlandsCoreMod-FarlandsLua-Functions-LuaGameObjectComponent-Result'></a>
### Result `property`

##### Summary

Resultado de la ejecucion del script LUA : SUPONGO

<a name='P-FarlandsCoreMod-FarlandsLua-Functions-LuaGameObjectComponent-StartFunction'></a>
### StartFunction `property`

##### Summary

Metodo donde guarda el codigo LUA que se ejecutara con Start de Unity

<a name='P-FarlandsCoreMod-FarlandsLua-Functions-LuaGameObjectComponent-UpdateFunction'></a>
### UpdateFunction `property`

##### Summary

Metodo donde guarda el codigo LUA que se ejecutara con Update de Unity

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaGameObjectComponent-Start'></a>
### Start() `method`

##### Summary

Metodo Start de Unity,

##### Parameters

This method has no parameters.

<a name='T-FarlandsCoreMod-FarlandsLua-LuaManager'></a>
## LuaManager `type`

##### Namespace

FarlandsCoreMod.FarlandsLua

<a name='P-FarlandsCoreMod-FarlandsLua-LuaManager-MOD'></a>
### MOD `property`

##### Summary

name: MOD
especie de getter y setter para la variable global _mod_

<a name='M-FarlandsCoreMod-FarlandsLua-LuaManager-ExecuteEvent-System-String[]-'></a>
### ExecuteEvent(ev) `method`

##### Summary

name: ExecuteEvent
    Ejecuta un evento en todos los mods cargados

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| ev | [System.String[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String[] 'System.String[]') | No se que es |

<a name='M-FarlandsCoreMod-FarlandsLua-LuaManager-GetFilesInMod-System-String-'></a>
### GetFilesInMod(path) `method`

##### Summary

Método para obtener los archivos de un mod

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Direccion del archivo |

<a name='M-FarlandsCoreMod-FarlandsLua-LuaManager-GetFromMod-System-String-'></a>
### GetFromMod(path) `method`

##### Summary

Método para obtener datos de un mod
    ¿cuales datos? Ni puta idea

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |

<a name='M-FarlandsCoreMod-FarlandsLua-LuaManager-Init'></a>
### Init() `method`

##### Summary

Método para inicializar el Manager
    Se inicia al iniciar el juego

##### Parameters

This method has no parameters.

<a name='T-FarlandsCoreMod-Properties-Resources'></a>
## Resources `type`

##### Namespace

FarlandsCoreMod.Properties

##### Summary

Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.

<a name='P-FarlandsCoreMod-Properties-Resources-Culture'></a>
### Culture `property`

##### Summary

Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
  búsquedas de recursos mediante esta clase de recurso fuertemente tipado.

<a name='P-FarlandsCoreMod-Properties-Resources-ResourceManager'></a>
### ResourceManager `property`

##### Summary

Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.

<a name='P-FarlandsCoreMod-Properties-Resources-Version'></a>
### Version `property`

##### Summary

Busca una cadena traducida similar a 0.1.3.

<a name='T-FarlandsCoreMod-Attributes-SO'></a>
## SO `type`

##### Namespace

FarlandsCoreMod.Attributes

<a name='M-FarlandsCoreMod-Attributes-SO-#ctor-System-String,System-Type-'></a>
### #ctor(sceneName,type) `constructor`

##### Summary

Modifica la escena utilizando una clase

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| sceneName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | nombre de la escena |
| type | [System.Type](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Type 'System.Type') | clase a utilizar |
