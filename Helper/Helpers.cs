using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using robotManager.FiniteStateMachine;
using robotManager.Helpful;
using robotManager.Products;
using WholesomeDungeons.Bot;
using wManager;
using wManager.Wow;
using wManager.Wow.Bot.Tasks;
using wManager.Wow.Enums;
using wManager.Wow.Helpers;
using wManager.Wow.ObjectManager;
using static WholesomeDungeons.Bot.WholesomeDungeonsSettings;
using Math = System.Math;
//using Move = SmoothMove.Move;

namespace WholesomeDungeons.Helper {
    internal static class Helpers {
        private static Task _currentMovementTask;
        private static CancellationTokenSource _currentMovementToken;
        private static Vector3 _currentMovementTarget;
        private static readonly object Lock = new object();
        public static List<Vector3>CurrentMovementPath { get; private set; }

        public static bool IsMovementThreadRunning
        {
            get
            {
                //Check if actual Movement Thread is running and gives back boolean
                lock (Lock)
                {
                    return !_currentMovementTask?.Finished() ?? false;
                }
            }
        }

        public static Vector3 CurrentMovementTarget
        {
            get
            {
                //Gives back the Current Movment Target as Vector3
                lock (Lock)
                {
                    return IsMovementThreadRunning ? _currentMovementTarget : null;
                }
            }
        }

        public static void StopAllMove()
        {
            //stops all movement, regardless of using smoothmove or the default movement  Engine
            StopCurrentMovementThread();
            MovementManager.StopMoveNewThread();
            MovementManager.StopMove();
        }

        public static void StopCurrentMovementThread()
        {
            try
            {
                lock (Lock)
                {
                    //Stops the current Movement  Thread, according t the CancellationToken
                    _currentMovementToken?.Cancel();
                    if (!_currentMovementTask?.Wait(5000, _currentMovementToken?.Token ?? CancellationToken.None) ??
                        false)
                        Logger.Log("Unable to end movement thread.");
                    // ResetCurrentMovementCache();
                }
            }
            catch
            {
                // We can safely ignore this
            }
        }
        private static void ResetCurrentMovementCache()
        {
            //resets the complete Movement Cache inside a Framelock
            lock (Lock)
            {
                _currentMovementTask = null;
                _currentMovementToken = null;
                _currentMovementTarget = null;
            }
        }

        //Starts a new GoToThread
        public static CancellationTokenSource StartGoToThread(Vector3 target,
            bool face = true, bool precise = false, Func<bool> abortIf = null, float randomizeEnd = 0,
            float randomization = 0, bool checkCurrent = true, float precision = 1,
            bool shortCut = false, int jumpRareness = 2, bool showPath = false)
        {
            //cancel old movmement when firing a new GoToThread
            if (MovementManager.InMovement)
            {
                MovementManager.StopMove();
            }


            lock (Lock)
            {
                if (IsMovementThreadRunning)
                {
                    //checks if the Distance to the current Target is < 0, then returns null and stops current movementThread to be able to fire a new one
                    if (checkCurrent && CurrentMovementTarget.DistanceTo(target) < 2)
                        return null;
                    StopCurrentMovementThread();
                }

                var cts = new CancellationTokenSource();
                Task goToTask = Task.Factory.StartNew(()
                //=> Move.GoTo(target, face, precise, randomizeEnd, randomization, cts.Token,
                //    precision: precision, shortCut: shortCut, jumpRareness: jumpRareness,
                //    showPath: showPath), cts.Token).ContinueWith(
                //task => ResetCurrentMovementCache(), cts.Token);
                => MovementManager.MoveTo(target), cts.Token).ContinueWith(
                task => ResetCurrentMovementCache(), cts.Token);

                if (abortIf != null)
                    Task.Factory.StartNew(() => {
                        while (!goToTask.IsCompleted && !goToTask.IsCanceled && !goToTask.IsFaulted
                               && !cts.Token.IsCancellationRequested)
                        {
                            if (abortIf())
                            {
                                cts.Cancel();
                                break;
                            }

                            Thread.Sleep(100);
                        }
                    }, cts.Token);

                _currentMovementToken = cts;
                _currentMovementTarget = target;
                _currentMovementTask = goToTask;
                return cts;
            }
        }

        private static bool Finished(this Task task) => task.IsCompleted || task.IsCanceled || task.IsFaulted;

