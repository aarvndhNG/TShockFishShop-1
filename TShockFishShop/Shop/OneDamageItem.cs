using TShockAPI;

namespace FishShop.Shop;


/// <summary>
/// 灵犀飞鱼
/// </summary>
public class OneDamageItem : ShopItem
{
    public OneDamageItem(ShopItemData si) : base(si)
    {
    }

    public override string CanBuy()
    {
        var msg = base.CanBuy();
        if (msg != "") return msg;

        if (!NPCHelper.AnyBoss())
        {
            return "There is currently no boss!";
        }

        return "";
    }

    public override void ProvideGoods()
    {
    }


}
