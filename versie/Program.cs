using System.Reflection.Metadata.Ecma335;

namespace Opdracht6b
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int MAX_TAKENS = 100;

            bool isVoltooien = false;
            bool isVerwijderen = false;    

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

                // ===== TAKEN =====
                Console.WriteLine("Taken:");
                for (int i = 0; i < aantalActieveTaken; i++)
                {
                    if (!heeftDeadline[i])
                    {
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
                        if (isVoltooien || isVerwijderen)
                        {
                            Console.WriteLine($"[{i + 1}] deadline: {deadlines[i]:dd/MM/yyyy} (nog: {dagen} dagen) | {actieveTaken[i]}");
                        }
                         else
                        {
                            Console.WriteLine($"- deadline: {deadlines[i]:dd/MM/yyyy} (nog: {dagen} dagen) | {actieveTaken[i]}");
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

                // ===== MENU =====
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
                else { Console.Write("Acties ([T]oevoegen / [V]oltooien / [D]efinitief verwijderen) ?: "); }

                string keuze = Console.ReadLine().ToUpper();

                isVoltooien = false;
                // ===== VOLTOOIEN =====
                if (keuze == "V")
                {
                    isVoltooien = true;
                    continue;
                }
            }
        }
    }
}