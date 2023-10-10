using FishShop.Shop;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TShockAPI;

namespace FishShop
{
    /// <summary>
    /// Shop Item ID
    /// Description: Shop Item IDs, Unlock IDs, and Trade Item IDs share a common ID system, and some IDs can be both Unlock IDs and Shop Item IDs.
    /// </summary>
    public class ShopItemID
    {
        // Custom shop item IDs
        // Moon phases, fireworks, quest fish swaps, etc.
        // Summoning bosses, NPCs, and enemies
        // ------------------------------------------------------------------------------------------
        public const int MoonphaseStart = -131;
        public const int MoonphaseNext = -139;      // Next moon phase
        public const int Moonphase1 = -131;         // Full moon
        public const int Moonphase2 = -132;         // Waning gibbous
        public const int Moonphase3 = -133;         // Third quarter moon
        public const int Moonphase4 = -134;         // Waning crescent
        public const int Moonphase5 = -135;         // New moon
        public const int Moonphase6 = -136;         // Waxing crescent
        public const int Moonphase7 = -137;         // First quarter moon
        public const int Moonphase8 = -138;         // Waxing gibbous

        public const int Firework = -140;           // Fireworks
        public const int FireworkRocket = -141;     // Launch fireworks
        public const int AnglerQuestSwap = -142;    // Swap angler quest fish
        public const int RainingStart = -143;       // Start rain
        public const int RainingStop = -144;        // Stop rain
        public const int BuffGoodLucky = -145;      // Good luck buff
        public const int InvasionStop = -146;       // Skip invasion
        public const int TimeToDay = -147;          // Set to daytime
        public const int TimeToNight = -148;        // Set to nighttime
        public const int BloodMoonStart = -149;     // Summon Blood Moon
        public const int BloodMoonStop = -150;      // Skip Blood Moon

        public const int Buff = -159;               // Buff
        public const int RawCmd = -160;             // Command

        public const int ReliveNPC = -161;          // Revive NPC
        public const int TPHereAll = -162;          // Teleport all players to you
        public const int CelebrateAll = -163;       // Celebrate with all players
        public const int TimeToNoon = -164;         // Set to noon
        public const int TimeToMidNight = -165;     // Set to midnight

        // Buffs
        public const int BuffWhipPlayer = -166;     // Whip player
        public const int BuffFaster = -167;         // Catcher of shrimp
        public const int BuffMining = -168;         // Gold digger
        public const int BuffFishing = -169;        // Angler
        public const int BuffIncitant = -170;       // Stimulant

        // Summon invasions
        public const int InvasionGoblins = -171;        // Summon Goblin Army
        public const int InvasionSnowmen = -172;        // Summon Frost Legion
        public const int InvasionPirates = -173;        // Summon Pirate Invasion
        public const int InvasionPumpkinmoon = -174;    // Summon Pumpkin Moon
        public const int InvasionFrostmoon = -175;      // Summon Frost Moon
        public const int InvasionMartians = -176;       // Summon Martian Madness

        // Events
        public const int ToggleRain = -177;             // Rain
        public const int ToggleSlimeRain = -178;        // Slime Rain
        public const int ToggleSandStorm = -179;        // Sandstorm
        public const int ToggleWindyDay = -180;         // Windy Day
        public const int ToggleStorming = -181;         // Storm
        public const int ToggleBloodMoon = -182;        // Blood Moon
        public const int ToggleEclipse = -183;          // Solar Eclipse
        public const int ToggleParty = -184;            // Party
        public const int TriggerDropMeteor = -185;      // Meteorite
        public const int StarfallStart = -186;          // Starfall
        public const int LanternsNightStart = -187;     // Lanterns Night
        public const int OverworldDayStart = -188;      // Beautiful Day
        public const int DirtiestBlock = -189;          // Dirtiest Block
        public const int OneDamage = -190;              // One Damage (Invalid)
        public const int Forge = -191;                  // Ten Strikes

        // Summon NPCs
        // -1000-[npcID]
        private const int SpawnStart = -1000;
        private const int SpawnEnd = -1999;

