using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class PhazonCore : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Phazon");
			// Tooltip.SetDefault("'Very radioactive.'\n" + "Glows with Phazon energy");

			Item.ResearchUnlockCount = 1;
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 1;
			Item.uniqueStack = true;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.PhazonCore>();
		}
	}
}
