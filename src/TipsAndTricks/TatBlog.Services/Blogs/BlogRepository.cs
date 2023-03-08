using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TatBlog.Services.Blogs
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDbContext _context;

        public BlogRepository(BlogDbContext context)
        {
            _context = context;
        }

        //Tìm bài viết có tên định danh là 'slug' và được đăng vào tháng 'month' năm 'year'
        public async Task<Post> GetPostAsync(
            int year,
            int month,
            string slug,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postsQuery = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author);
            if (year > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Year == year);
            }
            if (month > 0)
            {
                postsQuery = postsQuery.Where(x => x.PostedDate.Month == month);
            }
            if (!string.IsNullOrWhiteSpace(slug))
            {
                postsQuery = postsQuery.Where(x => x.UrlSlug == slug);
            }

            return await postsQuery.FirstOrDefaultAsync(cancellationToken);
        }

        //Tìm Top N bài viết phổ được nhiều người xem nhất
        public async Task<IList<Post>> GetPopularArticlesAsync(
            int numPosts,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }

        //Kiểm tra xem tên định danh của bài viết đã có hay chưa
        public async Task<bool> IsPostSlugExistedAsync(
            int postId, string slug,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
        }

        //Tăng số lượt xem của một bài viết
        public async Task IncreaseViewCountAsync(
            int postId,
            CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
                .Where(x => x.Id == postId)
                .ExecuteUpdateAsync(p =>
                    p.SetProperty(x => x.ViewCount, x => x.ViewCount + 1), cancellationToken);
        }

        //Lấy danh sách chuyên mục và số lượng bài viết nằm thuộc từng chuyên mục/chủ đề
        public async Task<IList<CategoryItem>> GetCategoriesAsync(
            bool showOnMenu = false,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categories = _context.Set<Category>();
            if (showOnMenu)
            {
                categories = categories.Where(x => x.ShowOnMenu);
            }
            return await categories
                .OrderBy(x => x.Name)
                .Select(x => new CategoryItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    ShowOnMenu = x.ShowOnMenu,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }

        //Lấy danh sách từ khóa/thẻ và phân trang theo các tham số pagingParams
        public async Task<IPagedList<TagItem>> GetPagedTagsAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default)
        {
            var tagQuery = _context.Set<Tag>()
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });
            return await tagQuery
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<Tag> GetTagBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            IQueryable<Tag> tagsQuery = _context.Set<Tag>()
                .Include(t => t.Posts);
            if (!string.IsNullOrWhiteSpace(slug))
            {
                tagsQuery = tagsQuery.Where(x => x.UrlSlug == slug);
            }
            return await tagsQuery.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IList<TagItem>> GetAllTagsAsync(CancellationToken cancellationToken = default)
        {
            IQueryable<Tag> tags = _context.Set<Tag>();
            return await tags
                .OrderBy(x => x.Name)
                .Select(x => new TagItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                })
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> DeleteTagByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var tagToDelete = await _context.Set<Tag>()
               .Include(t => t.Posts)
               .Where(t => t.Id == id)
               .FirstOrDefaultAsync(cancellationToken);
            if (tagToDelete == null)
            {
                return false;
            }
            _context.Set<Tag>().Remove(tagToDelete);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<Category> GetCategoryBySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categoriesQuery = _context.Set<Category>()
                .Include(t => t.Posts);
            if (!string.IsNullOrWhiteSpace(slug))
            {
                categoriesQuery = categoriesQuery.Where(x => x.UrlSlug == slug);
            }
            return await categoriesQuery.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Category> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public Task<bool> CheckExistCategorySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return _context.Set<Category>()
                .AnyAsync(c => c.UrlSlug == slug, cancellationToken);
        }

        public async Task<bool> CheckExistCategorySlugAsync(int id, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .AnyAsync(c => c.Id != id && c.UrlSlug == slug, cancellationToken);
        }

        public async Task AddOrUpdateCategoryAsync(Category category, CancellationToken cancellationToken = default)
        {
            if (category.Id > 0)
            {
                Category categoryEdit = await _context.Set<Category>()
                    .Where(c => c.Id == category.Id)
                    .FirstOrDefaultAsync(cancellationToken);
                if (categoryEdit == null)
                { return; }
                if (categoryEdit.UrlSlug != category.UrlSlug && CheckExistCategorySlugAsync(category.Id, category.UrlSlug).Result)
                {
                    await Console.Out.WriteLineAsync("Da ton tai UrlSlug");
                    return;
                }
                _context.Entry(categoryEdit).CurrentValues.SetValues(category);
            }
            else
            {
                if (await CheckExistCategorySlugAsync(category.Id, category.UrlSlug))
                {
                    await Console.Out.WriteLineAsync("Da ton tai UrlSlug");
                    return;
                }
                _context.Set<Category>().Add(category);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var categoryToDelete = await _context.Set<Category>()
               .Where(c => c.Id == id)
               .FirstOrDefaultAsync(cancellationToken);
            if (categoryToDelete == null)
            {
                return false;
            }
            _context.Set<Category>().Remove(categoryToDelete);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
            IPagingParams pagingParams,
            CancellationToken cancellationToken = default)
        {
            var categoriesQuery = _context.Set<Category>()
                .Select(x => new CategoryItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    PostCount = x.Posts.Count(p => p.Published)
                });
            return await categoriesQuery
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<IList<PostItems>> GetPostInNMonthAsync(int n, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Select(p => new PostItems()
                {
                    Year = p.PostedDate.Year,
                    Month = p.PostedDate.Month,
                    PostCount = _context.Set<Post>()
                    .Where(x => x.PostedDate == p.PostedDate)
                    .Count()
                })
                .Distinct()
                .OrderByDescending(p => p.Year).ThenByDescending(p => p.Month)
                .Take(n)
                .ToListAsync(cancellationToken);
        }

        public async Task<Post> GetPostByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task AddOrUpdatePostAsync(Post post, IList<Tag> tags, CancellationToken cancellationToken = default)
        {
            if (post.Id > 0)
            {
                Post postEdit = await _context.Set<Post>()
                    .Include(p => p.Tags)
                    .Where(c => c.Id == post.Id)
                    .FirstOrDefaultAsync(cancellationToken);
                if (postEdit == null)
                { return; }
                if (postEdit.UrlSlug != post.UrlSlug && CheckExistCategorySlugAsync(post.Id, post.UrlSlug).Result)
                {
                    await Console.Out.WriteLineAsync("Da ton tai UrlSlug");
                    return;
                }
                postEdit.Tags = tags;
                _context.Entry(postEdit).CurrentValues.SetValues(post);
            }
            else
            {
                if (await CheckExistCategorySlugAsync(post.Id, post.UrlSlug))
                {
                    await Console.Out.WriteLineAsync("Da ton tai UrlSlug");
                    return;
                }
                post.Tags = tags;
                _context.Set<Post>().Add(post);
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task ChangePublishedPostAsync(int id, bool published, CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
                .Where(p => p.Id == id)
                .ExecuteUpdateAsync(p => p.SetProperty(p => p.Published, published), cancellationToken);
        }

        public async Task<IList<Post>> GetNRandomPostsAsync(int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                    .OrderBy(p => Guid.NewGuid())
                    .Take(numPosts)
                    .ToListAsync(cancellationToken);
        }

        private IQueryable<Post> FindPostByQueryable(PostQuery query) 
        {
            IQueryable<Post> postQuery = _context.Set<Post>()
                .Include(p => p.Author)
                .Include(p => p.Tags)
                .Include(p => p.Category);
            if(!string.IsNullOrEmpty(query.KeyWord))
            {
                postQuery = postQuery.Where(p => p.Title.Contains(query.KeyWord) 
                || p.Description.Contains(query.KeyWord) 
                || p.ShortDescription.Equals(query.KeyWord)
                || p.UrlSlug.Equals(query.KeyWord)
                || p.Tags.Any(t => t.Name.Contains(query.KeyWord))
                );
            }
            if(query.PostMonth > 0)
            {
                postQuery = postQuery
                    .Where(p => p.PostedDate.Month == query.PostMonth);
            }
            if(query.CategoryId > 0)
            {
                postQuery = postQuery
                    .Where(p => p.CategoryId == query.CategoryId);
            }
            if(query.AuthorId > 0)
            {
                postQuery = postQuery
                    .Where(p => p.AuthorId == query.AuthorId);
            }
            if(!string.IsNullOrEmpty(query.CategoryName))
            {
                postQuery = postQuery
                    .Where(p => p.Category.Name == query.CategoryName);
            }
            var tags = query.GetTag();
            if(tags.Count > 0)
            {
                foreach( var tag in tags)
                {
                    postQuery = postQuery.Include(p => p.Tags)
                        .Where(p => p.Tags.Any(t => t.Name == tag));
                }
            }
            return postQuery;
        }

        public async Task<IList<Post>> FindPostByQueryAsync(PostQuery query, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> posts = FindPostByQueryable(query);
            return await posts.ToListAsync(cancellationToken);
        }

        public async Task<int> CountPostQueryAsync(PostQuery query, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> posts = await Task.Run(() => FindPostByQueryable(query));
            return posts.Count();
        }

        public async Task<IPagedList<Post>> GetPagesPostQueryAsync(PostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> posts = FindPostByQueryable(query);
            return await posts
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<IPagedList<T>> GetPagesPostsAsync<T>(PostQuery query, 
            IPagingParams pagingParams, 
            Func<IQueryable<Post>, IQueryable<T>> mapper, 
            CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postsFindResultQuery = FindPostByQueryable(query);
            IQueryable<T> result = mapper(postsFindResultQuery);

            return await result
              .ToPagedListAsync(pagingParams, cancellationToken);
        }
    }
}
