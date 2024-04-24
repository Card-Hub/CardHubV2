<script setup lang="ts">

import { storeToRefs } from "pinia";
import Une from "~/pages/games/Une.vue";
import AvatarSelection from "~/components/AvatarSelection.vue";
import Chat from "~/components/Chat.vue";

const store = useWebSocketStore();
const { isPlayer, messages, users, room, lobbyUsers } = storeToRefs(store);
const { sendMessage, startGame } = store;
const uneStore = useUneStore();
const { gameType, gameStarted } = storeToRefs(uneStore);

const gameboardStart = () => {
  startGame();
  navigateTo("/gameboard/" + gameType.value.toLowerCase());
}

const playerStart = () => {
  return navigateTo("/playerview/" + gameType.value.toLowerCase());
}

const getIcon = (avatar: string) => {
  if (avatar == "" || avatar == null) {
    return new URL(`../assets/icons/avatars/lyssie.png`, import.meta.url);
  }
  else {
    return new URL(`../assets/icons/avatars/${avatar}.png`, import.meta.url);
  }
};

const getIconGivenName = (name: string) => {
  lobbyUsers.value.forEach(function (user: LobbyUser) {
    // console.log(value);
    if (user.Name == name) {
      return getIcon(user.Avatar);
    }
  });
  // if that fails
  return getIcon("");
}

</script>

<template>
  <div>
    <div v-if="isPlayer" class="m-8">
      <h1>
        {{ gameType }}
      </h1>
      <p>
        Waiting for the host to start the game. Sit back and relax for now.
      </p>
      
      <AvatarSelection class="align-center"/>
      <div class="">
        <Button class="mt-48" @click="playerStart" v-if="gameStarted">Join Game</Button>
      </div>
      <div class="m-10"></div>
      <Chat/>
<!--      <p>gfjagajkhgkj</p>-->
    </div>
    <div v-else-if="!isPlayer" class="flex min-h-screen">
      <div class="flex flex-col w-1/3 bg-neutral-950 shadow-inner">
        <div class="flex-none">
          <svgo-logo-wordmark class="w-20 h-20 ml-4 mt-2" :fontControlled="false" filled/>
        </div>
        <div class="flex-1 overflow-hidden relative">
          <SvgoStandardDeckDiamonds class="suit w-80 h-80 absolute z-0 -left-24 rotate-[-39deg]" :fontControlled="false" filled/>
          <SvgoStandardDeckClubs class="suit w-80 h-80 absolute z-0 top-20 -right-20 rotate-[240deg]" :fontControlled="false" filled/>
          <SvgoStandardDeckHearts class="suit w-80 h-80 absolute z-0 -bottom-40 -right-10" :fontControlled="false" filled/>
          <SvgoStandardDeckSpades class="suit w-80 h-80 absolute z-0 bottom-12 -left-24 rotate-[-20deg]" :fontControlled="false" filled/>
          
          <div class="m-8 flex flex-col gap-4">
            <div v-for="lobbyUser in lobbyUsers as LobbyUser[]" class="rounded-full flex card items-center justify-content h-16 w-full">
              <!--<i class="pi pi-user mx-4 text-neutral-300" style="font-size: 1.5rem"></i>-->
              <img :src="getIcon(lobbyUser.Avatar)" alt="avatar Icon" class="lobby-player-icon-img">
              <span class="text-2xl text-neutral-300">{{ lobbyUser.Name }} </span>
            </div>
          </div>
        </div>
      </div>
      <div class="flex justify-center w-1/3">
        <div class="flex flex-col items-center">
          <h1 class="text-6xl">
            Une
          </h1>
          <p class="mt-24 text-xl">
            Room Code
          </p>
          <p class="text-6xl">
            {{ room }}
          </p>
          <Button class="mt-48" @click="gameboardStart">Start Game</Button>
        </div>
      </div>
      <div class="flex flex-col w-1/3">
      </div>
    </div>
    <div v-else>
      Error: No user type found
    </div>
  </div>
</template>

<style scoped>

  .card {
    background: rgba( 255, 255, 255, 0.1 );
    box-shadow: 0 8px 32px 0 rgba( 74, 1, 29, 0.15);
    backdrop-filter: blur( 20px );
    -webkit-backdrop-filter: blur( 20px );
    border-radius: 28px;
    border: 1px solid rgba( 255, 255, 255, 0.10 );
  }

  .suit {
    opacity: 35%;
  }

  .lobby-player-icon-img {
    width: 3em;
    border-radius: 50%; /* Ensure the player icon is circular */
  overflow: hidden;
  }
</style>

