using System.IO;
using UnityEditor;
using UnityEngine;

public static class ScreenshotHelper
{
    private const string ScreenshotFolderName = "Game-Screenshots/";

    [MenuItem("Helper/Take screenshot %#D")]
    public static void TakeScreenShot()
    {
        CreateScreenshotFolderIfNeeded();

        // var date = DateTime.Now.ToString().Replace(" ", "_").Replace("/", "-").Replace(":", "-");
        string screenshotBaseFilePath = ScreenshotFolderName + Application.productName + "-" + Screen.width + "x" +
                                     Screen.height;
        string screenshotFinalFilePath = GetIterationNameForFile(screenshotBaseFilePath) + ".png";
        ScreenCapture.CaptureScreenshot(screenshotFinalFilePath);
        Debug.Log(
            "[MWM-Utils] Screenshot located at: " + screenshotFinalFilePath + "\n" +
            "Wait for a few seconds for the screenshots to be written on your disk before calling this method again");
    }

    private static void CreateScreenshotFolderIfNeeded()
    {
        if (!Directory.Exists(ScreenshotFolderName))
        {
            Directory.CreateDirectory(ScreenshotFolderName);
        }
    }

    private static string GetIterationNameForFile(string baseFilePath)
    {
        if (!File.Exists(baseFilePath + ".png")) return baseFilePath;

        int iteration = 1;
        while (File.Exists(baseFilePath + "_" + iteration + ".png"))
        {
            iteration++;
        }

        return baseFilePath + "_" + iteration;
    }
}
