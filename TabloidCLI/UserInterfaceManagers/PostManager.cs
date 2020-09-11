//Handles command-line inputs and displays options for modifying posts
//Erik Lindstrom
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using TabloidCLI.Models;
using TabloidCLI.Repositories;

namespace TabloidCLI.UserInterfaceManagers
{
    class PostManager : IUserInterfaceManager
    {
        private readonly IUserInterfaceManager _parentUI;
        private PostRepository _postRepository;
        private string _connectionString;

        public PostManager(IUserInterfaceManager parentUI,string connectionString)
        {
            _parentUI = parentUI;
            _postRepository = new PostRepository(connectionString);
            _connectionString = connectionString;
        }
        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Post Menu");
            Console.WriteLine(" 1) List Posts");
            Console.WriteLine(" 2) Post Details");
            Console.WriteLine(" 3) Add Post");
            Console.WriteLine(" 4) Edit Post");
            Console.WriteLine(" 5) Remove Post");
            Console.WriteLine(" 0) Go Back");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    List();
                    return this;
                case "2":
                    Post post = Choose();
                    if (post == null)
                    {
                        return this;
                    }
                    else
                    {
                        throw new NotImplementedException();
                        //return new PostDetailManager(this, _connectionString, post.Id);
                    }
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
        
        private void List()
        {
            List<Post> posts = _postRepository.GetAll();
            foreach (Post post in posts)
            {
                Console.WriteLine($"{ post.Title} by {post.Author.FirstName} {post.Author.LastName} at {post.Url} on {post.PublishDateTime.ToString()}");
            }
            Console.WriteLine("");
        }

        private Post Choose(string prompt = null)
        {
            if(prompt == null)
            {
                prompt = "Please choose a Post";
            }
            Console.WriteLine(prompt);

            List<Post> posts = _postRepository.GetAll();

            for (int i = 0; i <  posts.Count; i++)
            {
                Post post = posts[i];
                Console.WriteLine($" {i + 1}) {post.Title}");
            }
            Console.Write("> ");

            string input = Console.ReadLine();
            try
            {
                int choice = int.Parse(input);
                return posts[choice - 1];
            }
            catch (Exception ex)
            {
                Console.WriteLine("Invalid Selection");
                return null;
            }

        }

        private void Add()
        {
            Console.WriteLine("New Post");
            Post post = new Post();

            Console.Write("Title: ");
            Title:
            string title = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine();
                Console.WriteLine("Please Enter a title:");
                goto Title;
            }

            else if (title.Length> 55)
            {
                Console.WriteLine();
                Console.WriteLine("Title was too long. Please Enter a title:");
                goto Title;
            }
                post.Title = title;
            
            Console.Write("URL: ");
            Url:
            string url = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(url))
            {

                Console.WriteLine();
                Console.WriteLine("Please Enter a url:");
                goto Url;
            }
            else if (url.Length >2000)
            {

                Console.WriteLine();
                Console.WriteLine("Url was too long. Please Enter a url:");
                goto Url;
            }
            post.Url = url;

            bool badDate = true;
            while(badDate)
            {
                try
                {
                    Console.Write("Date published: ");
                    post.PublishDateTime = Convert.ToDateTime(Console.ReadLine());
                    badDate = false;

                }
                catch (Exception)
                {

                    Console.WriteLine("Date not accepted, please try again");
                }
            }

            
            

            AuthorManager authorManager= new AuthorManager(this, _connectionString);
            post.Author = authorManager.ChooseAuthor();
            post.Blog = new Blog()
            {
                Id = 1,
                Title = "New",
                Url = "google.com"
            };
            //BlogManager blogManager = new BlogManager(this, _connectionString);
            //post.Blog = blogManager.ChooseAuthor();
            
            _postRepository.Insert(post);
        }

        private void Edit()
        {
            Post postToEdit = Choose("Which post would you like to edit?");
            if (postToEdit == null) return;
            
            Console.WriteLine();
            EditTitle:
            Console.Write("New title for the post (blank to leave unchanged): ");
            string title = Console.ReadLine();
            if (title.Length > 55)
            {
                Console.WriteLine();
                Console.WriteLine("Title was too long. Please Enter a title:");
                goto EditTitle;
            }
            else if (!string.IsNullOrWhiteSpace(title))
            {
                postToEdit.Title = title;
            }

            Console.Write("New URL for the post (blank to leave unchanged): ");
            EditUrl:
            string url = Console.ReadLine();
            if (url.Length > 2000)
            {
                Console.WriteLine();
                Console.WriteLine("Url was too long. Please Enter a url:");
                goto EditUrl;
            }
            else if (!string.IsNullOrWhiteSpace(url))
            {
                postToEdit.Url = url;
            }

            Console.Write("New publishing date for the post (blank to leave unchanged): ");
            string datePublished = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(datePublished))
            {
                postToEdit.PublishDateTime = Convert.ToDateTime(datePublished); ;
            }
      

            AuthorManager authorManager = new AuthorManager(this, _connectionString);
            postToEdit.Author = authorManager.ChooseAuthor();

            //BlogManager blogManager = new BlogManager(this, _connectionString);
            //postToEdit.Blog = blogManager.ChooseAuthor();

            _postRepository.Update(postToEdit);

        }
        private void Remove()
        {
            Post postToDelete = Choose("Which post would you like to delete?");
                if (postToDelete != null)
            {
                _postRepository.Delete(postToDelete.Id);
                Console.WriteLine("Post has been removed.");

            }
            Console.WriteLine();
        }
    }
}
