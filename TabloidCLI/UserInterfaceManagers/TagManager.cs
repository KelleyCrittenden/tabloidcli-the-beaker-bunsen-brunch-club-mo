using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Microsoft.Data.SqlClient;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    public class TagManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private string _connectionString;
        private TagRepository _tagRepository;

        public TagManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _connectionString = connectionString;
            _tagRepository = new TagRepository(connectionString);
        }

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Tag Menu");
            Console.WriteLine(" 1) List Tags");
            Console.WriteLine(" 2) Add Tag");
            Console.WriteLine(" 3) Edit Tag");
            Console.WriteLine(" 4) Remove Tag");
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
                    Edit();
                    return this;
                case "4":
                    Remove();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        private Tag Choose(string prompt = null)
        {
        ChooseTag:
            if (prompt == null)
            {
                prompt = "Please choose a Tag:";
            }

            Console.WriteLine(prompt);

            List<Tag> tags = _tagRepository.GetAll();

            for (int i = 0; i < tags.Count; i++)
            {
                Tag tag = tags[i];
                Console.WriteLine($" {i + 1}) {tag.Name}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return tags[choice - 1];
            }
            catch (Exception)
            {
                Console.WriteLine();
                Console.WriteLine("Invalid Selection");
                Console.WriteLine();
                goto ChooseTag;
            }
        }

        private void List()
        {
            throw new NotImplementedException();
        }

        private void Add()
        {
            throw new NotImplementedException();
        }

        private void Edit()
        {
            Tag tagToEdit = Choose("Which tag would you like to edit?");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine(tagToEdit);
        }

        private void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
