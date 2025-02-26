using System;
using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.ID;
using System.Linq;
using TShockAPI;


namespace FishShop
{
    public class FishHelper
    {
        public static void AnglerQuestSwap(TSPlayer player)
        {
            Main.AnglerQuestSwap();
            int itemID = Main.anglerQuestItemNetIDs[ Main.anglerQuest ];
            string itemName = utils.GetItemDesc(itemID);
            TSPlayer.All.SendSuccessMessage($"{player.Name} purchased the task fish. Today’s task fish has been randomly changed to {itemName}.");
            if (!player.RealPlayer) player.SendInfoMessage($"Today’s task fish is replaced by {itemName}");
        }

        // 指定今天的任务鱼（物品id或物品名）
        public static void FishQuestSwap(TSPlayer player, string itemNameOrId)
		{
			if (Main.netMode == 1)
			{
				return;
			}

            int itemID = 0;
            string itemName = "";
            if( int.TryParse( itemNameOrId, out itemID) ){
                // 鱼的物品id
                if( !Main.anglerQuestItemNetIDs.Contains( itemID ) ){
                    itemName = utils.GetItemDesc(itemID);
                    // Lang.GetItemNameValue(4444);
                    player.SendErrorMessage($"{itemID} = {itemName}，Not an effective task fish！");
                    return;
                }
            } else {

                // 鱼的名字
                List<Item> found = TShock.Utils.GetItemByName(itemNameOrId);
                if( found.Count==0 )
                {

                    // 追加判断，加入一些容易叫错的物品名
                    itemID = ShopItemID.GetIDByName(itemNameOrId);
                    if( itemID==0 )
                    {
                        player.SendErrorMessage($"{itemNameOrId} Not a valid task fish！");
                        return;
                    }


                } else {
                    // 有可能搜到2个或更多的物品，这里简单处理，取搜索到的一个
                    itemID = found[0].netID;
                }
            }

            if ( !Main.anglerQuestItemNetIDs.Contains(itemID)  )
            {
                player.SendErrorMessage($"{itemNameOrId} Not a valid task fish！");
                return;
            }

            Main.anglerQuest = Main.anglerQuestItemNetIDs.ToList().IndexOf( itemID );
			Main.anglerWhoFinishedToday.Clear();
			Main.anglerQuestFinished = false;
			NetMessage.SendAnglerQuest(-1);

			// NetMessage.SendData(76, player.Index, -1, NetworkText.Empty, player.Index);
			// NetMessage.SendData(76, -1, -1, NetworkText.Empty, player.Index);

            itemName = utils.GetItemDesc(itemID);
            player.SendSuccessMessage($"Today’s mission fish has been designated as {itemName}");
		}

        public static void FishInfo(TSPlayer player)
        {
            if( NPCHelper.CheckNPCActive("369") )
            {
                int itemID = Main.anglerQuestItemNetIDs[ Main.anglerQuest ];
                string questText = Language.GetTextValue("AnglerQuestText.Quest_" + ItemID.Search.GetName(itemID));
                string[] splits = questText.Split("\n\n".ToCharArray());
                if( splits.Count()>1 ){
                    questText = splits[splits.Count()-1];
                    questText = questText.Replace("（Capture location：", "");
                    questText = questText.Replace("）", "");
                }
                string itemName = utils.GetItemDesc(itemID);
                player.SendInfoMessage($"mission fish: {itemName}（{questText}）");
            } else {
                player.SendInfoMessage($"Mission fish: The fisherman is not present");
            }

            player.SendInfoMessage($"Moon phases: {utils.MoonPhases[Main.moonPhase]}");

            // 时间
            double time = Main.time / 3600.0;
            time += 4.5;
            if (!Main.dayTime)
                time += 15.0;
            time = time % 24.0;
            player.SendInfoMessage("Time: {0}:{1:D2}", (int)Math.Floor(time), (int)Math.Floor((time % 1.0) * 60.0));

            if( player.RealPlayer )
                player.SendInfoMessage($"Fisherman tasks completed: {player.TPlayer.anglerQuestsFinished} ");

            // 在线玩家任务完成情况
            player.SendInfoMessage(ShowAnglerFinishedDetail());
        }

        // 在线玩家任务完成情况
        private static string ShowAnglerFinishedDetail()
		{
            List<string> players = new List<string>();
			foreach (TSPlayer ply in TShock.Players)
			{
				if (ply!=null && ply.Active)
                    players.Add(ply.Name);
			}

            int finished = 0;
            string swarpStr;
            for (int i = 0; i < players.Count; i++)
            {
                if( i!=0 && i%9==0 ){
                    swarpStr = "\n";
                } else {
                    swarpStr = "";
                }
                if( Main.anglerWhoFinishedToday.Contains( players[i] ) )
                {
                    finished ++;
                    players[i] = $"{swarpStr}[c/96FF96:✔{players[i]}]";
                } else {
                    players[i] = $"{swarpStr}-{players[i]}";
                }
            }

            return $"Today’s completion of the fisherman’s mission({finished}/{players.Count}):\n {string.Join(", ", players)}";
		}
        
        public static bool NeedBuyChangeMoonPhase(TSPlayer player, int id, int amount = 1)
        {
            int index = ShopItemID.MoonphaseStart - id;
            if (index == 8)
                index = (Main.moonPhase + amount) % 8;
            if( index==Main.moonPhase )
            {
                player.SendInfoMessage("The moon phases to be switched are the same, no purchase required");
                return false;
            }
            return true;
        }

        public static void ChangeMoonPhaseByID(TSPlayer player, int id, int amount=1)
        {
            int index = ShopItemID.MoonphaseStart-id;
            if( index==8 )
                index = (Main.moonPhase+amount)%8;
            ChangeMoonPhase(player, index);
        }

        private static void ChangeMoonPhase(TSPlayer player, int index)
        {
            if( index<0 || index>7 )
                return;
            string name = utils.MoonPhases[index];
            Main.dayTime = false;
            Main.moonPhase = index;
            Main.time = 0.0;
            TSPlayer.All.SendData(PacketTypes.WorldInfo);
            player.SendSuccessMessage("The moon phase has been changed to {0}", name);
            TSPlayer.All.SendInfoMessage("{0} Switched {1}",player.Name, name);
        }
    }

}
