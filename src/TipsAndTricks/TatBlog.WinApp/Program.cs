using Mapster;
using TatBlog.Core.Constants;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.WinApp;
using static Azure.Core.HttpHeader;

//Tạo đối tượng DbContext để quản lý phiên làm việc với CSDL và trạng thái của các đối tượng
var context = new BlogDbContext();

//Tạo đối tượng khởi tạo dữ liệu mẫu
var seeder = new DataSeeder(context);

//Gọi hàm Inititalize để nhập dữ liệu mẫu
seeder.Initialize();

//Tạo đối tượng BlogRepository
IBlogRepository blogRepo = new BlogRepository(context);
IAuthorRepository authorRepo = new AuthorRepository(context);

#region Tag
#region GetTagBySlug
//var tag = await blogRepo.GetTagBySlugAsync("google");
//Console.WriteLine("{0,-5}{1,-20}{2,-50}",
//    "ID", "Tag", "Slug");
//Console.WriteLine("{0,-5}{1,-20}{2,-50}",
//    tag.Id, tag.Name, tag.UrlSlug);
#endregion

#region GetAllTags
//var tags = await blogRepo.GetAllTagsAsync();
//Console.WriteLine("{0,-5}{1,-50}{2,10}",
//    "ID", "Name", "Count");
//foreach (var tag in tags)
//    Console.WriteLine("{0,-5}{1,-50}{2,10}",
//        tag.Id, tag.Name, tag.PostCount);
#endregion

#region DeleteTagById
//await blogRepo.DeleteTagByIdAsync(2);
#endregion

#region Page
////Tạo đối tượng chứa tham số phân trang
//var pagingParams = new PagingParams
//{
//    PageNumber = 1,         //Lấy kết quả ở trang số 1
//    PageSize = 5,           //Lấy 5 mẫu tin
//    SortColumn = "Name",    //Sắp xếp theo tên
//    SortOrder = "DESC"      //Theo chiều giảm dần
//};

////Lấy danh sách từ khóa
//var tagsList = await blogRepo.GetPagedTagsAsync(pagingParams);

//Console.WriteLine("{0,-5}{1,-50}{2,10}",
//    "ID", "Name", "Count");

//foreach(var item in tagsList)
//{
//    Console.WriteLine("{0,-5}{1,-50}{2,10}",
//    item.Id, item.Name, item.PostCount);
//}
#endregion
#endregion

#region Category
#region CategoryList
////Lấy danh sách chuyên mục
//var categories = await blogRepo.GetCategoriesAsync();

//Console.WriteLine("{0,-5}{1,-50}{2,10}",
//    "ID", "Name", "Count");

//foreach(var item in categories)
//{
//    Console.WriteLine("{0,-5}{1,-50}{2,10}",
//    item.Id, item.Name, item.PostCount);
//}
#endregion

#region GetCategoryBySlug
//var category = await blogRepo.GetCategoryBySlugAsync("net-core");
//Console.WriteLine("{0,-5}{1,-20}{2,-50}",
//    "ID", "Category", "Slug");
//Console.WriteLine("{0,-5}{1,-20}{2,-50}",
//    category.Id, category.Name, category.UrlSlug);
#endregion

#region GetCategoryById
//var category = await blogRepo.GetCategoryByIdAsync(1);
//Console.WriteLine("{0,-5}{1,-20}{2,-20}{3,-50}",
//    "ID", "Category", "Description", "Slug");
//Console.WriteLine("{0,-5}{1,-20}{2,-20}{3,-50}",
//    category.Id, category.Name, category.Description, category.UrlSlug);
#endregion

#region AddOrUpdateCategory
//Category categoryAdd = new()
//{
//    Name = "Internet",
//    UrlSlug = "internet",
//    Description = "Mang may tinh",
//    ShowOnMenu = true
//};

//Category categoryUpdate = new()
//{
//    Id = 10,
//    Name = "Git",
//    UrlSlug = "git",
//    Description = "Git",
//    ShowOnMenu = true
//};

//await blogRepo.AddOrUpdateCategoryAsync(categoryAdd);
//await blogRepo.AddOrUpdateCategoryAsync(categoryUpdate);
#endregion

#region DeleteCategory
//await blogRepo.DeleteCategoryByIdAsync(16);
#endregion

#region Page
////Tạo đối tượng chứa tham số phân trang
//var pagingParams = new PagingParams
//{
//    PageNumber = 3,         //Lấy kết quả ở trang số 3
//    PageSize = 5,           //Lấy 5 mẫu tin
//    SortColumn = "Name",    //Sắp xếp theo tên
//    SortOrder = "DESC"      //Theo chiều giảm dần
//};

