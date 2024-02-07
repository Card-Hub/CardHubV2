<script setup lang="ts">
import { defineProps, defineEmits, ref } from 'vue';
import CardDisplay from './CardDisplay.vue';

type Card = {
  id: number
  value: string
  suit: string
};

const props = defineProps({
  playerHand: {
    type: Array as PropType<Card[]>,
    required: true
  },
});

const selectedCard = ref<Card | null>(null);
const emits = defineEmits();

const handleCardClick = (card: Card) => {
  selectedCard.value = card;
  emits('cardClick', card);
};
</script>

<template>
  <div class="player-hand">
    <CardDisplay v-for="card in playerHand"
                 :key="card.id"
                 :card="card"
                 :isSelected="selectedCard === card"
                  @cardClicked="handleCardClick"
                  />
  </div>
</template>


<style scoped>
  .player-hand {
    display: flex;
    justify-content: space-evenly;
    flex-wrap: wrap;
  }
</style>
