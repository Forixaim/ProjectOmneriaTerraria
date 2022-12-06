using Terraria;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;

namespace ProjectOmneriaTerraria.Biomes
{
	internal class UnderworldBiome : ILoadable, IShoppingBiome
	{
		public string NameKey => "Underworld";

		public bool IsInBiome(Player player) => player.ZoneUnderworldHeight;

		public void Load(Mod mod)
		{
			
		}

		public void Unload()
		{

		}
	}
}
