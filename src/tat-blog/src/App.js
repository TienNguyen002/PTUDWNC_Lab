import './App.css';
import Navbar from './Components/Navbar';
import Sidebar from './Components/Sidebar';
import Footer from './Components/Footer';
import {
  BrowserRouter as Router,
} from 'react-router-dom'

function App(){
  return (
    <div>
      <Router>
        <Navbar />
        <div className='container-fluid'>
          <div className="row">
            <div className="col-9">

            </div>
            <div className='col-3 border-start'>
              <Sidebar/>
            </div>
          </div>
        </div>
        <Footer/>
      </Router>
    </div>
  );
}

export default App;