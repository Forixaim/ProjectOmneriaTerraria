using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace ProjectOmneriaTerraria.World
{
	internal class WorldValues : ModSystem
	{
		public bool CharleFought = false;
		public bool CharleDefeated = false;
		public int CharleDeaths = 0;
		//The first 2 values of Charlemagne
		public bool NPCCharlemagneThingSaid = false;
		public bool NPCCharlemagneThingSaid2 = false;
		public bool NPCCharlemagneThingSaid3 = false;
		public bool NPCCharlemagneThingSaid4 = false;
	}
}
