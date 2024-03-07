<script setup lang="ts">
import { defineProps, defineEmits, ref } from 'vue';

const props = defineProps({
  playerHand: {
    type: Array as PropType<StandardCard[]>,
    required: true
  },
});

const selectedCard = ref<StandardCard | null>(null);
const emit = defineEmits<{
  cardClick: [card: StandardCard]
}>()

const handleCardClick = (card: StandardCard) => {
  selectedCard.value = card;
  emit('cardClick', card);
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
