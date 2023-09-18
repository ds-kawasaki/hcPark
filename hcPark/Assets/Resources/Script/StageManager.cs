using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager
{
    private static StageManager pInstance = null;
    public static StageManager Instance
    {
        get
        {
            if (pInstance != null) { return pInstance; }
            pInstance = new StageManager();
            return pInstance;
        }
    }

    private List<string> scenes;
    private int now;

    private StageManager()
    {
        this.now = 0;
        this.scenes = new List<string>();
        //ここに各ステージシーン名を登録する
        this.scenes.Add("Stage1");
        this.scenes.Add("Stage2");
    }

    public void ChangeScene(bool isNext)
    {
        if (isNext)
        {
            this.now++;
            if (this.now >= this.scenes.Count) { this.now = 0; }
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(this.scenes[this.now]);
    }
}
