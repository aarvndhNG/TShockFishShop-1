using System;
using Terraria;
using Terraria.GameContent.Events;
using Terraria.ID;
using TShockAPI;


namespace FishShop
{
    public class CmdHelper
    {

        // 调时间
        public static void SwitchTime(TSPlayer player, string type = "noon")
        {
            switch (type)
            {
                case "day":
                    // 0.0 04:30
                    // 13500    08:15
                    TSPlayer.Server.SetTime(true, 13500.0);
                    TSPlayer.All.SendInfoMessage("{0} Set time to morning（8:15）", player.Name);
                    break;

                case "noon":
                    TSPlayer.Server.SetTime(true, 27000.0);
                    TSPlayer.All.SendInfoMessage("{0} Set time to noon（12:00）", player.Name);
                    break;

                case "night":
                    TSPlayer.Server.SetTime(false, 0.0);
                    TSPlayer.All.SendInfoMessage("{0} Set time to night（19:30）", player.Name);
                    break;

                case "midnight":
                    TSPlayer.Server.SetTime(false, 16200.0);
                    TSPlayer.All.SendInfoMessage("{0} Set time to midnight（00:00）", player.Name);
                    break;
            }
        }


        #region 雨、史莱姆雨、沙尘暴、血月、日食、
        // 雨
        public static void ToggleRaining(TSPlayer op, bool on, bool toggleMode = false)
        {
            if (toggleMode) on = !Main.raining;
            if (on)
            {
                if (!Main.raining)
                {
                    Main.StartRain();
                    TSPlayer.All.SendData(PacketTypes.WorldInfo);
                    TSPlayer.All.SendInfoMessage("{0} Let it rain", op.Name);
                }
                else
                    op.SendInfoMessage("It's already raining~");
            }
            else
            {
                if (Main.raining)
                {
                    Main.StopRain();
                    TSPlayer.All.SendData(PacketTypes.WorldInfo);
                    TSPlayer.All.SendInfoMessage("{0} Let the rain stop", op.Name);
                }
                else
                    op.SendInfoMessage("It's not raining");
            }
        }

        // 血月
        public static void ToggleBloodMoon(TSPlayer op, bool on, bool toggleMode = false)
        {
            if (toggleMode) on = !Main.bloodMoon;
            if (on)
            {
                if (!Main.bloodMoon)
                {
                    TSPlayer.Server.SetBloodMoon(on);
                    TSPlayer.All.SendInfoMessage("{0} Summoned the Blood Moon", op.Name);
                }
                else
                    op.SendInfoMessage("It’s already the blood moon period");
            }
            else
            {
                if (Main.bloodMoon)
                {
                    TSPlayer.Server.SetBloodMoon(on);
                    TSPlayer.All.SendInfoMessage("{0} Skip the blood moon", op.Name);
                }
                else
                    op.SendInfoMessage("It’s not a blood moon anymore");
            }
        }

        // 日食
        public static void ToggleEclipse(TSPlayer op, bool on, bool toggleMode = false)
        {
            if (toggleMode) on = !Main.eclipse;
            if (on)
            {
                if (!Main.eclipse)
                {
                    TSPlayer.Server.SetEclipse(on);
                    TSPlayer.All.SendInfoMessage("{0} Summoned a solar eclipse", op.Name);
                }
                else
                    op.SendInfoMessage("It’s already the solar eclipse period");
            }
            else
            {
                if (Main.eclipse)
                {
                    TSPlayer.Server.SetEclipse(on);
                    TSPlayer.All.SendInfoMessage("{0} Skip the eclipse", op.Name);
                }
                else
                    op.SendInfoMessage("It’s no longer a solar eclipse");
            }
        }

