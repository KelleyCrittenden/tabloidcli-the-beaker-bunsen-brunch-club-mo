using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    // BlogManager class inherits from IUserInterfaceManager base class
    public class BlogManager : IUserInterfaceManager
    {
        // Declaring private variables
        private readonly IUserInterfaceManager _parentUI;
        private BlogRepository _blogRepository;
        private string _connectionString;

        // Constructor takes in parentUI and connectionString as parameters and instantiates private variables
        public BlogManager(IUserInterfaceManager parentUI, string connectionString)
        {
            _parentUI = parentUI;
            _blogRepository = new BlogRepository(connectionString);
            _connectionString = connectionString;
        }

        // Execute() method inherited from base class displays blog management menu options
        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Blog Menu");
            Console.WriteLine(" 1) List Blogs");
            Console.WriteLine(" 2) Blog Details");
            Console.WriteLine(" 3) Add Blog");
            Console.WriteLine(" 4) Edit Blog");
            Console.WriteLine(" 5) Remove Blog");
            Console.WriteLine(" 0) Go Back");

            Console.WriteLine("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    return this;
                case "3":
                    Add();
                    return this;
                case "4":
                    Edit();
                    return this;
                case "5":
                    Remove();
                    return this;
                case "0":
                    return _parentUI;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }

        // Method invokes the GetAll() method from BlogRepository class and displays list of blogs (title and url)
        private void List()
        {
            List<Blog> blogs = _blogRepository.GetAll();
            foreach (Blog blog in blogs)
            {
                Console.WriteLine(blog);
            }
        }

        // Method displays list of blogs for user to choose and returns selected blog, accepts optional prompt parameter or displays default
        private Blog Choose(string prompt = null)
        {
            ChooseBlog:
            if (prompt == null)
            {
                prompt = "Please choose a blog:";
            }

            Console.WriteLine(prompt);

            List<Blog> blogs = _blogRepository.GetAll();

            for (int i = 0; i < blogs.Count; i++)
            {
                Blog blog = blogs[i];
                Console.WriteLine($" {i + 1}) {blog.Title} {blog.Url}");
            }

            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return blogs[choice - 1];
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid Selection");
                goto ChooseBlog;
            }
        }

        // Method creates a new blog object using user's input and invokes Insert() method in BlogRepository class to insert new blog to database
        private void Add()
        {
            Console.WriteLine("New Blog");
            Blog blog = new Blog();
            int titleMaxChar = 5;
            int urlMaxChar = 5;

            BlogTitle:
            Console.Write("Blog Title: ");
            blog.Title = Console.ReadLine();
            if (blog.Title == "")
            {
                Console.WriteLine("Please enter a blog title");
                goto BlogTitle;
            }
            else if (blog.Title.Length > titleMaxChar)
            {
                Console.WriteLine($"Title is too long, please limit to {titleMaxChar} characters");
                goto BlogTitle;
            }

            BlogURL:
            Console.Write("Blog URL: ");
            blog.Url = Console.ReadLine();
            if (blog.Url == "")
            {
                Console.WriteLine("Please enter a blog url");
                goto BlogURL;
            }
            else if (blog.Url.Length > urlMaxChar)
            {
                Console.WriteLine($"URL is too long, please limit to {urlMaxChar} characters");
                goto BlogURL;
            }

            _blogRepository.Insert(blog);
        }

        // Method provides user list of blogs to edit, checks to see if input field is empty (meaning leave field unchanged) then invokes the Update() method in BlogRepository class 
        private void Edit()
        {
            Blog blogToEdit = Choose("Which blog would you like to edit?");
            int titleMaxChar = 5;
            int urlMaxChar = 5;

            if (blogToEdit == null)
            {
                return;
            }

            Console.WriteLine();
            EditTitle:
            Console.Write("New blog title (blank to leave unchanged): ");
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
            {
                if (title.Length > titleMaxChar)
                {
                    Console.WriteLine($"Title is too long, please limit to {titleMaxChar} characters");
                    goto EditTitle;
                }
                blogToEdit.Title = title;
            }

            EditURL:
            Console.Write("New blog url (blank to leave unchanged): ");
            string url = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(url))
            {
                if (url.Length > urlMaxChar)
                {
                    Console.WriteLine($"URL is too long, please limit to {urlMaxChar} characters");
                    goto EditURL;
                }
                blogToEdit.Url = url;
            }

            _blogRepository.Update(blogToEdit);
        }

        // Method displays list of blogs for removal, invokes Delete() method in BlogRepository class
        private void Remove()
        {
            Blog blogToDelete = Choose("Which blog would you like to remove?");
            if (blogToDelete != null)
            {
                _blogRepository.Delete(blogToDelete.Id);
            }
        }

        // Public Choose() method for Post component
        public Blog ChoosePost(string prompt = null)
        {
            return Choose(prompt);
        }
    }
}
