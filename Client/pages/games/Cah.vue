<script setup lang="ts">
import PlayerHand from "~/components/PlayerHand.vue";
import { ref, computed } from 'vue';
import UNOCardDisplay from "~/components/Card/UNOCardDisplay.vue";
import StandardCardDisplay from "~/components/Card/StandardCardDisplay.vue";

import { storeToRefs } from "pinia";
import { useWebSocketStore } from "~/stores/webSocketStore";
import CahRules from "~/components/gameRules/CahRules.vue";

const store = useWebSocketStore();
const { connection, isConnected, messages, user, room } = storeToRefs(store);
const { tryCreateRoom, tryJoinRoom, sendGameType } = store;

const connectGameboard = async (): Promise<void> => {
  const isRoomCreated = await tryCreateRoom();
  if (isRoomCreated) {
    sendGameType('CAH');
    // await navigateTo('/playerview');
    await navigateTo("/lobby");
  }
};

// create standard deck of cards
const standardDeck = [];
const suits = ["hearts", "diamonds", "clubs", "spades"];
const values = ["2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace"];

for (const suit of suits) {
  for (const value of values) {
    standardDeck.push({
      id: standardDeck.length + 1,
      suit,
      value
    });
  }
}

const playerHand = ref<StandardCard[]>(standardDeck);

const getCards = () => {
  return new URL(`../../assets/icons/cah/cah.svg`, import.meta.url);
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
  <div class="blackjack">
    <NuxtLink href="/games" class="go-back-btn">
      <Button class="go-back">Go Back</Button>
    </NuxtLink>

    <div class="column-container">
      <div class="column left-column">
        <div class="deck-view">
          <div class="game-logo flex justify-center items-center w-52 h-80 rounded-md shadow-md mb-2">
            <img :src="getCards()"
                 alt="game icon"
                 class="deck-logo"/>
          </div>
        </div>
      </div>

      <div class="column right-column">
        <h1 class="text-7xl">Cards Against Humanity</h1>
        <h3>Game Description: </h3>
        <p> The following game includes cards from the original Cards Against Humanity, so credit for them creating this popular game. "Cards Against Humanity" is a party game where players complete fill-in-the-blank statements using words or phrases on playing cards, typically with humor, satire, or absurdity. Each round, one player draws a prompt card and reads it aloud. Then, other players choose from their hand of answer cards to fill in the blanks, aiming to create the funniest or most outrageous combination. The player who initially drew the prompt card then selects their favorite answer, awarding that player a point. The game often involves dark humor, pop culture references, and unconventional combinations, making it a favorite for gatherings and parties. </p>
        
        <div>
          <p>Copyright 2024 Cards Against Humanity LLC. Used within license terms.</p>
        </div>
        <NuxtLink href="/lobby">
          <Button class="play" label="Secondary" severity="secondary" @click="connectGameboard"> Play CAH </Button>
        </NuxtLink>
      </div>

      <!--      <div class="column">-->
      <!--        <h1>UNO</h1>-->
      <!--      </div>-->
    </div>

    <div class="menu-container">
      <TabMenu v-model:activeIndex="active" :model="items" activeItem="None" class="tab-menu"/>
    </div>

    <div class="rules-container" v-if="active === 1">
      <CahRules />
    </div>
    
    <div v-if="active === 2" class="card-container">
        <StandardCardDisplay v-for="card in standardDeck"
                             :key="card.id"
                             :card="card"
        />
    </div>
    
    <div v-else-if="active === 0">
    </div>
  </div>
</template>

<style scoped>
.blackjack {
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

.game-logo{
  background-color: #0a0a0a;
}

.rules-container {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  width: 100%;
}

.menu-container{
  width: 90%;
  align-self: center;
  margin-top: 2.5%;
  margin-bottom: 2%;
}
</style>