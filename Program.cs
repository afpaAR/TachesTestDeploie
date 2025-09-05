namespace TachesTestDeploie
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TacheRepository repo = new TacheRepository();
            while (true)
            {
                Console.WriteLine("\n1. Ajouter tâche\n2. Lister tâches\n3. Marquer terminée\n4. Supprimer tâche\n5. Quitter");
                Console.Write("Choix: ");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        Console.Write("Nom de la tâche: ");
                        string name = Console.ReadLine();
                        Console.Write("Description de la tâche: ");
                        string desc = Console.ReadLine();
                        repo.AddTache(new Taches { Nom = name, Description = desc, DateCreation = System.DateTime.Now.Date });
                        break;
                    case "2":
                        var Taches = repo.GetTaches();
                        Console.WriteLine("Liste des taches :");
                        for (int i = 0; i < Taches.Count; i++)
                        {
                            Console.WriteLine($"Numéro : {Taches[i].Id}. {Taches[i].Nom} - {((Taches[i].DateFermeture == DateTime.MinValue) ? "En cours" : "Terminée")} - {Taches[i].Description}");
                        }
                       break;
                    case "3":
                        Console.Write("Numéro de la tâche à terminer: ");
                        if (int.TryParse(Console.ReadLine(), out int Id))
                            repo.MarkCompleted(Id);
                        break;
                    case "4":
                        Console.Write("Numéro de la tâche à supprimer: ");
                        if (int.TryParse(Console.ReadLine(), out int Idd))
                            repo.DeleteTache(Idd);
                        break;
                    case "5": return;
                }
            }
        }
    }
}
