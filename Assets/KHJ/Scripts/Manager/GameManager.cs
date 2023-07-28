using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEditor.EditorTools;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    private static PoolManager poolManager;
    private static ResourceManager resourceManager;
    private static UIManager uiManager;
    //private static DataManager dataManager;
    //private static SceneManager sceneManager;
    //private static SoundManager soundManager;

    public static GameManager Instance { get { return instance; } }
    public static PoolManager Pool { get { return poolManager; } }
    public static ResourceManager Resource { get { return resourceManager; } }
    public static UIManager UI { get { return uiManager; } }
    //public static DataManager Data { get { return dataManager; } }
    //public static SceneManager Scene { get { return sceneManager; } }
    //public static SoundManager Sound { get { return soundManager; } }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
        InitManagers();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    private void InitManagers()
    {
        GameObject resourceObj = new GameObject();
        resourceObj.name = "ResourceManager";
        resourceObj.transform.parent = transform;
        resourceManager = resourceObj.AddComponent<ResourceManager>();

        GameObject poolObj = new GameObject();
        poolObj.name = "PoolManager";
        poolObj.transform.parent = transform;
        poolManager = poolObj.AddComponent<PoolManager>();

        GameObject UIObj = new GameObject();
        UIObj.name = "UIManager";
        UIObj.transform.parent = transform;
        uiManager = UIObj.AddComponent<UIManager>();

        //GameObject DataObj = new GameObject();
        //DataObj.name = "DataManager";
        //DataObj.transform.parent = transform;
        //dataManager = DataObj.AddComponent<DataManager>();
        //
        //GameObject SceneObj = new GameObject();
        //SceneObj.name = "SceneManager";
        //SceneObj.transform.parent = transform;
        //sceneManager = SceneObj.AddComponent<SceneManager>();
        //
        //GameObject SoundObj = new GameObject();
        //SoundObj.name = "SoundManager";
        //SoundObj.transform.parent = transform;
        //soundManager = SoundObj.AddComponent<SoundManager>();
    }
}