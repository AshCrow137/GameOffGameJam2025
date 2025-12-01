using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;


public class AIController : MonoBehaviour
{
    public static AIController Instance { get; private set; }
    private Dictionary<BaseGridUnitScript, AIUnitTask> unitsToAct;
    private Dictionary<GridCity, AICityTask> citiesToAct;
    CancellationTokenSource cancellationTokenSource;
    private List<Vector3Int> reservedPositions;
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
            
            
            int attempt = 0;
            int actionCount = 0;
            #region unitTasks
            while (attempt<=10)
            {
                attempt++;
                List<BaseGridUnitScript> kingdomUnits = new List<BaseGridUnitScript>(kingdom.ControlledUnits);
                if (attempt>=9)
                {

                }
                
                unitsToAct = new Dictionary<BaseGridUnitScript, AIUnitTask>();
                reservedPositions = new List<Vector3Int>();
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
                    foreach(KeyValuePair<GridCity,AICityTask> pair in citiesToAct)
                    {
                        GridCity city = pair.Key;
                        AICityTask cityTask = pair.Value;
                        switch(cityTask.Action)
                        {
                            case AICityAction.BuildProductionBuilding:
                                
                                GridBuilding buildingToPlace = cityTask.taskTarget as GridBuilding;
                                if (!buildingToPlace) break;
                                List<Vector3Int> adjPos = HexTilemapManager.Instance.GetCellsInRange(city.GetCellPosition(), 1, new List<TileState>() { TileState.Land });
                                if (adjPos.Count > 1 && CheckResources(kingdom, buildingToPlace.GetBuilding().resource))
                                {
                                    Vector3Int randPos = adjPos[UnityEngine.Random.Range(0, adjPos.Count - 1)];
                                    BuildingManager.Instance.QueueBuildingAtPosition(randPos, city, buildingToPlace);
                                }
                                break;
                            case AICityAction.BuildUnitTechBuilding:
                                
                                GridBuilding techbuildingToPlace = cityTask.taskTarget as GridBuilding;
                                if (!techbuildingToPlace) break;
                                List<Vector3Int> adjPoses = HexTilemapManager.Instance.GetCellsInRange(city.GetCellPosition(), 1, new List<TileState>() { TileState.Land });
                                if (adjPoses.Count > 1 && CheckResources(kingdom, techbuildingToPlace.GetBuilding().resource))
                                {
                                    Vector3Int randPos = adjPoses[UnityEngine.Random.Range(0, adjPoses.Count - 1)];
                                    BuildingManager.Instance.QueueBuildingAtPosition(randPos, city, techbuildingToPlace);
                                }
                                break;
                            case AICityAction.BuildMeleeUnit:
                                BaseGridUnitScript buildUnit = cityTask.taskTarget as BaseGridUnitScript;
                                if (!buildUnit) break;
                                List<Vector3Int> unitAdjPoses = HexTilemapManager.Instance.GetCellsInRange(city.GetCellPosition(), 1, buildUnit.GetPossibleSpawnTiles());
                                if (unitAdjPoses.Count > 0 && CheckResources(kingdom, buildUnit.resource))
                                {
                                    Vector3Int randPos = unitAdjPoses[UnityEngine.Random.Range(0, unitAdjPoses.Count - 1)];
                                    UnitSpawner.Instance.QueueUnitAtPosition(randPos, city, buildUnit);
                                }
                                break;
                        }
                    }
                }
                else
                {
                    break;
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

        token.CancelAfter(TimeSpan.FromSeconds(3));
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
                if(potentialTarget&&potentialTarget.GetOwner()!=kingdom&&potentialTarget.GetComponent<IDamageable>()!=null&&potentialTarget.CanBeActtacked())
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
        List<BaseGridEntity> targets = new List<BaseGridEntity>();
        if(kingdom.GetCurrentMadnessEffect().CanFight)
        {
            targets = CheckForTargets(unit, notFogTiles, kingdom);
        }
         
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
                            foreach(Vector3Int pos in closestAdjPos)
                            {
                                if(!reservedPositions.Contains(pos))
                                {
                                    reservedPositions.Add(pos);
                                    targetPos = pos;
                                    unitsToAct.Add(unit, new AIUnitTask(AIUnitAction.Move, targetPos));
                                    return AIUnitAction.Move;
                                }
                            }
                            foreach(Vector3Int pos in adjPos)
                            {
                                if (!reservedPositions.Contains(pos))
                                {
                                    reservedPositions.Add(pos);
                                    targetPos = pos;
                                    unitsToAct.Add(unit, new AIUnitTask(AIUnitAction.Move, targetPos));
                                    return AIUnitAction.Move;
                                }
                            }
                            
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
                            foreach (Vector3Int pos in closestAdjPos)
                            {
                                if (!reservedPositions.Contains(pos))
                                {
                                    reservedPositions.Add(pos);
                                    targetPos = pos;
                                    unitsToAct.Add(unit, new AIUnitTask(AIUnitAction.Move, targetPos));
                                    return AIUnitAction.Move;
                                }
                            }
                            foreach (Vector3Int pos in adjPos)
                            {
                                if (!reservedPositions.Contains(pos))
                                {
                                    reservedPositions.Add(pos);
                                    targetPos = pos;
                                    unitsToAct.Add(unit, new AIUnitTask(AIUnitAction.Move, targetPos));
                                    return AIUnitAction.Move;
                                }
                            }
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
        //if(kingdom.HasPMS )
        //{
        //    return AICityAction.None;
        //}
        Resource kingdomResources = kingdom.Resources();
        List<GridBuilding> unlockedBuildings = kingdom.GetUnlockedBuildings();
        List<BaseGridUnitScript> unlockedUnits = kingdom.GetunlockedUnits();
        Dictionary<Vector3Int,GridBuilding> cityBuildings = new Dictionary<Vector3Int, GridBuilding>(city.buildings);
        List<FabricResourses>  productionBuildings = new List<FabricResourses>();
        CityProductionQueue productionQueue = city.GetComponent<CityProductionQueue>();
        List<Production> cityProduction = productionQueue.productionQueue;
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
            //Build economic buildings
            if (productionBuildings.Count < 2)
            {
                FabricResourses mainProductionFabric = GetUnlockedFabric(kingdom, unlockedBuildings);
                if (mainProductionFabric != null)
                {
                    List<Production> totalProduction = new List<Production>(cityProduction);
                    if (productionQueue.currentProduction != null)
                    {
                        totalProduction.Add(productionQueue.currentProduction);
                    }
                    int totalProductionBuilding = productionBuildings.Count;
                    foreach (Production prod in totalProduction)
                    {
                        if (prod.building?.buildingPrefab.GetComponent<FabricResourses>())
                        {
                            totalProductionBuilding++;
                        }
                    }
                    if (totalProductionBuilding < 2 && CheckResources(kingdom, mainProductionFabric.GetBuilding().resource))
                    {
                        citiesToAct.Add(city, new AICityTask(AICityAction.BuildProductionBuilding, mainProductionFabric));
                        return AICityAction.BuildProductionBuilding;
                    }
                }
            }
            List<BaseGridUnitScript> notUnlockedUnits = kingdom.GetNotUnlockedUnits();
            //build tech buildings
            if(notUnlockedUnits.Count > 0)
            {
                Debug.Log(notUnlockedUnits);
                List<OpenNewUnit> techBuildings = new List<OpenNewUnit>();
                foreach(GridBuilding techBuilding in unlockedBuildings)
                {
                    if(techBuilding is OpenNewUnit)
                    {
                        techBuildings.Add((OpenNewUnit)techBuilding);
                    }
                }
                if(techBuildings.Count > 0)
                {
                    foreach(OpenNewUnit tBuilding in techBuildings)
                    {
                        if(notUnlockedUnits.Contains(tBuilding.GetUnitToOpen()))
                        {
                            bool bHasSimilarBuildingInProduction = false;
                            if (cityProduction.Count > 0||productionQueue.currentProduction!=null)
                            {
                                if(productionQueue.currentProduction!=null&&productionQueue.currentProduction.building?.buildingPrefab==tBuilding.gameObject)
                                {
                                    bHasSimilarBuildingInProduction = true;
                                }
                                foreach (Production production in cityProduction)
                                {
                                    if (tBuilding != null && production.building?.buildingPrefab == tBuilding.gameObject)
                                    {
                                        bHasSimilarBuildingInProduction = true;
                                        break;
                                    }
                                }

                            }
                            if(!bHasSimilarBuildingInProduction&&CheckResources(kingdom,tBuilding.GetBuilding().resource))
                            {
                                citiesToAct.Add(city, new AICityTask(AICityAction.BuildUnitTechBuilding, tBuilding));
                                return AICityAction.BuildUnitTechBuilding;
                            }
                        }
                    }
                }
            }
            //build units
            List<BaseGridUnitScript> potentialUnits = new List<BaseGridUnitScript>();
            int highestTier = 0;
            foreach (BaseGridUnitScript unlockedUnit in unlockedUnits)
            {
                if (!CheckResources(kingdom, unlockedUnit.resource)) continue;

                if(unlockedUnit.GetTier()>highestTier)
                {
                    potentialUnits.Clear();
                    potentialUnits.Add(unlockedUnit);
                    highestTier = unlockedUnit.GetTier();
                }
                else if(unlockedUnit.GetTier()==highestTier)
                {
                    potentialUnits.Add(unlockedUnit);
                } 
            }
            if(potentialUnits.Count>0)
            {
                BaseGridUnitScript potentialUnit = potentialUnits[UnityEngine.Random.Range(0, potentialUnits.Count)];
                List<Production> totalProduction = new List<Production>(cityProduction);
                if (productionQueue.currentProduction != null)
                {
                    totalProduction.Add(productionQueue.currentProduction);
                }
                int similarUnitsInQueue = 0;
                foreach (Production production in totalProduction)
                {
                    if(production.prefab==potentialUnit.gameObject)
                    {
                        similarUnitsInQueue++;
                    }
                }

                if (potentialUnit != null&& CheckResources(kingdom, potentialUnit.resource)&&similarUnitsInQueue<=2)
                {
                    citiesToAct.Add(city, new AICityTask(AICityAction.BuildMeleeUnit, potentialUnit));
                    return AICityAction.BuildMeleeUnit;
                }
            }
        }
        else
        {

            FabricResourses mainProductionFabric = GetUnlockedFabric(kingdom, unlockedBuildings);
            bool bHasSimilarBuildingInProduction = false;

            if(productionQueue.currentProduction !=null&& productionQueue.currentProduction.building?.buildingPrefab== mainProductionFabric.gameObject)
            {
                    bHasSimilarBuildingInProduction = true;
            }
            else
            {
                if (cityProduction.Count > 0)
                {
                    foreach (Production production in cityProduction)
                    {
                        if (mainProductionFabric!=null&&production.building.buildingPrefab == mainProductionFabric.gameObject)
                        {
                            bHasSimilarBuildingInProduction = true;
                            break;
                        }
                    }
                }
                
            }
            if (mainProductionFabric != null && CheckResources(kingdom, mainProductionFabric.GetBuilding().resource)&&!bHasSimilarBuildingInProduction)
            {
                citiesToAct.Add(city, new AICityTask(AICityAction.BuildProductionBuilding, mainProductionFabric));
                return AICityAction.BuildProductionBuilding;
            }
        }
        return AICityAction.None;
    }
    private FabricResourses GetUnlockedFabric(AIKingdom kingdom,List<GridBuilding> unlockedBuildings)
    {
        List<FabricResourses> unlockedProductionBuildings = new List<FabricResourses>();
        foreach (GridBuilding unlockedBuilding in unlockedBuildings)
        {
            if (unlockedBuilding is FabricResourses)
            {
                unlockedProductionBuildings.Add((FabricResourses)unlockedBuilding);
            }
        }
        foreach (FabricResourses fabric in unlockedProductionBuildings)
        {
            foreach (KeyValuePair<ResourceType, int> pair in fabric.GetProduction())
            {
                if (pair.Key == kingdom.GetMainResourceType() && pair.Value > 0)
                {
                    return fabric;
                }
            }
        }
        return null;
    }
    private bool CheckResources(AIKingdom kingdom, Dictionary<ResourceType, int> required)
    {
        if (kingdom.Resources().HasEnough(required) == null) 
        {
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
        public BaseGridEntity taskTarget;

        public AICityTask(AICityAction action, BaseGridEntity taskTarget)
        {
            this.Action = action;
            Finished = false;
            this.taskTarget = taskTarget;
        }

    }


    
    
}
