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
            if (circle.GetCurrentState() != "SpawnState")
            {
                Debug.Log("TRIGGER COLLIDER ");

                IngameController.instance.GameOver();
            }
        }
    }
    public void GameOverEvent(bool isGameOver)
    {
        if (isGameOver)
        {
            LoseDialogParam param = new ();
            param.score = IngameController.instance.Score;
            DialogManager.Instance.ShowDialog(DialogIndex.LoseDialog, param ,() => {
                Player.instance.canDrop = false;
            }) ;
        }
    }
}
