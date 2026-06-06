using System;
using Microsoft.Xna.Framework;
using thegame.Core;
using thegame.Entities;
using thegame.Entities.WorldObjects.Interactables;
using thegame.Entities.WorldObjects.Solo;

namespace thegame.Maps;

public class EntityInteractionManager(GameContext context, WorldActionService worldActions)
{
    private readonly GameContext _context = context;
    private readonly WorldActionService _worldActions = worldActions;
    private string Click;

    public void HandleClick(Entity entity, Point point, string click = "left")
    {
        Click = click;
        if (entity == null)
            return;

        switch (entity.Id)
        {
            case "Tronco":
                HandleTronco(entity);
                break;

            case "Pedra1" or "Pedra2":
                HandlePedra(entity);
                break;

            case "Wood":
                _worldActions.PickupItem(entity);
                break;

            // objetos do mundo
            case "Bau":
                if (click == "left")
                { }
                else
                {
                    Bau bau = (Bau)entity;

                    if (bau.Aberto)
                        bau.CloseBau();
                    else
                        bau.OpenBau();
                }
                break;
            default:
                HandleIsGround(entity, point);
                break;
        }
    }

    private void HandleTronco(Entity tronco)
    {
        if (_context.State.ActiveEquipe.Id != "AxeTool")
            return;

        if (_context.State.Player.IsAnimated)
            return;

        if (IsEntityFartherThanPlayer(tronco))
            return;

        tronco.Shake();
        tronco.Life -= _context.State.ActiveEquipe.Damage;

        if (tronco.Life > 0)
            return;

        _worldActions.DestroyEntity(tronco);
        _worldActions.DropItem("Wood", 1, tronco.Posicao);
    }

    private void HandlePedra(Entity pedra)
    {
        if (_context.State.ActiveEquipe.Id != "PickaxeTool")
            return;

        if (_context.State.Player.IsAnimated)
            return;

        if (IsEntityFartherThanPlayer(pedra))
            return;
        pedra.Shake();
        pedra.Life -= _context.State.ActiveEquipe.Damage;

        if (pedra.Life > 0)
            return;

        _worldActions.DestroyEntity(pedra);
        _worldActions.DropItem("PedraDrop", 1, pedra.Posicao);
    }

    private void HandleBau(Entity bau) { }
    private void HandleIsGround(Entity ground, Point point)
    {
        if (ground is Soil01)
        {
            _worldActions.ChangeEntity(point, ground, "Soil02");
        }
        else if (ground is Soil02)
        {
            _worldActions.ChangeEntity(point, ground, "Soil03");
        }
    }
    protected bool IsEntityFartherThanPlayer(Entity entity, int maxTiles = 2)
    {
        int tileSize = 16;

        Point playerTile = new(
            _context.State.Player.Hitbox.Center.X / tileSize,
            _context.State.Player.Hitbox.Center.Y / tileSize
        );

        Point entityTile = new(
            entity.Hitbox.Center.X / tileSize,
            entity.Hitbox.Center.Y / tileSize
        );

        int distanceX = Math.Abs(entityTile.X - playerTile.X);
        int distanceY = Math.Abs(entityTile.Y - playerTile.Y);

        return distanceX > maxTiles || distanceY > maxTiles;
    }
}