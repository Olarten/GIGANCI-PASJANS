//Niektóre informacje dla jasności;
//Do niektórych bibliotek spytałem się sztucznej inteligencji, jak działają oraz aby podała ich zastosowania w pasjansie
//Niektóre moje notatki da się znaleźć na dole kodu.

using System;
using Microsoft.VisualBasic;
using Spectre.Console;

//Główny plik programu zawierający logikę gry i obsługę interfejsu użytkownika
namespace Pasjans
{
    // Główna klasa programu zarządzająca stanem gry i interakcją z użytkownikiem
    class Program
    {
        // Zmienne kontrolujące stan gry
        static int deckRepeatCount = 0; // Licznik przetasowań talii
        public static int currentColumn = 0; // Aktualnie wybrana kolumna
        static int totalColumns = 9;  // Całkowita liczba kolumn (7 na karty + talia + podstawy)

        // Deklaracje list przechowujących karty
        public static List<Card> deck = Cards.GetDeck(); // Główna talia 52 kart
        static List<Card> SelectedDrawnDeck = new List<Card>(); // Karty dobrane z talii
        static List<Card> selectedCards = new List<Card>(); // Tymczasowa lista wybranych kart
        
        // Kolumny na stole
        public static List<Card> column1 = new List<Card>(); 
        public static List<Card> column2 = new List<Card>(); 
        public static List<Card> column3 = new List<Card>(); 
        public static List<Card> column4 = new List<Card>(); 
        public static List<Card> column5 = new List<Card>(); 
        public static List<Card> column6 = new List<Card>();
        public static List<Card> column7 = new List<Card>();

        // Podstawy do układania kart według kolorów
        static List<Card> foundation1 = new List<Card>(); // Podstawa na kier (♥)
        static List<Card> foundation2 = new List<Card>(); // Podstawa na karo (♦)
        static List<Card> foundation3 = new List<Card>(); // Podstawa na trefl (♣)
        static List<Card> foundation4 = new List<Card>(); // Podstawa na pik (♠)

