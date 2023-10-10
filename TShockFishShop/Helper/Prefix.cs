using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace FishShop.Helper
{
    public class Prefix
    {
        static readonly Dictionary<int, string> _prefixes = new()
        {
            {1, "Large"},
            {2, "Huge"},
            {3, "Dangerous"},
            {4, "Ferocious"},
            {5, "Sharp"},
            {6, "Pointed"},
            {7, "Tiny"},
            {8, "Terrifying"},
            {9, "Small"},
            {10, "Dull"},
            {11, "Unlucky"},
            {12, "Clumsy"},
            {13, "Shameful"},
            {14, "Heavy"},
            {15, "Light"},
            {16, "Precise"},
            {17, "Swift"},
            {18, "Rapid"},
            {19, "Terrifying"},
            {20, "Deadly"},
            {21, "Reliable"},
            {22, "Dreadful"},
            {23, "Feeble"},
            {24, "Clunky"},
            {25, "Powerful"},
            {26, "Mysterious"},
            {27, "Exquisite"},
            {28, "Masterful"},
            {29, "Awkward"},
            {30, "Ignorant"},
            {31, "Chaotic"},
            {32, "Mighty"},
            {33, "Forbidden"},
            {34, "Celestial"},
            {35, "Furious"},
            {36, "Sharp"},
            {37, "High-End"},
            {38, "Strong"},
            {39, "Fractured"},
            {40, "Broken"},
            {41, "Rough"},
            {42, "Swift"},
            {43, "Deadly II"},
            {44, "Agile"},
            {45, "Nimble"},
            {46, "Savage"},
            {47, "Slow"},
            {48, "Dull"},
            {49, "Stupid"},
            {50, "Annoying"},
            {51, "Dangerous"},
            {52, "Manic"},
            {53, "Wounding"},
            {54, "Potent"},
            {55, "Rough"},
            {56, "Weak"},
            {57, "Merciless"},
            {58, "Furious"},
            {59, "Godly"},
            {60, "Demonic"},
            {61, "Fanatical"},
            {62, "Hard"},
            {63, "Guarding"},
            {64, "Armored"},
            {65, "Warding"},
            {66, "Mystic"},
            {67, "Accurate"},
            {68, "Lucky"},
            {69, "Serrated"},
            {70, "Spiked"},
            {71, "Angry"},
            {72, "Wicked"},
            {73, "Nimble"},
            {74, "Quick"},
            {75, "Rapid II"},
            {76, "Swift II"},
            {77, "Wild"},
            {78, "Rash"},
            {79, "Valiant"},
            {80, "Violent"},
            {81, "Legendary"},
            {82, "Illusory"},
            {83, "Mythical"},
            {84, "Legendary II"},
        };

        public static int GetPrefix(string idOrName)
        {
            if (int.TryParse(idOrName, out int num))
            {
                if (num <= 84 && num > 0)
                {
                    return num;
                }
                else
                {
                    return 0;
                }
            }
            switch (idOrName)
            {
                case "Light": return 15;
                case "Weak": return 56;
            }
            var li = _prefixes.Where(obj => obj.Value == idOrName);
            if (li.Any())
            {
                return li.First().Key;
            }
            return 0;
        }

        public static string GetName(int prefix)
        {
            if (_prefixes.ContainsKey(prefix))
            {
                return _prefixes[prefix];
            }
            return "";
        }

        public static bool CanHavePrefixes(Item item)
        {
            if (item.type != 0 && item.maxStack == 1)
            {
                if (item.damage <= 0)
                {
                    return item.accessory;
                }
                return true;
            }
            return false;
        }
    }
}
