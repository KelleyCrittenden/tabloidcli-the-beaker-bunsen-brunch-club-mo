using System;
using System.Collections.Generic;
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
                Console.WriteLine(post.Title);
                //Need more console logging?
            }
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
            post.Title = Console.ReadLine();
            Console.Write("URL: ");
            post.Url = Console.ReadLine();
            Console.Write("Published: ");
            post.PublishDateTime = Convert.ToDateTime(Console.ReadLine());

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
            Console.Write("New title for the post (blank to leave unchanged): ");
            string title = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(title))
            {
                postToEdit.Title = title;
            }
            Console.Write("New URL for the post (blank to leave unchanged): ");
            string url = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(url))
            {
                postToEdit.Url = url;
            }
                Console.Write("It's empty");
            Console.Write("New publishing date for the post (blank to leave unchanged): ");
            string datePublished = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(datePublished))
            {
                postToEdit.PublishDateTime = Convert.ToDateTime(datePublished); ;
            }
                Console.Write("It's empty");

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
