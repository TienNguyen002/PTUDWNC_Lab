using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeders
{
    public class DataSeeder : IDataSeeder
    {
        private readonly BlogDbContext _dbContext;

        public DataSeeder(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Initialize()
        {
            _dbContext.Database.EnsureCreated();
            if (_dbContext.Posts.Any()) return;

            var authors = AddAuthors();
            var categories = AddCategories();
            var tags = AddTags();
            var posts = AddPosts(authors, categories, tags);
            var subscribers = AddSubscribers();
            var comments = AddComments(posts);
        }

        private IList<Author> AddAuthors() 
        {
            var authors = new List<Author>()
            {
                new()
                {
                    FullName = "Jason Mouth",
                    UrlSlug = "jason-mouth",
                    Email = "json@gmail.com",
                    JoinedDate = new DateTime(2022, 10, 21)
                },
                new()
                {
                    FullName = "Jessica Wonder",
                    UrlSlug = "jessica-wonder",
                    Email = "jessica665@motip.com",
                    JoinedDate = new DateTime(2020, 4, 19)
                },
                new()
                {
                    FullName = "John Wasson",
                    UrlSlug = "john-wasson",
                    Email = "johnwas098@gmail.com",
                    JoinedDate = new DateTime(2021, 12, 24)
                },
                new()
                {
                    FullName = "Alice Jessi",
                    UrlSlug = "alice-jessi",
                    Email = "jessalice22@gmail.com",
                    JoinedDate = new DateTime(2019, 4, 4)
                },
                new()
                {
                    FullName = "Mira John",
                    UrlSlug = "mira-john",
                    Email = "miraaaa2848@gmail.com",
                    JoinedDate = new DateTime(2023, 1, 8)
                },
            };
            foreach (var author in authors)
            {
                if (!_dbContext.Authors.Any(a => a.UrlSlug == author.UrlSlug))
                {
                    _dbContext.Authors.Add(author);
                }
            }
            _dbContext.SaveChanges();
            return authors;
        }

        private IList<Category> AddCategories() 
        {
            var categories = new List<Category>()
            {
                new()
                {
                    Name = ".NET Core",
                    Description = ".NET Core",
                    UrlSlug = "net-core",
                    ShowOnMenu = true
                },
                new()
                {
                    Name = "Architecture",
                    Description = "Architecture",
                    UrlSlug = "architecture",
                    ShowOnMenu = true
                },
                new()
                {
                    Name = "Messaging",
                    Description = "Messaging",
                    UrlSlug = "messaging",
                    ShowOnMenu = true
                },
                new()
                {
                    Name = "OOP",
                    Description = "Object-Oriented Programming",
                    UrlSlug = "oop",
                    ShowOnMenu = true
                },
                new()
                {
                    Name = "Design Patterns",
                    Description = "Design Patterns",
                    UrlSlug = "design-patterns",
                    ShowOnMenu = true
                },
                new()
                {
                    Name = "Front-End",
                    Description = "Front-End",
                    UrlSlug = "front-end",
                    ShowOnMenu = true
                },
                new()
                {
                    Name = "Python",
                    Description = "Python",
                    UrlSlug = "python",
                    ShowOnMenu = true
                },
                new()
                {
                    Name = "Java",
                    Description = "Java",
                    UrlSlug = "java",
                    ShowOnMenu = true
                },
                new()
                {
                    Name = "Backend",
                    Description = "Backend",
                    UrlSlug = "backend",
                    ShowOnMenu = true
                },
                new()
                {
                    Name = "Git/Github",
                    Description = "Git/Github",
                    UrlSlug = "git-github",
                    ShowOnMenu = true
                },
                new()
                {
                    Name = "Graphic Design",
                    Description = "Graphic Design",
                    UrlSlug = "graphic-design",
                    ShowOnMenu = true
                },
                new()
                {
                    Name = "C/C++",
                    Description = "C/C++",
                    UrlSlug = "c-cplusplus",
                    ShowOnMenu = true
                },
                new()
                {
                    Name = "Games",
                    Description = "Games",
                    UrlSlug = "games",
                    ShowOnMenu = true
                },
                new()
                {
                    Name = "Data Science",
                    Description = "Data Science",
                    UrlSlug = "data-science",
                    ShowOnMenu = true
                },
                new()
                {
                    Name = "Full-Stack",
                    Description = "Full-Stack",
                    UrlSlug = "full-stack",
                    ShowOnMenu = true
                },
            };
            foreach(var category in categories)
            {
                if (!_dbContext.Categories.Any(c => c.UrlSlug == category.UrlSlug))
                {
                    _dbContext.Categories.Add(category);
                }
            }
            _dbContext.SaveChanges();
            return categories;
        }

        private IList<Tag> AddTags() 
        {
            var tags = new List<Tag>()
            {
                new()
                {
                    Name = "Google",
                    Description = "Google applications",
                    UrlSlug = "google"
                },
                new()
                {
                    Name = "ASP.NET MVC",
                    Description = "ASP.NET MVC",
                    UrlSlug = "aspnet-mvc"
                },
                new()
                {
                    Name = "Razor Page",
                    Description = "Razor Page",
                    UrlSlug = "razor-page"
                },
                new()
                {
                    Name = "Blazor",
                    Description = "Blazor",
                    UrlSlug = "blazor"
                },
                new()
                {
                    Name = "Deep Learning",
                    Description = "Deep Learning",
                    UrlSlug = "deep-learning"
                },
                new()
                {
                    Name = "Neural Network",
                    Description = "Neural Network",
                    UrlSlug = "neural-network"
                },
                new()
                {
                    Name = "JavaScript",
                    Description = "JavaScript",
                    UrlSlug = "javascript"
                },
                new()
                {
                    Name = "React",
                    Description = "React",
                    UrlSlug = "react"
                },
                new()
                {
                    Name = "Python",
                    Description = "Python",
                    UrlSlug = "python"
                },
                new()
                {
                    Name = "Java",
                    Description = "Java",
                    UrlSlug = "java"
                },
                new()
                {
                    Name = "PHP",
                    Description = "PHP",
                    UrlSlug = "php"
                },
                new()
                {
                    Name = "NodeJs",
                    Description = "NodeJs",
                    UrlSlug = "nodejs"
                },
                new()
                {
                    Name = "Ruby",
                    Description = "Ruby",
                    UrlSlug = "ruby"
                },
                new()
                {
                    Name = "Git",
                    Description = "Git",
                    UrlSlug = "git"
                },
                new()
                {
                    Name = "Photoshop",
                    Description = "Photoshop",
                    UrlSlug = "photoshop"
                },
                new()
                {
                    Name = "Illustrator",
                    Description = "Illustrator",
                    UrlSlug = "illustrator"
                },
                new()
                {
                    Name = "C/C++",
                    Description = "C/C++",
                    UrlSlug = "c-cplusplus"
                },
                new()
                {
                    Name = "Games",
                    Description = "Games",
                    UrlSlug = "games"
                },
                new()
                {
                    Name = "C#",
                    Description = "C#",
                    UrlSlug = "c-sharp"
                },
                new()
                {
                    Name = "HTML",
                    Description = "HTML",
                    UrlSlug = "html"
                },
                new()
                {
                    Name = "CSS",
                    Description = "CSS",
                    UrlSlug = "css"
                },
                new()
                {
                    Name = "Data Science",
                    Description = "Data Science",
                    UrlSlug = "data-science"
                },
                new()
                {
                    Name = "Docker",
                    Description = "Docker",
                    UrlSlug = "docker"
                },
                new()
                {
                    Name = "Kubernetes",
                    Description = "Kubernetes",
                    UrlSlug = "kubernetes"
                },
                new()
                {
                    Name = "Machine Learning",
                    Description = "Machine Learning",
                    UrlSlug = "machine-learning"
                },
                new()
                {
                    Name = "Windows",
                    Description = "Windows",
                    UrlSlug = "windows"
                },
            };
            foreach (var tag in tags)
            {
                if (!_dbContext.Tags.Any(t => t.UrlSlug == tag.UrlSlug))
                {
                    _dbContext.Tags.Add(tag);
                }
            }
            _dbContext.SaveChanges();
            return tags;
        }

        private IList<Post> AddPosts(
            IList<Author> authors,
            IList<Category> categories,
            IList<Tag> tags)
        {
            var posts = new List<Post>()
            {
                new()
                {
                    Title = "ASP.NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repository filled",
                    Description = "Here's a few great DON'T and DO examples",
                    Meta = "David and friends has a great repository filled",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021,9,30,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                        tags[1]
                    }
                },
                new()
                {
                    Title = "Optimizing Software In C++",
                    ShortDescription = "This is an optimization manual for advanced C++ programmers",
                    Description = "This book are not for beginners",
                    Meta = "This is an optimization manual for advanced C++ programmers",
                    UrlSlug = "optimizing-software-in-cplusplus",
                    Published = true,
                    PostedDate = new DateTime(2020,8,21,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 17,
                    Author = authors[3],
                    Category = categories[11],
                    Tags = new List<Tag>()
                    {
                        tags[16]
                    }
                },
                new()
                {
                    Title = "2D Game Development: From Zero to Hero",
                    ShortDescription = "This is a small project that aims to gather some knowledge about game development",
                    Description = "Game Development",
                    Meta = "This is a small project that aims to gather some knowledge about game development",
                    UrlSlug = "2d-game-development-from-zero-to-hero",
                    Published = true,
                    PostedDate = new DateTime(2020,5,11,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 30,
                    Author = authors[2],
                    Category = categories[12],
                    Tags = new List<Tag>()
                    {
                        tags[17]
                    }
                },
                new()
                {
                    Title = "Ruby Regexp",
                    ShortDescription = "This book will help you learn Ruby Regular Expressions",
                    Description = "A magical tool for text processing",
                    Meta = "This book will help you learn Ruby Regular Expressions",
                    UrlSlug = "ruby-regexp",
                    Published = true,
                    PostedDate = new DateTime(2020,3,31,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 7,
                    Author = authors[1],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[12]
                    }
                },
                new()
                {
                    Title = "Libelf by Example",
                    ShortDescription = "This tutorial introduces libelf",
                    Description = "A tutorial introduction to the ELF(3) & GELF(3) APIs",
                    Meta = "This tutorial introduces libelf",
                    UrlSlug = "libeft-by-example",
                    Published = true,
                    PostedDate = new DateTime(2020,10,2,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 5,
                    Author = authors[3],
                    Category = categories[11],
                    Tags = new List<Tag>()
                    {
                        tags[16]
                    }
                },
                new()
                {
                    Title = "Biopython: Tutorial and Cookbook",
                    ShortDescription = "The Biopython Project is an international association of developers tools for Computational Molecular Biology",
                    Description = "Pyhon Tools for Computational Molecular Biology",
                    Meta = "The Biopython Project is an international association of developers tools for Computational Molecular Biology",
                    UrlSlug = "biopython-tutorial-and-cookbook",
                    Published = true,
                    PostedDate = new DateTime(2020,4,21,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 19,
                    Author = authors[4],
                    Category = categories[6],
                    Tags = new List<Tag>()
                    {
                        tags[8]
                    }
                },
                new()
                {
                    Title = "C++ Today",
                    ShortDescription = "C++ is a complicated beast that's not easy to learn",
                    Description = "The Beast is Back",
                    Meta = "C++ is a complicated beast that's not easy to learn",
                    UrlSlug = "cplusplus-today",
                    Published = true,
                    PostedDate = new DateTime(2015,7,12,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 27,
                    Author = authors[3],
                    Category = categories[11],
                    Tags = new List<Tag>()
                    {
                        tags[16]
                    }
                },
                new()
                {
                    Title = "PHPUnit Manual",
                    ShortDescription = "PHPUnit is a unit testing framework for the PHP programming language",
                    Description = "PHPUnit 9.1",
                    Meta = "PHPUnit is a unit testing framework for the PHP programming language",
                    UrlSlug = "phpunit-manual",
                    Published = true,
                    PostedDate = new DateTime(2020,2,27,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 16,
                    Author = authors[2],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new()
                {
                    Title = "The JavaScript Way",
                    ShortDescription = "Love it or hate it, JavaScript is avidly eating the world of software development",
                    Description = "A gentle introduction to an essential language",
                    Meta = "Love it or hate it, JavaScript is avidly eating the world of software development",
                    UrlSlug = "the-javascript-way",
                    Published = true,
                    PostedDate = new DateTime(2020,9,18,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 30,
                    Author = authors[2],
                    Category = categories[5],
                    Tags = new List<Tag>()
                    {
                        tags[6]
                    }
                },
                new()
                {
                    Title = "JavaScript Notes for Professionals",
                    ShortDescription = "The JavaScript Notes for Professionals book is compliled from Stack Overflow Documentation",
                    Description = "Professional JavaScript",
                    Meta = "The JavaScript Notes for Professionals book is compliled from Stack Overflow Documentation",
                    UrlSlug = "javascript-notes-for-professionals",
                    Published = true,
                    PostedDate = new DateTime(2019,4,12,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[0],
                    Category = categories[5],
                    Tags = new List<Tag>()
                    {
                        tags[6]
                    }
                },
                new()
                {
                    Title = "Learn Ruby on Rails: Book One",
                    ShortDescription = "This book contains the background that's missing from ruby tutorials",
                    Description = "Ruby on Rails",
                    Meta = "This book contains the background that's missing from ruby tutorials",
                    UrlSlug = "learn-ruby-on-rails-book-one",
                    Published = true,
                    PostedDate = new DateTime(2016,1,30,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 22,
                    Author = authors[1],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[12]
                    }
                },
                new()
                {
                    Title = "Learn Ruby on Rails: Book Two",
                    ShortDescription = "In this book, you'll build a working web application so you'll gain hands-on experience",
                    Description = "Hands-on learning",
                    Meta = "In this book, you'll build a working web application so you'll gain hands-on experience",
                    UrlSlug = "learn-ruby-on-rails-book-two",
                    Published = true,
                    PostedDate = new DateTime(2017,4,30,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 23,
                    Author = authors[1],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[12]
                    }
                },
                new()
                {
                    Title = "Kubernetes for Full-Stack Developers",
                    ShortDescription = "Getting started with Kubernetes",
                    Description = "Learn set up monitoring, alerting, automation for application on Kubernetes",
                    Meta = "Getting started with Kubernetes",
                    UrlSlug = "kubernetes-for-full-stack-developers",
                    Published = true,
                    PostedDate = new DateTime(2020,10,15,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[4],
                    Category = categories[14],
                    Tags = new List<Tag>()
                    {
                        tags[22],
                        tags[23]
                    }
                },
                new()
                {
                    Title = "Acceptance Test Driven Development with React",
                    ShortDescription = "This book describes how to apply the Acceptance Test Driven Development when developing a Web Application named bookish with React",
                    Description = "This book describes how to apply the Acceptance Test Driven Development when developing a Web Application named bookish with React",
                    Meta = "This book describes how to apply the Acceptance Test Driven Development when developing a Web Application named bookish with React",
                    UrlSlug = "acceptance-test-driven-development-with-react",
                    Published = true,
                    PostedDate = new DateTime(2021,9,30,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 14,
                    Author = authors[2],
                    Category = categories[14],
                    Tags = new List<Tag>()
                    {
                        tags[6],
                        tags[7]
                    }
                },
                new()
                {
                    Title = "Java Everywhere Again with DukeScript",
                    ShortDescription = "Java Everywhere Again with DukeScript",
                    Description = "Java Everywhere Again with DukeScript",
                    Meta = "Java Everywhere Again with DukeScript",
                    UrlSlug = "java-everywhere-again-with-dukescript",
                    Published = true,
                    PostedDate = new DateTime(2015,8,22,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 72,
                    Author = authors[1],
                    Category = categories[7],
                    Tags = new List<Tag>()
                    {
                        tags[9]
                    }
                },
                new()
                {
                    Title = "Learning Node.js",
                    ShortDescription = "Node.js is an event-based, non-blocking, asynchronous I/O framework that uses Google's V8 JavaScript engine",
                    Description = "Node.js applications are written in pure JavaScript and can be run within Node.js environment on Windows, Linux etc.",
                    Meta = "Node.js is an event-based, non-blocking, asynchronous I/O framework that uses Google's V8 JavaScript engine",
                    UrlSlug = "learning-nodejs",
                    Published = true,
                    PostedDate = new DateTime(2019,11,3,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[1],
                    Category = categories[14],
                    Tags = new List<Tag>()
                    {
                        tags[6],
                        tags[11]
                    }
                },
                new()
                {
                    Title = "ASP.NET Core 2",
                    ShortDescription = " ASP.NET Core 2 is a key component of that progress, delivering modularity, better performance, and flexibility to web development",
                    Description = "How to build projects using the framework, and how to deploy apps to IIS or Azure",
                    Meta = " ASP.NET Core 2 is a key component of that progress, delivering modularity, better performance, and flexibility to web development",
                    UrlSlug = "aspnet-core-2",
                    Published = true,
                    PostedDate = new DateTime(2019,9,30,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 14,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                        tags[1]
                    }
                },
                new()
                {
                    Title = "Git Notes for Professionals",
                    ShortDescription = "The Git Notes for Professionals book is compiled from Stack Overflow Documentation",
                    Description = "Git Learning",
                    Meta = "The Git Notes for Professionals book is compiled from Stack Overflow Documentation",
                    UrlSlug = "git-notes-for-professionals",
                    Published = true,
                    PostedDate = new DateTime(2019,5,21,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 14,
                    Author = authors[1],
                    Category = categories[9],
                    Tags = new List<Tag>()
                    {
                        tags[13]
                    }
                },
                new()
                {
                    Title = "Front-end Developer Handbook",
                    ShortDescription = "This is a guide that everyone can use to learn about the practice of front-end development",
                    Description = "HTML, CSS, DOM, and JavaScript",
                    Meta = "This is a guide that everyone can use to learn about the practice of front-end development",
                    UrlSlug = "front-end-developer-handbook",
                    Published = true,
                    PostedDate = new DateTime(2019,3,22,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 51,
                    Author = authors[1],
                    Category = categories[5],
                    Tags = new List<Tag>()
                    {
                        tags[6],
                        tags[19],
                        tags[20]
                    }
                },
                new()
                {
                    Title = "Learning Java",
                    ShortDescription = "Java is a class-based, object-oriented programming language that is designed to have as few implementation dependencies as possible",
                    Description = "Java Learning",
                    Meta = "Java is a class-based, object-oriented programming language that is designed to have as few implementation dependencies as possible",
                    UrlSlug = "learning-java",
                    Published = true,
                    PostedDate = new DateTime(2019,7,12,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 41,
                    Author = authors[3],
                    Category = categories[7],
                    Tags = new List<Tag>()
                    {
                        tags[9]
                    }
                },
                new()
                {
                    Title = "Kubernetes Patterns",
                    ShortDescription = "The way developers design, build, and run software has changed significantly with the evolution of microservices and containers",
                    Description = "Reusable Elements for Designing Cloud-Native Applications",
                    Meta = "The way developers design, build, and run software has changed significantly with the evolution of microservices and containers",
                    UrlSlug = "kubernetes-patterns",
                    Published = true,
                    PostedDate = new DateTime(2019,6,20,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 11,
                    Author = authors[1],
                    Category = categories[14],
                    Tags = new List<Tag>()
                    {
                        tags[23]
                    }
                },
                new()
                {
                    Title = "Games and Rules",
                    ShortDescription = "Why do we play games and why do we play them on computers? The contributors of Games and Rules take a closer look at the core of each game and the motivational system that is the game mechanics.",
                    Description = "Game Mechanics for the Magic Circle",
                    Meta = "Why do we play games and why do we play them on computers? The contributors of Games and Rules take a closer look at the core of each game and the motivational system that is the game mechanics.",
                    UrlSlug = "games-and-rules",
                    Published = true,
                    PostedDate = new DateTime(2019,1,2,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 42,
                    Author = authors[4],
                    Category = categories[12],
                    Tags = new List<Tag>()
                    {
                        tags[17]
                    }
                },
                new()
                {
                    Title = "Learning React Native",
                    ShortDescription = "React Native is an open-source mobile application framework created by Facebook.",
                    Description = "React Native",
                    Meta = "React Native is an open-source mobile application framework created by Facebook.",
                    UrlSlug = "learning-react-native",
                    Published = true,
                    PostedDate = new DateTime(2019,9,22,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 31,
                    Author = authors[1],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[7]
                    }
                },
                new()
                {
                    Title = "C# Notes for Professionals",
                    ShortDescription = "The C# Notes for Professionals book is compiled from Stack Overflow Documentation",
                    Description = "C#",
                    Meta = "The C# Notes for Professionals book is compiled from Stack Overflow Documentation",
                    UrlSlug = "csharp-notes-for-professionals",
                    Published = true,
                    PostedDate = new DateTime(2019,6,11,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 12,
                    Author = authors[0],
                    Category = categories[3],
                    Tags = new List<Tag>()
                    {
                        tags[18]
                    }
                },
                new()
                {
                    Title = "Gameplay, Emotions and Narrative",
                    ShortDescription = "This book is devoted to emotional and narrative immersion in the experience of gameplay.",
                    Description = "Independent Games Experienced",
                    Meta = "This book is devoted to emotional and narrative immersion in the experience of gameplay.",
                    UrlSlug = "gameplay-emotions-and-narrative",
                    Published = true,
                    PostedDate = new DateTime(2019,5,27,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 15,
                    Author = authors[0],
                    Category = categories[12],
                    Tags = new List<Tag>()
                    {
                        tags[17]
                    }
                },
                new()
                {
                    Title = "Introduction to Data Science",
                    ShortDescription = "The demand for skilled data science practitioners in industry, academia, and government is rapidly growing.",
                    Description = "Data Analysis and Prediction Algorithms with R",
                    Meta = "The demand for skilled data science practitioners in industry, academia, and government is rapidly growing.",
                    UrlSlug = "introduction-to-data-science",
                    Published = true,
                    PostedDate = new DateTime(2019,9,30,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 13,
                    Author = authors[1],
                    Category = categories[13],
                    Tags = new List<Tag>()
                    {
                        tags[21]
                    }
                },
                new()
                {
                    Title = "HTML5 Canvas Notes for Professionals",
                    ShortDescription = "The HTML5 Canvas Notes for Professionals book",
                    Description = "HTML5 Canvas",
                    Meta = "The HTML5 Canvas Notes for Professionals book",
                    UrlSlug = "html5-canvas-notes-for-professionals",
                    Published = true,
                    PostedDate = new DateTime(2019,3,20,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 11,
                    Author = authors[1],
                    Category = categories[5],
                    Tags = new List<Tag>()
                    {
                        tags[19]
                    }
                },
                new()
                {
                    Title = "Code Club Book of Scratch",
                    ShortDescription = "The first ever Code Club book is here! With it, you'll learn how to code using Scratch, the block-based programming language",
                    Description = "Simple coding for total beginners",
                    Meta = "The first ever Code Club book is here! With it, you'll learn how to code using Scratch, the block-based programming language",
                    UrlSlug = "code-clug-book-of-scratch",
                    Published = true,
                    PostedDate = new DateTime(2018,9,11,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 20,
                    Author = authors[0],
                    Category = categories[12],
                    Tags = new List<Tag>()
                    {
                        tags[17]
                    }
                },
                new()
                {
                    Title = "Coding with Minecraft",
                    ShortDescription = "In Coding with Minecraft, you'll create a virtual robot army with Lua, a programming language used by professional game developers.",
                    Description = "Build Taller, Farm Faster, Mine Deeper, and Automate the Boring Stuff",
                    Meta = "In Coding with Minecraft, you'll create a virtual robot army with Lua, a programming language used by professional game developers.",
                    UrlSlug = "coding-with-minecraft",
                    Published = true,
                    PostedDate = new DateTime(2018,9,12,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 4,
                    Author = authors[4],
                    Category = categories[12],
                    Tags = new List<Tag>()
                    {
                        tags[17]
                    }
                },
                new()
                {
                    Title = "PHP Notes for Professionals",
                    ShortDescription = "The PHP Notes for Professionals book ",
                    Description = "PHP Notes",
                    Meta = "The PHP Notes for Professionals book ",
                    UrlSlug = "php-notes-for-professionals",
                    Published = true,
                    PostedDate = new DateTime(2018,12,20,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[0],
                    Category = categories[8],
                    Tags = new List<Tag>()
                    {
                        tags[10]
                    }
                },
                new()
                {
                    Title = "Python Notes for Professionals",
                    ShortDescription = "The Python Notes for Professionals book",
                    Description = "Python Notes",
                    Meta = "The Python Notes for Professionals book",
                    UrlSlug = "python-notes-for-professionals",
                    Published = true,
                    PostedDate = new DateTime(2018,10,1,10,20,0),
                    ModifiedDate = null,
                    ViewCount = 10,
                    Author = authors[4],
                    Category = categories[6],
                    Tags = new List<Tag>()
                    {
                        tags[8]
                    }
                },
            };
            foreach (var post in posts)
            {
                if (!_dbContext.Posts.Any(a => a.UrlSlug == post.UrlSlug))
                {
                    _dbContext.Posts.Add(post);
                }
            }
            _dbContext.SaveChanges();
            return posts;
        }

        private IList<Subscriber> AddSubscribers()
        {
            var subscribers = new List<Subscriber>() 
            {
                new()
                {
                    Email = "tiennguyenn002@gmail.com",
                    SubscribeDate = new DateTime(2023, 3, 22, 12, 22, 0),
                },
                new()
                {
                    Email = "hieupor01@gmail.com",
                    SubscribeDate = new DateTime(2023, 3, 22, 12, 22, 0),
                },
                new()
                {
                    Email = "hoanglong6622@gmail.com",
                    SubscribeDate = new DateTime(2023, 3, 22, 12, 22, 0),
                },
                new()
                {
                    Email = "duattran00@gmail.com",
                    SubscribeDate = new DateTime(2023, 3, 22, 12, 22, 0),
                },
                new()
                {
                    Email = "xuanhung01@gmail.com",
                    SubscribeDate = new DateTime(2023, 3, 22, 12, 22, 0),
                },
                new()
                {
                    Email = "minhtienne@gmail.com",
                    SubscribeDate = new DateTime(2023, 3, 22, 12, 22, 0),
                },
            };
            foreach (var subscriber in subscribers)
            {
                if (!_dbContext.Subscribers.Any(s => s.Email == subscriber.Email))
                {
                    _dbContext.Subscribers.Add(subscriber);
                }
            }
            _dbContext.SaveChanges();
            return subscribers;
        }

        private IList<Comment> AddComments(IList<Post> posts)
        {
            var comments = new List<Comment>() 
            {
                new()
                {
                    Email = "tiennguyenn002@gmail.com",
                    Username = "Tien Nguyen",
                    Post = posts[1],
                    DateComment = new DateTime(2023,3,22,12,22,0),
                    Content = "Cac ban nen doc bai viet nay, bai viet chi rat tan tinh",
                },
                new()
                {
                    Email = "minhtienne@gmail.com",
                    Username = "Minh Tien",
                    Post = posts[1],
                    DateComment = new DateTime(2023,3,22,12,22,0),
                    Content = "Toi thay bai nay khong nen doc",
                },
            };
            foreach (var comment in comments)
            {
                if (!_dbContext.Comments
                  .Any(c => c.Content == comment.Content
                    && c.Email == comment.Email
                    && c.PostId == comment.PostId))
                {
                    _dbContext.Comments.Add(comment);
                }
            }
            _dbContext.SaveChanges();
            return comments;
        }
    }
}
