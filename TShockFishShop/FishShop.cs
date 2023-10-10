using FishShop.Record;
using System;
using System.IO;
using System.Reflection;
using Terraria;
using Terraria.Localization;
using TerrariaApi.Server;
using TShockAPI;

namespace FishShop
{
    [ApiVersion(2, 1)]
    public partial class Plugin : TerrariaPlugin
    {
        public override string Name => "FishShop";
        public override string Author => "hufang360";
        public override string Description => "Fish Shop";
        public override Version Version => Assembly.GetExecutingAssembly().GetName().Version;

        public static readonly string savedir = Path.Combine(TShock.SavePath, "FishShop");
        public static readonly string settingsFile = Path.Combine(savedir, "settings.json");
        public static readonly string configFile = Path.Combine(savedir, "config.json");
        public static readonly string recordFile = Path.Combine(savedir, "record.json");

        // Configuration
        public static Config _config;

        private static bool isLoaded = false;

        public Plugin(Main game) : base(game)
        {
        }

        public override void Initialize()
        {
            Commands.ChatCommands.Add(new Command(Manage, "fishshop", "fish", "fs", "鱼店") { HelpText = "Fish Shop" });

            if (!Directory.Exists(savedir))
            {
                Directory.CreateDirectory(savedir);
            }
            Settings.Load(settingsFile);
            Config.GenConfig(configFile);
            Records.RecodFile = recordFile;
            utils.Init();
        }

        private void Manage(CommandArgs args)
        {
            TSPlayer op = args.Player;

            #region help
            void ShowHelpText()
            {
                op.SendInfoMessage("/fish list - View the shelf");
                op.SendInfoMessage("/fish ask <number> - Inquire about prices");
                op.SendInfoMessage("/fish buy <number> - Purchase");
                op.SendInfoMessage("/fish info - Display fishing information");
                op.SendInfoMessage("/fish rank - Consumption ranking");
                op.SendInfoMessage("/fish basket - Fish basket ranking");

                if (op.HasPermission(Permissions.Finish)) op.SendInfoMessage("/fish finish <times> - Modify your fisherman quest completion count");
                if (op.HasPermission(Permissions.Change)) op.SendInfoMessage("/fish change - Change today's fishing task");
                if (op.HasPermission(Permissions.ChangeSuper)) op.SendInfoMessage("/fish changesuper <item id | item name> - Specify today's fishing task fish");

                if (op.HasPermission(Permissions.Reload))
                {
                    op.SendInfoMessage("/fish reload - Reload the configuration");
                    op.SendInfoMessage("/fish reset - Reset purchase limit");
                }

                if (op.HasPermission(Permissions.Special))
                {
                    op.SendInfoMessage("/fish special - View special commands");
                }
            }
            #endregion

            if (TShock.ServerSideCharacterConfig.Settings.Enabled && op.Group.Name == TShock.Config.Settings.DefaultGuestGroupName)
            {
                op.SendErrorMessage("Guests cannot access the Fish Shop.");
                return;
            }

            if (args.Parameters.Count == 0)
            {
                op.SendErrorMessage("Syntax error, use /fish help for usage.");
                return;
            }

            switch (args.Parameters[0].ToLowerInvariant())
            {
                default:
                    ListGoods(args);
                    op.SendInfoMessage("Please use /fish help to check usage.");
                    break;

                case "h":
                case "help":
                case "帮助":
                    ShowHelpText();
                    return;

                case "l":
                case "list":
                case "货架":
                case "逛店":
                    ListGoods(args);
                    break;

                case "a":
                case "ask":
                case "询价":
                    AskGoods(args);
                    break;

                case "b":
                case "buy":
                case "购买":
                case "买":
                    BuyGoods(args);
                    break;

                case "i":
                case "info":
                case "信息":
                    FishHelper.FishInfo(op);
                    break;

                case "rank":
                case "消费榜":
                    Records.ShowRank(args);
                    break;

                case "basket":
                case "鱼篓榜":
                    Records.ShowBasket(args);
                    break;

                case "forge":
                    ForgeHelper.Manage(args);
                    break;

                #region admin command
                case "f":
                case "finish":
                    if (!op.RealPlayer)
                    {
                        op.SendErrorMessage("This command needs to be executed in-game!");
                        break;
                    }
                    if (!op.HasPermission(Permissions.Finish))
                    {
                        op.SendErrorMessage("You don't have permission to change fishing completion count!");
                        break;
                    }
                    if (args.Parameters.Count < 2)
                    {
                        op.SendErrorMessage("You need to input the completion count, e.g., /fish finish 10");
                        break;
                    }
                    if (int.TryParse(args.Parameters[1], out int finished))
                    {
                        op.TPlayer.anglerQuestsFinished = finished;
                        NetMessage.SendData(76, op.Index, -1, NetworkText.Empty, op.Index);
                        NetMessage.SendData(76, -1, -1, NetworkText.Empty, op.Index);
                        op.SendSuccessMessage($"Your angler quest completion count has been changed to {finished} times");
                    }
                    else
                    {
                        op.SendErrorMessage("Invalid count input, e.g., /fish finish 10");
                    }
                    break;

                case "change":
                case "swap":
                case "next":
                case "pass":
                    if (!op.HasPermission(Permissions.Change))
                        op.SendErrorMessage("You don't have permission to change fishing tasks!");
                    else
                        FishHelper.AnglerQuestSwap(op);
                    break;

                case "cs":
                case "changesuper":
                    if (!op.HasPermission(Permissions.ChangeSuper))
                    {
                        op.SendErrorMessage("You don't have permission to specify today's fishing task!");
                    }
                    else
                    {
                        if (args.Parameters.Count < 2)
                        {
                            op.SendErrorMessage("You need to input the task fish's name/item id, e.g., /fish cs Guide Voodoo Fish");
                            break;
                        }
                        FishHelper.FishQuestSwap(op, args.Parameters[1]);
                    }
                    break;

                case "reload":
                case "r":
                    if (!op.HasPermission(Permissions.Reload))
                    {
                        op.SendErrorMessage("You don't have permission to perform reload operation!");
                    }
                    else
                    {
                        double t1 = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
                        Settings.Load(settingsFile);
                        LoadConfig(true);
                        args.Player.SendSuccessMessage("[fishshop] Fish Shop configuration has been reloaded");
                        double t2 = DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
                        op.SendInfoMessage($"Time elapsed: {t2 - t1} milliseconds");
                    }
                    break;

                case "reset":
                    if (!op.HasPermission(Permissions.Reload))
                    {
                        op.SendErrorMessage("You don't have permission to perform this operation!");
                    }
                    else
                    {
                        Records.ResetRecord();
                        args.Player.SendSuccessMessage("[fishshop] Purchase limit data has been reset");
                    }
                    break;
                #endregion

                #region special
                case "docs":
                    if (!op.HasPermission(Permissions.Special))
                    {
                        op.SendErrorMessage("You don't have permission to execute this command!");
                    }
                    else
                    {
                        DocsHelper.GenDocs(op, savedir);
                    }
                    break;

                case "special":
                case "spe":
                    if (!op.HasPermission(Permissions.Special))
                    {
                        op.SendErrorMessage("You don't have permission to execute this command!");
                    }
                    else
                    {
                        op.SendInfoMessage("/fish docs - Generate reference documentation");
                        op.SendInfoMessage("/fish relive - Revive NPC");
                        op.SendInfoMessage("/fish tpall - Gather");
                        op.SendInfoMessage("/fish jump - Group jump");
                        op.SendInfoMessage("/fish firework - Fireworks");
                    }
                    break;

                case "jump":
                    if (!op.HasPermission(Permissions.Special))
                        op.SendErrorMessage("You don't have permission to execute this command!");
                    else
                        CmdHelper.Jump(op);
                    break;

                case "firework":
                case "fw":
                    if (!op.HasPermission(Permissions.Special))
                        op.SendErrorMessage("You don't have permission to execute this command!");
                    else
                        CmdHelper.FireworkRocket(op);
                    break;

                case "relive":
                    if (!op.HasPermission(Permissions.Special))
                        op.SendErrorMessage("You don't have permission to execute this command!");
                    else
                        NPCHelper.ReliveNPC(op);
                    break;

                case "tpall":
                    if (!op.HasPermission(Permissions.Special))
                        op.SendErrorMessage("You don't have permission to execute this command!");
                    else
                        CmdHelper.TPHereAll(op);
                    break;
                #endregion
            }
        }

