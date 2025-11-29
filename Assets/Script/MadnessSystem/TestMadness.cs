using UnityEngine;
using UnityEngine.UI;

public class TestMadness : MonoBehaviour
{
    public BaseKingdom kingdom;

    public Slider madnessLevel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        madnessLevel.maxValue = kingdom.maxMadnessLevel;
    }

    // Update is called once per frame
    void Update()
    {
        madnessLevel.value = kingdom.madnessLevel;
    }

    public void Add(int value)
    {
        kingdom.IncreaseMadness(value);
    }

    public void Sub(int value)
    {
        kingdom.DecreaseMadness(value);
    }

    //public void GetEffect()
    //{
    //    Debug.Log(kingdom.GetMadnessEffects());
    //}
}