        // 沙尘暴
        public static void ToggleSandstorm(TSPlayer op, bool on, bool toggleMode = false)
        {
            if (toggleMode) on = !Sandstorm.Happening;
            if ( on )
            {
                if(!Sandstorm.Happening)
                {
                    Sandstorm.StartSandstorm();
                    TSPlayer.All.SendData(PacketTypes.WorldInfo);
                    TSPlayer.All.SendInfoMessage("{0} Summoned a sandstorm", op.Name);
                }
                else
                    op.SendInfoMessage("There's already a sandstorm blowing~");
            }
            else
            {
                if( Sandstorm.Happening)
                {
                    Sandstorm.StopSandstorm();
                    TSPlayer.All.SendData(PacketTypes.WorldInfo);
                    TSPlayer.All.SendInfoMessage("{0} Stopped the sandstorm", op.Name);
                }
                else
                    op.SendInfoMessage("No sandstorm blowing");
            }
        }
        // 雷雨
        public static void ToggleStorming(TSPlayer op, bool on, bool toggleMode = false)
        {
            if (toggleMode) on = !Main.IsItStorming;
            if (on)
            {
                if (!Main.IsItStorming)
                {
                    Main.rainTime = 3600;
                    Main.ChangeRain();
                    Main.raining = true;
                    Main.cloudAlpha = 1.0f;
                    Main.windSpeedCurrent = Main._maxWind;
                    Main.windSpeedTarget = Main._maxWind;
                    TSPlayer.All.SendData(PacketTypes.WorldInfo);
                    TSPlayer.All.SendInfoMessage("{0} summoned a thunderstorm", op.Name);
                }
                else
                    op.SendInfoMessage("It's already a thunderstorm~");
            }
            else
            {
                if (Main.IsItStorming)
                {
                    Main.StopRain();
                    Main.windSpeedCurrent = 0;
                    Main.windSpeedTarget = 0;
                    TSPlayer.All.SendData(PacketTypes.WorldInfo);
                    TSPlayer.All.SendInfoMessage("{0} The thunderstorm stopped", op.Name);
                }
                else
                    op.SendInfoMessage("It's not a thunderstorm day now");
            }
        }

        // 大风天 
        public static void ToggleWindyDay(TSPlayer op, bool on, bool toggleMode = false)
        {
            if (toggleMode) on = !Main.IsItAHappyWindyDay;
            if (on)
            {
                if (!Main.IsItAHappyWindyDay)
                {
                    Main.windSpeedCurrent = Main._maxWind;
                    Main.windSpeedTarget = Main._maxWind;
                    TSPlayer.All.SendData(PacketTypes.WorldInfo);
                    TSPlayer.All.SendInfoMessage("{0} Summoned the windy day", op.Name);
                }
                else
                    op.SendInfoMessage("It's already a windy day~");
            }
            else
            {
                if (Main.IsItAHappyWindyDay)
                {
                    Main.windSpeedCurrent = 0;
                    Main.windSpeedTarget = 0;
                    TSPlayer.All.SendData(PacketTypes.WorldInfo);
                    TSPlayer.All.SendInfoMessage("{0} Stopped the windy day", op.Name);
                }
                else
                    op.SendInfoMessage("It's not a windy day now");
            }
        }

        // 派对
        public static void ToggleParty(TSPlayer op, bool on, bool toggleMode = false)
        {
            if (toggleMode) on = !BirthdayParty._wasCelebrating;
            if (on)
            {
                if (!BirthdayParty._wasCelebrating)
                {
                    BirthdayParty.GenuineParty = true;
                    //NPC.freeCake = true;
                    //BirthdayParty.PartyDaysOnCooldown = 5;
                    //BirthdayParty._wasCelebrating = on;
                    TSPlayer.All.SendData(PacketTypes.WorldInfo);
                    TSPlayer.All.SendInfoMessage("{0} having a party", op.Name);
                }
                else
                    op.SendInfoMessage("There's already a party~");
            }
            else
            {
                if (BirthdayParty._wasCelebrating)
                {
                    BirthdayParty.GenuineParty = false;
                    //NPC.freeCake = false;
                    TSPlayer.All.SendData(PacketTypes.WorldInfo);
                    TSPlayer.All.SendInfoMessage("{0} Stopped the party", op.Name);
                }
                else
                    op.SendInfoMessage("Not having a party");
            }
        }

        // 史莱姆雨
        public static void ToggleSlimeRain(TSPlayer op, bool on, bool toggleMode=false)
        {
            if( toggleMode ) on = !Main.slimeRain;

            if (on)
            {
                if (!Main.slimeRain)
                {
                    Main.StartSlimeRain(false);
                    TSPlayer.All.SendData(PacketTypes.WorldInfo);
                    TSPlayer.All.SendInfoMessage("{0} Stabbed the slime nest", op.Name);
                }
                else
                {
                    op.SendInfoMessage("It’s already raining slime~");
                }
            }
            else
            {
                if (Main.slimeRain)
                {
                    Main.StopSlimeRain(false);
                    TSPlayer.All.SendData(PacketTypes.WorldInfo);
                    TSPlayer.All.SendInfoMessage("{0} Let the slime army retreat", op.Name);
                }
                else
                {
                    op.SendInfoMessage("It’s not raining slime");
                }
            }
        }

