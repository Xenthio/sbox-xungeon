using Sandbox;
using System;

[Library( "dg_npc_test", Description = "Test NPC" )]
public partial class npc_test : NPC
{
	public override string ItemName => "Test Npc";
	public override float maxHealth => 100;
	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/citizen/citizen.vmdl" );

		EnableHitboxes = true;
	}

	public override void onHoverStart()
	{
		base.onHoverStart();
		/*if ( IsClient ) 
		{

			Log.Info( "oh" );
			infoPanel = new ItemInfoPanel( this ); 
			infoPanel.Transform = Transform;
			
		}*/
	}
	public override void onHoverEnd()
	{

		//base.onHoverEnd();
		base.onHoverEnd();
		//infoPanel.Delete();
	}
}