        public static CancellationTokenSource StartMoveAlongThread(List<Vector3> pathRaw,
            bool face = true, bool precise = false, Func<bool> abortIf = null, float randomization = 0,
            bool checkCurrent = true, float precision = 1, List<byte> customRadius = null, bool shortCut = false,
            int jumpRareness = 2, bool showPath = false)
        {
            // var path = new List<Vector3>(pathRaw.Count) {ObjectManager.Me.PositionWithoutType};
            // path.AddRange(pathRaw);
            if (pathRaw.Count <= 0)
            {
                Logger.Log("Called MoveAlongThread without a path.");
                return null;
            }

            List<Vector3> path = pathRaw;

            //if (MovementManager.InMovement)
            //    MovementManager.StopMove();

            if (IsMovementThreadRunning)
            {
                if (checkCurrent && CurrentMovementTarget.DistanceTo(path.LastOrDefault()) < 2)
                    return null;
                StopCurrentMovementThread();
            }

            var cts = new CancellationTokenSource();
            Task moveAlongTask = Task.Factory.StartNew(()
                        =>
            //Move.MoveAlong(path, face, precise, randomization, cts.Token, customRadius: customRadius,
            //     shortCut: shortCut, precision: precision, jumpRareness: jumpRareness, showPath: showPath),
            //cts.Token);
            MovementManager.Go(path),
            cts.Token);
            //.ContinueWith(task => ResetCurrentMovementCache(), cts.Token);

            if (abortIf != null)
                Task.Factory.StartNew(() => {
                    while (!moveAlongTask.IsCompleted && !moveAlongTask.IsCanceled && !moveAlongTask.IsFaulted
                           && !cts.Token.IsCancellationRequested)
                    {
                        if (abortIf())
                        {
                            cts.Cancel();
                            break;
                        }

                        Thread.Sleep(100);
                    }
                }, cts.Token);

            _currentMovementToken = cts;
            _currentMovementTarget = path.LastOrDefault();
            _currentMovementTask = moveAlongTask;
            CurrentMovementPath = path;
            return cts;
        }

        public static (List<Vector3>, List<byte>) SplitPathData(this List<(Vector3, byte)> pathWithData)
        {
            var path = new List<Vector3>(pathWithData.Count);
            var rnds = new List<byte>(pathWithData.Count);

            foreach ((Vector3 point, byte radian) in pathWithData)
            {
                path.Add(point);
                rnds.Add(radian);
            }

            return (path, rnds);
        }

    public static float PointDistanceToLine(Vector3 start, Vector3 end, Vector3 point) { //this Function is to calculate the distance of an  Enemy to a given path, made by  Felix
            float vLenSquared = (start.X - end.X) * (start.X - end.X) +
                                (start.Y - end.Y) * (start.Y - end.Y) +
                                (start.Z - end.Z) * (start.Z - end.Z);
            if (vLenSquared == 0f) return point.DistanceTo(start);

            Vector3 ref1 = point - start;
            Vector3 ref2 = end - start;
            float clippedSegment = Math.Max(0, Math.Min(1, Vector3.Dot(ref ref1, ref ref2) / vLenSquared));

            Vector3 projection = start + (end - start) * clippedSegment;
            return point.DistanceTo(projection);
        }

        public static WoWUnit FindClosestUnit(Func<WoWUnit, bool> predicate, Vector3 referencePosition = null) { //this function calculates the flosest Unit
            //first clear ol foundUnit
            WoWUnit foundUnit = null;
            var distanceToUnit = float.MaxValue;
            //checks for a given reference position, if not there then use our position
            Vector3 position = referencePosition != null ? referencePosition : ObjectManager.Me.Position;
            //build a List of each Unit and their Distance
            foreach (WoWUnit unit in ObjectManager.GetObjectWoWUnit()) {
                if (!predicate(unit)) continue;

                if (foundUnit == null) {
                    distanceToUnit = position.DistanceTo(unit.Position);
                    foundUnit = unit;
                } else {
                    //float currentDistanceToUnit = myPosition.DistanceTo(unit.PositionWithoutType);
                    //checks the Distance of the Unit to the given Position
                    float currentDistanceToUnit = CalculatePathTotalDistance(position, unit.PositionWithoutType);
                    if (currentDistanceToUnit < distanceToUnit) {
                        foundUnit = unit;
                        distanceToUnit = currentDistanceToUnit;
                    }
                }
            }
            return foundUnit;
        }

