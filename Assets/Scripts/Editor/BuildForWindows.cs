using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using WisockiSampleCompileWindow;

public class BuildForWindows : MonoBehaviour
{

    /*
     * 3.2. Build script
        Create a batch or shell script that receives the platform and the environment as
        arguments, and outputs the final build. Android and Windows support is enough for the scope of
        this test. You can offer any other arguments you want to increase the script’s flexibility.
     */
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
        
        string arg1 = "targetPlatform";
        string arg2 = "targetEnviroment";

        string basePath = null; // not used here yet
#if test
        // LAN local network - IP fixo reservado: 192.168.25.241
        basePath = @"http://192.168.25.241:8080/";// + assetBundleName;
        basePath = @"http://192.168.25.242:8080/";// + assetBundleName;
        arg2 = "test";
#endif

#if homologation

        // using ddns for homologation

        arg2 = "homologation";
#endif

#if production
        
        // production url
        basePath = @"http://blocksxr.com/";

        arg2 = "production";
#endif

        if (targetPlatform == targetPipaPlatform.android)
        {
            path = "bin/Android";

            arg1 = "android";

            target = BuildTarget.Android;

            CompileWindow.ShowToast("Building for Android");
        }
        else
        if (targetPlatform == targetPipaPlatform.windows)
        {
            path = "bin/Windows";

            arg1 = "windows";

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
        proc.StartInfo.FileName = path + "/doit_upload.bat "+arg1+" "+arg2;
        proc.Start();
    }

}
