using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Thalmic.Myo;
using UnityEngine;

public class LoggingExampleMyoEMG : MonoBehaviour
{
    private LoggingManager loggingManager;

    // Myo Object
    [SerializeField] public ThalmicMyo thalmicMyo;

    void Start()
    {
        // Find the logging Manager in the scene.
        loggingManager = GameObject.Find("Logging").GetComponent<LoggingManager>();

        // Start by telling logging manager to create a new collection of logs
        // and optionally pass the column headers.
        loggingManager.CreateLog("MyoExemple");

        // Add event handlers to the Myo device to receive EMG data.
        thalmicMyo._myo.EmgData += onReceiveData;
    }

    // Event handler for receiving EMG data from the Myo device.
    private void onReceiveData(object sender, EmgDataEventArgs data)
    {
        Dictionary<string, object> emgData = new Dictionary<string, object>() {
                            {"EMG1", data.Emg[0]},
                            {"EMG2", data.Emg[1]},
                            {"EMG3", data.Emg[2]},
                            {"EMG4", data.Emg[3]},
                            {"EMG5", data.Emg[4]},
                            {"EMG6", data.Emg[5]},
                            {"EMG7", data.Emg[6]},
                            {"EMG8", data.Emg[7]},
                        };

        // Time.frameCount (used in LogStore) can only be accessed from the main
        // thread so we use MainThreadDispatcher to enqueue the logging action.
        MainThreadDispatcher.Enqueue(() =>
        {
            loggingManager.Log("MyoExemple", emgData);
        });
    }

    // Write the logs to disk when the application quits.
    void OnApplicationQuit()
    {
        loggingManager.SaveLog("MyoExemple", true);
    }
}
