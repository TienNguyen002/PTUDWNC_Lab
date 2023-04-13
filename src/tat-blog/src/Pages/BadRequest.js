import { Image } from "react-bootstrap";
import { Link, useNavigate } from "react-router-dom";
import { useQuery } from "../Utils/Utils";

const BadRequest = () => {
    const navigate = useNavigate();
    let query = useQuery(),
        redirectTo = query.get('redirectTo') ?? '/';
    return (
        <div className='text-center'>
            <div>
                <Image src="https://tenten.vn/tin-tuc/wp-content/uploads/2022/08/2-3.jpg" />
            </div>
            <div >
                <Link to={redirectTo}
                    className='btn btn-primary'
                    onClick={() => navigate(-1)}>
                    Quay về trang trước
                </Link>
            </div>
        </div>
    )
}

export default BadRequest;