using Microsoft.Xna.Framework;
using thegame.Core;
using thegame.Entities;
using thegame.Entities.Npcs;

namespace thegame.Maps;

public class CityMap : BaseMap
{
    protected override string[] LayersBeforeEntities => ["Ground", "cerca casa", "Back"];
    protected override string[] LayersAfterEntities => ["Front"];

    public CityMap(GameContext context) : base(context, "city", "Maps/HomeMap.tmj")
    {
        EntityWorld.Add(new Aldeao(
            context,
            "Aldeão",
            "Olá, viajante!",
            new Vector2(1100, 205),
            "npc-pack/Old man/Old man walk-Sheet"
        ));

        foreach (var obj in Map.GetObjects("Objects"))
        {
            Entity entity = EntityFactory.Create(Context, obj);

            if (entity != null)
                EntityWorld.Add(entity);

            // if (obj.Type == "Arvore")
            // {
            //     EntityWorld.Add(new Arvore(
            //         Context,
            //         new Vector2(obj.X, obj.Y)
            //     ));
            // }
        }
    }


    protected override void UpdateMap(GameTime gameTime)
    {
        foreach (Npc npc in EntityWorld.GetEntities<Npc>())
            npc.UpdateInteraction(Context.State.Player);
    }
}