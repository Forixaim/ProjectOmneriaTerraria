using Terraria;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.ModLoader;

namespace ProjectOmneriaTerraria.Biomes
{
	internal class SpaceBiome : ILoadable, IShoppingBiome
	{
		public string NameKey => "Space";

		public bool IsInBiome(Player player) => player.ZoneSkyHeight;

		public void Load(Mod mod)
		{
			
		}

		public void Unload()
		{

		}
	}
}
