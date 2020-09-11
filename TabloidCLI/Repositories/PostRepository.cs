using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using TabloidCLI.Models;

namespace TabloidCLI.Repositories
{
    public class PostRepository : DatabaseConnector, IRepository<Post>
    {
        public PostRepository(string connectionString) : base(connectionString) { }

        public List<Post> GetAll()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT p.Id, p.Title, p.URL, p.AuthorId, p.BlogId, 
                            a.FirstName, a.LastName, a.Bio, 
                            b.Title, b.URL as BlogURL
                        FROM Post p
                        LEFT JOIN Author a ON p.AuthorId = a.Id
                        LEFT JOIN Blog b ON p.BlogId = b.Id";

                    SqlDataReader reader = cmd.ExecuteReader();
                    List<Post> posts = new List<Post>();

                    while (reader.Read())
                    {
                        Blog blog = new Blog
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("BlogId")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Url = reader.GetString(reader.GetOrdinal("BlogURL"))
                        };

                        Author author = new Author
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            Bio = reader.GetString(reader.GetOrdinal("Bio"))
                        };

                        Post post = new Post()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Url = reader.GetString(reader.GetOrdinal("URL")),
                            Blog = blog,
                            Author = author
                        };
                        posts.Add(post);
                    }
                    reader.Close();
                    return posts;
                }
            }
        }

        public Post Get(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT p.Id, p.Title, p.URL, p.AuthorId, p.BlogId, 
                            a.FirstName, a.LastName, a.Bio, 
                            b.Title, b.URL as BlogURL
                        FROM Post p
                        LEFT JOIN Author a ON p.AuthorId = a.Id
                        LEFT JOIN Blog b ON p.BlogId = b.Id
                        WHERE p.id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    Post post = null;
                    SqlDataReader reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        if( post == null)
                        {
                            Blog blog = new Blog
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BlogId")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Url = reader.GetString(reader.GetOrdinal("BlogURL"))
                            };

                            Author author = new Author
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Bio = reader.GetString(reader.GetOrdinal("Bio"))
                            };

                            post = new Post()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Url = reader.GetString(reader.GetOrdinal("URL")),
                                Blog = blog,
                                Author = author
                            };
                        }

                        //if(!reader.IsDBNull(reader.GetOrdinal("TagId")))
                        //{
                        //    post.Tags.Add(new Tag()
                        //    {
                        //        Id = reader.GetInt32(reader.GetOrdinal("TagId")),
                        //        Name = reader.GetString(reader.GetOrdinal("Name")),
                        //    });
                        //}
                    }
                    reader.Close();
                    return post;
                }
            }
        }

        public List<Post> GetByAuthor(int authorId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"SELECT p.id,
                                               p.Title As PostTitle,
                                               p.URL AS PostUrl,
                                               p.PublishDateTime,
                                               p.AuthorId,
                                               p.BlogId,
                                               a.FirstName,
                                               a.LastName,
                                               a.Bio,
                                               b.Title AS BlogTitle,
                                               b.URL AS BlogUrl
                                          FROM Post p 
                                               LEFT JOIN Author a on p.AuthorId = a.Id
                                               LEFT JOIN Blog b on p.BlogId = b.Id 
                                         WHERE p.AuthorId = @authorId";
                    cmd.Parameters.AddWithValue("@authorId", authorId);
                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Post> posts = new List<Post>();
                    while (reader.Read())
                    {
                        Post post = new Post()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Title = reader.GetString(reader.GetOrdinal("PostTitle")),
                            Url = reader.GetString(reader.GetOrdinal("PostUrl")),
                            PublishDateTime = reader.GetDateTime(reader.GetOrdinal("PublishDateTime")),
                            Author = new Author()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("AuthorId")),
                                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                Bio = reader.GetString(reader.GetOrdinal("Bio")),
                            },
                            Blog = new Blog()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("BlogId")),
                                Title = reader.GetString(reader.GetOrdinal("BlogTitle")),
                                Url = reader.GetString(reader.GetOrdinal("BlogUrl")),
                            }
                        };
                        posts.Add(post);
                    }

                    reader.Close();

                    return posts;
                }
            }
        }

        public void Insert(Post post)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"INSERT INTO Post (Title, URL, PublishDateTime, BlogId, AuthorId)
                                            VALUES(@title, @url, @publishDateTime, @blogId, @authorId)";
                    cmd.Parameters.AddWithValue("@title", post.Title);
                    cmd.Parameters.AddWithValue("@url",post.Url);
                    cmd.Parameters.AddWithValue("@publishDateTime", post.PublishDateTime);
                    cmd.Parameters.AddWithValue("@blogId",post.Blog.Id);
                    cmd.Parameters.AddWithValue("@authorId",post.Author.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Post post)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Post 
                                           SET Title = @title, 
                                                Url = @url, 
                                                PublishDateTime = @publishDateTime, 
                                                BlogId = @blogId, 
                                                AuthorId = @authorId
                                         WHERE id = @id";

                    cmd.Parameters.AddWithValue("@title", post.Title);
                    cmd.Parameters.AddWithValue("@url", post.Url);
                    cmd.Parameters.AddWithValue("@publishDateTime", post.PublishDateTime);
                    cmd.Parameters.AddWithValue("@blogId", post.Blog.Id);
                    cmd.Parameters.AddWithValue("@authorId", post.Author.Id);
                    cmd.Parameters.AddWithValue("@id", post.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"DELETE FROM POST WHERE id = @id";
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
