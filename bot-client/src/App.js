import React, { useState } from 'react';
import './App.css';
import BoardState from './Models/BoardState';
import Player from './Components/Player';


function App() {
  const [board] = useState(BoardState);
  let degreeAngle = 40;
  let currentAngle = -40;

  return (
    <div className='player-container'>
      {
        board.players.map((player, i) => {
          currentAngle += degreeAngle;
          return <Player key={i} player={player} degreeAngle={currentAngle} />;
        })
      }
    </div>
  );
}

export default App;
