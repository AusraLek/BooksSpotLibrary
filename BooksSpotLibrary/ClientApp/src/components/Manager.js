import React, { useState, useEffect } from 'react';
import Book from './Book';

const Manager = () => {
    const [books, setBooks] = useState([]);

    const getBooks = () => {
        const requestOptions = {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({status:["Borrowed"], text: ""})
      };
        fetch("/books/search", requestOptions)
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
    
    useEffect(() => getBooks(), []);

    return (
        <div className="row">
            {
            books.map(item => (<Book key={item.id} book={item} return={true}/>))
            }
        </div>
  );
}

export default Manager;