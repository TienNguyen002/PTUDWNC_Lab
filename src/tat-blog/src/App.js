import './App.css';
import Layout from './Pages/User/Layout';
import Index from './Pages/User/Index';
import About from './Pages/User/About';
import Contact from './Pages/User/Contact';
import Rss from './Pages/User/Rss';
import Footer from './Components/Shared/Footer'
import AdminLayout from './Pages/Admin/Layout'
import {
  BrowserRouter as Router, Route, Routes,
} from 'react-router-dom'
import * as AdminIndex from './Pages/Admin/Index';
import Authors from './Pages/Admin/Authors';
import Categories from './Pages/Admin/Categories';
import Comments from './Pages/Admin/Comments';
import Posts from './Pages/Admin/Post/Posts';
import Tags from './Pages/Admin/Tags';
import NotFound from './Pages/Shared/NotFound';
import BadRequest from './Pages/Shared/BadRequest';
import Edit from './Pages/Admin/Post/Edit';

function App() {
  return (
    <Router>
      <Routes>
        <Route path='*' element={<NotFound/>}/>
        <Route path='/400' element={<BadRequest/>}/>
        <Route path='/' element={<Layout />}>
          <Route path='/' element={<Index />} />
          <Route path='blog' element={<Index />} />
          <Route path='blog/Contact' element={<Contact />} />
          <Route path='blog/About' element={<About />} />
          <Route path='blog/RSS' element={<Rss />} />
        </Route>
        <Route path='/admin' element={<AdminLayout/>}>
          <Route path='/admin' element={<AdminIndex.default/>}/>
          <Route path='/admin/authors' element={<Authors/>}/>
          <Route path='/admin/categories' element={<Categories/>}/>
          <Route path='/admin/comments' element={<Comments/>}/>
          <Route path='/admin/posts' element={<Posts/>}/>
          <Route path='/admin/posts/edit' element={<Edit/>}/>
          <Route path='/admin/posts/edit/:id' element={<Edit/>}/>
          <Route path='/admin/tags' element={<Tags/>}/>
        </Route>
      </Routes>
      <Footer/>
    </Router>
  );
}

export default App;