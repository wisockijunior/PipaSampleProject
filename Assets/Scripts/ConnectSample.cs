using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectSample : MonoBehaviour
{
    private string basePath;
    private string url;

    public Text UI_TextOutput_URL;
    public Text UI_TextOutput_basePath;

    // Start is called before the first frame update
    void Start()
    {
        bookmark.step_3_1 Unity_project_configuration;
        /*
         
            3.1.Unity project configuration

            Create an empty Unity project.Assuming the game would have online features and we’ll
            have 3 environments(e.g.test, homologation and production, with different server URL’s),
            create a way to define which environment to connect.When hitting ‘Play’, the game must display
            which environment is selected.
        */

        #region SetupBasePath
        // locallow file
        //url = "file:///" + Application.dataPath + "/AssetBundles/" + assetBundleName;
        //basePath = @"file://D:/testeAssetBundle/AssetBundles/" + assetBundleName;
        //basePath = @"http://192.168.10.103:8080/" + assetBundleName;


        basePath = @"file://D:/AssetBundles/";

#if test
        // LAN local network - IP fixo reservado: 192.168.25.241
        basePath = @"http://192.168.25.241:8080/";// + assetBundleName;
        basePath = @"http://192.168.25.242:8080/";// + assetBundleName;
#endif

#if homologation

        // using ddns for homologation
        basePath = @"http://blocksxr.ddns.net:8080/";

#endif

#if production
        
        // production url
        basePath = @"http://blocksxr.com/";
#endif

        // just debug
        bool debug_forceUseLocal = false;
        if (debug_forceUseLocal)
            basePath = @"file://D:/AssetBundles/";



        if (Application.isEditor)
        {
            //notebook local file...
            basePath = @"file://Y:/";
            basePath = @"file://D:/AssetBundles/";

            //basePath = @"file://error/";

            //test local file SpaceShip HoloTable
            //basePath = @"file://D:/AssetBundles/";
            //basePath = @"file://Y:/AssetBundles/";


            //simulate error
            //basePath = @"file://error/";
        }

        
        string themePath = "western";        
        string assetBundleName = themePath + "/avatar";

        if (Application.platform == RuntimePlatform.WebGLPlayer)
            url = basePath + "WebGL/" + assetBundleName;
        else
        if (Application.platform == RuntimePlatform.Android)
            url = basePath + "Android/" + assetBundleName;
        else
        if (Application.platform == RuntimePlatform.WindowsPlayer
            || Application.platform == RuntimePlatform.WindowsEditor)
        {
            // default - all other platforms

            url = basePath + "Windows/" + assetBundleName;
        }


#if UNITY_WEBGL
        //basePath = @"http://177.96.18.133:8080/";
        //basePath = @"http://187.112.57.215:8080/";
        //basePath = @"http://192.168.25.26:8080/";
#endif

        #endregion

        Debug.LogWarning("basePath:" + basePath);
        Debug.LogWarning("url:" + url);

        UI_TextOutput_URL.text = url;
        UI_TextOutput_basePath.text = basePath;


        // first time
        JustLoad_a_sample_avatar();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void JustLoad_a_sample_avatar()
    {
        //instantiate character
        doit_AsyncLoad(true, go3 => {

            GameObject GO1;
            GameObject GO2;
            GameObject GO3;

            GO1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            GO2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //GO3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //GO3 = GameObject.Instantiate(PrefabsXR.instance.HMD_SimpleTestGO);
            GO1.GetComponent<BoxCollider>().enabled = false;
            GO2.GetComponent<BoxCollider>().enabled = false;

            GO1.transform.localScale = Vector3.one * 0.02f;
            GO2.transform.localScale = Vector3.one * 0.02f;

            //go3.name = "InstantiateOK_" + go3.name;
            go3.name = "avatar_" + this.name + "_" + go3.name;
            Debug.Log("instantiate - " + go3.name);

            /*
            if (wisockiView.Get_Owner_IsLocal())
            {
                GameObject lookAtPos;
                lookAtPos = new GameObject();
                lookAtPos.name = "lookAtPos";
                lookAtPos.transform.parent = Camera.main.transform;
                lookAtPos.transform.localPosition = new Vector3(0, 0, 0.5f);
                lookAtPos.transform.localRotation = default;

                AvatarIK avatarIK;
                avatarIK = go3.GetComponent<AvatarIK>();
                if (avatarIK == null)
                {
                    Debug.LogError("avatarIK not found");
                    avatarIK = go3.AddComponent<AvatarIK>();
                }
                if (avatarIK != null)
                    avatarIK.headObj = lookAtPos.transform;

                go3.transform.localScale = Vector3.one * 1.0f;
                go3.SetActive(false);
            }
            else
            {
                go3.transform.localScale = Vector3.one * 1.0f;
                NetGo3 = go3.AddComponent<NetSyncPlayerWsk>();
            }*/

            GO3 = go3;

            //playerXR.GO3 = go;
        }, go3 => {

            //ERROR
            Debug.LogError("load avatar fail");


        });
    }


    public class WskRequest
    {
        //public PlayerXR playerXR;
        public Action<GameObject> OnSuccess;
        public Action<GameObject> OnError;
    }
    public class AssetBundleEx
    {
        internal string url;
        internal AssetBundle bundle;
        internal bool loading;
        internal bool isReady_loadedOK;

        /// <summary>
        /// queue
        /// </summary>
        public List<WskRequest> Requestlist;

        internal string assetItem_PrefabName;
        internal string loadedPrefabFrom_assetItem_PrefabName;
        internal GameObject prefab;

    }


    
    public List<AssetBundleEx> bundles = new List<AssetBundleEx>();
    public void doit_AsyncLoad(bool doLoadAvatar,/*PlayerXR playerXR, */
       Action<GameObject> OnSuccess,
       Action<GameObject> OnError)
    {
        WskRequest request;
        request = new WskRequest();
        //request.playerXR = playerXR;
        request.OnSuccess = OnSuccess;
        request.OnError = OnError;

        //test
        string assetBundleName = "environment/forest1.avatar1";

        string themePath;
        themePath = "western";
        //themePath = "spaceshipTheme";
        themePath = "spaceship1";

        // avatar
        if (doLoadAvatar)
        {
            //assetBundleName = "western/avatar";
            assetBundleName = themePath + "/avatar";

        }

        //scene
        //assetBundleName = "western/rooms";
        //table
        //assetBundleName = "western/tables";


        if (!doLoadAvatar)
        {
            //if (Application.platform == RuntimePlatform.Android)
            //    assetBundleName = "Android/western/rooms";
            //else
            //{
            //    //if (Application.platform == RuntimePlatform.Windows)
            //    assetBundleName = "Windows/western/rooms";
            //}

            //assetBundleName = "western/rooms";
            assetBundleName = themePath + "/rooms";
        }

        string assetItem_PrefabName = "assets/polygonstarter/prefabs/characters/sm_bean_cop_01.prefab";

        InstantiateObject(request, assetItem_PrefabName, assetBundleName);
    }

    public void InstantiateObject(WskRequest wskRequest, string assetItem_PrefabName, string assetBundleName)
    {
        




        /*
         (3) Environment selection & build script

            3.1. Unity project configuration
            Create an empty Unity project. Assuming the game would have online features and we’ll
            have 3 environments (e.g. test, homologation and production, with different server URL’s),
            create a way to define which environment to connect. When hitting ‘Play’, the game must display
            which environment is selected.

        */






        if (Application.platform == RuntimePlatform.WebGLPlayer)
            url = basePath + "WebGL/" + assetBundleName;
        else
        if (Application.platform == RuntimePlatform.Android)
            url = basePath + "Android/" + assetBundleName;
        else
        if (Application.platform == RuntimePlatform.WindowsPlayer
            || Application.platform == RuntimePlatform.WindowsEditor)
        {
            // default - all other platforms

            url = basePath + "Windows/" + assetBundleName;
        }



        //TODO: 2 simultaneous AssetBundle request ==> only 1 async web request
        AssetBundleEx bundle_ex = null;
        for (int i = 0; i <= bundles.Count - 1; i++)
        {
            if (bundles[i].url == url)
                bundle_ex = bundles[i];
        }

        if (bundle_ex == null)
        {
            bundle_ex = new AssetBundleEx();
            if (bundle_ex.Requestlist == null)
                bundle_ex.Requestlist = new List<WskRequest>();
            bundle_ex.url = url;
            bundles.Add(bundle_ex);
        }

        //TODO: use correct assetname
        //string assetname = "avatar1";
        bundle_ex.assetItem_PrefabName = assetItem_PrefabName;


        bundle_ex.Requestlist.Add(wskRequest);
        Debug.Log("queue:" + bundle_ex.Requestlist.Count);



        if (bundle_ex.isReady_loadedOK)
        {
            //BundleReady_LoadedOK(bundle_ex);
        }
        else
        if (bundle_ex.loading)
        {
            // already loading, wait finish
        }
        else
        {
            // async request

            // start load and wait finish
            bundle_ex.loading = true;
            //StartCoroutine(RequestAssetBundleAsync(bundle_ex));
        }
    }
}
