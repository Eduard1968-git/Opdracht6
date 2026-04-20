using System;
using System.Collections.Generic;
using System.IO;
namespace Opdracht6_18._04._26
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> actieveTaken = new List<string>();
            List<DateTime> deadlines = new List<DateTime>();
            List<bool> heeftDeadline = new List<bool>();
            List<string> voltooideTaken = new List<string>();

            bool isVoltooien = false;
            bool isVerwijderen = false;

            // ===== LADEN UIT BESTAND =====
            if (File.Exists("taken.txt"))
            {
                using (StreamReader reader = new StreamReader("taken.txt"))
                {
                    string line;
                    bool isActive = false;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line == "#ACTIVE")
                        { isActive = true; continue; }
                        if (line == "#VOLTOOID")
                        { isActive = false; continue; }
                        if (isActive)
                        {
                            string[] p = line.Split('|');
                            actieveTaken.Add(p[0]);
                            heeftDeadline.Add(bool.Parse(p[1]));
                            deadlines.Add(DateTime.Parse(p[2]));
                        }
                        else voltooideTaken.Add(line);
                    }
                }
            }

            // ===== VOORBEELDDATA (als bestand leeg is) =====
            if (actieveTaken.Count == 0 && voltooideTaken.Count == 0)
            {
                actieveTaken.Add("Stofzuiger zoeken en kopen"); heeftDeadline.Add(false); deadlines.Add(DateTime.MinValue);
                actieveTaken.Add("Uitnodigingen opstellen feestje"); heeftDeadline.Add(true); deadlines.Add(new DateTime(2026, 3, 9));
                actieveTaken.Add("testtaak X"); heeftDeadline.Add(true); deadlines.Add(new DateTime(2026, 3, 14));
                voltooideTaken.Add("voltooid op: 13/03/2026 | Gras afrijden");
                voltooideTaken.Add("voltooid op: 13/03/2026 | deadline: 15/04/2026 | Belastingsbrief invullen");
                voltooideTaken.Add("voltooid op: 28/02/2026 | deadline: 5/03/2026 | Papieren RVA invullen");
                voltooideTaken.Add("voltooid op: 4/03/2026 | Containerpark");
                voltooideTaken.Add("voltooid op: 25/02/2026 | Afspraak maken bij kapper");
                voltooideTaken.Add("voltooid op: 13/03/2026 | deadline: 14/03/2026 | testtaak8");
            }

            // ===== LOOP =====
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Taken:");
                for (int i = 0; i < actieveTaken.Count; i++)
                    if (!heeftDeadline[i])
                        Console.WriteLine((isVoltooien || isVerwijderen) ? $"[{i + 1}] {actieveTaken[i]}" : $"- {actieveTaken[i]}");
                Console.WriteLine();

                Console.WriteLine("Taken met deadline:");
                for (int i = 0; i < actieveTaken.Count; i++)
                    if (heeftDeadline[i])
                    {
                        int dagen = (deadlines[i] - DateTime.Today).Days;
                        string line = (isVoltooien || isVerwijderen) ? $"[{i + 1}] " : "- ";
                        Console.WriteLine($"{line}deadline: {deadlines[i]:dd/MM/yyyy} (nog: {dagen} dagen) | {actieveTaken[i]}");
                    }
                Console.WriteLine();

                Console.WriteLine("Voltooide taken:");
                foreach (string t in voltooideTaken)
                    Console.WriteLine($"- {t}");
                Console.WriteLine();

                // ===== MODUS: VOLTOOIEN =====
                if (isVoltooien)
                {
                    if (actieveTaken.Count == 0)
                    {
                        Console.WriteLine("Geen actieve taken om te voltooien.");
                        Console.ReadKey();
                        isVoltooien = false;
                        continue;
                    }
                    Console.Write("\nWelke taak (voer nummer in of [A]nnuleren)?: ");
                    string invoer = Console.ReadLine()?.Trim() ?? "";
                    if (invoer.ToUpper() != "A" && int.TryParse(invoer, out int nr) && nr >= 1 && nr <= actieveTaken.Count)
                    {
                        int index = nr - 1;
                        string tekst = $"voltooid op: {DateTime.Today:dd/MM/yyyy}";
                        tekst += heeftDeadline[index] ? $" | deadline: {deadlines[index]:dd/MM/yyyy} | {actieveTaken[index]}" : $" | {actieveTaken[index]}";
                        voltooideTaken.Add(tekst);
                        actieveTaken.RemoveAt(index);
                        deadlines.RemoveAt(index);
                        heeftDeadline.RemoveAt(index);
                        // OPSLAAN
                        using (StreamWriter w = new StreamWriter("taken.txt"))
                        {
                            w.WriteLine("#ACTIVE");
                            for (int i = 0; i < actieveTaken.Count; i++)
                            {
                                w.WriteLine($"{actieveTaken[i]}|{heeftDeadline[i]}|{deadlines[i]:yyyy-MM-dd}");
                            }
                            w.WriteLine("#VOLTOOID");
                            foreach (string t in voltooideTaken) w.WriteLine(t);
                        }
                    }
                    else if (invoer.ToUpper() != "A")
                        Console.WriteLine("Ongeldige invoer.");
                    isVoltooien = false;
                    continue;
                }

                // ===== MODUS: VERWIJDEREN =====
                if (isVerwijderen)
                {
                    if (actieveTaken.Count == 0)
                    {
                        Console.WriteLine("Geen actieve taken om te verwijderen.");
                        Console.ReadKey();
                        isVerwijderen = false;
                        continue;
                    }
                    Console.Write("\nWelke taak (voer nummer in of [A]nnuleren)?: ");
                    string invoer = Console.ReadLine()?.Trim() ?? "";
                    if (invoer.ToUpper() != "A" && int.TryParse(invoer, out int nr) && nr >= 1 && nr <= actieveTaken.Count)
                    {
                        int index = nr - 1;
                        actieveTaken.RemoveAt(index);
                        deadlines.RemoveAt(index);
                        heeftDeadline.RemoveAt(index);
                        // OPSLAAN
                        using (StreamWriter w = new StreamWriter("taken.txt"))
                        {
                            w.WriteLine("#ACTIVE");
                            for (int i = 0; i < actieveTaken.Count; i++)
                            {
                                w.WriteLine($"{actieveTaken[i]}|{heeftDeadline[i]}|{deadlines[i]:yyyy-MM-dd}");
                            }
                            w.WriteLine("#VOLTOOID");
                            foreach (string t in voltooideTaken) w.WriteLine(t);
                        }
                    }
                    else if (invoer.ToUpper() != "A")
                        Console.WriteLine("Ongeldige invoer.");
                    isVerwijderen = false;
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
                        Console.WriteLine();
                        Console.Write("Omschrijving van de nieuwe taak (max 30 karakters)?: ");
                        omschrijving = Console.ReadLine()?.Trim() ?? "";
                        if (string.IsNullOrWhiteSpace(omschrijving))
                            Console.WriteLine("Ongeldige invoer, de omschrijving mag niet leeg zijn.");
                        else if (omschrijving.Length > 30)
                            Console.WriteLine("Ongeldige invoer, de omschrijving mag maximaal 30 karakters bevatten.");
                        else
                            break;
                    }
                    Console.Write("Deadline van de nieuwe taak (formaat: yyyy-MM-dd, optioneel: druk Enter om over te slaan)?:  ");
                    string inp = Console.ReadLine()?.Trim() ?? "";
                    if (string.IsNullOrWhiteSpace(inp))
                    {
                        actieveTaken.Add(omschrijving);
                        heeftDeadline.Add(false);
                        deadlines.Add(DateTime.MinValue);
                    }
                    else if (DateTime.TryParseExact(inp, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime dt))
                    {
                        actieveTaken.Add(omschrijving);
                        heeftDeadline.Add(true);
                        deadlines.Add(dt);
                    }
                    else
                    {
                        Console.WriteLine("Ongeldige invoer, voer een datum in het formaat yyyy-MM-dd in of druk Enter om over te slaan.");
                        Console.ReadKey();
                        continue;
                    }
                    // OPSLAAN
                    using (StreamWriter w = new StreamWriter("taken.txt"))
                    {
                        w.WriteLine("#ACTIVE");
                        for (int i = 0; i < actieveTaken.Count; i++)
                        {
                            w.WriteLine($"{actieveTaken[i]}|{heeftDeadline[i]}|{deadlines[i]:yyyy-MM-dd}");
                        }
                        w.WriteLine("#VOLTOOID");
                        foreach (string t in voltooideTaken)
                            w.WriteLine(t);
                    }
                }
                else if (keuze == "V")
                {
                    isVoltooien = true;
                    continue;
                }
                else if (keuze == "D")
                {
                    isVerwijderen = true;
                    continue;
                }
                Console.WriteLine("\nDruk op een toets om door te gaan...");
                Console.ReadKey();
            }
        }
    }
}
