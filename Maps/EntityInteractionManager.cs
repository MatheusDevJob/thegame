using thegame.Core;
using thegame.Entities;

namespace thegame.Maps;

public class EntityInteractionManager(GameContext context, WorldActionService worldActions)
{
    private readonly GameContext _context = context;
    private readonly WorldActionService _worldActions = worldActions;

    public void HandleClick(Entity entity)
    {
        if (entity == null)
            return;

        switch (entity.Id)
        {
            case "Tronco":
                HandleTronco(entity);
                break;

            case "Pedra":
                HandlePedra(entity);
                break;

            case "Wood":
                _worldActions.PickupItem(entity);
                break;
        }
    }

    private void HandleTronco(Entity tronco)
    {
        if (!_context.State.PlayerHasTool("AxeTool"))
            return;

        if (_context.State.Player.IsAnimated)
            return;

        if (_context.State.ActiveTool == null)
            return;

        _context.State.Player.PlayActionAnimation(10, 8, () =>
        {
            tronco.Life -= _context.State.ActiveTool.Damage;

            if (tronco.Life > 0)
                return;

            _worldActions.DestroyEntity(tronco);
            _worldActions.DropItem("Wood", 1, tronco.Posicao);
        });
    }

    private void HandlePedra(Entity pedra)
    {
        if (!_context.State.PlayerHasTool("PickaxeTool"))
            return;

        if (_context.State.ActiveTool == null)
            return;

        pedra.Life -= _context.State.ActiveTool.Damage;

        if (pedra.Life > 0)
            return;

        _worldActions.DestroyEntity(pedra);
        _worldActions.DropItem("Stone", 1, pedra.Posicao);
    }
}