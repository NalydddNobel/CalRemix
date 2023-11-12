﻿using CalamityMod;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Furniture.CraftingStations;
using CalamityMod.Items.Placeables.FurnitureAbyss;
using CalamityMod.Items.Potions;
using CalamityMod.NPCs.Yharon;
using CalRemix.Items.Materials;
using CalRemix.Projectiles.Weapons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Localization.IME;
using ReLogic.OS;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.UI;

namespace CalRemix.UI
{
    public class Anomaly109UI : UIState
    {
        public static int CurrentPage = 0;
        public static int ClickCooldown = 0;
        public static string TextInput = "";
        private static string Underscore = "_";
        private static int UnderscoreTimer = 0;
        public static bool TryUnlock = false;
        private static int HeldRightTimer = 0;

        public enum InputType
        {
            text,
            integer,
            number
        }
        public static InputType inputType;
        public override void Update(GameTime gameTime)
        {
            bool shouldShow = !Main.gameMenu;


            base.Update(gameTime);
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            // This prevents drawing unless we are using an ExampleCustomResourceWeapon
            if (Main.gameMenu)
                return;
            if (!(Main.LocalPlayer.TryGetModPlayer<CalRemixPlayer>(out CalRemixPlayer p) && p.anomaly109UI))
            {
                return;
            }
            if (Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                Main.blockInput = false;
                Main.LocalPlayer.GetModPlayer<CalRemixPlayer>().anomaly109UI = false;
                return;
            }

            base.Draw(spriteBatch);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            float bgWidth = Main.screenWidth * 0.6f;
            float bgHeight = Main.screenHeight * 0.7f;
            Anomaly109Option selectedOption = new Anomaly109Option("aa", "aaa", "aaaaa", () => { }, false);
            Rectangle selectedRectangle = new Rectangle();

            DrawBackground(spriteBatch, bgWidth, bgHeight, out Rectangle borderframe, out Rectangle mainframe);

            BlockClicks(borderframe);
            if (CurrentPage <= Anomaly109Manager.options.Count() / 12)
            {
                DrawOptions(spriteBatch, mainframe, out selectedRectangle, out selectedOption, bgWidth, bgHeight);
                DrawPrompt(spriteBatch, mainframe);
                SelectOption(spriteBatch, selectedOption, selectedRectangle);
            }
            else
                DrawFanny(spriteBatch, mainframe);
            DrawArrows(spriteBatch, mainframe);
        }

