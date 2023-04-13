import { useState } from "react";
import Form from 'react-bootstrap/Form'
import  Button  from "react-bootstrap/Button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPaperPlane } from "@fortawesome/free-solid-svg-icons";
import { FormControl } from "react-bootstrap";

const NewsletterForm = () => {
  const [email, setEmail] = useState('');

  const handleSubmit = (e) => {
    e.preventDefault();
    window.location = `/blog`;
  }

  return(
    <div className="mb-4">
      <h3 className="text-success mb-2">
          Đăng ký nhận thông báo
        </h3>
      <Form method="get" onSubmit={handleSubmit}>
        <Form.Group className="input-group mb-3">
          <FormControl
          type="email"
          value={email}
          onChange = {(e) => setEmail(e.target.value)}
          aria-label = 'Nhập email để đăng ký'
          aria-describedby="btnSend"
          placeholder="Nhập email để đăng ký"/>

          <Button
          id='btnSend'
          variant = 'outline-secondary'
          type='submit'>
            <FontAwesomeIcon icon={faPaperPlane}/>
          </Button>

        </Form.Group>

      </Form>
    </div>
  )
}

export default NewsletterForm;