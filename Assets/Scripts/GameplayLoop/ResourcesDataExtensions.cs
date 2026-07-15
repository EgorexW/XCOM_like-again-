public static class ResourcesDataExtensions{
    public static float RetirementRate(this ResourcesData resourcesData){
        float retiredMembersCount = resourcesData.RetiredMembers.Count;
        float deadMembersCount = resourcesData.DeadMembers.Count;
        return retiredMembersCount / (retiredMembersCount + deadMembersCount);
    }
}