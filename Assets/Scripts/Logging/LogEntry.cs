using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogEntry
{
    private string tag;
    private string message;
    private int shotsFired;
    private int shotsHit;
    private int damageTaken;
    private int grenadesThrown;
    private int grenadesHit;
    private double timeSpentInSeconds;
    private int deathCount;
    private int interactiblesCollected;
    private int meleeAttacks;
    private int difficultyIndex;
    private int triesForThisLevel;

    public LogEntry(string message, string tag, int shotsFired, int shotsHit, int damageTaken, int grenadesThrown, int grenadesHit,
        double timeSpentInSeconds, int deathCount, int interactiblesCollected, int meleeAttacks, int difficultyIndex,
        int triesForThisLevel)
    {
        this.message = message;
        this.tag = tag;
        this.shotsFired = shotsFired;
        this.shotsHit = shotsHit;
        this.damageTaken = damageTaken;
        this.grenadesThrown = grenadesThrown;
        this.grenadesHit = grenadesHit;
        this.timeSpentInSeconds = timeSpentInSeconds;
        this.deathCount = deathCount;
        this.interactiblesCollected = interactiblesCollected;
        this.meleeAttacks = meleeAttacks;
        this.difficultyIndex = difficultyIndex;
        this.triesForThisLevel = triesForThisLevel;
    }

    public override string ToString()
    {
        return message + "," +
               tag + "," +
               shotsFired + "," +
               shotsHit + "," +
               damageTaken + "," +
               grenadesThrown + "," +
               grenadesHit + "," +
               timeSpentInSeconds + "," +
               deathCount + "," +
               interactiblesCollected + "," +
               meleeAttacks + "," +
               difficultyIndex + "," +
               triesForThisLevel;
    }

    public static string HeaderRow()
    {
        return
            "message,tag,shots,shotsHit,damageTaken,grenadesThrown,grenadesHit,timeSpent,deaths,interactiblesCollected,meleeAttacks,difficultyIndex,triesForThisLevel";
    }
}