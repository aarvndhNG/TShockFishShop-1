using FishShop.Record;
using Terraria;
using TShockAPI;

namespace FishShop.Shop
{
    public class ShopItem
    {
        /// <summary>
        /// Data for the item in the shop.
        /// </summary>
        protected readonly ShopItemData shopItemData;

        public TSPlayer op;
        public int amount = 1;
        public string extra = "";

        public ShopItem(ShopItemData si)
        {
            shopItemData = si;
        }

        /// <summary>
        /// Check if the item can be bought.
        /// </summary>
        virtual public string CanBuy()
        {
            // Common checks and unlock conditions
            string msg = "";
            foreach (ItemData d in shopItemData.unlock)
            {
                if (!UnlockID.CheckUnlock(d, op, out string s))
                {
                    msg += " " + s;
                }
            }

            // User group
            bool groupPass = op.HasPermission(Permissions.AllowGroup) || shopItemData.allowGroup.Count == 0;
            if (!groupPass) groupPass = shopItemData.allowGroup.Contains(op.Group.Name);
            if (!groupPass)
            {
                msg += $" You are not in the {string.Join(" or ", shopItemData.allowGroup)} group(s)";
            }

            if (msg != "")
            {
                return $"Cannot purchase at the moment due to: {msg}";
            }

            int id = shopItemData.id;

            // Purchase limit
            bool CheckLimit()
            {
                if (string.IsNullOrEmpty(op.Name))
                    return true;

                var limit = shopItemData.limit;
                var serverLimit = shopItemData.serverLimit;

                if (limit > 0 && Records.GetPlayerRecord(op, id) >= limit)
                    return false;

                if (serverLimit > 0 && Records.CountShopItemRecord(id) >= serverLimit)
                    return false;

                return true;
            }

            if (!CheckLimit())
            {
                return "Oops, this item is so popular that it's sold out.";
            }

            // Time-based restrictions
            if (Main.dayTime)
            {
                if (!shopItemData.DayCanBuyItem())
                {
                    return "Can only be purchased at night!";
                }
            }
            else
            {
                if (!shopItemData.NightCanBuyItem())
                {
                    return "Can only be purchased during the day!";
                }
            }

            // Cannot buy buff-type items when dead
            if (op.Dead && !shopItemData.DeadCanBuyItem())
            {
                return "Please wait until you are revived to purchase this item!";
            }

            return "";
        }

        /// <summary>
        /// Provide the goods or service.
        /// </summary>
        virtual public void ProvideGoods()
        {
        }

        /// <summary>
        /// Check if the player has enough money.
        /// </summary>
        virtual public string CheckCost()
        {
            InventoryHelper.CheckCost(op, shopItemData, amount, out string msg);
            return msg;
        }

        /// <summary>
        /// Deduct money for the purchase.
        /// </summary>
        virtual public void DeductCost(out int costMoney, out int costFish)
        {
            // Deduct the cost
            InventoryHelper.DeductCost(op, shopItemData, amount, out costMoney, out costFish);
        }

        /// <summary>
        /// Debug log for the purchase.
        /// </summary>
        public void BuyDebug(TSPlayer op, int amount = 1)
        {
            var d = shopItemData;
            // [Log Recording]
            utils.Log(string.Format("{0} wants to buy {1} {2}(s)", op.Name, amount, d.GetItemDesc()));
            utils.Log($"item: {d.name} {d.id} {d.stack} {d.prefix}");
            foreach (ItemData _d in d.unlock)
            {
                utils.Log($"unlock: {_d.name} {_d.id} {_d.stack}");
            }
            foreach (ItemData _d in d.cost)
            {
                utils.Log($"cost: {_d.name} {_d.id} {_d.stack}");
            }
            utils.Log($"Balance: {InventoryHelper.GetCoinsCountDesc(op, false)}");
        }
    }
}
