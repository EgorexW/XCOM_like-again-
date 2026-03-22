using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class OnKeyDownOpenDebug : OnKeyDownTrigger
{
    protected override void Trigger()
    {
        // AI GENERATED

        var path = Application.consoleLogPath;

        if (!File.Exists(path)){
            Debug.LogWarning("Log file not found: " + path);
            return;
        }

        try{
            if (Application.platform == RuntimePlatform.WindowsPlayer ||
                Application.platform == RuntimePlatform.WindowsEditor){
                Process.Start(new ProcessStartInfo(path){ UseShellExecute = true });
            }
            else if (Application.platform == RuntimePlatform.OSXPlayer ||
                     Application.platform == RuntimePlatform.OSXEditor){
                Process.Start("open", "\"" + path + "\""); // Wrap in quotes to handle spaces
            }
            else if (Application.platform == RuntimePlatform.LinuxPlayer ||
                     Application.platform == RuntimePlatform.LinuxEditor){
                Process.Start("xdg-open", "\"" + path + "\""); // Wrap in quotes
            }
        }
        catch (Exception e){
            Debug.LogError("Failed to open log: " + e);
        }
    }
}