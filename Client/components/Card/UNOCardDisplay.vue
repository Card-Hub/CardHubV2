<script setup lang="ts">
import { defineProps, defineEmits } from "vue";
import { UnoColor, UnoValue } from "~/types";

export interface Props {
  card?: UNOCard;
  isSelected?: Boolean;
}

const props = withDefaults(defineProps<Props>(), {
  card: () => ({
    color: UnoColor.Black,
    value: UnoValue.Zero,
    id: 0
  } as UNOCard),
  isSelected: () => false
});

const actionValues = [UnoValue.Wild, UnoValue.Skip, UnoValue.Reverse, UnoValue.SkipAll];
const drawValues = [UnoValue.DrawTwo, UnoValue.WildDrawFour];
const numberValues = [UnoValue.Zero, UnoValue.One, UnoValue.Two, UnoValue.Three, UnoValue.Four, UnoValue.Five,
  UnoValue.Six, UnoValue.Seven, UnoValue.Eight, UnoValue.Nine];

const isActionCard = actionValues.includes(props.card.value);
const isDrawCard = drawValues.includes(props.card.value);
const isNumberCard = numberValues.includes(props.card.value);

const emits = defineEmits();
const handleClick = () => {
  emits("cardClicked", props.card);
};

const getDisplayValue = (value: UnoValue) => {
  switch (value) {
    case UnoValue.WildDrawFour:
      return "+4";
    case UnoValue.DrawTwo:
      return "+2";
    default:
      return value;
  }
};

const getIcon = (value: UnoValue) => {
  switch (value) {
    case UnoValue.DrawTwo:
      return new URL(`../../assets/icons/unoDeck/plus_two.svg`, import.meta.url);
    case UnoValue.Reverse:
      return new URL(`../../assets/icons/unoDeck/reverse.svg`, import.meta.url);
    case UnoValue.Skip:
      return new URL(`../../assets/icons/unoDeck/skip.svg`, import.meta.url);
    case UnoValue.SkipAll:
      return new URL(`../../assets/icons/unoDeck/skip_all.svg`, import.meta.url);
    case UnoValue.Wild:
      return new URL(`../../assets/icons/unoDeck/wild_circle.svg`, import.meta.url);
    case UnoValue.WildDrawFour:
      return new URL(`../../assets/icons/unoDeck/plus_four.svg`, import.meta.url);
    default:
      return new URL(`../../assets/icons/unoDeck/UNE.svg`, import.meta.url);
  }
};

const cardColor = (color: UnoColor) => {
  switch (color) {
    case UnoColor.Red:
      return { backgroundColor: "#d12c15" };
    case UnoColor.Yellow:
      return { backgroundColor: "#ffce30" };
    case UnoColor.Green:
      return { backgroundColor: "#7abb18" };
    case UnoColor.Blue:
      return { backgroundColor: "#1166ac" };
    case UnoColor.Black:
      return { backgroundColor: "#151515" };
    default:
      return { backgroundColor: color };
  }
};

//add this after the v-ifs
//          style="box-shadow: 6px -6px 3px rgba(200, 200, 200, 0.4);"
</script>

<template>
  <div class="flex flex-wrap justify-center items-center">
    <div v-if="isNumberCard"
         class="card flex flex-col justify-center items-center w-20 h-32 m-2 rounded-md shadow-md border-white border-4 p-2"
         :style="cardColor(card.color)"
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

    <div v-else-if="isActionCard"
         class="card flex flex-col justify-center items-center w-20 h-32 m-2 rounded-md shadow-md border-black border-4 p-2"
         :style="cardColor(card.color)"
         @click="handleClick"
         :class="{ 'selected': isSelected }">
      <img :src="getIcon(card.value)"
           alt="action card icon"
           class="relative top-2 left-2"/>
      <img :src="getIcon(card.value)"
           alt="action card icon"
           class="relative bottom-2 right-2 w-14 h-14"/>
    </div>

    <div v-else-if="isDrawCard"
         class="card flex flex-col justify-center items-center w-20 h-32 m-2 rounded-md shadow-md border-black border-4 p-2"
         :style="cardColor(card.color)"
         @click="handleClick"
         :class="{ 'selected': isSelected }">
      <div class="relative top-2 left-2 text-3xl font-bold jost-font">
        {{ getDisplayValue(card.value) }}
      </div>
      <img :src="getIcon(card.value)"
           alt="draw card icon"
           class="relative bottom-2 right-2 w-14 h-14"/>
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

.card {
  border: 1px solid white;
}
</style>