using Microsoft.Xna.Framework;
using thegame.Core;
using thegame.Entities;
using thegame.Entities.Npcs;

namespace thegame.Maps;

public class CityMap(GameContext context) : BaseMap(context, "city", "Maps/HomeMap.tmj")
{
    protected override string[] LayersBeforeEntities => ["Ground", "cerca casa", "Back"];
    protected override string[] LayersAfterEntities => ["Front"];

    // protected override void OnTileClicked(Point tile)
    // {
    //     logs.Add($"Clicou no tile! -> {tile.X} - {tile.Y}");
    //     // clicou no chão/tile
    // }

    public override void OnEnter()
    {
        base.OnEnter();
        EntityWorld.Add(new Aldeao(
            Context,
            "Aldeão",
            "Olá, viajante!",
            new Vector2(1100, 205)
        ));
    }

    protected override void UpdateMap(GameTime gameTime)
    {
        foreach (Npc npc in EntityWorld.GetEntities<Npc>())
            npc.UpdateInteraction(Context.State.Player);

        base.UpdateMap(gameTime);
    }
}