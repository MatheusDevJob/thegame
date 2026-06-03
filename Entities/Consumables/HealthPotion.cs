// using System.Numerics;
// using thegame.Core;

// namespace thegame.Entities.Consumables;

// public class HealthPotion : EquipableEntity
// {
//     public HealthPotion(GameContext context, Vector2 position) : base(context, "Items/health_potion", position)
//     {
//         Id = "HealthPotion";
//     }

//     public override void Use(Player player)
//     {
//         Life += 25;
//         Context.State.Inventory.RemoveItem(Id, 1);
//     }
// }