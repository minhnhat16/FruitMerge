using DG.Tweening;
using System.Collections;
using UnityEngine;
public class BootLoader : MonoBehaviour
{
    private GameManager gameManager;
    IEnumerator Start()
    {
        DontDestroyOnLoad(this);
        DOTween.SetTweensCapacity(1000, 50);
        yield return new WaitForSeconds(0.5f);
        gameManager = GetComponentInChildren<GameManager>();
        DataAPIController.instance.InitData(() =>
        {
            ConfigFileManager.Instance.Init(() =>
            {
                gameManager.SetupGameManager();
                MainScreenViewParam param = new MainScreenViewParam(); 
                param.totalGold = DataAPIController.instance.GetGold();
                ViewManager.Instance.SwitchView(ViewIndex.MainScreenView, param, () =>
                {
                    //GameManager.instance.LoadIngameSence();
                });
            });

        });
    }
    //private void LoadBuffer()
    //{
    //    SceneManager.LoadScene("Ingame");
    //    IngameController.instance.gameObject.SetActive(true);
    //    //Load data api  
    //}

    //private void InitConfigDone()
    //{

    //}
    private void InitDataDone()
    {
        LoadSceneManager.instance.LoadSceneByName("InGame", () =>
        {
            //gameManager.SetupGameManager();
            DataAPIController.instance.InitData(() =>
            {
                gameManager.LoadBufferScene();
            });
            ViewManager.Instance.SwitchView(ViewIndex.MainScreenView);
        });

    }
}
