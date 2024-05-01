<script setup lang="ts">
import PlayerHand from "~/components/PlayerHand.vue";
import { ref, computed } from 'vue';
import UNOCardDisplay from "~/components/Card/UNOCardDisplay.vue";
import StandardCardDisplay from "~/components/Card/StandardCardDisplay.vue";

import { storeToRefs } from "pinia";
import { useWebSocketStore } from "~/stores/webSocketStore";
import UnoRules from "~/components/gameRules/UnoRules.vue";
import TexasholdemRules from "~/components/gameRules/TexasholdemRules.vue";

const store = useWebSocketStore();
const { connection, isConnected, messages, user, room } = storeToRefs(store);
const { tryCreateRoom, tryJoinRoom, sendGameType } = store;

const connectGameboard = async (): Promise<void> => {
  const isRoomCreated = await tryCreateRoom();
  if (isRoomCreated) {
    sendGameType('Texas Hold \'Em');
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
  <div class="blackjack">
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
        <h1 class="text-7xl">Texas Hold 'Em</h1>
        <h3>Game Description: </h3>
        <p> Texas Hold'em is a popular variation of poker played in both casual settings and professional tournaments worldwide. In Texas Hold'em, each player is dealt two private cards (known as "hole cards") that belong to them alone, and five community cards are dealt face-up on the "board." Players use a combination of their hole cards and the community cards to make the best possible five-card poker hand.

          The game proceeds through several rounds of betting, with players having the option to check, bet, raise, or fold depending on the strength of their hand and their confidence in winning the pot. The community cards are dealt in stages: three cards, known as the "flop," are dealt first, followed by another single card called the "turn" or "fourth street," and finally, a fifth card called the "river" or "fifth street."

          The objective of Texas Hold'em is to win chips or money by either having the best hand at showdown or by getting all opponents to fold their hands before the showdown. It's a game of skill, strategy, and psychology, where players must carefully manage their resources and make calculated decisions based on their understanding of the game and their opponents.
        </p>
        <NuxtLink href="/lobby">
          <Button class="play" label="Secondary" severity="secondary" @click="connectGameboard"> Play Texas Hold 'Em </Button>
        </NuxtLink>
      </div>

    </div>

    <div class="menu-container">
      <TabMenu v-model:activeIndex="active" :model="items" activeItem="None" class="tab-menu"/>
    </div>

    <div class="rules-container" v-if="active === 1">
      <TexasholdemRules />
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