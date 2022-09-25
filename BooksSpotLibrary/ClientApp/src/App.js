import React from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import 'bootstrap';
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap/dist/js/bootstrap.js';
import 'bootstrap/js/dist/dropdown';
import './custom.css'
import Home from './components/Home';
import Manager from './components/Manager';

const App = () => {

  return (
      <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/manager' component={Manager} />
      </Layout>
  );
}

export default App;



