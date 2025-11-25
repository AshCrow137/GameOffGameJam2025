using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.GridLayoutGroup;

public class AIController : MonoBehaviour
{
    public static AIController Instance { get; private set; }
    private Dictionary<BaseGridUnitScript, AIUnitTask> unitsToAct;
    private Dictionary<GridCity, AICityTask> citiesToAct;
    CancellationTokenSource cancellationTokenSource;
    public void Initialize()
    {
        Instance = this;

    }

    /// <summary>
    /// Make turn for current bot
    /// </summary>
    /// <param name="kingdom"></param>'
    public bool ExecuteTurn(AIKingdom kingdom)
    {
        VisionManager kingdomVision = kingdom.GetComponent<VisionManager>();
        //foreach(KeyValuePair<Vector3Int, bool> visionPair in kingdomVision.notBlackFog)
        //{
        //    Vector3Int pos = visionPair.Key;
        //    bool value = visionPair.Value;
        //    if (value)
        //    {
        //        HexTilemapManager.Instance.PlaceColoredMarkerOnPosition(pos, MarkerColor.Blue);
        //    }
        //}
        HexTilemapManager.Instance.RemoveAllMarkers();
        //StartCoroutine(TurnCoroutine(kingdom));
        ExecuteTurnAsync(kingdom);
        return true;
    }
    //unit without action if tilesREmain<=0 && (attackCount==0 || no enemies in range)
    //private IEnumerator TurnCoroutine(AIKingdom kingdom)
    //{
    //    List<BaseGridUnitScript> kingdomUnits = new List<BaseGridUnitScript>(kingdom.ControlledUnits);
    //    while ( true)
    //    {
    //        unitsToAct = new Dictionary<BaseGridUnitScript, AIUnitTask>();
    //        foreach (BaseGridUnitScript unit in kingdomUnits)
    //        {
    //            AIUnitAction unitAction = AssignAction(unit);
                
    //        }
    //        if(unitsToAct.Count <=0) 
    //        {
    //            break;
    //        }
    //        List<Action> UnitActions = new List<Action>();
    //        int actionCount = 0;
    //        foreach(KeyValuePair<BaseGridUnitScript,AIUnitTask> pair in  unitsToAct)
    //        {
    //            BaseGridUnitScript unit = pair.Key;
    //            AIUnitTask unitTask = pair.Value;
    //            AIUnitAction unitAction = unitTask.Action;
    //            switch (unitAction)
    //            {
    //                case AIUnitAction.Explore:
    //                    List<Vector3Int> fogTiles = new List<Vector3Int>();
    //                    VisionManager ownerVision = kingdom.GetComponent<VisionManager>();
    //                    for (int i = 1; i < unit.GetVision()+unit.GetMovementDistance(); i++)
    //                    {
    //                        List<Vector3Int> allCellsOnDistance = HexTilemapManager.Instance.GetAllCellsOnDistance(unit.GetCellPosition(), unit.GetVision() + i);
                          

