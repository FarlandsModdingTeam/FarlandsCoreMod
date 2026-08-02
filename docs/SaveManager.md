# SaveManager

Este documento explica como funciona el sistema de guardado de **Farlands** y como lo modifica **FarlandsCoreMod**

## Analisis

Primero vamos a estudiar el funcionamiento de **Farlands** y para ello primero vamos a mostrar un diagrama con las principales clases implicadas

```mermaid
classDiagram
    class JanduSoftSaveData {
        <<MonoBehaviour>>
        +JanduSoftAllData allData
    }

    class JanduSoftAllData {
        +JanduSoftSettingsData settings
        +JanduSoftGameData gameData
        +int CURRENT_VERSION_NUMBER
        +int versionSavenumber
    }

    class JanduSoftSettingsData {
        +string[] languages$
        +float generalVolume
        +float musicVolume
        +float soundVolume
        +bool fullscreen
        +int graphics
        +int language
        +bool uiBigMode
        +bool hasCustomResolution
        +int resolutionWidth
        +int resolutionHeight
        +int fullScreenMode
        -bool isDirty
        +Language int
        +FullScreen bool
        +Graphics int
        +GeneralVolume float
        +SoundVolume float
        +MusicVolume float
        +UIBigMode bool
        +AutoDetectLanguage() void
        +Reset() void
        +SetResolution(int width, int height, FullScreenMode mode) void
        +IsDirty() bool
        +ResetDirty() void
    }

    class JanduSoftGameData {
        +JanduSoftSlotData[] slotData
        +int currentSlot
        +Reset() void
        +GetCurrentSlotData() JanduSoftSlotData
        +IsDirty() bool
        +ResetDirty() void
    }

    class JanduSoftSlotData

    namespace JanduSoft {
        class DataManager {
            <<Singleton>>
            +ISave pcSave
            +IData data
        }

        class ISave {
            <<Interface>>
            +Initialize() void
            +LoadFile() void
            +SaveFile() void
            +DeleteData() void
        }

        class PCSave

        class IData {
            <<Interface>>
            +IsDirty() bool
            +Save(ISave saveManager) void
            +Load(object data) void
            +Reset() void
        }
    }

    ISave <|.. PCSave
    IData <|.. JanduSoftSaveData

    JanduSoftSaveData o-- JanduSoftAllData
    JanduSoftAllData o-- JanduSoftSettingsData
    JanduSoftAllData o-- JanduSoftGameData
    JanduSoftGameData "1" o-- "3" JanduSoftSlotData : contains

    DataManager o-- ISave
    DataManager o-- IData
```

`DataManager` es quien controla la carga de datos en **Farlands** y podemos observar que tiene dos interfaces principales `ISave` y `IData`.
`ISave` es la encargada de guardar los datos, mientras que `IData` parece ser la estructura de los datos en sí.
