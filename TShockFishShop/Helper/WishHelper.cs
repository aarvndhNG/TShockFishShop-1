using System.Collections.Generic;
using Terraria;
using TShockAPI;

namespace FishShop
{
    /// <summary>
    /// Wishing Well
    /// </summary>
    public class WishHelper
    {

        public static void Manage(CommandArgs args)
        {
            TSPlayer op = args.Player;
            void ShowHelpText()
            {
                op.SendInfoMessage("/wish list, view the shelves");
            }

            if (TShock.ServerSideCharacterConfig.Settings.Enabled && op.Group.Name == TShock.Config.Settings.DefaultGuestGroupName)
            {
                op.SendErrorMessage("Guests cannot use the Wishing Well");
                return;
            }

            if (args.Parameters.Count == 0)
            {
                op.SendErrorMessage("Syntax error, type /wish help to see usage");
                return;
            }

            string kw = args.Parameters[0].ToLowerInvariant();
            switch (kw)
            {
                // Help
                case "h":
                case "help":
                    ShowHelpText();
                    return;
            }

            List<Item> items = TShock.Utils.GetItemByIdOrName(args.Parameters[0]);
            if (items.Count > 1)
            {
                args.Player.SendInfoMessage("Multiple items matched");
                string text = "";
                for (int i = 0; i < items.Count; i++)
                {
                    text = text + Lang.GetItemNameValue(items[i].type) + ",";
                    if ((i + 1) % 5 == 0)
                    {
                        text += "\n";
                    }
                }
                text = text.Trim(',');
                args.Player.SendInfoMessage(text);
                return;
            }
            if (items.Count < 1)
            {
                args.Player.SendErrorMessage("This item does not exist");
                return;
            }
            utils.Log($"{items[0].Name} prefix:{items[0].prefix} stack:{items[0].stack}");
        }
    }
}
