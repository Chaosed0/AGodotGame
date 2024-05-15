
using EdTestGame.Backend;
using Godot;

public partial class CreatureTextureMap : Resource
{
    [Export]
    public CreatureType type;
    
    [Export]
    public Texture2D tex;
}
