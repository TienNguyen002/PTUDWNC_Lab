import { useState, useEffect } from "react";
import ListGroup from "react-bootstrap/ListGroup";
import { Link } from "react-router-dom";
import { getArchives } from "../../../Services/Widgets";

const ArchivesWidget = () => {
  const [archiveList, setArchiveList] = useState([]);
  useEffect(() => {
    getArchives().then(data => {
      if(data){
        setArchiveList(data);
      }
      else{
        setArchiveList([]);
      }
    });
  }, [])

  return (
    <div className="mb-4">
        <h3 className="text-success mb-2">
          Danh sách lưu trữ
        </h3>

        {archiveList.length > 0 && 
          <ListGroup>
            {archiveList.map((item, index) => {
              return (
                <ListGroup.Item key={index}>
                  <Link to={`/blog/archives/${item.year}/${item.month}`}
                  title={item.monthName}
                  key={index}>
                  {item.monthName} {item.year}
                  <span>&nbsp;({item.postCount})</span>
                  </Link>
                </ListGroup.Item>
              );
            })}
          </ListGroup>
        }
    </div>
  );
}

export default ArchivesWidget;