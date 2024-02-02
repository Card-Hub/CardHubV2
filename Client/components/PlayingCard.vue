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

  // const getSuitIcon = async (suit: string) => {
  //   try{
  //     switch (suit) {
  //       case 'Hearts':
  //         return (await import('../assets/icons/heart.svg')).default;
  //       case 'Diamonds':
  //         return (await import(../assets/icons/diamonds.svg')).default;
  //       case 'Clubs':
  //         return (await import('../assets/icons/clubs.svg')).default;
  //       case 'Spades':
  //         return (await import('../assets/icons/spade.svg')).default;
  //       default:
  //         return '';
  //     }
  //   } catch (error) {
  //     console.error(`Error loading icon for suit ${suit}:`, error);
  //     return '';
  //   }
  //  
  // };

const getSuitIcon = async (suit: string) => {
  try {
    const iconModule = await import(`@/assets/icons/${suit.toLowerCase()}.svg`);
    return iconModule.default;
  } catch (error) {
    console.error(`Error loading icon for suit ${suit}:`, error);
    return '';
  }
};
</script>

<template>
  <div class="flex flex-wrap justify-center items-center">
    <div
        v-for="card in playingCards"
        class="relative w-20 h-32 m-2 bg-white rounded-md shadow-md p-2"
    >
      <img
          :src="getSuitIcon(card.suit)"
          alt="suit icon"
          class="absolute bottom-2 right-2 w-10 h-10"
      />
      <div class="absolute top-2 left-2 text-Slg font-bold text-black">{{ getDisplayValue(card.value) }}</div>
<!--      <div class="absolute bottom-2 right-2 text-lg font-bold text-black">{{ card.value }}</div>-->
    </div>
  </div>
</template>

<style scoped>

</style>