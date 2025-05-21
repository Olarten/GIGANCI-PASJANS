using Spectre.Console;

namespace Pasjans
{
    public static class CardMoving //Klasa statyczna, która odpowiada za ruch kart
    {
        private static readonly string[] values = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" }; //Lista wartości kart

        public static List<Card> GetColumnByIndex(int index) //Zwraca kolumnę na podstawie indeksu
        {
            return index switch
            {
                0 => Program.column1,
                1 => Program.column2,
                2 => Program.column3,
                3 => Program.column4,
                4 => Program.column5,
                5 => Program.column6,
                6 => Program.column7,
                _ => new List<Card>()
            };
        }

        public static bool TryMoveCards(int sourceIndex, int targetIndex) //Funkcja sprawdzająca czy można przenieść karty
        {
            var sourceColumn = GetColumnByIndex(sourceIndex); //Zmienne służace do zaznaczania dwóch kolumn
            var targetColumn = GetColumnByIndex(targetIndex);

            if (sourceColumn.Count == 0) return false;

            var cardsToMove = new List<Card>();
            
            // Find all face-up cards
            var faceUpCards = new List<Card>();
            int startIndex = sourceColumn.Count - 1;
            while (startIndex >= 0 && !sourceColumn[startIndex].IsReversed)
            {
                faceUpCards.Insert(0, sourceColumn[startIndex]);
                startIndex--;
            }

            // No face-up cards to move
            if (faceUpCards.Count == 0) return false;

            // Find the starting position for the move
            int moveStartIndex = -1;
            for (int i = 0; i < faceUpCards.Count; i++)
            {
                if (targetColumn.Count == 0)
                {
                    if (faceUpCards[i].Value == "K")
                    {
                        moveStartIndex = i;
                        break;
                    }
                }
                else if (CanStackCards(faceUpCards[i], targetColumn[^1]))
                {
                    moveStartIndex = i;
                    break;
                }
            }

            if (moveStartIndex == -1) return false;

            // Get all cards from the valid starting position
            cardsToMove.AddRange(faceUpCards.Skip(moveStartIndex));

            // Move the cards
            targetColumn.AddRange(cardsToMove);
            sourceColumn.RemoveRange(sourceColumn.Count - cardsToMove.Count, cardsToMove.Count);
            if (sourceColumn.Count > 0)
                sourceColumn[^1].IsReversed = false;
            return true;
        }

        public static bool TryMoveFromDeckToColumn(int targetIndex, List<Card> SelectedDrawnDeck) //Funkcja sprawdzająca czy można przenieść karty z talii do kolumny
        {
            if (SelectedDrawnDeck.Count == 0) return false;

            var targetColumn = GetColumnByIndex(targetIndex);
            var cardToMove = SelectedDrawnDeck[^1];

            if (targetColumn.Count == 0)
            {
                if (cardToMove.Value == "K")
                {
                    targetColumn.Add(cardToMove);
                    SelectedDrawnDeck.RemoveAt(SelectedDrawnDeck.Count - 1);
                    cardToMove.IsReversed = false;
                    return true;
                }
                return false;
            }

            var targetCard = targetColumn[^1];
            
            if (CanStackCards(cardToMove, targetCard))
            {
                targetColumn.Add(cardToMove);
                SelectedDrawnDeck.RemoveAt(SelectedDrawnDeck.Count - 1);
                return true;
            }

            return false;
        }

        public static bool TryMoveToFoundation(Card card, List<Card> foundation1, List<Card> foundation2, List<Card> foundation3, List<Card> foundation4) //Funkcja sprawdzająca czy można przenieść karty na podstawy
        {
            var targetFoundation = card.Suite switch
            {
                "♥" => foundation1,
                "♦" => foundation2,
                "♣" => foundation3,
                "♠" => foundation4,
                _ => null
            };

            if (targetFoundation == null) return false;

            if (targetFoundation.Count == 0)
            {
                if (card.Value == "A") 
                {
                    targetFoundation.Add(card);
                    card.IsReversed = false;
                    return true;
                }
                return false;
            }

            var topCard = targetFoundation[^1];
            var topCardIndex = Array.IndexOf(values, topCard.Value);
            var newCardIndex = Array.IndexOf(values, card.Value);

            if (newCardIndex == topCardIndex + 1)
            {
                targetFoundation.Add(card);
                card.IsReversed = false;
                return true;
            }

            return false;
        }

        private static bool CanStackCards(Card cardToMove, Card targetCard) //Funkcja sprawdzająca czy można przenieść kart na kolumny
        {
            bool isDifferentColor = (cardToMove.Color == "red" && targetCard.Color == "blue") ||
                                   (cardToMove.Color == "blue" && targetCard.Color == "red");
            
            var targetIndex = Array.IndexOf(values, targetCard.Value);
            var moveIndex = Array.IndexOf(values, cardToMove.Value);
            
            return isDifferentColor && moveIndex == targetIndex - 1;
        }
    }
}