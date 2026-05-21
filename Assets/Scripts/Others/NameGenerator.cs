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
        "Viper", "Ghost", "Maverick", "Shadow", "Reaper", "Tank", "Doc", "Jester", "Rookie", "Boomer",
        "Snipe", "Eagle", "Hawk", "Wolf", "Bear", "Fox", "Sparky", "Grizzly", "Raven", "Ice",
        "Cobra", "Psycho", "Bulldog", "Whiskey", "Tango", "Echo", "Zero", "Nomad", "Hunter", "Chief"
    };

    const float NICKNAME_CHANCE = 0.3f;
    public static string RandomName(){
        string firstName = FirstNames[Random.Range(0, FirstNames.Length)];
        string lastName = LastNames[Random.Range(0, LastNames.Length)];
        
        // 30% chance for the soldier to have a callsign/nickname
        if (Random.value < NICKNAME_CHANCE) {
            string nickname = Nicknames[Random.Range(0, Nicknames.Length)];
            return $"{firstName} '{nickname}' {lastName}";
        }
        
        return $"{firstName} {lastName}";
    }
}