    //                        foreach (Vector3Int pos in allCellsOnDistance)
    //                        {
    //                            if (HexTilemapManager.Instance.GetHexTile(pos) && !ownerVision.notBlackFog.TryGetValue(pos, out bool value) && HexTilemapManager.Instance.GetTileState(pos) == TileState.Land) fogTiles.Add(pos);
    //                        }
    //                        if (fogTiles.Count > 0) break;
    //                    }
    //                    if (fogTiles.Count <= 0)
    //                    {
    //                        Dictionary<Vector3Int, bool> fogTilesDict = ownerVision.notBlackFog;
    //                        fogTiles = new List<Vector3Int>();
    //                        foreach (KeyValuePair<Vector3Int,bool> fpair in fogTilesDict)
    //                        {
    //                            Vector3Int pos = fpair.Key;
    //                            bool inFog = fpair.Value;
    //                            if (!inFog && HexTilemapManager.Instance.GetTileState(pos) == TileState.Land) fogTiles.Add(pos);
    //                        }
    //                        if (fogTiles.Count <= 0)//TODO no more fog tiles on map 
    //                        {
    //                            unit.BanExploring();
    //                            break;
    //                        }
    //                        int dist = 0;
    //                        List<Vector3Int> tempFogTiles = new List<Vector3Int>(fogTiles);
    //                        foreach(Vector3Int tempPos in fogTiles)
    //                        {
    //                            int distDelta = HexTilemapManager.Instance.GetDistanceInCells(unit.GetCellPosition(), tempPos);
    //                            if (distDelta>dist)
    //                            {
    //                                dist = distDelta;
    //                                tempFogTiles.Clear();
    //                                tempFogTiles.Add(tempPos);
    //                            }
    //                            else if(distDelta==dist)
    //                            {
    //                                tempFogTiles.Add(tempPos);
    //                            }
    //                        }
    //                        fogTiles = new List <Vector3Int>(tempFogTiles);
    //                    }
    //                    foreach (Vector3Int pos in fogTiles)
    //                    {
    //                        TileState state = HexTilemapManager.Instance.GetTileState(pos);
    //                        HexTilemapManager.Instance.PlaceColoredMarkerOnPosition(pos, MarkerColor.Blue);
    //                    }
    //                    Vector3Int randomTile = fogTiles[UnityEngine.Random.Range(0, fogTiles.Count - 1)];

    //                    Debug.Log("Try to move unit");
    //                    if(unit.TryToMoveUnitToTile(randomTile))
    //                    {
    //                        //HexTilemapManager.Instance.PlaceColoredMarkerOnPosition(randomTile, MarkerColor.Green);
    //                        Action action = () => actionCount--;
    //                        actionCount++;
    //                        unit.MovementFinishEvent.AddListener(action.Invoke);
    //                        UnitActions.Add(action);
    //                    }
    //                    //yield return new WaitUntil(() => unitTask.Finished);
    //                    //unit.MovementFinishEvent.RemoveListener(action.Invoke);
    //                    //List<Vector3Int> adjPos = HexTilemapManager.Instance.GetCellsInRange(unit.GetCellPosition(), 1, new List<TileState> { TileState.Land });
    //                    //if (adjPos.Count <= 0) break;
    //                    //Vector3Int randomPos = adjPos[Random.Range(0,adjPos.Count-1)];
    //                    //unit.TryToMoveUnitToTile(randomPos);
    //                    break;
    //                case AIUnitAction.None:
    //                    continue;
    //            }

