import { useState, useEffect } from "react";
import ListGroup from "react-bootstrap/ListGroup";
import { Link } from "react-router-dom";
import { getRandomPosts } from "../Services/Widgets";

const RandomPostsWidget = () => {
  const [randomList, setRandomList] = useState([]);
  useEffect(() => {
    getRandomPosts().then(data => {
      if(data){
        setRandomList(data);
      }
      else{
        setRandomList([]);
      }
    });
  }, [])

  return (
    <div className="mb-4">
        <h3 className="text-success mb-2">
          5 bài viết ngẫu nhiên
        </h3>

        {randomList.length > 0 && 
          <ListGroup>
            {randomList.map((item, index) => {
              return (
                <ListGroup.Item key={index}>
                  <Link to={`/blog/post/${item.urlSlug}`}
                  title="Xem chi tiết"
                  key={index}>
                  {item.title}
                  </Link>
                </ListGroup.Item>
              );
            })}
          </ListGroup>
        }
    </div>
  );
}

export default RandomPostsWidget;