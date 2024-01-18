using UnityEngine;
using UnityEngine.Events;

public class TopWall : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent<bool> gameOverEvent = new UnityEvent<bool>();
    private void OnEnable()
    {
        gameOverEvent = IngameController.instance.gameOverEvent;
        gameOverEvent.AddListener(GameOverEvent);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("MergeCircle"))
        {
            Debug.Log("MergeCircle");
            var circle = collision.GetComponentInParent<CircleObject>();
            if (circle != null )
            {
                Debug.Log("TRIGGER COLLIDER ");
                if(circle.GetCurrentState() != "SpawnState") IngameController.instance.GameOver();
            }
        }
    }
    public void GameOverEvent(bool isGameOver)
    {
        if (isGameOver)
        {
            isGameOver = !isGameOver;
            LoseDialogParam param = new ();
            param.score = IngameController.instance.Score;
            DialogManager.Instance.ShowDialog(DialogIndex.LoseDialog, param ,() => {
                Player.instance.canDrop = false;
                Vector3 scale = new Vector3(0.75f, 0.75f, 0.75f);
                CirclePool.instance.transform.localScale = scale;
                WallScript.Instance.transform.localScale = scale;
            }) ;
        }
    }
}
