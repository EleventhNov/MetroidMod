﻿using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

using MetroidModPorted.Common.GlobalItems;
using MetroidModPorted.Common.Players;
using MetroidModPorted.Content.DamageClasses;
using MetroidModPorted.Content.Projectiles;
//using MetroidMod.Projectiles.chargelead;

namespace MetroidModPorted.Content.Items.Weapons
{
	public class MissileLauncher : ModItem
	{
		// Failsaves.
		private Item[] _missileMods;
		public Item[] MissileMods
		{
			get
			{
				if (_missileMods == null)
				{
					_missileMods = new Item[MetroidModPorted.missileSlotAmount];
					for (int i = 0; i < _missileMods.Length; ++i)
					{
						_missileMods[i] = new Item();
						_missileMods[i].TurnToAir();
					}
				}

				return _missileMods;
			}
			set { _missileMods = value; }
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Missile Launcher");
			Tooltip.SetDefault("Select this item in your hotbar and open your inventory to open the Missile Addon UI");

			SacrificeTotal = 1;
		}
		//public override void SetDefaults()
		public override void SetDefaults()
		{
			Item.damage = 30;
			Item.DamageType = ModContent.GetInstance<HunterDamageClass>();
			Item.width = 24;
			Item.height = 16;
			Item.scale = 0.8f;
			Item.useTime = 9;
			Item.useAnimation = 9;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.noMelee = true;
			Item.knockBack = 5.5f;
			Item.value = 20000;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundLoader.GetLegacySoundSlot(Mod, "Assets/Sounds/MissileSound");
			Item.autoReuse = false;
			//Item.shoot = ModContent.ProjectileType<MissileShot>();
			Item.shootSpeed = 8f;
			Item.crit = 10;

			MGlobalItem mi = Item.GetGlobalItem<MGlobalItem>();
			mi.statMissiles = 5;
			mi.maxMissiles = 5;
		}
		public override void AddRecipes()
		{
			CreateRecipe(1)
				.AddIngredient<Miscellaneous.ChoziteBar>(10)
				.AddIngredient<Tiles.EnergyTank>(1)
				.AddTile(TileID.Anvils)
				.Register();
		}

		public override void UseStyle(Player player, Rectangle heldItemFrame)
		{
			player.itemLocation.X = player.MountedCenter.X - (float)Item.width * 0.5f;
			player.itemLocation.Y = player.MountedCenter.Y - (float)Item.height * 0.5f;
		}

		public override bool CanUseItem(Player player)
		{
			MGlobalItem mi = Item.GetGlobalItem<MGlobalItem>();
			if (player.whoAmI == Main.myPlayer && Item.type == Main.mouseItem.type)
			{
				return false;
			}
			return player.whoAmI == Main.myPlayer && mi.statMissiles > 0;
		}

		int finalDmg = 0;

		int useTime = 9;

		string shot = "MissileShot";
		string chargeShot = "DiffusionMissileShot";
		string shotSound = "MissileSound";
		string chargeShotSound = "SuperMissileSound";
		string chargeUpSound = "ChargeStartup_Power";
		string chargeTex = "ChargeLead_PlasmaRed";
		int dustType = 6;
		Color dustColor = default(Color);
		Color lightColor = MetroidModPorted.plaRedColor;

		float comboKnockBack = 5.5f;

		bool isCharge = false;
		bool isSeeker = false;
		int isHeldCombo = 0;
		int chargeCost = 5;
		int comboSound = 0;
		float comboDrain = 5f;
		bool noSomersault = false;
		bool useFlameSounds = false;
		bool useVortexSounds = false;

		bool isShotgun = false;
		int shotgunAmt = 5;

		bool isMiniGun = false;
		int miniRateIncr = 2;
		int miniGunCostReduct = 2;
		int miniGunAmt = 1;

		int comboUseTime = 4;
		int comboCostUseTime = 12;
		int comboShotAmt = 1;
		float chargeMult = 1f;

		float leadAimSpeed = 0f;

		string altTexture => this.Texture + "_alt";
		string texture = "";

