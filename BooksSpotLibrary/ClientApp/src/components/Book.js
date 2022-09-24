import React from 'react';

const Book = (props) => {

    const borrow = () => {
        fetch("/books/borrow/" + props.book.id)
            .then(
              (result) => {
                console.log(result);
              },
              (error) => {
                console.log(error);
              }
            )
      }

      const reserve = () => {
        fetch("/books/reserve/" + props.book.id)
            .then(
              (result) => {
                console.log(result);
              },
              (error) => {
                console.log(error);
              }
            )
      }

    return (
        <div className="col-3">
            <div className="border border-info p-2">
                <h5 className="text-info">{props.book.title}</h5>
                <div>Author: {props.book.author}</div>
                <div>Publisher: {props.book.publisher}</div>
                <div>Publishing Year: {new Date(props.book.publishDate).getFullYear()}</div>
                <div>Genre: {props.book.genre}</div>
                <div>ISBN code: {props.book.isbn}</div>
                <div>Book Status: {props.book.bookStatus}</div>
                <a href="#" className="btn btn-warning m-2" onClick={reserve}>Reserve</a>
                <a href="#" className="btn btn-info" onClick={borrow}>Borrow</a>
            </div>
        </div>
    );
}

export default Book;
