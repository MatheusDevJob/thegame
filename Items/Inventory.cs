using System.Collections.Generic;
using System.Linq;
namespace thegame.Items;

public class Inventory
{
    public List<ItemStack> Itens { get; private set; } = [];

    public void AddItem(string id, string nome, int quantidade)
    {
        ItemStack itemExistente = Itens.FirstOrDefault(i => i.Id == id);

        if (itemExistente != null)
        {
            itemExistente.Quantidade += quantidade;
            return;
        }

        Itens.Add(new ItemStack(id, nome, quantidade));
    }

    public bool RemoveItem(string id, int quantidade)
    {
        ItemStack item = Itens.FirstOrDefault(i => i.Id == id);

        if (item == null || item.Quantidade < quantidade)
            return false;

        item.Quantidade -= quantidade;

        if (item.Quantidade <= 0)
            Itens.Remove(item);

        return true;
    }

    public int GetQuantidade(string id)
    {
        ItemStack item = Itens.FirstOrDefault(i => i.Id == id);
        return item?.Quantidade ?? 0;
    }
}