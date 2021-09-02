using System;

namespace ExcelToGraph
{
    public class CharacterProperty
    {
        public string name;
        public Position position;
        public Emotion emotion;


        public CharacterProperty(string name, Position position, Emotion emotion)
        {
            this.name = name;
            this.position = position;
            this.emotion = emotion;
        }
    }

    public enum Emotion
    {
        Usual,
        Happy,
        Sad,
        Lust,
        Severe,
        Surprised,
    }
    public enum Direction
    {
        FromLeft,
        FromRight,
        FromButtom,
        FromTop,
    }
    public enum Position
    {
        Left,
        Middle,
        Right,
    }
}
