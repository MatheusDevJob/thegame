using System.Collections.Generic;
using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities.Decoracoes;

public class Cerca : Entity
{
    private const int TileSize = 16;

    public Point TilePosition => new(
        (int)Posicao.X / TileSize,
        (int)Posicao.Y / TileSize
    );

    private static readonly Dictionary<int, Point> SpriteByMask = new()
    {
        // X é coluna ------------ Y é linha
        [0] = new Point(0, 3),   // isolada

        [1] = new Point(0, 2),   // cima
        [2] = new Point(1, 0),   // direita
        [4] = new Point(0, 0),   // baixo
        [8] = new Point(3, 0),   // esquerda

        [5] = new Point(0, 1),   // cima + baixo
        [10] = new Point(2, 0),  // esquerda + direita

        [3] = new Point(1, 3),   // cima + direita
        [6] = new Point(1, 1),   // direita + baixo
        [12] = new Point(3, 1),  // baixo + esquerda
        [9] = new Point(3, 3),   // esquerda + cima

        [7] = new Point(1, 2),   // cima + direita + baixo
        [14] = new Point(2, 1),  // direita + baixo + esquerda
        [13] = new Point(3, 2),  // baixo + esquerda + cima
        [11] = new Point(2, 3),  // esquerda + cima + direita

        [15] = new Point(2, 2),  // todos os lados
    };

    public Cerca(GameContext context, Vector2 posicao) : base(context, "Cerca", posicao)
    {
        BloqueiaMovimento = true;

        SpriteRow = 1;
        SpriteColumn = 1;
        IsSpawnavel = true;
    }

    public void AtualizarSprite(EntityWorld world)
    {
        Point tile = TilePosition;

        int mask = 0;

        if (world.HasCercaAt(new Point(tile.X, tile.Y - 1), this))
            mask |= 1; // cima

        if (world.HasCercaAt(new Point(tile.X + 1, tile.Y), this))
            mask |= 2; // direita

        if (world.HasCercaAt(new Point(tile.X, tile.Y + 1), this))
            mask |= 4; // baixo

        if (world.HasCercaAt(new Point(tile.X - 1, tile.Y), this))
            mask |= 8; // esquerda

        Point sprite = SpriteByMask.GetValueOrDefault(mask, Point.Zero);

        SpriteColumn = sprite.X;
        SpriteRow = sprite.Y;
    }
}