////Lấy danh sách từ khóa
//var categoriesList = await blogRepo.GetPagedCategoriesAsync(pagingParams);

//Console.WriteLine("{0,-5}{1,-20}{2,-20}{3,-50}{4,10}",
//    "ID", "Name", "UrlSlug", "Description", "Count");

//foreach (var item in categoriesList)
//{
//    Console.WriteLine("{0,-5}{1,-20}{2,-20}{3,-50}{4,10}",
//    item.Id, item.Name, item.UrlSlug, item.Description, item.PostCount);
//}
#endregion
#endregion

#region Post
#region PostList 
////Đọc danh sách bài viết từ cơ sở dữ liệu
////Lấy kèm tên tác giả và chuyên mục
//var posts = context.Posts
//    .Where(p => p.Published)
//    .OrderBy(p => p.Title)
//    .Select(p => new
//    {
//        Id = p.Id,
//        Title = p.Title,
//        ViewCount = p.ViewCount,
//        PostedDate = p.PostedDate,
//        Author = p.Author.FullName,
//        Category = p.Category.Name,
//    })
//    .ToList();

//foreach(var post in posts)
//{
//    Console.WriteLine("ID       : {0}", post.Id);
//    Console.WriteLine("Title    : {0}", post.Title);
//    Console.WriteLine("View     : {0}", post.ViewCount);
//    Console.WriteLine("Date     : {0:MM/dd/yyyy}", post.PostedDate);
//    Console.WriteLine("Author   : {0}", post.Author);
//    Console.WriteLine("Category : {0}", post.Category);
//    Console.WriteLine("".PadRight(80,'-'));
//}
#endregion

#region 3_Popular_Posts
////Tìm 3 bài viết được xem/đọc nhiều nhất
//var posts = await blogRepo.GetPopularArticlesAsync(3);

//foreach(var post in posts)
//{
//    Console.WriteLine("ID       : {0}", post.Id);
//    Console.WriteLine("Title    : {0}", post.Title);
//    Console.WriteLine("View     : {0}", post.ViewCount);
//    Console.WriteLine("Date     : {0:MM/dd/yyyy}", post.PostedDate);
//    Console.WriteLine("Author   : {0}", post.Author.FullName);
//    Console.WriteLine("Category : {0}", post.Category.Name);
//    Console.WriteLine("".PadRight(80, '-'));
//}
#endregion

#region GetPostInNMonth
//var posts = await blogRepo.GetPostInNMonthAsync(2);
//Console.WriteLine("{0,-10}{1,-10}{2,10}",
//    "Year", "Month", "Count");

//foreach (var item in posts)
//{
//    Console.WriteLine("{0,-10}{1,-10}{2,10}",
//    item.Year, item.Month, item.PostCount);
//}
#endregion

#region GetPostById
//var post = await blogRepo.GetPostByIdAsync(1);
//Console.WriteLine("{0,-5}{1,-50}{2,-70}{3,-50}",
//    "ID", "Title", "Description", "Slug");
//Console.WriteLine("{0,-5}{1,-50}{2,-70}{3,-50}",
//    post.Id, post.Title, post.ShortDescription, post.UrlSlug);
#endregion

#region AddOrUpdatePost
//Post postUpdate = new Post()
//        {
//            Id = 7,
//            Title = "C++",
//            ShortDescription = "C++ is not easy to learn",
//            Description = "The Beast is Back",
//            Meta = "C++ is not easy to learn",
//            UrlSlug = "c2plus",
//            Published = true,
//            PostedDate = new DateTime(2020, 2, 12, 10, 20, 0),
//            ModifiedDate = new DateTime(2023, 3, 6, 10, 20, 0),
//            AuthorId = context.Authors.ToList()[4].Id,
//            CategoryId = context.Categories.ToList()[2].Id,
//            ViewCount = 33,
//        };

//var tagEditForPost = new List<Tag>()
//{
//    context.Tags.ToList()[17]
//};

//await blogRepo.AddOrUpdatePostAsync(postUpdate, tagEditForPost);
#endregion

#region EditPostPublished
//await blogRepo.ChangePublishedPostAsync(31, false);
#endregion

#region GetRandomPost
//var postListRandom = await blogRepo.GetNRandomPostsAsync(5);
//Console.WriteLine("{0,-5}{1,-50}{2,-50}{3,-50}",
//    "ID", "Title", "ShortDescription", "Slug");
//foreach (var post in postListRandom)
//{
//    Console.WriteLine("{0,-5}{1,-50}{2,-50}{3,-50}",
//        post.Id, post.Title, post.Description, post.UrlSlug);
//}
#endregion
#endregion

