using System.Collections.Generic;
using UnityEngine;

public class CityProductionQueue : MonoBehaviour
{
    //public static CityProductionQueue Instance { get; private set; }

    public Production currentProduction { get; private set; } = null;
    public List<Production> productionQueue { get; private set; } = new List<Production>();

    private GridCity ownerCity;
    private BaseKingdom ownerKingdom;

    public void Initialize(BaseKingdom kingdom, GridCity city)
    {
        ownerCity = city;
        ownerKingdom = kingdom;
    }

    public void OnTurnEnd()
    {
        ProcessProduction();
    }
    public void ProcessProduction()
    {
        if (currentProduction == null)
        {
            Debug.LogWarning("No current production to process");
            return;
        }
        currentProduction.UpdateProduction();
        CheckIfProductionComplete();
        if(ownerKingdom is PlayerKingdom)
        {
            ProductionQueueUI.Instance.UpdateUI(this);
        }
        

    }

    private void CheckIfProductionComplete()
    {
        if (currentProduction.IsComplete())
        {

            currentProduction.EndProduction(GetComponent<GridCity>());
            RemoveProduction(currentProduction);
            //ProceedProductionQueue();
        }
    }


    public void SetCurrentProduction(Production production)
    {
        currentProduction = production;
        currentProduction.StartProduction(GetComponent<GridCity>());
        CheckIfProductionComplete();
    }

    public void ProceedProductionQueue()
    {
        if (productionQueue.Count == 0)
        {
            Debug.LogWarning("No production queue to process");
            return;
        }
        if (currentProduction != null)
        {
            Debug.LogWarning("Current Production cancelled and queue moved up");
        }
        Production production = productionQueue[0];
        productionQueue.RemoveAt(0);
        SetCurrentProduction(production);
    }

    /// <summary>
    /// Inserts a Production object to the end of the queue
    /// </summary>
    public void AddToQueue(Production production)
    {
        if (productionQueue == null)
        {
            productionQueue = new List<Production>();
        }
        productionQueue.Add(production);
        if (currentProduction == null)
        {
            ProceedProductionQueue();
        }
        if (ownerKingdom is PlayerKingdom)
        {
            ProductionQueueUI.Instance.UpdateUI(this);
        }

    }

    ///<summary>
    /// Removes a production from the queue. This is called from production Item UI.
    /// </summary>
    /// <param name="production">The production to remove</param>
    public void RemoveProduction(Production production)
    {
        if (production == null)
        {
            Debug.LogError("Cannot remove null Production");
            return;
        }
        production.Cancel(GetComponent<GridCity>());
        Destroy(production.placedObject);
        productionQueue.Remove(production);
        if (currentProduction == production)
        {
            RemoveCurrentProduction();
        }
        if (ownerKingdom is PlayerKingdom)
        {
            ProductionQueueUI.Instance.UpdateUI(this);
        }

    }

    /// <summary>
    /// Removes the current production
    /// </summary>
    public void RemoveCurrentProduction()
    {
        if (currentProduction == null)
        {
            Debug.LogWarning("No current production to remove");
            return;
        }
        currentProduction = null;

        ProceedProductionQueue();
    }

    /// <summary>
    /// Inserts a Production object at a particular position in the queue
    /// </summary>
    //public void InsertAtPosition(Production production, int position)
    //{
    //    if (production == null)
    //    {
    //        Debug.LogError("Cannot insert null Production to queue");
    //        return;
    //    }

    //    if (position < 0)
    //    {
    //        Debug.LogError($"Cannot insert at negative position: {position}");
    //        return;
    //    }

    //    if (position > productionQueue.Count)
    //    {
    //        Debug.LogError($"Position {position} is out of range. Queue size is {productionQueue.Count}");
    //        return;
    //    }

    //    productionQueue.Insert(position, production);

    //}

    /// <summary>
    /// Moves current production to a particular position in the queue.
    /// The first Production in the queue will become the current production.
    /// </summary>
    //public void MoveCurrentProductionToPosition(int position)
    //{
    //    if (currentProduction == null)
    //    {
    //        Debug.LogError("No current production to move");
    //        return;
    //    }

    //    if (productionQueue.Count == 0)
    //    {
    //        Debug.LogWarning("Cannot move current production to queue: queue is empty");
    //        return;
    //    }

    //    InsertAtPosition(currentProduction, position);

    //    // Make the first production in the queue the current production
    //    SetCurrentProduction(productionQueue[0]);
    //    RemoveFromQueue(0);
    //}

    /// <summary>
    /// Removes a production from the queue at the specified index
    /// </summary>
    //public void RemoveFromQueue(int index)
    //{
    //    if (index < 0)
    //    {
    //        Debug.LogError($"Cannot remove at negative index: {index}");
    //        return;
    //    }

    //    if (index >= productionQueue.Count)
    //    {
    //        Debug.LogError($"Index {index} is out of range. Queue size is {productionQueue.Count}");
    //        return;
    //    }

    //    productionQueue.RemoveAt(index);
    //}


    /// <summary>
    /// Rearranges the production queue by moving an item from initial position to final position
    /// </summary>
    //public void RearrangeQueue(int initialPosition, int finalPosition)
    //{
    //    if (initialPosition < 0)
    //    {
    //        Debug.LogError($"Cannot move from negative initial position: {initialPosition}");
    //        return;
    //    }

    //    if (initialPosition >= productionQueue.Count)
    //    {
    //        Debug.LogError($"Initial position {initialPosition} is out of range. Queue size is {productionQueue.Count}");
    //        return;
    //    }

    //    if (finalPosition < 0)
    //    {
    //        Debug.LogError($"Cannot move to negative final position: {finalPosition}");
    //        return;
    //    }

    //    if (finalPosition >= productionQueue.Count)
    //    {
    //        Debug.LogError($"Final position {finalPosition} is out of range. Queue size is {productionQueue.Count}");
    //        return;
    //    }

    //    if (initialPosition == finalPosition)
    //    {
    //        Debug.LogWarning($"Initial and final positions are the same ({initialPosition}). No action taken.");
    //        return;
    //    }

    //    Production productionToMove = productionQueue[initialPosition];
    //    RemoveFromQueue(initialPosition);
    //    InsertAtPosition(productionToMove, finalPosition);
    //}


}

