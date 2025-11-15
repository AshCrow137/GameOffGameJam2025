using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class UIElements : MonoBehaviour
{
    [Header("Stats Pannel")]
    [SerializeField]
    public Sprite PlayerPannel;
    [SerializeField]
    public Sprite EnemyPannel;

    [Header("Infantary Panel")]
    [SerializeField]
    public Sprite EnemyType;
    [SerializeField]
    public Sprite PlayerType;

    [Header("City Pannel")]
    [SerializeField]
    public Sprite CityPanel;
    [SerializeField]
    public Image[] UnitsProduction;

    [Header("Essence System")]
    [Header("Essence Images")]
    [SerializeField]
    public Sprite[] EssenceImgs;
}