    //        }
    //        yield return new WaitUntil(() => actionCount <= 0);
    //        foreach (KeyValuePair<BaseGridUnitScript, AIUnitTask> pair in unitsToAct)
    //        {
    //            BaseGridUnitScript unit = pair.Key;
    //            unit.MovementFinishEvent.RemoveAllListeners();
    //        }
    //            yield return null;
    //    }
    //    kingdom.EndTurn();
    //    StopAllCoroutines();
    //}
    private async void ExecuteTurnAsync(AIKingdom kingdom)
    {
        cancellationTokenSource = new CancellationTokenSource();
        try
        {
            
            List<BaseGridUnitScript> kingdomUnits = new List<BaseGridUnitScript>(kingdom.ControlledUnits);
            int attempt = 0;
            int actionCount = 0;
            #region unitTasks
            while (attempt<=10)
            {
                attempt++;
                if(attempt>=9)
                {

                }
                
                unitsToAct = new Dictionary<BaseGridUnitScript, AIUnitTask>();
                foreach (BaseGridUnitScript unit in kingdomUnits)
                {
                    AIUnitAction unitAction = AssignAction(unit, kingdom);
                    
                }
                if (unitsToAct.Count <= 0)
                {
                    break;
                }
                List<Action> UnitActions = new List<Action>();
                actionCount = 0;

                foreach (KeyValuePair<BaseGridUnitScript, AIUnitTask> pair in unitsToAct)
                {
                    BaseGridUnitScript unit = pair.Key;
                    AIUnitTask unitTask = pair.Value;
                    AIUnitAction unitAction = unitTask.Action;
                    Action action = () => actionCount--;unitTask.Finished = true;
                    Debug.Log($"{unit} action: {unitAction}");
                    switch (unitAction)
                    {
                        case AIUnitAction.Explore:

                            //get all unexplored tiles, if had at least one, move to that tile. If more than one, choose random tile within same closest distance
                            Debug.Log("Try to move unit");
                            if (unit.TryToMoveUnitToTile(unitTask.Target))
                            {
                                actionCount++;
                                unit.MovementFinishEvent.AddListener(action.Invoke);
                                UnitActions.Add(action);


                            }
                            break;
                        case AIUnitAction.Attack:
                            if (!HexTilemapManager.Instance.GetEntityOnCell(unitTask.Target)) break;
                            if (unit.TryToAttack(HexTilemapManager.Instance.GetEntityOnCell(unitTask.Target), unitTask.Target))
                            {
                                actionCount++;
                                unit.AttackFinishEvent.AddListener(action.Invoke);
 

                            }

                            break;
                        case AIUnitAction.Move:
                            if (unit.TryToMoveUnitToTile(unitTask.Target))
                            {
 
                                actionCount++;
                                unit.MovementFinishEvent.AddListener(action.Invoke);
                                UnitActions.Add(action);
                                //}
                            }
                            break;
                        case AIUnitAction.None:
                            continue;
                    }

                }
                if (await WaitUntil(() => actionCount <= 0, cancellationTokenSource))
                //if (await WaitUntil(CheckForEndTurn, cancellationTokenSource))
                {
                    
                    foreach (KeyValuePair<BaseGridUnitScript, AIUnitTask> pair in unitsToAct)
                    {
                        BaseGridUnitScript unit = pair.Key;
                        unit.MovementFinishEvent.RemoveAllListeners();
                        unit.AttackFinishEvent.RemoveAllListeners();
                    }
                }
                else
                {
                    Debug.Log(actionCount);
                    foreach (KeyValuePair<BaseGridUnitScript, AIUnitTask> pair in unitsToAct)
                    {
                        if (!pair.Value.Finished)
                        {
                            Debug.Log($"{pair.Key.name} don't finish task {pair.Value.Action}");
                        }
                    }
                    break;
                }
               

            }
            #endregion

            if(attempt>=10)
            {
                Debug.LogError("Too many attempts");
            }
            attempt = 0;
            #region macroTasks
            while (attempt<=10)
            {
                attempt++;
                List<GridCity> controlledCities = new List<GridCity>(kingdom.GetControlledCities());
                citiesToAct = new Dictionary<GridCity, AICityTask>();
                foreach (GridCity city in controlledCities)
                {
                    AICityAction cityAction = AssignCityAction(city, kingdom);
                }
                if (citiesToAct.Count > 0)
                {

                }
                
            }
            #endregion
            kingdom.EndTurn();
            cancellationTokenSource.Cancel();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            cancellationTokenSource.Cancel();
        }
        finally { 
            cancellationTokenSource.Dispose(); 
            cancellationTokenSource = null;
        }
    }

    private bool CheckForEndTurn()
    {
        foreach (KeyValuePair<BaseGridUnitScript, AIUnitTask> pair in unitsToAct)
        {
            if (!pair.Value.Finished)
            {
                return false;
            }
        }
        return true;
    }

