using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;
using TatBlog.Services.Blogs;
using TatBlog.WinApp;

//Tạo đối tượng DbContext để quản lý phiên làm việc
//với CSDL và trạng thái của các đối tượng
var context = new BlogDbContext();

//Tạo đối tượng khởi tạo dữ liệu mẫu
var seeder = new DataSeeder(context);

//Gọi hàm Inititalize để nhập dữ liệu mẫu
seeder.Initialize();

//Tạo đối tượng BlogRepository
IBlogRepository blogRepo = new BlogRepository(context);

////Đọc danh sách tác giả từ cơ sở dữ liệu
//var authors = context.Authors.ToList();

////Xuất danh sách tác giả ra màn hình
//Console.WriteLine("{0,-4}{1,-30},{2,-30},{3,12}",
//    "ID", "Full Name", "Email", "Joined Date");

//foreach(var author in authors)
//{
//    Console.WriteLine("{0,-4}{1,-30},{2,-30},{3,12:MM/dd/yyyy}",
//        author.Id, author.FullName, author.Email, author.JoinedDate);
//}

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

////Xuất danh sách bài viết ra màn hình
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

////Tìm 3 bài viết được xem/đọc nhiều nhất
//var posts = await blogRepo.GetPopularArticlesAsync(3);

////Xuất danh sách bài viết ra màn hình
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

////Lấy danh sách chuyên mục
//var categories = await blogRepo.GetCategoriesAsync();

////Xuất ra màn hình
//Console.WriteLine("{0,-5}{1,-50}{2,10}",
//    "ID", "Name", "Count");
//foreach(var item in categories)
//{
//    Console.WriteLine("{0,-5}{1,-50}{2,10}",
//    item.Id, item.Name, item.PostCount);
//}

//Tạo đối tượng chứa tham số phân trang
var pagingParams = new PagingParams
{
    PageNumber = 1,         //Lấy kết quả ở trang số 1
    PageSize = 5,           //Lấy 5 mẫu tin
    SortColumn = "Name",    //Sắp xếp theo tên
    SortOrder = "DESC"      //Theo chiều giảm dần
};

//Lấy danh sách từ khóa
var tagsList = await blogRepo.GetPagedTagsAsync(pagingParams);

//Xuất ra màn hình
Console.WriteLine("{0,-5}{1,-50}{2,10}",
    "ID", "Name", "Count");

foreach(var item in tagsList)
{
    Console.WriteLine("{0,-5}{1,-50}{2,10}",
    item.Id, item.Name, item.PostCount);
}