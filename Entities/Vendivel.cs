using Microsoft.Xna.Framework;
using thegame.Core;

namespace thegame.Entities;

public class Vendivel(GameContext context, string id, Vector2 posicao, float life = 0, float damage = 0) : Entity(context, id, posicao, life, damage)
{
    public int Preco { get; protected set; }
}