using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class BuildVersionUpdater : IPreprocessBuildWithReport
{
    public int callbackOrder => 0;

    private const string initVer = "0.0";

    public void OnPreprocessBuild(BuildReport report)
    {
        string currentVersion = GetCurrentVersion();
        UpdateVersion(currentVersion);
    }

    private string GetCurrentVersion()
    {
        string[] currentVersion = PlayerSettings.bundleVersion.Split('[', ']');

        return currentVersion.Length == 1 ? initVer : currentVersion[1];
    }

    private void UpdateVersion(string version)
    {
        if(float.TryParse(version, out float versionNum))
        {
            float newVer = versionNum + 0.01f;
            string date = DateTime.Now.ToString("d");
            PlayerSettings.bundleVersion = string.Format("V. [{0}] - {1}", newVer, date);
            Debug.Log(PlayerSettings.bundleVersion);
        }
    }
}
