namespace versie2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int MAX = 100;

            string[] taken = new string[MAX];
            DateTime[] deadlines = new DateTime[MAX];
            bool[] heeftDeadline = new bool[MAX];
            int aantal = 0;

            string[] done = new string[MAX];
            int doneAantal = 0;

            // init
            taken[aantal] = "Stofzuiger zoeken en kopen";
            heeftDeadline[aantal++] = false;

            taken[aantal] = "Uitnodigingen opstellen feestje";
            heeftDeadline[aantal] = true;
            deadlines[aantal++] = new DateTime(2026, 3, 9);

            taken[aantal] = "testtaak X";
            heeftDeadline[aantal] = true;
            deadlines[aantal++] = new DateTime(2026, 3, 14);

            while (true)
            {
                Console.Clear();

                // TAKEN
                Console.WriteLine("Taken:");
                for (int i = 0; i < aantal; i++)
                {
                    if (!heeftDeadline[i])
                        Console.WriteLine($"- {taken[i]}");
                }

                Console.WriteLine();

                // DEADLINES
                Console.WriteLine("Taken met deadline:");
                for (int i = 0; i < aantal; i++)
                {
                    if (heeftDeadline[i])
                    {
                        int dagen = (deadlines[i] - DateTime.Today).Days;
                        Console.WriteLine($"- deadline: {deadlines[i]:dd/MM/yyyy} (nog: {dagen} dagen) | {taken[i]}");
                    }
                }

                Console.WriteLine();

                // DONE
                Console.WriteLine("Voltooide taken:");
                for (int i = 0; i < doneAantal; i++)
                {
                    Console.WriteLine($"- {done[i]}");
                }

                Console.WriteLine();

                Console.Write("Acties ([T]oevoegen / [V]oltooien / [D]efinitief verwijderen) ?: ");
                string keuze = Console.ReadLine().ToUpper();

                // ===== TOEVOEGEN =====
                if (keuze == "T")
                {
                    Console.Write("Omschrijving?: ");
                    string oms = Console.ReadLine();

                    Console.Write("Deadline (yyyy-MM-dd of Enter): ");
                    string input = Console.ReadLine();

                    if (input == "")
                    {
                        taken[aantal] = oms;
                        heeftDeadline[aantal] = false;
                        aantal++;
                    }
                    else if (DateTime.TryParse(input, out DateTime d))
                    {
                        taken[aantal] = oms;
                        heeftDeadline[aantal] = true;
                        deadlines[aantal] = d;
                        aantal++;
                    }
                    else
                    {
                        Console.WriteLine("Foute datum!");
                        Console.ReadKey();
                    }
                }

                // ===== VOLTOOIEN =====
                else if (keuze == "V")
                {
                    Console.WriteLine("\nTaken:");

                    for (int i = 0; i < aantal; i++)
                    {
                        Console.WriteLine($"[ {i + 1} ] {taken[i]}");
                    }

                    Console.Write("Nummer?: ");
                    if (int.TryParse(Console.ReadLine(), out int nr))
                    {
                        int index = nr - 1;

                        if (index >= 0 && index < aantal)
                        {
                            string tekst = $"voltooid op: {DateTime.Today:dd/MM/yyyy}";

                            if (heeftDeadline[index])
                                tekst += $" | deadline: {deadlines[index]:dd/MM/yyyy} | {taken[index]}";
                            else
                                tekst += $" | {taken[index]}";

                            done[doneAantal++] = tekst;

                            for (int i = index; i < aantal - 1; i++)
                            {
                                taken[i] = taken[i + 1];
                                deadlines[i] = deadlines[i + 1];
                                heeftDeadline[i] = heeftDeadline[i + 1];
                            }

                            aantal--;
                        }
                    }
                }

                // ===== DELETE =====
                else if (keuze == "D")
                {
                    Console.WriteLine("\nTaken:");

                    for (int i = 0; i < aantal; i++)
                    {
                        Console.WriteLine($"[ {i + 1} ] {taken[i]}");
                    }

                    Console.Write("Nummer?: ");
                    if (int.TryParse(Console.ReadLine(), out int nr))
                    {
                        int index = nr - 1;

                        if (index >= 0 && index < aantal)
                        {
                            for (int i = index; i < aantal - 1; i++)
                            {
                                taken[i] = taken[i + 1];
                                deadlines[i] = deadlines[i + 1];
                                heeftDeadline[i] = heeftDeadline[i + 1];
                            }

                            aantal--;
                        }
                    }
                }

                Console.WriteLine("\nDruk op toets...");
                Console.ReadKey();
            }
        }
    }
}
