<script setup lang="ts">
import {defineComponent, ref, onMounted} from "vue";
import {storeToRefs} from "pinia";
import {useWebSocketStore} from "~/stores/webSocketStore";

import StandardCardDisplay from "~/components/Card/StandardCardDisplay.vue";
import StandardnoshadowCard from "~/components/noShadowCard/StandardnoshadowCard.vue";

const store = useWebSocketStore();
const uneStore = useUneStore();
// uncomment these out later
// const {  cards, users, room } = storeToRefs(store);
// const { cards } = storeToRefs(store);

const dealerCards = ref<StandardCard[]>([
  {Id: 1, Suit: "hearts", Value: "Jack"},
  {Id: 2, Suit: "hearts", Value: "Ace"}]);

const newCards = ref<number[]>([]);
//const currentColor = ref<string>("red");
const { currentColor, players, currentPlayer, discardPile  } = storeToRefs(uneStore);
const getCardStyle = (index: number) => {
  const randomX = Math.floor(Math.random() * 50) - 5; // Random offset for X-axis
  const randomY = Math.floor(Math.random() * 50) - 5; // Random offset for Y-axis
  const randomRotation = Math.floor(Math.random() * 20) - 10; // Random rotation

  return {
    transform: `translate(${randomX}px, ${randomY}px) rotate(${randomRotation}deg)`,
    zIndex: index,
  };
};

const cardStyle = (num: number) => {
  let randomX = num * 50;
  let randomY = num * 3;
  let randomRotation = 0;

  return {
    transform: `translate(${randomX}px, ${randomY}px) rotate(${randomRotation}deg)`,
    zIndex: num,
  };
};

const getPlayerIcon = (player: string) => {
  return new URL(`../../assets/icons/avatars/${player}.png`, import.meta.url);
};


const getPlayerIconStyle = (index: number) => {
  const totalPlayers = players.value.length;
  let xpos = 0;
  let ypos = 0;
  
  if (totalPlayers === 2) {//good, might need order change
    xpos = index === 0 ? 180 : -180;
    ypos = index === 0 ? 315 : 315;
  }else if(totalPlayers === 3){//good, might need order change
    xpos = index === 0 ? 151 : index === 1 ? -251 : 251;
    ypos = index === 0 ? 300 : index === 1 ? 300 : 300;
  } else if (totalPlayers === 4) {//good, might need order change
    xpos = index === 0 ? 550 : index === 1 ? 180 : index === 2 ? -180 : -550;
    ypos = index === 0 ? 280 : index === 1 ? 315 : index === 2 ? 315 : 280;
  } else if (totalPlayers === 5) {//good, might need order change
    xpos = index === 0 ? 225 : index === 1 ? -350 : index === 2 ? -251 : index === 3 ? 151 : 250;
    ypos = index === 0 ? 330 : index === 1 ? 220 : index === 2 ? 300 : index === 3 ? 300 : 220;
  } else if (totalPlayers === 6) {//good good
    xpos = index === 0 ? 750 : index === 1 ? 490 : index === 2 ? 185 : index === 3 ? -185 : index === 4 ? -490 : -750;
    ypos = index === 0 ? 200 : index === 1 ? 300 : index === 2 ? 300 : index === 3 ? 300 : index === 4 ? 300 : 200;
  } else if (totalPlayers === 7) {//good good
    xpos = index === 0 ? 750 : index === 1 ? 490 : index === 2 ? 245 : index === 3 ? 0 : index === 4 ? -245 : index === 5 ? -490 : -750;
    ypos = index === 0 ? 200 : index === 1 ? 300 : index === 2 ? 300 : index === 3 ? 300 : index === 4 ? 300 : index === 5 ? 300 : 200;
  } 
  return {
    transform: `translate(${xpos}%, ${ypos}%)`,
  };
};

const isCurrentPlayer = (player: string) => {
  if (currentPlayer.value != "") {
    if (player.toLowerCase() === currentPlayer.value.toLowerCase()) {
      return {
        border: 'red 3px solid',
        boxShadow: '0 0 10px #D60E26',
      };
    }
  }
};

