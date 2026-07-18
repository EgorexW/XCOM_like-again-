using UnityEngine;

public static class NameGenerator{
    private static readonly string[] FirstNames = {
        "James", "John", "Robert", "Michael", "William", "David", "Richard", "Joseph", "Thomas", "Charles",
        "Mary", "Patricia", "Jennifer", "Linda", "Elizabeth", "Barbara", "Susan", "Jessica", "Sarah", "Karen",
        "Alex", "Sam", "Jordan", "Taylor", "Morgan", "Casey", "Riley", "Jamie", "Avery", "Peyton",
        "Elena", "Ivan", "Yuki", "Aisha", "Omar", "Chen", "Mei", "Lars", "Ingrid", "Diego"
    };

    private static readonly string[] LastNames = {
        "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
        "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin",
        "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson",
        "Ivanov", "Smirnov", "Sato", "Suzuki", "Kim", "Park", "Wong", "Chen", "Müller", "Schmidt"
    };

    private static readonly string[] Nicknames = {
        // --- The Originals ---
        "Viper", "Ghost", "Maverick", "Shadow", "Reaper", "Tank", "Doc", "Jester", "Rookie", "Boomer",
        "Snipe", "Eagle", "Hawk", "Wolf", "Bear", "Fox", "Sparky", "Grizzly", "Raven", "Ice",
        "Cobra", "Psycho", "Bulldog", "Whiskey", "Tango", "Echo", "Zero", "Nomad", "Hunter", "Chief",

        // --- Tactical & Military ---
        "Bravo", "Alpha", "Kilo", "Hazard", "Specter", "Phantom", "Ranger", "Striker", "Vanguard", "Sentinel",
        "Aegis", "Bastion", "Garrison", "Raider", "Recon", "Outlaw", "Gunslinger", "Trigger", "Breacher", "Reload",

        // --- Weather & Elements ---
        "Frost", "Blaze", "Cinder", "Pyro", "Vortex", "Cyclone", "Typhoon", "Avalanche", "Thunder", "Bolt",
        "Storm", "Gale", "Nova", "Cosmos", "Solar", "Eclipse", "Zenith", "Apex", "Summit", "Ridge",

        // --- Fauna & Predators ---
        "Jaguar", "Panther", "Cougar", "Raptor", "Falcon", "Badger", "Wolverine", "Vulture", "Hyena", "Python",
        "Mamba", "Scorpion", "Spider", "Hornet", "Wasp", "Razorback", "Mammoth", "Titan", "Colossus", "Goliath",

        // --- Mythological & Sci-Fi ---
        "Odin", "Thor", "Zeus", "Hades", "Ares", "Apollo", "Phoenix", "Siren", "Valkyrie", "Medusa",
        "Kraken", "Hydra", "Chimera", "Gorgon", "Titan", "Atlas", "Chronos", "Orion", "Sirius", "Vega",

        // --- Dark & Edgy ---
        "Bane", "Grim", "Dirge", "Crypt", "Wraith", "Specter", "Malice", "Rogue", "Slayer", "Executioner"
    };

    const float NICKNAME_CHANCE = 0.3f;
    public static string RandomName(){
        // string firstName = FirstNames[Random.Range(0, FirstNames.Length)];
        // string lastName = LastNames[Random.Range(0, LastNames.Length)];
        //
        // // 30% chance for the soldier to have a callsign/nickname
        // if (Random.value < NICKNAME_CHANCE) {
        //     string nickname = Nicknames[Random.Range(0, Nicknames.Length)];
        //     return $"{firstName} '{nickname}' {lastName}";
        // }
        //
        // return $"{firstName} {lastName}";
        return Nicknames[Random.Range(0, Nicknames.Length)];
    }
}