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



const displayText = (text: string) => {

  return text;
};

</script>

<template>
  <div id="card">
    <div id="text-container">
      <span>{{ card.text }}</span>
    </div>
  </div>
</template>

<style scoped>
#card {
  width: 100px;
  height: 150px;
  border-radius: 10px;
  box-shadow: 0 0 10px rgba(0, 0, 0, 0.5);
  cursor: pointer;
}

#text-container {
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center;
  align-items: center;
}
</style>