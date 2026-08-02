# SaveManager

This document explains how **Farlands**' save system works and how **FarlandsCoreMod** modifies it.

## Analysis

First, we will analyze the inner workings of **Farlands** by examining a class diagram of the main involved components:

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

`DataManager` controls data loading and storing in **Farlands**, featuring two primary interfaces: `ISave` and `IData`. `ISave` is responsible for saving the data, whereas `IData` represents the data structure itself.

### Settings
Although there is a `JanduSoftSettingsData` it's not used for some cases.
- `Language`: `PlayerPrefs("Language")` in functions 
    - `PixelCrushers.UILocationManager.Initialize()`
    - `PixelCrushers.UILocationManager.UpdateUIs(...)`
- `Audio`: This is using the `JanduSoftSettingsData`

## Mods

### Alternative SaveFile

To prevent issues with the original game save file, its save location has been changed.
By default, the original location is `C:\users\USERNAME\AppData\LocalLow\JanduSoft\Farlands\gamedata.dat`, the modified location is `C:\users\USERNAME\AppData\LocalLow\JanduSoft\Farlands\gamedata.fcm.dat`
