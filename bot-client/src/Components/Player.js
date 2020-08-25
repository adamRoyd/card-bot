import React from 'react';

const Player = ({player, degreeAngle}) => {
    return(
        <div className='player' style={{transform:`rotate(${degreeAngle+9}deg) translate(15em) rotate(-${degreeAngle}deg)`}}>
            <p>{player.name}</p>
            <p>{degreeAngle}</p>
        </div>
    )
}

export default Player;