        /// <summary>
        /// Check if the shop is ready
        /// </summary>
        static bool ShopIsReady(TSPlayer op)
        {
            // Update the shelf
            LoadConfig();

            string msg = "";
            foreach (ItemData d in _config.unlock)
            {
                if (!UnlockID.CheckUnlock(d, op, out string s))
                {
                    msg += " " + s;
                }
            }
            if (msg != "")
            {
                if (op != null)
                {
                    op.SendInfoMessage($"【{_config.name}】 is closed because: {msg}");
                }
                else
                {
                    utils.Log($"【{_config.name}】 is closed because: {msg}");
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Load the configuration
        /// </summary>
        /// <param name="forceLoad">Force loading</param>
        static void LoadConfig(bool forceLoad = false)
        {
            if (!isLoaded || forceLoad)
            {
                _config = Config.Load(configFile);

                foreach (ItemData d in _config.unlock)
                {
                    d.FixIDByName(true);
                }

                var i = 0;
                foreach (ShopItemData siData in _config.shop)
                {
                    siData.serial = i + 1;
                    i++;
                    siData.Filling();
                    foreach (ItemData d in siData.unlock)
                    {
                        d.FixIDByName(true);
                    }
                    foreach (ItemData d in siData.cost)
                    {
                        d.FixIDByName(false);
                    }
                }
                isLoaded = true;
            }
            Records.Load(forceLoad);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
            base.Dispose(disposing);
        }
    }
}
