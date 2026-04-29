using System;
using System.IO;
using System.Globalization;
namespace Opdracht6b
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] actieveTaken = new string[0];
            DateTime[] deadlines = new DateTime[0];
            bool[] heeftDeadline = new bool[0];
            int aantalActieveTaken = 0;

            string[] voltooideTaken = new string[0];
            int aantalVoltooideTaken = 0;

            bool normaalModus = true;
            bool voltooiModus = false;
            bool verwijderModus = false;
            // ===== LADEN UIT BESTAND =====
            if (File.Exists("taken.txt"))
            {
                using (StreamReader reader = new StreamReader("taken.txt"))
                {
                    string line;
                    bool isActive = false;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line == "#ACTIVE") { isActive = true; continue; }
                        if (line == "#VOLTOOID") { isActive = false; continue; }
                        if (isActive)
                        {
                            string[] p = line.Split('|');
                            Array.Resize(ref actieveTaken, aantalActieveTaken + 1);
                            Array.Resize(ref deadlines, aantalActieveTaken + 1);
                            Array.Resize(ref heeftDeadline, aantalActieveTaken + 1);
                            actieveTaken[aantalActieveTaken] = p[0];
                            heeftDeadline[aantalActieveTaken] = bool.Parse(p[1]);
                            deadlines[aantalActieveTaken] = DateTime.ParseExact(p[2], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            aantalActieveTaken++;
                        }
                        else
                        {
                            Array.Resize(ref voltooideTaken, aantalVoltooideTaken + 1);
                            voltooideTaken[aantalVoltooideTaken] = line;
                            aantalVoltooideTaken++;
                        }
                    }
                }
            }
            // ===== VOORBEELDDATA (als bestand leeg is) =====
            if (aantalActieveTaken == 0 && aantalVoltooideTaken == 0)
            {
                Array.Resize(ref actieveTaken, 3);
                Array.Resize(ref deadlines, 3);
                Array.Resize(ref heeftDeadline, 3);
                actieveTaken[0] = "Stofzuiger zoeken en kopen"; heeftDeadline[0] = false; deadlines[0] = DateTime.MinValue;
                actieveTaken[1] = "Uitnodigingen opstellen feestje"; heeftDeadline[1] = true; deadlines[1] = new DateTime(2026, 3, 9);
                actieveTaken[2] = "testtaak X"; heeftDeadline[2] = true; deadlines[2] = new DateTime(2026, 3, 14);
                aantalActieveTaken = 3;
                Array.Resize(ref voltooideTaken, 6);
                voltooideTaken[0] = "voltooid op: 13/03/2026 | Gras afrijden";
                voltooideTaken[1] = "voltooid op: 13/03/2026 | deadline: 15/04/2026 | Belastingsbrief invullen";
                voltooideTaken[2] = "voltooid op: 28/02/2026 | deadline: 5/03/2026 | Papieren RVA invullen";
                voltooideTaken[3] = "voltooid op: 4/03/2026 | Containerpark";
                voltooideTaken[4] = "voltooid op: 25/02/2026 | Afspraak maken bij kapper";
                voltooideTaken[5] = "voltooid op: 13/03/2026 | deadline: 14/03/2026 | testtaak8";
                aantalVoltooideTaken = 6;
            }
            // ===== LOOP =====
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Taken:");
                for (int i = 0; i < aantalActieveTaken; i++)
                    if (!heeftDeadline[i])
                        if (normaalModus) Console.WriteLine($"- {actieveTaken[i]}");
                        else Console.WriteLine($"[{i + 1}] {actieveTaken[i]}");
                Console.WriteLine();

                Console.WriteLine("Taken met deadline:");
                for (int i = 0; i < aantalActieveTaken; i++)
                    if (heeftDeadline[i])
                    {
                        int dagen = (deadlines[i] - DateTime.Today).Days;
                        if (normaalModus) Console.WriteLine($"- deadline: {deadlines[i]:dd/MM/yyyy} (nog: {dagen} dagen) | {actieveTaken[i]}");
                        else Console.WriteLine($"[{i + 1}] deadline: {deadlines[i]:dd/MM/yyyy} (nog: {dagen} dagen) | {actieveTaken[i]}");
                    }
                Console.WriteLine();

                Console.WriteLine("Voltooide taken:");
                for (int i = 0; i < aantalVoltooideTaken; i++) Console.WriteLine($"- {voltooideTaken[i]}");
                Console.WriteLine();
                // ===== MODUS: VOLTOOIEN =====
                if (voltooiModus)
                {
                    if (aantalActieveTaken == 0)
                    {
                        Console.WriteLine("Geen actieve taken om te voltooien.");
                        Console.ReadKey();
                        voltooiModus = false; normaalModus = true;
                        continue;
                    }
                    Console.Write("\nWelke taak (voer nummer in of [A]nnuleren) ?: ");
                    string invoer = Console.ReadLine()?.Trim() ?? "";
                    if (invoer.ToUpper() != "A" && int.TryParse(invoer, out int nr) && nr >= 1 && nr <= aantalActieveTaken)
                    {
                        int index = nr - 1;
                        string tekst = $"voltooid op: {DateTime.Today:dd/MM/yyyy}";
                        tekst += heeftDeadline[index] ? $" | deadline: {deadlines[index]:dd/MM/yyyy} | {actieveTaken[index]}" : $" | {actieveTaken[index]}";
                        Array.Resize(ref voltooideTaken, aantalVoltooideTaken + 1);
                        voltooideTaken[aantalVoltooideTaken++] = tekst;
                        for (int i = index; i < aantalActieveTaken - 1; i++)
                        {
                            actieveTaken[i] = actieveTaken[i + 1];
                            deadlines[i] = deadlines[i + 1];
                            heeftDeadline[i] = heeftDeadline[i + 1];
                        }
                        Array.Resize(ref actieveTaken, aantalActieveTaken - 1);
                        Array.Resize(ref deadlines, aantalActieveTaken - 1);
                        Array.Resize(ref heeftDeadline, aantalActieveTaken - 1);
                        aantalActieveTaken--;
                        using (StreamWriter w = new StreamWriter("taken.txt"))
                        {
                            w.WriteLine("#ACTIVE");
                            for (int i = 0; i < aantalActieveTaken; i++) w.WriteLine($"{actieveTaken[i]}|{heeftDeadline[i]}|{deadlines[i]:yyyy-MM-dd}");
                            w.WriteLine("#VOLTOOID");
                            for (int i = 0; i < aantalVoltooideTaken; i++) w.WriteLine(voltooideTaken[i]);
                        }
                    }
                    else if (invoer.ToUpper() != "A") Console.WriteLine("Ongeldige invoer.");
                    voltooiModus = false; normaalModus = true;
                    continue;
                }
                // ===== MODUS: VERWIJDEREN =====
                if (verwijderModus)
                {
                    if (aantalActieveTaken == 0)
                    {
                        Console.WriteLine("Geen actieve taken om te verwijderen.");
                        Console.ReadKey();
                        verwijderModus = false; normaalModus = true;
                        continue;
                    }
                    Console.Write("\nWelke taak (voer nummer in of [A]nnuleren) ?: ");
                    string invoer = Console.ReadLine()?.Trim() ?? "";
                    if (invoer.ToUpper() != "A" && int.TryParse(invoer, out int nr) && nr >= 1 && nr <= aantalActieveTaken)
                    {
                        int index = nr - 1;
                        for (int i = index; i < aantalActieveTaken - 1; i++)
                        {
                            actieveTaken[i] = actieveTaken[i + 1];
                            deadlines[i] = deadlines[i + 1];
                            heeftDeadline[i] = heeftDeadline[i + 1];
                        }
                        Array.Resize(ref actieveTaken, aantalActieveTaken - 1);
                        Array.Resize(ref deadlines, aantalActieveTaken - 1);
                        Array.Resize(ref heeftDeadline, aantalActieveTaken - 1);
                        aantalActieveTaken--;
                        using (StreamWriter w = new StreamWriter("taken.txt"))
                        {
                            w.WriteLine("#ACTIVE");
                            for (int i = 0; i < aantalActieveTaken; i++) w.WriteLine($"{actieveTaken[i]}|{heeftDeadline[i]}|{deadlines[i]:yyyy-MM-dd}");
                            w.WriteLine("#VOLTOOID");
                            for (int i = 0; i < aantalVoltooideTaken; i++) w.WriteLine(voltooideTaken[i]);
                        }
                    }
                    else if (invoer.ToUpper() != "A") Console.WriteLine("Ongeldige invoer.");
                    verwijderModus = false; normaalModus = true;
                    continue;
                }
                // ===== MENU =====
                Console.Write("Acties ([T]oevoegen / [V]oltooien / [D]efinitief verwijderen) ?: ");
                string keuze = Console.ReadLine()?.ToUpper();
                if (keuze == "T")
                {
                    string omschrijving = "";
                    while (true)
                    {
                        Console.Write("Omschrijving van de nieuwe taak (max 30 karakters) ?: ");
                        omschrijving = Console.ReadLine()?.Trim() ?? "";
                        if (string.IsNullOrWhiteSpace(omschrijving))
                            Console.WriteLine("Ongeldige invoer, de omschrijving mag niet leeg zijn.");
                        else if (omschrijving.Length > 30)
                            Console.WriteLine("Ongeldige invoer, de omschrijving mag maximaal 30 karakters bevatten.");
                        else break;
                    }
                    Console.Write("Deadline van de nieuwe taak (formaat: yyyy-MM-dd, optioneel: druk Enter om over te slaan)?: ");
                    string inp = Console.ReadLine()?.Trim() ?? "";
                    Array.Resize(ref actieveTaken, aantalActieveTaken + 1);
                    Array.Resize(ref deadlines, aantalActieveTaken + 1);
                    Array.Resize(ref heeftDeadline, aantalActieveTaken + 1);
                    if (string.IsNullOrWhiteSpace(inp))
                    {
                        actieveTaken[aantalActieveTaken] = omschrijving;
                        heeftDeadline[aantalActieveTaken] = false;
                        deadlines[aantalActieveTaken] = DateTime.MinValue;
                        aantalActieveTaken++;
                    }
                    else if (DateTime.TryParseExact(inp, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                    {
                        actieveTaken[aantalActieveTaken] = omschrijving;
                        heeftDeadline[aantalActieveTaken] = true;
                        deadlines[aantalActieveTaken] = dt;
                        aantalActieveTaken++;
                    }
                    else
                    {
                        Console.WriteLine("Ongeldige invoer, voer een datum in het formaat yyyy-MM-dd in of druk Enter om over te slaan.");
                        Console.ReadKey(); continue;
                    }
                    using (StreamWriter w = new StreamWriter("taken.txt"))
                    {
                        w.WriteLine("#ACTIVE");
                        for (int i = 0; i < aantalActieveTaken; i++) w.WriteLine($"{actieveTaken[i]}|{heeftDeadline[i]}|{deadlines[i]:yyyy-MM-dd}");
                        w.WriteLine("#VOLTOOID");
                        for (int i = 0; i < aantalVoltooideTaken; i++) w.WriteLine(voltooideTaken[i]);
                    }
                }
                else if (keuze == "V")
                {
                    if (aantalActieveTaken == 0)
                    {
                        Console.WriteLine("Geen actieve taken om te voltooien.");
                        Console.ReadKey();
                    }
                    else
                    {
                        normaalModus = false; voltooiModus = true;
                    }
                    continue;
                }
                else if (keuze == "D")
                {
                    if (aantalActieveTaken == 0)
                    {
                        Console.WriteLine("Geen actieve taken om te verwijderen.");
                        Console.ReadKey();
                    }
                    else
                    {
                        normaalModus = false; verwijderModus = true;
                    }
                    continue;
                }
                Console.WriteLine("\nDruk op een toets om door te gaan....");
                Console.ReadKey();
            }
        }
    }
}