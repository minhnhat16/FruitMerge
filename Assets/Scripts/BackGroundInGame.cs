using UnityEngine;

public class BackGroundInGame : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetUpBG()
    {
        if (!gameObject.activeSelf)
        {
            Debug.Log("ACTIVE BG");
            gameObject.SetActive(true);
            float rateX = 1920 /  CameraMain.instance.main.pixelWidth ;
            Debug.Log("rateX" + rateX);
            float rateY = 1080 / CameraMain.instance.main.pixelHeight;
            Debug.Log("rateY" + rateX);

        }
    }
}
