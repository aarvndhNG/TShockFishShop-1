using FishShop.Helper;
using System.Collections.Generic;
using System.Linq;
using Terraria;

namespace FishShop.Shop
{
    public class ForgeItem : ShopItem
    {
        public ForgeItem(ShopItemData si) : base(si)
        {
        }

        public override string CanBuy()
        {
            var msg = base.CanBuy();
            if (msg != "") return msg;

            Item forgeItem = op.TPlayer.inventory[0];
            int id = forgeItem.netID;
            if (id == 0) return "You need to place the item to be reforged in the first slot of your inventory!";
            if (!Prefix.CanHavePrefixes(forgeItem)) return $"[i:{id}] cannot be reforged";

            if (extra == "")
            {
                return "You need to specify a prefix name after the buy command, for example: /fish buy reforging Mythical";
            }
            else
            {
                byte targetPrefix = (byte)Prefix.GetPrefix(extra);
                if (targetPrefix == forgeItem.prefix)
                {
                    return "The item already has this prefix, no need to reforge!";
                }
                else if (targetPrefix == 0)
                {
                    return "Invalid prefix input! You can use numbers 1-84 instead of Chinese names.";
                }
            }

            var npc = NPCHelper.FindNearNPC(op, 107);
            if (npc == null) return "You need to be near the Goblin Tinkerer!";

            long ownedCoins = InventoryHelper.GetCoinsCount(op);
            int needCoins = ForgeCost(forgeItem, op.TPlayer, npc) * 10;
            if (ownedCoins < needCoins)
            {
                return $"Not enough money! The budget for this reforge is {utils.GetMoneyDesc(needCoins)}";
            }

            return "";
        }

        public override void DeductCost(out int costMoney, out int costFish)
        {
            var npc = NPCHelper.FindNearNPC(op, 107);
            Item forgeItem = op.TPlayer.inventory[0];
            int needCoins = ForgeCost(forgeItem, op.TPlayer, npc) * 10;
            costMoney = needCoins;
            costFish = 0;
            InventoryHelper.DeductMoney(op, needCoins);
        }

        public override void ProvideGoods()
        {
            Item forgeItem = op.TPlayer.inventory[0];
            int id = forgeItem.netID;

            Item item = new Item();
            item.SetDefaults(id);
            item.prefix = forgeItem.prefix;

            var npc = NPCHelper.FindNearNPC(op, 107);

            List<byte> history = new List<byte>();
            long totalCoins = 0;
            for (int i = 0; i < 10; i++)
            {
                totalCoins += ForgeCost(item, op.TPlayer, npc);
                item.ResetPrefix();
                item.Prefix(-2);
                history.Add(item.prefix);
            }

            byte targetPrefix = (byte)Prefix.GetPrefix(extra);
            var last = targetPrefix;

            int needCoins = ForgeCost(forgeItem, op.TPlayer, npc) * 10;
            var coinsTips = $"Cost: {utils.GetMoneyDesc(needCoins)} | Balance: {InventoryHelper.GetCoinsCountDesc(op)}";
            var li = history.Select(p => Prefix.GetName(p));
            var hisStr = " | " + string.Join(" ", li);
            if (history.Contains(targetPrefix))
            {
                var luckyNum = history.IndexOf(targetPrefix) + 1;
                op.SendSuccessMessage($"Reforge Successful: [i/p{targetPrefix}:{id}] | {coinsTips}{hisStr}");
            }
            else
            {
                last = history.Last();
                var tips = "";
                if (!byte.TryParse(extra, out byte tempPrefix))
                {
                    tips = $" | To reforge again, you can input /fish forge {targetPrefix}";
                }
                op.SendInfoMessage($"Reforge Completed: [i/p{last}:{id}] | {coinsTips}{tips}{hisStr}");
            }

            forgeItem.prefix = last;
            utils.PlayerSlot(op, forgeItem, 0);

            op.SendData(PacketTypes.PlayerSlot, "", op.Index, 0, last);

            var remain = needCoins - totalCoins;
            if (remain > 0)
            {
                InventoryHelper.Refund(op, remain);
                op.SendInfoMessage($"Budget surplus, refunding {utils.GetMoneyDesc(remain)}");
            }
            op.SendInfoMessage($"{needCoins}   {totalCoins}");
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
