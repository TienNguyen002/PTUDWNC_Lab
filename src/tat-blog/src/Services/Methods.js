import axios from 'axios';

export async function get_api(your_api){
    try{
        const response = await axios.get(your_api);
        const data = response.data;
        if(data.isSuccess)
            return data.result;
        else
            return null;
    } catch (error){
        console.log('Error', error.message);
        return null;
    }
}