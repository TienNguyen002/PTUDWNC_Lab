using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using SlugGenerator;
using TatBlog.Core;
using TatBlog.Core.Contracts;
using TatBlog.Core.DTO.Category;
using TatBlog.Core.DTO.Post;
using TatBlog.Core.DTO.Tag;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;
using TatBlog.Services.Extensions;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TatBlog.Services.Blogs
{
    public class BlogRepository : IBlogRepository
    {
        private readonly BlogDbContext _context;
        private readonly IMemoryCache _memoryCache;
        public BlogRepository(BlogDbContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        #region Tag
        #region GetPagesTagsAsync (Lấy ds Tag và phân trang theo pagingParams)
        //Lấy danh sách từ khóa/thẻ và phân trang theo các tham số pagingParams
        public async Task<IPagedList<TagItem>> GetPagesTagsAsync(
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
        #endregion

        #region IsTagExistBySlugAsync (Kiểm tra slug tồn tại của tag)
        public async Task<bool> IsTagExistBySlugAsync(
            int id,
            string slug,
            CancellationToken cancellationToken = default
        )
        {
            return await _context.Set<Tag>()
              .AnyAsync(c => c.Id != id && c.UrlSlug == slug, cancellationToken);
        }
        #endregion

        #region GetTagBySlugAsync (Lấy ds Tag bằng slug)
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
        #endregion

        #region GetTagByIdAsync (Lấy ds Tag bằng Id)
        public async Task<Tag> GetTagByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                  .Include(c => c.Posts)
                  .Where(c => c.Id == id)
                  .FirstOrDefaultAsync(cancellationToken);
        }
        #endregion

        #region GetAllTagsAsync (Lấy ds tất cả các Tag)
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
        #endregion

        #region AddOrUpdateTagAsync (Thêm/Cập nhật Tag)
        public async Task<bool> AddOrUpdateTagAsync(Tag tag, CancellationToken cancellationToken = default)
        {
            if (tag.Id > 0)
            {
                _context.Set<Tag>().Update(tag);
            }
            else
            {
                _context.Set<Tag>().Add(tag);
            }
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        #endregion

        #region DeleteTagByIdAsync (Xóa Tag theo Id)
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
        #endregion

        #region CheckExistTagSlugByIdAsync (Kiểm tra slug tồn tại bằng id - Tag)
        public async Task<bool> CheckExistTagSlugByIdAsync(int id, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Tag>()
                .AnyAsync(c => c.Id != id && c.UrlSlug == slug, cancellationToken);
        }
        #endregion

        #region FindTagByQueryable (Tìm Tag theo Queryable)
        private IQueryable<Tag> FindTagByQueryable(TagQuery query)
        {
            IQueryable<Tag> tagQuery = _context.Set<Tag>()
                .Include(c => c.Posts);
            if (!string.IsNullOrWhiteSpace(query.KeyWord))
            {
                tagQuery = tagQuery.Where(c => c.Name.Contains(query.KeyWord)
                || c.Description.Contains(query.KeyWord)
                || c.UrlSlug.Contains(query.KeyWord));
            }
            return tagQuery;
        }
        #endregion

        #region GetPagesTagsAsync (Lấy ds Tag và phân trang theo các tham số Paging)
        public async Task<IPagedList<Tag>> GetPagesTagsAsync(TagQuery tagQuery,
            int pageNumber,
            int pageSize,
            string sortColumn = "Id",
            string sortOrder = "ASC",
            CancellationToken cancellationToken = default)
        {
            var pagingParams = new PagingParams()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortColumn = sortColumn,
                SortOrder = sortOrder
            };
            IQueryable<Tag> tagResult = FindTagByQueryable(tagQuery);
            return await tagResult.ToPagedListAsync(pagingParams, cancellationToken);
        }
        #endregion

        #region GetPagedTagsAsync<T>
        public async Task<IPagedList<T>> GetPagedTagsAsync<T>(
                TagQuery query,
                IPagingParams pagingParams,
                Func<IQueryable<Tag>, IQueryable<T>> mapper,
                CancellationToken cancellationToken = default)
        {
            IQueryable<Tag> tagsQuery = FindTagByQueryable(query);

            IQueryable<T> result = mapper(tagsQuery);

            return await result.ToPagedListAsync<T>(pagingParams,
              cancellationToken);
        }
        #endregion
        #endregion

        #region Category
        #region GetCategoriesAsync (Lấy ds Category và số lượng bài viết thuộc Category)
        //Lấy danh sách chuyên mục và số lượng bài viết nằm thuộc từng chuyên mục/chủ đề
        public async Task<IList<CategoryItem>> GetCategoriesAsync(
            bool showOnMenu = true,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categories = _context.Set<Category>()
                .Where(c => c.ShowOnMenu == true);
            //if (showOnMenu)
            //{
            //    categories = categories.Where(x => x.ShowOnMenu);
            //}
            return await categories
                .OrderBy(x => x.Name)
                .Select(x => new CategoryItem()
                {
                    Id = x.Id,
                    Name = x.Name,
                    UrlSlug = x.UrlSlug,
                    Description = x.Description,
                    ShowOnMenu = x.ShowOnMenu,
                    PostCount = x.Posts.Count(/*p => p.Published*/)
                })
                .ToListAsync(cancellationToken);
        }
        #endregion

        #region IsCategoryExistBySlugAsync (Kiểm tra slug tồn tại của chủ đề)
        public async Task<bool> IsCategoryExistBySlugAsync(
            int id,
            string slug,
            CancellationToken cancellationToken = default
        )
        {
            return await _context.Set<Category>()
              .AnyAsync(c => c.Id != id && c.UrlSlug == slug, cancellationToken);
        }
        #endregion

        #region GetCategoryBySlugAsync (Lấy ds Category bằng Slug)
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
        #endregion

        #region GetCategoryByIdAsync (Lấy ds Category bằng Id)
        public async Task<Category> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                  .Include(c => c.Posts)
                  .Where(c => c.Id == id)
                  .FirstOrDefaultAsync(cancellationToken);
        }
        #endregion

        #region GetCachedCategoryByIdAsync
        public async Task<Category> GetCachedCategoryByIdAsync(int categoryId)
        {
            return await _memoryCache.GetOrCreateAsync(
                $"category.by-id.{categoryId}",
                async (entry) =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30);
                    return await GetCategoryByIdAsync(categoryId);
                });
        }
        #endregion

        #region CheckExistCategorySlugAsync (Kiểm tra slug tồn tại - Category)
        public Task<bool> CheckExistCategorySlugAsync(string slug, CancellationToken cancellationToken = default)
        {
            return _context.Set<Category>()
                .AnyAsync(c => c.UrlSlug == slug, cancellationToken);
        }
        #endregion

        #region CheckExistCategorySlugByIdAsync (Kiểm tra slug tồn tại bằng id - Category)
        public async Task<bool> CheckExistCategorySlugByIdAsync(int id, string slug, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .AnyAsync(c => c.Id != id && c.UrlSlug == slug, cancellationToken);
        }
        #endregion

        #region AddOrUpdateCategoryAsync (Thêm/Cập nhật Category)
        public async Task<bool> AddOrUpdateCategoryAsync(Category category, CancellationToken cancellationToken = default)
        {
            if (category.Id > 0)
            {
                _context.Set<Category>().Update(category);
            }
            else
            {
                _context.Set<Category>().Add(category);
            }
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        #endregion

        #region DeleteCategoryByIdAsync (Xóa Category bằng Id)
        public async Task<bool> DeleteCategoryByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var categoryToDelete = await _context.Set<Category>()
               .Include(c => c.Posts)
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
        #endregion

        #region FindCategoryByQueryable (Tìm Category theo Queryable)
        private IQueryable<Category> FindCategoryByQueryable(CategoryQuery query)
        {
            IQueryable<Category> categoryQuery = _context.Set<Category>()
                .Include(c => c.Posts);
            if (!string.IsNullOrWhiteSpace(query.KeyWord))
            {
                categoryQuery = categoryQuery.Where(c => c.Name.Contains(query.KeyWord)
                || c.Description.Contains(query.KeyWord)
                || c.UrlSlug.Contains(query.KeyWord));
            }
            if (query.NotShowOnMenu)
            {
                categoryQuery = categoryQuery.Where(c => !c.ShowOnMenu);
            }
            return categoryQuery;
        }
        #endregion

        #region ChangeShowOnMenuCategoryAsync
        public async Task ChangeShowOnMenuCategoryAsync(int id, CancellationToken cancellationToken = default)
        {
            await _context.Set<Category>()
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(c => c.SetProperty(c => c.ShowOnMenu, c => !c.ShowOnMenu), cancellationToken);
        }
        #endregion

        #region GetPagesCategoriesAsync (Lấy ds Category và phân trang theo pagingParams)
        public async Task<IPagedList<CategoryItem>> GetPagesCategoriesAsync(
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
        #endregion

        #region GetPagedCategoriesAsync <CategoryItem> (Lấy ds Category with name và phân trang theo các tham số của Paging)
        public async Task<IPagedList<CategoryItem>> GetPagedCategoriesAsync(
            IPagingParams pagingParams,
            string name = null,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Category>()
                .AsNoTracking()
                .WhereIf(!string.IsNullOrWhiteSpace(name),
                    x => x.Name.Contains(name))
                .Select(a => new CategoryItem()
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    UrlSlug = a.UrlSlug,
                    ShowOnMenu = a.ShowOnMenu,
                    PostCount = a.Posts.Count(p => p.Published)
                })
                .ToPagedListAsync(pagingParams, cancellationToken);
        }
        #endregion

        #region GetPagedCategoriesAsync (Lấy ds Category và phân trang theo các tham số của Paging)
        public async Task<IPagedList<Category>> GetPagedCategoriesAsync(
            CategoryQuery categoryQuery,
            int pageNumber,
            int pageSize,
            string sortColumn = "Id",
            string sortOrder = "ASC",
            CancellationToken cancellationToken = default)
        {
            var pagingParams = new PagingParams()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortColumn = sortColumn,
                SortOrder = sortOrder
            };
            IQueryable<Category> categoryResult = FindCategoryByQueryable(categoryQuery);
            return await categoryResult.ToPagedListAsync(pagingParams, cancellationToken);
        }
        #endregion

        #region GetPagedCategoriesAsync<T>
        public async Task<IPagedList<T>> GetPagedCategoriesAsync<T>(
      CategoryQuery query,
        IPagingParams pagingParams,
      Func<IQueryable<Category>, IQueryable<T>> mapper,
      CancellationToken cancellationToken = default)
        {
            IQueryable<Category> categoryFilter = FindCategoryByQueryable(query);

            IQueryable<T> resultQuery = mapper(categoryFilter);

            return await resultQuery
              .ToPagedListAsync<T>(pagingParams, cancellationToken);
        }
        #endregion
        #endregion

        #region Post
        #region GetPostAsync (Tìm Post theo slug và month year)
        //Tìm bài viết có tên định danh là 'slug' và được đăng vào tháng 'month' năm 'year'
        public async Task<Post> GetPostAsync(
            int year,
            int month,
            string slug,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postsQuery = _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags)
                .Include(x => x.Comments);
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
        #endregion

        #region GetPopularArticlesAsync (Tìm N Post dc nhiều ng xem nhất)
        //Tìm Top N bài viết phổ được nhiều người xem nhất
        public async Task<IList<Post>> GetPopularArticlesAsync(
            int numPosts,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Include(x => x.Author)
                .Include(x => x.Category)
                .Where(x => x.Published)
                .OrderByDescending(p => p.ViewCount)
                .Take(numPosts)
                .ToListAsync(cancellationToken);
        }
        #endregion

        #region IsPostSlugExistedAsync (Kiểm tra Post đã có slug chưa)
        //Kiểm tra xem tên định danh của bài viết đã có hay chưa
        public async Task<bool> IsPostSlugExistedAsync(
            int postId, string slug,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .AnyAsync(x => x.Id != postId && x.UrlSlug == slug, cancellationToken);
        }
        #endregion

        #region IncreaseViewCountAsync (Tăng lượt xem của Post)
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
        #endregion

        #region GetNRandomPostsAsync (Lấy ngẫu nhiên N Post)
        public async Task<IList<Post>> GetNRandomPostsAsync(int numPosts, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                    .OrderBy(p => Guid.NewGuid())
                    .Take(numPosts)
                    .ToListAsync(cancellationToken);
        }
        #endregion

        #region GetPostInNMonthAsync (Lấy ds Post theo tháng)
        public async Task<IList<PostItemsByMonth>> GetPostInNMonthAsync(int n, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .GroupBy(p => new { p.PostedDate.Year, p.PostedDate.Month })
                .Select(p => new PostItemsByMonth()
                {
                    Year = p.Key.Year,
                    Month = p.Key.Month,
                    PostCount = p.Count(),
                })
                .OrderByDescending(p => p.Year).ThenByDescending(p => p.Month)
                .Take(n)
                .ToListAsync(cancellationToken);
        }
        #endregion

        #region GetPostInMonthAndYearAsync (Lấy ds Post theo tháng và năm)
        public async Task<IList<PostItemsByMonth>> GetPostInMonthAndYearAsync(int n, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Select(p => new PostItemsByMonth()
                {
                    Year = p.PostedDate.Year,
                    Month = p.PostedDate.Month,
                    PostCount = _context.Set<Post>()
                    .Where(x => x.PostedDate.Month == p.PostedDate.Month)
                    .Where(x => x.PostedDate.Year == p.PostedDate.Year)
                    .Count()
                })
                .Distinct()
                .OrderByDescending(p => p.Year).ThenByDescending(p => p.Month)
                .Take(n)
                .ToListAsync(cancellationToken);
        }
        #endregion

        #region GetPostByIdAsync (Lấy ds Post bằng Id)
        public async Task<Post> GetPostByIdAsync(int id, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            if (!includeDetails)
            {
                return await _context.Set<Post>().FindAsync(id);
            }
            return await _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
        }
        #endregion

        #region GetPostBySlugAsync (Lấy ds Post bằng Slug)
        public async Task<Post> GetPostBySlugAsync(string slug, bool includeDetails = false, CancellationToken cancellationToken = default)
        {
            if (!includeDetails)
            {
                return await _context.Set<Post>().Where(p => p.UrlSlug == slug)
                    .FirstOrDefaultAsync(cancellationToken);
            }
            return await _context.Set<Post>()
                .Include(x => x.Category)
                .Include(x => x.Author)
                .Include(x => x.Tags)
                .Where(x => x.UrlSlug == slug)
                .FirstOrDefaultAsync(cancellationToken);
        }
        #endregion

        #region AddOrUpdatePostAsync (Thêm/Cập nhật Post)
        public async Task<bool> AddOrUpdatePostAsync(Post post, IEnumerable<string> tags, CancellationToken cancellationToken = default)
        {
            if (post.Id > 0)
            {
                await _context.Entry(post).Collection(x => x.Tags).LoadAsync(cancellationToken);
            }
            else
            {
                post.Tags = new List<Tag>();
            }

            var validTags = tags.Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => new
                {
                    Name = x,
                    Slug = x.GenerateSlug()
                })
                .GroupBy(x => x.Slug)
                .ToDictionary(g => g.Key, g => g.First().Name);


            foreach (var kv in validTags)
            {
                if (post.Tags.Any(x => string.Compare(x.UrlSlug, kv.Key, StringComparison.InvariantCultureIgnoreCase) == 0)) continue;

                var tag = await GetTagBySlugAsync(kv.Key, cancellationToken) ?? new Tag()
                {
                    Name = kv.Value,
                    Description = kv.Value,
                    UrlSlug = kv.Key
                };

                post.Tags.Add(tag);
            }

            post.Tags = post.Tags.Where(t => validTags.ContainsKey(t.UrlSlug)).ToList();

            if (post.Id > 0)
            {
                post.ModifiedDate = DateTime.Now;
                _context.Update(post);
            }      
            else
                _context.Add(post);

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
        #endregion

        #region ChangePublishedPostAsync (Thay đổi trạng thái Publish/NotPushlish của Post)
        public async Task ChangePublishedPostAsync(int id, CancellationToken cancellationToken = default)
        {
            await _context.Set<Post>()
                .Where(p => p.Id == id)
                .ExecuteUpdateAsync(p => p.SetProperty(p => p.Published, p => !p.Published), cancellationToken);
        }
        #endregion

        #region Find(All)PostByQueryable (Tìm Post theo Queryable)
        private IQueryable<Post> FindPostByQueryable(PostQuery query)
        {
            IQueryable<Post> postQuery = _context.Set<Post>()
                .Include(p => p.Author)
                .Include(p => p.Tags)
                .Include(p => p.Category)
                .Where(p => p.Published);
            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                postQuery = postQuery.Where(p => p.Title.Contains(query.KeyWord)
                || p.Description.Contains(query.KeyWord)
                || p.ShortDescription.Equals(query.KeyWord)
                || p.UrlSlug.Equals(query.KeyWord)
                || p.Tags.Any(t => t.Name.Contains(query.KeyWord)
                || p.Author.UrlSlug.Contains(query.KeyWord))
                );
            }
            if (!string.IsNullOrWhiteSpace(query.CategorySlug))
            {
                postQuery = postQuery
                    .Where(p => p.Category.UrlSlug == query.CategorySlug);
            }
            if (!string.IsNullOrWhiteSpace(query.AuthorName))
            {
                postQuery = postQuery
                    .Where(p => p.Author.FullName.Contains(query.AuthorName));
            }
            if (!string.IsNullOrWhiteSpace(query.AuthorSlug))
            {
                postQuery = postQuery
                    .Where(p => p.Author.UrlSlug == query.AuthorSlug);
            }
            if (!string.IsNullOrWhiteSpace(query.TagName))
            {
                postQuery = postQuery
                    .Where(p => p.Tags.Any(t => t.Name == query.TagName));
            }
            if (query.PostMonth > 0)
            {
                postQuery = postQuery
                    .Where(p => p.PostedDate.Month == query.PostMonth);
            }
            if (query.PostYear > 0)
            {
                postQuery = postQuery
                    .Where(p => p.PostedDate.Year == query.PostYear);
            }
            if (query.CategoryId > 0)
            {
                postQuery = postQuery
                    .Where(p => p.CategoryId == query.CategoryId);
            }
            if (query.AuthorId > 0)
            {
                postQuery = postQuery
                    .Where(p => p.AuthorId == query.AuthorId);
            }
            if (!string.IsNullOrEmpty(query.CategoryName))
            {
                postQuery = postQuery
                    .Where(p => p.Category.Name == query.CategoryName);
            }
            if (query.NotPublished)
            {
                postQuery = postQuery.Where(p => !p.Published);
            }
            var tags = query.GetSelectedTags();
            if (tags.Count > 0)
            {
                foreach (var tag in tags)
                {
                    postQuery = postQuery.Include(p => p.Tags)
                        .Where(p => p.Tags.Any(t => t.Name == tag));
                }
            }
            return postQuery;
        }

        private IQueryable<Post> FindAllPostByQueryable(PostQuery query)
        {
            IQueryable<Post> postQuery = _context.Set<Post>()
                .Include(p => p.Author)
                .Include(p => p.Tags)
                .Include(p => p.Category);
            if (!string.IsNullOrEmpty(query.KeyWord))
            {
                postQuery = postQuery.Where(p => p.Title.Contains(query.KeyWord)
                || p.Description.Contains(query.KeyWord)
                || p.ShortDescription.Equals(query.KeyWord)
                || p.UrlSlug.Equals(query.KeyWord)
                || p.Tags.Any(t => t.Name.Contains(query.KeyWord))
                );
            }
            if (!string.IsNullOrWhiteSpace(query.CategorySlug))
            {
                postQuery = postQuery
                    .Where(p => p.Category.UrlSlug == query.CategorySlug);
            }
            if (!string.IsNullOrWhiteSpace(query.AuthorName))
            {
                postQuery = postQuery
                    .Where(p => p.Author.FullName.Contains(query.AuthorName));
            }
            if (!string.IsNullOrWhiteSpace(query.AuthorSlug))
            {
                postQuery = postQuery
                    .Where(p => p.Author.UrlSlug == query.AuthorSlug);
            }
            if (!string.IsNullOrWhiteSpace(query.TagName))
            {
                postQuery = postQuery
                    .Where(p => p.Tags.Any(t => t.Name == query.TagName));
            }
            if (query.PostMonth > 0)
            {
                postQuery = postQuery
                    .Where(p => p.PostedDate.Month == query.PostMonth);
            }
            if (query.PostYear > 0)
            {
                postQuery = postQuery
                    .Where(p => p.PostedDate.Year == query.PostYear);
            }
            if (query.CategoryId > 0)
            {
                postQuery = postQuery
                    .Where(p => p.CategoryId == query.CategoryId);
            }
            if (query.AuthorId > 0)
            {
                postQuery = postQuery
                    .Where(p => p.AuthorId == query.AuthorId);
            }
            if (!string.IsNullOrEmpty(query.CategoryName))
            {
                postQuery = postQuery
                    .Where(p => p.Category.Name == query.CategoryName);
            }
            if (query.NotPublished)
            {
                postQuery = postQuery.Where(p => !p.Published);
            }
            var tags = query.GetSelectedTags();
            if (tags.Count > 0)
            {
                foreach (var tag in tags)
                {
                    postQuery = postQuery.Include(p => p.Tags)
                        .Where(p => p.Tags.Any(t => t.Name == tag));
                }
            }
            return postQuery;
        }
        #endregion

        #region Find(All)PostByQueryAsync (Tìm ds Post bằng Query)
        public async Task<IList<Post>> FindPostByQueryAsync(PostQuery query, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> posts = FindPostByQueryable(query);
            return await posts.ToListAsync(cancellationToken);
        }

        public async Task<IList<Post>> FindAllPostByQueryAsync(PostQuery query, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> posts = FindAllPostByQueryable(query);
            return await posts.ToListAsync(cancellationToken);
        }
        #endregion

        #region CountPostQueryAsync (Đếm số lượng Post)
        public async Task<int> CountPostQueryAsync(PostQuery query, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> posts = await Task.Run(() => FindPostByQueryable(query));
            return posts.Count();
        }
        #endregion

        #region GetPages(All)PostQueryAsync (Lấy ds Post và phân trang theo pagingParams)
        public async Task<IPagedList<Post>> GetPagesAllPostQueryAsync(PostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> posts = FindAllPostByQueryable(query);
            return await posts
                .ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<IPagedList<Post>> GetPagesPostQueryAsync(PostQuery query, IPagingParams pagingParams, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> posts = FindPostByQueryable(query);
            return await posts
                .ToPagedListAsync(pagingParams, cancellationToken);
        }
        #endregion

        #region GetPagedPostsAsync<T> (Lấy ds Post theo T)
        public async Task<IPagedList<T>> GetPagedPostsAsync<T>(PostQuery query,
            IPagingParams pagingParams,
            Func<IQueryable<Post>, IQueryable<T>> mapper,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Post> postsFindResultQuery = FindPostByQueryable(query);
            IQueryable<T> result = mapper(postsFindResultQuery);
            return await result
              .ToPagedListAsync(pagingParams, cancellationToken);
        }
        #endregion

        #region Get(All)PagedPostQueryAsync (Lấy ds Post và phân trang theo các tham số của Paging)
        public async Task<IPagedList<Post>> GetPagedPostQueryAsync(PostQuery postQuery,
            int pageNumber,
            int pageSize,
            string sortColumn = "Id",
            string sortOrder = "ASC",
            CancellationToken cancellationToken = default)
        {
            var pagingParams = new PagingParams()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortColumn = sortColumn,
                SortOrder = sortOrder,
            };
            IQueryable<Post> postResult = FindPostByQueryable(postQuery);
            return await postResult.ToPagedListAsync(pagingParams, cancellationToken);
        }

        public async Task<IPagedList<Post>> GetAllPagedPostQueryAsync(PostQuery postQuery,
            int pageNumber,
            int pageSize,
            string sortColumn = "Id",
            string sortOrder = "ASC",
            CancellationToken cancellationToken = default)
        {
            var pagingParams = new PagingParams()
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                SortColumn = sortColumn,
                SortOrder = sortOrder,
            };
            IQueryable<Post> postResult = FindAllPostByQueryable(postQuery);
            return await postResult.ToPagedListAsync(pagingParams, cancellationToken);
        }
        #endregion

        #region DeletePostByIdAsync (Xóa Post theo Id)
        public async Task<bool> DeletePostByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var postDelete = await _context.Set<Post>()
                .Include(p => p.Tags)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync(cancellationToken);
            if (postDelete == null)
            {
                return false;
            }
            _context.Remove(postDelete);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
        #endregion

        #region SetImageUrlPostAsync (Đặt hình ảnh cho Post)
        public async Task<bool> SetImageUrlPostAsync(int id, string imageUrl, CancellationToken cancellationToken = default)
        {
            return await _context.Set<Post>()
                .Where(p => p.Id == id)
                .ExecuteUpdateAsync(p => p.SetProperty(p => p.ImageUrl, p => imageUrl)
                                          .SetProperty(p => p.ModifiedDate, p => DateTime.Now),
                                          cancellationToken) > 0;
        }
        #endregion

        #region GetNRandomPosts<T> (Lấy N ngẫu nhiên bài viết)
        public async Task<IList<T>> GetNRandomPostsAsync<T>(int n, Func<IQueryable<Post>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
        {
            IQueryable<Post> random = _context.Set<Post>()
                .Include(p => p.Author)
                .Include(p => p.Tags)
                .Include(p => p.Category)
                .OrderBy(p => Guid.NewGuid())
                .Take(n);
            return await mapper(random).ToListAsync(cancellationToken);
        }
        #endregion

        #region GetNPopularPosts (Lấy N bài viết có nhiều lượt xem nhất)
        public async Task<IList<T>> GetNPopularPostsAsync<T>(int n, Func<IQueryable<Post>, IQueryable<T>> mapper, CancellationToken cancellationToken = default)
        {
            var popular = _context.Set<Post>()
                .Include(p => p.Author)
                .Include(p => p.Tags)
                .Include(p => p.Category)
                .Where(p => p.Published)
                .OrderByDescending(p => p.ViewCount)
                .Take(n);
            return await mapper(popular).ToListAsync(cancellationToken);
        }
        #endregion
        #endregion   
    }
}