        // Clear NPCs
        // -4000-[npcID]
        private const int ClearNPCStart = -4000;
        private const int ClearNPCEnd = -4999;

        // Set Buffs
        // -5000-[buffID]
        private const int SetBuffStart = -5000;
        private const int SetBuffEnd = -5999;

        private static int GetSpawnID(int id) { return -(1000 + id); }
        public static int GetRealSpawnID(int id) { return id > SpawnEnd && id < SpawnStart ? SpawnStart - id : 0; }

        private static int GetClearNPCID(int id) { return -(4000 + id); }
        public static int GetRealClearNPCID(int id) { return id > ClearNPCEnd && id < ClearNPCStart ? ClearNPCStart - id : 0; }

        private static int GetSetBuffID(int id) { return -(5000 + id); }
        public static int GetRealBuffID(int id) { return id > SetBuffEnd && id < SetBuffStart ? SetBuffStart - id : 0; }

        public static string GetNameByID(int id, string prefix = "", int stack = 1)
        {
            string text = Settings.GetShopItemNameByID(id);
            if (!string.IsNullOrEmpty(text)) return text;

            int npcID;
            string npcName;
            
            // Summon NPCs
            if (id >= SpawnEnd && id <= SpawnStart)
            {
                npcID = SpawnStart - id;
                npcName = NPCHelper.GetNameByID(npcID);
                if (!string.IsNullOrEmpty(npcName)) return $"Summon {npcName}";
            }

            // Clear NPCs
            if (id >= ClearNPCEnd && id <= ClearNPCStart)
            {
                npcID = ClearNPCStart - id;
                npcName = NPCHelper.GetNameByID(npcID);
                if (!string.IsNullOrEmpty(npcName)) return $"Clear {npcName}";
            }

            // Set Buffs
            if (id >= SetBuffEnd && id <= SetBuffStart)
            {
                int buffID = SetBuffStart - id;
                string buffName = BuffHelper.GetBuffNameByID(buffID);
                if (!string.IsNullOrEmpty(buffName))
                    return $"{buffName}{BuffHelper.GetTimeDesc(stack)}";
            }

            return "";
        }

        public static int GetIDByName(string name = "")
        {
            if (string.IsNullOrEmpty(name)) return 0;

            int id = Settings.GetShopItemIDByName(name);
            if (id != 0) return id;

            switch (name)
            {
                case "Command": return RawCmd;
                case "Buff": return Buff;
                case "Copper": case "Copper Coin": return 71;
                case "Silver": case "Silver Coin": return 72;
                case "Gold": case "Gold Coin": return 73;
                case "Platinum": case "Platinum Coin": return 74;
                case "First Fractal": return 4722;
                case "Boring Bow": return 3853;
            }

            id = Settings.GetItemIDByName(name);
            if (id != 0) return id;

            if (name.Contains("Summon"))
            {
                string s = name.Replace(" ", "").Replace("Summon", "").ToLowerInvariant();
                int npcID = NPCHelper.GetIDByName(s);
                if (npcID != 0) return GetSpawnID(npcID);
            }

            if (name.Contains("Clear"))
            {
                string s = name.Replace(" ", "").Replace("Clear", "").ToLowerInvariant();
                int npcID = NPCHelper.GetIDByName(s);
                if (npcID != 0) return GetClearNPCID(npcID);
            }

            id = GetItemIDByName(name);
            if (id != 0) return id;

            id = BuffHelper.GetBuffIDByName(name);
            if (id != 0) return id;

            return 0;
        }

        private static int GetItemIDByName(string name)
        {
            List<Item> items = TShock.Utils.GetItemByName(name);
            if (items.Count > 0) return items[0].netID;

            return 0;
        }