const getCARD = () => {
  return new URL(`../../assets/icons/standardDeck/diamonds.svg`, import.meta.url);
};

const getCurrentColor = () => {
  const color = currentColor.value;
  return "background: ${currentColor.value}";
};
  </script>

<template>
  <div class="gameboard-container">
    <div class="gameboard">
      <div class="player-icons">
        <div class="player-icon" v-for="(player, index) in players" :key="index"
             :style="{ ...getPlayerIconStyle(index), ...isCurrentPlayer(player.Name) }">
          <img :src="getPlayerIcon(player.Avatar)" alt="Player Icon" class="player-icon-img"/>
          <p class="player-name"> {{ player.Name }} </p>
        </div>
      </div>
      <div class="game-table rounded-tr-full shadow-lg" v-bind:class="currentColor">
      <div class="column-container">
        <div class="column left-column">
            <div class="deck-view">
              <div class="deck-card flex justify-center items-center bg-zinc-800 rounded-md shadow-md mb-2">
                <img :src="getCARD()" alt="game icon" class="une-logo"/>
              </div>
            </div>
          </div>
          <div class="column right-column">
            <div class="card-pile">
              <StandardnoshadowCard class="singular-card" v-for="(card, index) in dealerCards"
                               :key="index"
                               :card="card"
                               :style="{ ...getCardStyle(index) }"
              />
              <div class = "blank-card relative w-20 h-32 m-2 bg-white rounded-md shadow-md p-2" >
                
              </div>
            </div>
          </div>


        </div>
      </div>
    </div>
  </div>


</template>

<style scoped>
.gameboard-container {
  width: 100%;
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  background: rgb(243, 19, 19);
  background: radial-gradient(ellipse at center, rgba(243, 19, 19, 0.5) 0%, rgba(152, 14, 17, 0.7) 40%, rgba(63, 8, 14, 0.9) 95%);
}

.gameboard {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  height: 100%;
  width: 100%;
}

.arrow-container {
  position: absolute; /* Use absolute positioning to place arrows relative to the screen */
  top: 100px;
  left: 100px;
  width: 100%;
  height: 100%;
}

.blank-card {
  transform: translate(50px, 3px) rotate(0deg);
  z-index: 1;
}


.player-name {
  font-size: 1.5rem;
  font-weight: bold;
  color: black;
  text-shadow: 2px 2px 4px #000000;
}

.card-pile {
  display: flex;
  justify-content: center;
  align-items: center;
  padding-bottom: 10%;
}

.singular-card {
  justify-self: center;
  align-self: center;
  position: absolute;

  transition: transform 0.5s;
}

.column-container {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  height: 75%;
  width: 75%;
}

.left-column, .right-column {
  flex: 1;
  align-items: center;
  justify-content: center;
  display: flex;
}

.deck-view {
  position: relative;
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100%;
  width: 100%;
}

.deck-card {
  position: absolute;
  border: grey 2px solid;
  transition: transform 0.5s;
  width: 20vw;
  height: 30vw;
  max-height: 300px;
  max-width: 200px;
}

.game-table {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 50%;
  max-height: 800px;
  width: 80%;
  max-width: 1000px;
  /*background-color: #f3f4f6;*/
  border-radius: 50% / 100%;
  box-shadow: 0 0 20px rgba(0, 0, 0, 0.1);
}

.une-logo {
  width: 100%;
  height: 100%;
  max-height: 300px;
  max-width: 200px;
}

.player-icons {
  position: absolute;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  display: flex;
  justify-content: center;
  align-items: center;
}

.player-icon {
  width: 100px; /* Adjust the size of the player icon */
  height: 100px; /* Adjust the size of the player icon */
  border-radius: 50%; /* Ensure the player icon is circular */
  overflow: hidden; /* Ensure the player icon is circular */
  margin-right: 10px; /* Adjust the spacing between player icons */
}

.player-icon-img {
  width: 100%;
  height: 100%;
  object-fit: cover; /* Ensure the player icon fills the circular container */
  background: rgba(255, 255, 255, 0.2); /* Adjust the background color of the player icon and opacity */
}

</style>