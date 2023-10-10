using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace FishShop
{
    /// <summary>
    /// Transaction Item ID
    /// Description: Product IDs, unlock IDs, and transaction item IDs share the same ID rules; some IDs are both unlock and product IDs.
    /// </summary>
    public class CostID
    {
        public const int QuestFish = -101;          // Today's quest fish
        public const int AnyQuestFish = -102;       // Any quest fish
        public const int AnyFish = -103;            // Any fish

        public const int AnyWood = -200;            // Any wood
        public const int AnyTorch = -201;           // Any torch
        public const int AnyIronBar = -202;         // Any iron bar
        public const int AnySand = -203;            // Any sand block
        public const int AnyPressurePlate = -204;   // Any pressure plate
        public const int AnyBird = -205;            // Any bird
        public const int AnyScorpion = -206;        // Any scorpion
        public const int AnySquirrel = -207;        // Any squirrel
        public const int AnyJungleBug = -208;       // Any jungle bug
        public const int AnyDuck = -209;            // Any duck
        public const int AnyButterfly = -210;       // Any butterfly
        public const int AnyFirefly = -211;         // Any firefly
        public const int AnySnail = -212;           // Any snail
        public const int AnyTurtle = -213;          // Any turtle
        public const int AnyMacaw = -214;           // Any macaw
        public const int AnyCockatiel = -215;       // Any cockatiel
        public const int AnyDragonfly = -216;       // Any dragonfly
        public const int AnyFruit = -217;           // Any fruit
        public const int AnyBalloon = -218;         // Any balloon
        public const int AnyCloudBalloon = -219;    // Any cloud balloon
        public const int AnyBlizzardBalloon = -220; // Any blizzard balloon
        public const int AnySandstormBalloon = -221; // Any sandstorm balloon

        public const int AnyHorseshoeBalloon = -222;    // Any horseshoe balloon
        public const int AnyCrate = -223;               // Any crate
        public const int AnyTombstone = -224;           // Any tombstone
        public const int AnyGoldCritter = -225;         // Any gold critter
        public const int AnyParrot = -226;              // Any parrot

        #region ID Collections
        // Any Quest Fish: Main.anglerQuestItemNetIDs
        // Any Crate: ItemID.Sets.IsFishingCrate

        /// <summary>
        /// Any fish
        /// </summary>
        public static readonly int[] Fishes = new int[] { /* List of fish item IDs */ };

        /// <summary>
        /// Any tombstone
        /// </summary>
        public static readonly int[] Tombstones = new int[] { /* List of tombstone item IDs */ };

        /// <summary>
        /// Any gold critter
        /// </summary>
        public static readonly int[] GoldCritters = new int[] { /* List of gold critter item IDs */ };

        /// <summary>
        /// Any horseshoe balloon
        /// </summary>
        public static readonly int[] HorseshoeBalloons = new int[] { /* List of horseshoe balloon item IDs */ };

        /// <summary>
        /// Any parrot
        /// </summary>
        public static readonly int[] Parrots = new int[] { /* List of parrot item IDs */ };

        /// <summary>
        /// Any crate
        /// </summary>
        public static readonly int[] Crates = new int[] { /* List of crate item IDs */ };

        #region Wiki Definitions
        // --------------------------------------------------------------------
        // Reference: https://terraria.wiki.gg/wiki/Alternative_crafting_ingredients
        // --------------------------------------------------------------------

        /// <summary>
        /// Any wood
        /// </summary>
        public static readonly int[] Woods = new int[] { /* List of wood item IDs */ };

        /// <summary>
        /// Any torch
        /// </summary>
        public static readonly int[] Torches = new int[] { /* List of torch item IDs */ };

        /// <summary>
        /// Any iron bar
        /// </summary>
        public static readonly int[] IronBars = new int[] { /* List of iron bar item IDs */ };

        /// <summary>
        /// Any sand block
        /// </summary>
        public static readonly int[] Sands = new int[] { /* List of sand block item IDs */ };

        /// <summary>
        /// Any pressure plate
        /// </summary>
        public static readonly int[] PressurePlates = new int[] { /* List of pressure plate item IDs */ };

        /// <summary>
        /// Any bird
        /// </summary>
        public static readonly int[] Birds = new int[] { /* List of bird item IDs */ };

        /// <summary>
        /// Any scorpion
        /// </summary>
        public static readonly int[] Scorpions = new int[] { /* List of scorpion item IDs */ };

        /// <summary>
        /// Any squirrel
        /// </summary>
        public static readonly int[] Squirrels = new int[] { /* List of squirrel item IDs */ };

        /// <summary>
        /// Any jungle bug
        /// </summary>
        public static readonly int[] JungleBugs = new int[] { /* List of jungle bug item IDs */ };

        /// <summary>
        /// Any duck
        /// </summary>
        public static readonly int[] Ducks = new int[] { /* List of duck item IDs */ };

        /// <summary>
        /// Any butterfly
        /// </summary>
        public static readonly int[] Butterflies = new int[] { /* List of butterfly item IDs */ };

        /// <summary>
        /// Any firefly
        /// </summary>
        public static readonly int[] Fireflies = new int[] { /* List of firefly item IDs */ };

        /// <summary>
        /// Any snail
        /// </summary>
        public static readonly int[] Snails = new int[] { /* List of snail item IDs */ };

        /// <summary>
        /// Any turtle
        /// </summary>
        public static readonly int[] Turtles = new int[] { /* List of turtle item IDs */ };

        /// <summary>
        /// Any macaw
        /// </summary>
        public static readonly int[] Macaws = new int[] { /* List of macaw item IDs */ };

        /// <summary>
        /// Any cockatiel
        /// </summary>
        public static readonly int[] Cockatiels = new int[] { /* List of cockatiel item IDs */ };

        /// <summary>
        /// Any dragonfly
        /// </summary>
        public static readonly int[] Dragonflies = new int[] { /* List of dragonfly item IDs */ };

        /// <summary>
        /// Any fruit
        /// </summary>
        public static readonly int[] Fruits = new int[] { /* List of fruit item IDs */ };

        /// <summary>
        /// Any balloon
        /// </summary>
        public static readonly int[] Balloons = new int[] { /* List of balloon item IDs */ };

        /// <summary>
        /// Any cloud balloon
        /// </summary>
        public static readonly int[] CloudBalloons = new int[] { /* List of cloud balloon item IDs */ };

        /// <summary>
        /// Any blizzard balloon
        /// </summary>
        public static readonly int[] BlizzardBalloons = new int[] { /* List of blizzard balloon item IDs */ };

        /// <summary>
        /// Any sandstorm balloon
        /// </summary>
        public static readonly int[] SandstormBalloons = new int[] { /* List of sandstorm balloon item IDs */ };
        #endregion

        #endregion

        /// <summary>
        /// ID and Name Mapping
        /// </summary>
        static readonly Dictionary<int, string> mapping = new()
        {
            {QuestFish, "Today's Quest Fish"},
            {AnyQuestFish, "Any Quest Fish"},
            {AnyFish, "Any Fish"},
            {AnyWood, "Any Wood"},
            {AnyTorch, "Any Torch"},
            {AnyIronBar, "Any Iron Bar"},
            {AnySand, "Any Sand Block"},
            {AnyPressurePlate, "Any Pressure Plate"},
            {AnyBird, "Any Bird"},
            {AnyScorpion, "Any Scorpion"},
            {AnySquirrel, "Any Squirrel"},
            {AnyJungleBug, "Any Jungle Bug"},
            {AnyDuck, "Any Duck"},
            {AnyButterfly, "Any Butterfly"},
            {AnyFirefly, "Any Firefly"},
            {AnySnail, "Any Snail"},
            {AnyTurtle, "Any Turtle"},
            {AnyMacaw, "Any Macaw"},
            {AnyCockatiel, "Any Cockatiel"},
            {AnyDragonfly, "Any Dragonfly"},
            {AnyFruit, "Any Fruit"},
            {AnyBalloon, "Any Balloon"},
            {AnyCloudBalloon, "Any Cloud Balloon"},
            {AnyBlizzardBalloon, "Any Blizzard Balloon"},
            {AnySandstormBalloon, "Any Sandstorm Balloon"},
            {AnyHorseshoeBalloon, "Any Horseshoe Balloon"},
            {AnyCrate, "Any Crate"},
            {AnyTombstone, "Any Tombstone"},
            {AnyGoldCritter, "Any Gold Critter"},
            {AnyParrot, "Any Parrot"}
        };

        public static string GetNameByType(int type, int stack = 1)
        {
            if (mapping.ContainsKey(type))
            {
                string s = mapping[type];

                int icon = GetIconID(type);
                if (icon != 0) s = $"{s}[i/s{stack}:{icon}]";
                if (stack > 1) s = $"{s}x{stack}";

                return s;
            }
            return "";
        }

        static int GetIconID(int type)
        {
            return type switch
            {
                QuestFish => 0,
                AnyQuestFish => 2472,
                AnyFish => 2290,
                AnyWood => 9,
                AnyTorch => 8,
                AnyIronBar => 22,
                AnySand => 169,
                AnyPressurePlate => 529,
                AnyBird => 2015,
                AnyScorpion => 2157,
                AnySquirrel => 2018,
                AnyJungleBug => 3194,
                AnyDuck => 2123,
                AnyButterfly => 1998,
                AnyFirefly => 1992,
                AnySnail => 2006,
                AnyTurtle => 4464,
                AnyMacaw => 5212,
                AnyCockatiel => 5312,
                AnyDragonfly => 4336,
                AnyFruit => 4009,
                AnyBalloon => 3738,
                AnyCloudBalloon => 389,
                AnyBlizzardBalloon => 1163,
                AnySandstormBalloon => 983,
                AnyHorseshoeBalloon => 1250,
                AnyCrate => 2334,
                AnyTombstone => 1176,
                AnyGoldCritter => 2890,
                AnyParrot => 5300,
                _ => 0,
            };
        }

        public static int GetTypeByName(string name = "")
        {
            if (string.IsNullOrEmpty(name))
                return 0;

            if (mapping.ContainsValue(name))
            {
                foreach (var obj in mapping)
                {
                    if (obj.Value == name) return obj.Key;
                }
            }

            return name switch
            {
                "Quest Fish" => AnyQuestFish,
                "Fish" => AnyFish,
                "Wood" => AnyWood,
                "Torch" => AnyTorch,
                "Iron Bar" => AnyIronBar,
                "Sand Block" => AnySand,
                "Pressure Plate" => AnyPressurePlate,
                "Bird" => AnyBird,
                "Scorpion" => AnyScorpion,
                "Squirrel" => AnySquirrel,
                "Jungle Bug" => AnyJungleBug,
                "Duck" => AnyDuck,
                "Butterfly" => AnyButterfly,
                "Firefly" => AnyFirefly,
                "Snail" => AnySnail,
                "Turtle" => AnyTurtle,
                "Macaw" => AnyMacaw,
                "Cockatiel" => AnyCockatiel,
                "Dragonfly" => AnyDragonfly,
                "Fruit" => AnyFruit,
                "Balloon" => AnyBalloon,
                "Cloud Balloon" => AnyCloudBalloon,
                "Blizzard Balloon" => AnyBlizzardBalloon,
                "Sandstorm Balloon" => AnySandstormBalloon,
                "Horseshoe Balloon" => AnyHorseshoeBalloon,
                "Crate" => AnyCrate,
                "Tombstone" => AnyTombstone,
                "Gold Critter" => AnyGoldCritter,
                "Parrot" => AnyParrot,
                _ => 0,
            };
        }

        public static int GetAnyType(int itemID)
        {
            if (Main.anglerQuestItemNetIDs.Contains(itemID)) return AnyQuestFish;
            if (Fishes.Contains(itemID)) return AnyFish;
            if (Woods.Contains(itemID)) return AnyWood;
            if (Torches.Contains(itemID)) return AnyTorch;
            if (IronBars.Contains(itemID)) return AnyIronBar;
            if (Sands.Contains(itemID)) return AnySand;
            if (PressurePlates.Contains(itemID)) return AnyPressurePlate;
            if (Birds.Contains(itemID)) return AnyBird;
            if (Scorpions.Contains(itemID)) return AnyScorpion;
            if (Squirrels.Contains(itemID)) return AnySquirrel;
            if (JungleBugs.Contains(itemID)) return AnyJungleBug;
            if (Ducks.Contains(itemID)) return AnyDuck;
            if (Butterflies.Contains(itemID)) return AnyButterfly;
            if (Fireflies.Contains(itemID)) return AnyFirefly;
            if (Snails.Contains(itemID)) return AnySnail;
            if (Turtles.Contains(itemID)) return AnyTurtle;
            if (Macaws.Contains(itemID)) return AnyMacaw;
            if (Cockatiels.Contains(itemID)) return AnyCockatiel;
            if (Dragonflies.Contains(itemID)) return AnyDragonfly;
            if (Fruits.Contains(itemID)) return AnyFruit;
            if (Balloons.Contains(itemID)) return AnyBalloon;
            if (CloudBalloons.Contains(itemID)) return AnyCloudBalloon;
            if (BlizzardBalloons.Contains(itemID)) return AnyBlizzardBalloon;
            if (SandstormBalloons.Contains(itemID)) return AnySandstormBalloon;
            if (HorseshoeBalloons.Contains(itemID)) return AnyHorseshoeBalloon;
            if (Crates.Contains(itemID)) return AnyCrate;
            if (Tombstones.Contains(itemID)) return AnyTombstone;
            if (GoldCritters.Contains(itemID)) return AnyGoldCritter;
            if (Parrots.Contains(itemID)) return AnyParrot;

            return 0;
        }
    }
}
