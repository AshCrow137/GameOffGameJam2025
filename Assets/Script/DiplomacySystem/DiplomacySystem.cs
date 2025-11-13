using TMPro;
using UnityEngine;

public class DiplomacySystem : MonoBehaviour
{ 
    private BaseKingdom currentKingdom;

    public void StartDiplomacy(BaseKingdom baseKingdom)
    {
        currentKingdom = baseKingdom;
        Debug.Log("Madness Level of " + currentKingdom.name + ": " + currentKingdom.madnessLevel);
        Debug.Log("Starting diplomacy with " + currentKingdom.name);
        Debug.Log(GlobalTexts.QUESTION);
        Debug.Log("1. " + GlobalTexts.DECLARE_WAR);
        Debug.Log("2. " + GlobalTexts.FREE_PASS);
        Debug.Log("3. " + GlobalTexts.TRADE);
        Debug.Log("4. " + GlobalTexts.OTHER);

    }

    public void DeclareWar()
    {
        Debug.Log("You have declared war on " + currentKingdom.name);
        currentKingdom.IncreaseMadness(20);
        // Implement war declaration logic here
    }

    public void RequestFreePass()
    {
        Debug.Log("You have requested a free pass from " + currentKingdom.name);
        currentKingdom.IncreaseMadness(5);
        // Implement free pass logic here
    }

    public void ProposeTrade()
    {
        Debug.Log("You have proposed a trade with " + currentKingdom.name);
        currentKingdom.IncreaseMadness(10);
        // Implement trade proposal logic here
    }

    public void OtherDiplomacyOptions()
    {
        Debug.Log("Exploring other diplomacy options with " + currentKingdom.name);
        currentKingdom.IncreaseMadness(15);
        // Implement other diplomacy options here
    }
}