        public static WoWGameObject FindClosestObject(Func<WoWGameObject, bool> predicate, Vector3 referencePosition = null) { //same like FindClosestUnit
            WoWGameObject foundObject = null;
            var distanceToObject = float.MaxValue;
            Vector3 position = referencePosition != null ? referencePosition : ObjectManager.Me.Position;

            foreach (WoWGameObject gameObject in ObjectManager.GetObjectWoWGameObject()) {
                if (!predicate(gameObject)) continue;

                if (foundObject == null) {
                    distanceToObject = position.DistanceTo(gameObject.Position);
                    foundObject = gameObject;
                } else {
                    float currentDistanceToObject = position.DistanceTo(gameObject.Position);
                    //float currentDistanceToObject = CalculatePathTotalDistance(position, gameObject.Position);
                    if (currentDistanceToObject < distanceToObject) {
                        foundObject = gameObject;
                        distanceToObject = currentDistanceToObject;
                    }
                }
            }
            return foundObject;
        }

        //Targeting Code for Tanks and Assists
        //Tank Targeting for First Target Iteration
        //unit attacking group but not the  Tank
        public static WoWUnit foundUnitattackGroup()
        {
            WoWUnit Unit = FindClosestUnit(unit =>
            unit.IsTargetingPartyMember
            && !unit.IsTargetingMe
            && unit.Reaction <= Reaction.Neutral
            && ObjectManager.Me.Position.DistanceTo(unit.Position) <= 60
            && unit.IsAlive, PointInMidOfGroup());
            return Unit;
        }
        //unit attacking grouppet but not the  Tank
        public static WoWUnit foundUnitattakPet()
        {
            WoWUnit Unit = FindClosestUnit(unit =>
            unit.IsTargetingPartyMember
            && unit.TargetObject == ObjectManager.GetObjectWoWUnit().Where(u => u.IsPet && u.Reaction >= Reaction.Neutral).FirstOrDefault()
            && unit.Reaction <= Reaction.Neutral
            && ObjectManager.Me.Position.DistanceTo(unit.Position) <= 60
            && unit.IsAlive, PointInMidOfGroup());
            return Unit;
        }

        //Assist Targeting
        //found Unit on Prio Target List
        public static WoWUnit foundUnitonpriolist()
        {
           WoWUnit Unit = FindClosestUnit(unit =>
           Lists.PriorityTargetListInt.Contains(unit.Entry)
           && unit.Reaction <= Reaction.Neutral
           && unit.IsAlive);
           return Unit;
        }


        //unit attacking Tank of the Group and is closest
        public static WoWUnit foundUnitattackingtank() 
        {
            WoWUnit Unit = FindClosestUnit(unit =>
            unit.Target == Bot.WholesomeDungeons.Tankunit.Guid
            && unit.Reaction <= Reaction.Neutral
            && unit.Position.DistanceTo(Bot.WholesomeDungeons.Tankunit.Position) <= 20
            && ObjectManager.Me.Position.DistanceTo(unit.Position) <= 60
            && unit.IsAlive);
            return Unit;
        }

        ////unit attacking Tank of the Group and is in 10 yards Range
        //public static WoWUnit foundUnitattackingtank10yards()
        //{
        //    WoWUnit Unit = ObjectManager.GetObjectWoWUnit().Where( unit=>
        //    unit.Target == Bot.WholesomeDungeons.Tankunit.Guid
        //    && unit.Reaction <= Reaction.Neutral
        //    && unit.Position.DistanceTo(Bot.WholesomeDungeons.Tankunit.Position) <= 12
        //    && unit.IsAlive).FirstOrDefault();
        //    return Unit;
        //}

        //DPS Targeting for Groups, find fleeing Units and target them First
        public static WoWUnit foundUnitfleeing()
        {
            WoWUnit Unit = FindClosestUnit(unit =>
            unit.Fleeing
            && unit.Reaction <= Reaction.Neutral
            && ObjectManager.Me.Position.DistanceTo(unit.Position) <= 60
            && unit.IsAlive, Bot.WholesomeDungeons.Tankunit.Position);
            return Unit;
        }


        //determine the center of a Group
        public static Vector3 PointInMidOfGroup()
        {
            // Configure
            float xvec = 0;
            float yvec = 0;
            float zvec = 0;

            // Process party members
            int counter = 0;
            foreach (WoWPlayer player in Party.GetPartyHomeAndInstance())
            {
                // Set
                xvec = xvec + player.Position.X;
                yvec = yvec + player.Position.Y;
                zvec = zvec + player.Position.Z;

                // Increase
                counter++;
            }

            // Return the point in the middle
            return new Vector3(xvec / counter, yvec / counter, zvec / counter);
        }

