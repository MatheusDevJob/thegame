using System.Collections.Generic;
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

        List<LojaItens> lojaItensEduarda = [
            new LojaItens{ItemId= "Cenoura", ItemCompraId = "SementeCenoura", Preco = 25, Quantidade = 3},
            new LojaItens{ItemId= "Beterraba", ItemCompraId = "SementeBeterraba", Preco = 25, Quantidade = 3},
            new LojaItens{ItemId= "Repolho", ItemCompraId = "SementeRepolho", Preco = 25, Quantidade = 3}
        ];

        EntityWorld.Add(new Vendedor(
            Context,
            "Eduarda",
            "Olá, viajante!",
            new Vector2(1295, 125),
            lojaItensEduarda
        ));

        EntityWorld.Add(new Vendedor(
            Context,
            "Soldado",
            "Olá, viajante!",
            new Vector2(1445, 125),
            [
                new LojaItens{ItemId= "Couve", ItemCompraId = "SementeCouve", Preco = 25, Quantidade = 5},
                new LojaItens{ItemId= "Chirivia", ItemCompraId = "SementeChirivia", Preco = 25, Quantidade = 1},
                new LojaItens{ItemId= "Batata", ItemCompraId = "SementeBatata", Preco = 25, Quantidade = 3}
            ]
        ));
    }

    protected override void UpdateMap(GameTime gameTime)
    {
        foreach (Npc npc in EntityWorld.GetEntities<Npc>())
            npc.UpdateInteraction(Context.State.Player);

        base.UpdateMap(gameTime);
    }
}