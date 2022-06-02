using Sandbox;
using Sandbox.Component;
using System;
using System.Linq;


	partial class DungeonPlayer : Player
	{
		//public int AmmoCount(AmmoType test) {return 100; }

		public ClothingContainer Clothing = new();

		public DungeonPlayer()
		{
			Inventory = new Inventory( this );
		}

		/// <summary>
		/// Initialize using this client
		/// </summary>
		public DungeonPlayer( Client cl ) : this()
		{
			// Load clothing from client data
			Clothing.LoadFromClient( cl );
		}

		Entity prevEnt;
		/// <summary>
		/// Called when the entity is first created 
		/// </summary>
		public override void Respawn()
		{

			//
			// Use a watermelon model
			//
			SetModel( "models/citizen/citizen.vmdl" );
			Controller = new WalkController();

			Animator = new StandardPlayerAnimator();
			CameraMode = new FirstPersonCamera();

			EnableAllCollisions = true;
			EnableDrawing = true;
			EnableHideInFirstPerson = true;
			EnableShadowInFirstPerson = true;

			Clothing.DressEntity( this );
			GiveAmmo( AmmoType.Pistol, 100 );
			base.Respawn();
		}


		/// <summary>
		/// Called every tick, clientside and serverside.
		/// </summary>
	public override void Simulate( Client cl )
	{
		base.Simulate( cl );


		//
		// Input requested a weapon switch
		//
		if ( Input.ActiveChild != null )
		{
			ActiveChild = Input.ActiveChild;
		}
		SimulateActiveChild( cl, ActiveChild );
		TickPlayerUse();

		if ( Input.Pressed( InputButton.Drop ) )
		{
			var dropped = Inventory.DropActive();
			if ( dropped != null )
			{
				if ( dropped.PhysicsGroup != null )
				{
					dropped.PhysicsGroup.Velocity = Velocity + (EyeRotation.Forward + EyeRotation.Up) * 300;
				}

				
			}
		}


	}

	/// <summary>
	/// Called every frame on the client
	/// </summary>
	///
	public override void FrameSimulate( Client cl )
	{

			base.FrameSimulate( cl );
			var tr = Trace.Ray( EyePosition, EyePosition + EyeRotation.Forward * (150 * Scale) )
			.Radius(1.5f)
			.HitLayer( CollisionLayer.Debris )
			.Ignore( this )
			.Run();
			var ent = tr.Entity;
			if ( ent.IsValid() && ent is IGlow && ent != prevEnt )
			{
			try
			{
				(prevEnt as IGlow).onHoverEnd();
			} catch { }
				(ent as IGlow).onHoverStart();
				prevEnt = ent;
				//var glow = ent.Components.GetOrCreate<Glow>();
				//glow.Active = true;
				//glow.RangeMin = 0;
				//glow.RangeMax = 1000;
				//glow.Color = new Color( 0.1f, 1.0f, 0.2f, 1.0f );
			
			} else if (prevEnt != null && prevEnt is IGlow && ent != prevEnt)
			{
				(prevEnt as IGlow).onHoverEnd();
				//prevEnt.Components.TryGet<Glow>( out var childglow );
				//childglow.Active = false;
				prevEnt = null;

			}
			 
		
			
	}

}

