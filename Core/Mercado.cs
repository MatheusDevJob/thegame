using System.Collections.Generic;
using System.Linq;

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

        int carteira = context.State.Player.Carteira;
        if (carteira < item.Preco) return;
        carteira -= item.Preco;

        context.State.Inventory.AddItem(item.ItemCompraId, "", Quantidade);
    }
}