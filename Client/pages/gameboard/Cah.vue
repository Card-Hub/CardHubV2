<script setup lang="ts">
import {type ConfigurableDocument, type MaybeElementRef, useFullscreen } from '@vueuse/core';
import {defineComponent, ref, onMounted, type ComputedRef, type Ref, computed} from "vue";
import {storeToRefs} from "pinia";

import {useBaseStore} from "~/stores/baseStore";
import {useCahStore} from "~/stores/cahStore";
// get players from store
const baseStore = useBaseStore();
const { isPlayer, messages, users, room, user, currentAvatar } = storeToRefs(baseStore);
const cahStore = useCahStore();
const { } = storeToRefs(cahStore);

//convert users to CahPlayer



//fulscreen
const { isFullscreen, enter, exit } = useFullscreen();
const el = ref(null)
const { toggle } = useFullscreen(el)

const getPrimeIcon = (name: string) => {
  return new URL(`../../assets/icons/primeIcons/${name}.svg`, import.meta.url);
}

// uncomment these out later
// const {  cards, users, room } = storeToRefs(store);
// const { cards } = storeToRefs(store);



const dealerCards = ref<StandardCard[]>([
  {Id: 1, Suit: "hearts", Value: "Jack"},
  {Id: 2, Suit: "hearts", Value: "Ace"}]);
//reverse dealer cards
dealerCards.value.reverse();

// const players = ref<BlackJackPlayer[]>([
//   {Name: "lyssie", Avatar: "lyssie", Afk: false, Hand: [{Id: 1, Suit: "hearts", Value: "Jack"}, {Id: 2, Suit: "hearts", Value: "Ace"}], CurrentScore: 21, TotalMoney: 100, CurrentBet: 10, HasBet: true, NotPlaying: false, Busted: false, Winner: false, StillPlaying: true, Standing: false},
//   {Name: "juno", Avatar: "juno", Afk: false,Hand: [{Id: 1, Suit: "hearts", Value: "Jack"}, {Id: 2, Suit: "hearts", Value: "Ace"}], CurrentScore: 21, TotalMoney: 100, CurrentBet: 10, HasBet: true, NotPlaying: false, Busted: false, Winner: false, StillPlaying: true, Standing: false},
//   {Name: "oli", Avatar: "oli", Afk: false,Hand: [{Id: 1, Suit: "hearts", Value: "Jack"}, {Id: 2, Suit: "hearts", Value: "Ace"}], CurrentScore: 21, TotalMoney: 100, CurrentBet: 10, HasBet: true, NotPlaying: false, Busted: false, Winner: false, StillPlaying: true, Standing: false},
//   {Name: "liam", Avatar: "liam", Afk: false,Hand: [{Id: 1, Suit: "hearts", Value: "Jack"}, {Id: 2, Suit: "hearts", Value: "Ace"}], CurrentScore: 21, TotalMoney: 100, CurrentBet: 10, HasBet: true, NotPlaying: false, Busted: false, Winner: false, StillPlaying: true, Standing: false},
//   {Name: "andy", Avatar: "andy", Afk: false,Hand: [{Id: 1, Suit: "hearts", Value: "Jack"}, {Id: 2, Suit: "hearts", Value: "Ace"}], CurrentScore: 21, TotalMoney: 100, CurrentBet: 10, HasBet: true, NotPlaying: false, Busted: false, Winner: false, StillPlaying: true, Standing: false},
//   {Name: "alex", Avatar: "alex", Afk: false,Hand: [{Id: 1, Suit: "hearts", Value: "Jack"}, {Id: 2, Suit: "hearts", Value: "Ace"}], CurrentScore: 21, TotalMoney: 100, CurrentBet: 10, HasBet: true, NotPlaying: false, Busted: false, Winner: false, StillPlaying: true, Standing: false},
//   {Name: "ruby", Avatar: "ruby",Afk: false, Hand: [{Id: 1, Suit: "hearts", Value: "Jack"}, {Id: 2, Suit: "hearts", Value: "Ace"}], CurrentScore: 21, TotalMoney: 100, CurrentBet: 10, HasBet: true, NotPlaying: false, Busted: false, Winner: false, StillPlaying: true, Standing: false},
//   {Name: "fairy", Avatar: "fairy", Afk: false,Hand: [{Id: 1, Suit: "hearts", Value: "Jack"}, {Id: 2, Suit: "hearts", Value: "Ace"}], CurrentScore: 21, TotalMoney: 100, CurrentBet: 10, HasBet: true, NotPlaying: false, Busted: false, Winner: false, StillPlaying: true, Standing: false},
// ]);

const currentPlayer = ref<string>("lyssie");
const dealersTurn = ref<boolean>(false);

//const currentColor = ref<string>("red");
// const { currentColor, players, currentPlayer, discardPile  } = storeToRefs(uneStore);
const getCardStyle = (index: number) => {
  let randomX = index * 50;
  let randomY = index * 3;

  return {
    transform: `translate(${randomX}px, ${randomY}px)`,
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

  if (totalPlayers === 2) {
    xpos = index === 0 ? 50 : -51;
    ypos = index === 0 ? -350 : 350;
  } else if (totalPlayers === 3) {
    xpos = index === 0 ? 151 : index === 1 ? -276 : 251;
    ypos = index === 0 ? -350 : index === 1 ? 350 : 350;
  } else if (totalPlayers === 4) {
    xpos = index === 0 ? 150 : index === 1 ? -456 : index === 2 ? -56 : 356;
    ypos = index === 0 ? -350 : index === 1 ? 0 : index === 2 ? 350 : 0;
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
    xpos = index === 0 ? 600 : index === 1 ? 30 : index === 2 ? -375 : index === 3 ? -475 : index === 4 ? -300 : index === 5 ? 50 : index === 6 ? 265 : 150;
    ypos = index === 0 ? -300 : index === 1 ? -300 : index === 2 ? -150 : index === 3 ? 150 : index === 4 ? 300 : index === 5 ? 300 : index === 6 ? 150 : -150;
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
      <div class="game-table rounded-tr-full shadow-lg">
        <div class="card-pile">
          <div v-if="dealersTurn">

          </div>
          <div class="!dealersTurn"></div>
          <StandardnoshadowCard :card="dealerCards[0]" class="standardCardDisplay"/>
          <StandardnoshadowCard :card="dealerCards[1]" class="standardCardDisplay"/>
          <div class="deck-card flex justify-center items-center bg-zinc-800 rounded-md shadow-md mb-2">
            <img :src="getCARD()" alt="game icon" class="une-logo"/>
          </div>
        </div>
        <div class = "blank-card relative w-20 h-32 m-2 bg-zinc-800 rounded-md shadow-md p-2" >
          <img :src="getCARD()" alt="game icon" class="une-logo"/>
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
  width: 15vw;
  height: 25vw;
  max-height: 250px;
  max-width: 150px;
  transform: translate(-55%, 1.5%);
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
  box-shadow: 0 0 20px rgba(0, 0, 0, 0.5);
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