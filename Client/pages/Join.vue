<script setup lang="ts">
import toast from "@/utils/toast";
import { useNuxtApp } from "nuxt/app";
import { storeToRefs } from "pinia";
import { computed } from "vue";
import { useBaseStore } from "~/stores/baseStore";
import { GameType } from "~/types";

const { $api, $gameToString } = useNuxtApp();

const cahStore = useCahStore();
const { registerHandlersCah } = cahStore;

const store = useBaseStore();
const { isBaseConnected, messages } = storeToRefs(store);
const { tryConnectPlayer } = store;

const user = ref("");
const room = ref("");

const isValidRoomCode = computed(() => {
  const digitRegex = /^\d+$/;
  return room.value.length === 6 && digitRegex.test(room.value);
});

const connectPlayer = async (): Promise<void> => {
  const type = await $api<GameType>(`game/verifycode/${ room.value }`, { method: "GET" });
  if (type === null || type === undefined) {
    console.log("Invalid room code");
    return;
  }

  let callback: any;
  switch (type) {
    case GameType.Cah:
      callback = registerHandlersCah;
      break;
    default:
      console.log("doing the default")
      break;
  }
  const isConnected = await tryConnectPlayer(user.value, room.value, type, callback);
  if (isConnected) {
    await navigateTo(`/lobby/${ $gameToString(type) }`);
    return;
  }

  toast.add({
    severity: "error",
    summary: "That pin doesn't look right",
    detail: "Please check and try again",
    life: 5000
  });
};

const navigateToLibrary = async (): Promise<void> => {
  await navigateTo("/games");
};

const navigateToHome = async (): Promise<void> => {
  await navigateTo("/");
};

// const connectGameboard = async (): Promise<void> => {
//   const isRoomCreated = await tryCreateRoom();
//   if (isRoomCreated) {
//     // await navigateTo('/playerview');
//     await navigateTo("/lobby");
//   }
// };

console.log("check here for connectivity", isBaseConnected.value);
</script>

<template>
  <div id="dimScreen">
    <Toast/>
    <template v-if="isBaseConnected === false">
      <div class="flex flex-col justify-center h-screen items-center">
        <div class="flex flex-col justify-center gap-4">
          <svgo-logo-combination class="w-52 h-52" :fontControlled="false" filled @click="navigateToHome"/>
        </div>
        <div class="flex flex-col justify-center gap-4 mb-8">
          <InputText type="text" v-model="user" placeholder="Name"/>
          <InputText type="text" v-model="room" placeholder="Game Pin"/>
          <Button label="Primary" @click="connectPlayer" :disabled="!isValidRoomCode">Enter</Button>
        </div>
        <div class="flex flex-col w-auto">
          <Button label="Secondary" severity="secondary" @click="navigateToLibrary">Start a game</Button>
        </div>
      </div>
    </template>
    <template v-else>
      <h1>Connected to WS</h1>
      <p>
        TODO: Reroute to lobby
      </p>
    </template>
  </div>
</template>

<style scoped>

#dimScreen {
  width: 100%;
  height: 100%;
  background: linear-gradient(20deg, #000000 0%, #313134 100%);
  position: fixed;
  top: 0;
  left: 0;
  z-index: 100;
}

</style>
