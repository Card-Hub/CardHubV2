<script setup lang="ts">
import {defineComponent, ref, onMounted} from "vue";
//impor
import {storeToRefs} from "pinia";

import UNEnoshadowCard from "~/components/noShadowCard/UNEnoshadowCard.vue";
import StandardCardDisplay from "~/components/Card/StandardCardDisplay.vue";
import UNOCardDisplay from "~/components/Card/UNECardDisplay.vue";

import toast from "@/utils/toast";

import { type ConfigurableDocument, type MaybeElementRef, useFullscreen } from '@vueuse/core';

// fullscreen
const { isFullscreen, enter, exit } = useFullscreen();
const el = ref(null)
const { toggle } = useFullscreen(el)

const getPrimeIcon = (name: string) => {
  return new URL(`../../assets/icons/primeIcons/${name}.svg`, import.meta.url);
}

// https://primevue.org/dialog
import Dialog from 'primevue/dialog';
import Chat from "~/components/Chat.vue";
import UnoRules from "~/components/gameRules/UneRules.vue";
import PokerRules from "~/components/gameRules/PokerRules.vue"; // for popup dialog
const rulesVisible = ref(false); // for popup dialog
const chatVisible = ref(false); // for popup dialog https://primevue.org/avatar/ for chat notification

//const store = useWebSocketStore();
// const {user, users, room } = storeToRefs(store);
// const { playCard, selectColor, drawCard } = store;
//const uneStore = useUneStore();
// const { winner, currentPlayer, yourCards, players, discardPile, someoneNeedsToSelectColor } = storeToRefs(uneStore);
 interface Player {
         Name: string
         Avatar: string
         Afk: boolean
     };
     
     interface PokerPlayer extends Player {
         CanFold: boolean
         CanCall: boolean
         CanRaise: boolean
         CanCheck: boolean
         Hand: StandardCard[]
         BestHand: string
         Folded: boolean
     };
const bestHand = ref<string>("");
const cards2 = ref<StandardCard[]>([ {Id: 3, Suit: "Spades", Value: "4"}, {Id: 3, Suit: "Spades", Value: "4"}]);
const canCall = ref<boolean>(true);
const canFold = ref<boolean>(true);
const canRaise = ref<boolean>(true);
const canCheck = ref<boolean>(false);
const folded = ref<boolean>(false);
const currentBoardBet = ref<number>(8);
const currentPersonalBet = ref<number>(6);
const personalPot = ref<number>(80);
const totalPot = ref<number>(16);
const littleBlind = ref<string>("Me");
const bigBlind = ref<string>("Juno");
const buttonPlayer = ref<string>("Juno2");

// object of players with information about avatar, name, and cards
const winner = ref<string>("");
const user = ref<string>("Me");
const currentPlayer = ref<string>("Me");
const showingRaisePopup = ref<boolean>(false); // needs to be in store w everything else

const amtToRaiseBy = ref<number>(0);

const showRaisePopup = () => {
  showingRaisePopup.value = true;
}
const closeRaisePopup = () => {
  showingRaisePopup.value = false;
}

const fold = () => {
  toast.add({
      severity: "secondary",
      summary: "That pin doesn't look right",
      detail: "Please check and try again",
      life: 5000
    });
  console.log("toasting");
}

const getUserIcon = () => {
  // iterate through players to find the user's avatar
  // let userIcon = "";
  // players.value.forEach(p => { if (p.Name === user.value) { userIcon = p.Avatar; } });
  //
  // return new URL(`../../assets/icons/avatars/${userIcon}.png`, import.meta.url);
  return new URL(`../../assets/icons/avatars/lyssie.png`, import.meta.url);
};

const handleExit = async () => {
  // connection.value = null;
  // room.value = '';

  // redirect to join page
  await navigateTo("/join");
};
</script>


