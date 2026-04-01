namespace versie3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int MAX_TAKENS = 100;

            // Флаги для режимов
            bool isVoltooien = false;      // режим выбора задачи для выполнения
            bool isVerwijderen = false;    // режим выбора задачи для удаления

            string[] actieveTaken = new string[MAX_TAKENS];
            DateTime[] deadlines = new DateTime[MAX_TAKENS];
            bool[] heeftDeadline = new bool[MAX_TAKENS];
            int aantalActieveTaken = 0;

            string[] voltooideTaken = new string[MAX_TAKENS];
            int aantalVoltooideTaken = 0;

            // ===== INPUT =====
            actieveTaken[aantalActieveTaken] = "Stofzuiger zoeken en kopen";
            heeftDeadline[aantalActieveTaken++] = false;

            actieveTaken[aantalActieveTaken] = "Uitnodigingen opstellen feestje";
            heeftDeadline[aantalActieveTaken] = true;
            deadlines[aantalActieveTaken++] = new DateTime(2026, 3, 9);

            actieveTaken[aantalActieveTaken] = "testtaak X";
            heeftDeadline[aantalActieveTaken] = true;
            deadlines[aantalActieveTaken++] = new DateTime(2026, 3, 14);

            voltooideTaken[aantalVoltooideTaken++] = "voltooid op: 13/03/2026 | Gras afrijden";
            voltooideTaken[aantalVoltooideTaken++] = "voltooid op: 13/03/2026 | deadline: 15/04/2026 | Belastingsbrief invullen";
            voltooideTaken[aantalVoltooideTaken++] = "voltooid op: 28/02/2026 | deadline: 5/03/2026 | Papieren RVA invullen";
            voltooideTaken[aantalVoltooideTaken++] = "voltooid op: 4/03/2026 | Containerpark";
            voltooideTaken[aantalVoltooideTaken++] = "voltooid op: 25/02/2026 | Afspraak maken bij kapper";
            voltooideTaken[aantalVoltooideTaken++] = "voltooid op: 13/03/2026 | deadline: 14/03/2026 | testtaak8";

            // ===== LOOP =====
            while (true)
            {
                Console.Clear();

                // ===== TAKEN (zonder deadline) =====
                Console.WriteLine("Taken:");
                for (int i = 0; i < aantalActieveTaken; i++)
                {
                    if (!heeftDeadline[i])
                    {
                        // Если в режиме выбора задачи — показываем с номерами
                        if (isVoltooien || isVerwijderen)
                        {
                            Console.WriteLine($"[{i + 1}] {actieveTaken[i]}");
                        }
                        else
                        {
                            Console.WriteLine($"- {actieveTaken[i]}");
                        }
                    }
                }
                Console.WriteLine();

                // ===== TAKEN MET DEADLINE =====
                Console.WriteLine("Taken met deadline:");
                for (int i = 0; i < aantalActieveTaken; i++)
                {
                    if (heeftDeadline[i])
                    {
                        int dagen = (deadlines[i] - DateTime.Today).Days;
                        string status = dagen < 0 ? $"nog: {dagen} dagen" : $"nog: {dagen} dagen";

                        // Если в режиме выбора задачи — показываем с номерами
                        if (isVoltooien || isVerwijderen)
                        {
                            Console.WriteLine($"[{i + 1}] deadline: {deadlines[i]:dd/MM/yyyy} ({status}) | {actieveTaken[i]}");
                        }
                        else
                        {
                            Console.WriteLine($"- deadline: {deadlines[i]:dd/MM/yyyy} ({status}) | {actieveTaken[i]}");
                        }
                    }
                }
                Console.WriteLine();

                // ===== VOLTOOIDE TAKEN =====
                Console.WriteLine("Voltooide taken:");
                for (int i = 0; i < aantalVoltooideTaken; i++)
                {
                    Console.WriteLine($"- {voltooideTaken[i]}");
                }
                Console.WriteLine();

                // ===== VERWERK MODUS (VOLTOOIEN OF VERWIJDEREN) =====
                if (isVoltooien)
                {
                    Console.Write("\nWelke taak (voor nummer in of [A]nnuleren) ?: ");
                    string invoer = Console.ReadLine()?.Trim() ?? "";

                    if (invoer.ToUpper() != "A")
                    {
                        if (int.TryParse(invoer, out int nr) && nr >= 1 && nr <= aantalActieveTaken)
                        {
                            int index = nr - 1;

                            string tekst = $"voltooid op: {DateTime.Today:dd/MM/yyyy}";

                            if (heeftDeadline[index])
                            {
                                tekst += $" | deadline: {deadlines[index]:dd/MM/yyyy} | {actieveTaken[index]}";
                            }
                            else
                            {
                                tekst += $" | {actieveTaken[index]}";
                            }

                            if (aantalVoltooideTaken < MAX_TAKENS)
                            {
                                voltooideTaken[aantalVoltooideTaken++] = tekst;
                            }

                            // Verwijder de taak uit actieve taken
                            for (int i = index; i < aantalActieveTaken - 1; i++)
                            {
                                actieveTaken[i] = actieveTaken[i + 1];
                                deadlines[i] = deadlines[i + 1];
                                heeftDeadline[i] = heeftDeadline[i + 1];
                            }
                            aantalActieveTaken--;
                        }
                        else
                        {
                            Console.WriteLine("Ongeldig nummer.");
                            Console.ReadKey();
                        }
                    }
                    isVoltooien = false;
                    continue;
                }
                else if (isVerwijderen)
                {
                    Console.Write("\nWelke taak (voor nummer in of [A]nnuleren) ?: ");
                    string invoer = Console.ReadLine()?.Trim() ?? "";

                    if (invoer.ToUpper() != "A")
                    {
                        if (int.TryParse(invoer, out int nr) && nr >= 1 && nr <= aantalActieveTaken)
                        {
                            int index = nr - 1;

                            // Verwijder de taak uit actieve taken (zonder toevoegen aan voltooide)
                            for (int i = index; i < aantalActieveTaken - 1; i++)
                            {
                                actieveTaken[i] = actieveTaken[i + 1];
                                deadlines[i] = deadlines[i + 1];
                                heeftDeadline[i] = heeftDeadline[i + 1];
                            }
                            aantalActieveTaken--;
                        }
                        else
                        {
                            Console.WriteLine("Ongeldig nummer.");
                            Console.ReadKey();
                        }
                    }
                    isVerwijderen = false;
                    continue;
                }

                // ===== MENU (alleen als we niet in een modus zitten) =====
                Console.Write("Acties ([T]oevoegen / [V]oltooien / [D]efinitief verwijderen) ?: ");
                string keuze = Console.ReadLine()?.ToUpper() ?? "";

                // ===== TOEVOEGEN =====
                if (keuze == "T")
                {
                    // Controle op volle array
                    if (aantalActieveTaken >= MAX_TAKENS)
                    {
                        Console.WriteLine("Te veel taken, kan geen nieuwe taak toevoegen.");
                        Console.ReadKey();
                        continue;
                    }

                    // Invoer omschrijving met validatie
                    string omschrijving = "";
                    while (true)
                    {
                        Console.Write("Omschrijving van de nieuwe taak (max 30 karakters) ?: ");
                        omschrijving = Console.ReadLine()?.Trim() ?? "";

                        if (string.IsNullOrWhiteSpace(omschrijving))
                        {
                            Console.WriteLine("Ongelijke invoer, de omschrijving mag niet leeg zijn.");
                        }
                        else if (omschrijving.Length > 30)
                        {
                            Console.WriteLine("Ongelijke invoer, de omschrijving mag maximaal 30 karakters bevatten.");
                        }
                        else
                        {
                            break;
                        }
                    }

                    // Invoer deadline
                    Console.Write("Deadline van de nieuwe taak (Formaat): yyyy-MM-dd, optioneel: druk Enter om over te slaan. ");
                    string input = Console.ReadLine()?.Trim() ?? "";

                    if (string.IsNullOrWhiteSpace(input))
                    {
                        // Taak zonder deadline
                        actieveTaken[aantalActieveTaken] = omschrijving;
                        heeftDeadline[aantalActieveTaken] = false;
                        aantalActieveTaken++;
                    }
                    else
                    {
                        // Controleer of de datum in het juiste formaat is
                        if (DateTime.TryParseExact(input, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime datum))
                        {
                            actieveTaken[aantalActieveTaken] = omschrijving;
                            heeftDeadline[aantalActieveTaken] = true;
                            deadlines[aantalActieveTaken] = datum;
                            aantalActieveTaken++;
                        }
                        else
                        {
                            Console.WriteLine("Ongelijke invoer, voer een datum in het formaat yyyy-MM-dd in of druk Enter om over te slaan.");
                            Console.ReadKey();
                        }
                    }
                }
                // ===== VOLTOOIEN =====
                else if (keuze == "V")
                {
                    if (aantalActieveTaken == 0)
                    {
                        Console.WriteLine("Geen actieve taken om te voltooien.");
                        Console.ReadKey();
                        continue;
                    }
                    isVoltooien = true;
                    continue;
                }
                // ===== VERWIJDEREN =====
                else if (keuze == "D")
                {
                    if (aantalActieveTaken == 0)
                    {
                        Console.WriteLine("Geen actieve taken om te verwijderen.");
                        Console.ReadKey();
                        continue;
                    }
                    isVerwijderen = true;
                    continue;
                }

                Console.WriteLine("\nDruk op een toets om door te gaan...");
                Console.ReadKey();
            }
        }
    }
}
        
    

