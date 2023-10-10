using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using TShockAPI;

namespace FishShop
{
    public class InventoryHelper
    {
        // Get Balance
        public static long GetCoinsCount(TSPlayer player)
        {
            bool overFlowing;
            long num = Terraria.Utils.CoinsCount(out overFlowing, player.TPlayer.inventory, 58, 57, 56, 55, 54);
            long num2 = Terraria.Utils.CoinsCount(out overFlowing, player.TPlayer.bank.item);
            long num3 = Terraria.Utils.CoinsCount(out overFlowing, player.TPlayer.bank2.item);
            long num4 = Terraria.Utils.CoinsCount(out overFlowing, player.TPlayer.bank3.item);
            long num5 = Terraria.Utils.CoinsCount(out overFlowing, player.TPlayer.bank4.item);
            long total = ((int)Terraria.Utils.CoinsCombineStacks(out overFlowing, num, num2, num3, num4, num5));

            return total;
        }

        // Balance Description
        public static string GetCoinsCountDesc(TSPlayer player, bool tagStyle = true)
        {
            long total = GetCoinsCount(player);
            return utils.GetMoneyDesc(total, tagStyle);
        }

        #region Check If Enough Money
        public static bool CheckCost(TSPlayer player, ShopItemData shopItemData, int amount, out string msg)
        {
            // Get the IDs of items to be deducted
            List<ItemData> costItems = shopItemData.GetCostItem(amount);

            msg = "";

            // Calculate the cost in money
            int costMoney = shopItemData.GetCostMoney(amount);

            // Builders get a 10% discount
            if (IsBuilder(player))
            {
                float discountMoney = costMoney * 0.1f;
                costMoney = (int)Math.Ceiling(discountMoney);
            }

            if (GetCoinsCount(player) < costMoney)
            {
                msg = "Not enough money";
                return false;
            }

            // Check for the corresponding items and quantities
            Item itemNet;
            ItemData itemData;
            for (int i = 0; i < NetItem.MaxInventory; i++)
            {
                if (i >= NetItem.InventorySlots)
                    break;

                itemNet = player.TPlayer.inventory[i];
                if (itemNet.stack < 1)
                    continue;

                itemData = shopItemData.PickCostItem(costItems, itemNet.netID);
                if (itemData.id != 0)
                {
                    if (itemNet.stack >= itemData.stack)
                    {
                        costItems.Remove(itemData);
                    }
                    else
                    {
                        itemData.stack -= itemNet.stack;
                    }
                }
            }
            if (costItems.Count > 0)
            {
                msg = "Not enough items";
                return false;
            }

            // Any other items...
            // ...

            return true;
        }
        #endregion

        /// <summary>
        /// Refund Money
        /// </summary>
        public static void Refund(TSPlayer op, long price)
        {
            utils.GetMoneyStack(price, out int stack1, out int stack2, out int stack3, out int stack4);
            if (stack1 > 0) op.GiveItem(71, stack1);
            if (stack2 > 0) op.GiveItem(72, stack2);
            if (stack3 > 0) op.GiveItem(73, stack3);
            if (stack4 > 0) op.GiveItem(74, stack4);
        }