        public static void _RunStateDungeonLogic() //this function helps to see if the character has to run the Dungeonlogic or is just a follower
        {
            //checks if variable Tankunit is null, if so call Groupcheck  Function
            if (Bot.WholesomeDungeons.Tankunit == null)
            {
                //call  Groupcheck
                GroupCheck();
            }
            if (!CurrentSetting.SetAsTank /*&& Bot.WholesomeDungeons.Tankunit.IsAlive*/) //if we are not set as Tank and Tankunit is Alive don´t execute Dungeonlogic
            {
                Logger.Log("Set States.DungeonLogic._DungeonLogicRunstate = false ");
                States.DungeonLogic._DungeonLogicRunstate = false;
            }


        }

        public static bool MeInsideDungeon() //compares the ID´s of DungeonList to see if we are inside a Dungeon
        {
            return Lists.AllDungeons.Count(d => d.MapId == Usefuls.ContinentId) >= 1;
        }

        public static bool GroupmemberRestNew() //check for each  Groupmember for the Buffs Drink and Food and announce it in Log
        {
            bool eatdrink = Party.GetPartyHomeAndInstance().Any(m => m.HaveBuff("Drink") || m.HaveBuff("Food"));
            if(eatdrink)
            {
                foreach(WoWUnit u in Party.GetPartyHomeAndInstance())
                {
                    if(u.HaveBuff("Drink") || u.HaveBuff("Food"))
                    {
                        Logger.Log("Reststate: Waiting for " + u.Name + " to finish Resting");
                    }
                }
            }
            return eatdrink;
        }

        public static bool GroupmemberRest() //check for each Groupmember for the Buffs Drind and Food to determine if they are resting
        {
            return Party.GetPartyHomeAndInstance().Any(m => m.HaveBuff("Drink") || m.HaveBuff("Food"));
        }

        public static bool GroupmembesOutOfRangeNew() //Check if any of the  Groupmembers is out of Range
        {
            bool outofrange = Party.GetPartyHomeAndInstance().Any(m => ObjectManager.Me.Position.DistanceTo(m.Position) >= 40);
            if(outofrange)
            {
                foreach(WoWUnit u in Party.GetPartyHomeAndInstance())
                {
                    if(ObjectManager.Me.Position.DistanceTo(u.Position) >= 40)
                    {
                        Logger.Log("Waitstate: Waiting for " + u.Name + " because out of Range");
                    }
                }
            }
            return outofrange;
        }

        public static bool GroupmembesOutOfRange() //Check if any of the  Groupmembers is out of Range
        {
            return Party.GetPartyHomeAndInstance().Any(m => ObjectManager.Me.Position.DistanceTo(m.Position) >= 40);
        }
        public static bool GroupmembersHealAlive() //check if a Groupmemberclass which should be able to rezz is alive
        {
            return Party.GetPartyHomeAndInstance().Any(m => (m.WowClass == WoWClass.Druid || m.WowClass == WoWClass.Paladin || m.WowClass == WoWClass.Priest || m.WowClass == WoWClass.Shaman) && ObjectManager.Me.Position.DistanceTo(m.Position) <= 50 && m.IsAlive);
        }
        public static bool GroupmemberisDead() //check if any of the Groupmembers is Dead
        {
            return Party.GetPartyHomeAndInstance().Any(m => m.IsDead);
        }

        public static void LUASetLFGRole() //set the Role according to the Settings in the Product through LUA
        {
            if(WholesomeDungeonsSettings.CurrentSetting.SetAsTank)
                Lua.LuaDoString("SetLFGRoles(false, true, false, false)");
            if(WholesomeDungeonsSettings.CurrentSetting.SetAsHeal)
                Lua.LuaDoString("SetLFGRoles(false, false, true, false)");
            if (WholesomeDungeonsSettings.CurrentSetting.SetAsRDPS)
                Lua.LuaDoString("SetLFGRoles(false, false, false, true)");
            if (WholesomeDungeonsSettings.CurrentSetting.SetAsMDPS)
                Lua.LuaDoString("SetLFGRoles(false, false, false, true)");
        }

