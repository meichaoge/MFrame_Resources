using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MFramework;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TestSequence : MonoBehaviour
{
    public bool m_IsLoop;
    public UnityEngine.UI.Image m_img;
    // Use this for initialization
    BaseSpriteSequenceInfor infor = null;
    string[] Spritenames = new string[12];


    void Start()
    {
        for (int dex = 0; dex < 12; ++dex)
        {
          //  Spritenames[dex] = SpriteResPath.ActiveSpritePath + "Free Chest0" + (135 + dex);
          //>>>>>>>>>>>>>>>
        }//(1-6 天的签到宝箱效果)
        if (m_IsLoop)
        {
            infor = new LoopSpriteSequenceInfor(Spritenames, "Free Chest0", CheckStop, true, 0.2f,0.8f);
        }

        else
            infor = new SingleLoopSpriteSequenceInfor(Spritenames, "Free Chest0", CheckStop, true);

        infor.PlayingSequqnceAction += () =>
        {
            if (infor.CurrentSprite != null)
                Debug.Log("Playing:"+infor.CurrentSprite.name);
        };
        infor.BeginAction += () => { Debug.Log("Begin.."); };
        infor.BreakAction += () => { Debug.Log("Break"); };
        infor.EndAction += () => { Debug.Log("End"); };


        SpriteSequenceUtility.GetInstance().RecordSpriteSequence(infor);
        SpriteSequenceUtility.GetInstance().StartSpriteSequence("Free Chest0", m_img);
        //#if UNITY_EDITOR
        //        EditorApplication.isPaused = true;
        //#endif

    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Continue");
            SpriteSequenceUtility.GetInstance().ContinueSequence(infor);
        } //打断后继续

        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Restart ");
            SpriteSequenceUtility.GetInstance().RestartSequence(infor);
        }//重启

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Pause");
            SpriteSequenceUtility.GetInstance().ForcePauseSequence(infor);
        } //暂停

        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("Stop");
            SpriteSequenceUtility.GetInstance().ForceStopSequence(infor);
        }//停止
    }

    bool CheckStop()
    {
        if (infor == null)
        {
            Debug.LogError("CheckStop Fail");
            return false;
        }
        if (infor.CurrentSprite == null)
        {
            Debug.Log("Not Start");
            return false;
        }

        if (infor.CurrentSprite.name == "Free Chest0140") return true;
        return false;
    }

}
