using System.Collections.Generic;

public interface ILevel
{
    List<CombatObjectSpawn> GetCombatObjectSpawns();
    List<MapNode> GetNodes(MapNodeType type, string group = null);
    List<string> GetAvailableGroups(MapNodeType type);
}
