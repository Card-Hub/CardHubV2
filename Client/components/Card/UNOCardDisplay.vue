<script setup lang="ts">
import {defineProps, defineEmits} from 'vue';
// import get = Reflect.get;

const props = defineProps<{
  card: UNOCard;
  isSelected?: Boolean;
}>();

// for action cards, they will be displayed a little differently
const isActionCard = (props.card.Value === 'Wild' || props.card.Value === 'Skip' || props.card.Value === 'Reverse' || props.card.Value === 'Skip All');
const isDrawCard = (props.card.Value === 'Draw Two' || props.card.Value === 'Wild Draw Four');
const isNumberCard = (props.card.Value === '0' || props.card.Value === '1' || props.card.Value === '2' || props.card.Value === '3' || props.card.Value === '4' || props.card.Value === '5' || props.card.Value === '6' || props.card.Value === '7' || props.card.Value === '8' || props.card.Value === '9');

const emits = defineEmits();

const handleClick = () => {
  emits('cardClicked', props.card);
  send(props.card);
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

const cardColor = (color: string) => {
  if (color.toLowerCase() === 'red') {
    return {backgroundColor: '#d12c15'};
  } else if (color.toLowerCase() === 'yellow') {
    return {backgroundColor: '#ffce30'};
  } else if (color.toLowerCase() === 'green') {
    return {backgroundColor: '#7abb18'};
  } else if (color.toLowerCase() === 'blue') {
    return {backgroundColor: '#1166ac'};
  } else if (color.toLowerCase() === 'black') {
    return {backgroundColor: '#151515'};
  }else{
    return {backgroundColor: color};
  }
};

</script>

<template>
  <div class="flex flex-wrap justify-center items-center">
    <div v-if="isNumberCard" class="card flex flex-col justify-center items-center w-20 h-32 m-2 rounded-md shadow-md p-2"
         style="box-shadow: 6px -6px 3px rgba(200, 200, 200, 0.4);"
         :style="cardColor(card.Color)"
         @click="handleClick"
         :class="{ 'selected': isSelected }">
      <div class="relative h-screen flex justify-center items-center">
        <div class="relative -left-6 -top-4 text-2xl jost-font">
          {{ getDisplayValue(card.Value) }}
        </div>
      </div>
        <div class="bottom-3 text-5xl jost-font">
          {{ getDisplayValue(card.Value) }}
        </div>
    </div>

    <div v-else-if="isActionCard" class="card flex flex-col justify-center items-center w-20 h-32 m-2 rounded-md shadow-md p-2"
         style="box-shadow: 6px -6px 3px rgba(200, 200, 200, 0.4);"
         :style="cardColor(card.Color)"
         @click="handleClick"
         :class="{ 'selected': isSelected }">
      <img :src="getIcon(card.Value)"
           alt="action card icon"
           class="relative -top-2.5 -left-4"/>
      <img :src="getIcon(card.Value)"
           alt="action card icon"
           class="relative -bottom-1 right-/5 w-14 h-14"/>
    </div>

    <div v-else-if="isDrawCard" class="card flex flex-col justify-center items-center w-20 h-32 m-2 rounded-md shadow-md p-2"
         style="box-shadow: 6px -6px 3px rgba(200, 200, 200, 0.4);"
         :style="cardColor(card.Color)"
         @click="handleClick"
         :class="{ 'selected': isSelected }">
      <div class="relative -top-4 -left-5 text-2xl font-bold jost-font">
        {{ getDisplayValue(card.Value) }}
      </div>
      <img :src="getIcon(card.Value)"
           alt="draw card icon"
           class="relative bottom-1 left-.5 w-14 h-14"/>
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