		public override void UpdateInventory(Player P)
		{
			MGlobalItem mi = Item.GetGlobalItem<MGlobalItem>();
			MPlayer mp = P.GetModPlayer<MPlayer>();

			/*int ic = mod.ItemType("IceMissileAddon");
			int sm = mod.ItemType("SuperMissileAddon");
			int icSm = mod.ItemType("IceSuperMissileAddon");
			int st = mod.ItemType("StardustMissileAddon");
			int ne = mod.ItemType("NebulaMissileAddon");

			int se = mod.ItemType("SeekerMissileAddon");*/

			Item slot1 = MissileMods[0];
			Item slot2 = MissileMods[1];
			Item exp = MissileMods[2];

			int damage = 30;
			useTime = 9;
			shot = "MissileShot";
			chargeShot = "";
			shotSound = "MissileSound";
			chargeShotSound = "SuperMissileSound";
			chargeUpSound = "";
			chargeTex = "";
			dustType = 0;
			dustColor = default(Color);
			lightColor = Color.White;

			texture = "";

			comboKnockBack = Item.knockBack;

			//isSeeker = (slot1.type == se);
			//isCharge = (!slot1.IsAir && !isSeeker);
			isHeldCombo = 0;
			chargeCost = 5;
			comboSound = 0;
			comboDrain = 5f;
			noSomersault = false;
			useFlameSounds = false;
			useVortexSounds = false;

			isShotgun = false;
			shotgunAmt = 5;

			isMiniGun = false;
			miniRateIncr = 2;
			miniGunCostReduct = 2;
			miniGunAmt = 1;

			comboUseTime = 4;
			comboCostUseTime = 12;
			comboShotAmt = 1;

			leadAimSpeed = 0f;

			mi.maxMissiles = 5 + (5 * exp.stack);
			if (mi.statMissiles > mi.maxMissiles)
			{
				mi.statMissiles = mi.maxMissiles;
			}

			// Default Combos

			/*if (slot2.type == sm)
			{
				shot = "SuperMissileShot";
			}
			else if (slot2.type == ic)
			{
				shot = "IceMissileShot";
			}
			else if (slot2.type == icSm)
			{
				shot = "IceSuperMissileShot";
			}
			else if (slot2.type == st)
			{
				shot = "StardustMissileShot";
			}
			else if (slot2.type == ne)
			{
				shot = "NebulaMissileShot";
			}*/

			/*int wb = mod.ItemType("WavebusterAddon");
			int icSp = mod.ItemType("IceSpreaderAddon");
			int sp = mod.ItemType("SpazerComboAddon");
			int ft = mod.ItemType("FlamethrowerAddon");
			int pl = mod.ItemType("PlasmaMachinegunAddon");
			int nv = mod.ItemType("NovaComboAddon");

			int di = mod.ItemType("DiffusionMissileAddon");

			// Charge Combos
			if (slot1.type == wb)
			{
				isHeldCombo = 1;
				comboSound = 1;
				noSomersault = true;
				chargeShot = "WavebusterShot";
				chargeUpSound = "ChargeStartup_Wave";
				chargeTex = "ChargeLead_WaveV2";
				dustType = 62;
				lightColor = MetroidMod.waveColor2;
				comboKnockBack = 0f;
				texture = "Wavebuster_Item";
			}
			if (slot1.type == icSp)
			{
				chargeShot = "IceSpreaderShot";
				chargeShotSound = "IceSpreaderSound";
				chargeUpSound = "ChargeStartup_Ice";
				chargeTex = "ChargeLead_Ice";
				dustType = 59;
				lightColor = MetroidMod.iceColor;
				texture = "IceSpreader_Item";
			}
			if (slot1.type == sp)
			{
				isShotgun = true;
				chargeShot = shot;
				chargeUpSound = "ChargeStartup_Power";
				chargeTex = "ChargeLead_Spazer";
				dustType = 64;
				lightColor = MetroidMod.powColor;
				texture = "SpazerCombo_Item";
			}
			if (slot1.type == ft)
			{
				isHeldCombo = 2;
				comboSound = 1;
				noSomersault = true;
				useFlameSounds = true;
				chargeShot = "FlamethrowerShot";
				chargeUpSound = "ChargeStartup_PlasmaRed";
				chargeTex = "ChargeLead_PlasmaRed";
				dustType = 6;
				lightColor = MetroidMod.plaRedColor;
				texture = "Flamethrower_Item";
			}
			if (slot1.type == pl)
			{
				isHeldCombo = 2;
				comboSound = 2;
				noSomersault = true;
				isMiniGun = true;
				chargeShot = "PlasmaMachinegunShot";
				chargeShotSound = "PlasmaMachinegunSound";
				chargeUpSound = "ChargeStartup_Power";
				chargeTex = "ChargeLead_PlasmaGreen";
				dustType = 61;
				lightColor = MetroidMod.plaGreenColor;
				texture = "PlasmaMachinegun_Item";
			}
			if (slot1.type == nv)
			{
				isHeldCombo = 1;
				comboSound = 1;
				noSomersault = true;
				leadAimSpeed = 0.85f;
				chargeShot = "NovaLaserShot";
				chargeUpSound = "ChargeStartup_Nova";
				chargeTex = "ChargeLead_Nova";
				dustType = 75;
				lightColor = MetroidMod.novColor;
				texture = "NovaLaser_Item";
			}

			if (slot1.type == di)
			{
				chargeShot = "DiffusionMissileShot";
				chargeUpSound = "ChargeStartup_Power";
				chargeTex = "ChargeLead_PlasmaRed";
				dustType = 6;
				lightColor = MetroidMod.plaRedColor;
				texture = "DiffusionMissile_Item";

				if (slot2.type == ic || slot2.type == icSm)
				{
					chargeShot = "IceDiffusionMissileShot";
					chargeUpSound = "ChargeStartup_Ice";
					chargeTex = "ChargeLead_Ice";
					dustType = 135;
					lightColor = MetroidMod.iceColor;
				}
				if (slot2.type == st)
				{
					chargeShot = "StardustDiffusionMissileShot";
					chargeUpSound = "ChargeStartup_Ice";
					chargeTex = "ChargeLead_Stardust";
					dustType = 87;
					lightColor = MetroidMod.iceColor;
				}
				if (slot2.type == ne)
				{
					chargeShot = "NebulaDiffusionMissileShot";
					chargeUpSound = "ChargeStartup_Wave";
					chargeTex = "ChargeLead_Nebula";
					dustType = 255;
					lightColor = MetroidMod.waveColor;
				}
			}
			if (isSeeker)
			{
				texture = "SeekerMissile_Item";
			}

			int sd = mod.ItemType("StardustComboAddon");
			int nb = mod.ItemType("NebulaComboAddon");
			int vt = mod.ItemType("VortexComboAddon");
			int sl = mod.ItemType("SolarComboAddon");

			if (slot1.type == sd)
			{
				chargeShot = "StardustComboShot";
				chargeShotSound = "IceSpreaderSound";
				chargeUpSound = "ChargeStartup_Ice";
				chargeTex = "ChargeLead_Stardust";
				dustType = 87;
				lightColor = MetroidMod.iceColor;
				texture = "StardustCombo_Item";
			}
			if (slot1.type == nb)
			{
				isHeldCombo = 1;
				comboSound = 1;
				noSomersault = true;
				chargeShot = "NebulaComboShot";
				chargeUpSound = "ChargeStartup_Wave";
				chargeTex = "ChargeLead_Nebula";
				dustType = 255;
				lightColor = MetroidMod.waveColor;
				texture = "NebulaCombo_Item";
			}
			if (slot1.type == vt)
			{
				isHeldCombo = 2;
				comboSound = 2;
				noSomersault = true;

				comboUseTime = 10;
				comboShotAmt = 3;

				useVortexSounds = true;

				chargeShot = "VortexComboShot";
				chargeShotSound = "PlasmaMachinegunSound";
				chargeUpSound = "ChargeStartup_Power";
				chargeTex = "ChargeLead_Vortex";
				dustType = 229;
				lightColor = MetroidMod.lumColor;
				texture = "VortexCombo_Item";
			}
			if (slot1.type == sl)
			{
				isHeldCombo = 1;
				comboSound = 1;
				noSomersault = true;
				leadAimSpeed = 0.9f;
				chargeShot = "SolarLaserShot";
				chargeUpSound = "ChargeStartup_PlasmaRed";
				chargeTex = "ChargeLead_SolarCombo";
				dustType = 6;
				lightColor = MetroidMod.plaRedColor;
				texture = "SolarCombo_Item";
			}*/

			/*if (!slot1.IsAir)
			{
				MGlobalItem mItem = slot1.GetGlobalItem<MGlobalItem>();
				chargeMult = mItem.addonChargeDmg;
				chargeCost = mItem.addonMissileCost;
				comboDrain = mItem.addonMissileDrain;
			}*/
			comboCostUseTime = (int)Math.Round(60.0 / (double)comboDrain);

			float addonDmg = 0f;
			float addonSpeed = 0f;
			/*if (!slot2.IsAir)
			{
				MGlobalItem mItem = slot2.GetGlobalItem<MGlobalItem>();
				addonDmg = mItem.addonDmg;
				addonSpeed = mItem.addonSpeed;
			}*/
			finalDmg = (int)Math.Round((double)((float)damage * (1f + addonDmg)));

			float shotsPerSecond = (60f / useTime) * (1f + addonSpeed);
			useTime = (int)Math.Max(Math.Round(60.0 / (double)shotsPerSecond), 2);

			Item.damage = finalDmg;
			Item.useTime = useTime;
			Item.useAnimation = useTime;
			//Item.shoot = ModContent.Find<ModProjectile>(Mod.Name, shot).Type;
			Item.UseSound = null;//mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/"+shotSound);

			Item.shootSpeed = 8f;
			Item.reuseDelay = 0;
			Item.mana = 0;
			Item.knockBack = 5.5f;
			Item.scale = 0.8f;
			Item.crit = 10;
			Item.value = 20000;

			Item.rare = ItemRarityID.Green;

			Item.Prefix(Item.prefix);
		}
		public override bool PreDrawInWorld(SpriteBatch sb, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			MGlobalItem mi = Item.GetGlobalItem<MGlobalItem>();
			Texture2D tex = Terraria.GameContent.TextureAssets.Item[Type].Value;//Main.itemTexture[Item.type];
			setTexture(mi);
			if (mi.itemTexture != null)
			{
				tex = mi.itemTexture;
			}
			float num5 = (float)(Item.height - tex.Height);
			float num6 = (float)(Item.width / 2 - tex.Width / 2);
			sb.Draw(tex, new Vector2(Item.position.X - Main.screenPosition.X + (float)(tex.Width / 2) + num6, Item.position.Y - Main.screenPosition.Y + (float)(tex.Height / 2) + num5 + 2f),
			new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), alphaColor, rotation, new Vector2((float)(tex.Width / 2), (float)(tex.Height / 2)), scale, SpriteEffects.None, 0f);
			return false;
		}
		public override bool PreDrawInInventory(SpriteBatch sb, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
		{
			MGlobalItem mi = Item.GetGlobalItem<MGlobalItem>();
			Texture2D tex = Terraria.GameContent.TextureAssets.Item[Type].Value;//Main.itemTexture[Item.type];
			setTexture(mi);
			if (mi.itemTexture != null)
			{
				tex = mi.itemTexture;
			}
			sb.Draw(tex, position, new Rectangle?(new Rectangle(0, 0, tex.Width, tex.Height)), drawColor, 0f, origin, scale, SpriteEffects.None, 0f);
			return false;
		}
		void setTexture(MGlobalItem mi)
		{
			if (texture != "")
			{
				string alt = "";
				if (MetroidModPorted.UseAltWeaponTextures)
				{
					alt = "_alt";
				}
				mi.itemTexture = ModContent.Request<Texture2D>(texture + alt).Value;// + "/" + texture).Value;
			}
			else
			{
				if (MetroidModPorted.UseAltWeaponTextures)
				{
					mi.itemTexture = ModContent.Request<Texture2D>(altTexture).Value;
				}
				else
				{
					mi.itemTexture = Terraria.GameContent.TextureAssets.Item[Type].Value;//Main.itemTexture[Item.type];
				}
			}

			if (mi.itemTexture != null)
			{
				Item.width = mi.itemTexture.Width;
				Item.height = mi.itemTexture.Height;
			}
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips);

			Player P = Main.player[Main.myPlayer];
			MPlayer mp = P.GetModPlayer<MPlayer>();

			if (Item == Main.HoverItem)
			{
				Item.ModItem.UpdateInventory(Main.player[Main.myPlayer]);
			}

			int cost = (int)((float)chargeCost * (mp.missileCost + 0.001f));
			string ch = "Charge shot consumes " + cost + " missiles";
			if (isHeldCombo > 0)
			{
				ch = "Charge initially costs " + cost + " missiles";
			}
			TooltipLine mCost = new(Mod, "ChargeMissileCost", ch);

			float drain = (float)Math.Round(comboDrain * mp.missileCost, 2);
			TooltipLine mDrain = new(Mod, "ChargeMissileDrain", "Drains " + drain + " missiles per second");

			for (int k = 0; k < tooltips.Count; k++)
			{
				if (tooltips[k].Name == "Knockback" && !MissileMods[0].IsAir && !isSeeker)
				{
					tooltips.Insert(k + 1, mCost);
					if (isHeldCombo > 0)
					{
						tooltips.Insert(k + 2, mDrain);
					}
				}

				if (tooltips[k].Name == "PrefixDamage")
				{
					double num19 = (double)((float)Item.damage - (float)finalDmg);
					num19 = num19 / (double)((float)finalDmg) * 100.0;
					num19 = Math.Round(num19);
					if (num19 > 0.0)
					{
						tooltips[k].Text = "+" + num19 + Lang.tip[39].Value;
					}
					else
					{
						tooltips[k].Text = num19 + Lang.tip[39].Value;
					}
				}
				if (tooltips[k].Name == "PrefixSpeed")
				{
					double num20 = (double)((float)Item.useAnimation - (float)useTime);
					num20 = num20 / (double)((float)useTime) * 100.0;
					num20 = Math.Round(num20);
					num20 *= -1.0;
					if (num20 > 0.0)
					{
						tooltips[k].Text = "+" + num20 + Lang.tip[40].Value;
					}
					else
					{
						tooltips[k].Text = num20 + Lang.tip[40].Value;
					}
				}
			}
		}