        // 流星雨
        public static void Starfall(TSPlayer op)
        {
            Star.starfallBoost = 4f;
            TSPlayer.All.SendData(PacketTypes.WorldInfo);
            TSPlayer.All.SendInfoMessage("{0} Summoned a meteor shower", op.Name);
        }

        // 陨石
        public static void DropMeteor(TSPlayer op)
        {
            WorldGen.spawnMeteor = false;
            WorldGen.dropMeteor();
            TSPlayer.All.SendData(PacketTypes.WorldInfo);
            TSPlayer.All.SendInfoMessage("{0} Summoned the meteorite", op.Name);
        }

        // 灯笼夜
        public static void LanternsNightStart(TSPlayer op)
        {
            if( !LanternNight.LanternsUp)
            {
                LanternNight.ToggleManualLanterns();
                TSPlayer.All.SendData(PacketTypes.WorldInfo);
            }
            TSPlayer.All.SendInfoMessage("{0} Turn on the lantern night", op.Name);
        }

        // 人间日
        public static void OverworldDay(TSPlayer op)
        {
            // 所有新创建的世界都开始于上午 8:15
            TSPlayer.Server.SetTime(true, 13500);

            Main.moonPhase = 0;
            Main.StopRain();
            Main.StopSlimeRain(false);
            Sandstorm.StopSandstorm();
            Main.eclipse = false;
            Main.invasionSize = 0;
            BirthdayParty.GenuineParty = false;

            TSPlayer.All.SendData(PacketTypes.WorldInfo);
            TSPlayer.All.SendInfoMessage("{0} 将时光倒回人间日", op.Name);
        }
        #endregion


        #region 入侵
        // 跳过入侵
        public static void StopInvasion(TSPlayer player)
        {
            if (Main.invasionSize > 0)
            {
                Main.invasionSize = 0;
                TSPlayer.All.SendInfoMessage("{0} Intrusion event skipped", player.Name);
            }
            else
            {
                player.SendInfoMessage("There are currently no intrusions");
            }
        }
        public static bool NeedBuyStopInvasion(TSPlayer player)
        {
            if(Main.invasionSize == 0)
            {
                player.SendInfoMessage("There are currently no intrusions, no purchase required");
                return false;
            }
            return true;
        }

        // 召唤入侵
        public static void StartInvasion(TSPlayer op, int shopID)
        {
            if (Main.invasionSize <= 0)
            {
                int wave = 1;
                switch (shopID)
                {
                    case ShopItemID.InvasionGoblins:
                        TSPlayer.All.SendInfoMessage("{0} Summoned the Goblin Army", op.Name);
                        StartInvasion(1);
                        break;

                    case ShopItemID.InvasionSnowmen:
                        TSPlayer.All.SendInfoMessage("{0} Summoned the Yeti Legion", op.Name);
                        StartInvasion(2);
                        break;

                    case ShopItemID.InvasionPirates:
                        TSPlayer.All.SendInfoMessage("{0} Summoned Pirate Invasion", op.Name);
                        StartInvasion(3);
                        break;

                    case ShopItemID.InvasionPumpkinmoon:
                        TSPlayer.Server.SetPumpkinMoon(true);
                        Main.bloodMoon = false;
                        NPC.waveKills = 0f;
                        NPC.waveNumber = wave;
                        TSPlayer.All.SendInfoMessage("{0} Summoned Pumpkin Moon", op.Name);
                        break;

                    case ShopItemID.InvasionFrostmoon:
                        TSPlayer.Server.SetFrostMoon(true);
                        Main.bloodMoon = false;
                        NPC.waveKills = 0f;
                        NPC.waveNumber = wave;
                        TSPlayer.All.SendInfoMessage("{0} Summoned Frost Moon", op.Name);
                        break;

                    case ShopItemID.InvasionMartians:
                        TSPlayer.All.SendInfoMessage("{0} Summoned the Martian Riot", op.Name);
                        StartInvasion(4);
                        break;
                }
            }
            else if (DD2Event.Ongoing)
            {
                //DD2Event.StopInvasion();
                //TSPlayer.All.SendInfoMessage("{0} 终止了 撒旦军队", op.Name);
                op.SendInfoMessage("Satan's army is already underway");
            }
            else
            {
                op.SendInfoMessage("There is an invasion in progress");
            }
        }


