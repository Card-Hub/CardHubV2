<script setup lang="ts">

export interface Props {
  card?: CahCard;
  isSelected?: Boolean;
}

const props = defineProps<Props>();
const emits = defineEmits();

const enum CahType {
  White,
  Black
}

const getcardColor = (type: CahType) => {
  if (type === CahType.Black) {
    return "black";
  } else {
    return "white";
  }
};
const handleClick = () => {
  emits("cardClicked", props.card);
};


const displayText = (type:CahType) => {
  if (type === CahType.Black) {
    return "white";
  } else {
    return "black";
  }
};

const getCahLogo = () => {
  return new URL(`../../assets/icons/cah/cah_logo.svg`, import.meta.url);
};

</script>

<template>
  <div id="card" :style="{background: getcardColor(card.type), color: displayText(card.type)} ">
    <div id="text-container">
      <span>{{ card.text }}</span>
    </div>
    <!--    <img :src="getCahLogo()" alt="cah logo" class="absolute bottom-2 right-2 w-14 h-14" />-->
  </div>
</template>

<style scoped>
#card {
  width: 15vw;
  height: 25vw;
  max-height: 250px;
  max-width: 150px;
  border-radius: 10px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);
  cursor: pointer;
}

#text-container {
  width: 100%;
  height: 100%;
  display: flex;
  padding-top: 15%;
  padding-left: 10%;
  align-items: flex-start;
}
</style>