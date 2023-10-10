using System.Collections.Generic;
using System.Linq;
using FishShop.Helper;
using Terraria;
using TShockAPI;

namespace FishShop
{
    public class ForgeHelper
    {
        public static void Manage(CommandArgs args)
        {
            args.Parameters.RemoveAt(0);
            if (!args.Player.RealPlayer)
            {
                args.Player.SendErrorMessage("This command needs to be executed in-game!");
                return;
            }

            List<string> msgs = new List<string>();
            Item forgeItem = args.Player.TPlayer.inventory[0];
            int id = forgeItem.netID;
            if (id == 0)
            {
                msgs.Add("The item to be reforged must be placed in the first slot of your inventory!");
            }
            else if (!Prefix.CanHavePrefixes(forgeItem))
            {
                msgs.Add($"[i:{id}] cannot be reforged.");
            }

            byte targetPrefix = 0;
            if (args.Parameters.Count == 0)
            {
                msgs.Add("You need to specify a prefix, e.g., /fish forge Unreal");
            }
            else
            {
                targetPrefix = (byte)Prefix.GetPrefix(args.Parameters[0]);
                if (targetPrefix == forgeItem.prefix)
                {
                    args.Player.SendInfoMessage("The item already has this prefix, no need to reforge!");
                    return;
                }
                else if (targetPrefix == 0)
                {
                    msgs.Add("Invalid prefix input! You can use numbers 1-84 instead of Chinese names.");
                }
            }
            var npc = NPCHelper.FindNearNPC(args.Player, 107);
            if (npc == null)
            {
                msgs.Add("You need to be near the Goblin Tinkerer!");
            }
            if (msgs.Count > 0)
            {
                args.Player.SendInfoMessage(string.Join("\n", msgs));
                return;
            }

            // Deduct money
            long ownedCoins = InventoryHelper.GetCoinsCount(args.Player);
            int needCoins = ForgeCost(forgeItem, args.Player.TPlayer, npc) * 10;
            if (ownedCoins < needCoins)
            {
                args.Player.SendInfoMessage($"Not enough money! The budget for this reforging is {utils.GetMoneyDesc(needCoins)}");
                return;
            }
            InventoryHelper.DeductMoney(args.Player, needCoins);

            Item item = new Item();
            item.SetDefaults(id);
            item.prefix = forgeItem.prefix;

            List<byte> history = new List<byte>();
            long totalCoins = 0;
            for (int i = 0; i < 10; i++)
            {
                totalCoins += ForgeCost(item, args.Player.TPlayer, npc);
                item.ResetPrefix();
                item.Prefix(-2);
                history.Add(item.prefix);
            }

            var last = targetPrefix;
            var coinsTips = $"Spent: {utils.GetMoneyDesc(needCoins)} | Balance: {InventoryHelper.GetCoinsCountDesc(args.Player)}";
            var li = history.Select(p => Prefix.GetName(p));
            var hisStr = " | " + string.Join(" ", li);
            if (history.Contains(targetPrefix))
            {
                var luckyNum = history.IndexOf(targetPrefix) + 1;
                args.Player.SendSuccessMessage($"Reforged successfully: [i/p{targetPrefix}:{id}] | {coinsTips}{hisStr}");
            }
            else
            {
                last = history.Last();
                var tips = "";
                if (!byte.TryParse(args.Parameters[0], out byte tempPrefix))
                {
                    tips = $" | You can reforge again using /fish forge {targetPrefix}";
                }
                args.Player.SendInfoMessage($"Reforge completed: [i/p{last}:{id}] | {coinsTips}{tips}{hisStr}");
            }

            // Update item prefix
            forgeItem.prefix = last;
            utils.PlayerSlot(args.Player, forgeItem, 0);
            args.Player.SendData(PacketTypes.PlayerSlot, "", args.Player.Index, 0, last);

            // Refund remaining money
            var remain = needCoins - totalCoins;
            if (remain > 0)
            {
                InventoryHelper.Refund(args.Player, remain);
                args.Player.SendInfoMessage($"Budget surplus, refunded {utils.GetMoneyDesc(remain)}");
            }
            args.Player.SendInfoMessage($"{needCoins}   {totalCoins}");
        }

        static int ForgeCost(Item item, Player plr, NPC npc)
        {
            int coins = item.value;
            if (plr.discountAvailable)
            {
                coins = (int)(coins * 0.8);
            }
            var settings = Main.ShopHelper.GetShoppingSettings(plr, npc);
            coins = (int)(coins * settings.PriceAdjustment);
            coins /= 3;

            return coins;
        }
    }
}
