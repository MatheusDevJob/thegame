using System;
using Microsoft.Xna.Framework;
using thegame.Core;
using thegame.Entities;
using thegame.Entities.Tools;
using thegame.Entities.WorldObjects.Interactables;
using thegame.Entities.WorldObjects.Solo;
using thegame.Entities.WorldObjects.Solo.Plantacoes;

namespace thegame.Maps;

public class EntityInteractionManager(GameContext context, WorldActionService worldActions)
{
    private string Click;

    public void HandleClick(Entity entity, Point point, string click = "left")
    {
        Click = click;
        if (entity == null)
            return;

        if (entity is Plantacao p)
        {
            HandlePlantacao(p, point);
            return;
        }

        switch (entity.Id)
        {
            case "Tronco":
                HandleTronco(entity);
                break;

            case "Pedra1" or "Pedra2":
                HandlePedra(entity);
                break;

            case "Wood":
                worldActions.PickupItem(entity);
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
            case "Soil":
                Entity ItemNaMao = context.State.ActiveEquipe;
                if (ItemNaMao is WaterTool)
                {
                    CampoArado campo = (CampoArado)entity;
                    campo.Molhar();
                }
                else if (ItemNaMao is Plantacao plantacao)
                {
                    Farming.Plantar(context, point, plantacao, entity);
                }
                break;
            default:
                HandleIsGround(entity, point);
                break;
        }
    }

    private void HandlePlantacao(Plantacao plantacao, Point point)
    {

        if (context.State.Player.IsAnimated)
            return;

        if (IsEntityFartherThanPlayer(plantacao))
            return;

        if (plantacao.IsColhivel)
        {
            worldActions.DestroyEntity(plantacao);
            worldActions.DropItem(plantacao.DropItemId, plantacao.DropItemQtd, plantacao.Posicao);
        }
    }
    private void HandleTronco(Entity tronco)
    {
        if (context.State.ActiveEquipe.Id != "AxeTool")
            return;

        if (context.State.Player.IsAnimated)
            return;

        if (IsEntityFartherThanPlayer(tronco))
            return;

        tronco.Shake();
        tronco.Life -= context.State.ActiveEquipe.Damage;

        if (tronco.Life > 0)
            return;

        worldActions.DestroyEntity(tronco);
        worldActions.DropItem("Wood", 1, tronco.Posicao);
    }

    private void HandlePedra(Entity pedra)
    {
        if (context.State.ActiveEquipe.Id != "PickaxeTool")
            return;

        if (context.State.Player.IsAnimated)
            return;

        if (IsEntityFartherThanPlayer(pedra))
            return;
        pedra.Shake();
        pedra.Life -= context.State.ActiveEquipe.Damage;

        if (pedra.Life > 0)
            return;

        worldActions.DestroyEntity(pedra);
        worldActions.DropItem("PedraDrop", 1, pedra.Posicao);
    }

    private void HandleBau(Entity bau) { }
    private void HandleIsGround(Entity ground, Point point)
    {
        if (context.State.ActiveEquipe.Id != "ShovelTool") return;

        if (ground is Soil01)
        {
            worldActions.ChangeEntity(point, ground, "Soil02");
        }
        else if (ground is Soil02)
        {
            worldActions.ChangeEntity(point, ground, "Soil03");
        }
    }
    protected bool IsEntityFartherThanPlayer(Entity entity, int maxTiles = 2)
    {
        int tileSize = 16;

        Point playerTile = new(
            context.State.Player.Hitbox.Center.X / tileSize,
            context.State.Player.Hitbox.Center.Y / tileSize
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