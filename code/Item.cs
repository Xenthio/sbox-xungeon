using Sandbox;
using Sandbox.UI;
using Sandbox.Component;
using System;
using System.Linq;


	
	partial class Item : ModelEntity, IUse, IGlow
	{
		public virtual string ItemName => "You should not see this";
		public virtual string ItemDescription => "Description Placeholder";

		 public ItemInfoPanel infoPanel { get; set; }
		

		public virtual void onHoverStart()
		{
			if ( IsClient )
			{
				infoPanel = new ItemInfoPanel( this );
				infoPanel.Transform = Transform;
			}
			var glow = this.Components.GetOrCreate<Glow>();
			glow.Active = true;
			glow.RangeMin = 0;
			glow.RangeMax = 1000;
			glow.Color = new Color( 0.1f, 1.0f, 0.2f, 1.0f );
		}
		public virtual void onHoverEnd()
		{
			infoPanel.Delete();
			//prevEnt.Components.TryGet<Glow>( out var childglow );
			//childglow.Active = false;
			Log.Info( "HoverEnd" );
			try
			{
				var glow = this.Components.GetOrCreate<Glow>();
				glow.Active = false;
			} catch { }

		}
		public virtual void removeUI()
		{
			infoPanel.Delete();
		}
		public virtual bool OnUse( Entity user )
		{
			Log.Info( "I've been used!" );
			var glow = this.Components.GetOrCreate<Glow>();
			glow.Active = false;
			
			Delete();
			
			return true;
		}

		public bool IsUsable( Entity user )
		{
			return true;
		}
	}
