<script setup lang="ts">
import {defineComponent, ref, onMounted} from "vue";
import {storeToRefs} from "pinia";
import {useWebSocketStore} from "~/stores/webSocketStore";

import UNEnoshadowCard from "~/components/noShadowCard/UNEnoshadowCard.vue";

const store = useWebSocketStore();
// uncomment these out later
// const {  cards, users, room } = storeToRefs(store);
// const { cards } = storeToRefs(store);

const newCards = ref<number[]>([]);

// object of players with information about avatar, name, and cards
interface Player {
  name: string;
  avatar: string;
  cards: number[];
}

const players = ref<Player[]>([
  {name: "juno", avatar: "juno", cards: [1, 2, 3, 4, 5]},
  {name: "fairy", avatar: "fairy", cards: [6, 7, 8, 9, 10]},
  {name: "oli", avatar: "oli", cards: [11, 12, 13, 14, 15]},
  { name: "femaleJuno", avatar: "femaleJuno", cards: [16, 17, 18, 19, 20] },
  {name: "andy", avatar: "andy", cards: [12, 22, 19]},
  {name: "lyssie", avatar: "lyssie", cards: [1, 2, 3, 4, 5]},
  {name: "star", avatar: "star", cards: [6, 7, 8, 9, 10]},
  {name: "rubi", avatar: "ruby", cards: [11, 12, 13, 14, 15]},
]);

const currentTurn = ref<string>("fairy");

// placeholder bc i don't want to use websockets every time
const cards = ref<UNOCard[]>([
  {id: 1, color: "red", value: "0"},
  {id: 2, color: "red", value: "1"},
  {id: 3, color: "red", value: "2"},
  {id: 4, color: "red", value: "3"},
  {id: 15, color: "red", value: "Skip"},
  {id: 16, color: "red", value: "Reverse"},
  {id: 17, color: "red", value: "Draw Two"},
  {id: 18, color: "red", value: "Skip All"},
  {id: 19, color: "yellow", value: "0"},
  {id: 20, color: "yellow", value: "1"},
  {id: 21, color: "yellow", value: "2"},
  {id: 22, color: "yellow", value: "3"},
  {id: 23, color: "yellow", value: "4"},
  {id: 24, color: "yellow", value: "5"},
  {id: 25, color: "yellow", value: "6"},
  {id: 26, color: "yellow", value: "7"},
  {id: 27, color: "blue", value: "Wild Draw Four"}]);

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
  let randomX = num * 5;
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
  
  if (totalPlayers === 2) {
    xpos = index === 0 ? 50 : -51;
    ypos = index === 0 ? -300 : 300;
  }else if(totalPlayers === 3){
    xpos = index === 0 ? 151 : index === 1 ? -251 : 251;
    ypos = index === 0 ? -300 : index === 1 ? 300 : 300;
  } else if (totalPlayers === 4) {
    xpos = index === 0 ? 150 : index === 1 ? -526 : index === 2 ? -56 : 426;
    ypos = index === 0 ? -300 : index === 1 ? 0 : index === 2 ? 300 : 0;
  } else if (totalPlayers === 5) {
    xpos = index === 0 ? 225 : index === 1 ? -475 : index === 2 ? -251 : index === 3 ? 151 : 375;
    ypos = index === 0 ? -300 : index === 1 ? 0 : index === 2 ? 300 : index === 3 ? 300 : 0;
  } else if (totalPlayers === 6) {
    xpos = index === 0 ? 500 : index === 1 ? -70 : index === 2 ? -525 : index === 3 ? -300 : index === 4 ? 60 : 325;
    ypos = index === 0 ? -300 : index === 1 ? -300 : index === 2 ? 0 : index === 3 ? 300 : index === 4 ? 300 : 0;
  } else if (totalPlayers === 7) {
    xpos = index === 0 ? 550 : index === 1 ? -25 : index === 2 ? -425 : index === 3 ? -530 : index === 4 ? -350 : index === 5 ? 0 : 265;
    ypos = index === 0 ? -300 : index === 1 ? -300 : index === 2 ? -150 : index === 3 ? 150 : index === 4 ? 300 : index === 5 ? 300 : 0;
  } else if (totalPlayers === 8) {
    xpos = index === 0 ? 550 : index === 1 ? -25 : index === 2 ? -425 : index === 3 ? -530 : index === 4 ? -350 : index === 5 ? 0 : index === 6 ? 265: 150;
    ypos = index === 0 ? -300 : index === 1 ? -300 : index === 2 ? -150 : index === 3 ? 150 : index === 4 ? 300 : index === 5 ? 300 : index === 6 ? 150: -150;
  }
  return {
    transform: `translate(${xpos}%, ${ypos}%)`,
  };
};

const isCurrentPlayer = (player: string) => {
  if (player.toLowerCase() === currentTurn.value.toLowerCase()) {
    return {
      border: 'red 3px solid',
      boxShadow: '0 0 10px #D60E26',
    };
  }
};

const getUNE = () => {
  return new URL(`../../assets/icons/unoDeck/UNE.svg`, import.meta.url);
};
how to 
</script>

<template>
  <div class="gameboard-container">
    <div class="gameboard">
      <div class="player-icons">
        <div class="player-icon" v-for="(player, index) in players" :key="index"
             :style="{ ...getPlayerIconStyle(index), ...isCurrentPlayer(player.name) }">
          <img :src="getPlayerIcon(player.avatar)" alt="Player Icon" class="player-icon-img"/>
          <p class="player-name"> {{ player.name }} </p>
        </div>
      </div>

      <div class="game-table rounded-tr-full shadow-lg">

        <div class="column-container">
          <div class="column left-column">
            <div class="deck-view">
              <!--            <div class="flex justify-center items-center w-52 h-80 bg-zinc-800 rounded-md shadow-md mb-2">-->
              <div class="deck-card flex justify-center items-center bg-zinc-800 rounded-md shadow-md mb-2"
                   v-for="n in 5" :key="n" :style="cardStyle(n)">
                <img :src="getUNE()" alt="game icon" class="une-logo"/>
              </div>
            </div>
          </div>

          <div class="column right-column">
            <div class="card-pile">
              <UNEnoshadowCard class="singular-card" v-for="(card, index) in cards"
                               :key="card.id"
                               :card="card"
                               :style="{ ...getCardStyle(index) }"
              />
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