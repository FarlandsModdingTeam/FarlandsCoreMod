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
- [GlobalFunctions](#T-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions')
  - [MOD(tag)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-MOD-System-String- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.MOD(System.String)')
  - [add_command(name,LuaFunc,help)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-add_command-System-String,MoonSharp-Interpreter-DynValue,System-String- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.add_command(System.String,MoonSharp.Interpreter.DynValue,System.String)')
  - [add_credits(amount)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-add_credits-System-Int32- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.add_credits(System.Int32)')
  - [add_item(id,amount)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-add_item-MoonSharp-Interpreter-DynValue,System-Int32- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.add_item(MoonSharp.Interpreter.DynValue,System.Int32)')
  - [add_language(path)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-add_language-System-String- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.add_language(System.String)')
  - [config(section,key,def,description)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-config-System-String,System-String,MoonSharp-Interpreter-DynValue,System-String- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.config(System.String,System.String,MoonSharp.Interpreter.DynValue,System.String)')
  - [create_inventory_item(args)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-create_inventory_item-MoonSharp-Interpreter-DynValue- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.create_inventory_item(MoonSharp.Interpreter.DynValue)')
  - [create_object(name)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-create_object-System-String- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.create_object(System.String)')
  - [create_plant(args)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-create_plant-MoonSharp-Interpreter-DynValue- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.create_plant(MoonSharp.Interpreter.DynValue)')
  - [create_scene(name)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-create_scene-System-String- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.create_scene(System.String)')
  - [execute_command(_comando)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-execute_command-System-String- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.execute_command(System.String)')
  - [find_object(args)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-find_object-System-String,System-String- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.find_object(System.String,System.String)')
  - [get_input(input)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-get_input-System-String- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.get_input(System.String)')
  - [get_object(args)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-get_object-System-Collections-Generic-List{MoonSharp-Interpreter-DynValue}- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.get_object(System.Collections.Generic.List{MoonSharp.Interpreter.DynValue})')
  - [load_scene()](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-load_scene-MoonSharp-Interpreter-DynValue- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.load_scene(MoonSharp.Interpreter.DynValue)')
  - [portrait_override(origin,path)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-portrait_override-System-String,System-String- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.portrait_override(System.String,System.String)')
  - [print(txt)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-print-System-String- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.print(System.String)')
  - [rayCast()](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-rayCast-UnityEngine-Vector3,UnityEngine-Vector3,System-Single,System-Int32,MoonSharp-Interpreter-UserData- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.rayCast(UnityEngine.Vector3,UnityEngine.Vector3,System.Single,System.Int32,MoonSharp.Interpreter.UserData)')
  - [sprite_override(origin,position,path)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-sprite_override-System-String,System-Int32[],System-String- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.sprite_override(System.String,System.Int32[],System.String)')
  - [texture_override(arg0,arg1)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-texture_override-MoonSharp-Interpreter-DynValue,MoonSharp-Interpreter-DynValue- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.texture_override(MoonSharp.Interpreter.DynValue,MoonSharp.Interpreter.DynValue)')
  - [translate_inventory_item(id,name,description)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-translate_inventory_item-System-Int32,System-Collections-Generic-List{MoonSharp-Interpreter-DynValue},System-Collections-Generic-List{MoonSharp-Interpreter-DynValue}- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GlobalFunctions.translate_inventory_item(System.Int32,System.Collections.Generic.List{MoonSharp.Interpreter.DynValue},System.Collections.Generic.List{MoonSharp.Interpreter.DynValue})')
- [LuaFunctions](#T-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions')
  - [AddChildObjects(parent,allObjects)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-AddChildObjects-UnityEngine-Transform,System-Collections-Generic-List{UnityEngine-GameObject}- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.AddChildObjects(UnityEngine.Transform,System.Collections.Generic.List{UnityEngine.GameObject})')
  - [GetAllGameObjectsInScene(scene)](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GetAllGameObjectsInScene-UnityEngine-SceneManagement-Scene- 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.GetAllGameObjectsInScene(UnityEngine.SceneManagement.Scene)')
  - [mathsFuncions()](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-mathsFuncions 'FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions.mathsFuncions')
- [LuaGameObjectComponent](#T-FarlandsCoreMod-FarlandsLua-Functions-LuaGameObjectComponent 'FarlandsCoreMod.FarlandsLua.Functions.LuaGameObjectComponent')
  - [Result](#P-FarlandsCoreMod-FarlandsLua-Functions-LuaGameObjectComponent-Result 'FarlandsCoreMod.FarlandsLua.Functions.LuaGameObjectComponent.Result')
  - [StartFunction](#P-FarlandsCoreMod-FarlandsLua-Functions-LuaGameObjectComponent-StartFunction 'FarlandsCoreMod.FarlandsLua.Functions.LuaGameObjectComponent.StartFunction')
  - [UpdateFunction](#P-FarlandsCoreMod-FarlandsLua-Functions-LuaGameObjectComponent-UpdateFunction 'FarlandsCoreMod.FarlandsLua.Functions.LuaGameObjectComponent.UpdateFunction')
  - [Start()](#M-FarlandsCoreMod-FarlandsLua-Functions-LuaGameObjectComponent-Start 'FarlandsCoreMod.FarlandsLua.Functions.LuaGameObjectComponent.Start')
- [LuaManager](#T-FarlandsCoreMod-FarlandsLua-LuaManager 'FarlandsCoreMod.FarlandsLua.LuaManager')
  - [EasyMods](#F-FarlandsCoreMod-FarlandsLua-LuaManager-EasyMods 'FarlandsCoreMod.FarlandsLua.LuaManager.EasyMods')
  - [LUA](#F-FarlandsCoreMod-FarlandsLua-LuaManager-LUA 'FarlandsCoreMod.FarlandsLua.LuaManager.LUA')
  - [OnEvents](#F-FarlandsCoreMod-FarlandsLua-LuaManager-OnEvents 'FarlandsCoreMod.FarlandsLua.LuaManager.OnEvents')
  - [UnityDebug](#F-FarlandsCoreMod-FarlandsLua-LuaManager-UnityDebug 'FarlandsCoreMod.FarlandsLua.LuaManager.UnityDebug')
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

Es el identificador del mod, ejemplo MOD("francoPeta")

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

<a name='T-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions'></a>
## GlobalFunctions `type`

##### Namespace

FarlandsCoreMod.FarlandsLua.Functions.LuaFunctions

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-MOD-System-String-'></a>
### MOD(tag) `method`

##### Summary

TODO: hacer

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| tag | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-add_command-System-String,MoonSharp-Interpreter-DynValue,System-String-'></a>
### add_command(name,LuaFunc,help) `method`

##### Summary

añade un comando a la terminal  
> sufrimiento TODO

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | nombre del comando |
| LuaFunc | [MoonSharp.Interpreter.DynValue](#T-MoonSharp-Interpreter-DynValue 'MoonSharp.Interpreter.DynValue') | funcion lua |
| help | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | texto del help de help |

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-add_credits-System-Int32-'></a>
### add_credits(amount) `method`

##### Summary

Añade los creidtos especificados

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| amount | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Cantidad añadir |

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-add_item-MoonSharp-Interpreter-DynValue,System-Int32-'></a>
### add_item(id,amount) `method`

##### Summary

> si el id es un numero da el objeto por el id  
> si ex 0x es un hexadecimal  
> si es un string busca el objeto por el nombre

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| id | [MoonSharp.Interpreter.DynValue](#T-MoonSharp-Interpreter-DynValue 'MoonSharp.Interpreter.DynValue') |  |
| amount | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-add_language-System-String-'></a>
### add_language(path) `method`

##### Summary

Se llama add_language, ¿Que cres que va hah hacer?

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | direccion del json |

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-config-System-String,System-String,MoonSharp-Interpreter-DynValue,System-String-'></a>
### config(section,key,def,description) `method`

##### Summary

descrcion basica TODO: hacer luego

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| section | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | La seccion de la configuracion, puede haber varias |
| key | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | codigo de la configuracion |
| def | [MoonSharp.Interpreter.DynValue](#T-MoonSharp-Interpreter-DynValue 'MoonSharp.Interpreter.DynValue') | valor de la configuracion |
| description | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | descripcion de ella |

##### Example

> _mod_.config = guarda las secciones que haya  
> _mod_.config guarda la configuracion del mod  
> if -> si la configuracion es un booleano  
>   agremaos la entra a la configuracion  
>   Cojes el mod en LUA  
>   Cojer la sepcion  
>   Cojer la clave que devulva el valor

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-create_inventory_item-MoonSharp-Interpreter-DynValue-'></a>
### create_inventory_item(args) `method`

##### Summary

Añade a la lista un objeto de inventario  
> Crea un nuevo objeto de inventario  
> Si

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| args | [MoonSharp.Interpreter.DynValue](#T-MoonSharp-Interpreter-DynValue 'MoonSharp.Interpreter.DynValue') | name->string, type->string, sprite->string, buy_price->int, sell_price->int, stackeable->bool, matter_percent->float |

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-create_object-System-String-'></a>
### create_object(name) `method`

##### Summary

Crea un nuevo gameObject con el nombre especificado

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | nombre del gameObject a crear |

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-create_plant-MoonSharp-Interpreter-DynValue-'></a>
### create_plant(args) `method`

##### Summary

agrega a una tabla un objeto de planta  
> algomas  
> genera la planta  
>   
> agrega la planta  
> devuelve el id

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| args | [MoonSharp.Interpreter.DynValue](#T-MoonSharp-Interpreter-DynValue 'MoonSharp.Interpreter.DynValue') | name->string, days_for_death->int, days_for_stage->int, grow_season->int, resources->table->(item->int, algomas->X, algomas->X), seed->string, stage_1->string, stage_2->string, stage_3->string, stage_4->string, stage_5->string |

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-create_scene-System-String-'></a>
### create_scene(name) `method`

##### Summary

Crea una escena con el nombre

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | nombre de la escena |

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-execute_command-System-String-'></a>
### execute_command(_comando) `method`

##### Summary

TODO

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| _comando | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Comando a ejecutar |

##### Example

> Si no existe el objeto de comandos  
> Obtiene la lista de comandos  
> Divide el comando en palabras  
> Recorre la lista de comandos  
> Si el comando es igual al _comando  
> Codigo Original :)

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-find_object-System-String,System-String-'></a>
### find_object(args) `method`

##### Summary

Te busca un gameObject en la escena por el nombre (cuidado con objeton con el mismo nombre)

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| args | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | nombre del objeto |

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-get_input-System-String-'></a>
### get_input(input) `method`

##### Summary

Devuelve si as pulsado el boton o tecla especificada

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| input | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Input esperado |

##### Example

> Obtiene el player
> Si el input especificado está pulsado

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-get_object-System-Collections-Generic-List{MoonSharp-Interpreter-DynValue}-'></a>
### get_object(args) `method`

##### Summary

te da un objeto dada la ruta de los objetos

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| args | [System.Collections.Generic.List{MoonSharp.Interpreter.DynValue}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{MoonSharp.Interpreter.DynValue}') | ruta en gameObject |

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-load_scene-MoonSharp-Interpreter-DynValue-'></a>
### load_scene() `method`

##### Summary

Carga la escena especificada por nombre o índice.

##### Parameters

This method has no parameters.

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-portrait_override-System-String,System-String-'></a>
### portrait_override(origin,path) `method`

##### Summary

Es un **texture_override** pero con un personaje

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| origin | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | portrait a reemplazar |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | dirección del archivo |

##### Example

> Crea una función en LUA que se ejecuta cuando se llama a la función  
> La función llama a **texture_override** con los parametros que se le pasan  
> **texture_override** remplaza la textura

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-print-System-String-'></a>
### print(txt) `method`

##### Summary

printea el texto

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| txt | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | lo que le pases lo dibuja por consola |

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-rayCast-UnityEngine-Vector3,UnityEngine-Vector3,System-Single,System-Int32,MoonSharp-Interpreter-UserData-'></a>
### rayCast() `method`

##### Summary

TODO agregar los nuevos

##### Parameters

This method has no parameters.

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-sprite_override-System-String,System-Int32[],System-String-'></a>
### sprite_override(origin,position,path) `method`

##### Summary

En vez de remplaza una region de la textura

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| origin | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | textura que se va a remplazar |
| position | [System.Int32[]](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32[] 'System.Int32[]') | posicion inicial(abajo -> arriba, izquierda -> derecha) |
| path | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | direccion del archivo |

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-texture_override-MoonSharp-Interpreter-DynValue,MoonSharp-Interpreter-DynValue-'></a>
### texture_override(arg0,arg1) `method`

##### Summary

TODO

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| arg0 | [MoonSharp.Interpreter.DynValue](#T-MoonSharp-Interpreter-DynValue 'MoonSharp.Interpreter.DynValue') |  |
| arg1 | [MoonSharp.Interpreter.DynValue](#T-MoonSharp-Interpreter-DynValue 'MoonSharp.Interpreter.DynValue') |  |

##### Example

> Si no existe el objeto de comandos  
> si un parametro, el nombre del archivo es la textura que va ah remplazar (no recomendado)  
> si dos parametros, el primero es el nombre de la textura que va ah remplazar y el segundo es la ruta de la textura

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GlobalFunctions-translate_inventory_item-System-Int32,System-Collections-Generic-List{MoonSharp-Interpreter-DynValue},System-Collections-Generic-List{MoonSharp-Interpreter-DynValue}-'></a>
### translate_inventory_item(id,name,description) `method`

##### Summary

Traducion del objeto de inventario

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| id | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | id del objeto |
| name | [System.Collections.Generic.List{MoonSharp.Interpreter.DynValue}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{MoonSharp.Interpreter.DynValue}') | nombres |
| description | [System.Collections.Generic.List{MoonSharp.Interpreter.DynValue}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{MoonSharp.Interpreter.DynValue}') | descripciones |

<a name='T-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions'></a>
## LuaFunctions `type`

##### Namespace

FarlandsCoreMod.FarlandsLua.Functions

##### Summary

**Clase** donde guardamos las funciones LUA   
> ME CAGO EN MI PUTA VIDA  
\n

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-AddChildObjects-UnityEngine-Transform,System-Collections-Generic-List{UnityEngine-GameObject}-'></a>
### AddChildObjects(parent,allObjects) `method`

##### Summary

MATENME

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| parent | [UnityEngine.Transform](#T-UnityEngine-Transform 'UnityEngine.Transform') |  |
| allObjects | [System.Collections.Generic.List{UnityEngine.GameObject}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Collections.Generic.List 'System.Collections.Generic.List{UnityEngine.GameObject}') |  |

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-GetAllGameObjectsInScene-UnityEngine-SceneManagement-Scene-'></a>
### GetAllGameObjectsInScene(scene) `method`

##### Summary

MATENME

##### Returns

allObjects

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| scene | [UnityEngine.SceneManagement.Scene](#T-UnityEngine-SceneManagement-Scene 'UnityEngine.SceneManagement.Scene') | la escena necesaria |

<a name='M-FarlandsCoreMod-FarlandsLua-Functions-LuaFunctions-mathsFuncions'></a>
### mathsFuncions() `method`

##### Summary

MATENME

##### Parameters

This method has no parameters.

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

<a name='F-FarlandsCoreMod-FarlandsLua-LuaManager-EasyMods'></a>
### EasyMods `constants`

##### Summary

Lista de mods cargados

<a name='F-FarlandsCoreMod-FarlandsLua-LuaManager-LUA'></a>
### LUA `constants`

##### Summary

LUA es un script que se encarga de ejecutar el codigo LUA

<a name='F-FarlandsCoreMod-FarlandsLua-LuaManager-OnEvents'></a>
### OnEvents `constants`

##### Summary

> TODO: ser menos gilipollas

<a name='F-FarlandsCoreMod-FarlandsLua-LuaManager-UnityDebug'></a>
### UnityDebug `constants`

##### Summary

- clase de BepInEx que se encarga de cargar los mods  
- Configuracion que se agrega al BepInEx

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
