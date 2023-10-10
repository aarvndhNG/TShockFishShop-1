using System;
using TShockAPI;

namespace FishShop
{
    public partial class Plugin
    {
        // View the shop
        static void ListGoods(CommandArgs args)
        {
            if (!ShopIsReady(args.Player))
            {
                return;
            }

            // Update the shelf
            float num = (float)_config.shop.Count / _config.pageSlots;
            int totalPage = (int)Math.Ceiling(num);

            // Input page number
            if (args.Parameters.Count > 1 && int.TryParse(args.Parameters[1], out int pageNum))
            {
                if (pageNum > totalPage)
                {
                    pageNum = totalPage;
                }
                else if (pageNum <= 0)
                {
                    pageNum = 1;
                }
            }
            else
            {
                pageNum = 1;
            }

            int totalSlots = _config.pageSlots * pageNum;

            // Display content of the specified page
            string msg = "";
            int rowCount = 0;
            int pageCount = 0;
            int totalCount = 0;
            int startSlot = _config.pageSlots * (pageNum - 1);
            for (int i = 0; i < _config.shop.Count; i++)
            {
                if (i < startSlot)
                {
                    continue;
                }

                rowCount++;
                pageCount++;

                msg += $"{_config.shop[i].GoodsName()}  ";

                totalCount = i + 1;
                if (i >= (totalSlots - 1))
                {
                    break;
                }

                if (rowCount != 1 && rowCount == _config.rowSlots)
                {
                    rowCount = 0;
                    msg += "\n";
                }
            }

            if (totalCount < _config.shop.Count)
            {
                msg += $"\n[c/96FF0A:Enter /fish list {pageNum + 1} to see more.]";
            }

            if (msg == "")
            {
                msg = "Today, we're just here to be cute, not to sell anything! ɜː";
            }
            else
            {
                msg = $"$"[c/96FF0A: Welcome to [{_config.name}], shelf ({pageNum}/{totalPage}): ]\n" + msg;
            }

            if (args.Player != null)
            {
                args.Player.SendInfoMessage(msg);
            }
            else
            {
                utils.Log(msg);
            }
        }
    }
}
