using FishShop.Helper;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.ID;
using TShockAPI;

namespace FishShop
{
    public class utils
    {
        public static List<string> MoonPhases = new() { "Full Moon", "Waning Gibbous", "Last Quarter", "Waning Crescent", "New Moon", "Waxing Crescent", "First Quarter", "Waxing Gibbous" };

        public static void Init()
        {
            // Enable support for the first fractal
            // Clear the list of deprecated items
            Array.Clear(ItemID.Sets.Deprecated, 0, ItemID.Sets.Deprecated.Length - 1);
        }

        public static string GetItemDesc(int id = 0, int stack = 1, string prefix = "")
        {
            if (id == 0)
                return "";

            string s = "";

            // https://terraria.fandom.com/wiki/Chat
            // [i:29]   Quantity
            // [i/s10:29]   Quantity
            // [i/p57:4]    Prefix

            // IDs from -24 to 5124 are original Terraria item IDs
            // IDs less than -24 are custom IDs from this plugin
            if (id < -24)
            {
                s = IDSet.GetNameByID(id, prefix, stack);
            }
            else
            {
                if (stack > 1)
                {
                    s = $"[i/s{stack}:{id}]";
                }
                else
                {
                    if (int.TryParse(prefix, out int num) && num != 0)
                        s = $"[i/p{GetPrefixInt(prefix)}:{id}]";
                    else
                        s = $"[i:{id}]";
                }
            }

            return s;
        }

        private static int GetPrefixInt(string prefix)
        {
            if (int.TryParse(prefix, out int num))
                return num;
            else
                return Prefix.GetPrefix(prefix);
        }

        public static string GetMoneyDesc(long price, bool tagStyle = true)
        {
            List<string> li = new();

            // Platinum Coins
            float num = price / 1000000;
            int stack = (int)Math.Floor(num);
            if (stack > 0)
            {
                price -= stack * 1000000;

                if (!tagStyle)
                {
                    li.Add($"{stack} Platinum");
                }
                else
                {
                    while (stack > 9999)
                    {
                        stack -= 9999;
                        li.Add("[i/s{9999}:74]");
                    }
                    li.Add($"[i/s{stack}:74]");
                }
            }

            // Gold Coins
            num = price / 10000;
            stack = (int)Math.Floor(num);
            if (stack > 0)
            {
                price -= stack * 10000;
                li.Add(tagStyle ? $"[i/s{stack}:73]" : $" {stack} Gold");
            }

            // Silver Coins
            num = price / 100;
            stack = (int)Math.Floor(num);
            if (stack > 0)
            {
                price -= stack * 100;
                li.Add(tagStyle ? $"[i/s{stack}:72]" : $" {stack} Silver");
            }

            // Copper Coins
            if (price > 0)
            {
                li.Add(tagStyle ? $"[i/s{stack}:71]" : $" {stack} Copper");
            }

            return string.Join("", li);
        }

        public static void GetMoneyStack(long price, out int stack1, out int stack2, out int stack3, out int stack4)
        {
            // Platinum Coins
            float num = price / 1000000;
            stack4 = (int)Math.Floor(num);

            // Gold Coins
            num = price / 10000;
            stack3 = (int)Math.Floor(num);

            // Silver Coins
            num = price / 100;
            stack2 = (int)Math.Floor(num);

            // Copper Coins
            stack1 = (int)price;
        }

        public static string AlignZero(int num)
        {
            if (num < 10) return $"00{num}";
            else if (num < 100) return $"0{num}";
            else return $"{num}";
        }

        public static void PlayerSlot(TSPlayer player, Item item, int slotIndex)
        {
            NetMessage.SendData((int)PacketTypes.PlayerSlot, -1, -1, null, player.Index, slotIndex);
            NetMessage.SendData((int)PacketTypes.PlayerSlot, player.Index, -1, null, player.Index, slotIndex);
        }

        public static int GetUnixTimestamp
        {
            get { return (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds; }
        }

        public static Rectangle GetScreen(TSPlayer op) { return GetScreen(op.TileX, op.TileY); }
        public static Rectangle GetScreen(int playerX, int playerY) { return new Rectangle(playerX - 61, playerY - 34 + 3, 122, 68); }

        public static int InvalidItemID { get { return -24; } }

        public static string FromEmbeddedPath(string path)
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(path);
            StreamReader streamReader = new(stream);
            return streamReader.ReadToEnd();
        }

        public static void Log(string msg) { TShock.Log.ConsoleInfo("[fish]" + msg); }
    }
}
