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
   
## FarlandsTextureMod
FarlandsCoreMod permite la modificación de texturas procedentes de los objetos.
Para ello lee los archivos .zip que se encuentran en la carpeta `BepInEx/plugins/FarlandsTextureMod`

Para reemplazar una textura por otra debe crear un zip con las siguientes carpetas:
- **Inventory**, aquellos que se muestran en el inventario
- **Placeable**, aquellos que se sirven como decoración
- `bajo revisión` **Plant**, las plantas
- **World** , los recursos del mundo
- **Other** , cualquier cosa que no cuadre con el resto. **ES MÁS LENTO QUE EL RESTO**

dentro de estas carpetas estarán los archivos .png con el nombre de la textura que se quiere cambiar. Es importante que sea el nombre de la textura y no el del objeto

## FarlandsDialogueMod
FarlandsCoreMod  permite al usuario editar los diálogos que tiene el juego.
Para ello lee los archivos .source.json que se encuentran en la carpeta `BepInEx/plugins/FarlandsDialogueMod`.
[Repositorio de Traducciones]([https://github.com/MagincyanGames/FarlandsCoreMod/tree/ExampleMod](https://github.com/MagincyanGames/FarlandsDialogueMod/tree/Translation))

## Configuraciones
Para configurar el mod, debes modificar el archivo `BepInEx/config/top.magincian.fcm.cfg`

### General/Debug
- SkipIntro `Boolean`: Si está activado no se reproducirá la introducción del juego
- QuitEarlyAccessScreen `Boolean`: Si está activado se saltará la pantalla del EarlyAccess

### FarlandsDialogueMod
- ExportDialogues `Boolean`: Si está activado se generará el archivo `BepInEx/plugins/FarlandsDialogueMod/export.json` que contiene todos los textos traducibles de Farlands

*Proyecto y documentación en desarrollo*
