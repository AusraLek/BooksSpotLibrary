import React, { useState, useEffect } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import 'bootstrap';
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap/dist/js/bootstrap.js';
import 'bootstrap/js/dist/dropdown';
import './custom.css'
import Book from './components/Book';

const App = () => {
  const [books, setBooks] = useState([]);
  const options = [
    {value: "bytitle", name: "By Title"},
    {value: "byauthor", name: "By Author"},
    {value: "bypublisher", name: "By Publisher"},
    {value: "bygenre", name: "By Genre"},
    {value: "byisbn", name: "By ISBN Code"},
    {value: "byyear", name: "By Publishing Year"}
  ];
  const [searchText, setSearchText] = useState("");
  const [searchCategoryId, setSearchCategoryId] = useState(0);

  const getBooks = () => {
    fetch("/books/search")
        .then(res => res.json())
        .then(
          (result) => {
            setBooks(result);
          },
          (error) => {
            console.log(error);
          }
        )
  }

  const searchBook = () => {
    fetch("/books/search/" + options[searchCategoryId].value + "/" + searchText)
        .then(res => res.json())
        .then(
          (result) => {
            setBooks(result);
          },
          (error) => {
            console.log(error);
          }
        )
  }

  const updateInputValue = (evt) => {
    const val = evt.target.value;
    setSearchText(val);
  }

  useEffect(() => getBooks(), []);

  return (
      <Layout>
        <div className="row">
          <div className="input-group mb-3">
          <input type="text" className="form-control" onChange={evt => updateInputValue(evt)} aria-label="Text input with dropdown button"/>
          <button className="btn btn-outline-secondary dropdown-toggle" type="button" data-bs-toggle="dropdown" aria-expanded="false">Search {options[searchCategoryId].name}</button>
          <ul className="dropdown-menu dropdown-menu-end">
          {
            options.map((option, idx) => (
              <li key={idx}><a className="dropdown-item" onClick={() => setSearchCategoryId(idx)}>{option.name}</a></li>
            ))
          }
          </ul>
          <button type="button" className="btn btn-secondary btn-lg" onClick={searchBook}>Search</button>
        </div>
        </div>
        <div className="row">
          {
            books.map(item => (<Book book={item}/>))
          }
        </div>
      </Layout>
  );
}

export default App;

/*
<Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/fetch-data' component={FetchData} />
        
*/