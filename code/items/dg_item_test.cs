using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;

using System;
using System.Linq;

[Library( "dg_item_test", Description = "Test Item" )]
partial class dg_item_test : Item, IUse
{

	public override string ItemName => "Item Test";
	public override string ItemDescription => "Item Description Placeholder";
	public override void Spawn()
	{
		Log.Info( "Yep " );
		SetModel( "models/sbox_props/watermelon/watermelon.vmdl" );
		SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );

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


	public override bool OnUse( Entity user )
	{
		base.OnUse( user );
		return true;
	}
}

