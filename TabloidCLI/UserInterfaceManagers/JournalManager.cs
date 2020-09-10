using System;
using System.Collections.Generic;
using TabloidCLI.Models;
using TabloidCLI.Repositories;


namespace TabloidCLI.UserInterfaceManagers
{
    public class JournalManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private JournalRepository _journalRepository;
        private string _connectionString;

        public JournalManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _journalRepository = new JournalRepository(connectionString);
            _connectionString = connectionString;
        }

        //Menu Selections Presented To User
        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Journal Menu");
            Console.WriteLine(" 1) List Journal Entries");
            Console.WriteLine(" 2) Add Journal Entry");
            Console.WriteLine(" 3) Edit Journal Entry");
            Console.WriteLine(" 4) Remove Journal Entry");
            Console.WriteLine(" 0) Go Back");
            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    Add();
                    return this;
                case "3":
                    Update();
                    return this;
               case "4":
                    Delete();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        //List Displaying All Journal Entries
        private void List()
        {
            List<Journal> journals = _journalRepository.GetAll();
            foreach (Journal journal in journals)
            {
                Console.WriteLine($"{journal.Title}:  {journal.CreateDateTime}");
                Console.WriteLine($"{journal.Content}");
                Console.WriteLine("-------------");
            }
        }

        //Adding New Journal Entry
        private void Add()
        {
            Console.WriteLine("New Journal Entry");
            Journal journal = new Journal();

            Console.Write("Title: ");
            journal.Title = Console.ReadLine();

            Console.Write("Content: ");
            journal.Content = Console.ReadLine();

            journal.CreateDateTime = DateTime.Now.AddDays(-1);

            _journalRepository.Insert(journal);
        }


        // Method to Choose a Single Entry To Modify/Remove
        private Journal Choose(string prompt = null)
        {
            if (prompt == null)
            {
                prompt = "Please choose a Journal Entry:";
            }

            Console.WriteLine(prompt);

            List<Journal> journals = _journalRepository.GetAll();

            for (int i = 0; i < journals.Count; i++)
            {
                Journal journal = journals[i];
                Console.WriteLine($" {i + 1}) {journal.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return journals[choice - 1];
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }
        }

        //Editing A Single Journal Entry
        private void Update()
        {
            Journal journalToEdit = Choose("Which Journal Entry would you like to edit?");
            if (journalToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            Console.Write("New Entry Title (blank to leave unchanged): ");
            string Title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(Title))
            {
                journalToEdit.Title = Title;
            }
            Console.Write("New Content (blank to leave unchanged): ");
            string Content = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(Content))
            {
                journalToEdit.Content = Content;
            }


            _journalRepository.Update(journalToEdit);
        }

        //Deleting A Single Journal Entry

        private void Delete()
        {
            Journal journalToDelete = Choose("Which Journal Entry would you like to remove?");
            if (journalToDelete != null)
            {
                _journalRepository.Delete(journalToDelete.Id);
                Console.WriteLine("This Journal Entry has been removed.");
            }

            Console.WriteLine();
        }
    }
}



