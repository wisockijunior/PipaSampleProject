using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using WisockiSampleCompileWindow;

public class BuildForWindows : MonoBehaviour
{
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    public enum targetPipaPlatform { android, windows };


    [MenuItem("PipaCompileSample/Windows Build With Postprocess")]
    public static void BuildGameWindows()
    {
        DoItCompile(targetPipaPlatform.windows);
    }



    public static void BuildGameAndroid()
    {
        DoItCompile(targetPipaPlatform.android);
    }

    
    private static void DoItCompile(targetPipaPlatform targetPlatform)
    {
        string path = null;

        // Get filename.
        //string path = EditorUtility.SaveFolderPanel("Choose Location of Built Game", "", "");

        //string[] levels = new string[] { "Assets/Scenes/SampleScene.unity", "Assets/Scenes/Scene2.unity" };
        string[] levels = new string[] { "Assets/Scenes/SampleScene.unity" };

        BuildTarget target = BuildTarget.StandaloneWindows;

        if (targetPlatform == targetPipaPlatform.android)
        {
            path = "bin/Android";            

            target = BuildTarget.Android;

            CompileWindow.ShowToast("Building for Android");
        }
        else
        if (targetPlatform == targetPipaPlatform.windows)
        {
            path = "bin/Windows";            

            target = BuildTarget.StandaloneWindows;

            CompileWindow.ShowToast("Building for Windows");
        }

        // ensure path exists
        System.IO.Directory.CreateDirectory(path);

        // Build player.
        BuildPipeline.BuildPlayer(levels, path + "/PipaSampleGame.exe", BuildTarget.StandaloneWindows, BuildOptions.None);

        // Copy a file from the project folder to the build folder, alongside the built game.
        FileUtil.CopyFileOrDirectory("Assets/Templates/Readme.txt", path + "Readme.txt");

        // Run the game (Process class from System.Diagnostics).
        Process proc = new Process();
        proc.StartInfo.FileName = path + "/doit_upload.bat";
        proc.Start();
    }

}
