using UnityEngine;

[CreateAssetMenu(fileName = "New City", menuName = "City System/City Data")]
public class CityData : ScriptableObject
{
    [Header("City Information")]
    public Sprite sprite;
    
    [Header("City Stats")]
    public float maxHP = 100f;
    // public int visionRadius = 6;
}

