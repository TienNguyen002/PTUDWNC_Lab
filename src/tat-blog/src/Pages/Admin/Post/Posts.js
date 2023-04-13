import { useState, useEffect } from "react";
import Table from "react-bootstrap/Table"
import { Link } from "react-router-dom";
import { getPosts } from "../../../Services/BlogRepository";
import Loading from "../../../Components/Shared/Loading"; 

const Posts = () => {
    const [postsList, setPostsList] = useState([]);
    const [isVisibleLoading, setIsVisibleLoading] = useState(true);

    let k = '', p = 1, ps = 10;
    
    useEffect(() => {
        document.title = 'Danh sách bài viết';
        
        getPosts(k, ps, p).then(data => {
            if(data)
                setPostsList(data.items);
            else
                setPostsList([]);
            setIsVisibleLoading(false);
        })
    }, [k, ps, p]);

    return (
        <>
            <h1>Danh sách bài viết</h1>
            {isVisibleLoading ? <Loading/> : 
                <Table striped responsive bordered>
                    <thead>
                        <tr>
                            <th>Tiêu đề</th>
                            <th>Tác giả</th>
                            <th>Chủ đề</th>
                            <th>Xuất bản</th>
                        </tr>
                    </thead>
                    <tbody>
                        {postsList.length > 0 ? postsList.map((item, index) =>
                        <tr key={index}>
                            <td>
                                <Link to={`/admin/posts/edit/${item.id}`}
                                className="text-bold">
                                {item.title}
                                </Link>
                                <p className="text-muted">{item.shortDescription}</p>
                            </td>
                            <td>{item.author.fullName}</td>
                            <td>{item.category.name}</td>
                            <td>{item.published ? "Có" : "Không"}</td>
                        </tr>
                        ) :
                        <tr>
                            <td colSpan={4}>
                                <h4 className="text-danger text-center">Không tìm thấy bài viết nào</h4>
                            </td>    
                        </tr>}
                    </tbody>
                </Table>
            }
        </>
    )
}

export default Posts;