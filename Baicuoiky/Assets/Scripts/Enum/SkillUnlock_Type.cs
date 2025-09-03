using UnityEngine;

public enum SkillUnlock_Type
{
    None,
    // ----- Dash Tree ------
    Dash,
    Dash_CloneOnStart, // Create a clone when dash starts
    Dash_CloneOnStartAndArrival, // Create a clone when dash starts and ends
    Dash_ShardOnStart, // Create a shard when dash starts
    Dash_ShardOnStartAndArrival, // Create a shard when dash starts and ends
    // ------ Shard Tree ------
    Shard, // The shard explodes when touched by an enemy or time goes up
    Shard_MoveToEnemy, // Shard will move towards nearest enemy
    Shard_MultiCast, // Shard ability can have up to 1W charges. You can cast them all in a row
    Shard_Teleport, // You can swap places with the last shard you created
    Shard_TeleportHPRewind // When you swap places with shard, your HP % is same as it was when you created shard.

}
