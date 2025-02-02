﻿using Terraria;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MetroidMod.Default
{
	[Autoload(false)]
	internal class MBAddonTile : ModTile
	{
		public ModMBAddon modMBAddon;

		public override string Texture => modMBAddon.TileTexture;

		public override string Name => modMBAddon.Name + "Tile";

		public MBAddonTile(ModMBAddon modMBAddon)
		{
			this.modMBAddon = modMBAddon;
		}

		public override void SetStaticDefaults()
		{
			modMBAddon.TileType = Type;
			//ItemDrop= modMBAddon.ItemType;
			Main.tileFrameImportant[Type] = true;
			Main.tileBlockLight[Type] = true;
			Main.tileSpelunker[Type] = true;
			Main.tileNoAttach[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.Table | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			TileID.Sets.DisableSmartCursor[Type] = true;
		}

		public override void NumDust(int i, int j, bool fail, ref int num) => num = fail ? 1 : 3;
		public override bool Slope(int i, int j) => false;
		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = modMBAddon.ItemType;
		}
		public override bool RightClick(int i, int j)
		{
			if (!modMBAddon.CanKillTile(i, j)) { return true; }
			WorldGen.KillTile(i, j, false, false, false);
			if (Main.netMode == NetmodeID.MultiplayerClient && !Main.tile[i, j].HasTile)
			{
				NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, (float)i, (float)j, 0f, 0, 0, 0);
			}
			return true;
		}
		public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
		{
			if (!modMBAddon.CanKillTile(i, j)) { fail = true; }
		}
	}
}
