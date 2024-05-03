<script setup lang="ts">
import { ref } from 'vue';
import StandardCardDisplay from './Card/StandardCardDisplay.vue';
import UNOCardDisplay from "~/components/Card/UNECardDisplay.vue";
// import { useWebSocketStore } from "~/stores/webSocketStore";
import { storeToRefs } from "pinia";

// const store = useWebSocketStore();
const { connection, isConnected, cards, messages, users, user, room } = storeToRefs(store);
const { tryJoinRoom, sendCard, drawCard } = store;

const selectedCard = ref<UNOCard | null>(null);

const handleCardClick = (card: UNOCard) => {
  selectedCard.value = card;
  sendCard(card);
};

// convert cards into uno cards
const unoCards = cards.value.map((card) => {
  return {
    Id: card.Id,
    Color: card.Color,
    Value: card.Value
  };
});

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
  return 'suit' in card && 'value' in card;
};

const isUNOCard = (card: Card): card is UNOCard => {
  return 'color' in card && 'value' in card;
};

</script>

<template>
  <div class="player-hand">
<!--    <component v-for="card in cards"-->
<!--                 :key="card.id"-->
<!--                 :is="getCardComponent(card)"-->
<!--                 :card="card"-->
<!--                 :isSelected="selectedCard === card"-->
<!--                  @cardClicked="handleCardClick"-->
<!--                  />-->
    <UNOCardDisplay v-for="card in unoCards"
               :key="card.Id"
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
