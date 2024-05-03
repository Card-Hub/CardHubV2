<script setup lang="ts">
import { ref } from "vue";
import { storeToRefs } from "pinia";
import { CahType } from "~/types";


const baseStore = useBaseStore();
const { isPlayer, messages, users, room, user, currentAvatar } = storeToRefs(baseStore);

const cahStore = useCahStore();
const {} = storeToRefs(cahStore);


const { isFullscreen, enter, exit } = useFullscreen();
const el = ref(null);
const { toggle } = useFullscreen(el);

const getPrimeIcon = (name: string) => {
  return new URL(`../../assets/icons/primeIcons/${ name }.svg`, import.meta.url);
};

const whiteCards = ref<CahCard[]>([
  { text: 'Hello', type: CahType.White },
  { text: 'UR mom', type: CahType.White },
  { text: 'uhhhh', type: CahType.White }
]);

const blackCard = ref<CahCard>({ text: 'Hello', type: CahType.Black });

const currentPlayer = ref<string>("lyssie");
const dealersTurn = ref<boolean>(false);


const getCardStyle = (index: number) => {
  let randomX = index * 50;
  let randomY = index * 3;

  return {
    transform: `translate(${ randomX }px, ${ randomY }px)`
  };
};

const cardStyle = (num: number) => {
  let randomX = num * 50;
  let randomY = num * 3;
  let randomRotation = 0;

  return {
    transform: `translate(${ randomX }px, ${ randomY }px) rotate(${ randomRotation }deg)`,
    zIndex: num
  };
};

const getPlayerIcon = (player: string) => {
  return new URL(`../../assets/icons/avatars/${ player }.png`, import.meta.url);
};

const isCurrentPlayer = (player: string) => {
  if (currentPlayer.value != "") {
    if (player.toLowerCase() === currentPlayer.value.toLowerCase()) {
      return {
        border: "red 3px solid",
        boxShadow: "0 0 10px #D60E26"
      };
    }
  }
};

const cardBlackStyle = () => {
  return {
    transform: `translate(0%, 0%) rotate(0deg)`,
    zIndex: 1
  };
};

const getCARD = () => {
  return new URL(`../../assets/icons/standardDeck/diamonds.svg`, import.meta.url);
};
</script>

<template>
  <div class="gameboard-container">
    <div class="gameboard">
<!--      <div class="player-icons">-->
<!--        <div class="player-icon" v-for="(player, index) in players" :key="index"-->
<!--             :style="{ ...getPlayerIconStyle(index), ...isCurrentPlayer(player.Name) }">-->
<!--          <img :src="getPlayerIcon(player.Avatar)" alt="Player Icon" class="player-icon-img"/>-->
<!--          <p class="player-name"> {{ player.Name }} </p>-->
<!--        </div>-->
<!--      </div>-->
      
      <div>
        <CahDisplay :card="blackCard" :style="cardBlackStyle()" />
        <CahDisplay v-for="(card, index) in whiteCards" :key="index" :card="card" :style="cardStyle(index)" />
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