        private static void DrawBackground(SpriteBatch spriteBatch, float bgWidth, float bgHeight, out Rectangle borderframe, out Rectangle mainframe)
        {
            int borderwidth = 4;
            mainframe = new Rectangle((int)(Main.screenWidth - bgWidth) / 2, (int)(Main.screenHeight - bgHeight) / 2, (int)bgWidth, (int)bgHeight);
            borderframe = new Rectangle((int)(Main.screenWidth - bgWidth) / 2 - borderwidth, (int)(Main.screenHeight - bgHeight) / 2 - borderwidth, (int)bgWidth + borderwidth * 2, (int)bgHeight + borderwidth * 2);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, borderframe, Color.Lime);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, mainframe, Color.Black);
        }

        private static void DrawPrompt(SpriteBatch spriteBatch, Rectangle mainframe)
        {
            int borderwidth = 4;
            Rectangle promptframe = new Rectangle(mainframe.X + (int)(mainframe.Width * 0.04f), mainframe.Y + (int)(mainframe.Height * 0.1f), (int)(mainframe.Width * 0.93f), (int)(mainframe.Height * 0.1f));
            Rectangle promptborderframe = new Rectangle(mainframe.X + (int)(mainframe.Width * 0.04f) - borderwidth, mainframe.Y + (int)(mainframe.Height * 0.1f) - borderwidth, (int)(mainframe.Width * 0.93f) + borderwidth * 2, (int)(mainframe.Height * 0.1f) + borderwidth * 2);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, promptborderframe, Color.Lime);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, promptframe, Color.Black);

            UnderscoreTimer++;
            if (UnderscoreTimer > 30)
            {
                if (Underscore == "_")
                {
                    Underscore = " ";
                }
                else if (Underscore == " ")
                {
                    Underscore = "_";
                }
                UnderscoreTimer = 0;
            }

            ProcessInput();
            for (int i = 0; i < Anomaly109Manager.options.Count; i++)
            {
                if (Anomaly109Manager.options[i].key == TextInput && !Anomaly109Manager.options[i].unlocked)
                {
                    SoundEngine.PlaySound(SoundID.Item7, Main.LocalPlayer.Center);
                    Anomaly109Manager.options[i].unlocked = true;
                    break;
                }
            }
            Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, ">C:\\remix\\" + TextInput + Underscore, promptframe.X + promptframe.Width / 64, promptframe.Y + promptframe.Height / 3f, Color.Lime * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero, (float)Main.screenWidth / (float)1745);
        }

        private static void ProcessInput()
        {
            PlayerInput.WritingText = true;
            Main.instance.HandleIME();

            string newText = Main.GetInputText(TextInput);
            if (TextInput.Length >= 64 && !Main.keyState.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Back))
                return;

            // input here more or less referenced from dragonlens
            if (inputType == InputType.integer && Regex.IsMatch(newText, "[0-9]*$"))
            {
                if (newText != TextInput)
                {
                    TextInput = newText;
                }
            }
            else if (inputType == InputType.number && Regex.IsMatch(newText, "(?<=^| )[0-9]+(.[0-9]+)?(?=$| )|(?<=^| ).[0-9]+(?=$| )"))
            {
                if (newText != TextInput)
                {
                    TextInput = newText;
                }
            }
            else
            {
                if (newText != TextInput)
                {
                    TextInput = newText;
                }
            }
        }

        private static void DrawOptions(SpriteBatch spriteBatch, Rectangle mainframe, out Rectangle optionRect, out Anomaly109Option selected, float bgWidth, float bgHeight)
        {
            Rectangle maus = new Rectangle((int)(Main.MouseWorld.X - Main.screenPosition.X), (int)(Main.MouseWorld.Y - Main.screenPosition.Y), 10, 10);
            int individualLength = (int)(bgWidth / 2 * 0.6f);
            int individualHeight = (int)(bgHeight / 4 * 0.4f);
            int spacingX = (int)(bgWidth / 3.15f);
            int spacingY = (int)(bgHeight / 7);
            int row = 1;
            int column = 1;
            selected = new Anomaly109Option("aa", "aaa", "aaaa", () => { }, false);
            optionRect = new Rectangle();
            for (int i = CurrentPage * 12; i < Anomaly109Manager.options.Count(); i++)
            {
                if (i >= CurrentPage * 12 + CurrentPage * 12 && i > 11)
                {
                    break;
                }
                column++;
                if (i % 3 == 0)
                {
                    column = 0;
                    row++;
                }
                Rectangle barframe = new Rectangle((int)(mainframe.X + mainframe.Width / 28 + 2) + spacingX * column, (int)(mainframe.Y + mainframe.Height / 48 + 2) + spacingY * row, individualLength, individualHeight);
                Rectangle barbg = new Rectangle((int)(mainframe.X + mainframe.Width / 28) + spacingX * column, (int)(mainframe.Y + mainframe.Height / 48) + spacingY * row, individualLength + 4, individualHeight + 4);

                Color outlineColor = Anomaly109Manager.options[i].unlocked ? Color.Lime : Color.Red;
                Color pathColor = Anomaly109Manager.options[i].unlocked ? Color.Lime : Color.Gray;
                Color nameColor = Anomaly109Manager.options[i].unlocked ? new(83, 83, 249) : Color.DarkGray;
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, barbg, outlineColor);
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, barframe, Color.Black);
                string address = "C:\\remix\\";

                Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, address, barbg.X + barbg.Width / 32, barbg.Y + barbg.Height / 4, pathColor * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero, (float)Main.screenWidth / (float)1745);
                Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, Anomaly109Manager.options[i].title, barbg.X + barbg.Width / 32 + (float)FontAssets.MouseText.Value.MeasureString(address).X * (float)Main.screenWidth / (float)1745, barbg.Y + barbg.Height / 4, nameColor * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero, (float)Main.screenWidth / (float)1745);

                if (maus.Intersects(barbg))
                {
                    selected = Anomaly109Manager.options[i];
                    optionRect = barbg;
                }
            }
        }

        private static void SelectOption(SpriteBatch spriteBatch, Anomaly109Option option, Rectangle optionRect)
        {
            Rectangle maus = new Rectangle((int)(Main.MouseWorld.X - Main.screenPosition.X), (int)(Main.MouseWorld.Y - Main.screenPosition.Y), 10, 10);
            if (option.key != "aa")
            {
                if (maus.Intersects(optionRect))
                {
                    string status = "Unlocked";
                    Color statusColor = Color.Lime;
                    string statusLiteral = "Status: ";
                    if (!option.unlocked)
                    {
                        status = "Locked";
                        statusColor = Color.Red;
                    }
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, option.message, (int)(Main.MouseWorld.X - Main.screenPosition.X) + 20, (int)(Main.MouseWorld.Y - Main.screenPosition.Y) + 20, Color.White * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero);
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, statusLiteral, (int)(Main.MouseWorld.X - Main.screenPosition.X) + 20, (int)(Main.MouseWorld.Y - Main.screenPosition.Y) + 52, Color.White * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero);
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, status, (int)(Main.MouseWorld.X - Main.screenPosition.X) + 20 + (float)FontAssets.MouseText.Value.MeasureString(statusLiteral).X, (int)(Main.MouseWorld.Y - Main.screenPosition.Y) + 52, statusColor * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero);
                    if (Main.mouseLeft && ClickCooldown <= 0 && option.unlocked)
                    {
                        option.toggle();
                        ClickCooldown = 8;
                    }
                }
            }
        }

        private static void DrawArrows(SpriteBatch spriteBatch, Rectangle mainframe)
        {
            Rectangle maus = new Rectangle((int)(Main.MouseWorld.X - Main.screenPosition.X), (int)(Main.MouseWorld.Y - Main.screenPosition.Y), 10, 10);
            int maxPages = Anomaly109Manager.options.Count() / 12;
            Rectangle arrowframer = new Rectangle(mainframe.Right - (int)(mainframe.Width * 0.0775f), mainframe.Bottom - (int)(mainframe.Height * 0.125f), (int)(mainframe.Width * 0.05f), (int)(mainframe.Width * 0.05f));
            Rectangle arrowframel = new Rectangle(mainframe.Left + (int)(mainframe.Width * 0.036f), mainframe.Bottom - (int)(mainframe.Height * 0.125f), (int)(mainframe.Width * 0.05f), (int)(mainframe.Width * 0.05f));
            if (CurrentPage < maxPages)
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, arrowframer, Color.Lime);
            if (CurrentPage > 0)
                spriteBatch.Draw(TextureAssets.MagicPixel.Value, arrowframel, Color.Lime);
            if (maus.Intersects(arrowframer))
            {
                if (CurrentPage < maxPages)
                {
                    if (Main.mouseLeft && ClickCooldown <= 0)
                    {
                        Main.LocalPlayer.releaseUseItem = true;
                        CurrentPage++;
                        ClickCooldown = 8;
                    }
                }
                else if (CurrentPage == maxPages)
                {
                    if (Main.mouseLeft)
                    {
                        Main.LocalPlayer.releaseUseItem = true;
                        HeldRightTimer++;
                        if (HeldRightTimer > 300)
                        {
                            CurrentPage++;
                            HeldRightTimer = 0;
                        }
                    }
                }
            }
            else
            if (maus.Intersects(arrowframel))
            {
                if (CurrentPage > 0)
                {
                    if (Main.mouseLeft && CurrentPage > 0 && ClickCooldown <= 0)
                    {
                        Main.LocalPlayer.releaseUseItem = true;
                        CurrentPage--;
                        ClickCooldown = 8;
                    }
                }
            }
            if (ClickCooldown > 0)
            {
                ClickCooldown--;
            }
        }

        private static void DrawFanny(SpriteBatch spriteBatch, Rectangle mainframe)
        {
            Rectangle maus = new Rectangle((int)(Main.MouseWorld.X - Main.screenPosition.X), (int)(Main.MouseWorld.Y - Main.screenPosition.Y), 10, 10);

            Texture2D cage = ModContent.Request<Texture2D>("CalamityMod/Projectiles/Magic/IceBlock").Value;
            Texture2D fanny = ModContent.Request<Texture2D>("CalRemix/UI/Fanny/FannyIdle").Value;

            Rectangle cageframe = new Rectangle(mainframe.X + (int)(mainframe.Width * 0.375f), mainframe.Y + (int)(mainframe.Height * 0.2f), (int)(mainframe.Width * 0.25f), (int)(mainframe.Height * 0.4f));
            Rectangle bgframe = new Rectangle(mainframe.X + (int)(mainframe.Width * 0.375f), mainframe.Y + (int)(mainframe.Height * 0.65f), (int)(mainframe.Width * 0.25f), (int)(mainframe.Height * 0.1f));
            Rectangle borderframe = new Rectangle(mainframe.X + (int)(mainframe.Width * 0.375f) - 2, mainframe.Y + (int)(mainframe.Height * 0.65f) - 2, (int)(mainframe.Width * 0.25f) + 4, (int)(mainframe.Height * 0.1f) + 4);
            Rectangle fannyframe = new Rectangle(mainframe.X + (int)(mainframe.Width * 0.375f), mainframe.Y + (int)(mainframe.Height * 0.22f), (int)(mainframe.Width * 0.25f), (int)(mainframe.Height * 0.35f));
            Rectangle fannytheFrame = fanny.Frame(1, 8, 0, 0);

            string FannyStatus = FannyManager.fannyEnabled ? "Fanny is currently free" : "Fanny is currently sealed";
            Color FannyColor = FannyManager.fannyEnabled ? Color.Lime : Color.Red;

            spriteBatch.Draw(TextureAssets.MagicPixel.Value, borderframe, FannyColor);
            spriteBatch.Draw(TextureAssets.MagicPixel.Value, bgframe, Color.Black);
            if (!FannyManager.fannyEnabled)
            {
                spriteBatch.Draw(fanny, fannyframe, fannytheFrame, Color.White);
            }
            spriteBatch.Draw(cage, cageframe, Color.White * 0.6f);
            Utils.DrawBorderStringFourWay(Main.spriteBatch, FontAssets.MouseText.Value, FannyStatus, bgframe.X + (int)(bgframe.Height * 0.3f), bgframe.Y + (int)(bgframe.Height * 0.3f), FannyColor * (Main.mouseTextColor / 255f), Color.Black, Vector2.Zero, (float)Main.screenWidth / (float)1745);
            if (maus.Intersects(bgframe))
            {
                if (Main.mouseLeft && ClickCooldown <= 0)
                {
                    FannyManager.fannyEnabled = !FannyManager.fannyEnabled;
                    CalRemixWorld.UpdateWorldBool();
                    ClickCooldown = 7;
                }
            }
        }

        private static void BlockClicks(Rectangle borderframe)
        {
            Rectangle maus = new Rectangle((int)(Main.MouseWorld.X - Main.screenPosition.X), (int)(Main.MouseWorld.Y - Main.screenPosition.Y), 10, 10);
            if (maus.Intersects(borderframe))
            {
                Main.blockMouse = true;
            }
        }
    }

    public class Anomaly109Manager : ModSystem
    {
        public static List<Anomaly109Option> options = new List<Anomaly109Option> { };
        public override void OnWorldLoad()
        {
            if (options.Count == 0)
            {
                options.Add(new Anomaly109Option("749257290", "alloy_bars", "Toggles Alloy Bars from recipes", () => { CalRemixWorld.alloyBars = !CalRemixWorld.alloyBars; }, false));
                options.Add(new Anomaly109Option("154382034", "essential_essence_bars", "Toggles Essential Essence Bars from recipes", () => { CalRemixWorld.essenceBars = !CalRemixWorld.essenceBars; }, false));
                options.Add(new Anomaly109Option("034683710", "yharim_bars", "Toggles Yharim Bars from recipes", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("963282883", "shimmer_essences", "Toggles Shimmer Essences from recipes", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("346018689", "cosmilite_slag", "Toggles tiershifted Cosmilite", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("132059332", "rear_gars", "Toggles Rear Gars and Uelibloom Ore removal", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("123508223", "side_gars", "Toggles Side Gars and Galactica Singularity recipe removal",  () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("123608122", "front_gars", "Toggles Front Gars and Reaper Tooth drops", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("873025872", "meld_gunk", "Toggles Meld Gunk", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("023582378", "crocodile_scales", "Toggles Crocodile Scales from recipes", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("018539273", "permanent_upgrades", "Toggles permanent upgrade recipes and alt obtainment methods", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("639202857", "starbuster_core", "Toggles the Starbuster Core's strange obtainment method", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("631208414", "plagued_jungle", "Toggles generation of the Plagued Jungle and related requirements", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("034683710", "hardmode_shrines", "Toggles generation for hardmode shrines", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("034683710", "life_ore", "Toggles generation for Life Ore", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("034683710", "resprites", "Toggles resprites for bosses and items", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("034683710", "boss_dialogue", "Toggles boss dialogue", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("034683710", "grimesand", "Toggles generation of Grimesand and its requirement for evil 2 bosses", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("034683710", "banana_clown", "...", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("034683710", "primal_aspid", "...", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("034683710", "clamitas", "...", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("034683710", "coyote_venom", "...", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("034683710", "parched_scales", "...", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("034683710", "fearmonger_retier", "...", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("034683710", "idkrn", "...", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("034683710", "idkrn2", "...", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("034683710", "idkrn3", "...", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
                options.Add(new Anomaly109Option("034683710", "la_ruga", "...", () => { CalRemixWorld.yharimBars = !CalRemixWorld.yharimBars; }, false));
            }
        }
        public override void SaveWorldData(TagCompound tag)
        {
            for (int i = 0; i < options.Count; i++)
            {
                Anomaly109Option msg = options[i];
                if (msg.unlocked)
                    tag["Anomaly109" + msg.title] = true;
            }
        }

        public override void LoadWorldData(TagCompound tag)
        {
            for (int i = 0; i < options.Count; i++)
            {
                Anomaly109Option msg = options[i];
                options[i].unlocked = tag.ContainsKey("Anomaly109" + msg.title);
            }
        }
    }

    public class Anomaly109Option
    {
        public string title { get; set; }
        public string message { get; set; }
        public string key { get; set; }

        public Action toggle { get; set; }

        public bool unlocked {  get; set; }

        public Anomaly109Option(string key, string title, string message, Action toggle, bool unlocked)
        {
            this.key = key;
            this.title = title;
            this.message = message;
            this.toggle = toggle;
            this.unlocked = unlocked;
        }


    }

    [Autoload(Side = ModSide.Client)]
    internal class Anomaly109System : ModSystem
    {
        private UserInterface Anomaly109UserInterface;

        internal Anomaly109UI Anoui;

        public static LocalizedText ExampleResourceText { get; private set; }

        public override void Load()
        {
            Anoui = new();
            Anomaly109UserInterface = new();
            Anomaly109UserInterface.SetState(Anoui);

            string category = "UI";
            ExampleResourceText ??= Mod.GetLocalization($"{category}.ExampleResource");
        }

        public override void UpdateUI(GameTime gameTime)
        {
            Anomaly109UserInterface?.Update(gameTime);
        }

        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
        {
            int resourceBarIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Resource Bars"));
            if (resourceBarIndex != -1)
            {
                layers.Insert(resourceBarIndex, new LegacyGameInterfaceLayer(
                    "ExampleMod: Example Resource Bar",
                    delegate {
                        Anomaly109UserInterface.Draw(Main.spriteBatch, new GameTime());
                        return true;
                    },
                    InterfaceScaleType.UI)
                );
            }
        }
    }
}