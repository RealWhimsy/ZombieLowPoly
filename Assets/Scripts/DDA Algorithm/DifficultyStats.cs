

using System.Collections.Generic;

public static class DifficultyStats
{
    public static Dictionary<string, Difficulty> difficulties = new Dictionary<string, Difficulty>()
    {
        [Const.Difficulties.Dif1] = new Difficulty(1, 5, 60, 10, 2.0, 2.0),
        [Const.Difficulties.Dif2] = new Difficulty(2, 7, 60, 13, 2.0, 2.0),
        [Const.Difficulties.Dif3] = new Difficulty(3, 9, 65, 15, 2.5, 2.0),
        [Const.Difficulties.Dif4] = new Difficulty(4, 12, 65, 17, 2.5, 2.0),
        [Const.Difficulties.Dif5] = new Difficulty(5, 15, 100, 20, 3.0, 2.0), // middle values
        [Const.Difficulties.Dif6] = new Difficulty(6, 17, 100, 23, 3.0, 2.0), // middle values
        [Const.Difficulties.Dif7] = new Difficulty(7, 19, 125, 25, 3.5, 2.0),
        [Const.Difficulties.Dif8] = new Difficulty(8, 21, 125, 26, 3.5, 2.0),
        [Const.Difficulties.Dif9] = new Difficulty(9, 23, 150, 27, 4.0, 2.0),
        [Const.Difficulties.Dif10] = new Difficulty(10, 25, 150, 30, 4.0, 2.0)
    };
}