        // Zmienne do obsługi wyboru kolumn
        public static int firstSelectedColumn = -1; // Indeks pierwszej wybranej kolumny
        public static bool isFirstSelection = true; // Flaga pierwszego wyboru
        // Główna pętla gry
        static void Main(string[] args)
        {
            // Inicjalizacja planszy
            var plansza = new Plansza(); // Utworzenie planszy gry
            Console.CursorVisible = false; // Wyłączenie kursora konsoli

            // Inicjalizacja początkowego stanu gry
            PrintTitle();             // Wyświetlenie tytułu
            DrawFoundations();        // Wyświetlenie podstaw
            
            // Rozdanie początkowych kart do kolumn
            column1 = chooseCards(deck, 1);       
            column2 = chooseCards(deck, 2);
            column3 = chooseCards(deck, 3);
            column4 = chooseCards(deck, 4);
            column5 = chooseCards(deck, 5);
            column6 = chooseCards(deck, 6);
            column7 = chooseCards(deck, 7);

            // Odkrycie ostatnich kart w każdej kolumnie
            if (column1.Count > 0) column1[^1].IsReversed = false;
            if (column2.Count > 0) column2[^1].IsReversed = false;
            if (column3.Count > 0) column3[^1].IsReversed = false;
            if (column4.Count > 0) column4[^1].IsReversed = false;
            if (column5.Count > 0) column5[^1].IsReversed = false;
            if (column6.Count > 0) column6[^1].IsReversed = false;
            if (column7.Count > 0) column7[^1].IsReversed = false;

            bool running = true;
            while (running)
            {
                plansza.DrawTable(selectedCards);              // Rysowanie planszy

                // Key input
                var key = Console.ReadKey(true);

                // Obsługa klawiszy
                if (key.Key == ConsoleKey.LeftArrow)
                {
                    if (currentColumn > 0)
                        currentColumn--;
                    else
                        currentColumn = totalColumns - 1; // Powrót na koniec
                }

                // Strzałka w prawo
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    if (currentColumn < totalColumns - 1)
                        currentColumn++;
                    else
                        currentColumn = 0; // Powrót na początek
                }

                // Wyjście z gry
                else if (key.Key == ConsoleKey.Escape)
                {
                    AnsiConsole.MarkupLine("\n[red]Zamykanie gry...[/]");
                    running = false;
                }

                // Potwierdzenie wyboru
                else if (key.Key == ConsoleKey.Enter && currentColumn != 7 && currentColumn != 8)
                {
                    if (isFirstSelection)
                    {
                        firstSelectedColumn = currentColumn;
                        AnsiConsole.MarkupLine($"\n[green]Wybrano kolumnę {currentColumn + 1}![/]");
                        AnsiConsole.MarkupLine("[grey]Wybierz drugą kolumnę[/]");
                        isFirstSelection = false;
                        AnsiConsole.MarkupLine("[grey](Naciśnij dowolny klawisz, aby kontynuować)[/]");
                       
                    }
                    else 
                    {
                        bool moved = CardMoving.TryMoveCards(firstSelectedColumn, currentColumn);
                        plansza.DrawTable(selectedCards); // Redraw immediately after move
                        if (moved)
                        {
                            AnsiConsole.MarkupLine("\n[green]Udało się przełożyć karty![/]");
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("\n[red]Ten ruch jest niemożliwy![/]");
                            AnsiConsole.MarkupLine("[grey](Naciśnij dowolny klawisz, aby kontynuować)[/]");
                            Console.ReadKey(true);
                        }

                        isFirstSelection = true;
                    }
                }
                else if (key.Key == ConsoleKey.Enter && currentColumn == 7)
                {
                    if (isFirstSelection)
                    {
                        firstSelectedColumn = 7;
                        isFirstSelection = false;
                    }

                    var key2 = Console.ReadKey(true);
                    bool cancelled = false;
                    while (key2.Key != ConsoleKey.Enter)
                    {
                        if (key2.Key == ConsoleKey.LeftArrow || key2.Key == ConsoleKey.RightArrow)
                        {
                            if (key2.Key == ConsoleKey.LeftArrow)
                            {
                                if (currentColumn > 0)
                                    currentColumn--;
                                else
                                    currentColumn = totalColumns - 1;
                            }
                            else
                            {
                                if (currentColumn < totalColumns - 1)
                                    currentColumn++;
                                else
                                    currentColumn = 0;
                            }
                            plansza.DrawTable(selectedCards);
                        }
                        else if (key2.Key == ConsoleKey.Escape)
                        {
                            // Anuluj wybór i wyjdź z pętli
                            isFirstSelection = true;
                            firstSelectedColumn = -1;
                            cancelled = true;
                            break;
                        }
                        key2 = Console.ReadKey(true);
                    }

                    if (cancelled)
                        continue;

                    if (currentColumn == 8)
                    {
                        bool moved = CardMoving.TryMoveToFoundation(SelectedDrawnDeck[^1], foundation1, foundation2, foundation3, foundation4);
                        if (moved)
                        {
                            SelectedDrawnDeck.RemoveAt(SelectedDrawnDeck.Count - 1);
                            plansza.DrawTable(selectedCards); 
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("\n[red]Ten ruch jest niemożliwy![/]");
                        }
                    }
                    else if (currentColumn < 7)
                    {
                        bool moved = CardMoving.TryMoveFromDeckToColumn(currentColumn, SelectedDrawnDeck);
                        plansza.DrawTable(selectedCards);
                        if (moved)
                        {
                            AnsiConsole.MarkupLine("\n[green]Udało się przełożyć kartę![/]");
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("\n[red]Ten ruch jest niemożliwy![/]");
                            AnsiConsole.MarkupLine("[grey](Naciśnij dowolny klawisz, aby kontynuować)[/]");
                            Console.ReadKey(true);
                        }
                    }

                    
                    isFirstSelection = true;
                    firstSelectedColumn = -1;
                }
                else if (key.Key == ConsoleKey.Enter && currentColumn == 8)
                {
                    if (!isFirstSelection)
                    {
                        List<Card> sourceColumn = CardMoving.GetColumnByIndex(firstSelectedColumn);
                        if (sourceColumn != null && sourceColumn.Count > 0)
                        {
                            Card cardToMove = sourceColumn[^1];
                            bool moved = CardMoving.TryMoveToFoundation(cardToMove, foundation1, foundation2, foundation3, foundation4);
                            if (moved)
                            {
                                sourceColumn.RemoveAt(sourceColumn.Count - 1);
                                if (sourceColumn.Count > 0)
                                {
                                    sourceColumn[^1].IsReversed = false;
                                }
                                plansza.DrawTable(selectedCards);
                            }
                            else
                            {
                                AnsiConsole.MarkupLine("\n[red]Ten ruch jest niemożliwy![/]");
                                AnsiConsole.MarkupLine("[grey](Naciśnij dowolny klawisz, aby kontynuować)[/]");
                                Console.ReadKey(true);
                            }
                        }
                        isFirstSelection = true; 
                        currentColumn = firstSelectedColumn;
                        firstSelectedColumn = -1; 
                    }

                }
                else if (key.Key == ConsoleKey.D)
                {
                    if (deck.Count > 0 && deckRepeatCount == 0)
                    {
                        var random = new Random();
                        int randomIndex = random.Next(deck.Count);
                        var drawnCard = deck[randomIndex];
                        drawnCard.IsReversed = false;
                        deck.RemoveAt(randomIndex);
                        SelectedDrawnDeck.Add(drawnCard);
                        plansza.DrawTable(selectedCards); // Natychmiastowe odświeżenie planszy

                    }
                    else if (deck.Count > 0 && deckRepeatCount >= 1)
                    {
                        SelectedDrawnDeck.Add(deck[0]);
                        deck.RemoveAt(0);
                        plansza.DrawTable(selectedCards); // Natychmiastowe odświeżenie planszy
                       
                    }
                    else
                    {
                        deckRepeatCount = +1;
                        for (int i = 0; i < SelectedDrawnDeck.Count; i++)
                        {
                            deck.Add(SelectedDrawnDeck[i]);
                        }
                        SelectedDrawnDeck = new List<Card>();
                        plansza.DrawTable(selectedCards); // Natychmiastowe odświeżenie planszy po przetasowaniu
                    }
                }
                else if (key.Key == ConsoleKey.R)
                {
                    AnsiConsole.MarkupLine("\n[yellow]Resetowanie gry...[/]");
                    AnsiConsole.MarkupLine("[grey](Naciśnij dowolny klawisz, aby kontynuować)[/]");
                    Console.ReadKey(true);
                    ResetGame();
                }
                else
                {
                    AnsiConsole.MarkupLine("\n[red]Nieznany klawisz![/]");
                    AnsiConsole.MarkupLine("[grey](Naciśnij dowolny klawisz, aby kontynuować)[/]");
                    Console.ReadKey(true);
                }
                Console.CursorVisible = true; // Przywrócenie kursora
            }
        }

