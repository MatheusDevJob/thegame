using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using thegame.Core;

namespace thegame.Entities.WorldObjects.Interactables;

public class Bau : Entity
{
    private const int TileSize = 16;
    public int QtdSlots { get; private set; } = 16;
    public List<ItemStackSave> Items { get; set; }

    public bool Aberto { get; private set; }

    public Bau(GameContext context, Vector2 posicao) : base(context, "Bau", posicao)
    {
        SpriteRow = 30;
        SpriteColumn = 38;
        BloqueiaMovimento = true;
        IsSpawnavel = true;
    }

    public override void Update(GameTime gameTime)
    {
        if (!Aberto) return;

        if (IsPlayerFartherThanMe(1))
            CloseBau();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        if (!Aberto) return;

        Rectangle sourceTampa = new(
            SpriteColumn * FrameWidth,
            (SpriteRow - 1) * FrameHeight,
            FrameWidth,
            FrameHeight
        );

        Rectangle destinationTampa = new(
            (int)(Posicao.X + DrawOffset.X),
            (int)(Posicao.Y + DrawOffset.Y - FrameHeight),
            FrameWidth,
            FrameHeight
        );

        spriteBatch.Draw(Sprite, destinationTampa, sourceTampa, Color.White);
    }


    public void OpenBau()
    {
        Aberto = true;
        SpriteColumn = 39;
        Context.State.EntidadeEmFoco = this;
    }

    public void CloseBau()
    {
        Aberto = false;
        SpriteColumn = 38;
        Context.State.EntidadeEmFoco = null;
    }

    public void AlimentarBau(Dictionary<string, string> data)
    {
        List<ItemStackSave> ItemsSave = [];
        if (!data.TryGetValue("items", out string json) || string.IsNullOrWhiteSpace(json))
        {
            for (int i = 0; i < QtdSlots; i++)
            {
                ItemsSave.Add(null);
            }
        }
        else
        {
            ItemsSave = JsonSerializer.Deserialize<List<ItemStackSave>>(json) ?? [];
            while (ItemsSave.Count < QtdSlots)
            {
                ItemsSave.Add(null);
            }
        }

        Items = ItemsSave;
    }
}