import { get_api } from "./Methods";

export function getCategories(){
  return get_api(`https://localhost:7251/api/categories`)
}

export function getFeaturedPosts(){
  return get_api(`https://localhost:7251/api/posts/feature/3`)
}

export function getRandomPosts(){
  return get_api(`https://localhost:7251/api/posts/random/5`)
}

export function getTags(){
  return get_api(`https://localhost:7251/api/tags/alltags`)
}

export function getBestAuthors(){
  return get_api(`https://localhost:7251/api/authors/best/4`)
}

export function getArchives(){
  return get_api(`https://localhost:7251/api/posts/archives/12`)
}