<script setup lang="ts">
import { defineProps, defineEmits } from 'vue';

// https://vuejs.org/guide/typescript/composition-api.html
// this source apples to many of the components in this .vue file, playerhand.vue

const props = defineProps<{
  card: {
    type: StandardCard,
    required: true
  };
  isSelected: Boolean;
}>();
// console.log(props.card);

//Will be used for when user clicks on a card
const emits = defineEmits();
const handleClick = () => {
  emits('cardClicked', props.card);
};

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
  return new URL(`../assets/icons/${suit.toLowerCase()}.svg`, import.meta.url);
};


const getSuitColor = (suit: string) => {
  return suit === 'Hearts' || suit === 'Diamonds' ? 'red' : 'black';
};
</script>

<template>
  <div class="flex flex-wrap justify-center items-center">
    <div class="relative w-20 h-32 m-2 bg-white rounded-md shadow-md p-2"
         style="box-shadow: 6px -6px 3px rgba(200, 200, 200, 0.4);"
         @click="handleClick"
         :class="{ 'selected': isSelected }"
    >
    <img :src="getSuitIcon(card.suit)"
         alt="suit icon"
         class="absolute bottom-2 right-2 w-14 h-14" />
    <div class="absolute top-2 left-2 text-4xl font-bold"
         :style="{ color: getSuitColor(card.suit) }">
      {{ getDisplayValue(card.value) }}</div>
      </div>
  </div>
</template>


<style scoped>
.selected {
  border: 2px solid red;
}
</style>