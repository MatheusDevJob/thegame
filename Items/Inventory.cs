using System.Collections.Generic;
using System.Linq;
using thegame.Core;

namespace thegame.Items;

public class Inventory
{
    public List<ItemStack> Itens { get; private set; } = [];
    public int BagLevel { get; set; } = 1;
    private int LimiteItensBag;
    public int GetLimiteItensBag => LimiteItensBag;

    public void LoadFromSave(List<ItemStackSave> savedItems, int bagLevel)
    {
        Itens.Clear();
        BagLevel = bagLevel;
        UpdateLimiteItens();

        for (int i = 0; i < LimiteItensBag; i++)
        {
            Itens.Add(null);
        }

        if (savedItems == null)
            return;

        for (int i = 0; i < savedItems.Count; i++)
        {
            ItemStackSave item = savedItems[i];
            if (item == null || string.IsNullOrWhiteSpace(item.ItemId) || item.Amount <= 0)
                continue;

            AddItem(item.ItemId, item.ItemId, item.Amount, item.ListIndex);
        }
    }

    public List<ItemStackSave> ToSaveItems()
    {
        return [.. Itens
            .Select((item, index) => new { item, index })
            .Where(x => x.item != null && !string.IsNullOrWhiteSpace(x.item.Id) && x.item.Quantidade > 0)
            .Select(x => new ItemStackSave
            {
                ItemId = x.item.Id,
                Amount = x.item.Quantidade,
                ListIndex = x.index
            })];
    }

    public bool AddItem(string id, string nome, int quantidade, int? indice = null)
    {
        if (string.IsNullOrWhiteSpace(id) || quantidade <= 0)
            return false;

        UpdateLimiteItens();

        indice ??= Itens.FindIndex(item => item == null);

        // se indice for -1 não achou espaço vazio na bag
        if (indice < 0)
            return false;

        ItemStack itemExistente = Itens.FirstOrDefault(i => i?.Id == id);
        if (itemExistente != null)
        {
            itemExistente.Quantidade += quantidade;
            return true;
        }

        Itens[(int)indice] = new ItemStack(id, nome, quantidade);
        return true;
    }

    public bool RemoveItem(string id, int quantidade)
    {
        if (string.IsNullOrWhiteSpace(id) || quantidade <= 0)
            return false;

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

    public void UpdateLimiteItens()
    {
        LimiteItensBag = BagLevel switch
        {
            1 => 8,
            2 => 12,
            3 => 16,
            4 => 20,
            5 => 24,
            _ => 8
        };
    }

    public int PossuiItem(string ItemId)
    {
        return Itens.FindIndex(item => item?.Id == ItemId);
    }
}