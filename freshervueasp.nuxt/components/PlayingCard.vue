<script setup lang="ts">
  // props are passed from the parent component
  
  type Card = {
    id: number
    value: string
    suit: string
  }
  
  const props = defineProps({
    playingCards: {
      type: Array as PropType<Array<Card>>,
      required: true
    }
  })
  console.log(props.playingCards)
  
  const getSuitIcon = (suit) => {
    switch (suit) {
      case 'Hearts':
        return '@/assets/icons/heart.svg'
      case 'Diamonds':
        return '@/assets/icons/diamond.svg'
      case 'Clubs':
        return '@/assets/icons/club.svg'
      case 'Spades':
        return '@/assets/icons/spade.svg'
      default:
        return ''
    }
  }
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
      <div class="absolute top-2 left-2 text-Slg font-bold text-black">{{ card.value }}</div>
<!--      <div class="absolute bottom-2 right-2 text-lg font-bold text-black">{{ card.value }}</div>-->
    </div>
  </div>
</template>

<style scoped>

</style>