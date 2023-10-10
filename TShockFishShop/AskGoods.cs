using System.Collections.Generic;
using System.Linq;
using Terraria;
using TShockAPI;

namespace FishShop
{
    public partial class Plugin
    {
        // Inquiry
        static void AskGoods(CommandArgs args)
        {
            // Check if the shop is ready
            if (!ShopIsReady(args.Player))
            {
                return;
            }

            if (args.Parameters.Count < 2)
            {
                args.Player.SendErrorMessage("You need to input the item number, for example: /fish ask 1");
                return;
            }

            TSPlayer op = args.Player;

            List<ShopItemData> FindGoods(int _id, string _prefix = "")
            {
                return _config.shop.Where(obj => obj.id == _id && (_prefix == "" || obj.prefix == _prefix)).ToList();
            }

            string itemNameOrId = args.Parameters[1];
            List<ShopItemData> goods = new();

            if (int.TryParse(itemNameOrId, out int goodsSerial))
            {
                // Validity check for the item number
                int count = _config.shop.Count;
                if (goodsSerial <= 0 || goodsSerial > count)
                {
                    op.SendErrorMessage($"The maximum number is: {count}, please use /fish list to view the shelf.");
                    return;
                }
                goods.Add(_config.shop[goodsSerial - 1]);
            }
            else
            {
                // Match by name and get the item's ID
                int customID = IDSet.GetIDByName(itemNameOrId);
                if (customID != 0)
                {
                    goods = FindGoods(customID);
                }
                else
                {
                    List<Item> matchedItems = TShock.Utils.GetItemByIdOrName(itemNameOrId);
                    if (matchedItems.Count == 0)
                    {
                        op.SendErrorMessage($"Item name/item ID: {itemNameOrId} is incorrect");
                        return;
                    }
                    foreach (Item item in matchedItems)
                    {
                        goods.AddRange(FindGoods(item.netID));
                    }
                }
            }

            /// <summary>
            /// Item description
            /// </summary>
            string Detail(ShopItemData d)
            {
                bool RealPlayer = op != null && op.RealPlayer;

                string iconDesc = d.GetIcon();
                string shopDesc = d.GetItemDesc();
                string costDesc = d.GetCostDesc();
                string s = $"{d.serial}.{iconDesc}{shopDesc} = {costDesc}";

                string unlockDesc = d.GetUnlockDesc();
                if (unlockDesc != "")
                {
                    s += $"\nUnlock Condition: Requires {unlockDesc}";
                }

                string comment = d.GetComment();
                if (comment != "")
                {
                    s += $"\nItem Comment: {comment}";
                }

                if (RealPlayer)
                {
                    string limitDesc = d.GetLimitDesc(op);
                    if (limitDesc != "")
                    {
                        s += $"\nItem Purchase Limit: {limitDesc}";
                    }
                }

                string allowGroupDesc = d.GetAllowGroupDesc();
                if (allowGroupDesc != "")
                {
                    s += $"\nUser Group Restriction: {allowGroupDesc}";
                }

                if (RealPlayer)
                {
                    string remainDesc = InventoryHelper.GetCoinsCountDesc(op);
                    if (!string.IsNullOrEmpty(remainDesc))
                    {
                        s += $"\nYour Balance: {remainDesc}";
                    }
                }
                return s;
            }

            foreach (ShopItemData shopItemData in goods)
            {
                var s = Detail(shopItemData);
                if (s != "")
                    op.SendInfoMessage(s);
            }

            if (goods.Count == 0)
            {
                op.SendErrorMessage($"No items with the name or ID {itemNameOrId} have been sold!");
            }
        }
    }
}
