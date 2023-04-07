import React from "react";
import { Navbar as Nb, Nav, NavItem } from "react-bootstrap";
import{
    Link
} from 'react-router-dom';

const Navbar = () => {
    return (
        <Nb collapseOnSelect expand='sm' bg='white' variant="light"
        className="border-bottom shadow">
            <div className="container-fluid">
                <Nb.Brand href="/">Tips & Tricks</Nb.Brand>
                <Nb.Toggle aria-controls="responsive-navbar-nav"/>
                <Nb.Collapse id="responsive-navbar-nav" className="d-sm-inline-flex
                justify-content-between">
                    <Nav className="mr-auto flex-grow-1">
                        <NavItem>
                            <Link to='/' className="nav-link text-dark">
                                Trang chủ
                            </Link>
                        </NavItem>
                        <NavItem>
                            <Link to='/blog/about' className="nav-link text-dark">
                                Giới thiệu
                            </Link>
                        </NavItem>
                        <NavItem>
                            <Link to='/blog/contact' className="nav-link text-dark">
                                Liên hệ
                            </Link>
                        </NavItem>
                        <NavItem>
                            <Link to='/blog/rss' className="nav-link text-dark">
                                RSS Feed
                            </Link>
                        </NavItem>
                    </Nav>
                </Nb.Collapse>
            </div>
        </Nb>
    )
}

export default Navbar;