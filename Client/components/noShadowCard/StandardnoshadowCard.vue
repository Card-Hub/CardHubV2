<script setup lang="ts">
import { defineProps } from 'vue';

// https://vuejs.org/guide/typescript/composition-api.html
// this source apples to many of the components in this .vue file, playerhand.vue

const props = defineProps<{
  card?: StandardCard;
}>();
// console.log(props.card);

const getDisplayValue = (value: string) => {
  // check if the value is a number
  if (!isNaN(parseInt(value))) {
    return value;
  } else if (value.toLowerCase() === 'jack') {
    return 'J';
  } else if (value.toLowerCase() === 'queen') {
    return 'Q';
  } else if (value.toLowerCase() === 'king') {
    return 'K';
  } else if (value.toLowerCase() === 'ace') {
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
  return suit.toLowerCase() === 'hearts' || suit.toLowerCase() === 'diamonds' ? 'red' : 'black';
};
</script>

<template>
  <div class="flex flex-wrap justify-center items-center">
    <div class="relative card-size m-2 bg-white rounded-md shadow-md p-2"
    >
      <img :src="getSuitIcon(card.Suit)"
           alt="suit icon"
           class="absolute bottom-2 right-2 w-28 h-28" />
      <div class="absolute top-2 left-2 text-7xl font-bold"
           :style="{ color: getSuitColor(card.Suit) }">
        {{ getDisplayValue(card.Value) }}</div>
    </div>
  </div>
</template>


<style scoped>
.card-size {
  width: 15vw;
  height: 25vw;
  max-height: 250px;
  max-width: 150px;
}

</style>
