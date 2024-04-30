<script setup lang="ts">
import { defineProps } from 'vue';

// https://vuejs.org/guide/typescript/composition-api.html
// this source apples to many of the components in this .vue file, playerhand.vue

const props = defineProps<{
  card?: StandardCard;
}>();
// console.log(props.card);

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
  return new URL(`../../assets/icons/standardDeck/${suit.toLowerCase()}.svg`, import.meta.url);
};


const getSuitColor = (suit: string) => {
  return suit.toLowerCase() === 'hearts' || suit === 'diamonds' ? 'red' : 'black';
};
</script>

<template>
  <div class="flex flex-wrap justify-center items-center">
    <div class="relative w-20 h-32 m-2 bg-white rounded-md shadow-md p-2"
    >
      <img :src="getSuitIcon(card.Suit)"
           alt="suit icon"
           class="absolute bottom-2 right-2 w-14 h-14" />
      <div class="absolute top-2 left-2 text-4xl font-bold"
           :style="{ color: getSuitColor(card.Suit) }">
        {{ getDisplayValue(card.Value) }}</div>
    </div>
  </div>
</template>


<style scoped>

</style>
