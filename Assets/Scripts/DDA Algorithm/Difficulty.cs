
using System;

public class Difficulty
{
    private int zombiesForWave;
    private int zombieHealth;
    private int zombieDamage;
    private double zombieSpeed;
    private double timeBetweenSpawns;
    private int stage;

    public Difficulty(int stage, int zombiesForWave, int zombieHealth, int zombieDamage, double zombieSpeed, double timeBetweenSpawns)
    {
        this.stage = stage;
        this.zombiesForWave = zombiesForWave;
        this.zombieHealth = zombieHealth;
        this.zombieDamage = zombieDamage;
        this.zombieSpeed = zombieSpeed;
        this.timeBetweenSpawns = timeBetweenSpawns;
    }

    public int Stage
    {
        get => stage;
        set => stage = value;
    }
}