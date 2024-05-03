<script setup lang="ts">

import { storeToRefs } from "pinia";
import AvatarSelection from "~/components/AvatarSelection.vue";
import dialog from 'primevue/dialog';
import Chat from "~/components/Chat.vue";
import { GameType } from "~/types";
import {useBaseStore} from "~/stores/baseStore";
import {useCahStore} from "~/stores/cahStore";

const { $gameToString } = useNuxtApp();

import {ref, computed, onMounted} from "vue";
import CahDisplay from "~/components/Card/CahDisplay.vue";
import Dialog from "primevue/dialog";
const baseStore = useBaseStore();
const { isPlayer, messages, users, room, currentAvatar } = storeToRefs(baseStore);
const {  } = baseStore;

const cahStore = useCahStore();
const { gameStarted } = storeToRefs(cahStore);
const { ping } = cahStore;


import sillyFun from '../../assets/audio/music/sillyFun.mp3';
const visible = ref(false);
const settingsVisible = ref(false);
const seVolume = ref(0.5);

const bgMusic = new Audio(sillyFun);
const bgVolume = ref(0.5);

const updateBGVolume = () => {
  bgMusic.volume = bgVolume.value;
};

onMounted(() => {
  if(!isPlayer.value && room.value !== "" && !gameStarted.value){
    bgMusic.play();
  }  else {
    bgMusic.loop = false;
  }
});

if(!isPlayer.value && room.value !== ""){
  // bgMusic.loop = false;
  // bgMusic.play();
}

const bgVolumeDown = () => {
  bgVolume.value = 0;
  bgMusic.volume = bgVolume.value;
};

const bgVolumeUp = () => {
  bgVolume.value = 1;
  bgMusic.volume = bgVolume.value;
};

const seVolumeDown = () => {
  seVolume.value = 0;
};

const seVolumeUp = () => {
  seVolume.value = 1;
};

watch(gameStarted, (value) => {
  if (value) {
    if (isPlayer.value) {
      playerStart();
    } else {
      gameboardStart();
    }
  }
});

const gameboardStart = async () => {
  await navigateTo(`/gameboard/${ $gameToString(GameType.Cah) }`);
}

const playerStart = () => {
  return navigateTo(`/playerview/${ $gameToString(GameType.Cah) }`);
}

const getIcon = (avatar: string) => {
  if (avatar === "" || avatar == null) {
    console.log("No avatar found: ", avatar);
    return new URL(`../../assets/icons/avatars/lyssie.png`, import.meta.url);
  }
  else {
    return new URL(`../../assets/icons/avatars/${avatar}.png`, import.meta.url);
  }
};

const getIconGivenName = (name: string) => {
  users.value.forEach(function (user: BasePlayer) {
    // console.log(value);
    if (user.name === name) {
      return getIcon(user.avatar);
    } else {
      return getIcon("lyssie");
    }
  });
  // if that fails
  return getIcon("lyssie");
}

const kickPlayer = (user: BasePlayer) => {
  console.log("Kicking player: " + user.name);
  kickPlayer(user);
}

// turn each player into a cah player
const cahPlayers = computed(() => users.value.map((user) => {
  return {
    Name: user.name,
    Avatar: user.avatar,
    Score: 0,
    Hand: [],
    IsCzar: false,
    IsWinner: false
  }
}));

// convert cah player to base player
const convertBase = (player: CahPlayer) => {
  return {
    name: player.Name,
    avatar: player.Avatar
  }
}

</script>

<template>
  <div>
    <div v-if="isPlayer" class="m-8">

      <div class="flex justify-between">
        <h1>
          {{ $gameToString(GameType.Cah) }}
        </h1>
        <div class="justify-left">
          <i class="pi pi-fw pi-comment" style="font-size: 2rem" @click="visible = true"></i>
          <Dialog v-model="visible" class="chat-container" header="Chat" :visible="visible" @update:visible="visible = $event">
            <Chat/>
          </Dialog>

          <i class="pi pi-fw pi-cog" style="font-size: 2rem" @click="settingsVisible = true"></i>
          <Dialog v-model="settingsVisible" class="w-auto h-auto" header="Settings" :visible="settingsVisible" @update:visible="settingsVisible = $event">
            <h1 class="text-center">Sound Effects</h1> <br>
            <div class="flex flex-row justify-content-center align-middle">
              <i class="pi pi-fw pi-volume-down" style="font-size: 2rem" @click="seVolumeDown"></i>
              <div class="content-around">
                <Slider v-model="seVolume" :min="0" :max="1" :step="0.1" class="w-96"/>
              </div>
              <i class="pi pi-fw pi-volume-up" style="font-size: 2rem" @click="seVolumeUp"></i>
            </div><br>
          </Dialog>
        </div>
      </div>

      <p>
        Waiting for the host to start the game. Sit back and relax for now.
      </p>

      <AvatarSelection class="align-center"/>
      <div class="">
        <Button class="mt-5" @click="playerStart" v-if="true">Join Game</Button>
      </div>
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

          <div class="m-8 flex flex-col justify-between gap-4">
            <div v-for="user in cahPlayers" class="rounded-full flex card items-center justify-content h-16 w-full">
              <!--<i class="pi pi-user mx-4 text-neutral-300" style="font-size: 1.5rem"></i>-->
              <img :src="getIcon(user.Avatar)" alt="avatar Icon" class="lobby-player-icon-img">
              <span class="text-2xl text-neutral-300">{{ user.Name }} </span>
              <Button class="kick-btn" @click="kickPlayer(convertBase(user))"> Kick </Button>
            </div>
          </div>
        </div>
      </div>
      <div class="flex justify-center w-1/3">
        <div class="flex flex-col items-center">
          <h1 class="text-6xl">
            {{ $gameToString(GameType.Cah) }}
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
        <div class="absolute top-4 right-4">
          <i class="pi pi-fw pi-cog" style="font-size: 2rem" @click="settingsVisible = true"></i>
          <Dialog v-model="settingsVisible" class="w-auto h-auto" header="Settings" :visible="settingsVisible" @update:visible="settingsVisible = $event">
            <h1 class="text-center">Background Music</h1> <br>
            <div class="flex flex-row justify-content-center align-middle">
              <i class="pi pi-fw pi-volume-down" style="font-size: 2rem" @click="bgVolumeDown"></i>
              <div class="content-around">
                <Slider v-model="bgVolume" :min="0" :max="1" :step="0.1" class="w-96" @change="updateBGVolume"/>
              </div>
              <i class="pi pi-fw pi-volume-up" style="font-size: 2rem" @click="bgVolumeUp"></i>
            </div><br>
          </Dialog>
        </div>
        
        <div class="chat-box">
          <Chat />
        </div>

      </div>
    </div>
    <div v-else>
      Error: No user type found
    </div>
  </div>
  <button @click="ping">
    Ping
  </button>
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
  margin-left: .5rem;
  margin-right: 1rem;
}

.chat-box {
  height: 100%;
  align-items: center;
  padding-top: 30%;
  width: 90%;
}

.chat-container {
  width: 80%;
  height: 80%;
  max-height: 400px;
}

.kick-btn {
  background-color: transparent;
  color: white;
  font-size: 1em;
  border: 1px solid white;
  padding: 0.5em;
  margin-left: 1.5em;

}
</style>
