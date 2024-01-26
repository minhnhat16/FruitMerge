using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemTemplate : MonoBehaviour
{
    [SerializeField] private Image backGround;
    [SerializeField] private Image itemImg;
    [SerializeField] private Text name_lb;
    [SerializeField] private Image ContainBox;
    [SerializeField] private Text total_lb;
    [SerializeField] private Text cost_lb;
    [SerializeField] private bool enable;

    public Image ItemImg { get => itemImg; set => itemImg = value; }
    public Text Name_lb { get => name_lb; set => name_lb = value; }
    public Image ContainBox1 { get => ContainBox; set => ContainBox = value; }
    public Text Total_lb { get => total_lb; set => total_lb = value; }
    public Text Cost_lb { get => cost_lb; set => cost_lb = value; }
    public bool Enable { get => enable; set => enable = value; }
}
