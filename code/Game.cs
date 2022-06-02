
using Sandbox;
using Sandbox.UI.Construct;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


	/// <summary>
	/// This is your game class. This is an entity that is created serverside when
	/// the game starts, and is replicated to the client. 
	/// 
	/// You can use this to create things like HUDs and declare which player class
	/// to use for spawned players.
	/// </summary>
	public partial class MyGame : Sandbox.Game
	{

		public MyGame()
		{
		}

		/// <summary>
		/// A client has joined the server. Make them a pawn to play with
		/// </summary>
		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			// Create a pawn for this client to play with
			var player = new DungeonPlayer();
			client.Pawn = player;
			player.Respawn();

		}

		[ConCmd.Admin( "respawn_entities" )]
		public static void RespawnEntities()
		{
			Map.Reset( DefaultCleanupFilter );
		}


		

	}
