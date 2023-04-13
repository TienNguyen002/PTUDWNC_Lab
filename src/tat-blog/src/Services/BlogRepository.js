import { get_api } from "./Methods";

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
    return get_api(`https://localhost:7251/api/posts/get-filter`);
}
