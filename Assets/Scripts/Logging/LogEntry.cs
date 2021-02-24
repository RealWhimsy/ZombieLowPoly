using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogEntry
{
    private string tag;
    private int shotsFired;
    private int shotsHit;
    private int damageTaken;
    private int grenadesThrown;
    private int grenadesHit;
    private double timeSpentInSeconds;
    private int deathCount;
    private int interactiblesCollected;
    private int meleeAttacks;

    public LogEntry(string tag, int shotsFired, int shotsHit, int damageTaken, int grenadesThrown, int grenadesHit, double timeSpentInSeconds, int deathCount, int interactiblesCollected, int meleeAttacks)
    {
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
    }

    public override string ToString()
    {
        return tag + "," +
               shotsFired + "," +
               shotsHit + "," +
               damageTaken + "," +
               grenadesThrown + "," +
               grenadesHit + "," +
               timeSpentInSeconds + "," +
               deathCount + "," +
               interactiblesCollected + "," +
               meleeAttacks;
    }

    public static string HeaderRow()
    {
        return "tag,shots,shotsHit,damageTaken,grenadesThrown,grenadesHit,timeSpent,deaths,interactiblesCollected,meleeAttacks";
    }
}
