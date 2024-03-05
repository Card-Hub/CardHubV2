<script setup lang="ts">
import {defineProps, defineEmits} from 'vue';
import get = Reflect.get;

const props = defineProps<{
  card: {
    type: UNOCard,
    required: true
  };
  isSelected: Boolean;
}>();

// for action cards, they will be displayed a little differently
const isActionCard = (props.card.value === 'Wild' || props.card.value === 'Skip' || props.card.value === 'Reverse' || props.card.value === 'Skip All') ? true : false;
const isDrawCard = (props.card.value === 'Draw Two' || props.card.value === 'Wild Draw Four') ? true : false;
const isNumberCard = (props.card.value === '0' || props.card.value === '1' || props.card.value === '2' || props.card.value === '3' || props.card.value === '4' || props.card.value === '5' || props.card.value === '6' || props.card.value === '7' || props.card.value === '8' || props.card.value === '9') ? true : false;

const emits = defineEmits();

const handleClick = () => {
  emits('cardClicked', props.card);
};

const getDisplayValue = (value: string) => {
  if (value === 'Wild Draw Four') {
    return '+4';
  } else if (value === 'Draw Two') {
    return '+2';
  } else {
    return value;
  }
};

const getIcon = (value: string) => {
  if (value === 'Wild Draw Four') {
    return new URL(`../../assets/icons/unoDeck/plus_four.svg`, import.meta.url);
  } else if (value === 'Wild') {
    return new URL(`../../assets/icons/unoDeck/wild_circle.svg`, import.meta.url);
  } else if (value === 'Skip All') {
    return new URL(`../../assets/icons/unoDeck/skip_all.svg`, import.meta.url);
  } else if (value === 'Draw Two') {
    return new URL(`../../assets/icons/unoDeck/plus_two.svg`, import.meta.url);
  } else {
    return new URL(`../../assets/icons/unoDeck/${value.toLowerCase()}.svg`, import.meta.url);
  }
};

// const cardColor = (color: string) => {
//   if (color === 'red') {
//     return 'rgba(255, 0, 0, 0.5)';
//   } else if (color === 'yellow') {
//     return 'rgba(255, 255, 0, 0.5)';
//   } else if (color === 'green') {
//     return 'rgba(0, 255, 0, 0.5)';
//   } else if (color === 'blue') {
//     return 'rgba(0, 0, 255, 0.5)';
//   } else {
//     return 'rgba(255, 255, 255, 0.5)';
//   }
// };

const cardColor = (color: string) => {
  return {backgroundColor: color};
};

</script>

<template>
  <div class="flex flex-wrap justify-center items-center">
    <div v-if="isNumberCard" class="flex flex-col justify-center items-center w-20 h-32 m-2 rounded-md shadow-md p-2"
         style="box-shadow: 6px -6px 3px rgba(200, 200, 200, 0.4);"
         :style="{ backgroundColor: card.color }"
         @click="handleClick"
         :class="{ 'selected': isSelected }">
      <div class="relative h-screen flex justify-center items-center">
        <div class="relative -left-6 -top-4 text-2xl jost-font">
          {{ getDisplayValue(card.value) }}
        </div>
      </div>
        <div class="bottom-3 text-5xl jost-font">
          {{ getDisplayValue(card.value) }}
        </div>
    </div>

    <div v-else-if="isActionCard" class="relative w-20 h-32 m-2 rounded-md shadow-md p-2"
         style="box-shadow: 6px -6px 3px rgba(200, 200, 200, 0.4);"
         :style="cardColor(card.color)"
         @click="handleClick"
         :class="{ 'selected': isSelected }">
      <img :src="getIcon(card.value)"
           alt="action card icon"
           class="absolute top-2 left-2"/>
      <img :src="getIcon(card.value)"
           alt="action card icon"
           class="absolute bottom-2 right-2 w-14 h-14"/>
    </div>

    <div v-else-if="isDrawCard" class="relative w-20 h-32 m-2 rounded-md shadow-md p-2"
         style="box-shadow: 6px -6px 3px rgba(200, 200, 200, 0.4);"
         :style="cardColor(card.color)"
         @click="handleClick"
         :class="{ 'selected': isSelected }">
      <div class="absolute top-2 left-2 text-3xl font-bold jost-font">
        {{ getDisplayValue(card.value) }}
      </div>
      <img :src="getIcon(card.value)"
           alt="draw card icon"
           class="absolute bottom-2 right-2 w-14 h-14"/>
    </div>
  </div>

</template>

<style scoped>
@import url('https://fonts.googleapis.com/css2?family=Jost:ital,wght@0,500;1,500&display=swap');

.jost-font {
  font-family: "Jost", sans-serif;
  font-optical-sizing: auto;
  font-weight: 500;
  font-style: normal;
}

.selected {
  border: 2px solid red;
}
</style>