import React, { useState } from 'react';
import './App.css';
import BoardState from './Models/BoardState';


function App() {
  const [board] = useState(BoardState);

  return (
    <div className="App">
      {
        board.players.map(p => {
          return <p>player!</p>
        })
      }    
    </div>
  );
}

export default App;
