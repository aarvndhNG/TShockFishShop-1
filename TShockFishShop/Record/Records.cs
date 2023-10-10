using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TShockAPI;

namespace FishShop.Record
{
    /// <summary>
    /// Purchase records
    /// </summary>
    public class Records
    {
        static int lastSave = 0;
        static bool isLoaded;
        static RecordConfig _config;

        public static string RecodFile = "";

        /// <summary>
        /// Load records
        /// </summary>
        public static void Load(bool forceLoad = false)
        {
            if (!isLoaded || forceLoad)
            {
                _config = RecordConfig.Load(RecodFile);
                isLoaded = true;
            }
        }

        /// <summary>
        /// Save records
        /// </summary>
        private static void Save()
        {
            if (!isLoaded) return;

            // Save if more than 2 seconds have passed since the last save
            if (utils.GetUnixTimestamp - lastSave > 2)
            {
                lastSave = utils.GetUnixTimestamp;
                File.WriteAllText(RecodFile, JsonConvert.SerializeObject(_config, Formatting.Indented));
            }
        }

        /// <summary>
        /// Record a purchase
        /// </summary>
        public static void Record(TSPlayer op, ShopItemData shopItemData, int amount, int costMoney, int costFish)
        {
            // Load records
            Load();

            int index = -1;
            for (int i = 0; i < _config.player.Count; i++)
            {
                if (_config.player[i].name == op.Name)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                _config.player[index].costMoney += costMoney;
                _config.player[index].costFish += costFish;
                List<RecordData> datas = _config.player[index].datas;

                index = -1;
                for (int i = 0; i < datas.Count; i++)
                {
                    if (datas[i].id == shopItemData.id)
                    {
                        index = i;
                        break;
                    }
                }

                if (index != -1)
                {
                    datas[index].Record(amount);
                }
                else
                {
                    datas.Add(new RecordData(shopItemData.name, shopItemData.id, amount));
                }
            }
            else
            {
                RecordPlayerData pd = new(op.Name);
                pd.costMoney += costMoney;
                pd.costFish += costFish;
                pd.datas.Add(new RecordData(shopItemData.name, shopItemData.id, amount));
                _config.player.Add(pd);
            }

            // Save records
            Save();
        }

        /// <summary>
        /// Get the number of times a player purchased a specific item
        /// </summary>
        public static int GetPlayerRecord(TSPlayer op, int goodsID)
        {
            Load();
            foreach (RecordPlayerData pd in _config.player)
            {
                if (pd.name == op.Name)
                {
                    foreach (RecordData d in pd.datas)
                    {
                        if (d.id == goodsID)
                            return d.count;
                    }
                    break;
                }
            }
            return -1;
        }

        /// <summary>
        /// Calculate the total sales quantity of a single item
        /// </summary>
        public static int CountShopItemRecord(int goodsID)
        {
            Load();

            int count = -1;
            foreach (RecordPlayerData pd in _config.player)
            {
                foreach (RecordData d in pd.datas)
                {
                    if (d.id == goodsID) count += d.count;
                }
            }
            return count;
        }

        /// <summary>
        /// Reset records
        /// </summary>
        public static void ResetRecord()
        {
            Load();
            _config.player.Clear();
            Save();
        }

        /// <summary>
        /// Display consumption rankings
        /// </summary>
        public static void ShowRank(CommandArgs args)
        {
            Load();
            var lists = _config.player.Where(obj => obj.costMoney > 0).OrderByDescending(obj => obj.costMoney).ToList();
            TSPlayer op = args.Player;

            if (lists.Count == 0)
            {
                op.SendInfoMessage("No consumption rankings available");
                return;
            }

            if (!PaginationTools.TryParsePageNumber(args.Parameters, 1, op, out int pageNumber))
                return;

            List<string> lines = new();
            for (int i = 0; i < lists.Count; i++)
            {
                lines.Add($"#{i + 1}, {lists[i].name}, {utils.GetMoneyDesc(lists[i].costMoney)}");
            }

            PaginationTools.SendPage(op, pageNumber, lines, new PaginationTools.Settings
            {
                HeaderFormat = "[c/96FF0A:'Consumption Rankings' ({0}/{1}):]",
                FooterFormat = $"Enter /fish rank {{0}} to view more".SFormat(Commands.Specifier)
            });
        }

        /// <summary>
        /// Show fish basket rankings
        /// </summary>
        public static void ShowBasket(CommandArgs args)
        {
            Load();
            var lists = _config.player.Where(obj => obj.costFish > 0).OrderByDescending(obj => obj.costFish).ToList();
            TSPlayer op = args.Player;

            if (lists.Count == 0)
            {
                op.SendInfoMessage("No fish basket rankings available");
                return;
            }

            if (!PaginationTools.TryParsePageNumber(args.Parameters, 2, op, out int pageNumber))
                return;

            List<string> lines = new();
            for (int i = 0; i < lists.Count; i++)
            {
                lines.Add($"#{i + 1}, {lists[i].name}, {lists[i].costFish} fish");
            }

            PaginationTools.SendPage(op, pageNumber, lines, new PaginationTools.Settings
            {
                HeaderFormat = "[c/96FF0A:'Fish Basket Rankings' ({0}/{1}):]",
                FooterFormat = $"Enter /fish basket {{0}} to view more".SFormat(Commands.Specifier)
            });
        }
    }
}
