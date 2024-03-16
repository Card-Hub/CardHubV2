<script setup lang="ts">
import { defineProps } from 'vue';
import StandardCardDisplay from './Card/StandardCardDisplay.vue';
import UNOCardDisplay from "~/components/Card/UNOCardDisplay.vue";

const props = defineProps(['selectedCard']);

// determine which type of card to display
const getCardComponent = (card: Card) => {
  if (isStandardCard(card)) {
    return StandardCardDisplay;
  } else if (isUNOCard(card)) {
    return UNOCardDisplay;
  } else {
    throw new Error('Invalid card type');
  }
};

// helper functions to determine card type
const isStandardCard = (card: Card): card is StandardCard => {
  return 'suit' in card;
};

const isUNOCard = (card: Card): card is UNOCard => {
  return 'color' in card;
};
// add more as needed
</script>

<template>
  <div class="selected-card">
    <UNOCardDisplay v-if="selectedCard" is-selected="1 === 1" :card="selectedCard" />
  </div>
</template>

<style scoped>
  .selected-card {
    display: flex;
    justify-content: center;
    align-items: center;
  }
</style>
