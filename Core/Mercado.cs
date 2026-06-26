using System.Collections.Generic;
using System.Linq;
using thegame.Entities;

namespace thegame.Core;

public class LojaItens
{
    public string ItemId { get; set; }
    public string ItemCompraId { get; set; }
    public int Quantidade { get; set; }
    public int Preco { get; set; }
}

public class Mercado
{
    public static void Comprar(GameContext context, string ItemId, int Quantidade)
    {
        if (context.State.Inventory.InventarioCheio()) return;

        List<LojaItens> lojaItens = context.UIState.ListProdutosLoja;
        LojaItens item = lojaItens.FirstOrDefault(e => e.ItemId == ItemId);
        if (item == null || item?.Preco == null) return;

        if (context.State.Player.Carteira < item.Preco) return;
        context.State.Player.Carteira -= item.Preco;

        context.State.Inventory.AddItem(item.ItemCompraId, Quantidade);
    }

    public static bool ComprarItem(GameContext context, string ItemId, int Quantidade)
    {
        if (context.State.Inventory.InventarioCheio()) return false;

        Entity entity = EntityFactory.Create(context, new() { Type = ItemId, X = 0, Y = 0 });
        if (entity is Vendivel venda)
        {
            bool removeu = context.State.Inventory.AddItem(ItemId, Quantidade);
            if (removeu)
            {
                context.State.Venda.DinheiroPendente -= venda.Preco;
                return true;
            }
        }
        return false;
    }

    public static bool VenderItem(GameContext context, string ItemId, int Quantidade)
    {
        Entity entity = EntityFactory.Create(context, new() { Type = ItemId, X = 0, Y = 0 });
        if (entity is Vendivel venda)
        {
            bool removeu = context.State.Inventory.RemoveItem(ItemId, Quantidade);
            if (removeu)
            {
                context.State.Venda.DinheiroPendente += venda.Preco;
                return true;
            }
        }
        return false;
    }
}