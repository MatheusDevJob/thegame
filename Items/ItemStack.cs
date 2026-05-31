namespace thegame.Items;

public class ItemStack(string id, string nome, int quantidade, int quantidadeMaxima = 99)
{
    public string Id { get; set; } = id;
    public string Nome { get; set; } = nome;
    public int Quantidade { get; set; } = quantidade;
    public int QuantidadeMaxima { get; set; } = quantidadeMaxima;
}