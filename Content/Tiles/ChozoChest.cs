using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.GameContent.ObjectInteractions;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MetroidMod.Content.Tiles
{
	public class ChozoChest : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileSpelunker[Type] = true;
			Main.tileContainer[Type] = true;
			Main.tileShine2[Type] = true;
			Main.tileShine[Type] = 1200;
			Main.tileFrameImportant[Type] = true;
			Main.tileNoAttach[Type] = true;
			Main.tileOreFinderPriority[Type] = 500;
			TileID.Sets.HasOutlines[Type] = true;
			TileID.Sets.BasicChest[Type] = true;
			TileID.Sets.DisableSmartCursor[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.newTile.HookCheckIfCanPlace = new PlacementHook(Chest.FindEmptyChest, -1, 0, true);
			TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(Chest.AfterPlacement_Hook, -1, 0, false);
			TileObjectData.newTile.AnchorInvalidTiles = new[] { 127 };
			TileObjectData.newTile.StyleHorizontal = true;
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.AnchorBottom = new AnchorData(AnchorType.SolidTile | AnchorType.SolidWithTop | AnchorType.SolidSide, TileObjectData.newTile.Width, 0);
			TileObjectData.addTile(Type);
			LocalizedText name = CreateMapEntryName();
			//name.SetDefault("Chozodian Chest");
			AddMapEntry(new Color(200, 200, 200), name);
			DustType = 87;
			TileID.Sets.DisableSmartCursor[Type] = true;//DisableSmartCursor = true;
			AdjTiles = new int[] { TileID.Containers };
		}

		public override bool HasSmartInteract(int i, int j, SmartInteractScanSettings settings) => true;

		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = 1;
		}
		public override LocalizedText DefaultContainerName(int frameX, int frameY)
		{
			int option = frameX / 36;
			return CreateMapEntryName();
		}
		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Chest.DestroyChest(i, j);
		}

		public override bool RightClick(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			Main.mouseRightRelease = false;
			int left = i;
			int top = j;
			if (tile.TileFrameX % 36 != 0)
			{
				left--;
			}
			if (tile.TileFrameY != 0)
			{
				top--;
			}
			if (player.sign >= 0)
			{
				SoundEngine.PlaySound(SoundID.MenuClose);
				player.sign = -1;
				Main.editSign = false;
				Main.npcChatText = "";
			}
			if (Main.editChest)
			{
				SoundEngine.PlaySound(SoundID.MenuTick);
				Main.editChest = false;
				Main.npcChatText = "";
			}
			if (player.editedChestName)
			{
				NetMessage.SendData(MessageID.SyncPlayerChest, -1, -1, NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f, 0f, 0f, 0, 0, 0);
				player.editedChestName = false;
			}
			if (Main.netMode == NetmodeID.MultiplayerClient)
			{
				if (left == player.chestX && top == player.chestY && player.chest >= 0)
				{
					player.chest = -1;
					Recipe.FindRecipes();
					SoundEngine.PlaySound(SoundID.MenuClose);
				}
				else
				{
					NetMessage.SendData(MessageID.RequestChestOpen, -1, -1, null, left, (float)top, 0f, 0f, 0, 0, 0);
					Main.stackSplit = 600;
				}
			}
			else
			{
				int chest = Chest.FindChest(left, top);
				if (chest >= 0)
				{
					Main.stackSplit = 600;
					if (chest == player.chest)
					{
						player.chest = -1;
						SoundEngine.PlaySound(SoundID.MenuClose);
					}
					else
					{
						SoundEngine.PlaySound(player.chest < 0 ? SoundID.MenuOpen : SoundID.MenuTick);
						player.chest = chest;
						Main.playerInventory = true;
						Main.recBigList = false;
						player.chestX = left;
						player.chestY = top;
					}
					Recipe.FindRecipes();
				}
			}
			return true;
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			Tile tile = Main.tile[i, j];
			int left = i;
			int top = j;
			if (tile.TileFrameX % 36 != 0)
			{
				left--;
			}
			if (tile.TileFrameY != 0)
			{
				top--;
			}
			int chest = Chest.FindChest(left, top);
			player.cursorItemIconID = -1;
			if (chest < 0)
			{
				player.cursorItemIconText = Language.GetTextValue("LegacyChestType.0");
			}
			else
			{
				string defaultName = TileLoader.DefaultContainerName(tile.TileType, tile.TileFrameX, tile.TileFrameY); // This gets the ContainerName text for the currently selected language
				player.cursorItemIconText = Main.chest[chest].name.Length > 0 ? Main.chest[chest].name : defaultName;
				player.cursorItemIconID = ModContent.ItemType<Items.Tiles.ChozoChest>();
				if (player.cursorItemIconText == "Chozodian Chest")
				{
					player.cursorItemIconText = "";
				}
			}
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
		}

		public override void MouseOverFar(int i, int j)
		{
			MouseOver(i, j);
			Player player = Main.LocalPlayer;
			if (player.cursorItemIconText == "")
			{
				player.cursorItemIconEnabled = false;
				player.cursorItemIconID = 0;
			}
		}
	}
}
