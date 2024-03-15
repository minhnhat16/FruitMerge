using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class TopWall : MonoBehaviour
{
    [SerializeField] private CircleObject crCircle;
    [HideInInspector]
    public UnityEvent<bool> gameOverEvent = new UnityEvent<bool>();
    private void OnEnable()
    {
        gameOverEvent = IngameController.instance.gameOverEvent;
        gameOverEvent.AddListener(GameOverEvent);
    }
    private void OnDisable()
    {
        gameOverEvent.RemoveAllListeners();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("MergeCircle") && crCircle == null)
        {
            crCircle = collision.GetComponentInParent<CircleObject>();
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("MergeCircle") && crCircle!=null)
        {
            //Debug.Log("MergeCircle touch topwall");
            StartCoroutine(GameOverCheck(crCircle));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (crCircle != null) crCircle = null;
    }
   
    IEnumerator GameOverCheck(CircleObject circle)
    {
        yield return new WaitForSeconds(1.5f);
        if (circle != null && circle.isActiveAndEnabled == true)
        {
            //Debug.Log("TRIGGER COLLIDER ");
            if (circle.GetCurrentState() == "GroundedState"
                && IngameController.instance.isGameOver == false
                    && circle.transform.position.y > transform.position.y)
            {
                //Debug.Log("TRIGGER GAME OVER");
                IngameController.instance.GameOver();
                gameObject.SetActive(false);
            }
            //else { yield break; }
        }
    }
    public void GameOverEvent(bool isGameOver)
    {
        if (isGameOver)
        {
            isGameOver = !isGameOver;
            DialogParam param = new();
             int score = IngameController.instance.Score;
            if (EndlessLevel.Instance.Life > 0)
            {
                EndlessLevel.Instance.FreezeCircleRev();
                DialogManager.Instance.ShowDialog(DialogIndex.ReviveDialog, param, () => {
                    Player.instance.canDrop = false;
                    Vector3 scale = new Vector3(0.75f, 0.75f, 0.75f);
                    CirclePool.instance.transform.localScale = scale;
                    WallScript.Instance.transform.localScale = scale;
                });
            }
            else
            {
                EndlessLevel.Instance.FreezeCircleDead();
                DialogManager.Instance.ShowDialog(DialogIndex.LoseDialog, param, () => {
                    Player.instance.canDrop = false;
                    Vector3 scale = new Vector3(0.75f, 0.75f, 0.75f);
                    CirclePool.instance.transform.localScale = scale;
                    WallScript.Instance.transform.localScale = scale;
                });
            }
            
        }
    }
}
    