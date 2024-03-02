<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useWebSocketStore } from "~/stores/webSocketStore";

const { $api } = useNuxtApp();

const store = useWebSocketStore();
const { connection, isConnected, cards, messages, users, user, room } = storeToRefs(store);
const { tryJoinRoom, sendCard } = store;

const playerHand = ref<StandardCard[]>([]);
const selectedCard = ref<StandardCard | null>(null);

//https://vuejs.org/guide/essentials/component-basics.html#passing-props
onMounted(async () => {
  console.log("in the onMounted for PlayerView");
  try {
    cards.value = await $api("Cards", { method: "GET" });

    // For illustration purposes, add all fetched cards to the player's hand
    playerHand.value = cards.value || [];

    // For illustration purposes, select the first card in the player's hand
    selectedCard.value = playerHand.value[0] || null;
  } catch (error) {
    console.error('Error fetching cards:', error);
  }
});

const handleCardClick = (card: StandardCard) => {
  selectedCard.value = card;
  sendCard(card)
};

console.log("check here for connectivity", isConnected.value);
console.log("check here for obj", connection.value);
</script>

<template>
  <div id="dimScreen">
    <PlayerHand :playerHand="playerHand" @cardClick="handleCardClick" />
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
