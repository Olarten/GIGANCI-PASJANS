using System.Drawing;
using Spectre.Console;

namespace Pasjans
{
    public class Card //Klasa karta, która zawiera kolor, wartość i informację o odwróceniu
    {
        public string Value { get; private set; }
        public string Suite { get; private set; }
        public string Color;
        public bool IsReversed { get; set; }

        public Card(string value, string suite, bool isReversed = true) //Konstruktor klasy 
        {
            Color = suite == "♥" || suite == "♦" ? "red" : "blue";
            Value = value;
            Suite = suite;
            IsReversed = isReversed;
        }

        public override string ToString() //Zamienia obiekt karta, na funkcjonującego stringa
        {
            if(!IsReversed)
            {
            return $"[{Color}]{Value} {Suite}[/]";
            }else
            {
            return "[grey]🃏 [/]";

            }
        }

    }

    public static class Cards //Klasa statyczna, która generuje talię kart
    {
        private static readonly string[] Values = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
        private static readonly string[] Suits = { "♥", "♦", "♣", "♠" };

        public static List<Card> GetDeck()
        {
            var deck = new List<Card>();

            foreach (var suit in Suits)
            {
                foreach (var value in Values)
                {
                    deck.Add(new Card(value, suit));
                }
            }

            return deck;
        }

        
    }

}