        #region Deduct Items
        /// <summary>
        /// Deduct Items
        /// </summary>
        public static void DeductCost(TSPlayer player, ShopItemData shopItemData, int amount, out int costMoney, out int costFish)
        {
            // Get the items to be deducted
            List<ItemData> costItems = shopItemData.GetCostItem(amount);

            Item itemNet;
            ItemData itemData;
            costFish = 0;
            for (int i = 0; i < NetItem.MaxInventory; i++)
            {
                if (i >= NetItem.InventorySlots)
                    break;

                itemNet = player.TPlayer.inventory[i];
                if (itemNet.stack < 1)
                    continue;
                if (itemNet.IsACoin)
                    continue;

                itemData = shopItemData.PickCostItem(costItems, itemNet.netID);
                if (itemData.id != 0)
                {
                    // Record fish deduction
                    bool IsFish = CostID.Fishes.Contains(itemNet.netID);
                    int stack;
                    if (itemNet.stack >= itemData.stack)
                    {
                        stack = itemData.stack;

                        itemNet.stack -= stack;
                        costItems.Remove(itemData);

                        if (IsFish) costFish += stack;
                    }
                    else
                    {
                        stack = itemNet.stack;

                        itemData.stack -= stack;
                        itemNet.stack = 0;

                        if (IsFish) costFish += stack;
                    }
                    utils.PlayerSlot(player, itemNet, i);
                }
            }
            if (costItems.Count > 0)
            {
                utils.Log($"Failed to deduct {costItems.Count} items!");
            }

            // Deduct money
            costMoney = shopItemData.GetCostMoney(amount);
            // Builders get a 10% discount
            if (IsBuilder(player))
            {
                float discountMoney = costMoney * 0.1f;
                costMoney = (int)Math.Ceiling(discountMoney);
            }

            bool success = DeductMoney(player, costMoney);
            if (!success)
            {
                utils.Log($"Failed to deduct gold! Amount: {costMoney} copper");
            }

            // Execute commands
            List<string> cmds = shopItemData.GetCostCMD();
            for (int i = 0; i < amount; i++)
            {
                foreach (string cmd in cmds)
                {
                    CmdHelper.ExecuteRawCmd(player, cmd);
                }
            }
        }

        public static bool IsBuilder(TSPlayer op)
        {
            return op.Group.Name == "builder" || op.Group.Name == "architect";
        }
        #endregion

        #region Deduct Money
        public static bool DeductMoney(TSPlayer player, int price)
        {
            int b1 = 0;
            int b2 = 0;
            int b3 = 0;
            int b4 = 0;
            List<Item> items = new();
            List<int> indexs = new();

            // Find the index of the current currency
            void record(Item _item, int _index)
            {
                if (_item.IsACoin)
                {
                    indexs.Add(_index);
                    items.Add(_item);
                }
            }
            for (int i = 0; i < 260; i++)
            {
                if (i < 54)
                {
                    record(player.TPlayer.inventory[i], i);
                }
                else if (i >= 99 && i < 139)
                {
                    record(player.TPlayer.bank.item[b1], i);
                    b1++;
                }
                else if (i >= 139 && i < 179)
                {
                    record(player.TPlayer.bank2.item[b2], i);
                    b2++;
                }
                else if (i >= 180 && i < 220)
                {
                    record(player.TPlayer.bank3.item[b3], i);
                    b3++;
                }
                else if (i >= 220 && i < 260)
                {
                    record(player.TPlayer.bank4.item[b4], i);
                    b4++;
                }
            }

            // Buy the item
            bool success = player.TPlayer.BuyItem(price);

            // Find the index of the currency after deduction
            b1 = 0;
            b2 = 0;
            b3 = 0;
            b4 = 0;
            List<Item> items2 = new();
            List<int> indexs2 = new();

            void record2(Item _item, int _index)
            {
                if (_item.IsACoin)
                {
                    indexs2.Add(_index);
                    items2.Add(_item);
                    if (indexs.Contains(_index))
                    {
                        var newIndex = indexs.IndexOf(_index);
                        indexs.RemoveAt(newIndex);
                        items.RemoveAt(newIndex);
                    }
                }
            }

            for (int i = 0; i < 260; i++)
            {
                if (i < 54)
                {
                    record2(player.TPlayer.inventory[i], i);
                }
                else if (i >= 99 && i < 139)
                {
                    record2(player.TPlayer.bank.item[b1], i);
                    b1++;
                }
                else if (i >= 139 && i < 179)
                {
                    record2(player.TPlayer.bank2.item[b2], i);
                    b2++;
                }
                else if (i >= 180 && i < 220)
                {
                    record2(player.TPlayer.bank3.item[b3], i);
                    b3++;
                }
                else if (i >= 220 && i < 260)
                {
                    record2(player.TPlayer.bank4.item[b4], i);
                    b4++;
                }
            }

            indexs.AddRange(indexs2);
            items.AddRange(items2);

            for (int i = 0; i < indexs.Count; i++)
            {
                utils.PlayerSlot(player, items[i], indexs[i]);
            }
            return success;
        }
        #endregion
    }
}