        private static void StartInvasion(int type)
        {
            int invasionSize = 0;

            if (TShock.Config.Settings.InfiniteInvasion)
            {
                invasionSize = 20000000;
            }
            else
            {
                invasionSize = 100 + (TShock.Config.Settings.InvasionMultiplier * TShock.Utils.GetActivePlayerCount());
            }

            Main.StartInvasion(type);

            Main.invasionSizeStart = invasionSize;
            Main.invasionSize = invasionSize;
        }


        public static bool NeedBuyStartInvasion(TSPlayer op)
        {
            if (Main.invasionSize <= 0)
            {
            }
            else if (DD2Event.Ongoing)
            {
                op.SendInfoMessage("Satan's army is already underway");
                return false;
            }
            else
            {
                op.SendInfoMessage("There is an invasion in progress");
                return false;
            }
            return true;
        }
        #endregion


        // 执行指令
        public static void ExecuteRawCmd(TSPlayer op, string rawCmd)
        {
            // List<string> args = new List<string>() {bossType};
            // if( times>0 )
            // 	args.Add( times.ToString() );
            // SpawnBossRaw( new CommandArgs("", player, args ) );
            if( string.IsNullOrEmpty(rawCmd) )
            {
                op.SendInfoMessage("The command content is empty");
                return;
            }

            op.tempGroup = new SuperAdminGroup();
            TShockAPI.Commands.HandleCommand(op, rawCmd);
            op.tempGroup = null;
        }

        // 集合打团
        public static void TPHereAll(TSPlayer op)
        {
            for (int i = 0; i < Main.maxPlayers; i++)
            {
                if (Main.player[i].active && (Main.player[i] != op.TPlayer))
                {
                    if (TShock.Players[i].Teleport(op.TPlayer.position.X, op.TPlayer.position.Y))
                        TShock.Players[i].SendSuccessMessage(string.Format("{0} Teleport you to him", op.Name));
                }
            }
            TSPlayer.All.SendInfoMessage($"{op.Name} Purchased to gather for a group fight and summon all players to his side");
        }

        // 集体庆祝
        public static void CelebrateAll(TSPlayer op)
        {
            Jump(op);
            TSPlayer.All.SendInfoMessage($"{op.Name} Purchased for collective celebration");
        }

        #region 烟花起飞
        public static void FireworkRocket(TSPlayer player)
        {
            // 火箭
            player.TPlayer.velocity.Y = -50;
            TSPlayer.All.SendData(PacketTypes.PlayerUpdate, "", player.Index);

            // 烟花
            Firework(player);
        }

        public static void Jump(TSPlayer op)
        {
            float x = op.TPlayer.position.X;
            float y = op.TPlayer.position.Y;

            for (int i = 0; i < Main.maxPlayers; i++)
            {
                // if( i==op.Index )
                // 	continue;
                if (!Main.player[i].active)
                    continue;

                Player op2 = Main.player[i];
                float x2 = op2.position.X;
                float y2 = op2.position.Y;
                if (x2 <= x + 970 && x2 >= x - 970 && y2 <= y + 530 && y2 >= y - 530)
                {
                    // 起跳
                    op2.velocity.Y = -8;
                    TSPlayer.All.SendData(PacketTypes.PlayerUpdate, "", i);
                    // 烟花
                    Firework(TShock.Players[i]);
                }
            }
        }

        public static void Firework(TSPlayer op)
        {
            // 烟花
            // 随机烟花
            short[] types = {
                ProjectileID.RocketFireworkRed,
                ProjectileID.RocketFireworkGreen,
                ProjectileID.RocketFireworkBlue,
                ProjectileID.RocketFireworkYellow,
                ProjectileID.RocketFireworksBoxRed,
                ProjectileID.RocketFireworksBoxGreen,
                ProjectileID.RocketFireworksBoxBlue,
                ProjectileID.RocketFireworksBoxYellow,
                ProjectileID.RocketFireworkRed
            };
            Random rnd = new Random();
            int index = rnd.Next(types.Length);
            int type = types[index];
            int p = Projectile.NewProjectile(Projectile.GetNoneSource(), op.TPlayer.position.X, op.TPlayer.position.Y - 64f, 0f, -8f, type, 0, 0);
            //int p = Projectile.NewProjectile(op.TPlayer.position.X, op.TPlayer.position.Y - 64f, 0f, -8f, type, 0, 0);
            Main.projectile[p].Kill();
        }
        #endregion
    }

}
