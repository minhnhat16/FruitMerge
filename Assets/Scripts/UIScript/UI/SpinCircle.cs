using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinCircle : MonoBehaviour
{
    [SerializeField] RectTransform rect;
    [SerializeField] float radius;
    [SerializeField] int numberOfObjects;
    [SerializeField] int minusStep;
    [SerializeField] List<SpinItem> _items;
    [SerializeField] List<float> angleSteps;
    [SerializeField] float angleCheck;
    [SerializeField] SpinConfig spinConfig;
    [SerializeField] ArrowSpin arrow;
    [SerializeField] SpinItem crItem;
    [SerializeField] Button button;
    [SerializeField] bool isSpining;
    // Start is called before the first frame update
    private void OnEnable()
    {
        spinConfig = ConfigFileManager.Instance.SpinConfig;
    }
    void Start()
    {
        isSpining = true;
        rect = GetComponent<RectTransform>();
        SpawnObjectsInCircle();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public IEnumerator HideViewAfterSpin()
    {
        yield return new WaitUntil(() => isSpining == false);
        yield return new WaitForSeconds(5f);    
        DialogManager.Instance.ShowDialog(DialogIndex.LableChooseDialog);
    }
    public void SpinningCircle()
    {
        isSpining = true;
        button.gameObject.SetActive(false);
        // FUNCT CALCULATE WHERE THE ITEM ON THEN ROTATE TO THAT ITEM POST
        float vect = AngleCalculator();
        //vect = Mathf.Clamp(vect, 180, -180);
        Debug.Log("VECT " + vect);
        Tween circleSpin = transform.DORotate(new Vector3(0, 0, vect + 360 * 5), 10, RotateMode.FastBeyond360);
        circleSpin.OnComplete(() => 
        {
            isSpining = false;
            arrow.StopArrowAnim(null);
            crItem.OnRewardItem();
            StartCoroutine(HideViewAfterSpin());

        });
    }
    // FUNTION FIND ITEM'S ANGLE
    public float AngleCalculator()
    {
        int random = Random.Range(0, 7);
        crItem  = _items[random];
        float newAngle = angleCheck =  280 - angleSteps[random];
        //item.gameObject.SetActive(false);
        return newAngle;
    }
    void SpawnObjectsInCircle()
    {
        Debug.Log("SpawnObjectsInCircle");
        float angleStep = 360f / numberOfObjects;
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * angleStep - minusStep;
            float x = Mathf.Cos(Mathf.Deg2Rad * (angle)) * radius;
            float y = Mathf.Sin(Mathf.Deg2Rad * (angle)) * radius;

            Vector3 localPost = new Vector3(x, y, 0f);
            Vector3 spawnPosition =  rect.TransformPoint(localPost);
            Debug.Log($"Spawn Position item spin {spawnPosition}"
                        + $"angle {angle}");
            angleSteps.Add(angle);
            Quaternion spawnRotation = Quaternion.Euler(0f, 0f, angle +80);
            GameObject item = Instantiate(Resources.Load("Prefab/UIPrefab/SpinItem", typeof(GameObject)), spawnPosition, spawnRotation, this.transform) as GameObject;
            var itemConfig = spinConfig.GetRecordByKeySearch(i);
            item.GetComponent<SpinItem>().InitItem(itemConfig);
            _items.Add(item.GetComponent<SpinItem>());
        }
        TestItemPosition();
    }
    void TestItemPosition()
    {
        foreach(var item in _items)
        {
            Debug.Log($"item.transform.position {item.transform.position}"
                + $"item.rotation {item.transform.rotation }");
        }
    }
}