        // Metoda resetująca grę do stanu początkowego
        private static void ResetGame()
        {
            // Resetowanie zmiennych stanu gry
            deckRepeatCount = 0;
            currentColumn = 0;
            firstSelectedColumn = -1;
            isFirstSelection = true;

            // Czyszczenie wszystkich kolekcji
            deck = Cards.GetDeck();
            SelectedDrawnDeck = new List<Card>();
            selectedCards = new List<Card>();
            column1 = new List<Card>();
            column2 = new List<Card>();
            column3 = new List<Card>();
            column4 = new List<Card>();
            column5 = new List<Card>();
            column6 = new List<Card>();
            column7 = new List<Card>();
            foundation1 = new List<Card>();
            foundation2 = new List<Card>();
            foundation3 = new List<Card>();
            foundation4 = new List<Card>();

            // Rozdanie nowych kart
            column1 = chooseCards(deck, 1);
            column2 = chooseCards(deck, 2);
            column3 = chooseCards(deck, 3);
            column4 = chooseCards(deck, 4);
            column5 = chooseCards(deck, 5);
            column6 = chooseCards(deck, 6);
            column7 = chooseCards(deck, 7);

            // Odkrycie ostatnich kart
            if (column1.Count > 0) column1[^1].IsReversed = false;
            if (column2.Count > 0) column2[^1].IsReversed = false;
            if (column3.Count > 0) column3[^1].IsReversed = false;
            if (column4.Count > 0) column4[^1].IsReversed = false;
            if (column5.Count > 0) column5[^1].IsReversed = false;
            if (column6.Count > 0) column6[^1].IsReversed = false;
            if (column7.Count > 0) column7[^1].IsReversed = false;

            // Czyszczenie ekranu i ponowne rysowanie
            AnsiConsole.Clear();
            PrintTitle();
            DrawFoundations();
        }

        // Metoda wyświetlająca tytuł gry
        public static void PrintTitle()
        {
            AnsiConsole.Write(
                new FigletText("Pasjans")
                    .Color(Color.Green));
        }

