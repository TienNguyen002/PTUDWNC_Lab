import Spinner from "react-bootstrap/Spinner";
import Button from "react-bootstrap/Button";

const Loading = () => {
    return (
        <div className="text-center">
            <Button varient='outline-success' disabled style={{border: 'none'}}>
                <Spinner as='span'
                        animation="grow"
                        size="sm"
                        role="status"
                        aria-hidden='true'/>
                        &nbsp;Đang tải . . .
            </Button>
        </div>
    )
}

export default Loading;