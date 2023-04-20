import { useState, useEffect } from "react";
import Table from "react-bootstrap/Table"
import { Link, useParams} from "react-router-dom";
import { changePublished, deletePost, getPostsFilter } from "../../../Services/BlogRepository";
import Loading from "../../../Components/Shared/Loading"; 
import PostFilterPane from "../../../Components/Admin/Posts/PostFilterPane";
import { useSelector } from "react-redux";
import { faTrash, faEye, faEyeSlash } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import "./style/style.css"

const Posts = () => {
    const [postsList, setPostsList] = useState([]);
    const [isVisibleLoading, setIsVisibleLoading] = useState(true),
    postFilter = useSelector(state => state.postFilter);

    let {id} = useParams(), p = 1, ps = 10;
    
    useEffect(() => {
        document.title = 'Danh sách bài viết';
        getPostsFilter(postFilter.keyword,
            postFilter.authorId,
            postFilter.categoryId,
            postFilter.year,
            postFilter.month,
            ps, p).then(data => {
            if(data)
                setPostsList(data.items);
            else
                setPostsList([]);
            setIsVisibleLoading(false);
        })
    }, [postFilter, p, ps, postsList]);

    const handleDelete = (e, id) => {
        e.preventDefault();
        DeletePost(id);

        async function DeletePost(id){
            if(window.confirm("Xóa bài viết này?")){
                const response = await deletePost(id);
                if(response)
                    alert("Xóa thành công!");
                else
                    alert("Lỗi!!");
            }
        }
    };

    const handleChangePublished = (e, id) => {
        e.preventDefault();
        ChangePublished(id);

        async function ChangePublished(id){
            const response = await changePublished(id);
            if(response)
                alert(response);
            else
                alert("Thay đổi không thành công");
        }
    }

    return (
        <>
            <h1>Danh sách bài viết {id}</h1>
            <PostFilterPane/>
            {isVisibleLoading ? <Loading/> : 
                <Table striped responsive bordered>
                    <thead>
                        <tr>
                            <th className="title">Tiêu đề</th>
                            <th>Tác giả</th>
                            <th>Chủ đề</th>
                            <th>Xuất bản</th>
                            <th>Xóa</th>
                        </tr>
                    </thead>
                    <tbody>
                        {postsList.length > 0 ? postsList.map((item, index) =>
                        <tr key={index}>
                            <td className="title">
                                <Link to={`/admin/posts/edit/${item.id}`}
                                className="text-bold">
                                {item.title}
                                </Link>
                                <p className="text-muted">{item.shortDescription}</p>
                            </td>
                            <td>{item.author.fullName}</td>
                            <td>{item.category.name}</td>
                            <td>
                                <div className="text-center"
                                    onClick={(e) => handleChangePublished(e, item.id)}>
                                        {item.published 
                                        ? <div className="published">
                                            <FontAwesomeIcon icon={faEye}/> Có
                                        </div>
                                        : <div className="not-published">
                                            <FontAwesomeIcon icon={faEyeSlash}/> Không
                                        </div>}
                                </div> 
                            </td>
                            <td>
                                <div className="text-center delete"
                                    onClick={(e) => handleDelete(e, item.id)}>
                                    <FontAwesomeIcon icon={faTrash}/>
                                </div>
                            </td>
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