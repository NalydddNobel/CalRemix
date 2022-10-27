﻿using CalamityMod.CalPlayer;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Potions.Alcohol;
using CalamityMod.Tiles.Furniture.CraftingStations;
using CalamityMod.Items.Accessories;
using CalamityMod.Projectiles.Typeless;
using CalamityMod.Projectiles.Summon;
using CalamityMod.Rarities;
using CalamityMod.Items;
using CalamityMod;
using Terraria.DataStructures;
using CalRemix.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using static Terraria.ModLoader.ModContent;
using Terraria.ModLoader;

namespace CalRemix.Items.Accessories
{
    public class ZenithArcanum : ModItem
    {
        public override string Texture => "CalamityMod/Items/Accessories/AstralArcanum";
        public override void SetStaticDefaults()
        {
            SacrificeTotal = 1;
            DisplayName.SetDefault("Zenith Arcanum");
            Tooltip.SetDefault("'Top of the food chain'\n"+
            "Summons various spirits of the world to protect you\n" +
            "20 % increase to summon damage and defense\n" +
            "+ 4 life regeneration, 15 % increased pick speed, and + 8 max minions\n" +
            "Increased minion knockback\n" +
            "Minions inflict a variety of debuffs and spawn skeletal limbs on enemy hits");
        }

        public override void SetDefaults()
        {
            Item.width = 42;
            Item.height = 36;
            Item.value = CalamityGlobalItem.Rarity15BuyPrice;
            Item.rare = RarityType<Violet>();
            Item.accessory = true;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            if (player.GetModPlayer<CalRemixPlayer>().arcanumHands)
                return false;

            return true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetDamage<SummonDamageClass>() += 0.2f;
            player.statDefense = (int)(1.2 * player.statDefense);
            player.pickSpeed = (int)(1.15 * player.pickSpeed);
            player.maxMinions += 8;
            player.lifeRegen += 4;

            CalamityPlayer caPlayer = player.Calamity();
            caPlayer.shadowMinions = true; 
            caPlayer.holyMinions = true; 
            caPlayer.voltaicJelly = true;
            caPlayer.starTaintedGenerator = true;
            caPlayer.nucleogenesis = true;

            caPlayer.howlTrio = true;
            caPlayer.howlsHeart = true;
            caPlayer.brimstoneWaifu = true;
            caPlayer.sirenWaifu = true;
            caPlayer.sandWaifu = true;
            caPlayer.sandBoobWaifu = true;
            caPlayer.cloudWaifu = true;
            caPlayer.youngDuke = true;
            caPlayer.miniOldDuke = true;
            caPlayer.allWaifus = true;
            caPlayer.elementalHeart = true;
            caPlayer.virili = true;

            CalRemixPlayer modPlayer = player.GetModPlayer<CalRemixPlayer>();
            modPlayer.arcanumHands = true; 
            int brimmy = ProjectileType<BrimstoneElementalMinion>();
            int siren = ProjectileType<WaterElementalMinion>();
            int healer = ProjectileType<SandElementalHealer>();
            int sandy = ProjectileType<SandElementalMinion>();
            int cloudy = ProjectileType<CloudElementalMinion>();
            int thomas = ProjectileType<PlaguePrincess>();
            int yd = ProjectileType<YoungDuke>();
            caPlayer.gladiatorSword = true;

            var source = player.GetSource_Accessory(Item);
            Vector2 velocity = new Vector2(0f, -1f);
            int elementalDmg = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(290);
            float kBack = 2f + player.GetKnockback<SummonDamageClass>().Additive;

            if (player.whoAmI == Main.myPlayer)
            {
                int baseDamage = 290;
                int swordDmg = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(baseDamage);
                if (player.ownedProjectileCounts[brimmy] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, brimmy, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 20;
                }
                if (player.ownedProjectileCounts[siren] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, siren, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 290;
                }
                if (player.ownedProjectileCounts[healer] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, healer, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 290;
                }
                if (player.ownedProjectileCounts[sandy] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, sandy, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 290;
                }
                if (player.ownedProjectileCounts[cloudy] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, cloudy, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 290;
                }
                if (player.ownedProjectileCounts[thomas] < 1)
                {
                    int p = Projectile.NewProjectile(source, player.Center, velocity, thomas, elementalDmg, kBack, player.whoAmI);
                    if (Main.projectile.IndexInRange(p))
                        Main.projectile[p].originalDamage = 290;
                }
                if (player.ownedProjectileCounts[ModContent.ProjectileType<GladiatorSword>()] < 1)
                {
                    var sword = Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, ModContent.ProjectileType<GladiatorSword>(), swordDmg, 2f, Main.myPlayer);
                    sword.originalDamage = baseDamage;

                    sword = Projectile.NewProjectileDirect(source, player.Center, Vector2.Zero, ModContent.ProjectileType<GladiatorSword2>(), swordDmg, 2f, Main.myPlayer);
                    sword.originalDamage = baseDamage;
                }
                if (player.ownedProjectileCounts[ProjectileType<HowlsHeartHowl>()] < 1)
                {
                    int damage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(290);
                    Projectile howl = Projectile.NewProjectileDirect(source, player.Center, -Vector2.UnitY, ProjectileType<HowlsHeartHowl>(), damage, 1f, player.whoAmI, 0f, 1f);
                    howl.originalDamage = damage;
                }
                if (player.ownedProjectileCounts[ProjectileType<HowlsHeartCalcifer>()] < 1)
                {
                    Projectile.NewProjectile(source, player.Center, -Vector2.UnitY, ProjectileType<HowlsHeartCalcifer>(), 0, 0f, player.whoAmI, 0f, 0f);
                }
                if (player.ownedProjectileCounts[ProjectileType<HowlsHeartTurnipHead>()] < 1)
                {
                    Projectile.NewProjectile(source, player.Center, -Vector2.UnitY, ProjectileType<HowlsHeartTurnipHead>(), 0, 0f, player.whoAmI, 0f, 0f);
                }
                if (player.ownedProjectileCounts[ProjectileType<YoungDuke>()] < 1)
                {
                    int damage = (int)player.GetTotalDamage<SummonDamageClass>().ApplyTo(290);
                    Projectile dud = Projectile.NewProjectileDirect(source, player.Center, -Vector2.UnitY, ProjectileType<YoungDuke>(), damage, 1f, player.whoAmI, 0f, 1f);
                    dud.originalDamage = damage;
                }
            }
        }
        public override void AddRecipes()
        {
            CreateRecipe().
                AddIngredient<ElementalQuiver>(1).
                AddIngredient<QuiverofNihility>(1).
                AddIngredient<DaawnlightSpiritOrigin>(1).
                AddIngredient<RedWine>(5).
                AddIngredient<AshesofAnnihilation>(5).
                AddTile<CosmicAnvil>().
                Register();
        }
    }
}
