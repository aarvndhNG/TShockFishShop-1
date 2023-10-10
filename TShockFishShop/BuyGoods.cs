using FishShop.Record;
using FishShop.Shop;
using System;
using Terraria;
using TShockAPI;

namespace FishShop
{
    public partial class Plugin
    {
        // Purchase
        static void BuyGoods(CommandArgs args)
        {
            if (!args.Player.RealPlayer)
            {
                args.Player.SendErrorMessage("This command needs to be executed in-game!");
                return;
            }

            // Check if the shop is ready
            if (!ShopIsReady(args.Player))
            {
                return;
            }

            TSPlayer op = args.Player;
            if (args.Parameters.Count < 2)
            {
                op.SendErrorMessage("You need to input the item name or item number, for example: /fish buy 1 or /fish buy Life Crystal");
                return;
            }

            // Find the corresponding item
            if (int.TryParse(args.Parameters[1], out int goodsSerial))
            {
                // Validity check for the item number
                int count = _config.shop.Count;
                if (goodsSerial <= 0 || goodsSerial > count)
                {
                    op.SendErrorMessage($"The maximum number is: {count}, please use /fish list to view the shelf.");
                    return;
                }
            }
            else
            {
                // Match by name and get the item's ID
                int goodsID = IDSet.GetIDByName(args.Parameters[1]);
                if (goodsID != 0)
                {
                    for (int i = 0; i < _config.shop.Count; i++)
                    {
                        if (_config.shop[i].id == goodsID)
                        {
                            goodsSerial = i + 1;
                            break;
                        }
                    }
                }

                if (goodsSerial == 0)
                {
                    op.SendErrorMessage($"No item with the name {args.Parameters[1]} found.");
                    return;
                }
            }
            ShopItemData shopItemData = _config.shop[goodsSerial - 1];

            // Purchase quantity / extra parameter
            int amount = 1;
            string extra = "";
            if (args.Parameters.Count > 2)
            {
                int.TryParse(args.Parameters[2], out amount);
                extra = args.Parameters[2];
            }
            if (amount < 1)
            {
                amount = 1;
            }
            // For items that can be purchased only one at a time
            amount = Math.Min(amount, shopItemData.BuyMax());
            if (amount < 1) amount = 1;

            // Create shop item (some logic processing)
            ShopItem shopItem = ShopItemCreate.Create(shopItemData);
            shopItem.op = args.Player;
            shopItem.amount = amount;
            shopItem.extra = extra;

            // Check if the purchase is valid
            string result = shopItem.CanBuy();
            if (result != "")
            {
                op.SendInfoMessage(result);
                return;
            }

            // Check item stacking limit [to be optimized]
            if (shopItemData.id > 0)
            {
                Item itemNet = new Item();
                itemNet.SetDefaults(shopItemData.id);
                if (shopItemData.stack * amount > itemNet.maxStack)
                {
                    float num = itemNet.maxStack / shopItemData.stack;
                    amount = (int)Math.Floor(num);
                    if (amount == 0)
                    {
                        op.SendErrorMessage($"[Fish Shop] This item has incorrect stack quantity configuration, name={shopItemData.name}, id={shopItemData.id}, stack={shopItemData.stack}");
                        return;
                    }
                }
            }

            // Inquiry
            string msg = shopItem.CheckCost();
            if (msg == "")
            {
                // Deduct money
                shopItem.DeductCost(out int costMoney, out int costFish);

                // Provide goods/services
                shopItem.ProvideGoods();

                string s = "";
                if (InventoryHelper.IsBuilder(op))
                {
                    s = $"(You are a builder, enjoying a 10% discount, only paying {utils.GetMoneyDesc(costMoney)})";
                }

                msg = $"You bought {amount} {shopItemData.GetItemDesc()} | Cost: {shopItemData.GetCostDesc(amount)}{s} | Balance: {InventoryHelper.GetCoinsCountDesc(op)}";
                op.SendSuccessMessage(msg);
                utils.Log($"{op.Name} bought {shopItemData.GetItemDesc()}");
                Records.Record(op, shopItemData, amount, costMoney, costFish);
            }
            else
            {
                op.SendInfoMessage($"Purchase failed because: {msg}, please use /fish ask {goodsSerial} to check the purchase conditions.");
            }
        }
    }
}
