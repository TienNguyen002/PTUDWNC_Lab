import "./style/contactstyle.css"
import React, { useEffect, useState } from "react";
import Form from 'react-bootstrap/Form'
import Button from "react-bootstrap/Button";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faMap, faPhone, faEnvelope } from "@fortawesome/free-solid-svg-icons";
import { FormControl } from "react-bootstrap";

const Contact = () => {
    useEffect(() => {
        document.title = 'Liên hệ';
    }, []);

    const [name, setName] = useState('');
    const [email, setEmail] = useState('');
    const [title, setTitle] = useState('');
    const [content, setContent] = useState('');

    const handleSubmit = (e) => {
        e.preventDefault();
        window.location = `/blog/contact`;
    }

    return (
        <div class="mb-4">
            <h2 class="h1-responsive font-weight-bold text-center my-4">Liên hệ với chúng tôi</h2>
            <p class="text-center w-responsive mx-auto mb-5">
                Nếu các bạn có bất kỳ thắc mắc nào. Hãy liên hệ với chúng tôi bằng cách gửi thắc mắc đến cho chúng tôi ngay bên dưới!
                <br />Chúng tôi sẽ sớm giải đáp thắc mắc của các bạn!
            </p>
            <div class="row">
                <div id="Form" class="col-md-9 mb-md-0 mb-5">
                    <Form method="get" onSubmit={handleSubmit}>
                        <label for="name" class="">Tên của bạn:</label>
                        <Form.Group>
                            <FormControl
                                type="text"
                                value={name}
                                onChange={(e) => setName(e.target.value)}
                                aria-label='Nhập tên của bạn'
                                aria-describedby="btnSend"
                                placeholder="Nhập tên của bạn" />
                        </Form.Group>
                        <Form.Group>
                            <label for="name" class="">Email của bạn:</label>
                            <FormControl
                                type="email"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                aria-label='Nhập email của bạn'
                                aria-describedby="btnSend"
                                placeholder="Nhập email của bạn" />
                        </Form.Group>
                        <Form.Group>
                            <label for="name" class="">Tiêu đề:</label>
                            <FormControl
                                type="text"
                                value={title}
                                onChange={(e) => setTitle(e.target.value)}
                                aria-label='Nhập tiêu đề'
                                aria-describedby="btnSend"
                                placeholder="Nhập tiêu đề" />
                        </Form.Group>
                        <Form.Group>
                            <label for="name" class="">Nội dung:</label>
                            <FormControl
                                type="textarea"
                                value={content}
                                as={'textarea'}
                                onChange={(e) => setContent(e.target.value)}
                                aria-label='Nội dung bạn muốn gửi'
                                aria-describedby="btnSend"
                                placeholder="Nội dung bạn muốn gửi" />
                        </Form.Group>
                        <div className="text-center">
                            <Button
                                id='btnSend'
                                variant='outline-secondary'
                                type='submit'>
                                Gửi
                            </Button>
                        </div>
                    </Form>
                    <div class="status"></div>
                </div>
                <div class="col-md-3 text-center">
                    <ul class="list-unstyled mb-0">
                        <li>
                            <FontAwesomeIcon icon={faMap} />
                            <p>Chưa cập nhật</p>
                        </li>

                        <li>
                            <FontAwesomeIcon icon={faPhone} />
                            <p>Chưa cập nhật</p>
                        </li>

                        <li>
                            <FontAwesomeIcon icon={faEnvelope} />
                            <p>Chưa cập nhật</p>
                        </li>
                    </ul>
                </div>
                <div>
                    <iframe title="map"
                        src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3903.2877902405253!2d108.44201621412589!3d11.95456563961217!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x317112d959f88991%3A0x9c66baf1767356fa!2zVHLGsOG7nW5nIMSQ4bqhaSBI4buNYyDEkMOgIEzhuqF0!5e0!3m2!1svi!2s!4v1633261535076!5m2!1svi!2s"></iframe>
                </div>
            </div>
            <div class="col-6">
                <div class="card-header">
                </div>
                <div class="card-body">
                </div>
            </div>
        </div>
    );
}

export default Contact;