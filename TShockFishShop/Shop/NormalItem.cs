using Terraria;
using TShockAPI;

namespace FishShop.Shop
{
    public class NormalItem : ShopItem
    {
        public NormalItem(ShopItemData si) : base(si)
        {
        }

        public override string CanBuy()
        {
            var msg = base.CanBuy();
            if (msg != "") return msg;

            // Fallen Stars can only be bought at night
            if (shopItemData.id == 75 && Main.dayTime)
            {
                return "You can only purchase Fallen Stars at night!";
            }

            if (!op.InventorySlotAvailable)
            {
                return "Your inventory is full, you cannot make the purchase!";
            }

            return "";
        }

        public override void ProvideGoods()
        {
            // Provide the item
            op.GiveItem(shopItemData.id, shopItemData.stack * amount, shopItemData.GetPrefixInt());
        }
    }
}
