using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;

//Tạo đối tượng DbContext để quản lý phiên làm việc
//với CSDL và trạng thái của các đối tượng
var context = new BlogDbContext();

////Tạo đối tượng khởi tạo dữ liệu mẫu
//var seeder = new DataSeeder(context);

////Gọi hàm Inititalize để nhập dữ liệu mẫu
//seeder.Initialize();

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

//Đọc danh sách bài viết từ cơ sở dữ liệu
//Lấy kèm tên tác giả và chuyên mục
var posts = context.Posts
    .Where(p => p.Published)
    .OrderBy(p => p.Title)
    .Select(p => new
    {
        Id = p.Id,
        Title = p.Title,
        ViewCount = p.ViewCount,
        PostedDate = p.PostedDate,
        Author = p.Author.FullName,
        Category = p.Category.Name,
    })
    .ToList();

//Xuất danh sách bài viết ra màn hình
foreach(var post in posts)
{
    Console.WriteLine("ID       : {0}", post.Id);
    Console.WriteLine("Title    : {0}", post.Title);
    Console.WriteLine("View     : {0}", post.ViewCount);
    Console.WriteLine("Date     : {0:MM/dd/yyyy}", post.PostedDate);
    Console.WriteLine("Author   : {0}", post.Author);
    Console.WriteLine("Category : {0}", post.Category);
    Console.WriteLine("".PadRight(80,'-'));
}

