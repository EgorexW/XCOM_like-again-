public static class StateExtensions{
    public static float RetirementRate(this MembersState membersState){
        float retiredMembersCount = membersState.RetiredMembers.Count;
        float deadMembersCount = membersState.DeadMembers.Count;
        return retiredMembersCount / (retiredMembersCount + deadMembersCount);
    }
}