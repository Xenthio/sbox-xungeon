using Sandbox;
using Sandbox.Component;
using System;

public partial class NPC : AnimEntity, IGlow
{


	private DamageInfo lastDamage;
	public virtual string ItemName => "Npc class name";
	public healthPanel hpPanel { get; set; }
	public virtual float maxHealth => 100;
	//public int Health = 100;

	public ModelEntity Corpse { get; set; }
	public override void Spawn()
	{
		base.Spawn();
		SetModel( "models/sbox_props/watermelon/watermelon.vmdl" );
		this.Health = maxHealth;
		UsePhysicsCollision = true;
		SetupPhysicsFromModel( PhysicsMotionType.Keyframed );
		Speed = Rand.Float( 100, 300 );
	}
	public virtual void onHoverStart()
	{
		if ( IsClient )
		{
			if ( Health > 0 )
			{
				hpPanel = new healthPanel( this );
				hpPanel.Transform = Transform;
			}
		}
		if (Health > 0) 
		{
			var glow = this.Components.GetOrCreate<Glow>();
			glow.Active = true;
			glow.RangeMin = 0;
			glow.RangeMax = 1000;
			glow.Color = new Color( 0.1f, 1.0f, 0.2f, 1.0f );
		}
	}
	public virtual void onHoverEnd()
	{
		try
		{
			hpPanel.Delete();
		}
		catch { }
		//prevEnt.Components.TryGet<Glow>( out var childglow );
		//childglow.Active = false;
		try
		{

			var glow = this.Components.GetOrCreate<Glow>();
			glow.Active = false;
		}
		catch { }

	}
	public virtual void removeUI()
	{
		hpPanel.Delete();
	}
	public override void TakeDamage( DamageInfo info )
	{
		if ( GetHitboxGroup( info.HitboxIndex ) == 1 )
		{
			info.Damage *= 10.0f;
		}

		lastDamage = info;

		// Ow! we took damage! we should be alert here for any of our enemies...
		base.TakeDamage( info );
	}
	public override void OnKilled()
	{

		
		onHoverEnd();
		//SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
		//PhysicsBody.ApplyForceAt( lastDamage.Position, lastDamage.Force );
		base.OnKilled();

		BecomeRagdollOnClient( Velocity, lastDamage.Flags, lastDamage.Position, lastDamage.Force, GetHitboxBone( lastDamage.HitboxIndex ) );
		PlaybackRate = 0;

		//base.OnKilled();
	}

	[ConVar.Replicated]
	public static bool nav_drawpath { get; set; }


	int search = 0;
	int boredom = 0;
	int alert = 0;

	int dumbassTick = 0;
	Vector3 placeOfInterest = Vector3.Zero;
	float Speed;

	private static Random randChance = new Random();
	private int reactDelay = randChance.Next( 5, 15 );
	private int searchDelay = randChance.Next( 180, 220 );
	int rand;

	NavPath Path = new NavPath();
	public NavSteer Steer;

	Vector3 goLookAt = Vector3.Zero;
	Vector3 Velocity;
	Vector3 InputVelocity;

	Vector3 LookDir;
	SoundResult soundRes;
	Vector3 lastHeard = Vector3.Zero;
	public void Ticker()
	{
		//Vector3 look = Position + Vector3.Random * 10;
		//boredom++;
		if ( boredom > 200 )
		{
			//Velocity = look * 2;
			//Log.Info( "poo" );
			//boredom = 0;
			/*
			InputVelocity = look * 3;
			Move( Time.Delta );
			LookDir = Vector3.Lerp( LookDir, InputVelocity.WithZ( 0 ) * 1000, Time.Delta * 100.0f );

			CitizenAnimationHelper animHelper = new CitizenAnimationHelper( this );
			animHelper.WithLookAt( EyePosition + LookDir );
			animHelper.WithVelocity( Velocity);
			animHelper.WithWishVelocity( InputVelocity ); */
			//Position = Vector3.Lerp( Position, look * 1, Time.Delta * 1.0f );
		}
	}
	[Event.Tick.Server]
	public void Tick()
	{
		//try
		//{
			AIThink();
		//} catch { }	
	}
	public void Bored()
	{
		// Bordem! Everyone hates being bored, so naturally these NPC's should aswell.
		// If we've reached here, it means we're bored. Let's go look for something to do.
		
		// TODO: Do something random here, right now we only have look left and right
		GameTask.RunInThreadAsync( lookLeftRight ); // Look left and right
		// Potentially do things like look around and if we see something interesting go walk over to it
		// Maybe if other enemies are around go look at them, maybe nod or something aswell
		// Maybe go pace around or something.
		boredom = 0;
	}
	public void Search()
	{
		Log.Info( "poo" );
		var wander = new Sandbox.Nav.Wander();
		wander.MinRadius = 150;
		wander.MaxRadius = 2000;


		if ( !wander.FindNewTarget( Position ) )
		{
			DebugOverlay.Text( EyePosition, "COULDN'T FIND A WANDERING POSITION!", 5.0f );
		}
		else
		{
			Steer = new NavSteer();
			Steer.Target = wander.Target;
			//Steer = wander;
		}


		boredom = 0;
	}

