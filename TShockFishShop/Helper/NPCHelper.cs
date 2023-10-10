using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using TShockAPI;
using TShockAPI.Localization;

namespace FishShop
{
    public class NPCHelper
    {
        public static string GetNameByID(int id)
        {
            // Town NPCs and bosses
            string s = Settings.GetNPCNameByID(id);
            if (!string.IsNullOrEmpty(s)) return s;

            // Other NPCs
            s = GetNPCNameValue(id);
            if (!string.IsNullOrEmpty(s)) return s;

            return "";
        }

        public static int GetIDByName(string name = "")
        {
            int id = Settings.GetNPCIDByName(name);
            if (id != 0) return id;

            id = GetNPCIDByName(name);
            if (id != 0) return id;

            return 0;
        }

        // Check if NPC is active (alive)
        public static bool CheckNPCActive(string npcNameOrId)
        {
            int id = 0;
            if (!int.TryParse(npcNameOrId, out id))
                id = GetIDByName(npcNameOrId);

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].netID == id)
                    return true;
            }
            return false;
        }

        private static string GetNPCNameValue(int id)
        {
            if (id < NPCID.Count && id != 0)
                return Lang.GetNPCNameValue(id);
            return "";
        }

        private static int GetNPCIDByName(string name)
        {
            var found = new List<int>();
            NPC npc = new();
            string nameLower = name.ToLowerInvariant();
            for (int i = -17; i < NPCID.Count; i++)
            {
                string englishName = EnglishLanguage.GetNpcNameById(i).ToLowerInvariant();

                npc.SetDefaults(i);
                if (npc.FullName.ToLowerInvariant() == nameLower || npc.TypeName.ToLowerInvariant() == nameLower
                    || nameLower == englishName)
                    return npc.netID;
                if (npc.FullName.ToLowerInvariant().StartsWith(nameLower) || npc.TypeName.ToLowerInvariant().StartsWith(nameLower)
                    || englishName?.StartsWith(nameLower) == true)
                    found.Add(npc.netID);
            }
            for (int i = -17; i < NPCID.Count; i++)
            {
                string englishName = Lang.GetNPCNameValue(i);

                npc.SetDefaults(i);
                if (npc.FullName.ToLowerInvariant() == nameLower || npc.TypeName.ToLowerInvariant() == nameLower
                    || nameLower == englishName)
                    return npc.netID;
                if (npc.FullName.ToLowerInvariant().StartsWith(nameLower) || npc.TypeName.ToLowerInvariant().StartsWith(nameLower)
                    || englishName?.StartsWith(nameLower) == true)
                    found.Add(npc.netID);
            }

            if (found.Count >= 1)
                return found[0];

            return 0;
        }

        // NPC respawn
        public static void ReliveNPC(TSPlayer op)
        {
            List<int> found = new();

            // Guide
            found.Add(22);

            // Rescue states
            // Angler
            if (NPC.savedAngler)
                found.Add(369);

            // Goblin Tinkerer
            if (NPC.savedGoblin)
                found.Add(107);

            // Mechanic
            if (NPC.savedMech)
                found.Add(124);

            // Stylist
            if (NPC.savedStylist)
                found.Add(353);

            // Tavernkeep
            if (NPC.savedBartender)
                found.Add(550);

            // Golfer
            if (NPC.savedGolfer)
                found.Add(588);

            // Wizard
            if (NPC.savedWizard)
                found.Add(108);

            // Tax Collector
            if (NPC.savedTaxCollector)
                found.Add(441);

            // Cat
            if (NPC.boughtCat)
                found.Add(637);

            // Dog
            if (NPC.boughtDog)
                found.Add(638);

            // Bunny
            if (NPC.boughtBunny)
                found.Add(656);

            // Bestiary unlocks
            List<int> remains = new() {
                19, // Arms Dealer
                54, // Clothier
                38, // Demolitionist
                20, // Dryad
                207, // Dye Trader
                17, // Merchant
                18, // Nurse
                227, // Painter
                208, // Party Girl
                228, // Witch Doctor
                633, // Zoologist
                209, // Mechanic
                229, // Pirate
                178, // Steampunker
                160, // Truffle
                663 // Princess
            };

            // Santa Claus
            if (Main.xMas)
                remains.Add(142);

            foreach (int npcID1 in remains)
            {
                if (DidDiscoverBestiaryEntry(npcID1))
                    found.Add(npcID1);
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (!Main.npc[i].active || !Main.npc[i].townNPC)
                    continue;

                found.Remove(Main.npc[i].type);
            }

            // Generate NPCs
            List<string> names = new();
            foreach (int npcID in found)
            {
                NPC npc = new();
                npc.SetDefaults(npcID);
                TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, 1, op.TileX, op.TileY, 5, 2);

                if (names.Count != 0 && names.Count % 10 == 0)
                {
                    names.Add("\n" + npc.FullName);
                }
                else
                {
                    names.Add(npc.FullName);
                }
            }

            if (found.Count > 0)
            {
                TSPlayer.All.SendInfoMessage($"{op.Name} revived {found.Count} NPCs:");
                TSPlayer.All.SendInfoMessage($"{string.Join("、", names)}");
            }
            else
            {
                op.SendInfoMessage("All previously housed NPCs are alive.");
            }
        }

        public static bool NeedBuyReliveNPC(TSPlayer op)
        {
            List<int> found = new();

            // Guide
            found.Add(22);

            // Rescue states
            // Angler
            if (NPC.savedAngler)
                found.Add(369);

            // Goblin Tinkerer
            if (NPC.savedGoblin)
                found.Add(107);

            // Mechanic
            if (NPC.savedMech)
                found.Add(124);

            // Stylist
            if (NPC.savedStylist)
                found.Add(353);

            // Tavernkeep
            if (NPC.savedBartender)
                found.Add(550);

            // Golfer
            if (NPC.savedGolfer)
                found.Add(588);

            // Wizard
            if (NPC.savedWizard)
                found.Add(108);

            // Tax Collector
            if (NPC.savedTaxCollector)
                found.Add(441);

            // Cat
            if (NPC.boughtCat)
                found.Add(637);

            // Dog
            if (NPC.boughtDog)
                found.Add(638);

            // Bunny
            if (NPC.boughtBunny)
                found.Add(656);

            // Bestiary unlocks
            List<int> remains = new() {
                19, // Arms Dealer
                54, // Clothier
                38, // Demolitionist
                20, // Dryad
                207, // Dye Trader
                17, // Merchant
                18, // Nurse
                227, // Painter
                208, // Party Girl
                228, // Witch Doctor
                633, // Zoologist
                209, // Mechanic
                229, // Pirate
                178, // Steampunker
                160, // Truffle
                663 // Princess
            };

            // Santa Claus
            if (Main.xMas)
                remains.Add(142);

            foreach (int npcID1 in remains)
            {
                if (DidDiscoverBestiaryEntry(npcID1))
                    found.Add(npcID1);
            }

            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (!Main.npc[i].active || !Main.npc[i].townNPC)
                    continue;

                found.Remove(Main.npc[i].type);
            }

            if (found.Count == 0)
            {
                op.SendInfoMessage("All previously housed NPCs are alive, no need to buy them.");
                return false;
            }
            return true;
        }

        private static bool DidDiscoverBestiaryEntry(int npcId)
        {
            return Main.BestiaryDB.FindEntryByNPCID(npcId).UIInfoProvider.GetEntryUICollectionInfo().UnlockState > BestiaryEntryUnlockState.NotKnownAtAll_0;
        }

        // Find nearby specified NPC
        public static NPC FindNearNPC(TSPlayer op, int npcID)
        {
            Rectangle rect = new(op.TileX - 59, op.TileY - 35 + 3, 120, 68);
            foreach (var npc in Main.npc.Where(npc => npc != null && npc.active && npc.netID == npcID))
            {
                Point pos = new((int)(npc.position.X / 16), (int)(npc.position.Y / 16));
                if (rect.Contains(pos))
                {
                    return npc;
                }
            }
            return null;
        }

        // Spawn NPC
        public static void SpawnNPC(TSPlayer op, int npcID, int times = 0)
        {
            string bossType = "";
            switch (npcID)
            {
                case 266: bossType = "brain of cthulhu"; break;
                case 134: bossType = "destroyer"; break;
                case 370: bossType = "duke fishron"; break;
                case 13: bossType = "eater of worlds"; break;
                case 4: bossType = "eye of cthulhu"; break;
                case 245: bossType = "golem"; break;
                case 50: bossType = "king slime"; break;
                case 262: bossType = "plantera"; break;
                case 127: bossType = "skeletron prime"; break;
                case 222: bossType = "queen bee"; break;
                case 35: bossType = "skeletron"; break;
                case 125: bossType = "twins"; break;
                case 126: bossType = "twins"; break;
                case 113: bossType = "wall of flesh"; break;
                case 396: bossType = "moon lord"; break;
                case 636: bossType = "empress of light"; break;
                case 657: bossType = "queen slime"; break;
                case 439: bossType = "lunatic cultist"; break;
                case 551: bossType = "betsy"; break;
                case 491: bossType = "flying dutchman"; break;
                case 325: bossType = "mourning wood"; break;
                case 327: bossType = "pumpking"; break;
                case 344: bossType = "everscream"; break;
                case 346: bossType = "santa-nk1"; break;
                case 345: bossType = "ice queen"; break;
                case 392: bossType = "martian saucer"; break;
                case 393: bossType = "martian saucer"; break;
                case 394: bossType = "martian saucer"; break;
                case 395: bossType = "martian saucer"; break;
                case 517: bossType = "solar pillar"; break;
                case 507: bossType = "nebula pillar"; break;
                case 422: bossType = "vortex pillar"; break;
                case 493: bossType = "stardust pillar"; break;
                case 668: bossType = "deerclops"; break;
            }

            if (!string.IsNullOrEmpty(bossType))
            {
                // Summon boss
                List<string> args = new() { bossType };
                if (times > 0)
                    args.Add(times.ToString());
                SpawnBossRaw(new CommandArgs("", op, args));
            }
            else
            {
                // Spawn NPC
                NPC npc = new();
                npc.SetDefaults(npcID);

                bool pass = true;
                if (npc.townNPC)
                {
                    if (CheckNPCActive(npcID.ToString()))
                    {
                        pass = false;
                    }
                }

                if (pass)
                    TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, times, op.TileX, op.TileY);
            }
        }

        // Clear NPC
        public static void ClearNPC(TSPlayer op, int npcID, int times = 0)
        {
            List<NPC> npcs = TShock.Utils.GetNPCByIdOrName(npcID.ToString());
            if (npcs.Count == 0)
            {
                op.SendErrorMessage("Could not find an NPC with the specified ID.");
            }
            else if (npcs.Count > 1)
            {
                op.SendMultipleMatchError(npcs.Select(n => $"{n.FullName}({n.type})"));
            }
            else
            {
                var npc = npcs[0];
                TSPlayer.All.SendSuccessMessage("{0} cleared {1} {2}(s)", op.Name, ClearNPCByID(npc.netID), npc.FullName);
            }
        }

        // Clear NPC by ID
        private static int ClearNPCByID(int npcID)
        {
            int cleared = 0;
            for (int i = 0; i < Main.maxNPCs; i++)
            {
                if (Main.npc[i].active && Main.npc[i].netID == npcID)
                {
                    Main.npc[i].active = false;
                    Main.npc[i].type = 0;
                    TSPlayer.All.SendData(PacketTypes.NpcUpdate, "", i);
                    cleared++;
                }
            }
            return cleared;
        }

        // SpawnBossRaw method
        private static void SpawnBossRaw(CommandArgs args)
        {
            if (args.Parameters.Count < 1 || args.Parameters.Count > 2)
            {
                args.Player.SendErrorMessage("Invalid syntax! Proper syntax: {0}spawnboss <boss type> [amount]", Commands.Specifier);
                return;
            }

            int amount = 1;
            if (args.Parameters.Count == 2 && (!int.TryParse(args.Parameters[1], out amount) || amount <= 0))
            {
                args.Player.SendErrorMessage("Invalid boss name!");
                return;
            }

            string message = "{0} spawned {1} {2}(s)";
            string spawnName = "";
            int npcID = 0;
            NPC npc = new();
            switch (args.Parameters[0].ToLower())
            {
                case "*":
                case "all":
                    int[] npcIds = { 4, 13, 35, 50, 125, 126, 127, 134, 222, 245, 262, 266, 370, 398, 439, 636, 657 };
                    TSPlayer.Server.SetTime(false, 0.0);
                    foreach (int i in npcIds)
                    {
                        npc.SetDefaults(i);
                        TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
                    }
                    spawnName = "Boss All-Star";
                    return;

                case "brain":
                case "brain of cthulhu":
                case "boc":
                    npcID = 266;
                    break;

                case "destroyer":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 134;
                    break;

                case "duke":
                case "duke fishron":
                case "fishron":
                    npcID = 370;
                    break;

                case "eater":
                case "eater of worlds":
                case "eow":
                    npcID = 13;
                    break;

                case "eye":
                case "eye of cthulhu":
                case "eoc":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 4;
                    break;

                case "golem":
                    npcID = 245;
                    break;

                case "king":
                case "king slime":
                case "ks":
                    npcID = 50;
                    break;

                case "plantera":
                    npcID = 262;
                    break;

                case "prime":
                case "skeletron prime":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 127;
                    break;

                case "queen bee":
                case "qb":
                    npcID = 222;
                    break;

                case "skeletron":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 35;
                    break;

                case "twins":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npc.SetDefaults(125);
                    TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
                    npc.SetDefaults(126);
                    TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
                    spawnName = "The Twins";
                    break;

                case "wof":
                case "wall of flesh":
                    if (Main.wofNPCIndex != -1)
                    {
                        args.Player.SendErrorMessage("The Wall of Flesh is already summoned!");
                        return;
                    }
                    if (args.Player.Y / 16f < Main.maxTilesY - 205)
                    {
                        args.Player.SendErrorMessage("The Wall of Flesh can only be summoned in Hell!");
                        return;
                    }
                    NPC.SpawnWOF(new Vector2(args.Player.X, args.Player.Y));
                    spawnName = "Wall of Flesh";
                    break;

                case "moon":
                case "moon lord":
                case "ml":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 398;
                    break;

                case "empress":
                case "empress of light":
                case "eol":
                    npcID = 636;
                    break;

                case "queen slime":
                case "qs":
                    npcID = 657;
                    break;

                case "lunatic":
                case "lunatic cultist":
                case "cultist":
                case "lc":
                    npcID = 439;
                    break;

                case "betsy":
                    npcID = 551;
                    break;

                case "flying dutchman":
                case "flying":
                case "dutchman":
                    npcID = 491;
                    break;

                case "mourning wood":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 325;
                    break;

                case "pumpking":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 327;
                    break;

                case "everscream":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 344;
                    break;

                case "santa-nk1":
                case "santa nk1":
                case "santa":
                case "nk1":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 346;
                    break;

                case "ice queen":
                case "queen":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 345;
                    break;

                case "martian":
                case "martian saucer":
                case "saucer":
                    TSPlayer.Server.SetTime(false, 0.0);
                    npcID = 392;
                    break;

                case "martian drone":
                case "martian boss":
                case "drone":
                    npcID = 393;
                    break;

                case "martian engineer":
                case "engineer":
                    npcID = 394;
                    break;

                case "martian officer":
                case "officer":
                    npcID = 395;
                    break;

                case "solar":
                case "solar pillar":
                    npcID = 517;
                    break;

                case "nebula":
                case "nebula pillar":
                    npcID = 507;
                    break;

                case "vortex":
                case "vortex pillar":
                    npcID = 422;
                    break;

                case "stardust":
                case "stardust pillar":
                    npcID = 493;
                    break;

                case "deerclops":
                case "dc":
                    npcID = 668;
                    break;

                default:
                    TSPlayer.Server.SetTime(false, 0.0);
                    npc = TShock.Utils.GetNPCByIdOrName(args.Parameters[0]);
                    if (npc.Count == 0)
                    {
                        args.Player.SendErrorMessage("Invalid boss name!");
                        return;
                    }
                    npcID = npc[0].netID;
                    break;
            }

            if (string.IsNullOrEmpty(spawnName))
            {
                npc.SetDefaults(npcID);
                TSPlayer.Server.SpawnNPC(npc.type, npc.FullName, amount, args.Player.TileX, args.Player.TileY);
                spawnName = npc.FullName;
            }

            if (amount == 1)
            {
                TSPlayer.All.SendSuccessMessage(message, args.Player.Name, spawnName);
            }
            else
            {
                TSPlayer.All.SendSuccessMessage(message, args.Player.Name, amount, spawnName);
            }
        }
    }
}