        public static string LUAGetLFGMode() //returns the actual LFG  Mode
        {
            return Lua.LuaDoString<string>("mode, submode= GetLFGMode(); if mode == nil then return 'nil' else return mode end;");
        }

        public static bool LUAIsInInstance() => Lua.LuaDoString<bool>(  //LUA Check for being in instance
            "local isInstance, instanceType = IsInInstance(); return isInstance ~= nil and instanceType == \"party\";");

        public static bool LUARoleTankSelected() => //LUA  Check for Tank  Role Selection
            Lua.LuaDoString<bool>("local leader, tank, healer, damage = GetLFGRoles(); return tank;");

        public static bool LUARoleHealSelected() => //LUA  Check for Heal  Role Selection
            Lua.LuaDoString<bool>("local leader, tank, healer, damage = GetLFGRoles(); return healer;");

        public static bool LUARoleDamageSelected() => //LUA  Check for Damage  Role Selection
            Lua.LuaDoString<bool>("local leader, tank, healer, damage = GetLFGRoles(); return damage;");

        public static bool LUARoleLeaderSelected() => //LUA  Check for Leader  Role Selection
            Lua.LuaDoString<bool>("local leader, tank, healer, damage = GetLFGRoles(); return leader;");
        //    Can have :
        //    abandonedInDungeon -  The party disbanded and player is still in the dungeon.
        //    lfgparty -            LFG dungeon is in-progress.
        //    nil -                 Player is not in LFG
        //    proposal -            LFG party formed, notifying matched players dungeon is ready.
        //    queued -              Player is in LFG queue.
        //    rolecheck -           Querying groupmates to select their LFG roles before queuing.

        //    submode -             Your LFG sub-status.Used to indicate priority for filling party slots. (string)

        //    empowered -           Indicates that your party has lost a player and is set to higher priority for finding a replacement
        //    nil -                 Not looking for more party members
        //    unempowered -         Default priority in the LFG system.
        public static string LUALFGStatus() =>
            Lua.LuaDoString<string>("local mode, submode = GetLFGMode(); return mode;");

        public static void LUAInstancePortToDungeon() //LUA  Command for teleport to  Dungeon
        {
            Lua.LuaDoString("LFGTeleport(false);");
        }
        public static void LUAInstancePortOutOfDungeon() //LUA  Command for teleport out of  Dungeon
        {
            Lua.LuaDoString("LFGTeleport(true);");
        }

        public static void LUALeaveParty() { //LUA  Command for leaving the Party
            Lua.LuaDoString("LeaveParty()");
        }

        public static bool LUAHaveKeyOnKeyring(int Key) //Check if we have a needed key through  LUA and KeyID check
        {
            bool haveKey = Lua.LuaDoString<bool>("local itemIdSearch = " + Key + "; local bag = KEYRING_CONTAINER; for slot = 1,MAX_CONTAINER_ITEMS do local itemLink = GetContainerItemLink(bag,slot); local _, itemCount = GetContainerItemInfo(bag,slot); if itemLink and itemCount then local _,_,itemId = string.find(itemLink, '.*|Hitem:(%d+):.*'); if itemId and tonumber(itemId) == itemIdSearch then return true end end end return false");
            return haveKey;
        }

