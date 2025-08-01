# EMGLoggingManager
Provides a UnityPackage for logging data to CSV and to MySQL databases. For examples of using the LoggingManager, see the included [LoggingExample.cs](https://github.com/med-material/LoggingManager/blob/master/Assets/LoggingManager/LoggingExample.cs). 

This repository is a special fork of LoggingManager that has added an example for recording Myo armband EMG data at 200hz.

### [Download The Unitypackage Here](https://github.com/med-material/LoggingManager/releases/latest)
And don't forget to copy myo.dll in the root folder

## How to Import Into Unity
 1. Download the [Unity Package](https://github.com/med-material/LoggingManager/releases/latest).
 2. Open Unity and go to `Assets -> Import Package -> Custom Package` and press `Import`. Make sure there are no compiling errors, but if there are please [file an issue](https://github.com/med-material/LoggingManager/issues/new/choose).
 3. Create an Empty GameObject that you can call `LoggingManager`
 4. Go to `Add Component` and write `LoggingManager` - Add the LoggingManager script to the game object.
 5. Enable CSV file saving and/or MySQL saving depending on what you will use
 6. Look at LoggingExample.cs for how to use it - CSV files are by default saved to `Documents` unless a file path is specified. Be aware, that if any custom file path is invalid, the CSV saving will fail, so it is safest to leave the filepath blank.

## CSV Logging
CSV Logs are stored by default in your computer's Documents folder. The file path can also be overriden by a custom path, set from Unity (be aware that the custom file path may not be transferable to other computers and fail silently.)  The LoggingManager produces CSV files with semicolon (";") separation by default, making to possible to inspect in spreadsheet software like Excel and Calc. By default LoggingManager will produce a Meta file, along with your log file. 

## Recommended Logging Structures
Users of LoggingManager are free to use any structure of logging as they please - there is no explicit structure imposed.We generally recommend logging to several CSV files and use multiple logging structures, depending on the nature of the data you want to capture. In the subsections below we explain a recommended structure, for others looking to log data. This recommended structure is used by LoggingLoader_R to combine and batch import data automatically (see https://github.com/med-material/LoggingLoader_R)

### Event.CSV
Log your event data to a file, whenever a new event happens in your application. The data in the event file is lightweight, because it is only logged whenever needed and not every x milliseconds. Typically, your event data file should have this structure, that centers around an Event column, with some extra context information around it. From the Event CSV, you get a complete timeline over what happened in your application.
| Timestamp                | Event        | GameState    | EnemiesLeft |
|--------------------------|--------------|--------------|-------------| 
| 2023-11-28 13:28:38.5890 | Game Started | GamePlaying  | 7           |
| 2023-11-28 13:28:43.5423 | Enemy Killed | GamePlaying  | 6           |
| 2023-11-28 13:28:59.5696 | Game Stopped | GameStopped  | 6           |

### Sample.CSV
Along with event-based data, you will want to log continuous data, for example tracking mouse movements or physiological data like player heart beat. By convention we call this log "Sample". Sampled data follows this structure, where every column represents a stream of data, logged synchronously with every other data stream, at a fixed interval (notice how the time between each timestamp is exactly the same):
| Timestamp                | MousePositionX | MousePositionY | PressureSensor |
|--------------------------|----------------|----------------|----------------|
| 2023-11-28 13:28:38.5890 | 0.0534         | 0.943          | 6.243          |
| 2023-11-28 13:28:38.5900 | 0.0678         | 0.844          | 7.942          |
| 2023-11-28 13:28:38.5910 | 0.0773         | 0.854          | 8.925          |


### Meta.CSV
LoggingManager by default will log a meta file for you automatically. The Meta file, is a single-row CSV file, where session-wide data is stored. This is for unchanging variables, for example, player name, session duration, hardware fingerprints and profile information. LoggingManager will not allow more than one row in the meta file and will overwrite values (update them) by default instead. New rows can be added anytime during the session.
| Timestamp                | SessionID                            | PlayerName | SessionDuration |
|--------------------------|--------------------------------------|------------|-----------------|
| 2023-11-28 13:28:38.5890 | 4ce30c1b-293b-4afc-bce1-ef74b5f699e0 | Ben        | 21              |

# Good Logging Practices
 - Log `System.DateTime.Now` (requires `using System;`) (This is automatically done by LoggingManager).
 - Log `Time.Framecount` so you can investigate issue of game lag and verify the timing of events. (This is done automatically by LoggingManager)
 - Log a `ParticipantID` to assign a unique ID to each participant.
 - Log a `TestID` to assign a unique ID to each test. Even if you do one test per participants, sometimes tests are redone, due to e.g. mistakes in the procedure or technological faults.
 - Rather than counting the occurance of something in Unity (aggregating data), log important events every frame. This way we can backtrack what happened. If I/O performance is a problem, then write initially to memory.
  - Log `SceneManager.GetActiveScene()` to log current scene (requires `using UnityEngine.SceneManagement;`).
 - Log `transform.eulerAngles` to log rotation of an object - you don't want to log object rotation, only to find out that you accidentically logged Quaternion coordinates (and forgot the `transform.rotation.w` coordinate).
 - Log `transform.localPosition` in addition to `transform.position`. Generally, `transform.position` will work fine, except if your VR scene calibrates by rotating some parent object. This will leave you with inconsistent coordinates between participants. Log both X, Y and Z coordinates.
 - Version number of what is running somehow. Need to investigate how to do this with Unity.
 - If you log an event which has a start, a duration and and end, make sure you always measure all three. Measuring Duration fx with Time.deltaTime might not be as reliable as measure distance between two System.Datetime.Now timestamps.
 
### For Eye Tracking
  - For Eye Tracking, log `hemi_gaze` in addition to any transformed `world_gaze`. When Unity transforms viewport points to world points, there may be a certain inaccuracy.

## ConnectToMySQL (Optional)
![ConnectToMySQL Dialog](https://raw.githubusercontent.com/med-material/ConnectToMySQL/master/connect-to-mysql-image.png)

The LoggingManager optionally supports sending data to a MySQL database.
When enabled, a dialog will ask for credentials at runtime and save them to disk (and never ask again).
Optionally the credentials can be compiled into binaries for deployment purposes.

Students should get credentials and database via their supervisor.
