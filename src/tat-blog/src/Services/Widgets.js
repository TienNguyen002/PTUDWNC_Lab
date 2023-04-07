import axios from "axios";

export async function getCategories(){
  try {
    const response = await axios.get(`https://localhost:7251/api/categories`);
    const data = response.data;
    if(data.isSuccess){
      return data.result;
    }
    else{
      return null;
    }
  } catch (error) {
    console.log('Error', error.message);
    return null;
  }
}

export async function getFeaturedPosts(){
  try {
    const response = await axios.get(`https://localhost:7251/api/posts/feature/3`);
    const data = response.data;
    if(data.isSuccess){
      return data.result;
    }
    else{
      return null;
    }
  } catch (error) {
    console.log('Error', error.message);
    return null;
  }
}

export async function getRandomPosts(){
  try {
    const response = await axios.get(`https://localhost:7251/api/posts/random/5`);
    const data = response.data;
    if(data.isSuccess){
      return data.result;
    }
    else{
      return null;
    }
  } catch (error) {
    console.log('Error', error.message);
    return null;
  }
}

export async function getTags(){
  try {
    const response = await axios.get(`https://localhost:7251/api/tags/alltags`);
    const data = response.data;
    if(data.isSuccess){
      return data.result;
    }
    else{
      return null;
    }
  } catch (error) {
    console.log('Error', error.message);
    return null;
  }
}

export async function getBestAuthors(){
  try {
    const response = await axios.get(`https://localhost:7251/api/authors/best/4`);
    const data = response.data;
    if(data.isSuccess){
      return data.result;
    }
    else{
      return null;
    }
  } catch (error) {
    console.log('Error', error.message);
    return null;
  }
}

export async function getArchives(){
  try {
    const response = await axios.get(`https://localhost:7251/api/posts/archives/12`);
    const data = response.data;
    if(data.isSuccess){
      return data.result;
    }
    else{
      return null;
    }
  } catch (error) {
    console.log('Error', error.message);
    return null;
  }
}