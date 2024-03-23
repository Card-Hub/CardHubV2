<script setup lang="ts">
import { defineComponent, ref, onMounted } from "vue";
import { storeToRefs } from "pinia";
import { useWebSocketStore } from "~/stores/webSocketStore";

import StandardCardDisplay from "~/components/Card/StandardCardDisplay.vue";
import UNEnoshadowCard from "~/components/noShadowCard/UNEnoshadowCard.vue";

const store = useWebSocketStore();
// uncomment these out later
// const {  cards, users, room } = storeToRefs(store);
// const { cards } = storeToRefs(store);

const newCards = ref<number[]>([]);
const players = ref<string[]>(["juno 0", "julio 1", "julie 2", "julian 3"]);
const currentTurn = ref<string>("julie 2");

// placeholder bc i don't want to use websockets every time
const cards = ref<UNOCard[]>([
  { id: 1, color: "red", value: "0" },
  { id: 2, color: "red", value: "1" },
  { id: 3, color: "red", value: "2" },
  { id: 4, color: "red", value: "3" },
  { id: 15, color: "red", value: "Skip" },
  { id: 16, color: "red", value: "Reverse" },
  { id: 17, color: "red", value: "Draw Two" },
  { id: 18, color: "red", value: "Skip All" },
  { id: 19, color: "yellow", value: "0" },
  { id: 20, color: "yellow", value: "1" },
  { id: 21, color: "yellow", value: "2" },
  { id: 22, color: "yellow", value: "3" },
  { id: 23, color: "yellow", value: "4" },
  { id: 24, color: "yellow", value: "5" },
  { id: 25, color: "yellow", value: "6" },
  { id: 26, color: "yellow", value: "7" },
  { id: 27, color: "blue", value: "Wild Draw Four" }]);

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
  return new URL(`../assets/icons/avatars/juno.png`, import.meta.url);
};

const getPlayerIconStyle = (index: number) => {
  // const totalPlayers = players.value.length;
  // let xpos = 0;
  // let ypos = 0;
  // for(let i = 0; i < totalPlayers; i++){
  //   if (index < totalPlayers/4) { // bottom half of the table
  //     xpos = totalPlayers * 65;
  //     ypos = totalPlayers * (i);
  //   }else if (index < totalPlayers/2){ // right half of the table
  //     xpos = 0;
  //     ypos = totalPlayers * 105;
  //   } else if (index < totalPlayers * 3/4){ // left half of the table
  //     xpos = 0;
  //     ypos = -totalPlayers * 105;
  //   } else { // top half of the table
  //     xpos = -totalPlayers * 70;
  //     ypos = -totalPlayers * i;
  //   }
  //
  // }
  //
  // return {
  //   transform: `translate(${ypos}%, ${xpos}%)`,
  // };
  const totalPlayers = players.value.length;
  const angle = (360 / totalPlayers) * index;
  const radius = 250; // Adjust the radius as needed

  const xpos = Math.cos((angle * Math.PI) / 180) * radius;
  const ypos = Math.sin((angle * Math.PI) / 180) * radius;

  return {
    transform: `translate(${xpos}px, ${ypos}px)`,
  };
};

const isCurrentPlayer = (player: string) => {
  if (player.toLowerCase() === currentTurn.value.toLowerCase()){
    return {
      border: '#D60E26 2px solid',
      boxShadow: '0 0 10px #D60E26',
    };
  }
};

watch(() => store.cards, (newValue, oldValue) => {
  const diff = newValue.length - oldValue.length;
  const newIndexes = Array.from({ length: diff }, (_, i) => i + oldValue.length);
  newCards.value = newIndexes; // Reset the array and then assign new indexes
});

const getUNE = () => {
  return new URL(`../assets/icons/unoDeck/UNE.svg`, import.meta.url);
};

// const getCardComponent = (card: UNOCard) => {
//   if (isStandardCard(card)) {
//     return StandardCardDisplay;
//   } else if (isUNOCard(card)) {
//     return UNOCardDisplay;
//   } else {
//     throw new Error('Invalid card type');
//   }
// };

// // helper functions to determine card type
// const isStandardCard = (card: Card): card is StandardCard => {
//   return 'suit' in card && 'value' in card;
// };
//
// const isUNOCard = (card: Card): card is UNOCard => {
//   return 'color' in card && 'value' in card;
// };
</script>

<template>
  <div class="gameboard-container">
    <div class="gameboard">
      <div class="player-icons">
        <div class="player-icon" v-for="(player, index) in players" :key="index" :style="{ ...getPlayerIconStyle(index), ...isCurrentPlayer(player) }">
          <img :src="getPlayerIcon(player)" alt="Player Icon" class="player-icon-img" />
          <p class="player-name"> {{ player }} </p>
        </div>
      </div>
      
      <div class="game-table rounded-tr-full shadow-lg">
      
        <div class="column-container">
          <div class="column left-column">
            <div class="deck-view">
  <!--            <div class="flex justify-center items-center w-52 h-80 bg-zinc-800 rounded-md shadow-md mb-2">-->
              <div class="deck-card flex justify-center items-center bg-zinc-800 rounded-md shadow-md mb-2" v-for="n in 5" :key="n" :style="cardStyle(n)">
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
  background-color: #f3f4f6;
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
  height:  100px; /* Adjust the size of the player icon */
  border-radius: 50%; /* Ensure the player icon is circular */
  overflow: hidden; /* Ensure the player icon is circular */
  margin-right: 10px; /* Adjust the spacing between player icons */
}

.player-icon-img {
  width: 100%;
  height: 100%;
  object-fit: cover; /* Ensure the player icon fills the circular container */
}

</style>