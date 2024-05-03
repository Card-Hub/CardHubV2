<script setup lang="ts">
import { defineComponent, ref, onMounted } from "vue";
import { storeToRefs } from "pinia";
import AvatarSelection from "~/components/AvatarSelection.vue";
import Chat from "~/components/Chat.vue";
import {useBaseStore} from "~/stores/baseStore";
import { useBlackJackStore } from "~/stores/blackJackStore";
import { GameType } from "~/types";
const { $gameToString } = useNuxtApp();



const baseStore = useBaseStore();
const store = useBlackJackStore();
const { isPlayer, messages, users, room, currentAvatar } = storeToRefs(baseStore);
const { gameStarted } = storeToRefs(store);
const { pingblackjack } = store;
// will allow for a popup of the chat
import dialog from 'primevue/dialog';
import {navigateTo} from "nuxt/app";
const visible = ref(false);

const gameboardStart = async () => {
  await navigateTo("/gameboard/" + $gameToString(GameType.BlackJack));
}

const playerStart = () => {
  return navigateTo("/playerview/" + $gameToString(GameType.BlackJack));
}

const getIcon = (avatar: string) => {
  if (avatar == "" || avatar == null) {
    return new URL(`../../assets/icons/avatars/lyssie.png`, import.meta.url);
  }
  else {
    return new URL(`../../assets/icons/avatars/${avatar}.png`, import.meta.url);
  }
};

const getIconGivenName = (name: string) => {
  users.value.forEach(function (user: BasePlayer) {
    // console.log(value);
    if (user.name == name) {
      return getIcon(user.avatar);
    }else{
      return getIcon("lyssie");
    }
  });
  // if that fails
  return getIcon("lyssie");
}

const blackJackPlayers = computed(() => users.value.map((user) => {
  return {
    Avatar: user.avatar,
    strConn: "",
    Hand: [],
    CurrentScore: 0,
    TotalMoney: 0,
    CurrentBet: 0,
    HasBet: false,
    NotPlaying: false,
    Busted: false,
    Winner: false,
    StillPlaying: true,
    Standing: false,
    Name: user.name,
  }
}));

const kickPlayer = (lobbyUser: LobbyUser) => {
  console.log("Kicking player: " + lobbyUser.Name);
  // send message to server to kick player
  // BOOT PLAYER HERE
  // kickPlayer(lobbyUser);
}

const convertBase = (player: BlackJackPlayer) => {
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
        <h1 >
          {{ $gameToString(GameType.BlackJack) }}
        </h1>
        <div class="justify-left">
          <i class="pi pi-fw pi-comment" style="font-size: 2rem" @click="visible = true"></i>
          <Dialog v-model="visible" class="w-[900px] h-[900px]" header="Chat" :visible="visible" @update:visible="visible = $event">
            <Chat/>
          </Dialog>
        </div>
      </div>

      <p>
        Waiting for the host to start the game. Sit back and relax for now.
      </p>

      <AvatarSelection class="align-center"/>
      <div v-if="gameStarted" >
        <NuxtLink to="/gameboard/blackjack" class="mt-5">
          <!--          <Button>Join Game</Button>-->
        </NuxtLink>
        <Button class="mt-5" @click="playerStart" v-if="gameStarted">Join Game</Button>
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

          <div class="m-8 flex flex-col gap-4">
            <div v-for="user in blackJackPlayers" class="rounded-full flex card items-center justify-content h-16 w-full justify-between">
              <!--<i class="pi pi-user mx-4 text-neutral-300" style="font-size: 1.5rem"></i>-->
              <div class="flex flex-row  items-center">
                <img :src="getIcon(user.Avatar)" alt="avatar Icon" class="lobby-player-icon-img">
                <span class="text-2xl text-neutral-300 ">{{ user.Name }} </span>
              </div>
              <Button class="kick-btn" @click="kickPlayer(user)"> Kick </Button>
            </div>
          </div>
        </div>
      </div>
      <div class="flex justify-center w-1/3">
        <div class="flex flex-col items-center">
          <h1 class="text-4xl">
            <!--{{ GameType.BlackJack }}-->
            BlackJack
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
        <div class="chat-box">
          <Chat />
        </div>

      </div>
    </div>
    <div v-else>
      Error: No user type found
    </div>
  </div>

  <div>
    <Button @click="pingblackjack">Ping</Button>
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
  margin-right: 1em;
}
</style>