# FarlandsCoreMod
Farlands Core Mod es un mod para [Farlands](https://store.steampowered.com/app/2252680/Farlands) que pretende simplificar el desarrollo de mods para dicho juego.

## Dependencias
* [BepInEx 6]([https://github.com/BepInEx/BepInEx](https://github.com/BepInEx/BepInEx/releases/download/v6.0.0-pre.1/BepInEx_UnityMono_x64_6.0.0-pre.1.zip))

## Instalación
Estos son los pasos que debes hacer para instalar un mod
1. Buscar la carpeta raíz de Farlands.
2. Extraer [BepInEx 6](https://github.com/BepInEx/BepInEx) en la carpeta raíz del juego.
3. Ejecutar la demo de Farlands.
4. Poner los mods en la carpeta *BepInEx/plugins* dela raíz del juego.
5. Al volver a ejecutar la demo de Farlands el mod ya estará instalado.
   
## FarlandsLua
Para simplificar varios aspectos a la hora de desarrollar mods simples, se está trabajando en agregar soporte para lua.
[Documentación Lua](./Docs/Lua.md)

## Configuraciones
Para configurar el mod, debes modificar el archivo `BepInEx/config/top.magincian.fcm.cfg`.
Aquí mostramos el archivo de configuración por defecto
```ini
## Settings file was created by plugin FarlandsCoreMod v0.1.3
## Plugin GUID: top.magincian.fcm

[Debug]

## If true the intro will be skipped
# Setting type: Boolean
# Default value: false
SkipIntro = true

## If true the Early Access Screen will be removed
# Setting type: Boolean
# Default value: false
QuitEarlyAccessScreen = true

# Setting type: Boolean
# Default value: false
UnityDebug = false

[FarlandsDialogueMod]

## If true, a export file will be created and will save all the dialogues
# Setting type: Boolean
# Default value: false
ExportDialogues = false

[FarlandsItems]

## The first id for mod objects
# Setting type: Int32
# Default value: 2000
FirstID = 2000

```