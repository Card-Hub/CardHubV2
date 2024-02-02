<script setup lang="ts">
import { defineProps } from 'vue';

  // props are passed from the parent component
  type Card = {
    id: number
    value: string
    suit: string
  };
  
  const props = defineProps({
    playingCards: {
      type: Array as PropType<Array<Card>>,
      required: true
    }
  });
  console.log(props.playingCards)

  const getDisplayValue = (value: string) => {
    if (value === 'Jack') {
      return 'J';
    } else if (value === 'Queen') {
      return 'Q';
    } else if (value === 'King') {
      return 'K';
    } else if (value === 'Ace') {
      return 'A';
    } else {
      return value;
    }
  };

// https://stackoverflow.com/questions/56624817/passing-and-binding-img-src-from-props-in-vue-js
const getSuitIcon = (suit: string) => {
  console.log(`../assets/icons/${suit.toLowerCase()}.svg`);
  return new URL(`../assets/icons/${suit.toLowerCase()}.svg`, import.meta.url);
};

const getSuitColor = (suit: string) => {
  if (suit === 'Hearts' || suit === 'Diamonds') {
    return 'red';
  } else {
    return 'black';
  }
};
</script>

<template>
  <div class="flex flex-wrap justify-center items-center">
    <div
        v-for="card in playingCards"
        :key="card.id"
        class="relative w-20 h-32 m-2 bg-white rounded-md shadow-md p-2"
        style="box-shadow: 6px -6px 3px rgba(200, 200, 200, 0.4);"
    >
      <img
          :src="getSuitIcon(card.suit)"
          alt="suit icon"
          class="absolute bottom-2 right-2 w-14 h-14"
      />
      <div
          class="absolute top-2 left-2 text-4xl font-bold"
          :style="{ color: getSuitColor(card.suit) }" 
          >{{ getDisplayValue(card.value) }}</div>
    </div>
  </div>
</template>

<style scoped>

</style>