        // Metoda rysująca talię i karty odkryte
        public static void DrawDeck()
        {      
            var table = new Table().Centered();
            string ButtonText = "";

            // Dodanie kolumn
            table.AddColumn("[grey]Karty Nieodkryte[/]");
            table.AddColumn("[grey]Karty Odkryte[/]");

            // Wyświetlenie tylko jednej karty w każdej kolumnie
            string hiddenCard = deck.Count > 0 ? "[grey]🃏[/]" : "[dim]---[/]";
            string visibleCard = SelectedDrawnDeck.Count > 0 
                ? SelectedDrawnDeck[^1].ToString() 
                : "[dim]---[/]";

            // Upewnienie się, że ostatnia dobrana karta jest widoczna
            if (SelectedDrawnDeck.Count > 0)
            {
                SelectedDrawnDeck[^1].IsReversed = false;
            }

            // Dodanie pojedynczego wiersza z jedną kartą w każdej kolumnie
            table.AddRow(hiddenCard, visibleCard);

            // Dodanie wskaźników liczby kart
            table.AddRow(
                $"[grey]({deck.Count} kart)[/]",
                $"[grey]({SelectedDrawnDeck.Count} kart)[/]"
            );

            // Panel przycisku
            if (deck.Count > 0)
            {
                ButtonText = "NACIŚNIJ D ABY DOBRAĆ! || ESCAPE ABY WYJŚĆ || R ABY ZRESETOWAĆ! || STRZAŁKI I ENTER ABY PRZENOSIĆ KARTY!";
            }
            else
            {
                ButtonText = "NACIŚNIJ D ABY PRZETASOWAĆ!";
            }
            var buttonPanel = new Panel($"[blue] {ButtonText} [/]")
                .Border(BoxBorder.Rounded)
                .BorderStyle(new Style(Color.Blue))
                .Padding(1, 1);

            // Rysowanie obu paneli
            AnsiConsole.Write(new Panel(table).Header("[blue]Talia[/]").Border(BoxBorder.Double));
            AnsiConsole.Write(buttonPanel);
        }

        // Metoda rysująca podstawy do układania kart
        public static void DrawFoundations()
        {
            var table = new Table().Centered();
            table.AddColumn("[grey]Podstawa ♥[/]");
            table.AddColumn("[grey]Podstawa ♦[/]");
            table.AddColumn("[grey]Podstawa ♣[/]");
            table.AddColumn("[grey]Podstawa ♠[/]");

            table.AddRow(
                foundation1.Count > 0 ? foundation1[^1].ToString() : "[dim]---[/]",
                foundation2.Count > 0 ? foundation2[^1].ToString() : "[dim]---[/]",
                foundation3.Count > 0 ? foundation3[^1].ToString() : "[dim]---[/]",
                foundation4.Count > 0 ? foundation4[^1].ToString() : "[dim]---[/]"
            );

            AnsiConsole.Write(new Panel(table).Header("Podstawy").Border(BoxBorder.Double));
        }

        // Metoda losująca określoną liczbę kart z talii
        public static List<Card> chooseCards(List<Card> deck, int count)
        {
            // Losowy wybór kart z talii
            var random = new Random();
            List<Card> list = new List<Card>();

            // Wybieranie określonej liczby kart
            for (int i = 0; i < count; i++)
            {
                if (deck.Count == 0)
                    break;

                int index = random.Next(deck.Count);
                list.Add(deck[index]);
                deck.RemoveAt(index);
            }
            return list;
        }
    }
}



        // Notatki dotyczące interfejsu Spectre.Console:
        // 1. AnsiConsole.MarkupLine("[kolor] {tekst} [/]") - Kolorowanie tekstu
        // 2. Panel(" xxxx ") - Tworzenie ramki
        // 2.1. Metody modyfikujące panel:
        // 2.2. .Border() - Styl ramki
        // 2.3. .BorderStyle() - Kolor ramki
        // 2.4. .Padding() - Marginesy wewnętrzne
        // 2.5. .Expand() - Rozszerzenie do dostępnego miejsca
        // 2.6. .Header() - Nagłówek panelu
        // 2.7. Przykład czerwonego serca:
        // new Panel("♥ A").Border(BoxBorder.Rounded).BorderStyle(new Style(Color.Red))
        // 3. WriteLine() - Nowa linia
        // 4. Table() - Tworzenie tabeli
        // 4.1. AddColumn() - Dodawanie kolumny
        // 4.2. AddRow() - Dodawanie wiersza
        // 5. Clear - Czyszczenie ekranu
        // 6. FigletText - Ozdobny tekst
