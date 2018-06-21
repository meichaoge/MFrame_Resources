using MFramework.Re;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_ABundle : MonoBehaviour
{
    public UnityEngine.UI.Image m_img;
    // Use this for initialization
    void Start()
    {
        Debug.Log(Application.dataPath);
        Debug.Log(Application.streamingAssetsPath);
        Debug.Log(Application.persistentDataPath);

        APPEngineManager.GetInstance().OnAssetUpdateDoneAct += Test;
        gameObject.SetActive(false);
    }


    void OnCheckABResourceCallback(bool isfinish)
    {
        Debug.Log("OnCheckABResourceCallback  isfinish=" + isfinish);
        if (isfinish)
        {
            ResourcesMgr.GetInstance();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    ResourcesMgr.GetInstance().LoadAsset("prefabs/Cube", (obj) =>
        //    {
        //        if (obj != null)
        //        {
        //            Debug.LogInfor("Success");
        //            Object ob = GameObject.Instantiate(obj);
        //        }
        //        else
        //        {
        //            Debug.LogError("Fail");
        //        }
        //    });
        //}


        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    ResourcesMgr.GetInstance().LoadSprite("Texture/SelfIcon", (obj) =>
        //    {
        //        m_img.sprite = obj as Sprite;
        //    });
        //}

        //if (Input.GetKeyDown(KeyCode.X))
        //{
        //    ResourcesMgr.GetInstance().LoadAsset("timg", (obj) =>
        //    {
        //        m_img.sprite = (obj as GameObject).GetComponent<SpriteRenderer>().sprite;
        //    }); 
        //}

    }

    public void Test()
    {
        gameObject.SetActive(true);
        //ResourcesMgr.GetInstance().LoadSprite("Texture/SelfIcon", (obj) =>
        //{
        //    m_img.sprite = obj as Sprite;
        //});

        ResourcesMgr.GetInstance().LoadAsset("Localization_Sprite/cn/Main/icon_shangdian", (obj) =>
        {
            m_img.sprite = (obj as GameObject).GetComponent<SpriteRenderer>().sprite;
        });
    }






}
