import './App.css';
import Navbar from './Components/Shared/Navbar';
import Sidebar from './Components/Shared/Sidebar';
import Footer from './Components/Shared/Footer';
import Layout from './Pages/Layout';
import Index from './Pages/Index';
import About from './Pages/About';
import Contact from './Pages/Contact';
import Rss from './Pages/Rss';
import {
  BrowserRouter as Router, Route, Routes,
} from 'react-router-dom'

function App() {
  return (
    <div>

      <Router>
        <Navbar />
        <div className='container-fluid'>
          <div className="row">
            <div className="col-9">
              <Routes>
                <Route path='/' element={<Layout/>}>
                  <Route path='/' element={<Index/>}/>
                  <Route path='blog' element={<Index/>}/>
                  <Route path='blog/Contact' element={<Contact/>}/>
                  <Route path='blog/About' element={<About/>}/>
                  <Route path='blog/RSS' element={<Rss/>}/>
                </Route>
              </Routes>
            </div>
            <div className='col-3 border-start'>
              <Sidebar />
            </div>
          </div>
        </div>
        <Footer />
      </Router>
    </div>
  );
}

export default App;