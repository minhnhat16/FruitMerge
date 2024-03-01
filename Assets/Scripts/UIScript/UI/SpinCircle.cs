using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class SpinCircle : MonoBehaviour
{
    [SerializeField] RectTransform rect;
    [SerializeField] float radius;
    [SerializeField] int numberOfObjects;
    [SerializeField] List<SpinItem> _items;
    [SerializeField] List<float> angleSteps;
    [SerializeField] float angleCheck;
    // Start is called before the first frame update
    private void OnEnable()
    {
    }
    void Start()
    {
        rect = GetComponent<RectTransform>(); 
        SpawnObjectsInCircle();

    }

    // Update is called once per frame
    void Update()
    {
    }
    public void SpinningCircle()
    {
        // FUNCT CALCULATE WHERE THE ITEM ON THEN ROTATE TO THAT ITEM POST
        float vect = AngleCalculator() + 360*3; 
        transform.DORotate(new Vector3(0,0, vect), 10,RotateMode.LocalAxisAdd );
    }
    // FUNTION FIND ITEM'S ANGLE
    public float AngleCalculator()
    {
        int random = Random.Range(0, 7);
        var item = _items[random];
        float newAngle = angleCheck = angleSteps[random];
        item.gameObject.SetActive(false);
        return newAngle;
    }
    void SpawnObjectsInCircle()
    {
        Debug.Log("SpawnObjectsInCircle");
        float angleStep = 360f / numberOfObjects;
        for (int i = 0; i < numberOfObjects; i++)
        {
            float angle = i * angleStep;
            float x = Mathf.Cos(Mathf.Deg2Rad * (angle)) * radius;
            float y = Mathf.Sin(Mathf.Deg2Rad * (angle)) * radius;
            Vector3 spawnPosition = transform.position + new Vector3(x * 6, y * 6, 0f);
            angleSteps.Add(angle);
            Quaternion spawnRotation = Quaternion.Euler(0f, 0f, angle -100);
            GameObject item = Instantiate(Resources.Load("Prefab/UIPrefab/SpinItem", typeof(GameObject)), spawnPosition, spawnRotation,this.transform) as GameObject;
            _items.Add(item.GetComponent<SpinItem>());
        }
    }

}
