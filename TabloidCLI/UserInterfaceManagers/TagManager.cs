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
        public TagManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
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
            if (prompt == null)
            {
                prompt = "Please choose a Tag";
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
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
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
            int nameMaxChar = 55;

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
            Tag tagToEdit = Choose("Which tag would you like to edit?");

            if (tagToEdit == null)
            {
                return;
            }
            tagEdit:
            Console.WriteLine();
            Console.Write("Please enter new tag name (Blank to leave unchanged): ");
            string tagName = Console.ReadLine();
            if (tagName.Length > 55 )
            {
                Console.WriteLine();
                Console.WriteLine("Tag name must be 55 characters or less.");
                Console.WriteLine();
                goto tagEdit;
            } else if (!string.IsNullOrEmpty(tagName))
            {
                tagToEdit.Name = tagName;
            }


            _tagRepository.Update(tagToEdit);
        }

        private void Remove()
        {
            Tag tagToDelete = Choose("Which tag would you like to delete?");
            if (tagToDelete != null)
            {
                _tagRepository.Delete(tagToDelete.Id);
                Console.WriteLine("Post has been removed.");

            }
            Console.WriteLine();
        }
    }
}
