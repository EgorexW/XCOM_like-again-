using UnityEngine;

public static class NameGenerator{
    private static readonly string[] Nicknames = {
        // --- The Drifter's Broadcast soldiers ---
        "Drifter", "Cat", "Failed", "Red Devil", "Meme", "Nightmare", "Ghost", "Arnie", "Headbanger", "Task Manager",
        "Dread Knight", "Santa", "Christmas", "Lady Claus", "Quiet Paw", "JoJo", "Phoenix", "Coffee", "Polar Koala", "Glass Cannon",
        "Bozo", "Dun Cakes", "Big Money", "Nukem", "Poseidon", "Moby", "Arctic", "Deathwing", "Icicle", "Viper Bait",
        "Knockoff Demon", "BFG", "Nitro", "Shooty", "Shield", "Medic", "Abomination", "Ratlord", "Killjoy", "Joker",
        "Hellborn", "Blue Hellborn", "Gravedigger", "Harbinger", "Tank", "Grim", "Demon", "Scourge", "Valkyrie", "Bones",
        "Outrider", "Brute",

        // --- XCOM 2 aliens ---
        "Sectoid", "Viper", "Muton", "Berserker", "Archon", "Andromedon", "Codex", "Avatar", "Gatekeeper", "Faceless",
        "Chryssalid", "Spectre"
    };

    public static string RandomName(){
        return Nicknames[Random.Range(0, Nicknames.Length)];
    }
}