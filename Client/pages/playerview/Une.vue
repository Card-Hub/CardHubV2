<!-- <script setup lang="ts">
import {defineComponent, ref, onMounted} from "vue";
//impor
import {storeToRefs} from "pinia";
import {useWebSocketStore} from "~/stores/webSocketStore";

import UNEnoshadowCard from "~/components/noShadowCard/UNEnoshadowCard.vue";
import UNOCardDisplay from "~/components/Card/UNOCardDisplay.vue";

const store = useWebSocketStore();
 const {user, users, room } = storeToRefs(store);
 const { playCard, selectColor, drawCard } = store;
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

</script> -->


<!-- <template>
  <div class="playerview-une-container w-full p-6">
    <div class="w-full margin-auto">
    <h1 class="text-center">
      <span v-if="currentPlayer === user && winner != user">
          Your Turn!
      </span>
      <span v-if="winner == user">
          You Won!
      </span>
      <span v-if="currentPlayer !== user && winner === ''">{{ currentPlayer }} is playing...
      </span>
      <span v-if="currentPlayer !== user && winner !== ''">
        {{ winner }} won!
      </span>
    </h1>
    </div>
    <Button class="float-right font-bold drawButton shadow mb-4" @click="drawCard">Draw</Button>

  <div class=" w-full flex overflow-x-auto border-2 border-solid border-[#960E16] border-radius-4 justify-center">
    <UNOCardDisplay class="uneCard flex-none"
        v-for="card in myCards"
                      :key="card.Id"
                      :card="card"
                      :isSelected="false"
        @click="playCard(JSON.stringify(card))"
        />
      </div>
      <div v-if="currentPlayer === user && someoneNeedsToSelectColor">
        
    
  </div>

  <div v-if="winner!=''" class="winner">
    <div class="winner-inner">
      <span v-if="winner == user">
          You Won!
      </span>
      <span v-if="currentPlayer !== user && winner !== ''">
        {{ winner }} won!
      </span>
    </div>
  </div>
  <div v-if="winner==='' && someoneNeedsToSelectColor && currentPlayer === user && someoneNeedsToSelectColor" class="select-color">
    <h2>Select a Color</h2>
    <div class="select-color-inner">
      <Button class="red-button color-button" @click="selectColor('red')">Red</Button>
          <Button class="green-button color-button" @click="selectColor('green')">Green</Button>
          <Button class="blue-button color-button" @click="selectColor('blue')">Blue</Button>
      <Button class="yellow-button color-button" @click="selectColor('yellow')">Yellow</Button>
    </div>
  </div>
</div>
  
  
</template> -->

<!-- <style scoped>
.cards-container {
  border-color: red;
  /*display: flex;*/
  /*flex-direction: row-reverse;*/
}
.playerview-une-container {
  width: 100%;
  height: 100vh;
  /*display: flex;*/
  justify-content: center;
  /*align-items: center;*/
  background: rgb(243, 19, 19);
  background: radial-gradient(ellipse at center, rgba(243, 19, 19, 0.5) 0%, rgba(152, 14, 17, 0.7) 40%, rgba(63, 8, 14, 0.9) 95%);
}

.drawButton {
  background-color: var(--cardhub-red);
  width: 15%;
  height: 8%;
  justify-content: center;
  color: white;
  font-size: 2em;
}
.shadow {
  box-shadow: 4px -4px 6px rgba(256, 256, 256, 0.15);
}
.winner {
  position: absolute;
  top: 50%;
  /* scooches it halfway its own width down and left, centering it*/
transform: translateY(-50%) translateX(-50%);
  left: 50%;
  background-color: #745609;
  width: 80%;
  height: 20%;
  display: flex;
  align-items: center;
  justify-content: center;
  text-align: center;
  /*justify-content: center;*/
  border-radius:4px;
  border-style: solid;
  border-width: 2px;
  padding: 10px;

}
.winner-inner {
  border-style: solid;
  border-width: 4px;
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center; /* center*/
  /*text-align: center;*/
  align-items: center; /*height*/
}
.select-color {
  position: absolute;
  top: 50%;
  /* scooches it halfway its own width down and left, centering it*/
transform: translateY(-50%) translateX(-50%);
  left: 50%;
  background-color: rgb(63, 8, 14);
  width: 40%;
  height: 30%;
  /*display: flex;*/
  /*flex-direction: column;*/
  /*align-items: center;*/
  /*justify-content: center;*/
  text-align: center;
  justify-content: center;
  border-radius:4px;
  border-style: solid;
  border-width: 2px;
  padding: 12px;
  border-color: var(--cardhub-red);
}
.select-color h2 {
  margin-top: 0;
}
.select-color-inner {
  display:grid;
  height: 80%;
  grid-template-columns: 48% 48%;
  grid-template-rows: auto auto;
  column-gap: 4%;
  row-gap: 6%;
  /*row-gap: 4%;*/
  /*grid-row: 48% 48%;*/
}

.red-button {
  background-color: var(--une-red);
}
.blue-button {
  background-color: var(--une-blue);
}
.green-button {
  background-color: var(--une-green);
}
.yellow-button {
  background-color: var(--une-yellow);
}
.color-button {
  font-weight: bold;
  font-size: 1.5em;
  text-align: center;
  justify-content: center;
}

</style> -->


<!-- !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!! -->



