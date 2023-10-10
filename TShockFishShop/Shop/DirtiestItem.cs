using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using TShockAPI;

namespace FishShop.Shop
{
    public class DirtiestItem : ShopItem
    {
        public Point posDirt = new Point();
        public Point posPoop = new Point();

        public DirtiestItem(ShopItemData si) : base(si)
        {
        }

        public override string CanBuy()
        {
            var msg = base.CanBuy();
            if (msg != "") return msg;

            if (!CheckDirtiestMatrix(op)) return "Couldn't find a stink matrix nearby you! (7x7 empty)";
            return "";
        }

        public override void ProvideGoods()
        {
            bool flag = FindDirtiest();
            MoveDirtiest(flag);
            TSPlayer.All.SendInfoMessage($"{op.Name} is performing a [i:5395]stinky ritual[i:5395]");
            if (flag)
                op.SendSuccessMessage("Stinky ritual completed, [i:5400]Dirtiest Block has been generated (σﾟ∀ﾟ)σ");
            else
                op.SendErrorMessage("Oops, couldn't find the [i:5400]Dirtiest Block anywhere in the world o(´^｀)o");
        }

        bool CheckDirtiestMatrix(TSPlayer op)
        {
            Rectangle rect = utils.GetScreen(op);
            for (int x = rect.X; x < rect.Right; x++)
            {
                for (int y = rect.Y; y < rect.Bottom; y++)
                {
                    ITile tile = Main.tile[x, y];
                    if (!tile.active()) continue;
                    if (tile.type == TileID.PoopBlock)
                    {
                        if (FindDirtiestMatrix(x, y))
                        {
                            posPoop = new Point(x, y);
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        bool FindDirtiestMatrix(int tileX, int tileY)
        {
            int x;
            int y;
            for (int i = 0; i < 49; i++)
            {
                x = tileX + i % 7;
                y = tileY + i / 7;

                ITile tile = Main.tile[x, y];
                if (i == 24)
                {
                    if (tile.active()) return false;
                }
                else
                {
                    if (!tile.active()) return false;
                    if (tile.type != TileID.PoopBlock) return false;
                }
            }
            return true;
        }

        bool FindDirtiest()
        {
            for (int x = 0; x < Main.maxTilesX; x++)
            {
                for (int y = 0; y < Main.maxTilesY; y++)
                {
                    ITile tile = Main.tile[x, y];
                    if (!tile.active()) continue;
                    if (tile.type == TileID.DirtiestBlock)
                    {
                        posDirt = new Point(x, y);
                        return true;
                    }
                }
            }
            return false;
        }

        void MoveDirtiest(bool needSuccess)
        {
            int x;
            int y;
            for (int i = 0; i < 49; i++)
            {
                x = posPoop.X + i % 7;
                y = posPoop.Y + i / 7;

                if (i == 24)
                {
                    ITile tile = Main.tile[x, y];
                    tile.type = needSuccess ? TileID.DirtiestBlock : TileID.Dirt;
                    tile.active(true);
                    tile.slope(0);
                    tile.halfBrick(false);
                    NetMessage.SendTileSquare(-1, x, y);
                }
                else
                {
                    ClearTile(x, y);
                }
            }

            utils.Log($"true: {posDirt.X} {posDirt.Y}");
            if (needSuccess)
            {
                ClearTile(posDirt.X, posDirt.Y);
                utils.Log($"{posDirt.X} {posDirt.Y}");
            }
        }

        static void ClearTile(int x, int y)
        {
            Main.tile[x, y].ClearTile();
            NetMessage.SendTileSquare(-1, x, y);
        }
    }
}
