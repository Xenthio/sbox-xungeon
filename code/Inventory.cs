using Sandbox;
using System;
using System.Linq;

partial class Inventory : BaseInventory
{
	public Inventory( Player player ) : base( player )
	{
	}

	public override bool CanAdd( Entity entity )
	{
		if ( !entity.IsValid() )
			return false;

		if ( !base.CanAdd( entity ) )
			return false;

		return !IsCarryingType( entity.GetType() );
	}

	public override bool Add( Entity entity, bool makeActive = false )
	{
		var player = Owner as DungeonPlayer;
		var weapon = entity as Weapon;

		if ( !entity.IsValid() )
			return false;

		if ( IsCarryingType( entity.GetType() ) )
			return false;
		
		//
		// We don't want to pick up the same weapon twice
		// But we'll take the ammo from it Winky Face
		//
		/*if ( weapon != null && IsCarryingType( entity.GetType() ) )
		{
			var ammo = weapon.AmmoClip;
			var ammoType = weapon.AmmoType;

			if ( ammo > 0 )
			{
				player.GiveAmmo( ammoType, ammo );
				Sound.FromWorld( "dm.pickup_ammo", entity.Position );
				//PickupFeed.OnPickup( To.Single( player ), $"+{ammo} {ammoType}" );
				
			}

			//ItemRespawn.Taken( ent );

			// Despawn it
			entity.Delete();
			return false;
		}*/

		return base.Add( entity, makeActive );
	}

	public bool IsCarryingType( Type t )
	{
		return List.Any( x => x?.GetType() == t );
	}

	public override bool Drop( Entity ent )
	{
		if ( !Host.IsServer )
			return false;

		if ( !Contains( ent ) )
			return false;

		if ( ent is BaseCarriable bc )
		{
			bc.OnCarryDrop( Owner );
		}

		return ent.Parent == null;
	}
}
