// Small example that shows when scripts are being compiled.

using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WisockiSampleCompileWindow
{

    public class CompileWindow : EditorWindow
    {
        private const string Title = "Pipa Compile Window";

        public static bool bShowProgressBar = false;

        [UnityEditor.MenuItem("Window/CompileWindow")]
        static void Init()
        {
            //EditorWindow window = GetWindowWithRect(typeof(CompileWindow), new Rect(0, 0, 200, 200));
            //window.Show();

            var window = GetWindowWithRect(typeof(CompileWindow), new Rect(0, 0, 200, 25));
            window.titleContent = new GUIContent(Title);
            window.Show();

            instance = window as CompileWindow;
        }

        CompileWindow()
        {
            //Debug.Log(" constructor...");

            //int newvalue = PlayerPrefs.GetInt("autoRunDisable");
            //Debug.Log(" constructor... autoRunDisable=" + newvalue);

            instance = this;
        }

        ~CompileWindow()
        {
            instance = null;
        }

        public static CompileWindow instance;


        private bool _compiling;
        private bool _playing;
        //private static bool doRUN;
        //private static bool doPlay;
        //private static int doPlay_countdown;

        //private static bool doSTOP;
        private int doRestart;
        private static float marktime;
        private static bool progressBar_visible;
        private static float marktime_close;
        private static string msg = null;
        private static string msg3 = null;


        public enum Action
        {

            action_just_play,
            action_stop,
            action_RESTART_stop,
            action_RESTART_play,

        }


        public class DoActionMgr
        {
            public string showmessage;
            public int frameskip;
            public float timeskip_marktime;
            public Action nextAction;
        }

        //public DoActionMgr next_action;
        public List<DoActionMgr> next_action_List = new List<DoActionMgr>();

        public void DoLog(string msg)
        {
            Debug.Log(msg);
        }

        public void DoIt(string msg, Action nextAction)
        {
            //debug
            Debug.LogWarning(" +++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            Debug.LogWarning("+++ NEW ACTION " + nextAction.ToString() + "  :" + msg);


            DoActionMgr action = new DoActionMgr();
            action.showmessage = msg;// " restart...";
            action.frameskip = 3;
            //action.timeskip_marktime = (float)EditorApplication.timeSinceStartup;
            action.nextAction = nextAction;

            //next_action = action;
            next_action_List.Add(action);
        }

        private bool initialized;

        public static float t3;
        public static float t2;
        public static string title8 = "";
        public static string message = "";
        private string debugStr1 = null;
        public bool autoRunDisable = true;
        private static bool capsLock;
        private static bool IGNORE_ONCE;
        private static bool STOP_ONCE;



        void OnGUI()
        {
            //Debug.LogWarning("left shift: "+ Input.GetKey(KeyCode.LeftShift));


            if (!initialized)
            {
                initialized = true;

                int newvalue2 = PlayerPrefs.GetInt("autoRunDisable");

                //Debug.LogWarning(" --------------------------------------------------------------");
                //Debug.LogWarning(" init... autoRunDisable=" + newvalue2);

                if (newvalue2 == 1)
                    autoRunDisable = true;
                else
                    autoRunDisable = false;
            }
            //EditorGUILayout.LabelField("Compiling:", EditorApplication.isCompiling ? "Yes" : "No");
            float time = (float)EditorApplication.timeSinceStartup;
            //this.Repaint();

            var isCompiling = EditorApplication.isCompiling;

            if (STOP_ONCE)
            {
                STOP_ONCE = false;
                // just stop
                if (CompileWindow.instance != null)
                    CompileWindow.instance.DoIt(" just stop...", Action.action_stop);
            }
            //---------------------------------------------
            // Compiling...   started....  finished...
            //---------------------------------------------
            //---------------------------------------------
            if (EditorApplication.isCompiling != _compiling)
            {
                _compiling = isCompiling;
                if (_compiling)
                {
                    //Debug.LogWarning(">>> >>> >>> compiling started...");

                    ShowToast("compiling started...");
                }
                else
                {
                    //Debug.LogWarning(">>> >>> >>> compiling finished...");

                    ShowToast("compiling finished...");

                    _pending_startPlay = true;



                }
            }
            //---------------------------------------------
            //---------------------------------------------



            //EditorGUILayout.LabelField(isCompiling ? "Yes" : "No");



            float totalProgTime = 0.15f;



            float t = 0f;// 0.5f;        
            if (frame_counter > 0)
            {
                frame_counter--;
                marktime = (float)EditorApplication.timeSinceStartup;
                //Debug.Log(" reset marktime... " + frame_counter);

                if (frame_counter == 0)
                {
                    if (_pending_startPlay == true)
                    {
                        _pending_startPlay = false;
                        // next action 
                        if (!CompileWindow.instance.autoRunDisable)
                        {
                            if (CompileWindow.instance != null)
                                CompileWindow.instance.DoIt(" play...", Action.action_just_play);
                        }
                    }
                }
            }
            if (marktime > 0)
                t = (time - marktime);

            if (t > totalProgTime)//0.25f)
            {
                marktime = 0f;
                msg = "";
            }






            DoActionMgr action = null;// = next_action;
            if (next_action_List.Count > 0)
                action = next_action_List[0];

            if (t > 0 || isCompiling || action != null)//isCompiling)//progress < secs)
            {
                title8 = "status...";

                if (isCompiling)
                {
                    title8 = "Compiling...";
                    message = "Compiling..." + msg;
                }
                else
                {
                    if (action != null)
                    {
                        message = msg + ";" + action.nextAction;
                    }
                    else
                        message = msg + " {msg}";

                    if (!string.IsNullOrEmpty(msg3))
                    {
                        message = message + "; [" + msg3 + "]";
                    }

                }
                //else
                //{
                //    title = "status...";
                //    message = msg;
                //    //message = "Compiling progress...";
                //}


                //if (current_state == PlayModeStateChange.ExitingPlayMode)
                //    t2 = (1.0f - t) / 10f;
                //else


                t2 = t / totalProgTime;// 0.25f;


                //if (isCompiling)
                //    t = 0.5f;
                //if (isCompiling)
                //    t2 = 0.5f + t2;
                //if (current_state == PlayModeStateChange.ExitingEditMode)
                //    t2 = 0.7f + t2;
                //if (current_state == PlayModeStateChange.EnteredPlayMode)
                //    t2 = 0.9f + t2;

                //if (current_state == PlayModeStateChange.ExitingPlayMode)
                //    t2 = 0.3f - t2;
                //if (current_state == PlayModeStateChange.EnteredEditMode)
                //    t2 = 0.1f - t2; 

                if (bShowProgressBar)
                {
                    EditorUtility.DisplayProgressBar(title8 + "-" + msg3, message, t2);
                    progressBar_visible = true;
                }
            }
            else
            {
                bool modoDebug = false;
                modoDebug = true;
                if (modoDebug)
                {
                    if (progressBar_visible)
                    {
                        progressBar_visible = false;
                        marktime_close = time;
                    }

                    if (marktime_close > 0)
                    {
                        t3 = (time - marktime_close);

                        if (t3 > totalProgTime)
                        {
                            marktime_close = 0;
                            //t3 = 0;

                            msg = "?";
                            msg3 = "";
                            EditorUtility.ClearProgressBar();
                        }
                        else
                        {
                            //reverse
                            float progress_t = (totalProgTime - t3) / totalProgTime;

                            if (bShowProgressBar)
                            {
                                EditorUtility.DisplayProgressBar(title8 + "-" + msg3, message, progress_t);
                            }
                        }
                    }
                }
                else
                {
                    EditorUtility.ClearProgressBar();
                }
            }



            if (action != null)
            {
                float t3 = 0;
                if (action.timeskip_marktime > 0)
                    t3 = time - action.timeskip_marktime;

                if (action.showmessage != null)
                {
                    string str3 = "";
                    if (t3 > 0)
                        str3 = "   timeskip:" + t3.ToString("N3");

                    debugStr1 = action.showmessage + action.frameskip + str3;
                    // aguardando... executar acao


                    //action.showmessage = null;
                }

                if (t3 > 0)
                {
                    //float t3 = (float)EditorApplication.timeSinceStartup - action.timeskip_marktime;

                    // 5 seconds
                    if (t3 > 5)
                    {
                        Debug.Log("skip time:" + t3.ToString("N0"));
                        action.timeskip_marktime = 0;
                    }

                }
                else
                if (action.frameskip > 0)
                {
                    //Debug.Log("skip frame:" + action.frameskip);
                    action.frameskip--;
                    if (action.frameskip == 0)
                        action.showmessage = null;
                }
                else
                {
                    if (action.nextAction == Action.action_RESTART_stop)
                    {
                        ShowToast("ScriptsReloaded - playing... STOP and RESTART...");

                        Debug.Log("  stop...  doRestart");
                        EditorApplication.isPlaying = false;
                        doRestart = 3;

                        // done
                        //next_action = null;
                        next_action_List.Remove(action);
                    }
                    else
                    if (action.nextAction == Action.action_RESTART_play)
                    {
                        Debug.Log(">>> >>> >>> restart ...  Play!");
                        // Unity 2019
                        //EditorApplication.EnterPlaymode()
                        if (!EditorApplication.isPlayingOrWillChangePlaymode)
                        {
                            EditorApplication.isPlaying = true;

                            // done
                            //next_action = null;
                            next_action_List.Remove(action);
                        }
                        else
                        {
                            // falhou
                            //next_action = null;
                            next_action_List.Remove(action);
                        }
                    }
                    else
                    if (action.nextAction == Action.action_stop)
                    {
                        ShowToast("ScriptsReloaded - playing... STOP and RESTART...");

                        Debug.Log(" stop.");
                        EditorApplication.isPlaying = false;

                        // done
                        //next_action = null;
                        next_action_List.Remove(action);
                    }
                    else
                    if (action.nextAction == Action.action_just_play)
                    {
                        ShowToast("ScriptsReloaded - not playing, just START");
                        //Debug.Log(">>> >>> >>> just start ...  Play!");
                        // Unity 2019
                        //EditorApplication.EnterPlaymode()
                        if (!EditorApplication.isPlayingOrWillChangePlaymode)
                        {
                            // not-compiling, not-playing,  play, 

                            EditorApplication.isPlaying = true;
                            Debug.Log(">>> >>> >>> play...");

                            // done
                            //next_action = null;
                            next_action_List.Remove(action);
                        }
                        else
                        {
                            // ignore, already playing
                            //next_action = null;
                            next_action_List.Remove(action);
                            //Debug.LogError(">>> >>> >>> play falhou.");
                        }
                    }
                }
            }




            if (EditorApplication.isPlaying)
            {
                if (!_playing)
                {
                    //Debug.LogWarning(">>> playing >>>");
                    //Debug.LogWarning(">>> >>> >>> playing");
                    _playing = true;
                }
            }
            else
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {

            }
            else
            {
                if (_playing)
                {
                    //Debug.LogWarning("<<< NOT playing <<<");
                    Debug.LogWarning(">>> <<< <<< NOT playing");
                    _playing = false;
                }
            }


            if (current_state != _state)
            {
                _state = current_state;


                if (!autoRunDisable)
                {
                    ShowToast("change:" + _state.ToString());
                }
            }


            if (EditorApplication.isPlaying)//isPlaying)
            {
                //if (doSTOP)
                {
                    //doSTOP = false;
                    //if (doRestart == 2)
                    //{
                    //    // next action 
                    //    CompileWindow.instance.DoIt(" restart...", Action.action_RESTART_play_stop);


                    //    doRestart = 3;
                    //    Debug.Log("  stop.ok  doRestart :" + doRestart);

                    //}
                }
            }
            else
            {
                if (current_state == PlayModeStateChange.ExitingPlayMode)
                {

                    if (doRestart == 3)
                    {
                        //Debug.Log("  restart ... wait...");// doRestart :" + doRestart);
                    }

                    // STOP...?

                    //wait
                    //Debug.Log("  restart ...  wait");
                }
                else
                if (current_state == PlayModeStateChange.EnteredEditMode)
                {
                    if (doRestart == 3)
                    {
                        Debug.Log("  restart ...  doRestart :" + doRestart);
                        Debug.Log("  restart ...  PlayModeStateChange.EnteredEditMode");
                        doRestart = 0;

                        // PLAY
                        if (!EditorApplication.isPlayingOrWillChangePlaymode)
                        {
                            //doPlay = true;
                            ShowToast("play...");
                            //doPlay_countdown = 3;

                            // next action 
                            CompileWindow.instance.DoIt(" restart...", Action.action_RESTART_play);

                        }
                        else
                        {
                            //PlayModeStateChange.EnteredEditMode
                            Debug.LogError("  restart error.");

                        }
                    }
                }
            }





            #region GUI
            bool newvalue = EditorGUILayout.Toggle("autoRunDisable", autoRunDisable);







            if (newvalue != autoRunDisable)
            {
                //autoRunDisable = EditorGUILayout.Toggle("auto-run", autoRunDisable);
                autoRunDisable = newvalue;
                if (autoRunDisable)
                    PlayerPrefs.SetInt("autoRunDisable", 1);
                else
                    PlayerPrefs.SetInt("autoRunDisable", 0);

                int newvalue2 = PlayerPrefs.GetInt("autoRunDisable");
                Debug.Log(" new autoRunDisable=" + newvalue2);

            }

            EditorGUILayout.LabelField(debugStr1);



            Event e = Event.current;

            if (e.capsLock)
            {
                if (!capsLock)
                {
                    capsLock = true;
                    Debug.LogWarning(" capsLock = TRUE; ");

                    IGNORE_ONCE = true;
                    if (EditorApplication.isPlaying) //./.isPlaying)
                    {
                        //Debug.LogWarning(" capsLock, STOP; ");
                        //STOP_ONCE = true;
                    }
                    else
                    {

                    }

                    if (CompileWindow.instance != null)
                        CompileWindow.instance.autoRunDisable = true;

                    /*
                    if (EditorApplication.isPlaying) //./.isPlaying)
                    {                    
                        if (CompileWindow.instance != null)
                            CompileWindow.instance.DoIt(" play...", Action.action_stop);                    
                    }
                    else
                    {
                        if (CompileWindow.instance != null)
                            CompileWindow.instance.DoIt(" play...", Action.action_just_play);
                    }
                    */
                }
                //GUI.Label(new Rect(10, 10, 100, 20), "CapsLock on.");
            }
            else
            {
                if (capsLock)
                {
                    Debug.LogWarning(" capsLock = false;");
                    capsLock = false;

                    //var isCompiling = EditorApplication.isCompiling;
                    if (!isCompiling)
                    {
                        if (EditorApplication.isPlaying)
                        {
                            //EditorGUILayout.LabelField("Playing");
                            if (EditorApplication.isPlaying) //./.isPlaying)
                            {
                                Debug.LogWarning(" capsLock, STOP; ");
                                STOP_ONCE = true;
                            }
                        }
                        else
                        if (EditorApplication.isPlayingOrWillChangePlaymode)
                        {
                            //EditorGUILayout.LabelField("WillChangePlaymode");
                        }
                        else
                        {
                            //EditorGUILayout.LabelField("Not playing");
                            //isCompiling
                            //EditorApplication.LockReloadAssemblies();
                            //EditorApplication.LockReloadAssemblies();

                            Debug.LogWarning(" capsLock, PLAY; ");
                            if (CompileWindow.instance != null)
                                CompileWindow.instance.DoIt(" play...", Action.action_just_play);
                        }

                    }
                }
            }

            if (e.capsLock)
            {
                GUI.Label(new Rect(10, 10, 100, 20), "CapsLock on.");
            }
            else
            {
                GUI.Label(new Rect(10, 10, 100, 20), "CapsLock off.");
            }

            if (action != null)
                EditorGUILayout.LabelField("next action:" + action.nextAction);
            else
                EditorGUILayout.LabelField("next action:  ---");


            if (EditorApplication.isPlaying)
            {
                EditorGUILayout.LabelField("Playing");
            }
            else
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                EditorGUILayout.LabelField("WillChangePlaymode");
            }
            else
            {
                EditorGUILayout.LabelField("Not playing");
            }

            EditorGUILayout.LabelField("time :" + t.ToString("N3"));// isCompiling ? "Yes" : "No");
                                                                    //EditorGUILayout.LabelField("marktime:" + marktime.ToString("N3"));// isCompiling ? "Yes" : "No");

            this.Repaint();
            #endregion

        }


        private bool _pending_startPlay;
        private static int frame_counter;
        private static PlayModeStateChange current_state;
        private PlayModeStateChange _state;

        public static void ShowToast(string newmsg)
        {
            msg = newmsg;
            marktime = (float)EditorApplication.timeSinceStartup;
            //Debug.LogWarning(" ----------------------------- ");
            //Debug.LogWarning(" (show message) " + msg + " ttime:" + MeshCollection.GetGlobalTime().ToString("N3"));

            frame_counter = 3;
        }

        public static void ShowProgressEx(string newtitle, string newmsg)
        {
            msg3 = reloadCounter.ToString() + newmsg;
            message = message + "; [" + msg3 + "]";

            if (bShowProgressBar)
            {
                EditorUtility.DisplayProgressBar(newtitle + "-" + msg3, message, 0.5f);
                progressBar_visible = true;
            }
            marktime = (float)EditorApplication.timeSinceStartup;
        }

        void OnApplicationFocus(bool hasFocus)
        {
            Debug.LogWarning("OnApplicationFocus");
            //; ; isPaused = !hasFocus;
        }

        void OnApplicationPause(bool pauseStatus)
        {
            Debug.LogWarning("OnApplicationFocus");
            //isPaused = pauseStatus;
        }

        private static int reloadCounter;
        [UnityEditor.Callbacks.DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            //float time = (float)EditorApplication.timeSinceStartup;
            // do something
            reloadCounter++;


            bool ignore_auto_start = false;
            // next action 
            if (CompileWindow.instance != null)
                if (CompileWindow.instance.autoRunDisable)
                    ignore_auto_start = true;

            //if (Input.mousePosition.x < 10)
            //if (Input.GetKey(KeyCode.LeftShift))
            //{
            //    Debug.LogWarning("skip autoplay");
            //    ignore_auto_start = true;
            //}

            if (IGNORE_ONCE)
            {
                IGNORE_ONCE = false;
                Debug.LogWarning("skip autoplay");
                ignore_auto_start = true;
            }


            if (EditorApplication.isPlaying) //./.isPlaying)
            {
                //Debug.LogWarning("playing... STOP and RESTART");
                //doSTOP = true;

                //marktime = (float)EditorApplication.timeSinceStartup;
                //msg = "Stop running.!!!...";

                //doRestart = 2;
                //if (doRestart == 2)
                if (CompileWindow.instance != null)
                {
                    if (capsLock)
                    {
                        Debug.LogWarning(" capsLock.. ativado, skip autoplay");

                        // capslock --> autoRun disable
                        if (!CompileWindow.instance.autoRunDisable)
                            ShowProgressEx("OnScriptsReloaded", "ScriptsReloaded, capslock --> autoRun disable");
                    }
                    else
                    {
                        Debug.LogWarning(" capsLock.. nao ativado ainda..., skip autoplay");


                        if (!CompileWindow.instance.autoRunDisable)
                            ShowProgressEx("OnScriptsReloaded", "ScriptsReloaded, restart...");

                        if (ignore_auto_start)
                        {

                            // just stop                        
                            CompileWindow.instance.DoIt(" just stop...", Action.action_stop);
                        }
                        else
                        {
                            // auto-start --> restart                        
                            CompileWindow.instance.DoIt(" restart...", Action.action_RESTART_stop);
                        }
                    }
                }


            }
            else
            {
                //Debug.LogWarning("not playing... just START");
                //doRUN = true;
                //marktime = (float)EditorApplication.timeSinceStartup;


                //doPlay_countdown = 3;
                //doPlay_countdown = 3;
                // next action 

                if (CompileWindow.instance != null)
                    if (!CompileWindow.instance.autoRunDisable)
                        ShowProgressEx("OnScriptsReloaded", "ScriptsReloaded, just play...");

                // next action 
                if (CompileWindow.instance != null)
                    if (!CompileWindow.instance.autoRunDisable)
                    {
                        if (ignore_auto_start)
                        {
                            Debug.LogWarning("skip autoplay");
                        }
                        else
                        {

                            if (CompileWindow.instance != null)
                            {
                                if (CompileWindow.instance.next_action_List.Count == 0)
                                {
                                    CompileWindow.instance.DoIt(" play...", Action.action_just_play);
                                }
                            }
                        }
                    }
            }
        }


        // ensure class initializer is called whenever scripts recompile
        [InitializeOnLoadAttribute]
        public static class PlayModeStateChangedExample
        {
            // register an event handler when the class is initialized
            static PlayModeStateChangedExample()
            {
                EditorApplication.playModeStateChanged += LogPlayModeState;
            }

            private static void LogPlayModeState(PlayModeStateChange state)
            {
                current_state = state;

                if (CompileWindow.instance != null)
                    if (!CompileWindow.instance.autoRunDisable)
                    {
                        ShowProgressEx("playModeStateChanged", "state:" + current_state.ToString());

                    }

                //Debug.Log(state);
            }
        }
    }

}