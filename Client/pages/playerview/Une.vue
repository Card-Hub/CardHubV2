<script setup lang="ts">
import {defineComponent, ref, onMounted} from "vue";
import {storeToRefs} from "pinia";
import {useWebSocketStore} from "~/stores/webSocketStore";

import UNEnoshadowCard from "~/components/noShadowCard/UNEnoshadowCard.vue";
import UNOCardDisplay from "~/components/Card/UNOCardDisplay.vue";

const store = useWebSocketStore();
 const {users, room } = storeToRefs(store);
const uneStore = useUneStore();
 const { currentPlayer, yourCards } = storeToRefs(uneStore);

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

</script>


<template>
  <p>hi</p>
  <div class="playerViewContainer">
    <UNOCardDisplay v-for="card in yourCards"
                      :key="card.id"
                      :card="card"
      />
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