<script setup lang="ts">
import {defineComponent, ref, onMounted} from "vue";

import StandardCardDisplay from "~/components/Card/StandardCardDisplay.vue";

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
import CahRules from "~/components/gameRules/CahRules.vue";
const rulesVisible = ref(false); // for popup dialog
const chatVisible = ref(false); // for popup dialog https://primevue.org/avatar/ for chat notification

// convert user to CahPlayer
import {storeToRefs} from "pinia";
import { useBaseStore} from "~/stores/baseStore";
import { useCahStore } from "~/stores/cahStore";

const baseStore = useBaseStore();
const { isPlayer, messages, users, room, user, currentAvatar } = storeToRefs(baseStore);


const getUserIcon = () => {
  // iterate through players to find the user's avatar
  let userIcon = "";
  users.value.forEach(p => { if (p.name === user.value) { userIcon = p.avatar; } });

  return new URL(`../../assets/icons/avatars/${currentAvatar.value}.png`, import.meta.url);
  // return new URL(`../../assets/icons/avatars/lyssie.png`, import.meta.url);
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
            <CahRules />
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

<!--    <div class="w-full margin-auto">-->
<!--      <h1 class="text-center">-->
<!--      <span v-if="currentPlayer === user && winner != user">-->
<!--          Your Turn!-->
<!--      </span>-->
<!--        <span v-if="winner == user">-->
<!--          You Won!-->
<!--      </span>-->
<!--        <span v-if="currentPlayer !== user && winner === ''">{{ currentPlayer }} is playing...-->
<!--      </span>-->
<!--        <span v-if="currentPlayer !== user && winner !== ''">-->
<!--        {{ winner }} won!-->
<!--      </span>-->
<!--      </h1>-->
<!--    </div>-->
    
    
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
<!--    <div v-if="winner!=''" class="winner">-->
<!--      <div class="winner-inner">-->
<!--      <span v-if="winner == user">-->
<!--          You Won!-->
<!--      </span>-->
<!--        <span v-if="currentPlayer !== user && winner !== ''">-->
<!--        {{ winner }} won!-->
<!--      </span>-->
<!--      </div>-->
<!--    </div>-->
    
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