#region Query
PostQuery postQuery = new PostQuery()
{
    KeyWord = "book",
    //PostMonth = 1,
    //Tags = "",
    //AuthorId = 1,
};

#region PostFindByQuery
//var postFind = await blogRepo.FindPostByQueryAsync(postQuery);
//foreach (var post in postFind)
//{
//    Console.WriteLine("----Title----");
//    Console.WriteLine(post.Title);
//    Console.WriteLine("----Tags----");
//    foreach (var tag in post.Tags)
//    {
//        Console.Write($"{tag.Name} ");
//    }
//    Console.WriteLine("\n");
//}
#endregion

#region CountPostQuery
//var count = await blogRepo.CountPostQueryAsync(postQuery);
//Console.WriteLine("Number of Posts have query: " + count);
#endregion

#region Page
//var pagingParams = new PagingParams
//{
//    PageNumber = 1,        
//    PageSize = 5,           
//    SortColumn = "Title",  
//    SortOrder = "ASC"     
//};

//var postList = await blogRepo.GetPagesPostQueryAsync(postQuery, pagingParams);
//Console.WriteLine("----Title----");
//foreach ( var post in postList)
//{

//    Console.WriteLine(post.Title);
//    //Console.WriteLine("----Tags----");
//    //foreach (var tag in post.Tags)
//    //{
//    //    Console.Write($"{tag.Name} ");
//    //}
//    Console.WriteLine("\n");
//}

//IPagedList<PostMapper> postMapper = await blogRepo
//  .GetPagesPostsAsync<PostMapper>(
//  postQuery,
//  pagingParams,
//  posts => posts.ProjectToType<PostMapper>()
//  );
//foreach (var post in postMapper)
//{
//    Console.WriteLine(post);
//}
#endregion
#endregion

#region Author
#region AuthorList
////Đọc danh sách tác giả từ cơ sở dữ liệu
//var authors = context.Authors.ToList();

//Console.WriteLine("{0,-4}{1,-30},{2,-30},{3,12}",
//    "ID", "Full Name", "Email", "Joined Date");

//foreach(var author in authors)
//{
//    Console.WriteLine("{0,-4}{1,-30},{2,-30},{3,12:MM/dd/yyyy}",
//        author.Id, author.FullName, author.Email, author.JoinedDate);
//}
#endregion

#region GetAuthorById
//var author = await authorRepo.GetAuthorByIdAsync(1);
//Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12}",
//    "ID", "Full Name", "Email", "Joined Date");
//Console.WriteLine("{0,-4}{1,-30}{2,-30}{3,12:MM/dd/yyyy}",
//        author.Id, author.FullName, author.Email, author.JoinedDate);
#endregion

#region GetAuthorBySlug
//var author = await authorRepo.GetAuthorBySlugAsync("jason-mouth");
//Console.WriteLine("{0,-4}{1,-30}{2,-20}{3,-30}{4,12}",
//    "ID", "Full Name", "UrlSlug", "Email", "Joined Date");
//Console.WriteLine("{0,-4}{1,-30}{2,-20}{3,-30}{4,12:MM/dd/yyyy}",
//        author.Id, author.FullName, author.UrlSlug, author.Email, author.JoinedDate);
#endregion

#region Page
var pagingParamsAuthor = new PagingParams
{
    PageNumber = 1,
    PageSize = 5,
    SortColumn = "FullName",
    SortOrder = "DESC"
};

//var authorList = await authorRepo.GetPagedAuthorsAsync(pagingParamsAuthor);
//foreach (var author in authorList)
//{
//    Console.WriteLine(author);
//}
#endregion

#region AddOrUpdateAuthor
//var authorAdd = new Author()
//{
//    FullName = "Nguyen Hoang Nhat Tien",
//    UrlSlug = "tien-nguyen",
//    Email = "tiennguyenn002@gmail.com",
//    JoinedDate = new DateTime(2023, 3, 6)
//};
//await authorRepo.AddOrUpdateAuthorAsync(authorAdd);
#endregion

#region GetNPopularAuthor
var topAuthors = await authorRepo.GetNPopularAuthors(3, pagingParamsAuthor);
Console.WriteLine("{0, -5}{1,-25}{2,-20}{3,-10}{4,-30}{5,-20}{6,-10}",
              "Id", "FullName", "UrlSlug", "ImageUrl", "JoinedDate", "Email", "Notes");
foreach (var author in topAuthors)
{
    Console.WriteLine(author);
}
#endregion
#endregion