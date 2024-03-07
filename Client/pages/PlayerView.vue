<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useWebSocketStore } from "~/stores/webSocketStore";

// const { $api } = useNuxtApp();

const store = useWebSocketStore();
const { connection, isConnected, cards, messages, users, user, room } = storeToRefs(store);
const { tryJoinRoom, sendCard, drawCard } = store;

// create standard deck of cards
// const standardDeck = [];
// const suits = ["hearts", "diamonds", "clubs", "spades"];
// const values = ["2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace"];
//
// for (const suit of suits) {
//   for (const value of values) {
//     standardDeck.push({
//       id: standardDeck.length + 1,
//       suit,
//       value
//     });
//   }
// }

// const playerHand = ref<StandardCard[]>(standardDeck);
// const selectedCard = ref<StandardCard | null>(null);

// create uno deck of cards
// const unoDeck = [];
// const colors = ["#d12c15", "#ffce30", "#7abb18", "#1166ac"];
// const unoValues = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "1", "2", "3", "4", "5", "6", "7", "8", "9", "Skip", "Reverse", "Draw Two", "Skip All", "Skip", "Reverse", "Draw Two", "Skip All"];
//
// for (const color of colors) {
//   for (const value of unoValues) {
//     unoDeck.push({
//       id: unoDeck.length + 1,
//       color,
//       value
//     });
//   }
// }
//
// // push 4 wild cards and 4 draw 4 wild cards
// for (let i = 0; i < 4; i++) {
//   unoDeck.push({
//     id: unoDeck.length + 1,
//     color: "#151515",
//     value: "Wild"
//   });
//   unoDeck.push({
//     id: unoDeck.length + 1,
//     color: "#151515",
//     value: "Wild Draw Four"
//   });
// }

// const playerHand = ref<UNOCard[]>(unoDeck);
// const selectedCard = ref<Card | null>(null);
const selectedCard = ref<UNOCard | null>(null);

//https://vuejs.org/guide/essentials/component-basics.html#passing-props
// onMounted(async () => {
//   console.log("in the onMounted for PlayerView");
//   try {
//     cards.value = await $api("Cards", { method: "GET" });
//
//     // For illustration purposes, add all fetched cards to the player's hand
//     playerHand.value = cards.value || [];
//
//     // For illustration purposes, select the first card in the player's hand
//     selectedCard.value = playerHand.value[0] || null;
//   } catch (error) {
//     console.error('Error fetching cards:', error);
//   }
// });

// const handleCardClick = (card: StandardCard) => {
//   selectedCard.value = card;
//   sendCard(card)
// };

// const handleCardClick = <C extends Card>(card: C) => {
//   selectedCard.value = card;
//   sendCard(card);
// };

const handleCardClick = (card: UNOCard) => {
  selectedCard.value = card;
  sendCard(card);
};

// Websockets stuff
console.log("check here for connectivity", isConnected.value);
console.log("check here for obj", connection.value);
</script>

<template>
  <div id="dimScreen">
    <Button @click="drawCard">Draw Card</Button>
    <PlayerHand :playerHand="cards" @cardClick="handleCardClick" />
    <h2 class="text-center text-2xl font-bold my-4">Selected Card</h2>
    <SelectedCard :selectedCard="selectedCard" />
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
