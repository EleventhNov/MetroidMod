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

namespace MetroidMod.Common
{
	internal static class ChozoStatueDropPool
	{
		public static int OrbItem()
		{
			MetroidBossDown bossesDown = MSystem.bossesDown;

			int item = ModContent.TileType<MorphBallTile>();
			WeightedChance[] list = new WeightedChance[SuitAddonLoader.AddonCount + 5 + MBAddonLoader.AddonCount + 35];
			int index = 0;
			foreach (ModSuitAddon addon in SuitAddonLoader.addons)
			{
				if (addon.CanGenerateOnChozoStatue()) { list[index++] = new WeightedChance(() => { item = addon.TileType; }, addon.GenerationChance()); }
			}
			foreach (ModMBAddon addon in MBAddonLoader.addons)
			{
				if (addon.CanGenerateOnChozoStatue()) { list[index++] = new WeightedChance(() => { item = addon.TileType; }, addon.GenerationChance()); }
			}
			list[index++] = new WeightedChance(() => { item = ModContent.TileType<ChargeBeamTile>(); }, 8);
			list[index++] = new WeightedChance(() => { item = ModContent.TileType<HiJumpBootsTile>(); }, 8);
			list[index++] = new WeightedChance(() => { item = ModContent.TileType<WaveBeamTile>(); }, 8);
			list[index++] = new WeightedChance(() => { item = ModContent.TileType<HomingMissile>(); }, 4);
			list[index++] = new WeightedChance(() => { item = ModContent.TileType<SpaceJumpBootsTile>(); }, 4);
			if (NPC.downedQueenBee || Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues)
			{
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<SpazerTile>(); }, 8);
			}
			if (NPC.downedBoss3 || Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues)
			{
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<IceBeamTile>(); }, 8);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<IceMissile>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<SpazerCombo>(); }, 4);
			}
			if (NPC.downedMechBoss2 || Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues)
			{
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<ChargeBeamV2Tile>(); }, 4);
			}
			if (NPC.downedMechBoss1 || Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues)
			{
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<WaveBeamV2Tile>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<Flamethrower>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<PlasmaMachinegun>(); }, 4);
			}
			if (NPC.downedMechBoss3 || Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues)
			{
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<WideBeamTile>(); }, 4);
			}
			if (bossesDown.HasFlag(MetroidBossDown.downedKraid) || Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues)
			{
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<PlasmaBeamGreenTile>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<PlasmaBeamRedTile>(); }, 4);
			}
			if (Main.hardMode || Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues)
			{
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<SuperMissile>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<IceSpreader>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<SeekerMissile>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<Wavebuster>(); }, 4);
			}
			if (NPC.downedPlantBoss || Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues)
			{
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<NovaCombo>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<NovaBeamTile>(); }, 4);
			}
			if (NPC.downedMoonlord || Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues)
			{
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<StardustCombo>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<StardustMissile>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<SolarCombo>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<NebulaCombo>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<NebulaMissile>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<SolarBeamTile>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<StardustBeamTile>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<VortexBeamTile>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<LuminiteBeamTile>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<NebulaBeamTile>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<OmegaCannonTile>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<PhazonBeamTile>(); }, 4);
			}
			if (NPC.downedMechBossAny || Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues)
			{
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<DiffusionMissile>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<SpaceJumpTile>(); }, 4);
			}
			if (NPC.downedChristmasIceQueen || Configs.MConfigMain.Instance.drunkWorldHasDrunkStatues)
			{
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<IceBeamV2Tile>(); }, 4);
				list[index++] = new WeightedChance(() => { item = ModContent.TileType<IceSuperMissile>(); }, 4);
			}
			Array.Resize(ref list, index);
			double numericValue = WorldGen.genRand.Next(0, (int)list.Sum(p => p.Ratio));

			foreach (WeightedChance parameter in list)
			{
				numericValue -= parameter.Ratio;

				if (!(numericValue <= 0)) { continue; }

				parameter.Func();
				break;
			}
			return item;
		}

	}
}
