<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useWebSocketStore } from "~/stores/webSocketStore";

// const { $api } = useNuxtApp();

const store = useWebSocketStore();
const { cards, users, user, timer } = storeToRefs(store);
const { tryJoinRoom, sendCard, drawCard } = store;


// const playerHand = ref<UNOCard[]>(unoDeck);
// const selectedCard = ref<Card | null>(null);
const selectedCard = ref<UNOCard | null>(null);

// const handleCardClick = <C extends Card>(card: C) => {
//   selectedCard.value = card;
//   sendCard(card);
// };

const handleCardClick = (card: UNOCard) => {
  selectedCard.value = card;
  sendCard(card);
};


import { computed, watch } from 'vue';



</script>

<template>
  <div id="dimScreen">
    <Button @click="drawCard">Draw Card</Button>
    <PlayerHand />
    <h2 class="text-center text-2xl font-bold my-4">Selected Card</h2>
    <SelectedCard :selectedCard="selectedCard" />
    <p>Timer => {{ timer }}</p>
  </div>
</template>

<style scoped>

#dimScreen {
  width: 100%;
  height: 100%;
  background: linear-gradient(20deg, #000000 0%, #313134 100%);
  position: absolute;
  top: 0;
  left: 0;
  z-index: 100;
}
</style>
