import React from 'react';

const Book = (props) => {

    const borrow = () => {
        fetch("/books/borrow/" + props.book.id)
            .then(
              (result) => {
                console.log(result);
                window.location.reload();
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
                window.location.reload();
              },
              (error) => {
                console.log(error);
              }
            )
      }

      const returnBook = () => {
        fetch("/books/return/" + props.book.id)
            .then(
              (result) => {
                console.log(result);
                window.location.reload();
              },
              (error) => {
                console.log(error);
              }
            )
      }

    return (
        <div className="col-3">
            <div className="border p-2">
                <h5>{props.book.title}</h5>
                <div>Author: {props.book.author}</div>
                <div>Publisher: {props.book.publisher}</div>
                <div>Publishing Year: {new Date(props.book.publishDate).getFullYear()}</div>
                <div>Genre: {props.book.genre}</div>
                <div>ISBN code: {props.book.isbn}</div>
                <div>Book Status: {props.book.bookStatus}</div>
                {
                    props.reserve === true && props.book.bookStatus === "Available" ? <a href="#" className="btn btn-warning m-1" onClick={reserve}>Reserve</a> : null
                }
                {
                    props.borrow === true  && props.book.bookStatus === "Reserved" ? <a href="#" className="btn btn-info m-1" onClick={borrow}>Borrow</a> : null
                }
                {
                    props.return === true ? <a href="#" className="btn btn-outline-danger m-1" onClick={returnBook}>Return</a> : null
                }

            </div>
        </div>
    );
}

export default Book;
