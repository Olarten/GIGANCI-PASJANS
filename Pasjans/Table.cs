using Pasjans;
using Spectre.Console;
using System;
using System.Diagnostics;

public class Plansza
{
    private readonly int totalColumns;

    public Plansza()
    {
        totalColumns = 9;
    }

    public void DrawTable(List<Card> selectedCards) //Funkcja do rysowania planszy
    {
        Random rng = new Random();

        AnsiConsole.Clear(); // Czyści ekran przed rysowaniem na nowo
        Program.PrintTitle();  // Rysuje tytuł ponownie
        Program.DrawDeck(); //Rysuje na nowo talię
        Program.DrawFoundations(); // Rysuje podstawy ponownie

        var table = new Table();

        // Dodawanie kolumn
        for (int i = 1; i <= 7; i++)
        {
            table.AddColumn($"[yellow]Kol {i}[/]");
        }

        // Maxymalna ilość kart w jednej kolumnie
        int max = 15;

        // powiększa miejsce maxymalne w kolumnach
        for (int i = 0; i < max; i++)
        {
            table.AddRow(
                i < Program.column1.Count ? Program.column1[i].ToString() : "   ",
                i < Program.column2.Count ? Program.column2[i].ToString() : "   ",
                i < Program.column3.Count ? Program.column3[i].ToString() : "   ",
                i < Program.column4.Count ? Program.column4[i].ToString() : "   ",
                i < Program.column5.Count ? Program.column5[i].ToString() : "   ",
                i < Program.column6.Count ? Program.column6[i].ToString() : "   ",
                i < Program.column7.Count ? Program.column7[i].ToString() : "   "
            );
        }

        AnsiConsole.WriteLine(); // Odstęp
        for (int i = 0; i < 7; i++)
        {
            if (i == Program.currentColumn)
                AnsiConsole.Markup($"[black on cyan] {i + 1} [/]  ");
            else if (i == Program.firstSelectedColumn && !Program.isFirstSelection)
                AnsiConsole.Markup($"[black on orange1] {i + 1} [/]  ");
            else
                AnsiConsole.Markup($"[grey] {i + 1} [/]  ");
        }
        if (Program.firstSelectedColumn == 7 && !Program.isFirstSelection)
        {
            AnsiConsole.Markup($"[black on orange1] Talia [/]  ");
        }
        else if (Program.currentColumn == 7)
        {
            AnsiConsole.Markup($"[black on cyan] Talia [/]  ");
        }
        else
        {
            AnsiConsole.Markup($"[grey] Talia [/]  ");
        }

        if (Program.firstSelectedColumn == 8 && !Program.isFirstSelection)
        {
            AnsiConsole.Markup($"[black on orange1] Podstawy [/]  ");
        }
        else if (Program.currentColumn == 8)
        {
            AnsiConsole.Markup($"[black on cyan] Podstawy [/]  ");
        }
        else
        {
            AnsiConsole.Markup($"[grey] Podstawy [/]  ");
        }

        AnsiConsole.WriteLine();
        AnsiConsole.Write(new Panel(table).Header("Plansza").Border(BoxBorder.Double));

        // Sprawdź, czy którakolumna jest pusta
        if (
            Program.deck.Count == 0 &&
            Program.column1.Count == 0 &&
            Program.column2.Count == 0 &&
            Program.column3.Count == 0 &&
            Program.column4.Count == 0 &&
            Program.column5.Count == 0 &&
            Program.column6.Count == 0 
        )
        {
            AnsiConsole.MarkupLine("[green] WYGRANA![/]");
            Environment.Exit(0);
        }
    }
}