using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.Component;

public partial class ItemInfoPanel : WorldPanel
{

	IGlow owner;
	Label nameLabel;
	Label descriptionLabel;
	public ItemInfoPanel(Entity ent)
	{
		owner = ent as IGlow;
		PanelBounds = new Rect( -500, -100, 1000, 200 );
		Log.Info( "ok" );
		//PanelBounds = new Rect( 0, -80, 800, 160 );
		nameLabel = Add.Label( owner.ItemName, "itemName" );
		descriptionLabel = Add.Label( owner.ItemDescription, "itemDesc" );

		StyleSheet.Load( "/ui/ItemInfoPanel.scss" );

	}

	[Event.PreRender]
	public void FrameUpdate()
	{
		var tx =  owner.Transform;
		tx.Position = owner.CollisionWorldSpaceCenter;
		tx.Position += Vector3.Up * 24.0f;
		tx.Rotation = Rotation.LookAt( -CurrentView.Rotation.Forward );
		
		this.Transform = tx;
	}
}

