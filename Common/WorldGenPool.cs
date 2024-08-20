using System;
using System.Linq;
using MetroidMod.Content.Tiles.ItemTile.Beam.Hunters;
using MetroidMod.Content.Tiles.ItemTile.Beam;
using MetroidMod.Content.Tiles.ItemTile.Missile;
using MetroidMod.Content.Tiles.ItemTile;
using Terraria.ModLoader;
using Terraria.WorldBuilding;
using Terraria;
using MetroidMod.Common.Systems;
using Terraria.Utilities;
using MetroidMod.Content.Items.Addons;
using MetroidMod.Content.Items.Accessories;
using MetroidMod.Content.Items.Addons.Hunters;
using MetroidMod.Content.Items.Addons.V2;
using MetroidMod.Content.Items.Addons.V3;
using MetroidMod.Content.Items.MissileAddons.BeamCombos;
using MetroidMod.Content.Items.MissileAddons;
using static MetroidMod.Sounds;
using System.Reflection;
using Terraria.ID;
using MetroidMod.Content.SuitAddons;

namespace MetroidMod.Common
{
	internal class WorldGenPool
	{
		public static int GetRandomChozoOrbItem()
		{
			WorldGenPool pool = new();
			MetroidBossDown bossesDown = MSystem.bossesDown;

			pool.AddItem<ChargeBeamAddon>(8);
			pool.AddItem<HiJumpBoots>(8);
			pool.AddItem<WaveBeamAddon>(8);
			pool.AddItem<HomingMissileAddon>(4);
			pool.AddItem<SpaceJumpBoots>(4);

			if(Condition(NPC.downedQueenBee))
			{
				pool.AddItem<SpazerAddon>(8);
			}

			if (Condition(NPC.downedBoss3))
			{
				pool.AddItem<IceBeamAddon>(8);
				pool.AddItem<IceMissileAddon>(4);
				pool.AddItem<SpazerComboAddon>(4);
			}

			if (Condition(Main.hardMode))
			{
				pool.AddItem<SuperMissileAddon>(4);
				pool.AddItem<IceSpreaderAddon>(4);
				pool.AddItem<SeekerMissileAddon>(4);
				pool.AddItem<WavebusterAddon>(4);
			}

			if (Condition(NPC.downedMechBossAny))
			{
				pool.AddItem<DiffusionMissileAddon>(4);
				pool.AddItem<SpaceJump>(4);
			}

			if (Condition(NPC.downedMechBoss1))
			{
				pool.AddItem<WaveBeamV2Addon>(4);
				pool.AddItem<FlamethrowerAddon>(4);
				pool.AddItem<PlasmaMachinegunAddon>(4);
			}

			if (Condition(NPC.downedMechBoss2))
			{
				pool.AddItem<ChargeBeamV2Addon>(4);
			}

			if (Condition(NPC.downedMechBoss3))
			{
				pool.AddItem<WideBeamAddon>(4);
			}

			if (Condition(NPC.downedPlantBoss))
			{
				pool.AddItem<NovaComboAddon>(4);
				pool.AddItem<NovaBeamAddon>(4);
			}

			if (Condition(NPC.downedChristmasIceQueen))
			{
				pool.AddItem<IceBeamV2Addon>(4);
				pool.AddItem<IceSuperMissileAddon>(4);
			}

			if (Condition(NPC.downedMoonlord))
			{
				pool.AddItem<StardustComboAddon>(4);
				pool.AddItem<StardustMissileAddon>(4);
				pool.AddItem<SolarComboAddon>(4);
				pool.AddItem<NebulaComboAddon>(4);
				pool.AddItem<NebulaMissileAddon>(4);
				pool.AddItem<SolarBeamAddon>(4);
				pool.AddItem<StardustBeamAddon>(4);
				pool.AddItem<VortexBeamAddon>(4);
				pool.AddItem<LuminiteBeamAddon>(4);
				pool.AddItem<NebulaBeamAddon>(4);
				pool.AddItem<OmegaCannonAddon>(4);
				pool.AddItem<PhazonBeamAddon>(4);
			}

			// TODO currently doesn't take into account the Blaze Beam rework
			if (Condition(bossesDown.HasFlag(MetroidBossDown.downedKraid)))
			{
				pool.AddItem<PlasmaBeamGreenAddon>(4);
				pool.AddItem<PlasmaBeamRedAddon>(4);
			}

			foreach (ModSuitAddon addon in SuitAddonLoader.addons)
			{
				if (addon.CanGenerateOnChozoStatue())
				{
					pool.Add(addon.ItemType, addon.GenerationChance());
				}
			}

			foreach (ModMBAddon addon in MBAddonLoader.addons)
			{
				if (addon.CanGenerateOnChozoStatue())
				{
					pool.Add(addon.ItemType, addon.GenerationChance());
				}
			}

			return pool.Get();
		}

		public static int GetRandomExpansionTile()
		{
			WorldGenPool pool = new();

			pool.Add(SuitAddonLoader.GetAddon<EnergyTank>().TileType, 14);
			pool.AddTile<MissileExpansionTile>(50);
			pool.AddTile<ImperialistTile>(1);
			pool.AddTile<JudicatorTile>(1);
			pool.AddTile<MagMaulTile>(1);
			pool.AddTile<BattleHammerTile>(1);
			pool.AddTile<VoltDriverTile>(1);
			pool.AddTile<ShockCoilTile>(1);
			pool.AddTile<UAExpansionTile>(12);

			return pool.Get();
		}


		private readonly WeightedRandom<int> weightedRandom = new();

		private void AddItem<T>(double weight) where T : ModItem
		{
			int itemType = ModContent.ItemType<T>();
			weightedRandom.Add(itemType, weight);
		}

		private void AddTile<T>(double weight) where T : ModTile
		{
			int itemType = ModContent.TileType<T>();
			weightedRandom.Add(itemType, weight);
		}

		private void Add(int type, double weight)
		{
			weightedRandom.Add(type, weight);
		}

		private int Get()
		{
			return weightedRandom.Get();
		}

		private static bool Condition(bool flag)
		{
			bool allItemsEnabled = Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues;
			return flag || allItemsEnabled;
		}
	}
}
