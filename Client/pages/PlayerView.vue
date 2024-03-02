<script setup lang="ts">
import PlayerHand from '../components/PlayerHand.vue';
import SelectedCard from '../components/SelectedCard.vue';
import { ref, onMounted } from 'vue';

type Card = {
  id: number;
  value: string;
  suit: string;
};

const cards = ref<Array<Card> | null>(null);
const playerHand = ref<Card[]>([]);
const selectedCard = ref<Card | null>(null);

//https://vuejs.org/guide/essentials/component-basics.html#passing-props
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

const handleCardClick = (card: Card) => {
  selectedCard.value = card;
};
</script>

<template>
  <div>
    <PlayerHand :playerHand="playerHand" @cardClick="handleCardClick" />
    <h2 class="text-center text-2xl font-bold my-4">Selected Card</h2>
    <SelectedCard :selectedCard="selectedCard" />
  </div>
</template>

<style scoped>

</style>
