using System;
using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Interactables;

public class Bau : Entity
{
    private const int TileSize = 16;
    public bool Aberto { get; private set; }

    public Bau(GameContext context, Vector2 posicao) : base(context, "Bau", posicao)
    {
        SpriteRow = 30;
        SpriteColumn = 38;
        BloqueiaMovimento = true;
        AtualizarHitbox();
    }

    public override void Update(GameTime gameTime)
    {
        if (!Aberto) return;

        if (IsPlayerFartherThanMe(1))
            CloseBau();
    }

    public void OpenBau()
    {
        Aberto = true;
        SpriteColumn = 39;
    }

    public void CloseBau()
    {
        Aberto = false;
        SpriteColumn = 38;
    }
}