<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useWebSocketStore } from "~/stores/webSocketStore";

const store = useWebSocketStore();
const { connection, messages, cards, users, user, room } = storeToRefs(store);
const { sendCard } = store;

// const cards = ref<Array<Card> | null>(null);
const playerHand = ref<StandardCard[]>([]);
const selectedCard = ref<StandardCard | null>(null);

onMounted(async () => {
  try {
    const response = await fetch('https://localhost:7085/Cards');
    if (!response.ok) {
      throw new Error('Network response was not ok');
    }
    cards.value = await response.json();

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