<script setup lang="ts">
import {defineComponent, ref, onMounted} from "vue";
//impor
import {storeToRefs} from "pinia";
import {useWebSocketStore} from "~/stores/webSocketStore";

import UNEnoshadowCard from "~/components/noShadowCard/UNEnoshadowCard.vue";
import UNOCardDisplay from "~/components/Card/UNOCardDisplay.vue";

const store = useWebSocketStore();
 const {user, users, room } = storeToRefs(store);
 const { playCard, selectColor, drawCard } = store;
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
  <div class="playerview-une-container w-full p-6">
    <div class="w-full margin-auto">
    <h1 class="text-center">
      <span v-if="currentPlayer === user && winner != user">
          Your Turn!
      </span>
      <span v-if="winner == user">
          You Won!
      </span>
      <span v-if="currentPlayer !== user && winner === ''">{{ currentPlayer }} is playing...
      </span>
      <span v-if="currentPlayer !== user && winner !== ''">
        {{ winner }} won!
      </span>
    </h1>
    </div>
    <Button class="float-right font-bold drawButton shadow mb-4" @click="drawCard">Draw</Button>
  <!--<p v-for="card in myCards"> {{card}}</p>-->
  <!--<div></div>-->
  <div class=" w-full flex overflow-x-auto border-2 border-solid border-[#960E16] border-radius-4 justify-center">
    <UNOCardDisplay class="uneCard flex-none"
        v-for="card in myCards"
                      :key="card.Id"
                      :card="card"
                      :isSelected="false"
        @click="playCard(JSON.stringify(card))"
        />
      </div>
      <div v-if="currentPlayer === user && someoneNeedsToSelectColor">
        
    
  </div>
  <!--  winner -->
  <div v-if="winner!=''" class="winner">
    <div class="winner-inner">
      <span v-if="winner == user">
          You Won!
      </span>
      <span v-if="currentPlayer !== user && winner !== ''">
        {{ winner }} won!
      </span>
    </div>
  </div>
  <div v-if="winner==='' && someoneNeedsToSelectColor && currentPlayer === user && someoneNeedsToSelectColor" class="select-color">
    <h2>Select a Color</h2>
    <div class="select-color-inner">
      <Button class="red-button color-button" @click="selectColor('red')">Red</Button>
          <Button class="green-button color-button" @click="selectColor('green')">Green</Button>
          <Button class="blue-button color-button" @click="selectColor('blue')">Blue</Button>
      <Button class="yellow-button color-button" @click="selectColor('yellow')">Yellow</Button>
    </div>
  </div>
</div>
  
  
</template>

<style scoped>
.cards-container {
  border-color: red;
  /*display: flex;*/
  /*flex-direction: row-reverse;*/
}
.playerview-une-container {
  width: 100%;
  height: 100vh;
  /*display: flex;*/
  justify-content: center;
  /*align-items: center;*/
  background: rgb(243, 19, 19);
  background: radial-gradient(ellipse at center, rgba(243, 19, 19, 0.5) 0%, rgba(152, 14, 17, 0.7) 40%, rgba(63, 8, 14, 0.9) 95%);
}

.drawButton {
  background-color: var(--cardhub-red);
  width: 15%;
  height: 8%;
  justify-content: center;
  color: white;
  font-size: 2em;
}
.shadow {
  box-shadow: 4px -4px 6px rgba(256, 256, 256, 0.15);
}
.winner {
  position: absolute;
  top: 50%;
  /* scooches it halfway its own width down and left, centering it*/
transform: translateY(-50%) translateX(-50%);
  left: 50%;
  background-color: #745609;
  width: 80%;
  height: 20%;
  display: flex;
  align-items: center;
  justify-content: center;
  text-align: center;
  /*justify-content: center;*/
  border-radius:4px;
  border-style: solid;
  border-width: 2px;
  padding: 10px;

}
.winner-inner {
  border-style: solid;
  border-width: 4px;
  width: 100%;
  height: 100%;
  display: flex;
  justify-content: center; /* center*/
  /*text-align: center;*/
  align-items: center; /*height*/
}
.select-color {
  position: absolute;
  top: 50%;
  /* scooches it halfway its own width down and left, centering it*/
transform: translateY(-50%) translateX(-50%);
  left: 50%;
  background-color: rgb(63, 8, 14);
  width: 40%;
  height: 30%;
  /*display: flex;*/
  /*flex-direction: column;*/
  /*align-items: center;*/
  /*justify-content: center;*/
  text-align: center;
  justify-content: center;
  border-radius:4px;
  border-style: solid;
  border-width: 2px;
  padding: 12px;
  border-color: var(--cardhub-red);
}
.select-color h2 {
  margin-top: 0;
}
.select-color-inner {
  display:grid;
  height: 80%;
  grid-template-columns: 48% 48%;
  grid-template-rows: auto auto;
  column-gap: 4%;
  row-gap: 6%;
  /*row-gap: 4%;*/
  /*grid-row: 48% 48%;*/
}

.red-button {
  background-color: var(--une-red);
}
.blue-button {
  background-color: var(--une-blue);
}
.green-button {
  background-color: var(--une-green);
}
.yellow-button {
  background-color: var(--une-yellow);
}
.color-button {
  font-weight: bold;
  font-size: 1.5em;
  text-align: center;
  justify-content: center;
}

</style>