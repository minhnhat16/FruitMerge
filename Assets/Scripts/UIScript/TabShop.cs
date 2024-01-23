using UnityEngine;
using UnityEngine.UIElements;

public class TabShop : MonoBehaviour
{
    public GameObject tabOn;
    public GameObject tabOff;
    public ScrollView scrollView;
    public Animator animator;

    public void OnClickTabOn()
    {
        Debug.Log("Clicked tab on");
        tabOn.SetActive(true);
        tabOff.SetActive(false);
        animator.Play("TabOn");
        DialogManager.Instance.HideAllDialog();
        DialogManager.Instance.ShowDialog(DialogIndex.ShopDialog);
    }
    public void OnTabOff()
    {
        tabOn.SetActive(false);
        tabOff.SetActive(true);
    }
}