        public static void ProvideGoods(TSPlayer player, ShopItemData shopItem, int amount = 1)
        {
            int id = shopItem.id;
            
            switch (id)
            {
                // Moon phases
                case Moonphase1:
                case Moonphase2:
                case Moonphase3:
                case Moonphase4:
                case Moonphase5:
                case Moonphase6:
                case Moonphase7:
                case Moonphase8:
                case MoonphaseNext: FishHelper.ChangeMoonPhaseByID(player, id, amount); return;

                case Firework: CmdHelper.Firework(player); return;
                case FireworkRocket: CmdHelper.FireworkRocket(player); return;
                case AnglerQuestSwap: FishHelper.AnglerQuestSwap(player); return;

                // Daytime, noon, nighttime, midnight
                case TimeToDay: CmdHelper.SwitchTime(player, "day"); return;
                case TimeToNoon: CmdHelper.SwitchTime(player, "noon"); return;
                case TimeToNight: CmdHelper.SwitchTime(player, "night"); return;
                case TimeToMidNight: CmdHelper.SwitchTime(player, "midnight"); return;

                // Buffs
                case BuffGoodLucky:
                case BuffWhipPlayer:
                case BuffFaster:
                case BuffMining:
                case BuffFishing:
                case BuffIncitant:
                case Buff:
                    BuffHelper.BuffCommon(player, shopItem.GetBuff(), shopItem.GetBuffSecond(), amount);
                    return;

                // Rain start and stop
                case RainingStart: CmdHelper.ToggleRaining(player, true); return;
                case RainingStop: CmdHelper.ToggleRaining(player, false); return;

                // Blood Moon start and stop
                case BloodMoonStart: CmdHelper.ToggleBloodMoon(player, true); return;
                case BloodMoonStop: CmdHelper.ToggleBloodMoon(player, false); return;

                // Event toggles
                case ToggleRain: CmdHelper.ToggleRaining(player, true, true); return;
                case ToggleSlimeRain: CmdHelper.ToggleSlimeRain(player, true, true); return;
                case ToggleSandStorm: CmdHelper.ToggleSandstorm(player, true, true); return;
                case ToggleWindyDay: CmdHelper.ToggleWindyDay(player, true, true); return;
                case ToggleStorming: CmdHelper.ToggleStorming(player, true, true); return;
                case ToggleBloodMoon: CmdHelper.ToggleBloodMoon(player, true, true); return;
                case ToggleEclipse: CmdHelper.ToggleEclipse(player, true, true); return;
                case ToggleParty: CmdHelper.ToggleParty(player, true, true); return;
                case TriggerDropMeteor: CmdHelper.DropMeteor(player); return;
                case StarfallStart: CmdHelper.Starfall(player); return;
                case LanternsNightStart: CmdHelper.LanternsNightStart(player); return;
                case OverworldDayStart: CmdHelper.OverworldDay(player); return;
                case DirtiestBlock: return;

                // One damage to bosses
                case OneDamage:
                    for (int i = 0; i < Main.npc.Length; i++)
                    {
                        NPC npc = Main.npc[i];
                        if (npc != null && npc.active && npc.boss)
                        {
                            NPCHelper.AttackBoss(player, npc);
                            TSPlayer.All.SendInfoMessage($"{player.Name} bought {shopItem.GetItemDesc()}, dealing 1 point of damage to the boss!");
                        }
                    }
                    return;

                default: break;
            }

            // Summon NPCs
            int id2 = GetRealSpawnID(id);
            if (id2 != 0)
            {
                NPCHelper.SpawnNPC(player, id2, amount);
                return;
            }

            // Clear NPCs
            id2 = GetRealClearNPCID(id);
            if (id2 != 0)
            {
                NPCHelper.ClearNPC(player, id2, amount);
                return;
            }

            // Set Buffs
            id2 = GetRealBuffID(id);
            if (id2 != 0)
            {
                BuffHelper.SetPlayerBuff(player, id2, shopItem.stack * amount);
                return;
            }

            // Custom player shop items with commands and buffs
            if (id <= -600 && id >= -999)
            {
                foreach (string s in shopItem.GetCMD().Where(s => !string.IsNullOrEmpty(s)))
                {
                    CmdHelper.ExecuteRawCmd(player, s);
                }

                BuffHelper.BuffCommon(player, shopItem.GetBuff(), shopItem.GetBuffSecond(), amount);
            }
        }

    }
}
