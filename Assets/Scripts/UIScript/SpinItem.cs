using UnityEngine;
using UnityEngine.UI;

public class SpinItem : MonoBehaviour
{
    [SerializeField] ItemType type;
    [SerializeField] int amount;
    [SerializeField] Image itemImg;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    public void InitItem(SpinConfigRecord record)
    {
        Debug.Log(record.Type + " " + record.Id );
        type = record.Type;
        amount = record.Amount;
        itemImg.sprite =SpriteLibControl.Instance.GetSpriteByName(record.ItemImg);
    }
}
