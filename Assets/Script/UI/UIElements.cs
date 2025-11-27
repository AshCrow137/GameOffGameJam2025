using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
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
    public Button[] UnitsProduction;

    [Header("NextTurn Images")]
    [SerializeField]
    public Sprite PlayerTurn;
    [SerializeField]
    public Sprite EnemyTurn;

    [Header("Essence System")]
    [Header("Essence Images")]
    [SerializeField]
    public Sprite[] EssenceImgs;
}
