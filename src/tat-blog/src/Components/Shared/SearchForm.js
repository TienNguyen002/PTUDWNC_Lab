import { useState } from "react";
import Form from 'react-bootstrap/Form'
import  Button  from "react-bootstrap/Button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faSearch } from "@fortawesome/free-solid-svg-icons";
import { FormControl } from "react-bootstrap";

const SearchForm = () => {
  const [keyword, setKeyword] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();
    window.location = `/blog?k=${keyword}`;
  }

  return(
    <div className="mb-4">
      <h3 className="text-success mb-2">
          Tìm kiếm bài viết
        </h3>
      <Form method="get" onSubmit={handleSubmit}>
        <Form.Group className="input-group mb-3">
          <FormControl
          type="text"
          name="k"
          value={keyword}
          onChange = {(e) => setKeyword(e.target.value)}
          aria-label = 'Nhập vào từ khoá'
          aria-describedby="btnSearchPost"
          placeholder="Nhập vào từ khoá"/>

          <Button
          id='btnSearchPost'
          variant = 'outline-secondary'
          type='submit'>
            <FontAwesomeIcon icon={faSearch}/>
          </Button>

        </Form.Group>

      </Form>
    </div>
  )
}

export default SearchForm;