using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRender : MonoBehaviour
{
    [SerializeField] private Sprite target;
    [SerializeField] private GameObject parent;

    private void Awake()
    {
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        transform.localScale = parent.transform.localScale/2;
        transform.DORotate(new Vector3(0, 0, -360),3f,RotateMode.FastBeyond360)
          .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart); 
    }

    public void EnableTarget()
    {
        Debug.Log("ENABLE TARGET");
        gameObject.SetActive(true);
    }
    public void DisableTarget()
    {
        Debug.Log("ENABLE TARGET");
        gameObject.SetActive(false);
    }
}
