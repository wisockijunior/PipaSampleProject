using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using WisockiSampleCompileWindow;
using System.IO;

public class BuildForWindows : MonoBehaviour
{

    /*
     * 3.2. Build script
        Create a batch or shell script 
        that receives the platform and the environment as arguments, 
        and outputs the final build. 
        Android and Windows support is enough for the scope of this test. 
        You can offer any other arguments you want to increase the script’s flexibility.
     */
    public enum targetPipaPlatform { android, windows };


    [MenuItem("PipaCompileSample/Build for Windows")]
    public static void Build_Windows()
    {
        bookmark.step_3_2 build_for_windows;
        DoItCompile(targetPipaPlatform.windows);
    }


    [MenuItem("PipaCompileSample/Build for Android")]
    public static void Build_Android()
    {
        bookmark.step_3_2 build_for_android;

        DoItCompile(targetPipaPlatform.android);
    }

    
    private static void DoItCompile(targetPipaPlatform targetPlatform)
    {
        UnityEngine.Debug.Log("build for " + targetPlatform);
        EditorUtility.DisplayProgressBar("building Pipa Sample Project", "build for " + targetPlatform, 0.5f);

        string path = null;
        string filename;

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
            filename = path + "/PipaSampleGame.apk";

            CompileWindow.ShowToast("Building for Android");
        }
        else
        if (targetPlatform == targetPipaPlatform.windows)
        {
            path = "bin/Windows";

            arg1 = "windows";

            target = BuildTarget.StandaloneWindows;
            filename = path + "/PipaSampleGame.exe";

            CompileWindow.ShowToast("Building for Windows");
        }

        try
        {

        
            // ensure path exists
            System.IO.Directory.CreateDirectory(path);

            filename = path + "/PipaSampleGame.exe";

            // Build player.
            BuildPipeline.BuildPlayer(levels, filename, target/*BuildTarget.StandaloneWindows*/, BuildOptions.None);

            EditorUtility.DisplayProgressBar("building Pipa Sample Project", "build ok.", 0.5f);


            #region File Copy...
            EditorUtility.DisplayProgressBar("building Pipa Sample Project", "file copy...", 0.5f);

            string targetFilename = path + "/README.md";
            if (File.Exists(targetFilename))
                File.Delete(targetFilename);

            // Copy a file from the project folder to the build folder, alongside the built game.
            FileUtil.CopyFileOrDirectory("README.md", targetFilename);

            if (basePath != null)
            {
                //Debug.LogWarning("basePath is not used here");
            }

            EditorUtility.DisplayProgressBar("building Pipa Sample Project", "file copy ok.", 0.5f);
            #endregion
        }
        catch (System.Exception e)
        {
            UnityEngine.Debug.LogError("exception: " + e);
            throw;
        }


        //--------------------
        // Run batch 
        //--------------------

        try
        {
            EditorUtility.DisplayProgressBar("building Pipa Sample Project", "run batch", 0.5f);

            //string batch_filename = "bin/
            string batch_filename = "bin/doit_upload.bat";
            batch_filename = System.IO.Path.GetFullPath("bin/doit_upload.bat");

            if (File.Exists(batch_filename))
            {
                UnityEngine.Debug.Log("run batch:" + batch_filename);

                Process proc = new Process();
                proc.StartInfo.FileName = batch_filename; 
                proc.StartInfo.Arguments = " " + arg1 + " " + arg2;
                proc.Start();
            }
            else
                UnityEngine.Debug.LogError("file not found:" + batch_filename);

            EditorUtility.DisplayProgressBar("building Pipa Sample Project", "run batch done.", 1.0f);

        }
        catch (System.Exception e)
        {
            EditorUtility.DisplayProgressBar("building Pipa Sample Project", "run batch failed", 0.5f);
            UnityEngine.Debug.LogError("exception: " + e);
            throw;
        }
    }

}
