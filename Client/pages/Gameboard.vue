<script setup lang="ts">
import { defineComponent } from "vue";
import { storeToRefs } from "pinia";
import { useWebSocketStore } from "~/stores/webSocketStore";

import SelectedCard from "~/components/SelectedCard.vue";
import UNOCardDisplay from "~/components/Card/UNOCardDisplay.vue";
import StandardCardDisplay from "~/components/Card/StandardCardDisplay.vue";

const store = useWebSocketStore();
const {  cards, users, room } = storeToRefs(store);
const { sendCard } = store;

const getCardStyle = (index: number) => {
  const randomX = Math.floor(Math.random() * 10) - 5; // Random offset for X-axis
  const randomY = Math.floor(Math.random() * 10) - 5; // Random offset for Y-axis
  return {
    transform: `translate(${randomX}px, ${randomY}px)`,
    zIndex: index,
  };
};

const selectedCard = ref<UNOCard | null>(null);
// const getCardComponent = (card: UNOCard) => {
//   if (isStandardCard(card)) {
//     return StandardCardDisplay;
//   } else if (isUNOCard(card)) {
//     return UNOCardDisplay;
//   } else {
//     throw new Error('Invalid card type');
//   }
// };

// helper functions to determine card type
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
      <div class="card-pile">
        <UNOCardDisplay class="singular-card" v-for="card in cards"
                        :key="card.id"
                        :is-selected="selectedCard === card"
                        :card="card"
                        :style="getCardStyle(cards.indexOf(card))"
        />
      </div>
    </div>
  </div>
  

</template>

<style scoped>
.gameboard-container {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
}

.gameboard {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  height: 100%;
  width: 100%;
}

.card-pile {
  justify-content: center;
  position: relative;
  width: 75%;
  height: 75%;
}

.singular-card {
  align-self: center;
  position: absolute;
  transition: transform 0.5s;
  transform: translate(2px, 2px);
  z-index: 2;
}
</style>