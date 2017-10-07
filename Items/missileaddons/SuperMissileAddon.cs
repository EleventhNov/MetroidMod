using Terraria.ID;
using Terraria.ModLoader;

namespace MetroidMod.Items.missileaddons
{
	public class SuperMissileAddon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Super Missile");
			Tooltip.SetDefault("Missile Launcher Addon\n" +
			"Slot Type: Primary\n" +
			"Shots are more powerful and create a larger explosion\n" + 
			"+200% damage");
		}
		public override void SetDefaults()
		{
			item.width = 10;
			item.height = 14;
			item.maxStack = 1;
			item.value = 2500;
			item.rare = 4;
			/*item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.createTile = mod.TileType("ChargeBeamTile");*/
			MGlobalItem mItem = item.GetGlobalItem<MGlobalItem>(mod);
			mItem.missileSlotType = 1;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(null, "ChoziteBar", 4);
			recipe.AddIngredient(ItemID.Emerald, 3);
			recipe.AddIngredient(null, "EnergyTank");
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
