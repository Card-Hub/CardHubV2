<script setup lang="ts">
import {defineComponent, ref, onMounted} from "vue";
//impor
import {storeToRefs} from "pinia";
import {useWebSocketStore} from "~/stores/webSocketStore";

import UNEnoshadowCard from "~/components/noShadowCard/UNEnoshadowCard.vue";
import UNOCardDisplay from "~/components/Card/UNOCardDisplay.vue";

const store = useWebSocketStore();
 const {user, users, room } = storeToRefs(store);
 const { playCard, selectColor } = store;
const uneStore = useUneStore();
 const { winner, currentPlayer, yourCards, players, discardPile, someoneNeedsToSelectColor } = storeToRefs(uneStore);
 interface Player {
         Name: string
         Avatar: string
         Afk: boolean
     };
     
     interface unePlayer extends Player {
         PickingWildColor: boolean
         Hand: UNOCard[]
     };

const myCards = computed(() => {
  let mc = <UNOCard[]>[];
  if (players.value != null) {
    players.value.forEach(player => {
      if (player.Name == user.value) {
        player.Hand.forEach(card => {
          // translate between json and unocard
          mc.push({id:card.Id, value:card.Value, color:card.Color});
        });
      }
    });
  }
  return mc;
});
// object of players with information about avatar, name, and cards

</script>


<template>
  <div class="display-block">
    <h1> Playing Une <span v-if="currentPlayer === user">- Your Turn!</span><span v-if="winner == user">- You Won!</span></h1>
    <Button class="mt-48" @click="draw">Draw</Button>
  </div>
  <!--<p v-for="card in myCards"> {{card}}</p>-->
  <div class="cardsContainer flex overflow-x-auto">
    <UNEnoshadowCard class="uneCard flex-none"
        v-for="card in myCards"
                      :key="card.Id"
                      :card="card"
                      :isSelected="true"
        @click="playCard(JSON.stringify(card))"
    />
  </div>
  <div v-if="currentPlayer === user && someoneNeedsToSelectColor">
    <Button class="mt-48" @click="selectColor('red')">Select Red</Button>
    <Button class="mt-48" @click="selectColor('green')">Select Green</Button>
    <Button class="mt-48" @click="selectColor('blue')">Select Blue</Button>
    <Button class="mt-48" @click="selectColor('yellow')">Select Yellow</Button>

  </div>
  

</template>

<style scoped>
.cards-container {
  /*display: flex;*/
  /*flex-direction: row-reverse;*/
}
</style>