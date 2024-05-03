<script setup lang="ts">
import { ref, computed } from 'vue';
import TabMenu from 'primevue/tabmenu';
import StandardCardDisplay from "~/components/Card/StandardCardDisplay.vue";

import { storeToRefs } from "pinia";
// import { useWebSocketStore } from "~/stores/webSocketStore";
import {navigateTo} from "nuxt/app";
import PokerRules from "~/components/gameRules/PokerRules.vue";

// const store = useWebSocketStore();
const { connection, isConnected, messages, user, room } = storeToRefs(store);
const { tryCreateRoom, tryJoinRoom, sendGameType } = store;

const connectGameboard = async (): Promise<void> => {
  const isRoomCreated = await tryCreateRoom();
  if (isRoomCreated) {
    sendGameType('Poker');
    // await navigateTo('/playerview');
    await navigateTo("/lobby/poker");
  }
};

// create standard deck of cards
const standardDeck = [];
const suits = ["hearts", "diamonds", "clubs", "spades"];
const values = ["2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace"];

for (const suit of suits) {
  for (const value of values) {
    standardDeck.push({
      Id: standardDeck.length + 1,
      Suit: suit,
      Value: value
    });
  }
}

const playerHand = ref<StandardCard[]>(standardDeck);

const getCards = () => {
  return new URL(`../../assets/icons/standardDeck/hearts.svg`, import.meta.url);
};

const active = ref(0); // 0 = none, 1 = rules, 2 = cards
//for the tab menu
const items = ref([
  { label: "None", icon: "pi pi-fw pi-eye-slash" },
  { label: "Rules", icon: "pi pi-fw pi-info-circle" },
  { label: "Cards", icon: "pi pi-fw pi-mobile" }
]);
</script>

<template>
  <div class="poker">
    <NuxtLink href="/games" class="go-back-btn">
      <Button class="go-back">Go Back</Button>
    </NuxtLink>

    <div class="column-container">
      <div class="column left-column">
        <div class="deck-view">
          <div class="flex justify-center items-center w-52 h-80 bg-white rounded-md shadow-md mb-2">
            <img :src="getCards()"
                 alt="game icon"
                 class="deck-logo"/>
          </div>
        </div>
      </div>

      <div class="column right-column">
        <h1 class="text-7xl">Poker</h1>
        <h3>Game Description: </h3>
        <p> Poker is a widely popular card game that combines elements of skill, strategy, and chance. Players compete to win chips or money by betting on the strength of their hands, which are composed of a combination of private ("hole") cards and community cards. Variants like Texas Hold'em, the most famous form, involve players making decisions based on incomplete information, assessing the risk versus reward of their actions, and leveraging psychological tactics against opponents. The game unfolds through rounds of betting, with players aiming to either have the best hand at showdown or to bluff and persuade others to fold. Whether in casual home games or high-stakes tournaments, poker challenges players to analyze situations, manage resources, and outmaneuver opponents in pursuit of victory. .
        </p>
        <NuxtLink href="/lobby">
          <Button class="play" label="Secondary" severity="secondary" @click="connectGameboard"> Play Poker </Button>
        </NuxtLink>
      </div>

    </div>

    <div class="menu-container">
      <TabMenu v-model:activeIndex="active" :model="items" activeItem="None" class="tab-menu"/>
    </div>

    <div class="rules-container" v-if="active === 1">
      <div class="rules-man">
        <PokerRules />
      </div>
    </div>
    
    <div v-if="active === 2" class="card-container">
        <StandardCardDisplay v-for="card in standardDeck"
                             :key="card.Id"
                             :card="card"
        />
    </div>
    
    <div v-else-if="active === 0">
    </div>
  </div>
</template>

<style scoped>
.poker {
  display: flex;
  flex-direction: column;
  justify-content: center;
  height: 100%;
  width: 100%;

}

.deck-view {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  padding-top: 50px;
  height: 100%;
  width: 100%;
}

.deck-logo {
  width: 75%;
  align-items: center;
}

.show-cards {
  margin-top: 20px;
  margin-bottom: 20px;
  align-self: center;
  color: white;
  background-color: transparent;
  border: 2px solid white;
  width: 130px;
  text-align: center;
}

.column-container {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  align-items: center;
  height: 100%;
  width: 100%;
}

.left-column {
  flex: 0.5;
  align-items: center;
}

.right-column {
  flex: 1.5;
}

.column {
  margin: 0 10px;
}

.go-back-btn {
  padding-left: 20px;
  padding-top: 20px;
  padding-bottom: 10px;
}

.go-back {
  background-color: transparent;
  color: white;
  border: 2px solid white;
}

.play {
  background: linear-gradient(20deg, #6e0000 0%, #ff0000 75%);
  color: white;
  border: 2px solid #151515;
  align-self: center;
}

.card-container {
  display: flex;
  flex-direction: row;
  justify-content: center;
  align-self: center;
  flex-wrap: wrap;
  align-items: center;
  height: 100%;
  width: 85%;
}

.rules-container {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  width: 100%;
}

.rules-man {
  width: 90%;
  align-self: center;
}

.menu-container{
  width: 90%;
  align-self: center;
  margin-top: 2.5%;
  margin-bottom: 2%;
}
</style>