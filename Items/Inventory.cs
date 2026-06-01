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

        if (savedItems == null)
            return;

        foreach (ItemStackSave item in savedItems)
        {
            if (item == null || string.IsNullOrWhiteSpace(item.ItemId) || item.Amount <= 0)
                continue;

            AddItem(item.ItemId, item.ItemId, item.Amount);
        }
    }

    public List<ItemStackSave> ToSaveItems()
    {
        return Itens
            .Where(item => item != null && !string.IsNullOrWhiteSpace(item.Id) && item.Quantidade > 0)
            .Select(item => new ItemStackSave
            {
                ItemId = item.Id,
                Amount = item.Quantidade
            })
            .ToList();
    }

    public void AddItem(string id, string nome, int quantidade)
    {
        if (string.IsNullOrWhiteSpace(id) || quantidade <= 0)
            return;

        UpdateLimiteItens();

        ItemStack itemExistente = Itens.FirstOrDefault(i => i.Id == id);

        if (itemExistente != null)
        {
            itemExistente.Quantidade += quantidade;
            return;
        }

        if (Itens.Count >= LimiteItensBag)
            return;

        Itens.Add(new ItemStack(id, nome, quantidade));
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
            _ => 24
        };
    }
}