import { delete_api, get_api, post_api } from "./Methods";

export function getPosts(keyword = '',
        pageSize = 10,
        pageNumber = 1,
        sortColumn = '',
        sortOrder = ''){
            return get_api(`https://localhost:7251/api/posts?KeyWord=${keyword}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
        }

export function getAuthors(name = '',
        pageSize = 10,
        pageNumber = 1,
        sortColumn = '',
        sortOrder = ''){
            return get_api(`https://localhost:7251/api/authors?Name=${name}&PageSize=${pageSize}&PageNumber=${pageNumber}&SortColumn=${sortColumn}&SortOrder=${sortOrder}`);
        }

export function getFilter(){
    return get_api(`https://localhost:7251/api/posts/get-filter`)
}

export function getPostsFilter(keyword = '',
    authorId = '',
    categoryId = '',
    year = '',
    month = '',
    pageSize = 10,
    pageNumber = 1,
    sortColumn = '',
    sortOrder = '')
{
    let url = new URL('https://localhost:7251/api/posts/get-posts-filter');
    keyword !== '' && url.searchParams.append('Keyword', keyword);
    authorId !== '' && url.searchParams.append('AuthorId', authorId);
    categoryId !== '' && url.searchParams.append('CategoryId', categoryId);
    year !== '' && url.searchParams.append('PostYear', year);
    month !== '' && url.searchParams.append('PostMonth', month);
    sortColumn !== '' && url.searchParams.append('SortColumn', sortColumn);
    sortOrder !== '' && url.searchParams.append('SortOrder', sortOrder);
    url.searchParams.append('PageSize', pageSize);
    url.searchParams.append('PageNumber', pageNumber);
    return get_api(url.href);
}

export async function getPostById(id = 0){
    if(id >0)
        return get_api(`https://localhost:7251/api/posts/${id}`)
    return null;
}

export function addOrUpdatePost(formData){
    return post_api(`https://localhost:7251/api/posts`, formData);
}

export function deletePost(id = 0){
    return delete_api(`https://localhost:7251/api/posts/${id}`)
}