	private void lookLeftRight()
	{
		Log.Info( "ltr" );

		goLookAt = Position + (Vector3.Left * 50) + ( Vector3.Backward * 1.1f );
		GameTask.Delay( 1500 );

		goLookAt = Position + (Vector3.Right * 50) + (Vector3.Backward * 1.1f);

		GameTask.Delay( 1500 );

		goLookAt = Position + (Vector3.Forward * 50); //+ (Vector3.Backward * 1.1f);
	}
	private void lookAt(Vector3 toLook)
	{

		goLookAt = toLook;
		/*if (i < 200, i++) {

		}*/
		GameTask.Delay( 2000 );

		goLookAt = Position + (Vector3.Right * 50) + (Vector3.Backward * 1.1f);

		GameTask.Delay( 2000 );

	}

	private void LookingAtWall()
	{
		// turn around, thats a wall dumbass.
		// TODO: what if this is a special wall, like a door or a button (if i even get around to something as deep as buttons)
		
		Steer = new NavSteer();
		Steer.Target = Position + (Vector3.Backward * 10);
	}

		public void AIThink()
	{ 
		float i = Position.z;
		//Vector3 look = Position + Vector3.Random * 10;
		//look.z = i;
		
		// TODO: Abstract this
		
		boredom++;
		
		
		
		if ( alert > 0 )
		{
			alert++;
		}
		if ( search > 0 )
		{
			search--;
		}
		if ( dumbassTick > 0 )
		{
			dumbassTick--;
		}

		if (boredom == 30) {

			Steer = null;
		
		}

		if ( search > 0 )
		{

			if (search == Math.Round( search / (double)searchDelay, 0 ) * searchDelay)

			{
				Search();
			}
		}
		if ( boredom > 200 )
		{
			Bored();
		}

		// ------ The code for the NPC to see ------ 
		// todo fix this absolute SHIT
		TraceResult tr = Trace.Ray( EyePosition, EyePosition + LookDir * (2000 * Scale) )
			.Radius( 30.0f )
			.HitLayer( CollisionLayer.Debris )
			.Ignore( this )
			.EntitiesOnly()
			.Run();
			// also make it so that the source of sounds are found if in "what was that noise?" mode.

		var ent = tr.Entity;
		if ( ent != null && ent.IsValid && ent is DungeonPlayer )
		{
			// ALERT ENEMY!!!!
			if ( Steer == null )
			{
				Steer = new NavSteer();
			}
			Steer.Target = tr.Entity.Position;
			

		} else if (ent != null && ent.IsValid)
		{
			if ( dumbassTick > 900 )
			{
				Log.Info( "oo shiny" );
				lookAt( ent.Position );
			}
			else if ( dumbassTick == 0 )
			{
				dumbassTick = 1000;
			}
			// look at props and stuff when they're close to them
		}
		// wall detection FIX THIS WHAT THE FUCK

		/*TraceResult tr2 = Trace.Ray( EyePosition, EyePosition + LookDir * (20 * Scale) )
			.Radius( 1.0f )
			.HitLayer( CollisionLayer.Debris )
			.Ignore( this )
			.WorldOnly()
			.Run();
		MoveHelper move = new( Position, Velocity );
		var tr2 = move.TraceDirection( Vector3.Forward * 10.0f );
		if (move.HitWall)
		{
			// too close to wall D:
			if (dumbassTick > 900)
			{
				Log.Info( "WALL" );
				LookingAtWall();
			} 
			else if (dumbassTick == 0)
			{
				dumbassTick = 1000;
			}
			
		}*/



		soundRes = Sounds.isSoundInRange( Position, 700, 10, lastHeard );
		if ( soundRes.near )
		{
			lastHeard = soundRes.position;
			Log.Info( "heard!" );
			alert = 2;
			placeOfInterest = soundRes.position;
		}

		// we heard something, look around, go investigate
		if ( alert == reactDelay )
		{
			//Log.Info( "Alert" );
			//If theres a POI look over there, (TODO) else look around
			goLookAt = placeOfInterest;
			search = 2000;
		}
		if ( search > 150 )
		{
			rand = randChance.Next( 0, 120 );
			if ( rand == 99 )
			{
				Log.Info( "alert3" );
				GameTask.RunInThreadAsync( lookLeftRight );
			} else if (rand == 98)
			{
			
				goLookAt = placeOfInterest;
			}
			//Log.Info( "Alert2" );
		}

		if ( alert > 380 )
		{
			alert = 0;
			goLookAt = Vector3.Zero;
		}

		if (placeOfInterest != Vector3.Zero) // why do i need this 'if' again?
		{
			//i'm interested, huh

		}

		{

			InputVelocity = 0;

			if ( Steer != null )
			{

				Steer.Tick( Position );

				if ( !Steer.Output.Finished )
				{
					boredom = 0;
					InputVelocity = Steer.Output.Direction.Normal;
					Velocity = Velocity.AddClamped( InputVelocity * Time.Delta * 500, Speed );

				} 
				

				
			}
			Move( Time.Delta );

			var walkVelocity = Velocity.WithZ( 0 );
			if ( walkVelocity.Length > 0.5f || goLookAt != Vector3.Zero)
			{
				var turnSpeed = walkVelocity.Length.LerpInverse( 0, 100, true );

				var targetRotation = Rotation.LookAt( walkVelocity.Normal, Vector3.Up );
				if ( goLookAt != Vector3.Zero )
				{
					targetRotation = Rotation.LookAt( goLookAt.WithZ( 0 ).Normal, Vector3.Up );
				}
				Rotation = Rotation.Lerp( Rotation, targetRotation, turnSpeed * Time.Delta * 20.0f );
			}

			var animHelper = new CitizenAnimationHelper( this );

			//LookDir = Vector3.Lerp( LookDir, InputVelocity.WithZ( 0 ) * 1000, Time.Delta * 100.0f );
			if ( goLookAt != Vector3.Zero )
			{

				LookDir = Vector3.Lerp( LookDir, goLookAt, Time.Delta * 100.0f );


				animHelper.WithLookAt( LookDir );
			}
			else
			{

				LookDir = Vector3.Lerp( LookDir, InputVelocity.WithZ( 0 ) * 1000, Time.Delta * 100.0f );


				animHelper.WithLookAt( EyePosition + LookDir );
			}
			animHelper.WithVelocity( Velocity );
			animHelper.WithWishVelocity( InputVelocity );
			
			
		}
	}
	protected virtual void Move( float timeDelta )
	{
		var bbox = BBox.FromHeightAndRadius( 64, 4 );
		//DebugOverlay.Box( Position, bbox.Mins, bbox.Maxs, Color.Green );

		MoveHelper move = new( Position, Velocity );
		move.MaxStandableAngle = 50;
		move.Trace = move.Trace.Ignore( this ).Size( bbox );

		if ( !Velocity.IsNearlyZero( 0.001f ) )
		{
			//	Sandbox.Debug.Draw.Once
			//						.WithColor( Color.Red )
			//						.IgnoreDepth()
			//						.Arrow( Position, Position + Velocity * 2, Vector3.Up, 2.0f );

				move.TryUnstuck();
			 
				move.TryMoveWithStep( timeDelta, 30 );
		}

		
		{
			var tr = move.TraceDirection( Vector3.Down * 10.0f );

			if ( move.IsFloor( tr ) )
			{
				GroundEntity = tr.Entity;

				if ( !tr.StartedSolid )
				{
					move.Position = tr.EndPosition;
				}

				if ( InputVelocity.Length > 0 )
				{
					var movement = move.Velocity.Dot( InputVelocity.Normal );
					move.Velocity = move.Velocity - movement * InputVelocity.Normal;
					move.ApplyFriction( tr.Surface.Friction * 10.0f, timeDelta );
					move.Velocity += movement * InputVelocity.Normal;

				}
				else
				{
					move.ApplyFriction( tr.Surface.Friction * 10.0f, timeDelta );
				}
			}
			else
			{
				GroundEntity = null;
				move.Velocity += Vector3.Down * 900 * timeDelta;
			}
		}

		Position = move.Position;
		Velocity = move.Velocity;
	}


	public void Think()
	{

	}
	
}