    //private bool CheckForEndTurn(KeyValuePair<BaseGridUnitScript, AIUnitTask> units)
    //{
    //    foreach (KeyValuePair<BaseGridUnitScript, AIUnitTask> pair in unitsToAct)
    //    {
    //        if(!pair.Value.Finished)
    //        {
    //            return false;
    //        }
    //    }
    //    return true;
    //}
    public static async Task<bool> WaitUntil(Func<bool> predicate, CancellationTokenSource token, int sleep = 50)
    {

        token.CancelAfter(TimeSpan.FromSeconds(5));
        float t = 0;
        try
        {
            while (!predicate())
            {
                t += sleep;
                await Task.Delay(sleep, token.Token);

            }
            return true;
        }
        catch (TaskCanceledException) 
        {
            Debug.Log($"wait time {t}");
            return false;
        }


    }
    private List<BaseGridEntity> CheckForTargets(BaseGridUnitScript unit, List<Vector3Int> kingdomVision,AIKingdom kingdom)
    {
        List<BaseGridEntity> potentialTargets = new List<BaseGridEntity>();
        foreach(Vector3Int pos in kingdomVision)
        {
            if(HexTilemapManager.Instance.GetTileState(pos)==TileState.OccupiedByUnit|| HexTilemapManager.Instance.GetTileState(pos) == TileState.OccuppiedByBuilding)
            {
                BaseGridEntity potentialTarget = HexTilemapManager.Instance.GetEntityOnCell(pos);
                if(potentialTarget&&potentialTarget.GetOwner()!=kingdom&&potentialTarget.GetComponent<IDamageable>()!=null)
                {
                    potentialTargets.Add(potentialTarget);
                }
            }
        }
        return potentialTargets;
    }
    private AIUnitAction AssignAction(BaseGridUnitScript unit, AIKingdom kingdom)
    {
        List<Vector3Int> fogTiles = new List<Vector3Int>();
        List<Vector3Int> notFogTiles = new List<Vector3Int>();
        VisionManager ownerVision = kingdom.GetComponent<VisionManager>();
        Dictionary<Vector3Int, bool> fogTilesDict = ownerVision.notBlackFog;
        foreach (KeyValuePair<Vector3Int, bool> fpair in fogTilesDict)
        {
            Vector3Int pos = fpair.Key;
            bool inFog = fpair.Value;
            if (!inFog && unit.GetWalkableTiles().Contains(HexTilemapManager.Instance.GetTileState(pos))) fogTiles.Add(pos);
            else if(inFog)
            {
                notFogTiles.Add(pos);
                //HexTilemapManager.Instance.PlaceColoredMarkerOnPosition(pos, MarkerColor.Blue);
            }
        }
        //Attack 
        List<BaseGridEntity> targets = CheckForTargets(unit, notFogTiles, kingdom);
        if(targets.Count > 0)
        {
            List<Vector3Int> potentialTargetPositions = new List<Vector3Int>();
            Dictionary<BaseGridEntity,AIAttackWeight> potentialTargetWithWeight = new Dictionary<BaseGridEntity, AIAttackWeight>();
            foreach (BaseGridEntity target in targets)
            {

                int distanceBetween = HexTilemapManager.Instance.GetDistanceInCells(unit.GetCellPosition(), target.GetCellPosition());
                if (distanceBetween <= unit.GetAtackDistance()&&unit.GetFinalDamageWithModifiers(unit,target)>=target.GetCurrentHealth()) 
                {
                    potentialTargetWithWeight.Add(target,new AIAttackWeight(6,false));
                }
                else if (distanceBetween <= unit.GetAtackDistance()+unit.GetMovementDistance() && unit.GetFinalDamageWithModifiers(unit, target) >= target.GetCurrentHealth()&&unit.tilesRemain>0)
                {
                    potentialTargetWithWeight.Add(target, new AIAttackWeight(5, true));
                }
                //TODO SpecialAbility
                else if(distanceBetween <= unit.GetAtackDistance()&&target.entityType==unit.getVulnerableEntityType(unit.entityType))
                {
                    if(target is BaseGridUnitScript)
                    {
                        BaseGridUnitScript unitTarget = (BaseGridUnitScript)target; 
                        if(unit.GetCurrentHealth() > unitTarget.GetRetaliationDamage() ) 
                        {
                            potentialTargetWithWeight.Add(target, new AIAttackWeight(4, false));
                        }
                    }
                   
                }
                //TODO else if target close than 6 tiles from one of kingdom cities
                else if (distanceBetween <= unit.GetAtackDistance() + unit.GetMovementDistance() && target.entityType == unit.getVulnerableEntityType(unit.entityType)&&unit.tilesRemain>0)
                {
                    if (target is BaseGridUnitScript)
                    {
                        BaseGridUnitScript unitTarget = (BaseGridUnitScript)target;
                        if (unit.GetCurrentHealth() > unitTarget.GetRetaliationDamage())
                        {
                            potentialTargetWithWeight.Add(target, new AIAttackWeight(3, true));
                        }
                    }
                }
                else if(distanceBetween <= unit.GetAtackDistance())
                {
                    if (target is BaseGridUnitScript)
                    {
                        BaseGridUnitScript unitTarget = (BaseGridUnitScript)target;
                        if (unit.GetCurrentHealth() > unitTarget.GetRetaliationDamage()||unit.GetAtackDistance()>1)
                        {
                            potentialTargetWithWeight.Add(target, new AIAttackWeight(2, false));
                        }
                    }
                    else
                    {
                        potentialTargetWithWeight.Add(target, new AIAttackWeight(2, false));
                    }
                }
                else if(distanceBetween <= unit.GetAtackDistance() + unit.GetMovementDistance()&&unit.tilesRemain>0)
                {
                    if (target is BaseGridUnitScript)
                    {
                        BaseGridUnitScript unitTarget = (BaseGridUnitScript)target;
                        if (unit.GetCurrentHealth() > unitTarget.GetRetaliationDamage() || unit.GetAtackDistance() > 1)
                        {
                            potentialTargetWithWeight.Add(target, new AIAttackWeight(1, true));
                        }
                    }
                    else
                    {
                        potentialTargetWithWeight.Add(target, new AIAttackWeight(1, true));
                    }
                }

                if (!potentialTargetWithWeight.TryGetValue(target,out AIAttackWeight value))
                {
                    potentialTargetPositions.Add(target.GetCellPosition());
                }
    
            }
            Vector3Int targetPos = new Vector3Int();
            if (potentialTargetWithWeight.Count> 0)
            {
                //select target by weight
                int weight = 0;
                BaseGridEntity potentialTarget = null;
                bool bNeedToMove = false;
                foreach (KeyValuePair<BaseGridEntity,AIAttackWeight> pair in potentialTargetWithWeight)
                {
                    BaseGridEntity entity = pair.Key;
                    AIAttackWeight targetWeight = pair.Value;
                    if(targetWeight.Weight>weight)
                    {
                        weight = targetWeight.Weight;
                        potentialTarget = entity;
                        bNeedToMove = targetWeight.bNeedToMove;
                    }
                }
                if(potentialTarget)
                {
                    targetPos = potentialTarget.GetCellPosition();
                    if(bNeedToMove)
                    {
                        List<Vector3Int> adjPos = HexTilemapManager.Instance.GetCellsInRange(targetPos, unit.GetAtackDistance(), unit.GetWalkableTiles());
                        List<Vector3Int> closestAdjPos = HexTilemapManager.Instance.GetClosestTiles(unit.GetCellPosition(), adjPos);
                        if (closestAdjPos.Count > 0)
                        {
                            targetPos = closestAdjPos[0];
                            unitsToAct.Add(unit, new AIUnitTask(AIUnitAction.Move, targetPos));
                            return AIUnitAction.Move;
                        }
                    }
                    else
                    {
                        if(unit.GetRemainAttacksCount()>0)
                        {
                            unitsToAct.Add(unit, new AIUnitTask(AIUnitAction.Attack, targetPos));
                            return AIUnitAction.Attack;
                        }

                    }
                    
                }

            }
            else
            {
                //secelt closest visible enemy unit
                if(potentialTargetPositions.Count> 0)
                {
                    List<Vector3Int> closestTargetPoses = HexTilemapManager.Instance.GetClosestTiles(unit.GetCellPosition(), potentialTargetPositions);
                    if (closestTargetPoses.Count > 0&& unit.tilesRemain > 0)
                    {
                        targetPos = closestTargetPoses[0];
                        List<Vector3Int> adjPos = HexTilemapManager.Instance.GetCellsInRange(targetPos, 1, unit.GetWalkableTiles());
                        List<Vector3Int> closestAdjPos = HexTilemapManager.Instance.GetClosestTiles(unit.GetCellPosition(), adjPos);
                        if(closestAdjPos.Count>0)
                        {
                            targetPos = closestAdjPos[0];
                            unitsToAct.Add(unit, new AIUnitTask(AIUnitAction.Move, targetPos));
                            return AIUnitAction.Move;
                        }
                        
                    }
                    
                }
                
            }
            
        }

        //Exploration
        if (unit.tilesRemain> 0&&fogTiles.Count > 0)
        {



                fogTiles = HexTilemapManager.Instance.GetClosestTiles(unit.GetCellPosition(), fogTiles);


                Vector3Int randomTile = fogTiles[UnityEngine.Random.Range(0, fogTiles.Count - 1)];
                unitsToAct.Add(unit, new AIUnitTask(AIUnitAction.Explore,randomTile));
                return AIUnitAction.Explore;

           
        }
        return AIUnitAction.None;
        //1) check for attack target
        //2) check for special ability
        // 3) check if available to move
        // 4) no more available actions
    }
    private AICityAction AssignCityAction(GridCity city, AIKingdom kingdom)
    {
        Resource kingdomResources = kingdom.Resources();
        List<GridBuilding> unlockedBuildings = kingdom.GetUnlockedBuildings();
        List<BaseGridUnitScript> unlockedUnits = kingdom.GetunlockedUnits();
        Dictionary<Vector3Int,GridBuilding> cityBuildings = new Dictionary<Vector3Int, GridBuilding>(city.buildings);
        List<FabricResourses>  productionBuildings = new List<FabricResourses>();
        if (cityBuildings.Count > 0)
        {
            foreach(KeyValuePair<Vector3Int,GridBuilding> pair in cityBuildings)
            {
                Vector3Int pos = pair.Key;
                GridBuilding building = pair.Value;
                if(building is FabricResourses)
                {
                    productionBuildings.Add(building as FabricResourses);
                }
            }

        }
        return AICityAction.None;
    }
    private bool CheckResources(AIKingdom kingdom, Dictionary<ResourceType, int> required)
    {
        if (kingdom.Resources().HasEnough(required) != null) // seem to be right, right?
        {
            kingdom.Resources().SpendResource(required);
            return true;
        }
        return false;
    }

    private void OnDestroy()
    {
        cancellationTokenSource?.Cancel();
    }

    private class AIAttackWeight
    {
        public int Weight;
        public bool bNeedToMove;
        public AIAttackWeight(int weight, bool bNeedToMove)
        {
            Weight = weight;
            this.bNeedToMove = bNeedToMove;
        }   
    }
    private class AIUnitTask
    {
        public bool Finished;
        public AIUnitAction Action;
        public Vector3Int Target;
        public AIUnitTask(AIUnitAction action, Vector3Int target)
        {
            this.Action = action;
            Finished = false;
            Target = target;
        }
    }
   private class AICityTask
    {
        public bool Finished;
        public AICityAction Action;
        public AICityTask(AICityAction action)
        {
            this.Action = action;
            Finished= false;
        }

    }


    
    
}
