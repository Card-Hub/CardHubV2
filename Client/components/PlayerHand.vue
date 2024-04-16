<script setup lang="ts">
import { ref } from "vue";
import { useWebSocketStore } from "~/stores/webSocketStore";
import UNOCardDisplay from "~/components/Card/UNOCardDisplay.vue";
import { UnoColor, UnoValue } from "~/types";

const store = useWebSocketStore();
const { cards, canSelectWildColor } = storeToRefs(store);
const { sendCard, sendColor } = store;

const selectedCard = ref<UNOCard | null>(null);

const isWildSelectorVisible = computed<boolean>(() => {
  if (selectedCard.value === null) return false;
  return (selectedCard.value.value === UnoValue.Wild || selectedCard.value.value === UnoValue.WildDrawFour) && canSelectWildColor.value;
});

const handleCardClick = (card: UNOCard) => {
  selectedCard.value = card;
  sendCard(card);
};

const handleColorSelection = (color: UnoColor) => {
  // if (selectedCard.value === null) return;
  // selectedCard.value.color = color;
  sendColor(color);
  canSelectWildColor.value = false;
};

</script>

<template>
  <div class="player-hand">
    <!--    <component v-for="card in playerHand"-->
    <!--                 :key="card.id"-->
    <!--                 :is="getCardComponent(card)"-->
    <!--                 :card="card"-->
    <!--                 :isSelected="selectedCard === card"-->
    <!--                  @cardClicked="handleCardClick"-->
    <!--                  />-->

    <UNOCardDisplay v-for="card in cards"
                    :card="card"
                    :isSelected="selectedCard === card"
                    @cardClicked="handleCardClick"
    />
  </div>
  <div v-if="true" class="grid-container">
    <div class="grid-item blue" @click="handleColorSelection(UnoColor.Blue)"></div>
    <div class="grid-item red" @click="handleColorSelection(UnoColor.Red)"></div>
    <div class="grid-item green" @click="handleColorSelection(UnoColor.Green)"></div>
    <div class="grid-item yellow" @click="handleColorSelection(UnoColor.Yellow)"></div>
  </div>
</template>

<style scoped>
.player-hand {
  display: flex;
  justify-content: space-evenly;
  flex-wrap: wrap;
}

.grid-container {
  display: grid;
  grid-template-columns: 1fr 1fr;
  grid-gap: 10px; /* spacing between grid items */
  width: 120px;
  height: 120px;
}

.grid-item {
  height: 100px;
}

.blue {
  background-color: blue;
}

.red {
  background-color: red;
}

.green {
  background-color: green;
}

.yellow {
  background-color: yellow;
}
</style>