        public static string LUAGetTankName() //determine the Tankname through LUA
        {
            return Lua.LuaDoString<string>(@"
                            for i = 1, 4 do 
                                local isTank,_,_ = UnitGroupRolesAssigned('party' .. i)
                                if isTank then
                                    name, realm = UnitName('party' .. i)
                                    return name;
                                end
                            end");
        }
        public static string GetLfgHealer() //determine the Healname through LUA
        {
            return Lua.LuaDoString<string>(@"
                            for i = 1, 4 do 
                                local isTank,isHeal,_ = UnitGroupRolesAssigned('party' .. i)
                                if isHeal then                                    
                                    name, realm = UnitName('party' .. i)
                                    return name;
                                end
                            end");
        }

        public static bool HaveMember(WoWClass Class) //checks for a specific class inside the group
        {
            if (Party.GetPartyHomeAndInstance().Any(u => u.WowClass == Class)) return true;
            return false;
        }

        public static void CloseWindow() { //closes all possible open windows through  LUA
            try {
                Memory.WowMemory.LockFrame();
                Lua.LuaDoString("CloseQuest()");
                Lua.LuaDoString("CloseGossip()");
                Lua.LuaDoString("CloseBankFrame()");
                Lua.LuaDoString("CloseMail()");
                Lua.LuaDoString("CloseMerchant()");
                Lua.LuaDoString("ClosePetStables()");
                Lua.LuaDoString("CloseTaxiMap()");
                Lua.LuaDoString("CloseTrainer()");
                Lua.LuaDoString("CloseAuctionHouse()");
                Lua.LuaDoString("CloseGuildBankFrame()");
                Lua.LuaDoString("CloseLoot()");
                Lua.RunMacroText("/Click QuestFrameCloseButton");
                Lua.LuaDoString("ClearTarget()");
                Thread.Sleep(150);
            } catch (Exception e) {
                Logger.LogError("public static void CloseWindow(): " + e);
            } finally {
                Memory.WowMemory.UnlockFrame();
            }
        }

        // Calculate real walking distance
        public static float CalculatePathTotalDistance(Vector3 from, Vector3 to) //calculate the total path distance from start to end
        {
            //sets distance to 0f
            float distance = 0.0f;
            //build List of vector3 from all pathing points, using pathfinder
            List<Vector3> path = PathFinder.FindPath(from, to, false);
            //checks for path for each path to path
            for (int i = 0; i < path.Count - 1; i++)
            {
                distance += path[i].DistanceTo(path[i + 1]);
            }
            //returns actual real distance
            return distance;
        }

        public static void GroupCheck()
        {
            //checks if we are inside a Dungeon
            if (Lists.AllDungeons.Any(d => d.MapId == Usefuls.ContinentId))
            {
                //sleeps 500ms
                Logger.Log("We are inside Dungeon, execute Tank detecting!");
                Thread.Sleep(500);
                //check if we are set as tank
                if(CurrentSetting.SetAsTank)
                {
                    //sets the variables to our name and ID
                    Logger.Log("We are set as Tank, set ourself");
                    Bot.WholesomeDungeons.Tankunit = ObjectManager.Me;
                    Bot.WholesomeDungeons.Tankname = ObjectManager.Me.Name;
                }
                //check if we are no tank
                if (!CurrentSetting.SetAsTank)
                {
                    //sets the variable tankname to the given tankname of the setting
                    Bot.WholesomeDungeons.Tankname = CurrentSetting.TankName;
                    //sets the tankunit as soon the tank is in ObjectManager Range.
                    if(ObjectManager.Me.Position.DistanceTo(ObjectManager.GetObjectWoWPlayer().FirstOrDefault(p => p.Name == Bot.WholesomeDungeons.Tankname).Position) <= 100)
                    {
                        Bot.WholesomeDungeons.Tankunit = ObjectManager.GetObjectWoWPlayer().FirstOrDefault(p => p.Name == Bot.WholesomeDungeons.Tankname);
                    }
                }
                Logger.Log($"[Set Tank Name]: {Bot.WholesomeDungeons.Tankname} ");
                if(Bot.WholesomeDungeons.Tankunit.Guid != 0)
                {
                    Logger.Log($"[Set Tank Unit to]: {Bot.WholesomeDungeons.Tankunit.Guid}");
                }

            }
        }

        public static void SleepRandomTime() //sleeps a random time, for more randomization
        {
            Random rnd = new Random();
            int sleepWaitMilliSeconds = rnd.Next(100, 2000);
            Thread.Sleep(sleepWaitMilliSeconds);
        }

        public static int GetMoney => (int)ObjectManager.Me.GetMoneyCopper; //gives back the amount of the actual money

        public static bool IsHorde() //returns if we are Horde 
        {
            return ObjectManager.Me.Faction == (uint)PlayerFactions.Orc || ObjectManager.Me.Faction == (uint)PlayerFactions.Tauren
                || ObjectManager.Me.Faction == (uint)PlayerFactions.Undead || ObjectManager.Me.Faction == (uint)PlayerFactions.BloodElf
                || ObjectManager.Me.Faction == (uint)PlayerFactions.Troll;
        }

        public static void AddState(Engine engine, State state, string replace) //function to add states on runtime, not needed anymore
        {
            bool statedAdded = engine.States.Exists(s => s.DisplayName == state.DisplayName);

            if (!statedAdded && engine != null)
            {
                try
                {
                    State stateToReplace = engine.States.Find(s => s.DisplayName == replace);

                    if (stateToReplace == null)
                    {
                        Logger.Log($"Couldn't find state {replace}");
                        return;
                    }

                    int priorityToSet = stateToReplace.Priority;


                    // Move all superior states one slot up
                    foreach (State s in engine.States)
                    {
                        if (s.Priority > priorityToSet)
                            s.Priority++;
                    }

                    engine.AddState(state);
                    state.Priority = priorityToSet + 1;
                    //Logger.Log($"Adding state {state.DisplayName} with prio {priorityToSet}");
                    engine.AddState(state);
                    engine.States.Sort();
                }
                catch (Exception ex)
                {
                    Logger.Log("Error : {0}" + ex.ToString());
                }
            }
        }


        public static string GetRangedWeaponType()//check if we have ranged weapon equipped, not needed anymore
        {
            uint myRangedWeapon = ObjectManager.Me.GetEquipedItemBySlot(InventorySlot.INVSLOT_RANGED);

            if (myRangedWeapon == 0)
                return null;
            else
            {
                List<WoWItem> equippedItems = EquippedItems.GetEquippedItems();
                foreach (WoWItem equippedItem in equippedItems)
                {
                    if (equippedItem.GetItemInfo.ItemSubType == "Crossbows" || equippedItem.GetItemInfo.ItemSubType == "Bows")
                        return "Bows";
                    if (equippedItem.GetItemInfo.ItemSubType == "Guns")
                        return "Guns";
                }
                return null;
            }
        }

        public static void AddItemToDoNotSellList(string itemName) //adds items to not selling list if they don´t exist
        {
            if (!wManagerSetting.CurrentSetting.DoNotSellList.Contains(itemName))
            {
                wManagerSetting.CurrentSetting.DoNotSellList.Add(itemName);
                wManagerSetting.CurrentSetting.Save();
            }
        }

        public static void RemoveItemFromDoNotSellList(string itemName) //removes items from not selling list
        {
            if (wManagerSetting.CurrentSetting.DoNotSellList.Contains(itemName))
            {
                wManagerSetting.CurrentSetting.DoNotSellList.Remove(itemName);
                wManagerSetting.CurrentSetting.Save();
            }
        }
        public static void AddItemToDoNotMailList(string itemName) //adds item to not  Mailing List
        {
            if (!wManagerSetting.CurrentSetting.DoNotMailList.Contains(itemName))
            {
                wManagerSetting.CurrentSetting.DoNotMailList.Add(itemName);
                wManagerSetting.CurrentSetting.Save();
            }
        }

        public static void SoftRestart() //restarts Products
        {
            Products.InPause = true;
            Thread.Sleep(100);
            Products.InPause = false;
        }

        public static void Restart()
        {
            new Thread(() =>
            {
                Products.ProductStop();
                Thread.Sleep(2000);
                Products.ProductStart();
            }).Start();
        }

        public static string GetWoWVersion() //returns the actial wow  Version
        {
            return Lua.LuaDoString<string>("v, b, d, t = GetBuildInfo(); return v");
        }



        public static List<string> GetVendorItemList() //returns the actual Vendorlist
        {
            return Lua.LuaDoString<List<string>>(@"local r = {}
                                            for i=1,GetMerchantNumItems() do 
	                                            local n=GetMerchantItemInfo(i);
	                                            if n then table.insert(r, tostring(n)); end
                                            end
                                            return unpack(r);");
        }

        public static void BuyItem(string name, int amount, int stackValue) //Function to Buy items from  vendor
        {
            double numberOfStacksToBuy = Math.Ceiling(amount / (double)stackValue);
            Logger.Log($"Buying {amount} x {name}");
            Lua.LuaDoString(string.Format(@"
                    local itemName = ""{0}""
                    local quantity = {1}
                    for i=1, GetMerchantNumItems() do
                        local name = GetMerchantItemInfo(i)
                        if name and name == itemName then 
                            BuyMerchantItem(i, quantity)
                        end
                    end", name, (int)numberOfStacksToBuy));
        }



 
        public static bool PlayerInBloodElfStartingZone() //checks if we are in the b11 starting zone
        {
            string zone = Lua.LuaDoString<string>("return GetRealZoneText();");
            return zone == "Eversong Woods" || zone == "Ghostlands" || zone == "Silvermoon City";
        }

        public static bool PlayerInDraneiStartingZone() //checks if we are in dhe Draenei starting zone
        {
            string zone = Lua.LuaDoString<string>("return GetRealZoneText();");
            return zone == "Azuremyst Isle" || zone == "Bloodmyst Isle" || zone == "The Exodar";
        }
    }
}