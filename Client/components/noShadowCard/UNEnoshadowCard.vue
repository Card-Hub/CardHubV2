<script setup lang="ts">
import {defineProps} from 'vue';
// import get = Reflect.get;

export interface Props {
  card?: UNOCard;
}

const props = withDefaults(defineProps<Props>(), {
  card: () => ({
    Color: 'black',
    Value: '0',
    Id: 0
  } as UNOCard),
});

// for action cards, they will be displayed a little differently
const isActionCard = (props.card.Value === 'Wild' || props.card.Value === 'Skip' || props.card.Value === 'Reverse' || props.card.Value === 'Skip All');
const isDrawCard = (props.card.Value === 'Draw Two' || props.card.Value === 'Wild Draw Four');
const isNumberCard = (props.card.Value === '0' || props.card.Value === '1' || props.card.Value === '2' || props.card.Value === '3' || props.card.Value === '4' || props.card.Value === '5' || props.card.Value === '6' || props.card.Value === '7' || props.card.Value === '8' || props.card.Value === '9');

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
  if (value.toLowerCase() === 'Wild Draw Four'.toLowerCase()) {
    return new URL(`../../assets/icons/unoDeck/plus_four.svg`, import.meta.url);
  } else if (value.toLowerCase() === 'Wild'.toLowerCase()) {
    return new URL(`../../assets/icons/unoDeck/wild_circle.svg`, import.meta.url);
  } else if (value.toLowerCase() === 'Skip All'.toLowerCase()) {
    return new URL(`../../assets/icons/unoDeck/skip_all.svg`, import.meta.url);
  } else if (value.toLowerCase() === 'Draw Two'.toLowerCase()) {
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


//add this after the v-ifs
//          style="box-shadow: 6px -6px 3px rgba(200, 200, 200, 0.4);"
</script>

<template>
  <div class="flex flex-wrap justify-center items-center">
    <div v-if="isNumberCard" class="card flex flex-col justify-center items-center m-2 rounded-md shadow-lg border-white border-4 p-2"
         :style="cardColor(card.Color)">
      <div class="relative h-screen flex justify-center items-center">
        <div class="relative -top-3 right-12 text-6xl jost-font">
          {{ getDisplayValue(card.Value) }}
        </div>
      </div>
      <div class="relative bottom-5 text-9xl jost-font">
        {{ getDisplayValue(card.Value) }}
      </div>
    </div>

    <div v-else-if="isActionCard" class="card flex flex-col justify-center items-center m-2 rounded-md shadow-lg border-black border-4 p-2"
         :style="cardColor(card.Color)">
      <img :src="getIcon(card.Value)"
           alt="action card icon"
           id="small-icon"
           class="relative -top-4 -left-10"/>
      <img :src="getIcon(card.Value)"
           alt="action card icon"
           id="draw-icon"
           class="relative bottom-13"/>
    </div>

    <div v-else-if="isDrawCard" class="card flex flex-col justify-center items-center m-2 rounded-md shadow-lg border-black border-4 p-2"
         :style="cardColor(card.Color)">
      <div class="relative -top-6 -left-8 text-6xl font-bold jost-font">
        {{ getDisplayValue(card.Value) }}
      </div>
      <img :src="getIcon(card.Value)"
           alt="draw card icon"
           id="draw-icon"
           class="relative bottom-.5"/>
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

.card {
  border: 1px solid white;
  width: 20vw;
  height: 30vw;
  max-height: 300px;
  max-width: 200px;
  display: flex;
  justify-content: center;
  align-items: center;
}

#draw-icon {
  height: 55%;
  width: 55%;
}

#small-icon {
  height: 35%;
  width: 35%;
}
</style>