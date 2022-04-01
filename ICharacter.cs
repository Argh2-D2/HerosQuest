using System;
using System.Collections.Generic;

namespace HerosQuest
{
    //Inheritance
    public interface ICharacter : ICharacterBase
    {
        int AttackMiss { get; }
        int AttackGraze { get; }
        int AttackHit { get; }

        ConsoleColor TextColor();
    }
}