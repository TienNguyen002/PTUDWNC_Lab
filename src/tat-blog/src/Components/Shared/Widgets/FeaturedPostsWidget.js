import { useState, useEffect } from "react";
import ListGroup from "react-bootstrap/ListGroup";
import { Link } from "react-router-dom";
import { getFeaturedPosts } from "../../../Services/Widgets";

const FeaturedPostsWidget = () => {
  const [featuredList, setFeaturedList] = useState([]);
  useEffect(() => {
    getFeaturedPosts().then(data => {
      if(data){
        setFeaturedList(data);
      }
      else{
        setFeaturedList([]);
      }
    });
  }, [])

  return (
    <div className="mb-4">
        <h3 className="text-success mb-2">
          3 bài viết có lượt xem cao nhất
        </h3>

        {featuredList.length > 0 && 
          <ListGroup>
            {featuredList.map((item, index) => {
              return (
                <ListGroup.Item key={index}>
                  <Link to={`/blog/post/${item.urlSlug}`}
                  title="Xem chi tiết"
                  key={index}>
                  {item.title}
                  <span>&nbsp;({item.viewCount} lượt xem)</span>
                  </Link>
                </ListGroup.Item>
              );
            })}
          </ListGroup>
        }
    </div>
  );
}

export default FeaturedPostsWidget;