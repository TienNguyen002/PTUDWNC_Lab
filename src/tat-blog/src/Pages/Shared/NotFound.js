import { Image } from "react-bootstrap";
import { Link } from "react-router-dom";

const NotFound = () => {
    return (
        <div>
            <div className="text-center">
                <Image src="https://cdn.tgdd.vn/hoi-dap/580732/loi-404-not-found-la-gi-9-cach-khac-phuc-loi-404-not-5-800x450.jpg" />
            </div>
            <div className='text-center'>
                <Link to='/'
                    className='btn btn-primary'>
                    Quay về trang chủ
                </Link>
            </div>
        </div>
    )
}

export default NotFound;