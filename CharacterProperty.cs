using System;
using System.Collections.Generic;

namespace ExcelToGraph
{
    public class CharacterProperty
    {
        private readonly Dictionary<string, Emotion> emotionStringEnum = new Dictionary<string, Emotion>()
        {
            {"стандарт", Emotion.Usual},
            {"радость", Emotion.Happy},
            {"суровость", Emotion.Severe},
            {"удивление", Emotion.Surprised},
            {"грусть", Emotion.Sad},
            {"похоть", Emotion.Lust},
        };

        public string name;
        public Position position;
        public Emotion emotion;


        public CharacterProperty(string name, Position position, Emotion emotion)
        {
            this.name = name;
            this.position = position;
            this.emotion = emotion;
        }
        public CharacterProperty(string name, string positionName, string emotionName)
        {
            this.name = name;
            this.position = ConvertToPosition(positionName);
            this.emotion = ConvertToEmotion(emotionName);
        }


        private Position ConvertToPosition(string positionName)
        {
            switch (positionName)
            {
                case "слева":
                    {
                        return Position.Left;
                    }
                case "центр":
                    {
                        return Position.Middle;
                    }
                case "справа":
                    {
                        return Position.Right;
                    }
                default:
                    {
                        throw new Exception("Cannot convert postion from string to enum");
                    }
            }
        }
        private Emotion ConvertToEmotion(string emotionName)
        {
            emotionName = emotionName.ToLower();

            foreach (var emotion in emotionStringEnum)
            {
                if (emotionName.Contains(emotion.Key))
                {
                    return emotion.Value;
                }
            }

            return Emotion.Usual;//throw new Exception("Cannot convert emotion from string to enum");
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
