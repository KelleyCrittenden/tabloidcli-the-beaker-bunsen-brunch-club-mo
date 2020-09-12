using System;
using System.Collections.Generic;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    public class TagManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private TagRepository _tagRepository;
        private string _connectionString;


        public TagManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _tagRepository = new TagRepository(connectionString);
            _connectionString = connectionString;
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

        private void List()
        {
           List<Tag> tags = _tagRepository.GetAll();
            Console.WriteLine();
            Console.WriteLine("Your Tags");
            Console.WriteLine("-------------------------");
            foreach (Tag tag in tags)
            {
                Console.WriteLine($"{tag.Name}");
                Console.WriteLine("-------------");
            }
        }

        private void Add()
        {
            Console.WriteLine("New Tag");
            Tag newTag = new Tag();
            int nameMaxChar = 5;

        TagName:
            Console.Write("Tag name: ");
            newTag.Name = Console.ReadLine();
            if (newTag.Name == "")
            {
                Console.WriteLine("Please enter a tag name");
                goto TagName;
            }
            else if (newTag.Name.Length > nameMaxChar)
            {
                Console.WriteLine($"Name is too long, please limit to {nameMaxChar} characters");
                goto TagName;
            }

            _tagRepository.Insert(newTag);
        }

        private void Edit()
        {
            throw new NotImplementedException();
        }

        private void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
