using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.Component;

public partial class healthPanel : WorldPanel
{

	IGlow owner;
	Label nameLabel;
	Label healthLabel;
	Panel healthBar;
	public healthPanel(Entity ent)
	{
		owner = ent as IGlow;
		PanelBounds = new Rect( -500, -100, 1000, 200 );
		Log.Info( "ok" );
		//PanelBounds = new Rect( 0, -80, 800, 160 );
		nameLabel = Add.Label( owner.ItemName, "itemName" );
		healthLabel = Add.Label( owner.ItemDescription, "health" );
		healthBar = Add.Panel( "healthbar" );

		StyleSheet.Load( "/ui/healthPanel.scss" );

	}
	public override void Tick()
	{
		base.Tick();
		healthLabel.Text = owner.Health.ToString() + "/" + owner.maxHealth.ToString();
		healthBar.Style.Height = 20;
		healthBar.Style.Width = Length.Percent((owner.Health / owner.maxHealth) * 100);
	}
	[Event.PreRender]
	public void FrameUpdate()
	{
		var tx =  owner.Transform;
		tx.Position = owner.CollisionWorldSpaceCenter;
		tx.Position += Vector3.Up * 0;
		tx.Position += Vector3.Up * (owner.CollisionBounds.Size.z / 1.5f);
		tx.Rotation = Rotation.LookAt( -CurrentView.Rotation.Forward );
		
		this.Transform = tx;
	}
}