<template>
  <Toast/>
  <div class="playerview-une-container w-full p-6">
    
    <div class="flex flex-row justify-between">
      <div class="user-info flex place-items-center gap-3 bg-gray-600">
        <img :src="getUserIcon()" alt="user" class="user-avatar"/>
        <p class="text-white"> {{ user }}</p>
      </div>

      <div class="left-div flex flex-row-reverse place-items-center gap-2">
        <img :src="getPrimeIcon('expand')" class="size-10" @click="toggle" />

        <div class="card">
          <i class="pi pi-fw pi-info-circle" style="font-size: 2rem" @click="rulesVisible = true"></i>
          <Dialog v-model="rulesVisible" header="Rules" class="w-[900px] h-[900px]" :visible="rulesVisible" @update:visible="rulesVisible = $event">
            <PokerRules />
            <div class="flex justify-content-end gap-2">
              <!--            <Button type="button" label="Exit" @click="rulesVisible = false"></Button>-->
            </div>
          </Dialog>
        </div>

        <div class="">
          <i class="pi pi-fw pi-comment" style="font-size: 2rem" @click="chatVisible = true"></i>
          <Dialog v-model="chatVisible" class="w-[900px] h-[900px]" header="Chat" :visible="chatVisible" @update:visible="chatVisible = $event">
            <Chat/>
          </Dialog>
        </div>

        <!--      <i class="pi pi-fw pi-eye" @click="!sideScroll" style="font-size: 2.5rem"></i>-->
      </div>
    </div>
    
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
    <div class="cards-and-buttons">
      <div class="cards-outer">
        <p class="your-hand">Your hand:</p>
        <div class="cards">
          <StandardCardDisplay :card="cards2[0]" class="standardCardDisplay"/>
          <StandardCardDisplay :card="cards2[1]" class="standardCardDisplay"/>  
        </div>
      </div>

      <div class="buttons">
        <Button class="font-bold button shadow" @click="fold" :disabled="!canFold">Fold</Button>
        <Button class=" font-bold button shadow" @click="call" :disabled="!canCall">Call ({{ currentBoardBet }})</Button>
        <Button class=" font-bold button shadow" @click="showRaisePopup" :disabled="!canRaise">Raise</Button>
        <Button class=" font-bold button shadow" @click="check" :disabled="!canCheck">Check</Button>
      </div>
    </div>
    <div class="footer">
      <div class="best-hand-container">
        <p v-if="bestHand != '' && folded != true">Your best hand: {{ bestHand }}</p>
        <p v-if="folded">You have folded.</p>
        <p v-if="user == littleBlind">You are the little blind.</p>
        <p v-if="user == bigBlind">You are the little blind.</p>
        <p v-if="user == buttonPlayer">You are the dealer/button player.</p>
      </div>
      <div class="bets-container content-center">
        <ul class="">
          <li>
            Your current bet: {{currentPersonalBet}}
          </li>
          <li>
            Your remaining pot: {{personalPot}}
          </li>
          <li>
            Total pot: {{totalPot}}
          </li>
        </ul>
      </div>
    </div>
  <!--<div class=" w-full flex overflow-x-auto border-2 border-solid border-[#960E16] border-radius-4 justify-center">
    <UNOCardDisplay class="uneCard flex-none"
        v-for="card in myCards"
                      :key="card.Id"
                      :card="card"
                      :isSelected="false"
        @click="playCard(JSON.stringify(card))"
        />
      </div>-->
      <!--<div v-if="currentPlayer === user && false">
      </div>-->
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
  <div v-if="winner==='' && showingRaisePopup && currentPlayer === user" class="select-color">
    <Button class="x-button float-right" @click="closeRaisePopup"> <span class="pi pi-times fill-white" /></Button>
    <h2>Select amount to raise by</h2>
    <!--<div class="select-color-inner">-->
      
      <InputNumber v-model="amtToRaiseBy" inputId="horizontal-buttons" showButtons buttonLayout="horizontal" :step="1" class="">
        <template #incrementbuttonicon>
            <span class="pi pi-plus" />
        </template>
        <template #decrementbuttonicon>
            <span class="pi pi-minus" />
        </template>
      </InputNumber>
      <Button class="font-bold button raise-button shadow mt-2" @click="raise(amtToRaiseBy)">Raise</Button>
    <!--</div>-->
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

.cards-and-buttons {
  /*background-color: blue;*/
  display: grid;
  grid-template-columns: 1fr 1fr;
  height: 60%;
}
.cards {
  /*background-color: green;*/
  display: grid;
  grid-template-columns: 1fr 1fr;
}
.standardCardDisplay {
  /*background-color: pink;*/
  height: 100%;
}
.buttons {
  display: grid;
  grid-template-columns: 1fr 1fr;
  grid-template-rows: 1fr 1fr;
}

.button {
  background-color: var(--cardhub-red);
  width: 90%;
  height: 80%;
  justify-content: center;
  color: white;
  font-size: 2em;
}
.raise-button {
  height: auto;
}
.button:disabled {
  background-color: grey;
}

.footer {
  /*background-color: yellow;*/
  grid-template-columns: 2fr 3fr;
  display: grid;
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

.user-avatar {
  width: 3rem;
  height: 3rem;
  border-radius: 50%;
  background: rgba(255, 255, 255, 0.2);
}

.exit-btn {
  background-color: transparent;
  width: 15%;
  height: 5%;
  justify-content: center;
  color: white;
  font-size: 1.5em;
  border: 2px solid white;
  transform: translateY(-50%) translateX(-50%);
  position: absolute;
  top: 70%;
  left: 50%;
}

.user-info {
  color: white;
  padding-left: 2px;
  padding-right: 10px;
  border-radius: 25%/50%;
}

</style>