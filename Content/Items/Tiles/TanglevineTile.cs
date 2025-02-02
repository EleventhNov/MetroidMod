﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Content.Items.Tiles
{
	public class TanglevineTile : ModItem
	{
		public override void SetStaticDefaults()
		{
			// DisplayName.SetDefault("Nettori's Vegetation");

			Item.ResearchUnlockCount = 100;
		}
		public override void SetDefaults()
		{
			Item.width = 16;
			Item.height = 16;
			Item.maxStack = 9999;
			Item.useTurn = true;
			Item.autoReuse = true;
			Item.useAnimation = 15;
			Item.useTime = 10;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.consumable = true;
			Item.createTile = ModContent.TileType<Content.Tiles.TanglevineTile>();
		}
		public override void AddRecipes()
		{
			CreateRecipe(25)
				.AddIngredient(ItemID.Cobweb, 25)
				//.AddIngredient<XParasite>()
				.Register();
			CreateRecipe(1)
				.AddIngredient(ItemID.Cobweb, 1)
				//.AddIngredient<XParasite>()
				.Register();
		}
	}
}
