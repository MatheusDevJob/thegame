using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using thegame.Core;

namespace thegame.Entities.Npcs;

public abstract class Npc : Entity
{
    public string Nome { get; protected set; }
    public string Fala { get; protected set; }

    private double _timer;
    private readonly double _frameSpeed = 0.2;
    private int _currentFrame;
    private readonly SpriteEffects _spriteEffects = SpriteEffects.None;
    private readonly int _frameWidth = 32;
    private readonly int _frameHeight = 32;
    private readonly int _currentRow = 0;
    private readonly float _scale = 0.7f;

    protected float DistanciaInteracao = 30f;
    protected bool PlayerPerto;
    protected Vector2 Center => Posicao + new Vector2(_frameWidth * _scale / 2f, _frameHeight * _scale / 2f);

    protected bool MostrarFala;

    protected Npc(GameContext context, string nome, string fala, Vector2 posicao) : base(context, nome, posicao, 0)
    {
        Nome = nome;
        Fala = fala;
        AtualizarHitbox();
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        UpdateAnimation(gameTime);
    }

    protected int UpdateAnimation(GameTime gameTime)
    {
        _timer += gameTime.ElapsedGameTime.TotalSeconds;

        if (_timer >= _frameSpeed)
        {
            _timer = 0;
            _currentFrame++;

            if (_currentFrame >= 6)
                _currentFrame = 0;
        }

        return _currentFrame;
    }

    public virtual void UpdateInteraction(Player player)
    {
        float distancia = Vector2.Distance(Center, player.Center);
        PlayerPerto = distancia <= DistanciaInteracao;

        if (!PlayerPerto)
        {
            MostrarFala = false;
            return;
        }

        if (Context.Input.IsKeyPressed(Keys.E))
            NpcAction();
    }

    protected abstract void NpcAction();

    public override void Draw(SpriteBatch spriteBatch)
    {
        Rectangle sourceRectangle = new(
            _currentFrame * _frameWidth,
            _currentRow * _frameHeight,
            _frameWidth,
            _frameHeight
        );

        Vector2 drawPosition = new(
            (float)Math.Round(Posicao.X),
            (float)Math.Round(Posicao.Y)
        );

        spriteBatch.Draw(
            Sprite,
            drawPosition,
            sourceRectangle,
            Color.White,
            0f,
            Vector2.Zero,
            _scale,
            _spriteEffects,
            0f
        );

        if (PlayerPerto && !MostrarFala)
        {
            Vector2 position = new(Center.X, Posicao.Y - 16);
            Context.UI.DrawKeyHint(spriteBatch, "E", position);
        }

        if (MostrarFala)
            Context.UI.DrawNpcSpeech(spriteBatch, Fala, Center, Posicao.Y);
    }

    protected override Rectangle CalcularHitbox(Vector2 posicao)
    {
        int visualWidth = (int)(_frameWidth * _scale);
        int visualHeight = (int)(_frameHeight * _scale);

        int hitboxWidth = 14;
        int hitboxHeight = 7;

        return new Rectangle(
            (int)(posicao.X + (visualWidth - hitboxWidth) / 2f),
            (int)(posicao.Y + visualHeight - hitboxHeight),
            hitboxWidth,
            hitboxHeight
        );
    }
}