		public override ModItem Clone(Item item)
		{
			MissileLauncher clone = (MissileLauncher)MemberwiseClone();//this.NewInstance(item);
			//MissileLauncher missileClone = (MissileLauncher)clone;
			clone.MissileMods = new Item[MetroidModPorted.missileSlotAmount];
			for (int i = 0; i < MetroidModPorted.missileSlotAmount; ++i)
			{
				clone.MissileMods[i] = this.MissileMods[i];
			}

			return clone;
		}

		public override void SaveData(TagCompound tag)
		{
			for (int i = 0; i < MissileMods.Length; ++i)
				tag.Add("missileItem" + i, ItemIO.Save(MissileMods[i]));

			MGlobalItem mi = Item.GetGlobalItem<MGlobalItem>();
			tag.Add("statMissiles", mi.statMissiles);
			tag.Add("maxMissiles", mi.maxMissiles);
		}
		public override void LoadData(TagCompound tag)
		{
			try
			{
				MissileMods = new Item[MetroidModPorted.missileSlotAmount];
				for (int i = 0; i < MissileMods.Length; i++)
				{
					Item item = tag.Get<Item>("missileItem" + i);
					MissileMods[i] = item;
				}

				MGlobalItem mi = Item.GetGlobalItem<MGlobalItem>();
				mi.statMissiles = tag.GetInt("statMissiles");
				mi.maxMissiles = tag.GetInt("maxMissiles");
			}
			catch { }
		}

		public override void NetSend(BinaryWriter writer)
		{
			for (int i = 0; i < MissileMods.Length; ++i)
			{
				ItemIO.Send(MissileMods[i], writer);
			}
			writer.Write(chargeLead);
		}
		public override void NetRecieve(BinaryReader reader)
		{
			for (int i = 0; i < MissileMods.Length; ++i)
			{
				MissileMods[i] = ItemIO.Receive(reader);
			}
			chargeLead = reader.ReadInt32();
		}
	}
}
