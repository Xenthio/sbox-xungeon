using Sandbox;
using System;
using System.Linq;

public interface IGlow
{

	public virtual float Health => 1337;
	public virtual float maxHealth => 42069;
	public virtual string ItemName => "You should not see this";
	public virtual string ItemDescription => "Description Placeholder";
	//public ItemInfoPanel infoPanel { get; set; }

	//object Transform { get; set; }

	Vector3 CollisionWorldSpaceCenter { get; }
	public Transform Transform { get;}
	public BBox CollisionBounds { get; }

	public virtual void onHoverStart() { }
	public virtual void onHoverEnd() { }
}
